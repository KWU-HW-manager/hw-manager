using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.VisualBasic.Devices;

namespace HWManager.Client
{
    public partial class MainForm : Form
    {
        private PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private ComputerInfo computerInfo = new ComputerInfo();
        private List<PerformanceCounter> gpuCounters = new List<PerformanceCounter>();
        private System.Windows.Forms.Timer dbLogTimer;

        public MainForm()
        {
            InitializeComponent();
            ApplyModernStyle();

            InitGpuCounters();
            InitDbLogTimer();
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