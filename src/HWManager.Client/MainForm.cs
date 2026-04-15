using HWManager.Core.Services;
using HWManager.Core.Models;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using HWManager.Core.Models;
using HWManager.Core.Services;

namespace HWManager.Client
{
    public partial class MainForm : Form
    {
        // 정확한 수집을 위한 서비스 선언
        private HardwareMonitorService _monitorService = new HardwareMonitorService();
        private System.Windows.Forms.Timer dbLogTimer;


        // 알림 서비스 추가
        private AlertService _alertService = new AlertService();
        private HashSet<string> _recentAlerts = new HashSet<string>(); // 중복 방지
        private Dictionary<string, DateTime> _lastAlertTime = new Dictionary<string, DateTime>(); // 마지막 알림 시간 기록

        public MainForm()
        {
            InitializeComponent();
            ApplyModernStyle();
            InitDbLogTimer();

            // 알림 이벤트 구독
            _alertService.AlertTriggered += AlertService_AlertTriggered;
        }





        private void InitDbLogTimer()
        {
            //10초 주기 타이머 유지
            dbLogTimer = new System.Windows.Forms.Timer();
            dbLogTimer.Interval = 10000; // 10초
            dbLogTimer.Tick += DbLogTimer_Tick;
            dbLogTimer.Start();
        }

        private void DbLogTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                // 1. 서비스에서 정확한 데이터 스냅샷 가져오기
                SystemSnapshot snapshot = _monitorService.GetCurrentStatus();

                // 2. 팀원 DB 함수 호출 (하드웨어 저장)
                DatabaseHelper.SaveHardwareLog(
                    snapshot.CpuUsage,
                    snapshot.RamUsage,
                    snapshot.GpuUsage
                );

                // 3. 프로세스 상위 10개 수집 및 저장
                var topProcs = Process.GetProcesses()
                                      .OrderByDescending(p => p.WorkingSet64)
                                      .Take(10)
                                      .Select(p => $"{p.ProcessName}({p.WorkingSet64 / 1024 / 1024}MB)")
                                      .ToArray();

                DatabaseHelper.SaveTop10ProcessRow(topProcs);
            }
            catch { }
        }

        //알림 이벤트 핸들러
       private void AlertService_AlertTriggered(object sender, AlertEventArgs e)
        {
            // UI 스레드에서 실행 보장
            if (InvokeRequired)
            {
                Invoke(new Action(() => HandleAlert(e.AlertRecord)));
                return;
            }

            HandleAlert(e.AlertRecord);
        }

        private void HandleAlert(AlertRecord record)
        {
            // 같은 리소스의 중복 알림 최소 60초 간격으로 제한
            string alertKey = record.ResourceType;
            
            // 마지막 알림 시간을 기록 (필드 추가 필요)
            if (_lastAlertTime.ContainsKey(alertKey))
            {
                var timeSinceLastAlert = DateTime.Now - _lastAlertTime[alertKey];
                if (timeSinceLastAlert.TotalSeconds < 60) // 60초 이내면 무시
                {
                    return;
                }
            }

            // 알림 시간 업데이트
            _lastAlertTime[alertKey] = DateTime.Now;

            string message = $"⚠️ {record.ResourceType} 알림\n\n" +
                           $"사용량: {record.UsagePercentage:F1}%\n" +
                           $"시간: {record.AlertTime:yyyy-MM-dd HH:mm:ss}";

            MessageBox.Show(message, "시스템 알림",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            // 데이터베이스 저장
            DatabaseHelper.SaveAlertLog(record.ResourceType, record.UsagePercentage, record.Details);
        }


        private void InitGpuCounters()
        {
            try
            {
                var category = new PerformanceCounterCategory("GPU Engine");
                foreach (var instance in category.GetInstanceNames())
                {
                    if (instance.EndsWith("engtype_3D"))
                    {
                        gpuCounters.Add(new PerformanceCounter("GPU Engine", "Utilization Percentage", instance));
                    }
                }
            }
            catch { }
        }

        private void ApplyModernStyle()
        {
            this.BackColor = Color.FromArgb(243, 243, 243);
            this.Text = "HWManager - Dashboard";

            var buttons = new List<Button> { btnMonitor, btnProcess, btnExit };

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

        private void btnMonitor_Click(object sender, EventArgs e)
        {
            MonitorForm monitor = new MonitorForm();
            monitor.Show();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            ProcessForm process = new ProcessForm();
            process.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("프로그램을 종료하시겠습니까?", "종료", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        // 프로그램 종료 시 하드웨어 리소스 해제
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _monitorService?.Dispose();
            base.OnFormClosing(e);
        }
    }
}