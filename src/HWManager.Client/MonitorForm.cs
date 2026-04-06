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

        private double[] cpuData = new double[60]; // 60초 분량의 데이터 저장소
        private ScottPlot.Plottables.DataStreamer cpuStreamer; // 실시간 스트리밍 객체
        private ScottPlot.Plottables.DataStreamer ramStreamer;
        private ScottPlot.Plottables.DataStreamer gpuStreamer;

        public MonitorForm()
        {
            InitializeComponent();
            InitGpuCounters(); // GPU 카운터 초기설정
            InitPlotCPU();
            InitPlotRAM();
            InitPlotGPU();
        }

        private void InitPlotCPU()
        {
            // 1. 데이터 스트리머 생성 (60초 분량)
            cpuStreamer = formsPlotCPU.Plot.Add.DataStreamer(60);

            // 2. 오른쪽 끝에서 시작해서 왼쪽으로 밀리는 모드 설정
            cpuStreamer.ViewScrollLeft();

            // 3. [핵심] 시작하자마자 0을 60개 채워넣어 '현재 위치'를 오른쪽 끝으로 보냄
            for (int i = 0; i < 60; i++)
            {
                cpuStreamer.Add(0);
            }
            double[] tickPositions = { 0, 10, 20, 30, 40, 50, 60 };
            string[] tickLabels = { "60", "50", "40", "30", "20", "10", "0" };
            formsPlotCPU.Plot.Axes.Bottom.SetTicks(tickPositions, tickLabels);

            // 축 제목도 달아주면 더 전문적으로 보입니다.
            formsPlotCPU.Plot.Axes.Bottom.Label.Text = "Seconds Ago";
            // --------------------------------------
            // 4. 선 스타일 설정 (파란색, 두께 2)
            cpuStreamer.Color = ScottPlot.Color.FromColor(System.Drawing.Color.Blue);
            cpuStreamer.LineWidth = 2;

            // 5. 축 범위 고정 (X축 0~60, Y축 0~100)
            formsPlotCPU.Plot.Axes.SetLimits(0, 60, 0, 100);

            // 6. 화면 새로고침
            formsPlotCPU.Refresh();
        }

        private void InitPlotRAM()
        {
            // 1. RAM용 스트리머 생성 (60초 분량)
            ramStreamer = formsPlotRAM.Plot.Add.DataStreamer(60);

            // 2. 왼쪽으로 흐르는 뷰 설정
            ramStreamer.ViewScrollLeft();

            // 3. 초기 위치를 오른쪽 끝으로 보내기 위해 0으로 채움
            for (int i = 0; i < 60; i++)
            {
                ramStreamer.Add(0);
            }

            // 4. X축 레이블 설정 (60 ~ 0)
            double[] tickPositions = { 0, 10, 20, 30, 40, 50, 60 };
            string[] tickLabels = { "60", "50", "40", "30", "20", "10", "0" };
            formsPlotRAM.Plot.Axes.Bottom.SetTicks(tickPositions, tickLabels);
            formsPlotRAM.Plot.Axes.Bottom.Label.Text = "Seconds Ago";

            // 5. 선 스타일 (녹색) 및 축 범위 고정
            ramStreamer.Color = ScottPlot.Color.FromColor(Color.Green);
            ramStreamer.LineWidth = 2;
            formsPlotRAM.Plot.Axes.SetLimits(0, 60, 0, 100);

            // 6. 차트 배경색 설정 (디자인 통일)
            formsPlotRAM.Plot.FigureBackground.Color = ScottPlot.Color.FromColor(Color.FromArgb(243, 243, 243));
            formsPlotRAM.Plot.DataBackground.Color = ScottPlot.Color.FromColor(Color.White);

            formsPlotRAM.Refresh();
        }

        private void InitPlotGPU()
        {
            // 1. GPU용 스트리머 생성 (60초 분량)
            gpuStreamer = formsPlotGPU.Plot.Add.DataStreamer(60);

            // 2. 왼쪽으로 흐르는 뷰 설정
            gpuStreamer.ViewScrollLeft();

            // 3. 초기 위치 동기화를 위해 0으로 채움
            for (int i = 0; i < 60; i++)
            {
                gpuStreamer.Add(0);
            }

            // 4. X축 레이블 설정 (60 ~ 0)
            double[] tickPositions = { 0, 10, 20, 30, 40, 50, 60 };
            string[] tickLabels = { "60", "50", "40", "30", "20", "10", "0" };
            formsPlotGPU.Plot.Axes.Bottom.SetTicks(tickPositions, tickLabels);
            formsPlotGPU.Plot.Axes.Bottom.Label.Text = "Seconds Ago";

            // 5. 선 스타일 (주황) 및 축 범위 고정
            gpuStreamer.Color = ScottPlot.Color.FromColor(Color.OrangeRed);
            gpuStreamer.LineWidth = 2;
            formsPlotGPU.Plot.Axes.SetLimits(0, 60, 0, 100);

            // 6. 차트 배경색 설정
            formsPlotGPU.Plot.FigureBackground.Color = ScottPlot.Color.FromColor(Color.FromArgb(243, 243, 243));
            formsPlotGPU.Plot.DataBackground.Color = ScottPlot.Color.FromColor(Color.White);

            formsPlotGPU.Refresh();
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
                if (cpuStreamer != null) // CPU 차트
                {
                    cpuStreamer.Add(cpuVal); // 새로운 점 찍기
                    formsPlotCPU.Refresh();  // 화면 다시 그리기
                }
                if (ramStreamer != null) // RAM 차트
                {
                    ramStreamer.Add(ramVal);
                    formsPlotRAM.Refresh();
                }
                if (gpuStreamer != null) // GPU 차트
                {
                    gpuStreamer.Add(gpuVal);
                    formsPlotGPU.Refresh();
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

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}