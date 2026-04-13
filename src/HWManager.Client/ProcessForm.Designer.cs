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
            dgvProcessLog = new DataGridView();
            btnRefreshLogs = new Button();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProcessLog).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1143, 750);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(tableLayoutPanel1);
            tabPage1.Location = new Point(4, 34);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1135, 712);
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
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Margin = new Padding(4, 5, 4, 5);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(14, 17, 14, 17);
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 117F));
            tableLayoutPanel1.Size = new Size(1129, 706);
            tableLayoutPanel1.TabIndex = 4;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnKill);
            panel1.Controls.Add(btnRefresh);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(18, 577);
            panel1.Margin = new Padding(4, 5, 4, 5);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(0, 0, 21, 0);
            panel1.Size = new Size(1093, 107);
            panel1.TabIndex = 0;
            // 
            // btnKill
            // 
            btnKill.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnKill.Location = new Point(151, 0);
            btnKill.Margin = new Padding(4, 5, 4, 5);
            btnKill.Name = "btnKill";
            btnKill.Size = new Size(137, 38);
            btnKill.TabIndex = 2;
            btnKill.Text = "프로세스 종료";
            btnKill.UseVisualStyleBackColor = true;
            btnKill.Click += btnKill_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRefresh.Location = new Point(0, 0);
            btnRefresh.Margin = new Padding(4, 5, 4, 5);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(143, 38);
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
            lvProcesses.Location = new Point(35, 172);
            lvProcesses.Margin = new Padding(21, 5, 21, 5);
            lvProcesses.Name = "lvProcesses";
            lvProcesses.Size = new Size(1059, 395);
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
            panel2.Location = new Point(18, 22);
            panel2.Margin = new Padding(4, 5, 4, 5);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(21, 25, 21, 25);
            panel2.Size = new Size(1093, 140);
            panel2.TabIndex = 1;
            // 
            // lblSummary
            // 
            lblSummary.AutoSize = true;
            lblSummary.Dock = DockStyle.Top;
            lblSummary.Location = new Point(21, 56);
            lblSummary.Margin = new Padding(4, 0, 4, 0);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(334, 25);
            lblSummary.TabIndex = 1;
            lblSummary.Text = "\"총 프로세스: 0개 | 메모리 사용량: 0GB\"";
            lblSummary.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtSearch
            // 
            txtSearch.Dock = DockStyle.Top;
            txtSearch.Location = new Point(21, 25);
            txtSearch.Margin = new Padding(0, 0, 0, 17);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "프로세스 검색...";
            txtSearch.Size = new Size(1051, 31);
            txtSearch.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(dgvProcessLog);
            tabPage2.Controls.Add(btnRefreshLogs);
            tabPage2.Location = new Point(4, 34);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1135, 712);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "로그 기록 조회";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvProcessLog
            // 
            dgvProcessLog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProcessLog.Dock = DockStyle.Bottom;
            dgvProcessLog.Location = new Point(3, 46);
            dgvProcessLog.Name = "dgvProcessLog";
            dgvProcessLog.RowHeadersWidth = 62;
            dgvProcessLog.Size = new Size(1129, 663);
            dgvProcessLog.TabIndex = 1;
            // 
            // btnRefreshLogs
            // 
            btnRefreshLogs.Location = new Point(6, 6);
            btnRefreshLogs.Name = "btnRefreshLogs";
            btnRefreshLogs.Size = new Size(112, 34);
            btnRefreshLogs.TabIndex = 0;
            btnRefreshLogs.Text = "새로고침";
            btnRefreshLogs.UseVisualStyleBackColor = true;
            btnRefreshLogs.Click += btnRefreshLogs_Click;
            // 
            // ProcessForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1143, 750);
            Controls.Add(tabControl1);
            Margin = new Padding(4, 5, 4, 5);
            Name = "ProcessForm";
            Text = "ProcessForm";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            tabPage2.ResumeLayout(false);
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
    }
}