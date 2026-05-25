using HWManager.Core.Services;
using HWManager.Core.Models;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HWManager.Client
{
    public partial class MainForm : Form
    {
        private HardwareMonitorService _monitorService = new HardwareMonitorService();
        private AlertService _alertService;
        private System.Windows.Forms.Timer? dbLogTimer;
        private Dictionary<string, DateTime> _lastAlertTime = new Dictionary<string, DateTime>();

        // 오버레이 서비스 및 타이머 변수 추가
        private readonly IOverlayService _overlayService = new OverlayService(); //WPF 오버레이 제어용 중계 서비스
        private System.Timers.Timer? _overlayTimer; // 오버레이 데이터 실시간 갱신용 백그라운드 타ㅣ엄
        public bool IsOverlayEnabled = false; // 오버레이 활성화 상태 (실시간 토글 제어용)
        public double OverlayOpacity = 0.8; // 오버레이 반투명도 수치 (0.0 ~ 1.0)
        public double OverlayScale = 1.0; // // 오버레이 UI 전체 크기 배율 (1.0 = 100%)

        public MainForm()
        {
            InitializeComponent();
            ApplyModernStyle();
            InitAlertService();
            InitDbLogTimer();

            // 앱이 켜질 때 내장 웹 모니터링 서버도 같이 시작한다.
            // showErrors:false인 이유는 웹 서버 시작 실패가 데스크톱 앱 실행 자체를 막으면 안 되기 때문이다.
            // 실패 내용은 Debug 출력으로만 남기고, 사용자는 기존 WinForms 기능을 계속 사용할 수 있게 한다.
            _ = ApplyWebMonitorSettingsAsync(showErrors: false);
            InitOverlayTimer();
        }

        private void InitAlertService()
        {
            AlertSettings alertSettings = null;
            
            try
            {
                // 데이터베이스에서 저장된 알림 설정 로드
                alertSettings = DatabaseHelper.LoadAlertSettings();
            }
            catch
            {
                // DB 로드 실패시 기본값 사용
                alertSettings = new AlertSettings();
            }

            _alertService = new AlertService(alertSettings);
            _alertService.AlertTriggered += AlertService_AlertTriggered;
        }

        /// <summary>
        /// AlertService 설정 업데이트 (ConfigForm에서 호출)
        /// </summary>
        public void RefreshAlertSettings()
        {
            var alertSettings = DatabaseHelper.LoadAlertSettings();
            _alertService.UpdateSettings(alertSettings);
        }

        /// <summary>
        /// 웹 모니터링 설정을 다시 적용한다.
        ///
        /// 현재 설정창 UI는 원래 상태로 되돌렸지만, 나중에 설정창에서 웹 모니터링 On/Off를 다시 제공할 경우
        /// 저장 후 이 메서드를 호출하면 서버 시작/중지가 즉시 반영된다.
        /// </summary>
        public Task RefreshWebMonitorSettings()
        {
            return ApplyWebMonitorSettingsAsync(showErrors: true);
        }

        /// <summary>
        /// DB에 저장된 WebMonitorSettings를 읽어 내장 웹 서버를 시작하거나 중지한다.
        ///
        /// - IsEnabled=true  : EmbeddedWebMonitorService.StartAsync(port)
        /// - IsEnabled=false : EmbeddedWebMonitorService.StopAsync()
        ///
        /// showErrors는 사용자에게 MessageBox를 띄울지 여부다.
        /// 앱 시작 시에는 조용히 실패하도록 false, 설정 변경 직후에는 사용자에게 알려야 하므로 true를 사용한다.
        /// </summary>
        private async Task ApplyWebMonitorSettingsAsync(bool showErrors)
        {
            var settings = WebMonitorSettings.Load();

            try
            {
                if (settings.IsEnabled)
                {
                    await EmbeddedWebMonitorService.Instance.StartAsync(settings.Port);
                }
                else
                {
                    await EmbeddedWebMonitorService.Instance.StopAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"웹 모니터링 시작 오류: {ex.Message}");

                if (showErrors)
                {
                    MessageBox.Show($"웹 모니터링 설정 적용 중 오류가 발생했습니다: {ex.Message}", "오류",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void InitDbLogTimer()
        {
            dbLogTimer = new System.Windows.Forms.Timer();
            dbLogTimer.Interval = 10000;
            dbLogTimer.Tick += DbLogTimer_Tick;
            dbLogTimer.Start();
        }

        private void DbLogTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                SystemSnapshot snapshot = _monitorService.GetCurrentStatus();

                DatabaseHelper.SaveHardwareLog(
                    snapshot.CpuUsage,
                    snapshot.RamUsage,
                    snapshot.GpuUsage
                );

                // 알림 기능이 활성화된 경우에만 알림 체크
                var currentSettings = _alertService.GetSettings();
                if (currentSettings.IsEnabled)
                {
                    _alertService.CheckAndAlert(
                        (float)snapshot.CpuUsage,
                        snapshot.RamUsage,
                        (float)snapshot.GpuUsage
                    );
                }

                var topProcs = Process.GetProcesses()
                                      .OrderByDescending(p => p.WorkingSet64)
                                      .Take(10)
                                      .Select(p => $"{p.ProcessName}({p.WorkingSet64 / 1024 / 1024}MB)")
                                      .ToArray();

                DatabaseHelper.SaveTop10ProcessRow(topProcs);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"타이머 오류: {ex.Message}");
            }
        }

        private void AlertService_AlertTriggered(object? sender, AlertEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => HandleAlert(e.AlertRecord)));
                return;
            }

            HandleAlert(e.AlertRecord);
        }

        private void HandleAlert(AlertRecord record)
        {
            string title = $"⚠️ {record.ResourceType} 알림";
            string message = record.Details;

            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            DatabaseHelper.SaveAlertLog(record.ResourceType, record.UsagePercentage, record.Details);
        }

        // ==========================================
        // [초기화 및 스타일 메서드]
        // ==========================================
        private void ApplyModernStyle()
        {
            this.BackColor = Color.FromArgb(243, 243, 243);
            this.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 129);
        }

        // ==========================================
        // [UI 컨트롤러 클릭 이벤트]
        // ==========================================
        private void btnMonitor_Click(object? sender, EventArgs e)
        {
            MonitorForm monitor = new MonitorForm();
            monitor.Show();
        }

        private void btnProcess_Click(object? sender, EventArgs e)
        {
            ProcessForm process = new ProcessForm();
            process.Show();
        }

        private void btnFocusMode_Click(object sender, EventArgs e)
        {
            FocusModeForm focus = new FocusModeForm();
            focus.Show();
        }
        private void btnStatistics_Click(object sender, EventArgs e)
        {
            StatisticsForm statistics = new StatisticsForm();
            statistics.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("프로그램을 종료하시겠습니까?", "종료", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Application.Run(new MainForm()) 구조에서는 메인 폼이 닫히면 앱 메시지 루프가 종료된다.
                // Application.Exit()보다 직접 Close()가 종료 흐름을 단순하게 만든다.
                Close();
            }
        }

        private async void btnWebMonitor_Click(object sender, EventArgs e)
        {
            try
            {
                var settings = WebMonitorSettings.Load();

                if (!EmbeddedWebMonitorService.Instance.IsRunning)
                {
                    await EmbeddedWebMonitorService.Instance.StartAsync(settings.Port);
                }

                string url = EmbeddedWebMonitorService.Instance.Url;
                if (string.IsNullOrWhiteSpace(url))
                {
                    url = $"http://localhost:{settings.Port}";
                }

                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"웹 모니터링 대시보드를 열 수 없습니다: {ex.Message}", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            ConfigForm configForm = new ConfigForm(this);
            configForm.ShowDialog();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // SQLite 하드웨어 로그 타이머를 먼저 멈춰서 폼 종료 중 DB 쓰기가 다시 발생하지 않게 한다.
            dbLogTimer?.Stop();

            // 종료 버튼이 UI 스레드에서 호출되므로 여기서는 웹 서버 종료를 기다리지 않는다.
            // Kestrel/하드웨어 수집 정리가 지연되면 폼 종료 자체가 막힐 수 있다.
            _ = Task.Run(async () =>
            {
                try
                {
                    await EmbeddedWebMonitorService.Instance.StopAsync().ConfigureAwait(false);
                    _monitorService?.Dispose();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"종료 정리 오류: {ex.Message}");
                }
            });

            base.OnFormClosing(e);
             
            _overlayTimer?.Stop(); // 오버레이 리소스 해제
            _overlayService?.HideOverlay();
        }

        /// <summary>
        /// 오버레이 구동에 필요한 1초 주기 타이머를 세팅하고 SQLite DB에서 최종 설정값을 복원.
        /// </summary>
        private void InitOverlayTimer()
        {
            _overlayTimer = new System.Timers.Timer();
            _overlayTimer.Interval = 1000; // 1초 주기 세팅
            _overlayTimer.Elapsed += OverlayTimer_Elapsed;

            // SQLite DB에서 마지막 On/Off 상태 및 시각 수치 로드
            IsOverlayEnabled = DatabaseHelper.LoadOverlaySettings();
            DatabaseHelper.LoadOverlayVisuals(out double opacity, out double scale);

            OverlayOpacity = opacity;
            OverlayScale = scale;

            // 로드 완료된 상태를 실시간 런타임에 최초 반영
            ApplyOverlaySettings();
        }

        /// <summary>
        /// 오버레이 활성화 상태에 따라 실시간 타이머 및 창을 제어하고 시각 수치(투명도, 크기)를 반영.
        /// </summary>
        public void ApplyOverlaySettings()
        {
            if (IsOverlayEnabled)
            {
                _overlayTimer?.Start();
                _overlayService?.ShowOverlay();

                // 중계 인터페이스를 구체 클래스로 캐스팅하여 WPF 창 내부 속성 원격 제어
                var service = _overlayService as OverlayService;
                service?.SetOpacity(OverlayOpacity);
                service?.SetScale(OverlayScale);
            }
            else
            {
                _overlayTimer?.Stop();
                _overlayService?.HideOverlay();
            }
        }

        // 1초 백그라운드 타이머 주기마다 백엔드 센서 값을 수집하여 WPF 오버레이 화면으로 밀어주는 핵심 루틴
        private void OverlayTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                // 하드웨어 모니터링 서비스 엔진으로부터 실시간 리소스 규격 데이터 수집
                SystemSnapshot snapshot = _monitorService.GetCurrentStatus();

                if (snapshot != null)
                {
                    // 비주얼 스튜디오 출력(Output) 창에서 실시간 데이터 매핑 여부를 검증하기 위한 추적 로그
                    System.Diagnostics.Trace.WriteLine($"[데이터 확인] CPU: {snapshot.CpuUsage}%, RAM: {snapshot.RamUsage}%, GPU: {snapshot.GpuUsage}%");

                    // 가공된 스냅샷 뭉치를 WPF 오버레이 레이어로 전송하여 화면 UI 갱신
                    _overlayService.UpdateHardwareData(snapshot);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"오버레이 오류: {ex.Message}");
            }
        }
    }
}