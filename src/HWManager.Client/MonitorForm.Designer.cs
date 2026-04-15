namespace HWManager.Client
{
    partial class MonitorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tableLayoutPanel1 = new TableLayoutPanel();
            formsPlotGPU = new ScottPlot.WinForms.FormsPlot();
            formsPlotRAM = new ScottPlot.WinForms.FormsPlot();
            panel1 = new Panel();
            lblCPU = new Label();
            pbCPU = new ProgressBar();
            panel2 = new Panel();
            lblRAM = new Label();
            pbRAM = new ProgressBar();
            formsPlotCPU = new ScottPlot.WinForms.FormsPlot();
            panel3 = new Panel();
            lblGPU = new Label();
            pbGPU = new ProgressBar();
            tabPage2 = new TabPage();
            dgvHardwareLog = new DataGridView();
            timer1 = new System.Windows.Forms.Timer(components);
            fileSystemWatcher1 = new FileSystemWatcher();
            tableLayoutPanel2 = new TableLayoutPanel();
            btnRefreshLogs = new Button();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHardwareLog).BeginInit();
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(2);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(800, 702);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(tableLayoutPanel1);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Margin = new Padding(2);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(2);
            tabPage1.Size = new Size(792, 674);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "실시간 모니터링";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(formsPlotGPU, 0, 5);
            tableLayoutPanel1.Controls.Add(formsPlotRAM, 0, 3);
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel2, 0, 2);
            tableLayoutPanel1.Controls.Add(formsPlotCPU, 0, 1);
            tableLayoutPanel1.Controls.Add(panel3, 0, 4);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.AddColumns;
            tableLayoutPanel1.Location = new Point(2, 2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(788, 670);
            tableLayoutPanel1.TabIndex = 8;
            // 
            // formsPlotGPU
            // 
            formsPlotGPU.Dock = DockStyle.Fill;
            formsPlotGPU.Location = new Point(3, 509);
            formsPlotGPU.Name = "formsPlotGPU";
            formsPlotGPU.Size = new Size(782, 158);
            formsPlotGPU.TabIndex = 11;
            // 
            // formsPlotRAM
            // 
            formsPlotRAM.Dock = DockStyle.Fill;
            formsPlotRAM.Location = new Point(3, 286);
            formsPlotRAM.Name = "formsPlotRAM";
            formsPlotRAM.Size = new Size(782, 157);
            formsPlotRAM.TabIndex = 10;
            // 
            // panel1
            // 
            panel1.Controls.Add(lblCPU);
            panel1.Controls.Add(pbCPU);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(782, 54);
            panel1.TabIndex = 7;
            // 
            // lblCPU
            // 
            lblCPU.AutoSize = true;
            lblCPU.Location = new Point(3, 6);
            lblCPU.Name = "lblCPU";
            lblCPU.Size = new Size(94, 15);
            lblCPU.TabIndex = 0;
            lblCPU.Text = "CPU 사용량: 0%";
            // 
            // pbCPU
            // 
            pbCPU.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            pbCPU.Location = new Point(3, 21);
            pbCPU.Name = "pbCPU";
            pbCPU.Size = new Size(776, 23);
            pbCPU.Style = ProgressBarStyle.Continuous;
            pbCPU.TabIndex = 4;
            // 
            // panel2
            // 
            panel2.Controls.Add(lblRAM);
            panel2.Controls.Add(pbRAM);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 226);
            panel2.Name = "panel2";
            panel2.Size = new Size(782, 54);
            panel2.TabIndex = 8;
            // 
            // lblRAM
            // 
            lblRAM.AutoSize = true;
            lblRAM.Location = new Point(3, 10);
            lblRAM.Name = "lblRAM";
            lblRAM.Size = new Size(97, 15);
            lblRAM.TabIndex = 1;
            lblRAM.Text = "RAM 사용량: 0%";
            // 
            // pbRAM
            // 
            pbRAM.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            pbRAM.Location = new Point(3, 25);
            pbRAM.Name = "pbRAM";
            pbRAM.Size = new Size(776, 23);
            pbRAM.Style = ProgressBarStyle.Continuous;
            pbRAM.TabIndex = 5;
            // 
            // formsPlotCPU
            // 
            formsPlotCPU.Dock = DockStyle.Fill;
            formsPlotCPU.Location = new Point(3, 63);
            formsPlotCPU.Name = "formsPlotCPU";
            formsPlotCPU.Size = new Size(782, 157);
            formsPlotCPU.TabIndex = 6;
            // 
            // panel3
            // 
            panel3.Controls.Add(lblGPU);
            panel3.Controls.Add(pbGPU);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 449);
            panel3.Name = "panel3";
            panel3.Size = new Size(782, 54);
            panel3.TabIndex = 9;
            // 
            // lblGPU
            // 
            lblGPU.AutoSize = true;
            lblGPU.Location = new Point(3, 9);
            lblGPU.Name = "lblGPU";
            lblGPU.Size = new Size(94, 15);
            lblGPU.TabIndex = 2;
            lblGPU.Text = "GPU 사용량: 0%";
            // 
            // pbGPU
            // 
            pbGPU.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            pbGPU.Location = new Point(3, 24);
            pbGPU.Name = "pbGPU";
            pbGPU.Size = new Size(776, 22);
            pbGPU.Style = ProgressBarStyle.Continuous;
            pbGPU.TabIndex = 3;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(tableLayoutPanel2);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Margin = new Padding(2);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(2);
            tabPage2.Size = new Size(792, 674);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "로그 기록 조회";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvHardwareLog
            // 
            dgvHardwareLog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHardwareLog.Dock = DockStyle.Fill;
            dgvHardwareLog.Location = new Point(2, 31);
            dgvHardwareLog.Margin = new Padding(2);
            dgvHardwareLog.Name = "dgvHardwareLog";
            dgvHardwareLog.RowHeadersWidth = 62;
            dgvHardwareLog.Size = new Size(784, 637);
            dgvHardwareLog.TabIndex = 1;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            // 
            // fileSystemWatcher1
            // 
            fileSystemWatcher1.EnableRaisingEvents = true;
            fileSystemWatcher1.SynchronizingObject = this;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(btnRefreshLogs, 0, 0);
            tableLayoutPanel2.Controls.Add(dgvHardwareLog, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(2, 2);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(788, 670);
            tableLayoutPanel2.TabIndex = 2;
            // 
            // btnRefreshLogs
            // 
            btnRefreshLogs.Anchor = AnchorStyles.Left;
            btnRefreshLogs.AutoSize = true;
            btnRefreshLogs.Location = new Point(2, 2);
            btnRefreshLogs.Margin = new Padding(2);
            btnRefreshLogs.Name = "btnRefreshLogs";
            btnRefreshLogs.Size = new Size(78, 25);
            btnRefreshLogs.TabIndex = 0;
            btnRefreshLogs.Text = "새로고침";
            btnRefreshLogs.UseVisualStyleBackColor = true;
            btnRefreshLogs.Click += btnRefreshHardware_Click;
            // 
            // MonitorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 702);
            Controls.Add(tabControl1);
            Name = "MonitorForm";
            Text = "MonitorForm";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvHardwareLog).EndInit();
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TableLayoutPanel tableLayoutPanel1;
        private ScottPlot.WinForms.FormsPlot formsPlotGPU;
        private ScottPlot.WinForms.FormsPlot formsPlotRAM;
        private Panel panel1;
        private Label lblCPU;
        private ProgressBar pbCPU;
        private Panel panel2;
        private Label lblRAM;
        private ProgressBar pbRAM;
        private ScottPlot.WinForms.FormsPlot formsPlotCPU;
        private Panel panel3;
        private Label lblGPU;
        private ProgressBar pbGPU;
        private TabPage tabPage2;
        private System.Windows.Forms.Timer timer1;
        private FileSystemWatcher fileSystemWatcher1;
        private DataGridView dgvHardwareLog;
        private TableLayoutPanel tableLayoutPanel2;
        private Button btnRefreshLogs;
    }
}