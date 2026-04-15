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

namespace HWManager.Client
{
    public partial class MainForm : Form
    {
        private PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private ComputerInfo computerInfo = new ComputerInfo();
        private List<PerformanceCounter> gpuCounters = new List<PerformanceCounter>();
        private System.Windows.Forms.Timer dbLogTimer;


        // 알림 서비스 추가
        private AlertService _alertService = new AlertService();
        private HashSet<string> _recentAlerts = new HashSet<string>(); // 중복 방지

        public MainForm()
        {
            InitializeComponent();
            ApplyModernStyle();

            InitGpuCounters();
            InitDbLogTimer();

            // 알림 이벤트 구독
            //_alertService.AlertTriggered += AlertService_AlertTriggered;
        }





        private void InitDbLogTimer()
        {
            dbLogTimer = new System.Windows.Forms.Timer();
            dbLogTimer.Interval = 10000;
            dbLogTimer.Tick += DbLogTimer_Tick;
            dbLogTimer.Start();
        }

        private void DbLogTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // 하드웨어 데이터 수집 및 저장
                float cpuVal = cpuCounter.NextValue();
                double totalRam = computerInfo.TotalPhysicalMemory;
                double availRam = computerInfo.AvailablePhysicalMemory;
                double ramVal = (totalRam - availRam) / totalRam * 100;

                float gpuVal = 0;
                foreach (var g in gpuCounters) gpuVal += g.NextValue();

                DatabaseHelper.SaveHardwareLog(cpuVal, ramVal, gpuVal);

                // 프로세스 상위 10개 수집 및 저장
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
            // 같은 리소스의 중복 알림 3초 내 방지
            string alertKey = $"{record.ResourceType}_{DateTime.Now:mm:ss}";
            if (_recentAlerts.Contains(alertKey)) return;

            _recentAlerts.Add(alertKey);

            string message = $"⚠️ {record.ResourceType} 알림\n\n" +
                           $"사용량: {record.UsagePercentage:F1}%\n" +
                           $"시간: {record.AlertTime:yyyy-MM-dd HH:mm:ss}";

            MessageBox.Show(message, "시스템 알림",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            // 데이터베이스 저장
            //DatabaseHelper.SaveAlertLog(record.ResourceType, record.UsagePercentage, record.Details);
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
    }
}