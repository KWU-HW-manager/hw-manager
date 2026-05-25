namespace HWManager.Client
{
    partial class ProcessForm
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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            btnKill = new Button();
            btnRefresh = new Button();
            lvProcesses = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            panel2 = new Panel();
            lblSummary = new Label();
            txtSearch = new TextBox();
            tabPage2 = new TabPage();
            tableLayoutPanel2 = new TableLayoutPanel();
            btnRefreshLogs = new Button();
            dgvProcessLog = new DataGridView();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            tabPage2.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProcessLog).BeginInit();
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
            tabControl1.Size = new Size(800, 450);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(tableLayoutPanel1);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Margin = new Padding(2);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(2);
            tabPage1.Size = new Size(792, 422);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "실시간 모니터링";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 2);
            tableLayoutPanel1.Controls.Add(lvProcesses, 0, 1);
            tableLayoutPanel1.Controls.Add(panel2, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(2, 2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(10);
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            tableLayoutPanel1.Size = new Size(788, 418);
            tableLayoutPanel1.TabIndex = 4;
            // 
            // panel1
            // 
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(13, 341);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(0, 0, 15, 0);
            panel1.Size = new Size(762, 64);
            panel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(btnKill);
            flowLayoutPanel1.Controls.Add(btnRefresh);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(747, 64);
            flowLayoutPanel1.TabIndex = 3;
            // 
            // btnKill
            // 
            btnKill.Location = new Point(648, 3);
            btnKill.Name = "btnKill";
            btnKill.Size = new Size(96, 23);
            btnKill.TabIndex = 2;
            btnKill.Text = "프로세스 종료";
            btnKill.UseVisualStyleBackColor = true;
            btnKill.Click += btnKill_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.Location = new Point(542, 3);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 23);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "목록 새로고침";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // lvProcesses
            // 
            lvProcesses.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            lvProcesses.Dock = DockStyle.Fill;
            lvProcesses.FullRowSelect = true;
            lvProcesses.Location = new Point(25, 103);
            lvProcesses.Margin = new Padding(15, 3, 15, 3);
            lvProcesses.Name = "lvProcesses";
            lvProcesses.Size = new Size(738, 232);
            lvProcesses.TabIndex = 0;
            lvProcesses.UseCompatibleStateImageBehavior = false;
            lvProcesses.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "프로세스이름";
            columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "ID";
            columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "메모리 사용량 (MB)";
            columnHeader3.TextAlign = HorizontalAlignment.Right;
            columnHeader3.Width = 150;
            // 
            // panel2
            // 
            panel2.Controls.Add(lblSummary);
            panel2.Controls.Add(txtSearch);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(13, 13);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(15);
            panel2.Size = new Size(762, 84);
            panel2.TabIndex = 1;
            // 
            // lblSummary
            // 
            lblSummary.AutoSize = true;
            lblSummary.Dock = DockStyle.Top;
            lblSummary.Location = new Point(15, 38);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(223, 15);
            lblSummary.TabIndex = 1;
            lblSummary.Text = "\"총 프로세스: 0개 | 메모리 사용량: 0GB\"";
            lblSummary.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtSearch
            // 
            txtSearch.Dock = DockStyle.Top;
            txtSearch.Location = new Point(15, 15);
            txtSearch.Margin = new Padding(0, 0, 0, 10);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "프로세스 검색...";
            txtSearch.Size = new Size(732, 23);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += textBox1_TextChanged;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(tableLayoutPanel2);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Margin = new Padding(2);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(2);
            tabPage2.Size = new Size(792, 422);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "로그 기록 조회";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(btnRefreshLogs, 0, 0);
            tableLayoutPanel2.Controls.Add(dgvProcessLog, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(2, 2);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(788, 418);
            tableLayoutPanel2.TabIndex = 2;
            // 
            // btnRefreshLogs
            // 
            btnRefreshLogs.Anchor = AnchorStyles.Left;
            btnRefreshLogs.AutoSize = true;
            btnRefreshLogs.Location = new Point(2, 2);
            btnRefreshLogs.Margin = new Padding(2);
            btnRefreshLogs.Name = "btnRefreshLogs";
            btnRefreshLogs.Size = new Size(79, 30);
            btnRefreshLogs.TabIndex = 0;
            btnRefreshLogs.Text = "새로고침";
            btnRefreshLogs.UseVisualStyleBackColor = true;
            btnRefreshLogs.Click += btnRefreshLogs_Click;
            // 
            // dgvProcessLog
            // 
            dgvProcessLog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProcessLog.Dock = DockStyle.Fill;
            dgvProcessLog.Location = new Point(2, 36);
            dgvProcessLog.Margin = new Padding(2);
            dgvProcessLog.Name = "dgvProcessLog";
            dgvProcessLog.RowHeadersWidth = 62;
            dgvProcessLog.Size = new Size(784, 380);
            dgvProcessLog.TabIndex = 1;
            // 
            // ProcessForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tabControl1);
            Name = "ProcessForm";
            Text = "ProcessForm";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            tabPage2.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProcessLog).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private Button btnKill;
        private Button btnRefresh;
        private ListView lvProcesses;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private Panel panel2;
        private Label lblSummary;
        private TextBox txtSearch;
        private TabPage tabPage2;
        private DataGridView dgvProcessLog;
        private Button btnRefreshLogs;
        private TableLayoutPanel tableLayoutPanel2;
        private FlowLayoutPanel flowLayoutPanel1;
    }
}