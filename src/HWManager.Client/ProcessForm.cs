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

namespace HWManager.Client
{
    public partial class ProcessForm : Form
    {
        public ProcessForm()
        {
            InitializeComponent();
            LoadProcesses();
        }
        private void LoadProcesses()
        {
            lvProcesses.Items.Clear(); // 기존 목록 삭제
            Process[] processes = Process.GetProcesses(); // 실행 중인 전체 프로세스 획득

            foreach (Process p in processes)
            {
                // 리스트뷰 아이템 생성 (이름, ID)
                ListViewItem item = new ListViewItem(p.ProcessName);
                item.SubItems.Add(p.Id.ToString());
                lvProcesses.Items.Add(item);
            }
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
        
        private void ProcessForm_Load(object sender, EventArgs e)
        {

        }
    }
}
