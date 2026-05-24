using System;
using System.Data;
using System.Data.SQLite;
using HWManager.Core.Models;

namespace HWManager.Client
{
    public static class DatabaseHelper
    {
        private static string connString = "Data Source=system_log.db;Version=3;";

        // 테이블 초기화: 하드웨어 수치와 프로세스 1~10위 칸을 모두 생성
        public static void Initialize()
        {
            using (var conn = new SQLiteConnection(connString))
            {
                conn.Open();
                
                // Logs 테이블
                string sqlLogs = @"CREATE TABLE IF NOT EXISTS Logs (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                LogTime DATETIME DEFAULT (datetime('now','localtime')),
                                Category TEXT,
                                CPU TEXT,
                                RAM TEXT,
                                GPU TEXT,
                                P1 TEXT, P2 TEXT, P3 TEXT, P4 TEXT, P5 TEXT, 
                                P6 TEXT, P7 TEXT, P8 TEXT, P9 TEXT, P10 TEXT,
                                ProcessInfo TEXT)";
                using (var cmd = new SQLiteCommand(sqlLogs, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                
                // Settings 테이블 (설정값 저장용)
                string sqlSettings = @"CREATE TABLE IF NOT EXISTS Settings (
                                Key TEXT PRIMARY KEY,
                                Value TEXT)";
                using (var cmd = new SQLiteCommand(sqlSettings, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // 하드웨어 로그 저장
        public static void SaveHardwareLog(float cpu, double ram, float gpu)
        {
            try
            {
                using (var conn = new SQLiteConnection(connString))
                {
                    conn.Open();
                    string sql = "INSERT INTO Logs (Category, CPU, RAM, GPU) VALUES ('Hardware', @cpu, @ram, @gpu)";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@cpu", cpu.ToString("F1") + "%");
                        cmd.Parameters.AddWithValue("@ram", ram.ToString("F1") + "%");
                        cmd.Parameters.AddWithValue("@gpu", gpu.ToString("F1") + "%");
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        // 프로세스 상위 10개를 한 줄(Tuple)에 저장
        public static void SaveTop10ProcessRow(string[] procs)
        {
            try
            {
                using (var conn = new SQLiteConnection(connString))
                {
                    conn.Open();
                    string sql = @"INSERT INTO Logs (Category, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10) 
                                   VALUES ('TopProcess', @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            string val = (procs.Length > i) ? procs[i] : "";
                            cmd.Parameters.AddWithValue($"@p{i + 1}", val);
                        }
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        // 프로세스 종료 로그 저장
        public static void SaveKillLog(string info)
        {
            try
            {
                using (var conn = new SQLiteConnection(connString))
                {
                    conn.Open();
                    string sql = "INSERT INTO Logs (Category, ProcessInfo) VALUES ('Kill', @info)";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@info", info);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        // 알림 로그 저장
        public static void SaveAlertLog(string resourceType, float usage, string details)
        {
            try
            {
                using (var conn = new SQLiteConnection(connString))
                {
                    conn.Open();
                    string sql = "INSERT INTO Logs (Category, CPU, ProcessInfo) VALUES ('Alert', @resource, @details)";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@resource", $"{resourceType} {usage:F1}%");
                        cmd.Parameters.AddWithValue("@details", details);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        // 데이터 조회 로직
        public static DataTable GetLogs(string category)
        {
            DataTable dt = new DataTable();
            using (var conn = new SQLiteConnection(connString))
            {
                conn.Open();
                string sql = "";

                if (category == "Hardware")
                {
                    sql = "SELECT strftime('%Y-%m-%d %H:%M:%S', LogTime) as 시간, CPU, RAM, GPU FROM Logs WHERE Category = 'Hardware' ORDER BY Id DESC LIMIT 100";
                }
                else if (category == "TopProcess")
                {
                    // 한 행에 시간 + 1~10위가 가로로 출력됨
                    sql = @"SELECT strftime('%Y-%m-%d %H:%M:%S', LogTime) as 시간, 
                            P1 as '1등', P2 as '2등', P3 as '3등', P4 as '4등', P5 as '5등', 
                            P6 as '6등', P7 as '7등', P8 as '8등', P9 as '9등', P10 as '10등' 
                            FROM Logs WHERE Category = 'TopProcess' ORDER BY Id DESC LIMIT 100";
                }
                else if (category == "Alert")
                {
                    sql = @"SELECT strftime('%Y-%m-%d %H:%M:%S', LogTime) as 시간, 
                            CPU as 리소스, ProcessInfo as 상세내용 
                            FROM Logs WHERE Category = 'Alert' ORDER BY Id DESC LIMIT 100";
                }
                else
                {
                    sql = "SELECT strftime('%Y-%m-%d %H:%M:%S', LogTime) as 시간, ProcessInfo as 상세내용 FROM Logs WHERE Category = 'Kill' ORDER BY Id DESC LIMIT 100";
                }

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    using (var adapter = new SQLiteDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// 설정값 저장
        /// </summary>
        public static void SaveAlertSettings(AlertSettings settings)
        {
            try
            {
                using (var conn = new SQLiteConnection(connString))
                {
                    conn.Open();
                    
                    // 기존 설정 삭제 후 저장
                    string sqlDelete = "DELETE FROM Settings WHERE Key LIKE 'AlertSettings_%'";
                    using (var cmd = new SQLiteCommand(sqlDelete, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    string sqlInsert = @"INSERT INTO Settings (Key, Value) VALUES (@key, @value)";
                    
                    var settingsData = new[]
                    {
                        ("AlertSettings_CpuThreshold", settings.CpuThreshold.ToString()),
                        ("AlertSettings_RamThreshold", settings.RamThreshold.ToString()),
                        ("AlertSettings_GpuThreshold", settings.GpuThreshold.ToString()),
                        ("AlertSettings_AlertInterval", settings.AlertInterval.ToString()),
                        ("AlertSettings_IsEnabled", settings.IsEnabled.ToString())
                    };

                    foreach (var (key, value) in settingsData)
                    {
                        using (var cmd = new SQLiteCommand(sqlInsert, conn))
                        {
                            cmd.Parameters.AddWithValue("@key", key);
                            cmd.Parameters.AddWithValue("@value", value);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 저장된 설정값 로드
        /// </summary>
        public static AlertSettings LoadAlertSettings()
        {
            var settings = new AlertSettings();

            try
            {
                using (var conn = new SQLiteConnection(connString))
                {
                    conn.Open();
                    string sql = "SELECT Key, Value FROM Settings WHERE Key LIKE 'AlertSettings_%'";
                    
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string key = reader["Key"].ToString();
                                string value = reader["Value"].ToString();

                                switch (key)
                                {
                                    case "AlertSettings_CpuThreshold":
                                        if (float.TryParse(value, out float cpuThreshold))
                                            settings.CpuThreshold = cpuThreshold;
                                        break;
                                    case "AlertSettings_RamThreshold":
                                        if (float.TryParse(value, out float ramThreshold))
                                            settings.RamThreshold = ramThreshold;
                                        break;
                                    case "AlertSettings_GpuThreshold":
                                        if (float.TryParse(value, out float gpuThreshold))
                                            settings.GpuThreshold = gpuThreshold;
                                        break;
                                    case "AlertSettings_AlertInterval":
                                        if (int.TryParse(value, out int alertInterval))
                                            settings.AlertInterval = alertInterval;
                                        break;
                                    case "AlertSettings_IsEnabled":
                                        if (bool.TryParse(value, out bool isEnabled))
                                            settings.IsEnabled = isEnabled;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch { }

            return settings;
        }

        // 오버레이 설정값 저장
        public static void SaveOverlaySettings(bool enabled)
        {
            try
            {
                using (var conn = new SQLiteConnection(connString))
                {
                    conn.Open();
                    // 기존 값이 있으면 덮어쓰고 없으면 삽입
                    string sql = "INSERT OR REPLACE INTO Settings (Key, Value) VALUES ('OverlaySettings_IsEnabled', @value)";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@value", enabled.ToString());
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        // 저장된 오버레이 설정값 로드
        public static bool LoadOverlaySettings()
        {
            try
            {
                using (var conn = new SQLiteConnection(connString))
                {
                    conn.Open();
                    string sql = "SELECT Value FROM Settings WHERE Key = 'OverlaySettings_IsEnabled'";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        var result = cmd.ExecuteScalar();
                        if (result != null && bool.TryParse(result.ToString(), out bool isEnabled))
                        {
                            return isEnabled;
                        }
                    }
                }
            }
            catch { }

            return false; // 데이터가 없거나 에러 시 기본값은 꺼짐
        }

        // 투명도와 크기만 저장
        public static void SaveOverlayVisuals(double opacity, double scale)
        {
            try
            {
                using (var conn = new SQLiteConnection(connString))
                {
                    conn.Open();
                    using (var trans = conn.BeginTransaction())
                    {
                        string sql = "INSERT OR REPLACE INTO Settings (Key, Value) VALUES (@key, @value)";
                        using (var cmd = new SQLiteCommand(sql, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@key", "OverlaySettings_Opacity");
                            cmd.Parameters.AddWithValue("@value", opacity.ToString());
                            cmd.ExecuteNonQuery();

                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@key", "OverlaySettings_Scale");
                            cmd.Parameters.AddWithValue("@value", scale.ToString());
                            cmd.ExecuteNonQuery();
                        }
                        trans.Commit();
                    }
                }
            }
            catch { }
        }

        // 투명도와 크기만 로드
        public static void LoadOverlayVisuals(out double opacity, out double scale)
        {
            opacity = 0.8;
            scale = 1.0;

            try
            {
                using (var conn = new SQLiteConnection(connString))
                {
                    conn.Open();
                    string sql = "SELECT Key, Value FROM Settings WHERE Key IN ('OverlaySettings_Opacity', 'OverlaySettings_Scale')";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string key = reader["Key"].ToString();
                            string val = reader["Value"].ToString();

                            if (key == "OverlaySettings_Opacity" && double.TryParse(val, out double op)) opacity = op;
                            if (key == "OverlaySettings_Scale" && double.TryParse(val, out double sc)) scale = sc;
                        }
                    }
                }
            }
            catch { }
        }
    }
}