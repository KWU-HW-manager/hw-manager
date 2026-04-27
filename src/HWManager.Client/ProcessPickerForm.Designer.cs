using System.Drawing;
using System.Windows.Forms;

namespace HWManager.Client
{
    partial class ProcessPickerForm
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
            rootLayout = new TableLayoutPanel();
            lblHeader = new Label();
            lvProcesses = new ListView();
            colName = new ColumnHeader();
            colMem = new ColumnHeader();
            colCount = new ColumnHeader();
            btnLayout = new TableLayoutPanel();
            btnRefresh = new Button();
            btnOk = new Button();
            btnCancel = new Button();

            rootLayout.SuspendLayout();
            btnLayout.SuspendLayout();
            SuspendLayout();

            // rootLayout
            rootLayout.ColumnCount = 1;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.Controls.Add(lblHeader, 0, 0);
            rootLayout.Controls.Add(lvProcesses, 0, 1);
            rootLayout.Controls.Add(btnLayout, 0, 2);
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.Padding = new Padding(10);
            rootLayout.RowCount = 3;
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));

            // lblHeader
            lblHeader.Dock = DockStyle.Fill;
            lblHeader.Text = "자동 종료 대상으로 추가할 프로세스를 선택하세요";
            lblHeader.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            lblHeader.TextAlign = ContentAlignment.MiddleLeft;

            // lvProcesses
            lvProcesses.Columns.AddRange(new ColumnHeader[] { colName, colMem, colCount });
            lvProcesses.Dock = DockStyle.Fill;
            lvProcesses.FullRowSelect = true;
            lvProcesses.MultiSelect = false;
            lvProcesses.View = View.Details;
            lvProcesses.Font = new Font("맑은 고딕", 10F);
            lvProcesses.DoubleClick += lvProcesses_DoubleClick;

            colName.Text = "프로세스 이름";
            colName.Width = 230;
            colMem.Text = "메모리 (MB)";
            colMem.Width = 120;
            colMem.TextAlign = HorizontalAlignment.Right;
            colCount.Text = "개수";
            colCount.Width = 60;
            colCount.TextAlign = HorizontalAlignment.Right;

            // btnLayout
            btnLayout.ColumnCount = 4;
            btnLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            btnLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            btnLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            btnLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            btnLayout.Controls.Add(btnRefresh, 1, 0);
            btnLayout.Controls.Add(btnOk, 2, 0);
            btnLayout.Controls.Add(btnCancel, 3, 0);
            btnLayout.Dock = DockStyle.Fill;
            btnLayout.Padding = new Padding(0, 8, 0, 0);
            btnLayout.RowCount = 1;
            btnLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            btnRefresh.Dock = DockStyle.Fill;
            btnRefresh.Text = "새로고침";
            btnRefresh.Click += btnRefresh_Click;
            btnRefresh.Margin = new Padding(3);

            btnOk.Dock = DockStyle.Fill;
            btnOk.Text = "선택";
            btnOk.Click += btnOk_Click;
            btnOk.Margin = new Padding(3);

            btnCancel.Dock = DockStyle.Fill;
            btnCancel.Text = "취소";
            btnCancel.Click += btnCancel_Click;
            btnCancel.Margin = new Padding(3);

            // Form
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(500, 500);
            Controls.Add(rootLayout);
            MinimumSize = new Size(420, 380);
            Name = "ProcessPickerForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "프로세스 선택";

            rootLayout.ResumeLayout(false);
            btnLayout.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel rootLayout;
        private Label lblHeader;
        private ListView lvProcesses;
        private ColumnHeader colName;
        private ColumnHeader colMem;
        private ColumnHeader colCount;
        private TableLayoutPanel btnLayout;
        private Button btnRefresh;
        private Button btnOk;
        private Button btnCancel;
    }
}
