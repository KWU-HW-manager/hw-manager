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
            btnRefresh = new Button();
            btnKill = new Button();
            SuspendLayout();
            // 
            // lvProcesses
            // 
            lvProcesses.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2 });
            lvProcesses.FullRowSelect = true;
            lvProcesses.Location = new Point(12, 12);
            lvProcesses.Name = "lvProcesses";
            lvProcesses.Size = new Size(639, 196);
            lvProcesses.TabIndex = 0;
            lvProcesses.UseCompatibleStateImageBehavior = false;
            lvProcesses.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "프로세스이름";
            columnHeader1.Width = 180;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "ID";
            columnHeader2.Width = 180;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(12, 214);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 23);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "목록 새로고침";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnKill
            // 
            btnKill.Location = new Point(118, 214);
            btnKill.Name = "btnKill";
            btnKill.Size = new Size(96, 23);
            btnKill.TabIndex = 2;
            btnKill.Text = "프로세스 종료";
            btnKill.UseVisualStyleBackColor = true;
            btnKill.Click += btnKill_Click;
            // 
            // ProcessForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnKill);
            Controls.Add(btnRefresh);
            Controls.Add(lvProcesses);
            Name = "ProcessForm";
            Text = "ProcessForm";
            Load += ProcessForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private ListView lvProcesses;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private Button btnRefresh;
        private Button btnKill;
    }
}