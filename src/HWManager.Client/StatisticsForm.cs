using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace HWManager.Client
{
    public partial class StatisticsForm : Form
    {
        private TabControl tabControl;

        private DateTimePicker dtpDaily;
        private DataGridView dgvDailySummary;
        private DataGridView dgvHourlySummary;

        private DateTimePicker dtpWeeklyStart;
        private DateTimePicker dtpWeeklyEnd;
        private Label lblHighestDate;
        private DataGridView dgvWeeklySummary;

        private ComboBox cboGraphPeriod;
        private CheckedListBox chkGraphResource;
        private ScottPlot.WinForms.FormsPlot plotUsage;

        private DateTimePicker dtpAlertStart;
        private DateTimePicker dtpAlertEnd;
        private DataGridView dgvAlertList;
        private DataGridView dgvAlertHour;
        private DataGridView dgvAlertResource;
        private ScottPlot.WinForms.FormsPlot plotAlert;

        private DateTimePicker dtpProcessStart;
        private DateTimePicker dtpProcessEnd;
        private TextBox txtProcessKeyword;
        private DataGridView dgvFrequentProcess;
        private DataGridView dgvMemoryProcess;

        public StatisticsForm()
        {
            InitializeComponent();
            BuildRuntimeUI();
            LoadAll();
        }

        private void BuildRuntimeUI()
        {
            Text = "통계 및 분석";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1200, 800);
            MinimumSize = new Size(900, 600);
            BackColor = Color.FromArgb(243, 243, 243);
            Font = new Font("맑은 고딕", 9F);

            tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;

            tabControl.TabPages.Add(CreateDailyTab());
            tabControl.TabPages.Add(CreateWeeklyTab());
            tabControl.TabPages.Add(CreateGraphTab());
            tabControl.TabPages.Add(CreateAlertTab());
            tabControl.TabPages.Add(CreateProcessTab());

            Controls.Add(tabControl);
        }

        private TabPage CreateDailyTab()
        {
            TabPage tab = new TabPage("일간 통계");

            TableLayoutPanel root = CreateRootLayout(2);
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            FlowLayoutPanel top = CreateTopPanel();

            top.Controls.Add(new Label
            {
                Text = "날짜",
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(0, 6, 0, 0)
            });

            dtpDaily = new DateTimePicker();
            dtpDaily.Width = 180;
            dtpDaily.Value = DateTime.Today;
            top.Controls.Add(dtpDaily);

            Button btnLoad = CreateButton("조회");
            btnLoad.Click += (s, e) => LoadDailyStats();
            top.Controls.Add(btnLoad);

            SplitContainer split = new SplitContainer();
            split.Dock = DockStyle.Fill;
            split.Orientation = Orientation.Horizontal;
            split.SplitterDistance = 300;

            dgvDailySummary = CreateGrid();
            dgvHourlySummary = CreateGrid();

            split.Panel1.Controls.Add(dgvDailySummary);
            split.Panel2.Controls.Add(dgvHourlySummary);

            root.Controls.Add(top, 0, 0);
            root.Controls.Add(split, 0, 1);

            tab.Controls.Add(root);
            return tab;
        }

        private TabPage CreateWeeklyTab()
        {
            TabPage tab = new TabPage("주간 통계");

            TableLayoutPanel root = CreateRootLayout(2);
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            FlowLayoutPanel top = CreateTopPanel();

            top.Controls.Add(CreateTextLabel("시작일"));
            dtpWeeklyStart = new DateTimePicker();
            dtpWeeklyStart.Width = 180;
            dtpWeeklyStart.Value = DateTime.Today.AddDays(-6);
            top.Controls.Add(dtpWeeklyStart);

            top.Controls.Add(CreateTextLabel("종료일"));
            dtpWeeklyEnd = new DateTimePicker();
            dtpWeeklyEnd.Width = 180;
            dtpWeeklyEnd.Value = DateTime.Today;
            top.Controls.Add(dtpWeeklyEnd);

            Button btnLoad = CreateButton("조회");
            btnLoad.Click += (s, e) => LoadWeeklyStats();
            top.Controls.Add(btnLoad);

            lblHighestDate = new Label();
            lblHighestDate.AutoSize = true;
            lblHighestDate.Padding = new Padding(15, 6, 0, 0);
            lblHighestDate.Text = "가장 사용량이 높았던 날짜:";
            top.Controls.Add(lblHighestDate);

            dgvWeeklySummary = CreateGrid();

            root.Controls.Add(top, 0, 0);
            root.Controls.Add(dgvWeeklySummary, 0, 1);

            tab.Controls.Add(root);
            return tab;
        }

        private TabPage CreateGraphTab()
        {
            TabPage tab = new TabPage("사용량 그래프");

            TableLayoutPanel root = CreateRootLayout(2);
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            FlowLayoutPanel top = CreateTopPanel();

            top.Controls.Add(CreateTextLabel("기간"));
            cboGraphPeriod = new ComboBox();
            cboGraphPeriod.DropDownStyle = ComboBoxStyle.DropDownList;
            cboGraphPeriod.Width = 150;
            cboGraphPeriod.Items.Add("오늘");
            cboGraphPeriod.Items.Add("최근 7일");
            cboGraphPeriod.Items.Add("최근 30일");
            cboGraphPeriod.SelectedIndex = 0;
            top.Controls.Add(cboGraphPeriod);

            Button btnLoad = CreateButton("그래프 조회");
            btnLoad.Click += (s, e) => LoadUsageGraph();
            top.Controls.Add(btnLoad);

            chkGraphResource = new CheckedListBox();
            chkGraphResource.Width = 150;
            chkGraphResource.Height = 70;
            chkGraphResource.CheckOnClick = true;
            chkGraphResource.Items.Add("CPU", true);
            chkGraphResource.Items.Add("RAM", true);
            chkGraphResource.Items.Add("GPU", true);
            top.Controls.Add(chkGraphResource);

            plotUsage = new ScottPlot.WinForms.FormsPlot();
            plotUsage.Dock = DockStyle.Fill;

            root.Controls.Add(top, 0, 0);
            root.Controls.Add(plotUsage, 0, 1);

            tab.Controls.Add(root);
            return tab;
        }

        private TabPage CreateAlertTab()
        {
            TabPage tab = new TabPage("알림 분석");

            TableLayoutPanel root = CreateRootLayout(2);
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            FlowLayoutPanel top = CreateTopPanel();

            top.Controls.Add(CreateTextLabel("시작일"));
            dtpAlertStart = new DateTimePicker();
            dtpAlertStart.Width = 180;
            dtpAlertStart.Value = DateTime.Today.AddDays(-6);
            top.Controls.Add(dtpAlertStart);

            top.Controls.Add(CreateTextLabel("종료일"));
            dtpAlertEnd = new DateTimePicker();
            dtpAlertEnd.Width = 180;
            dtpAlertEnd.Value = DateTime.Today;
            top.Controls.Add(dtpAlertEnd);

            Button btnLoad = CreateButton("조회");
            btnLoad.Click += (s, e) => LoadAlertStats();
            top.Controls.Add(btnLoad);

            SplitContainer mainSplit = new SplitContainer();
            mainSplit.Dock = DockStyle.Fill;
            mainSplit.SplitterDistance = 580;

            SplitContainer leftSplit = new SplitContainer();
            leftSplit.Dock = DockStyle.Fill;
            leftSplit.Orientation = Orientation.Horizontal;
            leftSplit.SplitterDistance = 300;

            dgvAlertList = CreateGrid();
            dgvAlertHour = CreateGrid();

            leftSplit.Panel1.Controls.Add(dgvAlertList);
            leftSplit.Panel2.Controls.Add(dgvAlertHour);

            SplitContainer rightSplit = new SplitContainer();
            rightSplit.Dock = DockStyle.Fill;
            rightSplit.Orientation = Orientation.Horizontal;
            rightSplit.SplitterDistance = 250;

            dgvAlertResource = CreateGrid();
            plotAlert = new ScottPlot.WinForms.FormsPlot();
            plotAlert.Dock = DockStyle.Fill;

            rightSplit.Panel1.Controls.Add(dgvAlertResource);
            rightSplit.Panel2.Controls.Add(plotAlert);

            mainSplit.Panel1.Controls.Add(leftSplit);
            mainSplit.Panel2.Controls.Add(rightSplit);

            root.Controls.Add(top, 0, 0);
            root.Controls.Add(mainSplit, 0, 1);

            tab.Controls.Add(root);
            return tab;
        }

        private TabPage CreateProcessTab()
        {
            TabPage tab = new TabPage("프로세스 분석");

            TableLayoutPanel root = CreateRootLayout(2);
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            FlowLayoutPanel top = CreateTopPanel();

            top.Controls.Add(CreateTextLabel("시작일"));
            dtpProcessStart = new DateTimePicker();
            dtpProcessStart.Width = 180;
            dtpProcessStart.Value = DateTime.Today.AddDays(-6);
            top.Controls.Add(dtpProcessStart);

            top.Controls.Add(CreateTextLabel("종료일"));
            dtpProcessEnd = new DateTimePicker();
            dtpProcessEnd.Width = 180;
            dtpProcessEnd.Value = DateTime.Today;
            top.Controls.Add(dtpProcessEnd);

            top.Controls.Add(CreateTextLabel("검색"));
            txtProcessKeyword = new TextBox();
            txtProcessKeyword.Width = 180;
            top.Controls.Add(txtProcessKeyword);

            Button btnLoad = CreateButton("조회");
            btnLoad.Click += (s, e) => LoadProcessStats();
            top.Controls.Add(btnLoad);

            SplitContainer split = new SplitContainer();
            split.Dock = DockStyle.Fill;
            split.Orientation = Orientation.Horizontal;
            split.SplitterDistance = 300;

            dgvFrequentProcess = CreateGrid();
            dgvMemoryProcess = CreateGrid();

            split.Panel1.Controls.Add(dgvFrequentProcess);
            split.Panel2.Controls.Add(dgvMemoryProcess);

            root.Controls.Add(top, 0, 0);
            root.Controls.Add(split, 0, 1);

            tab.Controls.Add(root);
            return tab;
        }

        private void LoadAll()
        {
            LoadDailyStats();
            LoadWeeklyStats();
            LoadUsageGraph();
            LoadAlertStats();
            LoadProcessStats();
        }

        private void LoadDailyStats()
        {
            dgvDailySummary.DataSource = DatabaseHelper.GetDailyHardwareSummary(dtpDaily.Value.Date);
            dgvHourlySummary.DataSource = DatabaseHelper.GetDailyHourlyAverage(dtpDaily.Value.Date);
        }

        private void LoadWeeklyStats()
        {
            DateTime start = dtpWeeklyStart.Value.Date;
            DateTime end = dtpWeeklyEnd.Value.Date;

            dgvWeeklySummary.DataSource = DatabaseHelper.GetWeeklyHardwareSummary(start, end);
            lblHighestDate.Text = "가장 사용량이 높았던 날짜: " + DatabaseHelper.GetHighestUsageDate(start, end);
        }

        private void LoadUsageGraph()
        {
            DateTime end = DateTime.Today;
            DateTime start = DateTime.Today;

            string period = cboGraphPeriod.SelectedItem?.ToString() ?? "오늘";

            if (period == "최근 7일")
            {
                start = DateTime.Today.AddDays(-6);
            }
            else if (period == "최근 30일")
            {
                start = DateTime.Today.AddDays(-29);
            }

            DataTable dt = DatabaseHelper.GetUsageGraphData(start, end);

            plotUsage.Plot.Clear();

            if (dt.Rows.Count == 0)
            {
                plotUsage.Plot.Title("조회된 로그가 없습니다.");
                plotUsage.Refresh();
                return;
            }

            double[] xs = new double[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                xs[i] = i;
            }

            if (chkGraphResource.CheckedItems.Contains("CPU"))
            {
                double[] ys = GetDoubleArray(dt, "CPU");
                var line = plotUsage.Plot.Add.Scatter(xs, ys);
                line.LegendText = "CPU";
            }

            if (chkGraphResource.CheckedItems.Contains("RAM"))
            {
                double[] ys = GetDoubleArray(dt, "RAM");
                var line = plotUsage.Plot.Add.Scatter(xs, ys);
                line.LegendText = "RAM";
            }

            if (chkGraphResource.CheckedItems.Contains("GPU"))
            {
                double[] ys = GetDoubleArray(dt, "GPU");
                var line = plotUsage.Plot.Add.Scatter(xs, ys);
                line.LegendText = "GPU";
            }

            plotUsage.Plot.Axes.Left.Label.Text = "사용량 (%)";
            plotUsage.Plot.Title("저장된 DB 로그 기반 사용량 그래프");
            plotUsage.Plot.Axes.SetLimitsY(0, 100);
            plotUsage.Plot.ShowLegend();

            plotUsage.Refresh();
        }

        private void LoadAlertStats()
        {
            DateTime start = dtpAlertStart.Value.Date;
            DateTime end = dtpAlertEnd.Value.Date;

            dgvAlertList.DataSource = DatabaseHelper.GetAlertList(start, end);
            dgvAlertHour.DataSource = DatabaseHelper.GetAlertHourlyStats(start, end);
            dgvAlertResource.DataSource = DatabaseHelper.GetAlertResourceStats(start, end);

            DrawAlertGraph(start, end);
        }

        private void DrawAlertGraph(DateTime start, DateTime end)
        {
            DataTable dt = DatabaseHelper.GetAlertTrendStats(start, end);

            plotAlert.Plot.Clear();

            if (dt.Rows.Count == 0)
            {
                plotAlert.Plot.Title("조회된 알림 로그가 없습니다.");
                plotAlert.Refresh();
                return;
            }

            double[] xs = new double[dt.Rows.Count];
            double[] ys = new double[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                xs[i] = i;
                ys[i] = Convert.ToDouble(dt.Rows[i]["알림횟수"]);
            }

            var line = plotAlert.Plot.Add.Scatter(xs, ys);
            line.LegendText = "알림 횟수";

            plotAlert.Plot.Title("알림 발생 추이");
            plotAlert.Plot.Axes.Left.Label.Text = "알림 횟수";
            plotAlert.Plot.ShowLegend();
            plotAlert.Refresh();
        }

        private void LoadProcessStats()
        {
            DateTime start = dtpProcessStart.Value.Date;
            DateTime end = dtpProcessEnd.Value.Date;
            string keyword = txtProcessKeyword.Text.Trim();

            dgvFrequentProcess.DataSource = DatabaseHelper.GetFrequentProcessStats(start, end, keyword);
            dgvMemoryProcess.DataSource = DatabaseHelper.GetTopMemoryProcessStats(start, end, keyword);
        }

        private static double[] GetDoubleArray(DataTable dt, string columnName)
        {
            double[] values = new double[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                values[i] = Convert.ToDouble(dt.Rows[i][columnName]);
            }

            return values;
        }

        private static TableLayoutPanel CreateRootLayout(int rowCount)
        {
            TableLayoutPanel root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.ColumnCount = 1;
            root.RowCount = rowCount;
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            return root;
        }

        private static FlowLayoutPanel CreateTopPanel()
        {
            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.Dock = DockStyle.Fill;
            panel.FlowDirection = FlowDirection.LeftToRight;
            panel.Padding = new Padding(10);
            panel.BackColor = Color.WhiteSmoke;
            return panel;
        }

        private static DataGridView CreateGrid()
        {
            DataGridView grid = new DataGridView();
            grid.Dock = DockStyle.Fill;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            return grid;
        }

        private static Button CreateButton(string text)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Width = 110;
            btn.Height = 28;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.White;
            btn.Cursor = Cursors.Hand;
            return btn;
        }

        private static Label CreateTextLabel(string text)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                Padding = new Padding(0, 6, 0, 0)
            };
        }
    }
}