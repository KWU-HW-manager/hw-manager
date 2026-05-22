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

        public MainForm()
        {
            InitializeComponent();
            ApplyModernStyle();
            InitAlertService();
            InitDbLogTimer();
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

        private void ApplyModernStyle()
        {
            this.BackColor = Color.FromArgb(243, 243, 243);
            this.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 129);
        }

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
        }
    }
}