using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;
using System.Drawing;

namespace HWManager.Client
{
    public partial class MonitorForm : Form
    {
        private PerformanceCounter cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private List<PerformanceCounter> gpus = new List<PerformanceCounter>();
        private ComputerInfo ram = new ComputerInfo();

        private double[] cpuData = new double[60];
        private ScottPlot.Plottables.DataStreamer cpuStreamer;
        private ScottPlot.Plottables.DataStreamer ramStreamer;
        private ScottPlot.Plottables.DataStreamer gpuStreamer;

        public MonitorForm()
        {
            InitializeComponent();
            InitGpuCounters();
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
            cpuStreamer.Color = ScottPlot.Color.FromColor(System.Drawing.Color.Blue);
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

        private void InitGpuCounters()
        {
            try
            {
                var category = new PerformanceCounterCategory("GPU Engine");
                foreach (var instance in category.GetInstanceNames())
                {
                    if (instance.EndsWith("engtype_3D"))
                        gpus.Add(new PerformanceCounter("GPU Engine", "Utilization Percentage", instance));
                }
            }
            catch { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                float cpuVal = cpu.NextValue();
                double totalRam = ram.TotalPhysicalMemory;
                double availRam = ram.AvailablePhysicalMemory;
                double ramVal = (totalRam - availRam) / totalRam * 100;
                float gpuVal = 0;
                foreach (var g in gpus) gpuVal += g.NextValue();

                if (cpuStreamer != null) { cpuStreamer.Add(cpuVal); formsPlotCPU.Refresh(); }
                if (ramStreamer != null) { ramStreamer.Add(ramVal); formsPlotRAM.Refresh(); }
                if (gpuStreamer != null) { gpuStreamer.Add(gpuVal); formsPlotGPU.Refresh(); }
                UpdateDisplay(cpuVal, ramVal, gpuVal);
            }
            catch { }
        }

        private void UpdateDisplay(float c, double r, float g)
        {
            pbCPU.Value = (int)Math.Min(c, 100);
            pbRAM.Value = (int)Math.Min(r, 100);
            pbGPU.Value = (int)Math.Min(g, 100);
            lblCPU.Text = $"CPU 사용량: {c:F1}%";
            lblRAM.Text = $"RAM 사용량: {r:F1}%";
            lblGPU.Text = $"GPU 사용량: {g:F1}%";
        }
    }
}