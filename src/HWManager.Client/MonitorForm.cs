using HWManager.Core.Models;
using HWManager.Core.Services;

namespace HWManager.Client
{
    public partial class MonitorForm : Form
    {
        // 서비스 및 스트리머 선언
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

        private void InitPlotCPU()
        {
            cpuStreamer = formsPlotCPU.Plot.Add.DataStreamer(60);
            cpuStreamer.ViewScrollLeft();
            for (int i = 0; i < 60; i++) cpuStreamer.Add(0);

            double[] tickPositions = { 0, 10, 20, 30, 40, 50, 60 };
            string[] tickLabels = { "60", "50", "40", "30", "20", "10", "0" };
            formsPlotCPU.Plot.Axes.Bottom.SetTicks(tickPositions, tickLabels);
            formsPlotCPU.Plot.Axes.Bottom.Label.Text = "Seconds Ago";

            cpuStreamer.Color = ScottPlot.Color.FromColor(Color.Blue);
            cpuStreamer.LineWidth = 2;
            formsPlotCPU.Plot.Axes.SetLimits(0, 60, 0, 100);
            formsPlotCPU.Refresh();
        }

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                // 서비스에서 데이터 뭉치 가져오기
                SystemSnapshot snapshot = _monitorService.GetCurrentStatus();

                // 차트 업데이트
                if (cpuStreamer != null) { cpuStreamer.Add(snapshot.CpuUsage); formsPlotCPU.Refresh(); }
                if (ramStreamer != null) { ramStreamer.Add(snapshot.RamUsage); formsPlotRAM.Refresh(); }
                if (gpuStreamer != null) { gpuStreamer.Add(snapshot.GpuUsage); formsPlotGPU.Refresh(); }

                // UI 업데이트
                UpdateDisplay(snapshot);
            }
            catch { }
        }

        private void UpdateDisplay(SystemSnapshot s)
        {
            pbCPU.Value = (int)Math.Min(s.CpuUsage, 100);
            pbRAM.Value = (int)Math.Min(s.RamUsage, 100);
            pbGPU.Value = (int)Math.Min(s.GpuUsage, 100);

            lblCPU.Text = $"CPU 사용량: {s.CpuUsage:F1}%";
            lblRAM.Text = $"RAM 사용량: {s.RamUsage:F1}%";
            lblGPU.Text = $"GPU 사용량: {s.GpuUsage:F1}%";
        }
    }
}