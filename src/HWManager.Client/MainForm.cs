using HWManager.Core.Services;
using HWManager.Core.Models;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("프로그램을 종료하시겠습니까?", "종료", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            ConfigForm configForm = new ConfigForm(this);
            configForm.ShowDialog();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            dbLogTimer?.Stop();
            _monitorService?.Dispose();
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