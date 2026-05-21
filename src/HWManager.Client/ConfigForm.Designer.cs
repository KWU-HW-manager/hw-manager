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
            tabControl = new TabControl();
            tableLayoutPanelButtons = new TableLayoutPanel();
            btnOK = new Button();
            btnCancel = new Button();

            tabControl.SuspendLayout();
            tableLayoutPanelButtons.SuspendLayout();
            SuspendLayout();

            // tabControl
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(500, 400);
            tabControl.TabIndex = 0;

            // tableLayoutPanelButtons
            tableLayoutPanelButtons.ColumnCount = 2;
            tableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanelButtons.Controls.Add(btnOK, 0, 0);
            tableLayoutPanelButtons.Controls.Add(btnCancel, 1, 0);
            tableLayoutPanelButtons.Dock = DockStyle.Bottom;
            tableLayoutPanelButtons.Location = new Point(0, 360);
            tableLayoutPanelButtons.Name = "tableLayoutPanelButtons";
            tableLayoutPanelButtons.RowCount = 1;
            tableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanelButtons.Size = new Size(500, 40);
            tableLayoutPanelButtons.TabIndex = 1;

            // btnOK
            btnOK.Dock = DockStyle.Fill;
            btnOK.Font = new Font("∏º¿∫ ∞ÌµÒ", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnOK.Location = new Point(5, 5);
            btnOK.Margin = new Padding(5);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(240, 30);
            btnOK.TabIndex = 0;
            btnOK.Text = "»Æ¿Œ";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;

            // btnCancel
            btnCancel.Dock = DockStyle.Fill;
            btnCancel.Font = new Font("∏º¿∫ ∞ÌµÒ", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnCancel.Location = new Point(255, 5);
            btnCancel.Margin = new Padding(5);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(240, 30);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "√Îº“";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;

            // ConfigForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(500, 400);
            Controls.Add(tabControl);
            Controls.Add(tableLayoutPanelButtons);
            Font = new Font("∏º¿∫ ∞ÌµÒ", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 129);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ConfigForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "º≥¡§";

            tabControl.ResumeLayout(false);
            tableLayoutPanelButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl;
        private TableLayoutPanel tableLayoutPanelButtons;
        private Button btnOK;
        private Button btnCancel;
    }
}