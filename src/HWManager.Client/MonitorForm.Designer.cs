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
            lblCPU = new Label();
            lblRAM = new Label();
            lblGPU = new Label();
            pbGPU = new ProgressBar();
            pbCPU = new ProgressBar();
            pbRAM = new ProgressBar();
            timer1 = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // lblCPU
            // 
            lblCPU.AutoSize = true;
            lblCPU.Location = new Point(12, 18);
            lblCPU.Name = "lblCPU";
            lblCPU.Size = new Size(94, 15);
            lblCPU.TabIndex = 0;
            lblCPU.Text = "CPU 사용량: 0%";
            // 
            // lblRAM
            // 
            lblRAM.AutoSize = true;
            lblRAM.Location = new Point(12, 119);
            lblRAM.Name = "lblRAM";
            lblRAM.Size = new Size(97, 15);
            lblRAM.TabIndex = 1;
            lblRAM.Text = "RAM 사용량: 0%";
            // 
            // lblGPU
            // 
            lblGPU.AutoSize = true;
            lblGPU.Location = new Point(12, 215);
            lblGPU.Name = "lblGPU";
            lblGPU.Size = new Size(94, 15);
            lblGPU.TabIndex = 2;
            lblGPU.Text = "GPU 사용량: 0%";
            // 
            // pbGPU
            // 
            pbGPU.Location = new Point(12, 233);
            pbGPU.Name = "pbGPU";
            pbGPU.Size = new Size(435, 23);
            pbGPU.Style = ProgressBarStyle.Continuous;
            pbGPU.TabIndex = 3;
            // 
            // pbCPU
            // 
            pbCPU.Location = new Point(12, 36);
            pbCPU.Name = "pbCPU";
            pbCPU.Size = new Size(435, 23);
            pbCPU.Style = ProgressBarStyle.Continuous;
            pbCPU.TabIndex = 4;
            // 
            // pbRAM
            // 
            pbRAM.Location = new Point(12, 137);
            pbRAM.Name = "pbRAM";
            pbRAM.Size = new Size(435, 23);
            pbRAM.Style = ProgressBarStyle.Continuous;
            pbRAM.TabIndex = 5;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            // 
            // MonitorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pbRAM);
            Controls.Add(pbCPU);
            Controls.Add(pbGPU);
            Controls.Add(lblGPU);
            Controls.Add(lblRAM);
            Controls.Add(lblCPU);
            Name = "MonitorForm";
            Text = "MonitorForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCPU;
        private Label lblRAM;
        private Label lblGPU;
        private ProgressBar pbGPU;
        private ProgressBar pbCPU;
        private ProgressBar pbRAM;
        private System.Windows.Forms.Timer timer1;
    }
}