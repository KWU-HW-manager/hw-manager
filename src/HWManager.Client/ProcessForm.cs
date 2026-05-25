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

        // 현재 구동 중인 모든 프로세스를 가져와 메모리 점유율이 높은 순으로 정렬
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

        // 검색 필터링 및 바이트 단위를 MB로 변환하여 리스트뷰에 갱신
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
                catch { continue; } // 시스템 보안상 접근 거부된 프로세스는 패스
            }
            lvProcesses.EndUpdate();
            lblSummary.Text = $"검색 결과: {filtered.Count}개 / 전체: {_allProcesses.Count}개";
        }

        // 목록 수동 새로고침
        private void btnRefresh_Click(object sender, EventArgs e) => LoadProcesses();

        // 선택한 프로세스명을 기준으로 동일 이름을 가진 모든 프로세스 일괄 강제 종료
        private void btnKill_Click(object sender, EventArgs e)
        {
            if (lvProcesses.SelectedItems.Count > 0)
            {
                try
                {
                    string procName = lvProcesses.SelectedItems[0].Text;

                    if (MessageBox.Show($"{procName}와(과) 관련된 모든 프로세스를 종료하시겠습니까?",
                        "강제 종료", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        // 서비스 레이어의 이름 기반 종료 함수 호출
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

        // 검색창 입력 내용 변경 시 실시간 리스트 갱신
        private void textBox1_TextChanged(object sender, EventArgs e) => UpdateProcessList(txtSearch.Text);

    }
}