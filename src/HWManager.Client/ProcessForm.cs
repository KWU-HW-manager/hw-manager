using HWManager.Core.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace HWManager.Client
{
    public partial class ProcessForm : Form
    {
        private ProcessService _processService = new ProcessService();
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
                    // [수정] PID(SubItems[1]) 대신 프로세스 이름(Text)을 가져옵니다.
                    string procName = lvProcesses.SelectedItems[0].Text;

                    if (MessageBox.Show($"{procName}와(과) 관련된 모든 프로세스를 종료하시겠습니까?",
                        "강제 종료", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        // 서비스의 새로운 '이름 기반 종료' 함수 호출
                        if (_processService.KillProcessesByName(procName))
                        {
                            MessageBox.Show("종료 명령을 전송했습니다.");
                            LoadProcesses(); // 목록 새로고침
                        }
                        else
                        {
                            MessageBox.Show("일부 프로세스를 종료하지 못했습니다.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("오류 발생: " + ex.Message);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e) => UpdateProcessList(txtSearch.Text);

    }
}