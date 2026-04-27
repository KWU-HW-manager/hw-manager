using System.Drawing;
using System.Windows.Forms;

namespace HWManager.Client
{
    partial class FocusModeForm
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

        // 디자이너로 열어도 깨지지 않도록, 절대 픽셀 대신 퍼센트/AutoSize 위주로 구성.
        // AutoScaleDimensions = (7F, 15F) 고정 → 어떤 DPI에서도 Font 기반으로 자동 스케일.
        private void InitializeComponent()
        {
            rootLayout = new TableLayoutPanel();
            settingsLayout = new TableLayoutPanel();
            grpThreshold = new GroupBox();
            thresholdLayout = new TableLayoutPanel();
            lblCpu = new Label();
            nudCpu = new NumericUpDown();
            lblCpuUnit = new Label();
            lblRam = new Label();
            nudRam = new NumericUpDown();
            lblRamUnit = new Label();
            lblGpu = new Label();
            nudGpu = new NumericUpDown();
            lblGpuUnit = new Label();
            lblThresholdSummary = new Label();
            grpKill = new GroupBox();
            killLayout = new TableLayoutPanel();
            lstKill = new ListBox();
            killBottomLayout = new TableLayoutPanel();
            txtKillInput = new TextBox();
            btnAddKill = new Button();
            btnRemoveKill = new Button();
            btnPickFromProcesses = new Button();
            grpTrigger = new GroupBox();
            triggerLayout = new TableLayoutPanel();
            lstTrigger = new ListBox();
            triggerBottomLayout = new TableLayoutPanel();
            txtTriggerInput = new TextBox();
            btnAddTrigger = new Button();
            btnRemoveTrigger = new Button();
            controlLayout = new TableLayoutPanel();
            chkEnable = new CheckBox();
            lblState = new Label();
            lblCurrentStatus = new Label();
            txtLog = new TextBox();
            sideLayout = new TableLayoutPanel();
            grpRecommend = new GroupBox();
            recommendLayout = new TableLayoutPanel();
            lvRecommend = new ListView();
            colRecName = new ColumnHeader();
            colRecMem = new ColumnHeader();
            recommendButtonLayout = new TableLayoutPanel();
            btnRefreshRecommend = new Button();
            btnAddRecommend = new Button();
            rtbManual = new RichTextBox();
            rootLayout.SuspendLayout();
            settingsLayout.SuspendLayout();
            grpThreshold.SuspendLayout();
            thresholdLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudCpu).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRam).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudGpu).BeginInit();
            grpKill.SuspendLayout();
            killLayout.SuspendLayout();
            killBottomLayout.SuspendLayout();
            grpTrigger.SuspendLayout();
            triggerLayout.SuspendLayout();
            triggerBottomLayout.SuspendLayout();
            controlLayout.SuspendLayout();
            sideLayout.SuspendLayout();
            grpRecommend.SuspendLayout();
            recommendLayout.SuspendLayout();
            recommendButtonLayout.SuspendLayout();
            SuspendLayout();
            // 
            // rootLayout
            // 
            rootLayout.ColumnCount = 2;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            rootLayout.Controls.Add(settingsLayout, 0, 0);
            rootLayout.Controls.Add(sideLayout, 1, 0);
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.Location = new Point(0, 0);
            rootLayout.Name = "rootLayout";
            rootLayout.Padding = new Padding(10);
            rootLayout.RowCount = 1;
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rootLayout.Size = new Size(1400, 900);
            rootLayout.TabIndex = 0;
            // 
            // settingsLayout
            // 
            settingsLayout.ColumnCount = 1;
            settingsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            settingsLayout.Controls.Add(grpThreshold, 0, 0);
            settingsLayout.Controls.Add(grpKill, 0, 1);
            settingsLayout.Controls.Add(grpTrigger, 0, 2);
            settingsLayout.Controls.Add(controlLayout, 0, 3);
            settingsLayout.Controls.Add(txtLog, 0, 4);
            settingsLayout.Dock = DockStyle.Fill;
            settingsLayout.Location = new Point(10, 10);
            settingsLayout.Margin = new Padding(0, 0, 10, 0);
            settingsLayout.Name = "settingsLayout";
            settingsLayout.RowCount = 5;
            settingsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 22F));
            settingsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 28F));
            settingsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 22F));
            settingsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            settingsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 18F));
            settingsLayout.Size = new Size(818, 880);
            settingsLayout.TabIndex = 0;
            // 
            // grpThreshold
            // 
            grpThreshold.Controls.Add(thresholdLayout);
            grpThreshold.Dock = DockStyle.Fill;
            grpThreshold.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            grpThreshold.Location = new Point(3, 3);
            grpThreshold.Margin = new Padding(3, 3, 3, 6);
            grpThreshold.Name = "grpThreshold";
            grpThreshold.Size = new Size(812, 184);
            grpThreshold.TabIndex = 0;
            grpThreshold.TabStop = false;
            grpThreshold.Text = "자원 임계값 설정";
            // 
            // thresholdLayout
            // 
            thresholdLayout.ColumnCount = 3;
            thresholdLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            thresholdLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            thresholdLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            thresholdLayout.Controls.Add(lblCpu, 0, 0);
            thresholdLayout.Controls.Add(nudCpu, 1, 0);
            thresholdLayout.Controls.Add(lblCpuUnit, 2, 0);
            thresholdLayout.Controls.Add(lblRam, 0, 1);
            thresholdLayout.Controls.Add(nudRam, 1, 1);
            thresholdLayout.Controls.Add(lblRamUnit, 2, 1);
            thresholdLayout.Controls.Add(lblGpu, 0, 2);
            thresholdLayout.Controls.Add(nudGpu, 1, 2);
            thresholdLayout.Controls.Add(lblGpuUnit, 2, 2);
            thresholdLayout.Controls.Add(lblThresholdSummary, 0, 3);
            thresholdLayout.Dock = DockStyle.Fill;
            thresholdLayout.Location = new Point(3, 37);
            thresholdLayout.Name = "thresholdLayout";
            thresholdLayout.Padding = new Padding(10, 6, 10, 6);
            thresholdLayout.RowCount = 4;
            thresholdLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            thresholdLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            thresholdLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            thresholdLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            thresholdLayout.Size = new Size(806, 144);
            thresholdLayout.TabIndex = 0;
            // 
            // lblCpu
            // 
            lblCpu.Location = new Point(13, 6);
            lblCpu.Name = "lblCpu";
            lblCpu.Size = new Size(100, 23);
            lblCpu.TabIndex = 0;
            // 
            // nudCpu
            // 
            nudCpu.Location = new Point(154, 9);
            nudCpu.Name = "nudCpu";
            nudCpu.Size = new Size(120, 41);
            nudCpu.TabIndex = 1;
            nudCpu.Value = new decimal(new int[] { 80, 0, 0, 0 });
            nudCpu.ValueChanged += nudCpu_ValueChanged;
            // 
            // lblCpuUnit
            // 
            lblCpuUnit.Location = new Point(405, 6);
            lblCpuUnit.Name = "lblCpuUnit";
            lblCpuUnit.Size = new Size(100, 23);
            lblCpuUnit.TabIndex = 2;
            // 
            // lblRam
            // 
            lblRam.Location = new Point(13, 39);
            lblRam.Name = "lblRam";
            lblRam.Size = new Size(100, 23);
            lblRam.TabIndex = 3;
            // 
            // nudRam
            // 
            nudRam.Location = new Point(154, 42);
            nudRam.Name = "nudRam";
            nudRam.Size = new Size(120, 41);
            nudRam.TabIndex = 4;
            nudRam.Value = new decimal(new int[] { 80, 0, 0, 0 });
            nudRam.ValueChanged += nudRam_ValueChanged;
            // 
            // lblRamUnit
            // 
            lblRamUnit.Location = new Point(405, 39);
            lblRamUnit.Name = "lblRamUnit";
            lblRamUnit.Size = new Size(100, 23);
            lblRamUnit.TabIndex = 5;
            // 
            // lblGpu
            // 
            lblGpu.Location = new Point(13, 72);
            lblGpu.Name = "lblGpu";
            lblGpu.Size = new Size(100, 23);
            lblGpu.TabIndex = 6;
            // 
            // nudGpu
            // 
            nudGpu.Location = new Point(154, 75);
            nudGpu.Name = "nudGpu";
            nudGpu.Size = new Size(120, 41);
            nudGpu.TabIndex = 7;
            nudGpu.Value = new decimal(new int[] { 80, 0, 0, 0 });
            nudGpu.ValueChanged += nudGpu_ValueChanged;
            // 
            // lblGpuUnit
            // 
            lblGpuUnit.Location = new Point(405, 72);
            lblGpuUnit.Name = "lblGpuUnit";
            lblGpuUnit.Size = new Size(100, 23);
            lblGpuUnit.TabIndex = 8;
            // 
            // lblThresholdSummary
            // 
            thresholdLayout.SetColumnSpan(lblThresholdSummary, 3);
            lblThresholdSummary.Dock = DockStyle.Fill;
            lblThresholdSummary.Font = new Font("맑은 고딕", 9F);
            lblThresholdSummary.ForeColor = Color.FromArgb(90, 90, 90);
            lblThresholdSummary.Location = new Point(13, 105);
            lblThresholdSummary.Name = "lblThresholdSummary";
            lblThresholdSummary.Size = new Size(780, 33);
            lblThresholdSummary.TabIndex = 9;
            lblThresholdSummary.Text = "설정된 임계값 | CPU 80%  RAM 80%  GPU 80%";
            lblThresholdSummary.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // grpKill
            // 
            grpKill.Controls.Add(killLayout);
            grpKill.Dock = DockStyle.Fill;
            grpKill.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            grpKill.Location = new Point(3, 196);
            grpKill.Margin = new Padding(3, 3, 3, 6);
            grpKill.Name = "grpKill";
            grpKill.Size = new Size(812, 237);
            grpKill.TabIndex = 1;
            grpKill.TabStop = false;
            grpKill.Text = "자동 종료 대상 프로세스";
            // 
            // killLayout
            // 
            killLayout.ColumnCount = 1;
            killLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            killLayout.Controls.Add(lstKill, 0, 0);
            killLayout.Controls.Add(killBottomLayout, 0, 1);
            killLayout.Dock = DockStyle.Fill;
            killLayout.Location = new Point(3, 37);
            killLayout.Name = "killLayout";
            killLayout.Padding = new Padding(8);
            killLayout.RowCount = 2;
            killLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            killLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            killLayout.Size = new Size(806, 197);
            killLayout.TabIndex = 0;
            // 
            // lstKill
            // 
            lstKill.Dock = DockStyle.Fill;
            lstKill.Font = new Font("맑은 고딕", 10F);
            lstKill.IntegralHeight = false;
            lstKill.Location = new Point(11, 11);
            lstKill.Name = "lstKill";
            lstKill.Size = new Size(784, 115);
            lstKill.TabIndex = 0;
            // 
            // killBottomLayout
            // 
            killBottomLayout.ColumnCount = 4;
            killBottomLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            killBottomLayout.ColumnStyles.Add(new ColumnStyle());
            killBottomLayout.ColumnStyles.Add(new ColumnStyle());
            killBottomLayout.ColumnStyles.Add(new ColumnStyle());
            killBottomLayout.Controls.Add(txtKillInput, 0, 0);
            killBottomLayout.Controls.Add(btnAddKill, 1, 0);
            killBottomLayout.Controls.Add(btnRemoveKill, 2, 0);
            killBottomLayout.Controls.Add(btnPickFromProcesses, 3, 0);
            killBottomLayout.Dock = DockStyle.Fill;
            killBottomLayout.Location = new Point(11, 132);
            killBottomLayout.Name = "killBottomLayout";
            killBottomLayout.Padding = new Padding(0, 6, 0, 0);
            killBottomLayout.RowCount = 1;
            killBottomLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            killBottomLayout.Size = new Size(784, 54);
            killBottomLayout.TabIndex = 1;
            // 
            // txtKillInput
            // 
            txtKillInput.Location = new Point(3, 9);
            txtKillInput.Name = "txtKillInput";
            txtKillInput.Size = new Size(100, 41);
            txtKillInput.TabIndex = 0;
            // 
            // btnAddKill
            // 
            btnAddKill.Location = new Point(550, 9);
            btnAddKill.Name = "btnAddKill";
            btnAddKill.Size = new Size(72, 23);
            btnAddKill.TabIndex = 1;
            // 
            // btnRemoveKill
            // 
            btnRemoveKill.Location = new Point(628, 9);
            btnRemoveKill.Name = "btnRemoveKill";
            btnRemoveKill.Size = new Size(72, 23);
            btnRemoveKill.TabIndex = 2;
            // 
            // btnPickFromProcesses
            // 
            btnPickFromProcesses.Location = new Point(706, 9);
            btnPickFromProcesses.Name = "btnPickFromProcesses";
            btnPickFromProcesses.Size = new Size(75, 23);
            btnPickFromProcesses.TabIndex = 3;
            // 
            // grpTrigger
            // 
            grpTrigger.Controls.Add(triggerLayout);
            grpTrigger.Dock = DockStyle.Fill;
            grpTrigger.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            grpTrigger.Location = new Point(3, 442);
            grpTrigger.Margin = new Padding(3, 3, 3, 6);
            grpTrigger.Name = "grpTrigger";
            grpTrigger.Size = new Size(812, 184);
            grpTrigger.TabIndex = 2;
            grpTrigger.TabStop = false;
            grpTrigger.Text = "트리거 프로그램 (실행 시 즉시 정리)";
            // 
            // triggerLayout
            // 
            triggerLayout.ColumnCount = 1;
            triggerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            triggerLayout.Controls.Add(lstTrigger, 0, 0);
            triggerLayout.Controls.Add(triggerBottomLayout, 0, 1);
            triggerLayout.Dock = DockStyle.Fill;
            triggerLayout.Location = new Point(3, 37);
            triggerLayout.Name = "triggerLayout";
            triggerLayout.Padding = new Padding(8);
            triggerLayout.RowCount = 2;
            triggerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            triggerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            triggerLayout.Size = new Size(806, 144);
            triggerLayout.TabIndex = 0;
            // 
            // lstTrigger
            // 
            lstTrigger.Dock = DockStyle.Fill;
            lstTrigger.Font = new Font("맑은 고딕", 10F);
            lstTrigger.IntegralHeight = false;
            lstTrigger.Location = new Point(11, 11);
            lstTrigger.Name = "lstTrigger";
            lstTrigger.Size = new Size(784, 62);
            lstTrigger.TabIndex = 0;
            // 
            // triggerBottomLayout
            // 
            triggerBottomLayout.ColumnCount = 3;
            triggerBottomLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            triggerBottomLayout.ColumnStyles.Add(new ColumnStyle());
            triggerBottomLayout.ColumnStyles.Add(new ColumnStyle());
            triggerBottomLayout.Controls.Add(txtTriggerInput, 0, 0);
            triggerBottomLayout.Controls.Add(btnAddTrigger, 1, 0);
            triggerBottomLayout.Controls.Add(btnRemoveTrigger, 2, 0);
            triggerBottomLayout.Dock = DockStyle.Fill;
            triggerBottomLayout.Location = new Point(11, 79);
            triggerBottomLayout.Name = "triggerBottomLayout";
            triggerBottomLayout.Padding = new Padding(0, 6, 0, 0);
            triggerBottomLayout.RowCount = 1;
            triggerBottomLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            triggerBottomLayout.Size = new Size(784, 54);
            triggerBottomLayout.TabIndex = 1;
            // 
            // txtTriggerInput
            // 
            txtTriggerInput.Location = new Point(3, 9);
            txtTriggerInput.Name = "txtTriggerInput";
            txtTriggerInput.Size = new Size(100, 41);
            txtTriggerInput.TabIndex = 0;
            // 
            // btnAddTrigger
            // 
            btnAddTrigger.Location = new Point(625, 9);
            btnAddTrigger.Name = "btnAddTrigger";
            btnAddTrigger.Size = new Size(75, 23);
            btnAddTrigger.TabIndex = 1;
            // 
            // btnRemoveTrigger
            // 
            btnRemoveTrigger.Location = new Point(706, 9);
            btnRemoveTrigger.Name = "btnRemoveTrigger";
            btnRemoveTrigger.Size = new Size(75, 23);
            btnRemoveTrigger.TabIndex = 2;
            // 
            // controlLayout
            // 
            controlLayout.ColumnCount = 2;
            controlLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
            controlLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
            controlLayout.Controls.Add(chkEnable, 0, 0);
            controlLayout.Controls.Add(lblState, 1, 0);
            controlLayout.Controls.Add(lblCurrentStatus, 0, 1);
            controlLayout.Dock = DockStyle.Fill;
            controlLayout.Location = new Point(3, 632);
            controlLayout.Margin = new Padding(3, 0, 3, 6);
            controlLayout.Name = "controlLayout";
            controlLayout.Padding = new Padding(4);
            controlLayout.RowCount = 2;
            controlLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            controlLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            controlLayout.Size = new Size(812, 82);
            controlLayout.TabIndex = 3;
            // 
            // chkEnable
            // 
            chkEnable.Dock = DockStyle.Fill;
            chkEnable.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            chkEnable.Location = new Point(7, 7);
            chkEnable.Name = "chkEnable";
            chkEnable.Size = new Size(331, 31);
            chkEnable.TabIndex = 0;
            chkEnable.Text = "자동 관리 활성화";
            chkEnable.CheckedChanged += chkEnable_CheckedChanged;
            // 
            // lblState
            // 
            lblState.Dock = DockStyle.Fill;
            lblState.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            lblState.ForeColor = Color.DimGray;
            lblState.Location = new Point(344, 4);
            lblState.Name = "lblState";
            lblState.Size = new Size(461, 37);
            lblState.TabIndex = 1;
            lblState.Text = "상태: 중지됨";
            lblState.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblCurrentStatus
            // 
            controlLayout.SetColumnSpan(lblCurrentStatus, 2);
            lblCurrentStatus.Dock = DockStyle.Fill;
            lblCurrentStatus.Font = new Font("맑은 고딕", 9.5F);
            lblCurrentStatus.ForeColor = Color.FromArgb(60, 60, 60);
            lblCurrentStatus.Location = new Point(7, 41);
            lblCurrentStatus.Name = "lblCurrentStatus";
            lblCurrentStatus.Size = new Size(798, 37);
            lblCurrentStatus.TabIndex = 2;
            lblCurrentStatus.Text = "현재 상태 | CPU --%  RAM --%  GPU --%";
            lblCurrentStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtLog
            // 
            txtLog.BackColor = Color.White;
            txtLog.Dock = DockStyle.Fill;
            txtLog.Font = new Font("Consolas", 9F);
            txtLog.Location = new Point(3, 723);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(812, 154);
            txtLog.TabIndex = 4;
            // 
            // sideLayout
            // 
            sideLayout.ColumnCount = 1;
            sideLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            sideLayout.Controls.Add(grpRecommend, 0, 0);
            sideLayout.Controls.Add(rtbManual, 0, 1);
            sideLayout.Dock = DockStyle.Fill;
            sideLayout.Location = new Point(841, 13);
            sideLayout.Name = "sideLayout";
            sideLayout.RowCount = 2;
            sideLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
            sideLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
            sideLayout.Size = new Size(546, 874);
            sideLayout.TabIndex = 1;
            // 
            // grpRecommend
            // 
            grpRecommend.Controls.Add(recommendLayout);
            grpRecommend.Dock = DockStyle.Fill;
            grpRecommend.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            grpRecommend.Location = new Point(3, 3);
            grpRecommend.Margin = new Padding(3, 3, 3, 6);
            grpRecommend.Name = "grpRecommend";
            grpRecommend.Size = new Size(540, 471);
            grpRecommend.TabIndex = 0;
            grpRecommend.TabStop = false;
            grpRecommend.Text = "자동 종료 추천 (메모리 상위)";
            // 
            // recommendLayout
            // 
            recommendLayout.ColumnCount = 1;
            recommendLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            recommendLayout.Controls.Add(lvRecommend, 0, 0);
            recommendLayout.Controls.Add(recommendButtonLayout, 0, 1);
            recommendLayout.Dock = DockStyle.Fill;
            recommendLayout.Location = new Point(3, 37);
            recommendLayout.Name = "recommendLayout";
            recommendLayout.Padding = new Padding(8);
            recommendLayout.RowCount = 2;
            recommendLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            recommendLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            recommendLayout.Size = new Size(534, 431);
            recommendLayout.TabIndex = 0;
            // 
            // lvRecommend
            // 
            lvRecommend.Columns.AddRange(new ColumnHeader[] { colRecName, colRecMem });
            lvRecommend.Dock = DockStyle.Fill;
            lvRecommend.Font = new Font("맑은 고딕", 10F);
            lvRecommend.FullRowSelect = true;
            lvRecommend.Location = new Point(11, 11);
            lvRecommend.Name = "lvRecommend";
            lvRecommend.Size = new Size(512, 349);
            lvRecommend.TabIndex = 0;
            lvRecommend.UseCompatibleStateImageBehavior = false;
            lvRecommend.View = View.Details;
            lvRecommend.DoubleClick += lvRecommend_DoubleClick;
            // 
            // colRecName
            // 
            colRecName.Text = "프로세스 이름";
            colRecName.Width = 180;
            // 
            // colRecMem
            // 
            colRecMem.Text = "메모리 (MB)";
            colRecMem.TextAlign = HorizontalAlignment.Right;
            colRecMem.Width = 110;
            // 
            // recommendButtonLayout
            // 
            recommendButtonLayout.ColumnCount = 2;
            recommendButtonLayout.ColumnStyles.Add(new ColumnStyle());
            recommendButtonLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            recommendButtonLayout.Controls.Add(btnRefreshRecommend, 0, 0);
            recommendButtonLayout.Controls.Add(btnAddRecommend, 1, 0);
            recommendButtonLayout.Dock = DockStyle.Fill;
            recommendButtonLayout.Location = new Point(11, 366);
            recommendButtonLayout.Name = "recommendButtonLayout";
            recommendButtonLayout.Padding = new Padding(0, 6, 0, 0);
            recommendButtonLayout.RowCount = 1;
            recommendButtonLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            recommendButtonLayout.Size = new Size(512, 54);
            recommendButtonLayout.TabIndex = 1;
            // 
            // btnRefreshRecommend
            // 
            btnRefreshRecommend.Location = new Point(3, 9);
            btnRefreshRecommend.Name = "btnRefreshRecommend";
            btnRefreshRecommend.Size = new Size(75, 23);
            btnRefreshRecommend.TabIndex = 0;
            // 
            // btnAddRecommend
            // 
            btnAddRecommend.Location = new Point(84, 9);
            btnAddRecommend.Name = "btnAddRecommend";
            btnAddRecommend.Size = new Size(75, 23);
            btnAddRecommend.TabIndex = 1;
            // 
            // rtbManual
            // 
            rtbManual.Dock = DockStyle.Fill;
            rtbManual.Location = new Point(3, 486);
            rtbManual.Margin = new Padding(3, 6, 3, 3);
            rtbManual.Name = "rtbManual";
            rtbManual.Size = new Size(540, 385);
            rtbManual.TabIndex = 1;
            rtbManual.Text = "";
            // 
            // FocusModeForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1400, 900);
            Controls.Add(rootLayout);
            Font = new Font("맑은 고딕", 9F);
            MinimumSize = new Size(1100, 760);
            Name = "FocusModeForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "커스텀 기반 자원 관리";
            rootLayout.ResumeLayout(false);
            settingsLayout.ResumeLayout(false);
            settingsLayout.PerformLayout();
            grpThreshold.ResumeLayout(false);
            thresholdLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nudCpu).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRam).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudGpu).EndInit();
            grpKill.ResumeLayout(false);
            killLayout.ResumeLayout(false);
            killBottomLayout.ResumeLayout(false);
            killBottomLayout.PerformLayout();
            grpTrigger.ResumeLayout(false);
            triggerLayout.ResumeLayout(false);
            triggerBottomLayout.ResumeLayout(false);
            triggerBottomLayout.PerformLayout();
            controlLayout.ResumeLayout(false);
            sideLayout.ResumeLayout(false);
            grpRecommend.ResumeLayout(false);
            recommendLayout.ResumeLayout(false);
            recommendButtonLayout.ResumeLayout(false);
            ResumeLayout(false);
        }

        // 임계값 행 공통 구성 (레이블 + NumericUpDown + "%")
        private static void SetupThresholdRow(Label label, string caption, NumericUpDown nud, Label unit)
        {
            label.Text = caption;
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);

            // NumericUpDown은 UpDownBase의 고정 높이 특성 때문에 Dock=Fill 을 쓰면
            // TableLayoutPanel 셀 내에서 위치가 틀어짐. Anchor(좌우)만 걸어 수직 중앙배치.
            nud.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            nud.Minimum = 10;
            nud.Maximum = 100;
            nud.Increment = 5;
            nud.TextAlign = HorizontalAlignment.Right;
            nud.Font = new Font("맑은 고딕", 10F);
            nud.Margin = new Padding(3, 4, 3, 4);

            unit.Text = "%";
            unit.Dock = DockStyle.Fill;
            unit.TextAlign = ContentAlignment.MiddleLeft;
            unit.Font = new Font("맑은 고딕", 10F);
        }

        private static void SetupInputBox(TextBox tb, string placeholder)
        {
            tb.Dock = DockStyle.Fill;
            tb.Font = new Font("맑은 고딕", 10F);
            tb.PlaceholderText = placeholder;
            tb.Margin = new Padding(3, 4, 3, 3);
        }

        private static void SetupLayoutButton(Button btn, string text, System.EventHandler onClick)
        {
            btn.Dock = DockStyle.Fill;
            btn.Text = text;
            btn.Margin = new Padding(3, 4, 3, 3);
            btn.Click += onClick;
        }

        #endregion

        private TableLayoutPanel rootLayout;
        private TableLayoutPanel settingsLayout;

        private GroupBox grpThreshold;
        private TableLayoutPanel thresholdLayout;
        private Label lblCpu;
        private NumericUpDown nudCpu;
        private Label lblCpuUnit;
        private Label lblRam;
        private NumericUpDown nudRam;
        private Label lblRamUnit;
        private Label lblGpu;
        private NumericUpDown nudGpu;
        private Label lblGpuUnit;
        private Label lblThresholdSummary;

        private GroupBox grpKill;
        private TableLayoutPanel killLayout;
        private ListBox lstKill;
        private TableLayoutPanel killBottomLayout;
        private TextBox txtKillInput;
        private Button btnAddKill;
        private Button btnRemoveKill;
        private Button btnPickFromProcesses;

        private GroupBox grpTrigger;
        private TableLayoutPanel triggerLayout;
        private ListBox lstTrigger;
        private TableLayoutPanel triggerBottomLayout;
        private TextBox txtTriggerInput;
        private Button btnAddTrigger;
        private Button btnRemoveTrigger;

        private TableLayoutPanel controlLayout;
        private CheckBox chkEnable;
        private Label lblState;
        private Label lblCurrentStatus;

        private TextBox txtLog;

        private TableLayoutPanel sideLayout;
        private GroupBox grpRecommend;
        private TableLayoutPanel recommendLayout;
        private ListView lvRecommend;
        private ColumnHeader colRecName;
        private ColumnHeader colRecMem;
        private TableLayoutPanel recommendButtonLayout;
        private Button btnRefreshRecommend;
        private Button btnAddRecommend;

        private RichTextBox rtbManual;
    }
}
