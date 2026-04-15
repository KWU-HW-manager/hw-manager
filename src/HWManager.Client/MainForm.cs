using System;
using System.Collections.Generic;
using System.Data;
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

        public MainForm()
        {
            InitializeComponent();
            ApplyModernStyle();
            InitDbLogTimer();
        }

        private void InitDbLogTimer()
        {
            //10초 주기 타이머 유지
            dbLogTimer = new System.Windows.Forms.Timer();
            dbLogTimer.Interval = 10000; // 10초
            dbLogTimer.Tick += DbLogTimer_Tick;
            dbLogTimer.Start();
        }

        private void DbLogTimer_Tick(object sender, EventArgs e)
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
            catch { } // 예외 발생 시 무시
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