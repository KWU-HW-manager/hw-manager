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
            lblRAM = new Label();
            lblGPU = new Label();
            pbGPU = new ProgressBar();
            pbCPU = new ProgressBar();
            pbRAM = new ProgressBar();
            timer1 = new System.Windows.Forms.Timer(components);
            fileSystemWatcher1 = new FileSystemWatcher();
            formsPlotCPU = new ScottPlot.WinForms.FormsPlot();
            tableLayoutPanel1 = new TableLayoutPanel();
            formsPlotGPU = new ScottPlot.WinForms.FormsPlot();
            formsPlotRAM = new ScottPlot.WinForms.FormsPlot();
            panel1 = new Panel();
            lblCPU = new Label();
            panel2 = new Panel();
            panel3 = new Panel();
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
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
            pbGPU.Location = new Point(3, 27);
            pbGPU.Name = "pbGPU";
            pbGPU.Size = new Size(782, 22);
            pbGPU.Style = ProgressBarStyle.Continuous;
            pbGPU.TabIndex = 3;
            // 
            // pbCPU
            // 
            pbCPU.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            pbCPU.Location = new Point(3, 24);
            pbCPU.Name = "pbCPU";
            pbCPU.Size = new Size(782, 23);
            pbCPU.Style = ProgressBarStyle.Continuous;
            pbCPU.TabIndex = 4;
            // 
            // pbRAM
            // 
            pbRAM.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            pbRAM.Location = new Point(3, 28);
            pbRAM.Name = "pbRAM";
            pbRAM.Size = new Size(782, 23);
            pbRAM.Style = ProgressBarStyle.Continuous;
            pbRAM.TabIndex = 5;
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
            // formsPlotCPU
            // 
            formsPlotCPU.Dock = DockStyle.Fill;
            formsPlotCPU.Location = new Point(3, 63);
            formsPlotCPU.Name = "formsPlotCPU";
            formsPlotCPU.Size = new Size(794, 170);
            formsPlotCPU.TabIndex = 6;
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
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(800, 708);
            tableLayoutPanel1.TabIndex = 7;
            // 
            // formsPlotGPU
            // 
            formsPlotGPU.Dock = DockStyle.Fill;
            formsPlotGPU.Location = new Point(3, 535);
            formsPlotGPU.Name = "formsPlotGPU";
            formsPlotGPU.Size = new Size(794, 170);
            formsPlotGPU.TabIndex = 11;
            // 
            // formsPlotRAM
            // 
            formsPlotRAM.Dock = DockStyle.Fill;
            formsPlotRAM.Location = new Point(3, 299);
            formsPlotRAM.Name = "formsPlotRAM";
            formsPlotRAM.Size = new Size(794, 170);
            formsPlotRAM.TabIndex = 10;
            // 
            // panel1
            // 
            panel1.Controls.Add(lblCPU);
            panel1.Controls.Add(pbCPU);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(794, 54);
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
            // panel2
            // 
            panel2.Controls.Add(lblRAM);
            panel2.Controls.Add(pbRAM);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 239);
            panel2.Name = "panel2";
            panel2.Size = new Size(794, 54);
            panel2.TabIndex = 8;
            // 
            // panel3
            // 
            panel3.Controls.Add(lblGPU);
            panel3.Controls.Add(pbGPU);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 475);
            panel3.Name = "panel3";
            panel3.Size = new Size(794, 54);
            panel3.TabIndex = 9;
            // 
            // MonitorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 708);
            Controls.Add(tableLayoutPanel1);
            Name = "MonitorForm";
            Text = "MonitorForm";
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Label lblRAM;
        private Label lblGPU;
        private ProgressBar pbGPU;
        private ProgressBar pbCPU;
        private ProgressBar pbRAM;
        private System.Windows.Forms.Timer timer1;
        private FileSystemWatcher fileSystemWatcher1;
        private ScottPlot.WinForms.FormsPlot formsPlotCPU;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private Label lblCPU;
        private Panel panel2;
        private Panel panel3;
        private ScottPlot.WinForms.FormsPlot formsPlotRAM;
        private ScottPlot.WinForms.FormsPlot formsPlotGPU;
    }
}