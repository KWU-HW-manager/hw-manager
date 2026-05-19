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
            btnFocusMode = new Button();
            btnExit = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();

            // 버튼은 Dock=Fill + Margin 기반으로 배치해야 DPI 스케일링이 안전하다.
            // (하드코딩된 Location/Size 는 PerMonitorV2 환경에서 어긋나기 쉬움)
            ConfigureCardButton(btnMonitor, "하드웨어 모니터링", 0);
            btnMonitor.Click += btnMonitor_Click;

            ConfigureCardButton(btnProcess, "프로그램 관리", 1);
            btnProcess.Click += btnProcess_Click;

            ConfigureCardButton(btnFocusMode, "커스텀 자원 관리", 2);
            btnFocusMode.Click += btnFocusMode_Click;

            ConfigureCardButton(btnExit, "종료", 3);
            btnExit.Click += btnExit_Click;

            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.BackColor = Color.WhiteSmoke;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(btnMonitor, 0, 0);
            tableLayoutPanel1.Controls.Add(btnProcess, 0, 1);
            tableLayoutPanel1.Controls.Add(btnFocusMode, 0, 2);
            tableLayoutPanel1.Controls.Add(btnExit, 0, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.TabIndex = 0;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(800, 510);
            MinimumSize = new Size(460, 420);
            Controls.Add(tableLayoutPanel1);
            Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 129);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "HWManager";
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        // 공통: 대시보드 카드 버튼 구성.
        // Dock=Fill + Margin 조합은 AutoScaleMode.Font 와 함께 쓰면
        // 해상도/배율이 달라도 비율이 유지된다.
        private void ConfigureCardButton(Button btn, string caption, int tabIndex)
        {
            btn.Dock = DockStyle.Fill;
            btn.Margin = new Padding(40, 15, 40, 15);
            btn.Name = "btn_" + tabIndex;
            btn.TabIndex = tabIndex;
            btn.Text = caption;
            btn.UseVisualStyleBackColor = true;
        }

        #endregion

        private Button btnMonitor;
        private Button btnProcess;
        private Button btnFocusMode;
        private Button btnExit;
        private TableLayoutPanel tableLayoutPanel1;
    }
}
