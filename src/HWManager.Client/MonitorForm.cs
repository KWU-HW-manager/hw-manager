using HWManager.Core.Models;
using HWManager.Core.Services;

namespace HWManager.Client
{
    public partial class MonitorForm : Form
    {
        // 하드웨어 모니터링 백엔드 서비스 및 실시간 차트 스트리머 객체
        private HardwareMonitorService _monitorService = new HardwareMonitorService();
        private ScottPlot.Plottables.DataStreamer cpuStreamer;
        private ScottPlot.Plottables.DataStreamer ramStreamer;
        private ScottPlot.Plottables.DataStreamer gpuStreamer;

        public MonitorForm()
        {
            InitializeComponent();

            // 차트 초기화 함수들 호출
            InitPlotCPU();
            InitPlotRAM();
            InitPlotGPU();
        }

        private void btnRefreshHardware_Click(object sender, EventArgs e)
        {
            // DatabaseHelper.cs: GetLogs 호출
            System.Data.DataTable dt = DatabaseHelper.GetLogs("Hardware");
            dgvHardwareLog.DataSource = dt;
            dgvHardwareLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // CPU 실시간 차트 초기화 (60초 분량 스크롤, 청색 선)
        private void InitPlotCPU()
        {
            cpuStreamer = formsPlotCPU.Plot.Add.DataStreamer(60);
            cpuStreamer.ViewScrollLeft(); // 왼쪽으로 밀리는 스크롤 연출
            for (int i = 0; i < 60; i++) cpuStreamer.Add(0); // 초기 데이터 0으로 채움

            double[] tickPositions = { 0, 10, 20, 30, 40, 50, 60 };
            string[] tickLabels = { "60", "50", "40", "30", "20", "10", "0" };
            formsPlotCPU.Plot.Axes.Bottom.SetTicks(tickPositions, tickLabels);
            formsPlotCPU.Plot.Axes.Bottom.Label.Text = "Seconds Ago";

            cpuStreamer.Color = ScottPlot.Color.FromColor(Color.Blue);
            cpuStreamer.LineWidth = 2;
            formsPlotCPU.Plot.Axes.SetLimits(0, 60, 0, 100); // Y축 범위 0 ~ 100% 고정
            formsPlotCPU.Refresh();
        }

        // RAM 실시간 차트 초기화 (60초 분량 스크롤, 녹색 선)
        private void InitPlotRAM()
        {
            ramStreamer = formsPlotRAM.Plot.Add.DataStreamer(60);
            ramStreamer.ViewScrollLeft();
            for (int i = 0; i < 60; i++) ramStreamer.Add(0);

            double[] tickPositions = { 0, 10, 20, 30, 40, 50, 60 };
            string[] tickLabels = { "60", "50", "40", "30", "20", "10", "0" };
            formsPlotRAM.Plot.Axes.Bottom.SetTicks(tickPositions, tickLabels);
            formsPlotRAM.Plot.Axes.Bottom.Label.Text = "Seconds Ago";

            ramStreamer.Color = ScottPlot.Color.FromColor(Color.Green);
            ramStreamer.LineWidth = 2;
            formsPlotRAM.Plot.Axes.SetLimits(0, 60, 0, 100);

            formsPlotRAM.Plot.FigureBackground.Color = ScottPlot.Color.FromColor(Color.FromArgb(243, 243, 243));
            formsPlotRAM.Plot.DataBackground.Color = ScottPlot.Color.FromColor(Color.White);
            formsPlotRAM.Refresh();
        }

        // GPU 실시간 차트 초기화 (60초 분량 스크롤, 주황 선)
        private void InitPlotGPU()
        {
            gpuStreamer = formsPlotGPU.Plot.Add.DataStreamer(60);
            gpuStreamer.ViewScrollLeft();
            for (int i = 0; i < 60; i++) gpuStreamer.Add(0);

            double[] tickPositions = { 0, 10, 20, 30, 40, 50, 60 };
            string[] tickLabels = { "60", "50", "40", "30", "20", "10", "0" };
            formsPlotGPU.Plot.Axes.Bottom.SetTicks(tickPositions, tickLabels);
            formsPlotGPU.Plot.Axes.Bottom.Label.Text = "Seconds Ago";

            gpuStreamer.Color = ScottPlot.Color.FromColor(Color.OrangeRed);
            gpuStreamer.LineWidth = 2;
            formsPlotGPU.Plot.Axes.SetLimits(0, 60, 0, 100);

            formsPlotGPU.Plot.FigureBackground.Color = ScottPlot.Color.FromColor(Color.FromArgb(243, 243, 243));
            formsPlotGPU.Plot.DataBackground.Color = ScottPlot.Color.FromColor(Color.White);
            formsPlotGPU.Refresh();
        }

        // 타이머 주기(예: 1초)마다 백엔드 센서 데이터를 읽어 차트와 UI 동시 반영
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                // 백엔드 서비스로부터 표준 데이터 스냅샷 수집
                SystemSnapshot snapshot = _monitorService.GetCurrentStatus();

                // 차트 스트리머에 새 데이터 포인트 추가 및 그래프 새로고침
                if (cpuStreamer != null) { cpuStreamer.Add(snapshot.CpuUsage); formsPlotCPU.Refresh(); }
                if (ramStreamer != null) { ramStreamer.Add(snapshot.RamUsage); formsPlotRAM.Refresh(); }
                if (gpuStreamer != null) { gpuStreamer.Add(snapshot.GpuUsage); formsPlotGPU.Refresh(); }

                // 프로그레스 바 및 텍스트 정보 업데이트
                UpdateDisplay(snapshot);
            }
            catch { }
        }

        // 프로그레스 바 수치 매핑 및 텍스트 레이블 문자열 포맷팅
        private void UpdateDisplay(SystemSnapshot s)
        {
            pbCPU.Value = (int)Math.Min(s.CpuUsage, 100);
            pbRAM.Value = (int)Math.Min(s.RamUsage, 100);
            pbGPU.Value = (int)Math.Min(s.GpuUsage, 100);

            lblCPU.Text = $"CPU 사용량: {s.CpuUsage:F1}%";
            lblRAM.Text = $"RAM 사용량: {s.RamUsage:F1}%";
            lblGPU.Text = $"GPU 사용량: {s.GpuUsage:F1}%";
        }

        // 폼이 닫힐 때 백그라운드 타이머를 중지하고 센서 서비스 자원을 안전하게 해제
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            timer1.Stop();
            _monitorService.Dispose(); // 오브젝트 메모리 누수 방지
            base.OnFormClosing(e);
        }
    }
}