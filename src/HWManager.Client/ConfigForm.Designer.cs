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
            lblAlertRam = new Label();
            lblAlertGpu = new Label();
            nudAlertCpu = new NumericUpDown();
            nudAlertRam = new NumericUpDown();
            nudAlertGpu = new NumericUpDown();
            lblAlertCpuUnit = new Label();
            lblAlertRamUnit = new Label();
            lblAlertGpuUnit = new Label();
            lblAlertInterval = new Label();
            nudAlertInterval = new NumericUpDown();
            lblAlertIntervalUnit = new Label();
            grpOverlay = new GroupBox();
            chkEnableOverlay = new CheckBox();
            tableLayoutPanelButtons = new TableLayoutPanel();
            btnSave = new Button();
            btnCancel = new Button();

            ((System.ComponentModel.ISupportInitialize)nudAlertCpu).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAlertRam).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAlertGpu).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAlertInterval).BeginInit();
            tableLayoutAlert.SuspendLayout();
            grpAlert.SuspendLayout();
            tableLayoutPanelMain.SuspendLayout();
            panelContent.SuspendLayout();
            tableLayoutPanelButtons.SuspendLayout();
            SuspendLayout();

            // panelContent
            panelContent.AutoScroll = true;
            panelContent.BackColor = Color.White;
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(0, 0);
            panelContent.Name = "panelContent";
            panelContent.Padding = new Padding(15);
            panelContent.Size = new Size(700, 500);
            panelContent.TabIndex = 0;
            panelContent.Controls.Add(tableLayoutPanelMain);

            // tableLayoutPanelMain
            tableLayoutPanelMain.AutoSize = true;
            tableLayoutPanelMain.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanelMain.ColumnCount = 1;
            tableLayoutPanelMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanelMain.Controls.Add(grpAlert, 0, 0);
            tableLayoutPanelMain.Controls.Add(grpOverlay, 0, 1);
            tableLayoutPanelMain.Dock = DockStyle.Fill;
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            tableLayoutPanelMain.RowCount = 2;
            tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableLayoutPanelMain.Margin = new Padding(0);

            // grpAlert
            grpAlert.AutoSize = true;
            grpAlert.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            grpAlert.Controls.Add(tableLayoutAlert);
            grpAlert.Dock = DockStyle.Top;
            grpAlert.Font = new Font("¸¼Àº °íµñ", 9F, FontStyle.Bold);
            grpAlert.Location = new Point(0, 0);
            grpAlert.Name = "grpAlert";
            grpAlert.Padding = new Padding(10);
            grpAlert.Size = new Size(670, 280);
            grpAlert.TabIndex = 0;
            grpAlert.TabStop = false;
            grpAlert.Text = "¾Ë¸² ¼³Á¤";
            grpAlert.Margin = new Padding(0, 0, 0, 15);

            // tableLayoutAlert
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
            tableLayoutAlert.Name = "tableLayoutAlert";
            tableLayoutAlert.RowCount = 5;
            tableLayoutAlert.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutAlert.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutAlert.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutAlert.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutAlert.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutAlert.Padding = new Padding(0);
            tableLayoutAlert.Margin = new Padding(0);

            // chkEnableAlert
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

            // lblAlertCpu
            lblAlertCpu.AutoSize = true;
            lblAlertCpu.Dock = DockStyle.Fill;
            lblAlertCpu.Font = new Font("¸¼Àº °íµñ", 9F);
            lblAlertCpu.Location = new Point(3, 33);
            lblAlertCpu.Name = "lblAlertCpu";
            lblAlertCpu.Size = new Size(114, 40);
            lblAlertCpu.TabIndex = 1;
            lblAlertCpu.Text = "CPU ÀÓ°è°ª";
            lblAlertCpu.TextAlign = ContentAlignment.MiddleLeft;

            // nudAlertCpu
            nudAlertCpu.Dock = DockStyle.Fill;
            nudAlertCpu.Font = new Font("¸¼Àº °íµñ", 9F);
            nudAlertCpu.Location = new Point(123, 36);
            nudAlertCpu.Minimum = 10;
            nudAlertCpu.Maximum = 100;
            nudAlertCpu.Increment = 5;
            nudAlertCpu.Name = "nudAlertCpu";
            nudAlertCpu.Size = new Size(74, 23);
            nudAlertCpu.TabIndex = 1;
            nudAlertCpu.TextAlign = HorizontalAlignment.Right;
            nudAlertCpu.Value = 90;

            // lblAlertCpuUnit
            lblAlertCpuUnit.AutoSize = true;
            lblAlertCpuUnit.Dock = DockStyle.Fill;
            lblAlertCpuUnit.Font = new Font("¸¼Àº °íµñ", 9F);
            lblAlertCpuUnit.Location = new Point(203, 33);
            lblAlertCpuUnit.Name = "lblAlertCpuUnit";
            lblAlertCpuUnit.Size = new Size(34, 40);
            lblAlertCpuUnit.TabIndex = 2;
            lblAlertCpuUnit.Text = "%";
            lblAlertCpuUnit.TextAlign = ContentAlignment.MiddleLeft;

            // lblAlertRam
            lblAlertRam.AutoSize = true;
            lblAlertRam.Dock = DockStyle.Fill;
            lblAlertRam.Font = new Font("¸¼Àº °íµñ", 9F);
            lblAlertRam.Location = new Point(3, 73);
            lblAlertRam.Name = "lblAlertRam";
            lblAlertRam.Size = new Size(114, 40);
            lblAlertRam.TabIndex = 3;
            lblAlertRam.Text = "RAM ÀÓ°è°ª";
            lblAlertRam.TextAlign = ContentAlignment.MiddleLeft;

            // nudAlertRam
            nudAlertRam.Dock = DockStyle.Fill;
            nudAlertRam.Font = new Font("¸¼Àº °íµñ", 9F);
            nudAlertRam.Location = new Point(123, 76);
            nudAlertRam.Minimum = 10;
            nudAlertRam.Maximum = 100;
            nudAlertRam.Increment = 5;
            nudAlertRam.Name = "nudAlertRam";
            nudAlertRam.Size = new Size(74, 23);
            nudAlertRam.TabIndex = 2;
            nudAlertRam.TextAlign = HorizontalAlignment.Right;
            nudAlertRam.Value = 90;

            // lblAlertRamUnit
            lblAlertRamUnit.AutoSize = true;
            lblAlertRamUnit.Dock = DockStyle.Fill;
            lblAlertRamUnit.Font = new Font("¸¼Àº °íµñ", 9F);
            lblAlertRamUnit.Location = new Point(203, 73);
            lblAlertRamUnit.Name = "lblAlertRamUnit";
            lblAlertRamUnit.Size = new Size(34, 40);
            lblAlertRamUnit.TabIndex = 4;
            lblAlertRamUnit.Text = "%";
            lblAlertRamUnit.TextAlign = ContentAlignment.MiddleLeft;

            // lblAlertGpu
            lblAlertGpu.AutoSize = true;
            lblAlertGpu.Dock = DockStyle.Fill;
            lblAlertGpu.Font = new Font("¸¼Àº °íµñ", 9F);
            lblAlertGpu.Location = new Point(3, 113);
            lblAlertGpu.Name = "lblAlertGpu";
            lblAlertGpu.Size = new Size(114, 40);
            lblAlertGpu.TabIndex = 5;
            lblAlertGpu.Text = "GPU ÀÓ°è°ª";
            lblAlertGpu.TextAlign = ContentAlignment.MiddleLeft;

            // nudAlertGpu
            nudAlertGpu.Dock = DockStyle.Fill;
            nudAlertGpu.Font = new Font("¸¼Àº °íµñ", 9F);
            nudAlertGpu.Location = new Point(123, 116);
            nudAlertGpu.Minimum = 10;
            nudAlertGpu.Maximum = 100;
            nudAlertGpu.Increment = 5;
            nudAlertGpu.Name = "nudAlertGpu";
            nudAlertGpu.Size = new Size(74, 23);
            nudAlertGpu.TabIndex = 3;
            nudAlertGpu.TextAlign = HorizontalAlignment.Right;
            nudAlertGpu.Value = 90;

            // lblAlertGpuUnit
            lblAlertGpuUnit.AutoSize = true;
            lblAlertGpuUnit.Dock = DockStyle.Fill;
            lblAlertGpuUnit.Font = new Font("¸¼Àº °íµñ", 9F);
            lblAlertGpuUnit.Location = new Point(203, 113);
            lblAlertGpuUnit.Name = "lblAlertGpuUnit";
            lblAlertGpuUnit.Size = new Size(34, 40);
            lblAlertGpuUnit.TabIndex = 6;
            lblAlertGpuUnit.Text = "%";
            lblAlertGpuUnit.TextAlign = ContentAlignment.MiddleLeft;

            // lblAlertInterval
            lblAlertInterval.AutoSize = true;
            lblAlertInterval.Dock = DockStyle.Fill;
            lblAlertInterval.Font = new Font("¸¼Àº °íµñ", 9F);
            lblAlertInterval.Location = new Point(3, 153);
            lblAlertInterval.Name = "lblAlertInterval";
            lblAlertInterval.Size = new Size(114, 40);
            lblAlertInterval.TabIndex = 7;
            lblAlertInterval.Text = "¾Ë¸² °£°Ý";
            lblAlertInterval.TextAlign = ContentAlignment.MiddleLeft;

            // nudAlertInterval
            nudAlertInterval.Dock = DockStyle.Fill;
            nudAlertInterval.Font = new Font("¸¼Àº °íµñ", 9F);
            nudAlertInterval.Location = new Point(123, 156);
            nudAlertInterval.Minimum = 10;
            nudAlertInterval.Maximum = 300;
            nudAlertInterval.Increment = 10;
            nudAlertInterval.Name = "nudAlertInterval";
            nudAlertInterval.Size = new Size(74, 23);
            nudAlertInterval.TabIndex = 4;
            nudAlertInterval.TextAlign = HorizontalAlignment.Right;
            nudAlertInterval.Value = 60;

            // lblAlertIntervalUnit
            lblAlertIntervalUnit.AutoSize = true;
            lblAlertIntervalUnit.Dock = DockStyle.Fill;
            lblAlertIntervalUnit.Font = new Font("¸¼Àº °íµñ", 9F);
            lblAlertIntervalUnit.Location = new Point(203, 153);
            lblAlertIntervalUnit.Name = "lblAlertIntervalUnit";
            lblAlertIntervalUnit.Size = new Size(34, 40);
            lblAlertIntervalUnit.TabIndex = 8;
            lblAlertIntervalUnit.Text = "ÃÊ";
            lblAlertIntervalUnit.TextAlign = ContentAlignment.MiddleLeft;

            // grpOverlay
            grpOverlay.AutoSize = true;
            grpOverlay.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            grpOverlay.Controls.Add(chkEnableOverlay);
            grpOverlay.Dock = DockStyle.Top;
            grpOverlay.Font = new Font("¸¼Àº °íµñ", 9F, FontStyle.Bold);
            grpOverlay.Location = new Point(0, 295);
            grpOverlay.Name = "grpOverlay";
            grpOverlay.Padding = new Padding(10);
            grpOverlay.Size = new Size(670, 60);
            grpOverlay.TabIndex = 1;
            grpOverlay.TabStop = false;
            grpOverlay.Text = "¿À¹ö·¹ÀÌ";
            grpOverlay.Margin = new Padding(0, 0, 0, 15);

            // chkEnableOverlay
            chkEnableOverlay.AutoSize = true;
            chkEnableOverlay.Checked = false;
            chkEnableOverlay.Dock = DockStyle.Fill;
            chkEnableOverlay.Font = new Font("¸¼Àº °íµñ", 9F);
            chkEnableOverlay.Location = new Point(10, 22);
            chkEnableOverlay.Name = "chkEnableOverlay";
            chkEnableOverlay.Size = new Size(650, 28);
            chkEnableOverlay.TabIndex = 5;
            chkEnableOverlay.Text = "¿À¹ö·¹ÀÌ ±â´É È°¼ºÈ­";
            chkEnableOverlay.UseVisualStyleBackColor = true;

            // tableLayoutPanelButtons
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

            // btnSave
            btnSave.BackColor = Color.FromArgb(79, 129, 189);
            btnSave.Dock = DockStyle.Fill;
            btnSave.Font = new Font("¸¼Àº °íµñ", 9F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnSave.ForeColor = Color.White;
            btnSave.Margin = new Padding(5);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(340, 40);
            btnSave.TabIndex = 6;
            btnSave.Text = "ÀúÀå";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;

            // btnCancel
            btnCancel.BackColor = Color.LightGray;
            btnCancel.Dock = DockStyle.Fill;
            btnCancel.Font = new Font("¸¼Àº °íµñ", 9F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnCancel.Margin = new Padding(5);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(340, 40);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Ãë¼Ò";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;

            // ConfigForm
            AutoScaleDimensions = new SizeF(9F, 20F);
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

            ((System.ComponentModel.ISupportInitialize)nudAlertCpu).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAlertRam).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAlertGpu).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAlertInterval).EndInit();
            tableLayoutAlert.ResumeLayout(false);
            grpAlert.ResumeLayout(false);
            tableLayoutPanelMain.ResumeLayout(false);
            panelContent.ResumeLayout(false);
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
    }
}