using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics; // 프로세스 제어용
using System.Linq; // 메모리 점유 순 정렬용

namespace HWManager.Client
{
    public partial class ProcessForm : Form
    {
        // 전체 프로세스 목록 캐시
        private List<Process> _allProcesses = new List<Process>();

        public ProcessForm()
        {
            InitializeComponent();
            LoadProcesses();
        }
        private void LoadProcesses()
        {
            try
            {
                // OS에서 최신 프로세스 목록 획득 및 메모리순 정렬
                _allProcesses = Process.GetProcesses()
                                       .OrderByDescending(p => p.WorkingSet64)
                                       .ToList();

                // 검색창이 비어있다면 전체 출력, 아니면 기존 검색어 유지
                UpdateProcessList(txtSearch.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"목록 로드 실패: {ex.Message}");
            }

            lvProcesses.BeginUpdate(); // 깜빡임 방지
            lvProcesses.Items.Clear();

            // 1. 실행 중인 프로세스를 가져와서 메모리 사용량(WorkingSet64) 내림차순으로 정렬
            var sortedProcesses = Process.GetProcesses()
                                         .OrderByDescending(p => p.WorkingSet64)
                                         .ToList();

            foreach (Process p in sortedProcesses)
            {
                try
                {
                    // 리스트뷰 아이템 생성
                    ListViewItem item = new ListViewItem(p.ProcessName);
                    item.SubItems.Add(p.Id.ToString());

                    // 메모리 사용량 계산 및 추가
                    long memUsage = p.WorkingSet64 / 1024 / 1024;
                    item.SubItems.Add($"{memUsage:N0} MB");

                    lvProcesses.Items.Add(item);
                }
                catch
                {
                    // 권한 없는 프로세스는 건너뜀
                    continue;
                }
            }

            lvProcesses.EndUpdate();

            // 2. 상단 요약 라벨 업데이트
            lblSummary.Text = $"총 프로세스: {sortedProcesses.Count}개 | 메모리 점유 순 정렬 완료";
        }

        private void UpdateProcessList(string filter)
        {
            lvProcesses.BeginUpdate(); // 깜빡임 방지
            lvProcesses.Items.Clear();

            // 이름에 검색어가 포함된 것만 추출 (대소문자 구분 X)
            var filtered = _allProcesses
                .Where(p => string.IsNullOrEmpty(filter) ||
                            p.ProcessName.Contains(filter, StringComparison.OrdinalIgnoreCase))
                .ToList();

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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadProcesses();
        }

        // 프로세스 종료 버튼
        private void btnKill_Click(object sender, EventArgs e)
        {
            if (lvProcesses.SelectedItems.Count > 0)
            {
                try
                {
                    // 선택된 항목의 ID 추출
                    int pid = int.Parse(lvProcesses.SelectedItems[0].SubItems[1].Text);
                    Process target = Process.GetProcessById(pid);

                    target.Kill(); // 프로세스 강제 종료
                    MessageBox.Show("종료되었습니다.");
                    LoadProcesses(); // 목록 갱신
                }
                catch (Exception ex)
                {
                    MessageBox.Show("종료할 수 없습니다: " + ex.Message);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateProcessList(txtSearch.Text);
        }
    }
}
