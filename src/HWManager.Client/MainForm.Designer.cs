namespace HWManager.Client
{
    partial class MainForm
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
            btnMonitor = new Button();
            btnProcess = new Button();
            btnExit = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();

            btnMonitor.Anchor = AnchorStyles.None;
            btnMonitor.Location = new Point(295, 47);
            btnMonitor.Name = "btnMonitor";
            btnMonitor.Size = new Size(209, 76);
            btnMonitor.TabIndex = 0;
            btnMonitor.Text = "하드웨어 모니터링";
            btnMonitor.UseVisualStyleBackColor = true;
            btnMonitor.Click += btnMonitor_Click;

            btnProcess.Anchor = AnchorStyles.None;
            btnProcess.Location = new Point(295, 216);
            btnProcess.Name = "btnProcess";
            btnProcess.Size = new Size(209, 77);
            btnProcess.TabIndex = 1;
            btnProcess.Text = "프로그램 관리";
            btnProcess.UseVisualStyleBackColor = true;
            btnProcess.Click += btnProcess_Click;

            btnExit.Anchor = AnchorStyles.None;
            btnExit.Location = new Point(295, 387);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(209, 75);
            btnExit.TabIndex = 2;
            btnExit.Text = "종료";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;

            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.BackColor = Color.WhiteSmoke;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(btnMonitor, 0, 0);
            tableLayoutPanel1.Controls.Add(btnProcess, 0, 1);
            tableLayoutPanel1.Controls.Add(btnExit, 0, 2);
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Size = new Size(800, 510);
            tableLayoutPanel1.TabIndex = 3;

            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(800, 510);
            Controls.Add(tableLayoutPanel1);
            Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 129);
            Name = "MainForm";
            Text = "HWManager";
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button btnMonitor;
        private Button btnProcess;
        private Button btnExit;
        private TableLayoutPanel tableLayoutPanel1;
    }
}