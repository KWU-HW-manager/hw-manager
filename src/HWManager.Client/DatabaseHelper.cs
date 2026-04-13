using System;
using System.Data;
using System.Data.SQLite;

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
                string sql = @"CREATE TABLE IF NOT EXISTS Logs (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                LogTime DATETIME DEFAULT (datetime('now','localtime')),
                                Category TEXT,
                                CPU TEXT,
                                RAM TEXT,
                                GPU TEXT,
                                P1 TEXT, P2 TEXT, P3 TEXT, P4 TEXT, P5 TEXT, 
                                P6 TEXT, P7 TEXT, P8 TEXT, P9 TEXT, P10 TEXT,
                                ProcessInfo TEXT)";
                using (var cmd = new SQLiteCommand(sql, conn))
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
    }
}