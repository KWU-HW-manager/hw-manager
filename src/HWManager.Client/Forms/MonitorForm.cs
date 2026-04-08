using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices; // 참조 추가한 라이브러리

namespace HWManager.Client
{
    public partial class MonitorForm : Form
    {
        // 성능 측정을 위한 객체들
        private PerformanceCounter cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private List<PerformanceCounter> gpus = new List<PerformanceCounter>();
        private ComputerInfo ram = new ComputerInfo();

        public MonitorForm()
        {
            InitializeComponent();
            InitGpuCounters(); // GPU 카운터 초기설정
        }

        // 윈도우 GPU 엔진 카운터 수집
        private void InitGpuCounters()
        {
            try
            {
                var category = new PerformanceCounterCategory("GPU Engine");
                foreach (var instance in category.GetInstanceNames())
                {
                    // 일반적인 3D 렌더링 점유율 인스턴스만 추가
                    if (instance.EndsWith("engtype_3D"))
                    {
                        gpus.Add(new PerformanceCounter("GPU Engine", "Utilization Percentage", instance));
                    }
                }
            }
            catch { /* GPU 카운터 획득 실패 시 처리 */ }
        }

        // 타이머가 1초마다 실행할 내용
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                // 1. CPU 값 계산
                float cpuVal = cpu.NextValue();

                // 2. RAM 값 계산 (전체 대비 사용량 %)
                double totalRam = ram.TotalPhysicalMemory;
                double availRam = ram.AvailablePhysicalMemory;
                double ramVal = (totalRam - availRam) / totalRam * 100;

                // 3. GPU 값 계산 (수집된 모든 3D 엔진 값 합산)
                float gpuVal = 0;
                foreach (var g in gpus)
                {
                    gpuVal += g.NextValue();
                }

                // UI 업데이트 (Progressbar & Label)
                UpdateDisplay(cpuVal, ramVal, gpuVal);
            }
            catch { /* 측정 오류 시 무시 */ }
        }

        private void UpdateDisplay(float c, double r, float g)
        {
            // 프로그레스바 수치 적용 (최대 100 제한)
            pbCPU.Value = (int)Math.Min(c, 100);
            pbRAM.Value = (int)Math.Min(r, 100);
            pbGPU.Value = (int)Math.Min(g, 100);

            // 텍스트 라벨 적용
            lblCPU.Text = $"CPU 사용량: {c:F1}%";
            lblRAM.Text = $"RAM 사용량: {r:F1}%";
            lblGPU.Text = $"GPU 사용량: {g:F1}%";
        }
    }
}