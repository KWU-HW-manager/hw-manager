using HWManager.Core.Models;
using HWManager.Core.Services;
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
            // 알림 설정 초기화
            var alertSettings = new AlertSettings
            {
                CpuThreshold = 90f,
                RamThreshold = 90f,
                GpuThreshold = 90f
            };

            _alertService = new AlertService(alertSettings);
            _alertService.AlertTriggered += AlertService_AlertTriggered;
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

                _alertService.CheckAndAlert(
                    (float)snapshot.CpuUsage,
                    snapshot.RamUsage,
                    (float)snapshot.GpuUsage
                );

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
            string alertKey = record.ResourceType;

            if (_lastAlertTime.ContainsKey(alertKey))
            {
                var timeSinceLastAlert = DateTime.Now - _lastAlertTime[alertKey];
                if (timeSinceLastAlert.TotalSeconds < 60)
                {
                    return;
                }
            }

            _lastAlertTime[alertKey] = DateTime.Now;

            string message = $"⚠️ {record.ResourceType} 알림\n\n" +
                           $"사용량: {record.UsagePercentage:F1}%\n" +
                           $"시간: {record.AlertTime:yyyy-MM-dd HH:mm:ss}";

            MessageBox.Show(message, "시스템 알림",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            DatabaseHelper.SaveAlertLog(record.ResourceType, record.UsagePercentage, record.Details);
        }

        private void ApplyModernStyle()
        {
            this.BackColor = Color.FromArgb(243, 243, 243);
            this.Text = "HWManager - Dashboard";

            var buttons = new List<Button> { btnMonitor, btnProcess, btnFocusMode, btnExit, btnSettings };

            foreach (var btn in buttons)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = Color.White;
                btn.Cursor = Cursors.Hand;
                btn.Font = new Font("맑은 고딕", 11, FontStyle.Bold);
                btn.MouseEnter += (s, e) => { btn.BackColor = Color.FromArgb(235, 235, 235); };
                btn.MouseLeave += (s, e) => { btn.BackColor = Color.White; };
            }
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
            ConfigForm configForm = new ConfigForm();
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