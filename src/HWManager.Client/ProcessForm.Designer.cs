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
            lvProcesses = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            btnRefresh = new Button();
            btnKill = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new Panel();
            panel2 = new Panel();
            lblSummary = new Label();
            txtSearch = new TextBox();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // lvProcesses
            // 
            lvProcesses.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            lvProcesses.Dock = DockStyle.Fill;
            lvProcesses.FullRowSelect = true;
            lvProcesses.Location = new Point(25, 103);
            lvProcesses.Margin = new Padding(15, 3, 15, 3);
            lvProcesses.Name = "lvProcesses";
            lvProcesses.Size = new Size(750, 264);
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
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRefresh.Location = new Point(560, 3);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 23);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "목록 새로고침";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnKill
            // 
            btnKill.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnKill.Location = new Point(666, 3);
            btnKill.Name = "btnKill";
            btnKill.Size = new Size(96, 23);
            btnKill.TabIndex = 2;
            btnKill.Text = "프로세스 종료";
            btnKill.UseVisualStyleBackColor = true;
            btnKill.Click += btnKill_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 2);
            tableLayoutPanel1.Controls.Add(lvProcesses, 0, 1);
            tableLayoutPanel1.Controls.Add(panel2, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(10);
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnKill);
            panel1.Controls.Add(btnRefresh);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(13, 373);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(0, 0, 15, 0);
            panel1.Size = new Size(774, 64);
            panel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(lblSummary);
            panel2.Controls.Add(txtSearch);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(13, 13);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(15);
            panel2.Size = new Size(774, 84);
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
            txtSearch.Size = new Size(744, 23);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // ProcessForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "ProcessForm";
            Text = "ProcessForm";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ListView lvProcesses;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private Button btnRefresh;
        private Button btnKill;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private Panel panel2;
        private TextBox txtSearch;
        private Label lblSummary;
        private ColumnHeader columnHeader3;
    }
}