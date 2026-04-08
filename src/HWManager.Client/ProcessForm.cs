using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using HWManager.Core.Models;
using HWManager.Core.Services;

namespace HWManager.Client
{
    public partial class ProcessForm : Form
    {
        private ProcessService _processService = new ProcessService();
        private List<ProcessInfo> _allProcesses = new List<ProcessInfo>();

        public ProcessForm()
        {
            InitializeComponent();
            LoadProcesses();
        }

        // 데이터 로드 및 전체 캐시 업데이트
        private void LoadProcesses()
        {
            _allProcesses = _processService.GetProcesses();
            UpdateProcessList(txtSearch.Text);
        }

        // 리스트뷰 화면 업데이트 (필터링 포함)
        private void UpdateProcessList(string filter)
        {
            lvProcesses.BeginUpdate();
            lvProcesses.Items.Clear();

            var filtered = _allProcesses
                .Where(p => string.IsNullOrEmpty(filter) ||
                            p.Name.Contains(filter, StringComparison.OrdinalIgnoreCase))
                .ToList();

            foreach (var p in filtered)
            {
                ListViewItem item = new ListViewItem(p.Name);
                item.SubItems.Add(p.Id.ToString());
                item.SubItems.Add($"{p.MemoryUsageMB:N0} MB");
                lvProcesses.Items.Add(item);
            }

            lvProcesses.EndUpdate();
            lblSummary.Text = $"검색 결과: {filtered.Count}개 / 전체: {_allProcesses.Count}개";
        }

        private void btnRefresh_Click(object sender, EventArgs e) => LoadProcesses();

        private void btnKill_Click(object sender, EventArgs e)
        {
            if (lvProcesses.SelectedItems.Count == 0) return;

            int pid = int.Parse(lvProcesses.SelectedItems[0].SubItems[1].Text);
            string name = lvProcesses.SelectedItems[0].Text;

            if (MessageBox.Show($"{name}를 종료하시겠습니까?", "종료 확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (_processService.KillProcess(pid))
                {
                    MessageBox.Show("종료되었습니다.");
                    LoadProcesses();
                }
                else
                {
                    MessageBox.Show("종료 실패 (권한 부족)");
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            UpdateProcessList(txtSearch.Text);
        }
    }
}