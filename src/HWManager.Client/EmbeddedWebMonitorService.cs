using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HWManager.Core.Models;
using HWManager.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HWManager.Client
{
    /// <summary>
    /// WinForms 클라이언트 프로세스 안에서 함께 실행되는 내장 웹 모니터링 서버.
    ///
    /// 별도 HWManager.Server 프로젝트를 실행하지 않아도, 데스크톱 앱이 켜져 있는 동안
    /// http://localhost:포트 로 접속해 CPU/RAM/GPU/프로세스/통계 정보를 볼 수 있게 한다.
    ///
    /// 주요 책임:
    /// 1. ASP.NET Core minimal API 서버(Kestrel)를 앱 내부에서 시작/중지
    /// 2. 1초마다 하드웨어 스냅샷을 수집해 최근 히스토리를 메모리에 보관
    /// 3. SQLite(system_log.db)에 저장된 로그를 읽어 일간/주간 통계와 알림 발생 시각 제공
    /// 4. 브라우저에서 볼 수 있는 대시보드 HTML을 반환
    /// </summary>
    internal sealed class EmbeddedWebMonitorService
    {
        // 실시간 그래프용 메모리 히스토리 보관 개수.
        // 1초마다 수집하므로 180개 = 최근 약 3분치 데이터.
        private const int HistoryCapacity = 180;

        // _history는 백그라운드 수집 Task와 HTTP 요청 처리 스레드에서 동시에 접근하므로 lock이 필요하다.
        private readonly object _syncRoot = new object();
        private readonly Queue<SystemSnapshot> _history = new Queue<SystemSnapshot>();

        // 기존 Core 서비스 재사용: 하드웨어 센서 수집과 프로세스 목록 조회를 담당한다.
        private readonly HardwareMonitorService _hardwareMonitor = new HardwareMonitorService();
        private const string LogConnectionString = "Data Source=system_log.db;Version=3;";
        private readonly ProcessService _processService = new ProcessService();

        // 실행 중인 Kestrel 웹앱 인스턴스. null이면 웹 서버가 꺼진 상태다.
        private WebApplication? _app;

        // 1초 주기 수집 루프를 종료하기 위한 토큰.
        private CancellationTokenSource? _collectorCts;
        private int _port;

        // 앱 전체에서 웹 서버는 하나만 있으면 되므로 싱글턴으로 둔다.
        // 여러 폼에서 접근해도 같은 서버 인스턴스를 제어한다.
        public static EmbeddedWebMonitorService Instance { get; } = new EmbeddedWebMonitorService();

        public bool IsRunning => _app != null;
        public int Port => _port;
        public string Url => _port > 0 ? $"http://localhost:{_port}" : string.Empty;

        private EmbeddedWebMonitorService()
        {
        }

        /// <summary>
        /// 내장 웹 서버를 시작한다.
        ///
        /// - 이미 같은 포트로 실행 중이면 아무 작업도 하지 않는다.
        /// - 다른 포트로 실행 중이면 기존 서버를 중지한 뒤 새 포트로 다시 시작한다.
        /// - 0.0.0.0에 바인딩해서 같은 네트워크의 다른 기기에서도 접속 가능하게 한다.
        ///   단, 외부 접속에는 Windows 방화벽/공유기 포트포워딩 설정이 별도로 필요하다.
        /// </summary>
        public async Task StartAsync(int port)
        {
            port = Math.Clamp(port, 1024, 65535);

            if (_app != null && _port == port)
                return;

            if (_app != null)
                await StopAsync().ConfigureAwait(false);

            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                Args = Array.Empty<string>(),
                ContentRootPath = AppContext.BaseDirectory
            });

            builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            var app = builder.Build();
            app.UseCors();
            MapRoutes(app);

            await app.StartAsync().ConfigureAwait(false);

            _app = app;
            _port = port;
            StartCollector();
        }

        /// <summary>
        /// 내장 웹 서버와 백그라운드 수집 루프를 중지한다.
        ///
        /// WinForms 종료 과정에서 이 메서드를 UI 스레드에서 오래 기다리면 화면이 닫히지 않는 문제가 생길 수 있다.
        /// 그래서 MainForm.OnFormClosing에서는 Task.Run으로 백그라운드에서 호출한다.
        /// </summary>
        public async Task StopAsync()
        {
            _collectorCts?.Cancel();
            _collectorCts?.Dispose();
            _collectorCts = null;

            if (_app != null)
            {
                await _app.StopAsync(TimeSpan.FromSeconds(2)).ConfigureAwait(false);
                await _app.DisposeAsync().ConfigureAwait(false);
                _app = null;
            }

            _port = 0;
        }

        /// <summary>
        /// CPU/RAM/GPU 스냅샷을 1초마다 수집하는 백그라운드 루프를 시작한다.
        ///
        /// 웹 요청이 들어올 때마다 센서를 직접 읽으면 응답이 느려질 수 있으므로,
        /// 별도 루프가 미리 값을 모아두고 /api/history가 그 값을 반환하는 구조다.
        /// </summary>
        private void StartCollector()
        {
            _collectorCts = new CancellationTokenSource();
            CancellationToken token = _collectorCts.Token;

            _ = Task.Run(async () =>
            {
                using var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));

                while (!token.IsCancellationRequested)
                {
                    AddSnapshot();

                    try
                    {
                        await timer.WaitForNextTickAsync(token);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }
            }, token);
        }

        /// <summary>
        /// 현재 하드웨어 상태를 한 번 읽어서 히스토리 큐에 추가한다.
        /// 큐가 너무 커지지 않게 HistoryCapacity를 초과하면 가장 오래된 값을 제거한다.
        /// </summary>
        private void AddSnapshot()
        {
            try
            {
                var snapshot = _hardwareMonitor.GetCurrentStatus();
                lock (_syncRoot)
                {
                    _history.Enqueue(snapshot);
                    while (_history.Count > HistoryCapacity)
                    {
                        _history.Dequeue();
                    }
                }
            }
            catch
            {
                // 웹 모니터링 수집 실패는 클라이언트 실행에 영향을 주지 않게 무시
            }
        }

        /// <summary>
        /// 가장 최근 수집된 스냅샷을 반환한다.
        /// 아직 수집 루프가 값을 넣기 전이면 즉시 한 번 읽어서 반환한다.
        /// </summary>
        private SystemSnapshot GetLatestSnapshot()
        {
            lock (_syncRoot)
            {
                if (_history.Count > 0)
                    return _history.Last();
            }

            var snapshot = _hardwareMonitor.GetCurrentStatus();
            lock (_syncRoot)
            {
                _history.Enqueue(snapshot);
            }
            return snapshot;
        }

        /// <summary>
        /// 실시간 그래프에 사용할 최근 스냅샷 배열을 반환한다.
        /// Queue 자체를 노출하지 않고 복사본을 반환해 스레드 안전성을 지킨다.
        /// </summary>
        private IReadOnlyList<SystemSnapshot> GetHistory()
        {
            lock (_syncRoot)
            {
                return _history.ToArray();
            }
        }

        /// <summary>
        /// 브라우저/대시보드에서 호출할 HTTP 라우트를 등록한다.
        ///
        /// /                 : 대시보드 HTML
        /// /api/metrics      : 현재 CPU/RAM/GPU, 프로세스 수, 메모리 상위 프로세스
        /// /api/history      : 최근 3분 실시간 그래프 데이터
        /// /api/alerts       : 최근 알림 로그
        /// /api/statistics   : 일간/주간 평균 사용량 + 알림 발생 시각
        /// /api/processes    : 프로세스 목록
        /// </summary>
        private void MapRoutes(WebApplication app)
        {
            app.MapGet("/", async context =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync(DashboardHtml);
            });

            app.MapGet("/health", () => Results.Ok(new { status = "healthy", measuredAt = DateTime.Now }));

            app.MapGet("/api/metrics", () =>
            {
                var latest = GetLatestSnapshot();

                return Results.Ok(new
                {
                    status = "ok",
                    machineName = Environment.MachineName,
                    os = Environment.OSVersion.VersionString,
                    uptimeSeconds = Environment.TickCount64 / 1000,
                    processCount = SafeProcessCount(),
                    latest = ToDto(latest),
                    topProcesses = _processService.GetProcesses()
                        .Take(10)
                        .Select(p => new
                        {
                            id = p.Id,
                            name = p.Name,
                            memoryUsageMB = Math.Round(p.MemoryUsageMB, 1)
                        })
                });
            });

            app.MapGet("/api/history", () => Results.Ok(GetHistory().Select(ToDto)));

            app.MapGet("/api/alerts", (int? top) =>
            {
                int take = Math.Clamp(top ?? 10, 1, 100);
                return Results.Ok(GetRecentAlerts(take));
            });

            app.MapGet("/api/statistics", () => Results.Ok(GetUsageStatistics()));

            app.MapGet("/api/processes", (int? top) =>
            {
                int take = Math.Clamp(top ?? 30, 1, 200);
                return Results.Ok(_processService.GetProcesses()
                    .Take(take)
                    .Select(p => new
                    {
                        id = p.Id,
                        name = p.Name,
                        memoryUsageMB = Math.Round(p.MemoryUsageMB, 1)
                    }));
            });
        }

        /// <summary>
        /// SQLite Logs 테이블에서 최근 사용량 알림을 조회한다.
        ///
        /// Alert 로그는 DatabaseHelper.SaveAlertLog()가 Category='Alert'로 저장한다.
        /// 현재 스키마에서는 CPU 컬럼에 "CPU 90.0%" 같은 리소스 문자열이 들어가고,
        /// ProcessInfo 컬럼에 알림 상세 내용이 들어간다.
        /// </summary>
        private static IReadOnlyList<object> GetRecentAlerts(int take)
        {
            var items = new List<object>();

            try
            {
                using var conn = new SQLiteConnection(LogConnectionString);
                conn.Open();

                using var cmd = new SQLiteCommand(@"
                    SELECT strftime('%Y-%m-%d %H:%M:%S', LogTime) AS TimeText,
                           COALESCE(CPU, '') AS ResourceText,
                           COALESCE(ProcessInfo, '') AS DetailsText
                    FROM Logs
                    WHERE Category = 'Alert'
                    ORDER BY Id DESC
                    LIMIT @take", conn);
                cmd.Parameters.AddWithValue("@take", take);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    items.Add(new
                    {
                        time = Convert.ToString(reader["TimeText"]) ?? string.Empty,
                        resource = Convert.ToString(reader["ResourceText"]) ?? string.Empty,
                        details = Convert.ToString(reader["DetailsText"]) ?? string.Empty
                    });
                }
            }
            catch
            {
                // 로그 DB가 아직 없거나 읽기 실패 시 빈 목록 반환
            }

            return items;
        }

        /// <summary>
        /// 대시보드 통계 영역에 필요한 데이터를 한 번에 반환한다.
        ///
        /// daily  : 오늘 하루 데이터를 시간대별로 묶어 평균 CPU/RAM/GPU 사용률을 계산
        /// weekly : 최근 7일 데이터를 날짜별로 묶어 평균 CPU/RAM/GPU 사용률을 계산
        /// alerts : 최근 알림 발생 시각과 상세 내용
        ///
        /// 주의: Logs 테이블의 CPU/RAM/GPU 값은 "12.3%" 문자열로 저장되어 있으므로,
        /// SQL에서 REPLACE(CPU, '%', '') 후 REAL로 변환해서 평균을 낸다.
        /// </summary>
        private static object GetUsageStatistics()
        {
            return new
            {
                daily = QueryUsageSeries(@"
                    SELECT strftime('%H:00', LogTime) AS Label,
                           AVG(CAST(REPLACE(CPU, '%', '') AS REAL)) AS CpuAvg,
                           AVG(CAST(REPLACE(RAM, '%', '') AS REAL)) AS RamAvg,
                           AVG(CAST(REPLACE(GPU, '%', '') AS REAL)) AS GpuAvg
                    FROM Logs
                    WHERE Category = 'Hardware'
                      AND date(LogTime) = date('now', 'localtime')
                    GROUP BY strftime('%H', LogTime)
                    ORDER BY strftime('%H', LogTime)"),
                weekly = QueryUsageSeries(@"
                    SELECT strftime('%m/%d', LogTime) AS Label,
                           AVG(CAST(REPLACE(CPU, '%', '') AS REAL)) AS CpuAvg,
                           AVG(CAST(REPLACE(RAM, '%', '') AS REAL)) AS RamAvg,
                           AVG(CAST(REPLACE(GPU, '%', '') AS REAL)) AS GpuAvg
                    FROM Logs
                    WHERE Category = 'Hardware'
                      AND date(LogTime) >= date('now', 'localtime', '-6 days')
                    GROUP BY date(LogTime)
                    ORDER BY date(LogTime)"),
                alerts = GetRecentAlerts(20)
            };
        }

        /// <summary>
        /// 일간/주간 사용량 SQL을 실행하고 차트가 바로 사용할 수 있는 형태로 변환한다.
        ///
        /// 반환 필드:
        /// - label    : X축 라벨(예: 13:00, 05/24)
        /// - cpuUsage : 평균 CPU 사용률
        /// - ramUsage : 평균 RAM 사용률
        /// - gpuUsage : 평균 GPU 사용률
        /// </summary>
        private static IReadOnlyList<object> QueryUsageSeries(string sql)
        {
            var items = new List<object>();

            try
            {
                using var conn = new SQLiteConnection(LogConnectionString);
                conn.Open();

                using var cmd = new SQLiteCommand(sql, conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    items.Add(new
                    {
                        label = Convert.ToString(reader["Label"]) ?? string.Empty,
                        cpuUsage = Math.Round(ToDouble(reader["CpuAvg"]), 1),
                        ramUsage = Math.Round(ToDouble(reader["RamAvg"]), 1),
                        gpuUsage = Math.Round(ToDouble(reader["GpuAvg"]), 1)
                    });
                }
            }
            catch
            {
                // 로그 DB가 아직 없거나 읽기 실패 시 빈 목록 반환
            }

            return items;
        }

        /// <summary>
        /// SQLite에서 읽은 숫자 값을 double로 안전하게 변환한다.
        /// AVG 결과가 NULL(DBNull)일 수 있으므로 이 경우 0으로 처리한다.
        /// </summary>
        private static double ToDouble(object value)
        {
            if (value == DBNull.Value)
                return 0;

            return double.TryParse(Convert.ToString(value), out double result) ? result : 0;
        }

        /// <summary>
        /// Core의 SystemSnapshot 모델을 웹 API 응답용 익명 객체로 변환한다.
        /// 소수점 한 자리로 반올림해서 브라우저 표시값과 API 값을 일관되게 맞춘다.
        /// </summary>
        private static object ToDto(SystemSnapshot snapshot) => new
        {
            measuredAt = snapshot.MeasuredAt,
            cpuUsage = Math.Round(snapshot.CpuUsage, 1),
            ramUsage = Math.Round(snapshot.RamUsage, 1),
            gpuUsage = Math.Round(snapshot.GpuUsage, 1)
        };

        /// <summary>
        /// 현재 실행 중인 프로세스 개수를 반환한다.
        /// 일부 프로세스 접근 실패가 전체 API 실패로 이어지지 않게 예외를 0으로 처리한다.
        /// </summary>
        private static int SafeProcessCount()
        {
            try { return Process.GetProcesses().Length; }
            catch { return 0; }
        }

        /// <summary>
        /// 브라우저에 내려줄 단일 파일 대시보드 HTML.
        ///
        /// 별도 프론트엔드 빌드 도구 없이 배포하기 위해 HTML/CSS/JavaScript를 문자열 하나에 포함했다.
        /// JavaScript는 아래 API들을 주기적으로 호출해 화면을 갱신한다.
        /// - /api/metrics    : 상단 실시간 카드와 프로세스 목록
        /// - /api/history    : 실시간 사용량 그래프
        /// - /api/statistics : 일간/주간 통계 그래프와 알림 발생 시각
        /// </summary>
        private const string DashboardHtml = """
<!doctype html>
<html lang="ko">
<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <title>HW Manager 웹 모니터링</title>
  <style>
    *{box-sizing:border-box}body{margin:0;background:#f8fafc;color:#111827;font-family:-apple-system,BlinkMacSystemFont,'Segoe UI','맑은 고딕',sans-serif}.page{width:min(1120px,calc(100vw - 32px));margin:0 auto;padding:28px 0 40px}header{display:flex;justify-content:space-between;gap:16px;align-items:center;margin-bottom:18px}h1{margin:0;font-size:26px;letter-spacing:-.04em}.sub{margin-top:6px;color:#64748b;font-size:13px}.pill{display:inline-flex;align-items:center;gap:8px;padding:8px 12px;border:1px solid #e2e8f0;border-radius:999px;background:white;color:#64748b;font-size:13px}.dot{width:10px;height:10px;border-radius:50%;background:#16a34a;box-shadow:0 0 0 3px #dcfce7}.cards{display:grid;grid-template-columns:repeat(4,minmax(0,1fr));gap:12px;margin-bottom:14px}.card{background:white;border:1px solid #e2e8f0;border-radius:14px;box-shadow:0 1px 4px rgba(0,0,0,.05);overflow:hidden}.metric{padding:18px 20px}.metric.dark{background:#111827;border-color:#111827}.label{color:#64748b;font-size:12px;font-weight:700;margin-bottom:8px}.value{font-size:28px;line-height:1;font-weight:800;letter-spacing:-.05em}.hint{color:#94a3b8;font-size:12px;margin-top:7px}.dark .label{color:#9ca3af}.dark .value{color:white}.dark .hint{color:#6b7280}.grid{display:grid;grid-template-columns:1.1fr .9fr;gap:14px}.head{display:flex;justify-content:space-between;align-items:center;padding:16px 20px;border-bottom:1px solid #f1f5f9}.title{font-size:15px;font-weight:800}.meta{color:#94a3b8;font-size:12px}.body{padding:18px 20px 20px}.row{margin-bottom:16px}.rowtop{display:flex;justify-content:space-between;align-items:baseline;margin-bottom:6px;color:#64748b;font-size:13px;font-weight:700}.rowtop strong{color:#111827}.bar{height:8px;background:#f1f5f9;border-radius:999px;overflow:hidden}.bar div{height:100%;width:0;background:#22c55e;border-radius:999px;transition:width .7s,background .7s}canvas{width:100%;height:250px;display:block;border:1px solid #f1f5f9;border-radius:10px}.legend{display:flex;gap:12px;margin-top:12px;color:#64748b;font-size:12px}.legend span:before{content:'';display:inline-block;width:9px;height:9px;border-radius:50%;margin-right:6px;background:var(--c)}.plist{display:grid;gap:8px}.proc{display:grid;grid-template-columns:1fr auto;gap:12px;align-items:center;padding:10px 12px;background:#f8fafc;border:1px solid #f1f5f9;border-radius:9px}.pname{font-size:13px;font-weight:800;overflow:hidden;text-overflow:ellipsis;white-space:nowrap}.pid{color:#94a3b8;font-size:11px;margin-top:2px}.mem{font-size:12px;font-weight:800}.err{display:none;margin-bottom:14px;padding:12px 14px;border:1px solid #fecaca;border-radius:10px;background:#fef2f2;color:#dc2626;font-size:13px}.stats{margin-top:14px}.alert-list{display:grid;gap:8px}.alert-item{padding:10px 12px;background:#fff7ed;border:1px solid #fed7aa;border-radius:9px}.alert-time{font-size:11px;color:#9a3412;font-weight:800;margin-bottom:3px}.alert-resource{font-size:13px;color:#dc2626;font-weight:900}.alert-detail{font-size:12px;color:#64748b;margin-top:3px;line-height:1.45}.empty{padding:18px;text-align:center;color:#94a3b8;font-size:13px;background:#f8fafc;border:1px solid #f1f5f9;border-radius:9px}@media(max-width:900px){header{align-items:flex-start;flex-direction:column}.cards{grid-template-columns:repeat(2,minmax(0,1fr))}.grid{grid-template-columns:1fr}}@media(max-width:520px){.page{width:min(100vw - 20px,1120px);padding-top:18px}.cards{grid-template-columns:1fr}h1{font-size:22px}}
  </style>
</head>
<body>
<main class="page">
  <header><div><h1>HW Manager 웹 모니터링</h1><div class="sub" id="machine">불러오는 중...</div></div><div class="pill"><span class="dot"></span><span id="status">온라인</span></div></header>
  <div class="err" id="err"></div>
  <section class="cards">
    <div class="card metric dark"><div class="label">CPU</div><div class="value" id="cpuV">--%</div><div class="hint">실시간 사용률</div></div>
    <div class="card metric"><div class="label">RAM</div><div class="value" id="ramV">--%</div><div class="hint">메모리 사용률</div></div>
    <div class="card metric"><div class="label">GPU</div><div class="value" id="gpuV">--%</div><div class="hint">그래픽 사용률</div></div>
    <div class="card metric"><div class="label">프로세스</div><div class="value" id="procV">--</div><div class="hint" id="uptime">업타임 --</div></div>
  </section>
  <section class="grid">
    <div class="card"><div class="head"><div class="title">시스템 사용량</div><div class="meta" id="updated">--</div></div><div class="body">
      <div class="row"><div class="rowtop"><span>CPU</span><strong id="cpuM">--%</strong></div><div class="bar"><div id="cpuB"></div></div></div>
      <div class="row"><div class="rowtop"><span>RAM</span><strong id="ramM">--%</strong></div><div class="bar"><div id="ramB"></div></div></div>
      <div class="row"><div class="rowtop"><span>GPU</span><strong id="gpuM">--%</strong></div><div class="bar"><div id="gpuB"></div></div></div>
      <canvas id="chart" width="850" height="250"></canvas><div class="legend"><span style="--c:#2563eb">CPU</span><span style="--c:#22c55e">RAM</span><span style="--c:#a855f7">GPU</span></div>
    </div></div>
    <div class="card"><div class="head"><div class="title">메모리 상위 프로세스</div><div class="meta">Top 10</div></div><div class="body"><div class="plist" id="plist"></div></div></div>
  </section>
  <section class="grid stats">
    <div class="card"><div class="head"><div class="title">일간 사용량</div><div class="meta">오늘 시간대별 평균</div></div><div class="body"><canvas id="dailyChart" width="850" height="230"></canvas><div class="legend"><span style="--c:#2563eb">CPU</span><span style="--c:#22c55e">RAM</span><span style="--c:#a855f7">GPU</span></div></div></div>
    <div class="card"><div class="head"><div class="title">주간 사용량</div><div class="meta">최근 7일 평균</div></div><div class="body"><canvas id="weeklyChart" width="850" height="230"></canvas><div class="legend"><span style="--c:#2563eb">CPU</span><span style="--c:#22c55e">RAM</span><span style="--c:#a855f7">GPU</span></div></div></div>
  </section>
  <section class="card stats">
    <div class="head"><div class="title">사용량 알림 발생 시각</div><div class="meta">최근 20건</div></div>
    <div class="body"><div class="alert-list" id="alertList"></div></div>
  </section>
</main>
<script>
// DOM 선택 축약 함수와 화면 표시용 포맷 함수들.
// 서버 API가 숫자를 내려주면 여기서 %, MB/GB, 업타임 문자열로 변환한다.
const $=id=>document.getElementById(id);let hist=[];const pct=v=>(Number(v||0)).toFixed(1)+'%';
const mb=v=>Number(v||0)>=1024?(Number(v)/1024).toFixed(1)+' GB':Number(v||0).toFixed(1)+' MB';
const up=s=>{s=Number(s||0);const d=Math.floor(s/86400),h=Math.floor(s%86400/3600),m=Math.floor(s%3600/60);return d?`업타임 ${d}일 ${h}h`:`업타임 ${h}h ${m}m`};
const esc=v=>String(v??'').replace(/[&<>'"]/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;',"'":'&#39;','"':'&quot;'}[c]));
function color(v){v=Number(v||0);return v>=90?'#ef4444':v>=70?'#f59e0b':'#22c55e'}
function bar(id,v){const e=$(id);e.style.width=Math.max(0,Math.min(Number(v||0),100))+'%';e.style.background=color(v)}
// Canvas에 CPU/RAM/GPU 3개 선을 그리는 공통 차트 함수.
// 실시간 그래프, 일간 그래프, 주간 그래프가 모두 같은 함수를 사용한다.
function drawLineChart(id,data){const c=$(id),x=c.getContext('2d'),w=c.width,h=c.height,p={l:40,r:12,t:14,b:30},pw=w-p.l-p.r,ph=h-p.t-p.b;x.clearRect(0,0,w,h);x.fillStyle='#fff';x.fillRect(0,0,w,h);x.strokeStyle='#f1f5f9';x.fillStyle='#94a3b8';x.font='11px Segoe UI';for(let y=0;y<=100;y+=25){const py=p.t+ph-(y/100)*ph;x.beginPath();x.moveTo(p.l,py);x.lineTo(w-p.r,py);x.stroke();x.fillText(y+'%',5,py+4)}if(!data||data.length<1)return;for(const [k,col] of [['cpuUsage','#2563eb'],['ramUsage','#22c55e'],['gpuUsage','#a855f7']]){x.beginPath();x.strokeStyle=col;x.lineWidth=2;data.forEach((q,i)=>{const px=p.l+(i/Math.max(data.length-1,1))*pw,py=p.t+ph-(Math.max(0,Math.min(Number(q[k]||0),100))/100)*ph;i?x.lineTo(px,py):x.moveTo(px,py)});x.stroke()}x.fillStyle='#94a3b8';data.forEach((q,i)=>{if(data.length>10&&i%Math.ceil(data.length/8)!==0)return;const px=p.l+(i/Math.max(data.length-1,1))*pw;x.fillText(q.label||'',px-14,h-8)})}
// 최근 알림 로그를 카드 목록으로 렌더링한다.
// 알림이 없으면 빈 상태 메시지를 보여준다.
function renderAlerts(items){$('alertList').innerHTML=(items&&items.length?items.map(a=>`<div class="alert-item"><div class="alert-time">${esc(a.time)}</div><div class="alert-resource">${esc(a.resource||'사용량 알림')}</div><div class="alert-detail">${esc(a.details||'상세 내용 없음')}</div></div>`).join(''):'<div class="empty">최근 사용량 알림이 없습니다.</div>')}
// 5초마다 실행되는 화면 갱신 루프.
// 서로 독립적인 API 3개를 Promise.all로 동시에 호출해 대시보드 전체를 갱신한다.
async function refresh(){try{const [m,h,s]=await Promise.all([fetch('/api/metrics',{cache:'no-store'}).then(r=>r.json()),fetch('/api/history',{cache:'no-store'}).then(r=>r.json()),fetch('/api/statistics',{cache:'no-store'}).then(r=>r.json())]);const l=m.latest||{};$('machine').textContent=(m.machineName||'Unknown')+' · '+(m.os||'');$('cpuV').textContent=pct(l.cpuUsage);$('ramV').textContent=pct(l.ramUsage);$('gpuV').textContent=pct(l.gpuUsage);$('procV').textContent=m.processCount??'--';$('uptime').textContent=up(m.uptimeSeconds);$('cpuM').textContent=pct(l.cpuUsage);$('ramM').textContent=pct(l.ramUsage);$('gpuM').textContent=pct(l.gpuUsage);$('updated').textContent=l.measuredAt?new Date(l.measuredAt).toLocaleTimeString('ko-KR'):'--';bar('cpuB',l.cpuUsage);bar('ramB',l.ramUsage);bar('gpuB',l.gpuUsage);$('plist').innerHTML=(m.topProcesses||[]).map(p=>`<div class="proc"><div><div class="pname" title="${esc(p.name)}">${esc(p.name)}</div><div class="pid">PID ${p.id}</div></div><div class="mem">${mb(p.memoryUsageMB)}</div></div>`).join('');hist=h;drawLineChart('chart',hist);drawLineChart('dailyChart',s.daily||[]);drawLineChart('weeklyChart',s.weekly||[]);renderAlerts(s.alerts||[]);$('err').style.display='none';$('status').textContent='온라인'}catch(e){$('err').style.display='block';$('err').textContent=e.message||'연결 실패';$('status').textContent='오류'}}refresh();setInterval(refresh,5000);
</script>
</body>
</html>
""";
    }
}
