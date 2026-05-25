using System;
using System.Data.SQLite;

namespace HWManager.Client
{
    /// <summary>
    /// 웹 모니터링 서버의 실행 여부와 포트 번호를 SQLite Settings 테이블에 저장/로드하는 클래스.
    ///
    /// 기존 프로젝트는 별도 설정 파일 대신 system_log.db 안의 Settings 테이블을 사용하고 있으므로,
    /// 웹 모니터링 설정도 같은 저장소를 사용한다.
    /// </summary>
    internal sealed class WebMonitorSettings
    {
        // Settings 테이블에 저장될 Key 이름.
        // 다른 설정(AlertSettings_*)과 충돌하지 않도록 WebMonitor_ 접두사를 사용한다.
        private const string EnabledKey = "WebMonitor_IsEnabled";
        private const string PortKey = "WebMonitor_Port";

        // DatabaseHelper와 동일하게 실행 파일 기준 system_log.db를 사용한다.
        private const string ConnectionString = "Data Source=system_log.db;Version=3;";

        /// <summary>
        /// 앱 시작 시 웹 모니터링 서버를 자동으로 켤지 여부.
        /// 기본값은 true라서 별도 설정이 없어도 http://localhost:5287 로 접속 가능하다.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 웹 모니터링 서버가 사용할 TCP 포트.
        /// 5287은 기존 HWManager.Server의 기본 개발 포트와 겹치지 않도록 별도로 잡은 값이다.
        /// </summary>
        public int Port { get; set; } = 5287;

        /// <summary>
        /// DB에서 웹 모니터링 설정을 읽는다.
        ///
        /// DB가 없거나 Settings 테이블이 아직 없는 경우에도 앱이 정상 실행되어야 하므로,
        /// 실패 시 예외를 밖으로 던지지 않고 기본값을 반환한다.
        /// </summary>
        public static WebMonitorSettings Load()
        {
            var settings = new WebMonitorSettings();

            try
            {
                using var conn = new SQLiteConnection(ConnectionString);
                conn.Open();
                EnsureSettingsTable(conn);

                // 필요한 Key 두 개만 조회한다.
                using var cmd = new SQLiteCommand(
                    "SELECT Key, Value FROM Settings WHERE Key IN (@enabledKey, @portKey)", conn);
                cmd.Parameters.AddWithValue("@enabledKey", EnabledKey);
                cmd.Parameters.AddWithValue("@portKey", PortKey);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string key = Convert.ToString(reader["Key"]) ?? string.Empty;
                    string value = Convert.ToString(reader["Value"]) ?? string.Empty;

                    if (key == EnabledKey && bool.TryParse(value, out bool enabled))
                    {
                        settings.IsEnabled = enabled;
                    }
                    else if (key == PortKey && int.TryParse(value, out int port))
                    {
                        // 1024 미만 포트는 관리자 권한/예약 포트 문제가 생길 수 있어 제한한다.
                        settings.Port = Math.Clamp(port, 1024, 65535);
                    }
                }
            }
            catch
            {
                // 설정 로드 실패 시 기본값 사용.
                // 웹 기능 때문에 기존 WinForms 앱 실행이 실패하면 안 된다.
            }

            return settings;
        }

        /// <summary>
        /// 현재 웹 모니터링 설정을 DB에 저장한다.
        /// 지금은 설정창 UI를 원래 상태로 되돌렸기 때문에 직접 호출되는 곳은 없을 수 있지만,
        /// 추후 설정창에서 웹 모니터링 On/Off를 다시 붙일 때 재사용할 수 있다.
        /// </summary>
        public static void Save(WebMonitorSettings settings)
        {
            try
            {
                using var conn = new SQLiteConnection(ConnectionString);
                conn.Open();
                EnsureSettingsTable(conn);

                SaveValue(conn, EnabledKey, settings.IsEnabled.ToString());
                SaveValue(conn, PortKey, Math.Clamp(settings.Port, 1024, 65535).ToString());
            }
            catch
            {
                // 기존 DatabaseHelper의 저장 메서드들과 동일한 정책:
                // 설정 저장 실패가 앱 종료/크래시로 이어지지 않게 무시한다.
            }
        }

        /// <summary>
        /// Settings 테이블이 없으면 생성한다.
        /// DatabaseHelper.Initialize()가 보통 먼저 만들지만, 안전하게 한 번 더 보장한다.
        /// </summary>
        private static void EnsureSettingsTable(SQLiteConnection conn)
        {
            using var cmd = new SQLiteCommand(
                "CREATE TABLE IF NOT EXISTS Settings (Key TEXT PRIMARY KEY, Value TEXT)", conn);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Settings 테이블에 Key/Value 한 쌍을 저장한다.
        /// INSERT OR REPLACE를 사용해 기존 값이 있으면 덮어쓴다.
        /// </summary>
        private static void SaveValue(SQLiteConnection conn, string key, string value)
        {
            using var cmd = new SQLiteCommand(
                "INSERT OR REPLACE INTO Settings (Key, Value) VALUES (@key, @value)", conn);
            cmd.Parameters.AddWithValue("@key", key);
            cmd.Parameters.AddWithValue("@value", value);
            cmd.ExecuteNonQuery();
        }
    }
}
