namespace HWManager.Client
{
    partial class ConfigForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            panelContent = new Panel();
            tableLayoutPanelMain = new TableLayoutPanel();
            grpAlert = new GroupBox();
            tableLayoutAlert = new TableLayoutPanel();
            chkEnableAlert = new CheckBox();
            lblAlertCpu = new Label();
            nudAlertCpu = new NumericUpDown();
            lblAlertCpuUnit = new Label();
            lblAlertRam = new Label();
            nudAlertRam = new NumericUpDown();
            lblAlertRamUnit = new Label();
            lblAlertGpu = new Label();
            nudAlertGpu = new NumericUpDown();
            lblAlertGpuUnit = new Label();
            lblAlertInterval = new Label();
            nudAlertInterval = new NumericUpDown();
            lblAlertIntervalUnit = new Label();
            grpOverlay = new GroupBox();
            tableLayoutOverlay = new TableLayoutPanel();
            tbScale = new TrackBar();
            chkEnableOverlay = new CheckBox();
            tbOpacity = new TrackBar();
            lblOpacity = new Label();
            lblScale = new Label();
            tableLayoutPanelButtons = new TableLayoutPanel();
            btnSave = new Button();
            btnCancel = new Button();
            panelContent.SuspendLayout();
            tableLayoutPanelMain.SuspendLayout();
            grpAlert.SuspendLayout();
            tableLayoutAlert.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudAlertCpu).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAlertRam).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAlertGpu).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAlertInterval).BeginInit();
            grpOverlay.SuspendLayout();
            tableLayoutOverlay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tbScale).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbOpacity).BeginInit();
            tableLayoutPanelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // panelContent
            // 
            panelContent.AutoScroll = true;
            panelContent.BackColor = Color.White;
            panelContent.Controls.Add(tableLayoutPanelMain);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(0, 0);
            panelContent.Name = "panelContent";
            panelContent.Padding = new Padding(15);
            panelContent.Size = new Size(700, 500);
            panelContent.TabIndex = 0;
            // 
            // tableLayoutPanelMain
            // 
            tableLayoutPanelMain.AutoSize = true;
            tableLayoutPanelMain.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanelMain.ColumnCount = 1;
            tableLayoutPanelMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanelMain.Controls.Add(grpAlert, 0, 0);
            tableLayoutPanelMain.Controls.Add(grpOverlay, 0, 1);
            tableLayoutPanelMain.Dock = DockStyle.Fill;
            tableLayoutPanelMain.Location = new Point(15, 15);
            tableLayoutPanelMain.Margin = new Padding(0);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            tableLayoutPanelMain.RowCount = 3;
            tableLayoutPanelMain.RowStyles.Add(new RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanelMain.Size = new Size(670, 470);
            tableLayoutPanelMain.TabIndex = 0;
            // 
            // grpAlert
            // 
            grpAlert.AutoSize = true;
            grpAlert.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            grpAlert.Controls.Add(tableLayoutAlert);
            grpAlert.Dock = DockStyle.Top;
            grpAlert.Font = new Font("¸¼Àº °íµñ", 9F, FontStyle.Bold);
            grpAlert.Location = new Point(0, 0);
            grpAlert.Margin = new Padding(0, 0, 0, 15);
            grpAlert.Name = "grpAlert";
            grpAlert.Padding = new Padding(10);
            grpAlert.Size = new Size(670, 226);
            grpAlert.TabIndex = 0;
            grpAlert.TabStop = false;
            grpAlert.Text = "¾Ë¸² ¼³Á¤";
            // 
            // tableLayoutAlert
            // 
            tableLayoutAlert.AutoSize = true;
            tableLayoutAlert.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutAlert.ColumnCount = 4;
            tableLayoutAlert.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tableLayoutAlert.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tableLayoutAlert.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            tableLayoutAlert.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutAlert.Controls.Add(chkEnableAlert, 0, 0);
            tableLayoutAlert.Controls.Add(lblAlertCpu, 0, 1);
            tableLayoutAlert.Controls.Add(nudAlertCpu, 1, 1);
            tableLayoutAlert.Controls.Add(lblAlertCpuUnit, 2, 1);
            tableLayoutAlert.Controls.Add(lblAlertRam, 0, 2);
            tableLayoutAlert.Controls.Add(nudAlertRam, 1, 2);
            tableLayoutAlert.Controls.Add(lblAlertRamUnit, 2, 2);
            tableLayoutAlert.Controls.Add(lblAlertGpu, 0, 3);
            tableLayoutAlert.Controls.Add(nudAlertGpu, 1, 3);
            tableLayoutAlert.Controls.Add(lblAlertGpuUnit, 2, 3);
            tableLayoutAlert.Controls.Add(lblAlertInterval, 0, 4);
            tableLayoutAlert.Controls.Add(nudAlertInterval, 1, 4);
            tableLayoutAlert.Controls.Add(lblAlertIntervalUnit, 2, 4);
            tableLayoutAlert.Dock = DockStyle.Fill;
            tableLayoutAlert.Location = new Point(10, 26);
            tableLayoutAlert.Margin = new Padding(0);
            tableLayoutAlert.Name = "tableLayoutAlert";
            tableLayoutAlert.RowCount = 5;
            tableLayoutAlert.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutAlert.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutAlert.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutAlert.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutAlert.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutAlert.Size = new Size(650, 190);
            tableLayoutAlert.TabIndex = 0;
            // 
            // chkEnableAlert
            // 
            chkEnableAlert.AutoSize = true;
            chkEnableAlert.Checked = true;
            chkEnableAlert.CheckState = CheckState.Checked;
            chkEnableAlert.Dock = DockStyle.Fill;
            chkEnableAlert.Font = new Font("¸¼Àº °íµñ", 9F, FontStyle.Bold);
            chkEnableAlert.Location = new Point(3, 3);
            chkEnableAlert.Name = "chkEnableAlert";
            chkEnableAlert.Size = new Size(114, 24);
            chkEnableAlert.TabIndex = 0;
            chkEnableAlert.Text = "¾Ë¸² ±â´É È°¼ºÈ­";
            chkEnableAlert.UseVisualStyleBackColor = true;
            chkEnableAlert.CheckedChanged += chkEnableAlert_CheckedChanged;
            // 
            // lblAlertCpu
            // 
            lblAlertCpu.AutoSize = true;
            lblAlertCpu.Dock = DockStyle.Fill;
            lblAlertCpu.Font = new Font("¸¼Àº °íµñ", 9F);
            lblAlertCpu.Location = new Point(3, 30);
            lblAlertCpu.Name = "lblAlertCpu";
            lblAlertCpu.Size = new Size(114, 40);
            lblAlertCpu.TabIndex = 1;
            lblAlertCpu.Text = "CPU ÀÓ°è°ª";
            lblAlertCpu.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudAlertCpu
            // 
            nudAlertCpu.Dock = DockStyle.Fill;
            nudAlertCpu.Font = new Font("¸¼Àº °íµñ", 9F);
            nudAlertCpu.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            nudAlertCpu.Location = new Point(123, 33);
            nudAlertCpu.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            nudAlertCpu.Name = "nudAlertCpu";
            nudAlertCpu.Size = new Size(74, 23);
            nudAlertCpu.TabIndex = 1;
            nudAlertCpu.TextAlign = HorizontalAlignment.Right;
            nudAlertCpu.Value = new decimal(new int[] { 90, 0, 0, 0 });
            // 
            // lblAlertCpuUnit
            // 
            lblAlertCpuUnit.AutoSize = true;
            lblAlertCpuUnit.Dock = DockStyle.Fill;
            lblAlertCpuUnit.Font = new Font("¸¼Àº °íµñ", 9F);
            lblAlertCpuUnit.Location = new Point(203, 30);
            lblAlertCpuUnit.Name = "lblAlertCpuUnit";
            lblAlertCpuUnit.Size = new Size(34, 40);
            lblAlertCpuUnit.TabIndex = 2;
            lblAlertCpuUnit.Text = "%";
            lblAlertCpuUnit.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblAlertRam
            // 
            lblAlertRam.AutoSize = true;
            lblAlertRam.Dock = DockStyle.Fill;
            lblAlertRam.Font = new Font("¸¼Àº °íµñ", 9F);
            lblAlertRam.Location = new Point(3, 70);
            lblAlertRam.Name = "lblAlertRam";
            lblAlertRam.Size = new Size(114, 40);
            lblAlertRam.TabIndex = 3;
            lblAlertRam.Text = "RAM ÀÓ°è°ª";
            lblAlertRam.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudAlertRam
            // 
            nudAlertRam.Dock = DockStyle.Fill;
            nudAlertRam.Font = new Font("¸¼Àº °íµñ", 9F);
            nudAlertRam.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            nudAlertRam.Location = new Point(123, 73);
            nudAlertRam.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            nudAlertRam.Name = "nudAlertRam";
            nudAlertRam.Size = new Size(74, 23);
            nudAlertRam.TabIndex = 2;
            nudAlertRam.TextAlign = HorizontalAlignment.Right;
            nudAlertRam.Value = new decimal(new int[] { 90, 0, 0, 0 });
            // 
            // lblAlertRamUnit
            // 
            lblAlertRamUnit.AutoSize = true;
            lblAlertRamUnit.Dock = DockStyle.Fill;
            lblAlertRamUnit.Font = new Font("¸¼Àº °íµñ", 9F);
            lblAlertRamUnit.Location = new Point(203, 70);
            lblAlertRamUnit.Name = "lblAlertRamUnit";
            lblAlertRamUnit.Size = new Size(34, 40);
            lblAlertRamUnit.TabIndex = 4;
            lblAlertRamUnit.Text = "%";
            lblAlertRamUnit.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblAlertGpu
            // 
            lblAlertGpu.AutoSize = true;
            lblAlertGpu.Dock = DockStyle.Fill;
            lblAlertGpu.Font = new Font("¸¼Àº °íµñ", 9F);
            lblAlertGpu.Location = new Point(3, 110);
            lblAlertGpu.Name = "lblAlertGpu";
            lblAlertGpu.Size = new Size(114, 40);
            lblAlertGpu.TabIndex = 5;
            lblAlertGpu.Text = "GPU ÀÓ°è°ª";
            lblAlertGpu.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudAlertGpu
            // 
            nudAlertGpu.Dock = DockStyle.Fill;
            nudAlertGpu.Font = new Font("¸¼Àº °íµñ", 9F);
            nudAlertGpu.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            nudAlertGpu.Location = new Point(123, 113);
            nudAlertGpu.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            nudAlertGpu.Name = "nudAlertGpu";
            nudAlertGpu.Size = new Size(74, 23);
            nudAlertGpu.TabIndex = 3;
            nudAlertGpu.TextAlign = HorizontalAlignment.Right;
            nudAlertGpu.Value = new decimal(new int[] { 90, 0, 0, 0 });
            // 
            // lblAlertGpuUnit
            // 
            lblAlertGpuUnit.AutoSize = true;
            lblAlertGpuUnit.Dock = DockStyle.Fill;
            lblAlertGpuUnit.Font = new Font("¸¼Àº °íµñ", 9F);
            lblAlertGpuUnit.Location = new Point(203, 110);
            lblAlertGpuUnit.Name = "lblAlertGpuUnit";
            lblAlertGpuUnit.Size = new Size(34, 40);
            lblAlertGpuUnit.TabIndex = 6;
            lblAlertGpuUnit.Text = "%";
            lblAlertGpuUnit.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblAlertInterval
            // 
            lblAlertInterval.AutoSize = true;
            lblAlertInterval.Dock = DockStyle.Fill;
            lblAlertInterval.Font = new Font("¸¼Àº °íµñ", 9F);
            lblAlertInterval.Location = new Point(3, 150);
            lblAlertInterval.Name = "lblAlertInterval";
            lblAlertInterval.Size = new Size(114, 40);
            lblAlertInterval.TabIndex = 7;
            lblAlertInterval.Text = "¾Ë¸² °£°Ý";
            lblAlertInterval.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudAlertInterval
            // 
            nudAlertInterval.Dock = DockStyle.Fill;
            nudAlertInterval.Font = new Font("¸¼Àº °íµñ", 9F);
            nudAlertInterval.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            nudAlertInterval.Location = new Point(123, 153);
            nudAlertInterval.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            nudAlertInterval.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            nudAlertInterval.Name = "nudAlertInterval";
            nudAlertInterval.Size = new Size(74, 23);
            nudAlertInterval.TabIndex = 4;
            nudAlertInterval.TextAlign = HorizontalAlignment.Right;
            nudAlertInterval.Value = new decimal(new int[] { 60, 0, 0, 0 });
            // 
            // lblAlertIntervalUnit
            // 
            lblAlertIntervalUnit.AutoSize = true;
            lblAlertIntervalUnit.Dock = DockStyle.Fill;
            lblAlertIntervalUnit.Font = new Font("¸¼Àº °íµñ", 9F);
            lblAlertIntervalUnit.Location = new Point(203, 150);
            lblAlertIntervalUnit.Name = "lblAlertIntervalUnit";
            lblAlertIntervalUnit.Size = new Size(34, 40);
            lblAlertIntervalUnit.TabIndex = 8;
            lblAlertIntervalUnit.Text = "ÃÊ";
            lblAlertIntervalUnit.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // grpOverlay
            // 
            grpOverlay.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            grpOverlay.Controls.Add(tableLayoutOverlay);
            grpOverlay.Dock = DockStyle.Top;
            grpOverlay.Font = new Font("¸¼Àº °íµñ", 9F, FontStyle.Bold);
            grpOverlay.Location = new Point(0, 241);
            grpOverlay.Margin = new Padding(0, 0, 0, 15);
            grpOverlay.Name = "grpOverlay";
            grpOverlay.Padding = new Padding(10);
            grpOverlay.Size = new Size(670, 135);
            grpOverlay.TabIndex = 1;
            grpOverlay.TabStop = false;
            grpOverlay.Text = "¿À¹ö·¹ÀÌ";
            // 
            // tableLayoutOverlay
            // 
            tableLayoutOverlay.ColumnCount = 2;
            tableLayoutOverlay.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.615385F));
            tableLayoutOverlay.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 91.38461F));
            tableLayoutOverlay.Controls.Add(tbScale, 1, 2);
            tableLayoutOverlay.Controls.Add(chkEnableOverlay, 0, 0);
            tableLayoutOverlay.Controls.Add(tbOpacity, 1, 1);
            tableLayoutOverlay.Controls.Add(lblOpacity, 0, 1);
            tableLayoutOverlay.Controls.Add(lblScale, 0, 2);
            tableLayoutOverlay.Dock = DockStyle.Fill;
            tableLayoutOverlay.Location = new Point(10, 26);
            tableLayoutOverlay.Name = "tableLayoutOverlay";
            tableLayoutOverlay.RowCount = 3;
            tableLayoutOverlay.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutOverlay.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutOverlay.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutOverlay.Size = new Size(650, 99);
            tableLayoutOverlay.TabIndex = 2;
            // 
            // tbScale
            // 
            tbScale.Dock = DockStyle.Fill;
            tbScale.Location = new Point(59, 67);
            tbScale.Maximum = 15;
            tbScale.Minimum = 5;
            tbScale.Name = "tbScale";
            tbScale.Size = new Size(588, 29);
            tbScale.TabIndex = 9;
            tbScale.Value = 10;
            tbScale.Scroll += tbScale_Scroll;
            // 
            // chkEnableOverlay
            // 
            chkEnableOverlay.AutoSize = true;
            tableLayoutOverlay.SetColumnSpan(chkEnableOverlay, 2);
            chkEnableOverlay.Dock = DockStyle.Left;
            chkEnableOverlay.Font = new Font("¸¼Àº °íµñ", 9F);
            chkEnableOverlay.Location = new Point(3, 3);
            chkEnableOverlay.Name = "chkEnableOverlay";
            chkEnableOverlay.Size = new Size(142, 26);
            chkEnableOverlay.TabIndex = 5;
            chkEnableOverlay.Text = "¿À¹ö·¹ÀÌ ±â´É È°¼ºÈ­";
            chkEnableOverlay.UseVisualStyleBackColor = true;
            chkEnableOverlay.CheckedChanged += chkEnableOverlay_CheckedChanged;
            // 
            // tbOpacity
            // 
            tbOpacity.Dock = DockStyle.Fill;
            tbOpacity.Location = new Point(59, 35);
            tbOpacity.Minimum = 2;
            tbOpacity.Name = "tbOpacity";
            tbOpacity.Size = new Size(588, 26);
            tbOpacity.TabIndex = 7;
            tbOpacity.Value = 8;
            tbOpacity.Scroll += tbOpacity_Scroll;
            // 
            // lblOpacity
            // 
            lblOpacity.AutoSize = true;
            lblOpacity.Dock = DockStyle.Fill;
            lblOpacity.Location = new Point(3, 32);
            lblOpacity.Name = "lblOpacity";
            lblOpacity.Size = new Size(50, 32);
            lblOpacity.TabIndex = 6;
            lblOpacity.Text = "Åõ¸íµµ";
            lblOpacity.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblScale
            // 
            lblScale.AutoSize = true;
            lblScale.Dock = DockStyle.Fill;
            lblScale.Location = new Point(3, 64);
            lblScale.Name = "lblScale";
            lblScale.Size = new Size(50, 35);
            lblScale.TabIndex = 8;
            lblScale.Text = "Å©±â";
            lblScale.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanelButtons
            // 
            tableLayoutPanelButtons.BackColor = Color.WhiteSmoke;
            tableLayoutPanelButtons.ColumnCount = 2;
            tableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanelButtons.Controls.Add(btnSave, 0, 0);
            tableLayoutPanelButtons.Controls.Add(btnCancel, 1, 0);
            tableLayoutPanelButtons.Dock = DockStyle.Bottom;
            tableLayoutPanelButtons.Location = new Point(0, 500);
            tableLayoutPanelButtons.Name = "tableLayoutPanelButtons";
            tableLayoutPanelButtons.Padding = new Padding(10);
            tableLayoutPanelButtons.RowCount = 1;
            tableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanelButtons.Size = new Size(700, 60);
            tableLayoutPanelButtons.TabIndex = 1;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(79, 129, 189);
            btnSave.Dock = DockStyle.Fill;
            btnSave.Font = new Font("¸¼Àº °íµñ", 9F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(15, 15);
            btnSave.Margin = new Padding(5);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(330, 30);
            btnSave.TabIndex = 6;
            btnSave.Text = "ÀúÀå";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.LightGray;
            btnCancel.Dock = DockStyle.Fill;
            btnCancel.Font = new Font("¸¼Àº °íµñ", 9F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnCancel.Location = new Point(355, 15);
            btnCancel.Margin = new Padding(5);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(330, 30);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Ãë¼Ò";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // ConfigForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(700, 560);
            Controls.Add(panelContent);
            Controls.Add(tableLayoutPanelButtons);
            Font = new Font("¸¼Àº °íµñ", 9F, FontStyle.Regular, GraphicsUnit.Point, 129);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ConfigForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "¼³Á¤";
            Load += ConfigForm_Load;
            panelContent.ResumeLayout(false);
            panelContent.PerformLayout();
            tableLayoutPanelMain.ResumeLayout(false);
            tableLayoutPanelMain.PerformLayout();
            grpAlert.ResumeLayout(false);
            grpAlert.PerformLayout();
            tableLayoutAlert.ResumeLayout(false);
            tableLayoutAlert.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudAlertCpu).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAlertRam).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAlertGpu).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAlertInterval).EndInit();
            grpOverlay.ResumeLayout(false);
            tableLayoutOverlay.ResumeLayout(false);
            tableLayoutOverlay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)tbScale).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbOpacity).EndInit();
            tableLayoutPanelButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button btnSave;
        private Button btnCancel;
        private Panel panelContent;
        private TableLayoutPanel tableLayoutPanelButtons;
        private TableLayoutPanel tableLayoutPanelMain;
        private GroupBox grpAlert;
        private TableLayoutPanel tableLayoutAlert;
        private CheckBox chkEnableAlert;
        private Label lblAlertCpu;
        private NumericUpDown nudAlertCpu;
        private Label lblAlertCpuUnit;
        private Label lblAlertRam;
        private NumericUpDown nudAlertRam;
        private Label lblAlertRamUnit;
        private Label lblAlertGpu;
        private NumericUpDown nudAlertGpu;
        private Label lblAlertGpuUnit;
        private Label lblAlertInterval;
        private NumericUpDown nudAlertInterval;
        private Label lblAlertIntervalUnit;
        private GroupBox grpOverlay;
        private CheckBox chkEnableOverlay;
        private TrackBar tbOpacity;
        private Label lblOpacity;
        private TableLayoutPanel tableLayoutOverlay;
        private TrackBar tbScale;
        private Label lblScale;
    }
}