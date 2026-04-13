using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace HWManager.Client
{
    public partial class ProcessForm : Form
    {
        private List<Process> _allProcesses = new List<Process>();

        public ProcessForm()
        {
            InitializeComponent();
            LoadProcesses();
        }

        private void btnRefreshLogs_Click(object sender, EventArgs e)
        {
            // DatabaseHelper.cs: GetLogs 호출
            DataTable dt = DatabaseHelper.GetLogs("TopProcess");
            dgvProcessLog.DataSource = dt;
            dgvProcessLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void LoadProcesses()
        {
            try
            {
                _allProcesses = Process.GetProcesses().OrderByDescending(p => p.WorkingSet64).ToList();
                UpdateProcessList(txtSearch.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"목록 로드 실패: {ex.Message}");
            }

            lvProcesses.BeginUpdate();
            lvProcesses.Items.Clear();
            foreach (Process p in _allProcesses)
            {
                try
                {
                    ListViewItem item = new ListViewItem(p.ProcessName);
                    item.SubItems.Add(p.Id.ToString());
                    long memUsage = p.WorkingSet64 / 1024 / 1024;
                    item.SubItems.Add($"{memUsage:N0} MB");
                    lvProcesses.Items.Add(item);
                }
                catch { continue; }
            }
            lvProcesses.EndUpdate();
            lblSummary.Text = $"총 프로세스: {_allProcesses.Count}개 | 메모리 점유 순 정렬 완료";
        }

        private void UpdateProcessList(string filter)
        {
            lvProcesses.BeginUpdate();
            lvProcesses.Items.Clear();
            var filtered = _allProcesses.Where(p => string.IsNullOrEmpty(filter) || p.ProcessName.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList();
            foreach (var p in filtered)
            {
                try
                {
                    ListViewItem item = new ListViewItem(p.ProcessName);
                    item.SubItems.Add(p.Id.ToString());
                    item.SubItems.Add($"{(p.WorkingSet64 / 1024 / 1024):N0} MB");
                    lvProcesses.Items.Add(item);
                }
                catch { continue; }
            }
            lvProcesses.EndUpdate();
            lblSummary.Text = $"검색 결과: {filtered.Count}개 / 전체: {_allProcesses.Count}개";
        }

        private void btnRefresh_Click(object sender, EventArgs e) => LoadProcesses();

        private void btnKill_Click(object sender, EventArgs e)
        {
            if (lvProcesses.SelectedItems.Count > 0)
            {
                try
                {
                    int pid = int.Parse(lvProcesses.SelectedItems[0].SubItems[1].Text);
                    Process target = Process.GetProcessById(pid);
                    target.Kill();
                    MessageBox.Show("종료되었습니다.");
                    LoadProcesses();
                }
                catch (Exception ex) { MessageBox.Show("종료할 수 없습니다: " + ex.Message); }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e) => UpdateProcessList(txtSearch.Text);

    }
}