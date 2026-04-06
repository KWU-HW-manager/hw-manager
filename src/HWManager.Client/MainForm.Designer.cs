namespace HWManager.Client
{
    partial class MainForm
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
            btnMonitor = new Button();
            btnProcess = new Button();
            btnExit = new Button();
            SuspendLayout();
            // 
            // btnMonitor
            // 
            btnMonitor.Location = new Point(286, 61);
            btnMonitor.Name = "btnMonitor";
            btnMonitor.Size = new Size(209, 67);
            btnMonitor.TabIndex = 0;
            btnMonitor.Text = "하드웨어 모니터링";
            btnMonitor.UseVisualStyleBackColor = true;
            btnMonitor.Click += btnMonitor_Click;
            // 
            // btnProcess
            // 
            btnProcess.Location = new Point(286, 172);
            btnProcess.Name = "btnProcess";
            btnProcess.Size = new Size(209, 68);
            btnProcess.TabIndex = 1;
            btnProcess.Text = "프로그램 관리";
            btnProcess.UseVisualStyleBackColor = true;
            btnProcess.Click += btnProcess_Click;
            // 
            // btnExit
            // 
            btnExit.Location = new Point(286, 283);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(209, 66);
            btnExit.TabIndex = 2;
            btnExit.Text = "종료";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnExit);
            Controls.Add(btnProcess);
            Controls.Add(btnMonitor);
            Name = "MainForm";
            Text = "HWManager";
            ResumeLayout(false);
        }

        #endregion

        private Button btnMonitor;
        private Button btnProcess;
        private Button btnExit;
    }
}