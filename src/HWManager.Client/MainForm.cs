using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HWManager.Client
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            ApplyModernStyle();
        }
        private void ApplyModernStyle()
        {
            // 전체 배경색: 윈도우 11 느낌의 연한 그레이
            this.BackColor = Color.FromArgb(243, 243, 243);
            this.Text = "HWManager - Dashboard"; // 상단 타이틀 변경

            // 현재 폼에 있는 버튼들을 리스트에 담기 (사용자님의 버튼 이름에 맞췄습니다)
            var buttons = new List<Button> { btnMonitor, btnProcess, btnExit };

            foreach (var btn in buttons)
            {
                // 윈도우 11 카드 스타일 설정
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0; // 테두리 제거
                btn.BackColor = Color.White;      // 카드 배경은 흰색
                btn.Cursor = Cursors.Hand;        // 마우스 올리면 손가락 모양

                // 폰트 설정 (Pretendard가 없으면 맑은 고딕으로 자동 적용됩니다)
                btn.Font = new Font("맑은 고딕", 11, FontStyle.Bold);

                // 마우스 호버(Hover) 효과: 살짝 어두워지게
                btn.MouseEnter += (s, e) => { btn.BackColor = Color.FromArgb(235, 235, 235); };
                btn.MouseLeave += (s, e) => { btn.BackColor = Color.White; };
            }
        }
        private void btnMonitor_Click(object sender, EventArgs e)
        {
            MonitorForm monitor = new MonitorForm();
            monitor.Show(); // 새 창으로 열기
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            ProcessForm process = new ProcessForm();
            process.Show(); // 새 창으로 열기
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("프로그램을 종료하시겠습니까?", "종료", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
