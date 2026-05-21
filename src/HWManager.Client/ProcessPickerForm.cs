using System;
using System.Drawing;
using System.Windows.Forms;
using HWManager.Core.Services;

namespace HWManager.Client
{
    // FocusModeForm 에서 자동 종료 대상을 고를 때 띄우는 선택 대화상자.
    // 집계/정렬은 Core 의 ProcessService.GetProcessSummary() 에 위임.
    public partial class ProcessPickerForm : Form
    {
        private readonly ProcessService _processService = new ProcessService();
        public string? SelectedProcessName { get; private set; }

        public ProcessPickerForm()
        {
            InitializeComponent();
            ApplyModernStyle();
            LoadProcesses();
        }

        private void ApplyModernStyle()
        {
            BackColor = Color.FromArgb(243, 243, 243);

            foreach (var btn in new[] { btnOk, btnCancel, btnRefresh })
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = Color.White;
                btn.Cursor = Cursors.Hand;
                btn.Font = new Font("맑은 고딕", 9, FontStyle.Bold);
                btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(235, 235, 235);
                btn.MouseLeave += (s, e) => btn.BackColor = Color.White;
            }
        }

        private void LoadProcesses()
        {
            lvProcesses.BeginUpdate();
            lvProcesses.Items.Clear();

            foreach (var g in _processService.GetProcessSummary())
            {
                var item = new ListViewItem(g.Name);
                item.SubItems.Add($"{g.TotalMemoryMB:N0} MB");
                item.SubItems.Add(g.InstanceCount.ToString());
                lvProcesses.Items.Add(item);
            }
            lvProcesses.EndUpdate();
        }

        private void btnRefresh_Click(object sender, EventArgs e) => LoadProcesses();

        private void btnOk_Click(object sender, EventArgs e) => Confirm();

        private void lvProcesses_DoubleClick(object sender, EventArgs e) => Confirm();

        private void Confirm()
        {
            if (lvProcesses.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "프로세스를 선택해 주세요.", "알림",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SelectedProcessName = lvProcesses.SelectedItems[0].Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
