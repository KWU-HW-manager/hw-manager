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
            panelContent = new Panel();
            tableLayoutPanelButtons = new TableLayoutPanel();
            btnSave = new Button();
            btnCancel = new Button();

            tableLayoutPanelButtons.SuspendLayout();
            SuspendLayout();

            // panelContent - ¼³Á¤ ³»¿ëÀÌ µé¾î°¥ ¿µ¿ª
            panelContent.BackColor = Color.White;
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(0, 0);
            panelContent.Name = "panelContent";
            panelContent.Padding = new Padding(15);
            panelContent.Size = new Size(600, 340);
            panelContent.TabIndex = 0;

            // tableLayoutPanelButtons - ¹öÆ° ·¹ÀÌ¾Æ¿ô
            tableLayoutPanelButtons.BackColor = Color.WhiteSmoke;
            tableLayoutPanelButtons.ColumnCount = 2;
            tableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanelButtons.Controls.Add(btnSave, 0, 0);
            tableLayoutPanelButtons.Controls.Add(btnCancel, 1, 0);
            tableLayoutPanelButtons.Dock = DockStyle.Bottom;
            tableLayoutPanelButtons.Location = new Point(0, 340);
            tableLayoutPanelButtons.Name = "tableLayoutPanelButtons";
            tableLayoutPanelButtons.Padding = new Padding(10);
            tableLayoutPanelButtons.RowCount = 1;
            tableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanelButtons.Size = new Size(600, 60);
            tableLayoutPanelButtons.TabIndex = 1;

            // btnSave
            btnSave.BackColor = Color.FromArgb(79, 129, 189);
            btnSave.Dock = DockStyle.Fill;
            btnSave.Font = new Font("¸¼Àº °íµñ", 9F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnSave.ForeColor = Color.White;
            btnSave.Margin = new Padding(5);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(285, 40);
            btnSave.TabIndex = 0;
            btnSave.Text = "ÀúÀå";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;

            // btnCancel
            btnCancel.BackColor = Color.LightGray;
            btnCancel.Dock = DockStyle.Fill;
            btnCancel.Font = new Font("¸¼Àº °íµñ", 9F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnCancel.Margin = new Padding(5);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(285, 40);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Ãë¼Ò";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;

            // ConfigForm
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(600, 400);
            Controls.Add(panelContent);
            Controls.Add(tableLayoutPanelButtons);
            Font = new Font("¸¼Àº °íµñ", 9F, FontStyle.Regular, GraphicsUnit.Point, 129);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ConfigForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "¼³Á¤";
            Load += ConfigForm_Load;

            tableLayoutPanelButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button btnSave;
        private Button btnCancel;
        private Panel panelContent;
        private TableLayoutPanel tableLayoutPanelButtons;
    }
}