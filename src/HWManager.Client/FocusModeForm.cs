using System;
using System.Drawing;
using System.Windows.Forms;
using HWManager.Core.Models;
using HWManager.Core.Services;

namespace HWManager.Client
{
    public partial class FocusModeForm : Form
    {
        private readonly CustomResourceManager _manager = new CustomResourceManager();
        private readonly HardwareMonitorService _monitor = new HardwareMonitorService();
        private readonly ProcessService _processService = new ProcessService();
        private readonly System.Windows.Forms.Timer _watchTimer = new System.Windows.Forms.Timer();
        private readonly System.Windows.Forms.Timer _recommendTimer = new System.Windows.Forms.Timer();

        public FocusModeForm()
        {
            InitializeComponent();
            // 중요: VS 디자이너가 InitializeComponent 내부의 헬퍼 호출을 재생성 시 제거해버려
            // 버튼 Text/Click, 라벨 Text, TextBox Placeholder 등이 사라지는 문제가 반복 발생.
            // Designer.cs 에 의존하지 않고 런타임에 강제로 다시 적용한다.
            ConfigureRuntimeLayout();
            ApplyModernStyle();
            InitManualText();
            BindManagerEvents();
            InitWatchTimer();
            InitRecommendTimer();
            LoadRecommendations();
        }

        // 디자이너가 지워버리는 속성/이벤트를 코드비하인드에서 복구.
        // Designer.cs 가 어떻게 망가져도 이 메서드만 실행되면 UI 가 정상화됨.
        private void ConfigureRuntimeLayout()
        {
            // 임계값 라벨 및 단위
            SetupThresholdLabel(lblCpu, "CPU");
            SetupThresholdLabel(lblRam, "RAM");
            SetupThresholdLabel(lblGpu, "GPU");
            SetupThresholdUnit(lblCpuUnit);
            SetupThresholdUnit(lblRamUnit);
            SetupThresholdUnit(lblGpuUnit);

            // NumericUpDown (Dock=Fill 은 UpDownBase 특성상 위치 버그가 있어 Anchor 사용)
            SetupNud(nudCpu);
            SetupNud(nudRam);
            SetupNud(nudGpu);

            // 임계값 요약 라벨
            lblThresholdSummary.Dock = DockStyle.Fill;
            lblThresholdSummary.TextAlign = ContentAlignment.MiddleLeft;
            lblThresholdSummary.Font = new Font("맑은 고딕", 9F);
            lblThresholdSummary.ForeColor = Color.FromArgb(90, 90, 90);
            lblThresholdSummary.Text = "설정된 임계값 | CPU 80%  RAM 80%  GPU 80%";

            // 입력 상자
            SetupInput(txtKillInput, "프로세스 이름 (예: chrome)");
            SetupInput(txtTriggerInput, "트리거 프로그램 (예: valorant)");

            // ListBox — Dock/Font 복구
            SetupListBox(lstKill);
            SetupListBox(lstTrigger);

            // 컨트롤 패널 라벨/체크박스 (Text 는 디자이너가 유지해주지만 Dock 이 날아갈 수 있음)
            chkEnable.Dock = DockStyle.Fill;
            chkEnable.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            lblState.Dock = DockStyle.Fill;
            lblState.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            lblState.TextAlign = ContentAlignment.MiddleLeft;
            lblCurrentStatus.Dock = DockStyle.Fill;
            lblCurrentStatus.Font = new Font("맑은 고딕", 9.5F);
            lblCurrentStatus.TextAlign = ContentAlignment.MiddleLeft;

            // 버튼 (Text + AutoSize + Click) — Click 핸들러는 디자이너가 매번 날려먹음
            SetupButton(btnAddKill, "추가", btnAddKill_Click);
            SetupButton(btnRemoveKill, "제거", btnRemoveKill_Click);
            SetupButton(btnPickFromProcesses, "프로세스 목록에서 선택", btnPickFromProcesses_Click);
            SetupButton(btnAddTrigger, "추가", btnAddTrigger_Click);
            SetupButton(btnRemoveTrigger, "제거", btnRemoveTrigger_Click);
            SetupButton(btnRefreshRecommend, "새로고침", btnRefreshRecommend_Click);
            SetupButton(btnAddRecommend, "선택 항목을 대상에 추가", btnAddRecommend_Click);

            // btnAddRecommend 는 Percent 100% 칸에 들어가므로 가로로 가득 채워야 자연스러움.
            // 다른 버튼들은 AutoSize 로 텍스트만큼만 차지하고, 이 버튼만 Dock=Fill 로 전환.
            btnAddRecommend.AutoSize = false;
            btnAddRecommend.Dock = DockStyle.Fill;
            btnAddRecommend.MinimumSize = new Size(0, 40);
            btnAddRecommend.Margin = new Padding(3, 8, 3, 8);

            // ListView 컬럼 헤더 Text 가 유지되는지 방어
            colRecName.Text = "프로세스 이름";
            colRecMem.Text = "메모리 (MB)";
            lvRecommend.Dock = DockStyle.Fill;
            lvRecommend.Font = new Font("맑은 고딕", 10F);
            lvRecommend.FullRowSelect = true;
            lvRecommend.MultiSelect = true;
            lvRecommend.View = View.Details;
        }

        private static void SetupThresholdLabel(Label lbl, string text)
        {
            lbl.Text = text;
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            lbl.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        }

        private static void SetupThresholdUnit(Label lbl)
        {
            lbl.Text = "%";
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            lbl.Font = new Font("맑은 고딕", 10F);
        }

        private static void SetupNud(NumericUpDown nud)
        {
            nud.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            nud.Minimum = 10;
            nud.Maximum = 100;
            nud.Increment = 5;
            nud.TextAlign = HorizontalAlignment.Right;
            nud.Font = new Font("맑은 고딕", 10F);
            nud.Margin = new Padding(3, 4, 3, 4);
        }

        private static void SetupInput(TextBox tb, string placeholder)
        {
            tb.Dock = DockStyle.Fill;
            tb.Font = new Font("맑은 고딕", 10F);
            tb.PlaceholderText = placeholder;
            tb.Margin = new Padding(3, 4, 3, 3);
        }

        private static void SetupListBox(ListBox lb)
        {
            lb.Dock = DockStyle.Fill;
            lb.Font = new Font("맑은 고딕", 10F);
            lb.IntegralHeight = false;
        }

        // 멱등(idempotent) 핸들러 연결: 중복 등록을 막기 위해 -= 후 += 수행.
        // AutoSize 로 두면 텍스트가 길거나 폰트가 커져도 가로로 잘리지 않고
        // TableLayoutPanel 의 AutoSize 칸이 버튼 너비에 맞춰 늘어남.
        // 세로는 부모 행이 60px 절대값이므로 Dock=Fill 로 셀을 가득 채워 가운데 정렬 효과.
        private void SetupButton(Button btn, string text, EventHandler onClick)
        {
            btn.Text = text;
            btn.AutoSize = true;
            btn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btn.MinimumSize = new Size(80, 40);
            btn.Padding = new Padding(16, 6, 16, 6);
            btn.Margin = new Padding(3, 8, 3, 8);
            btn.Click -= onClick;
            btn.Click += onClick;
        }

        private void ApplyModernStyle()
        {
            BackColor = Color.FromArgb(243, 243, 243);

            var buttons = new[]
            {
                btnAddKill, btnRemoveKill, btnPickFromProcesses,
                btnAddTrigger, btnRemoveTrigger,
                btnRefreshRecommend, btnAddRecommend
            };
            foreach (var btn in buttons)
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

        private void InitManualText()
        {
            rtbManual.ReadOnly = true;
            rtbManual.BackColor = Color.White;
            rtbManual.BorderStyle = BorderStyle.FixedSingle;
            rtbManual.Font = new Font("맑은 고딕", 10);
            rtbManual.Text =
                "■ 커스텀 기반 자원 관리 사용법\r\n" +
                "\r\n" +
                "이 기능은 설정한 조건이 충족되면 지정된 프로세스를\r\n" +
                "자동으로 종료하여 시스템 자원을 확보해 줍니다.\r\n" +
                "\r\n" +
                "──────────────────────────────\r\n" +
                "▣ 설정 순서\r\n" +
                "──────────────────────────────\r\n" +
                "\r\n" +
                "1. 임계값 설정\r\n" +
                "   CPU / RAM / GPU 사용률 상한선을 정합니다.\r\n" +
                "   (기본값 80%)\r\n" +
                "\r\n" +
                "2. 자동 종료 대상 추가\r\n" +
                "   자원이 부족할 때 종료할 백그라운드\r\n" +
                "   프로세스를 추가합니다. (예: chrome, discord)\r\n" +
                "   ▸ 오른쪽 '자동 종료 추천' 목록에서 더블클릭\r\n" +
                "     또는 '선택 항목을 대상에 추가' 버튼으로\r\n" +
                "     쉽게 등록할 수 있습니다.\r\n" +
                "   ▸ '프로세스 목록에서 선택' 버튼으로\r\n" +
                "     현재 실행 중인 프로세스를 모두 볼 수 있습니다.\r\n" +
                "\r\n" +
                "3. 트리거 프로그램 추가 (선택 사항)\r\n" +
                "   이 프로그램이 실행되면 즉시 정리를 시작합니다.\r\n" +
                "   (예: 고사양 게임, 무거운 개발 도구)\r\n" +
                "\r\n" +
                "4. '자동 관리 활성화' 체크\r\n" +
                "   체크하면 1초마다 상태를 감시하여\r\n" +
                "   조건이 만족되면 자동 종료를 실행합니다.\r\n" +
                "\r\n" +
                "──────────────────────────────\r\n" +
                "▣ 조건 판정 규칙\r\n" +
                "──────────────────────────────\r\n" +
                "\r\n" +
                "다음 중 하나라도 충족되면 등록된 대상\r\n" +
                "프로세스를 모두 종료합니다.\r\n" +
                "\r\n" +
                " · 트리거 프로그램이 실행 중일 때\r\n" +
                " · CPU 사용률 ≥ 설정 임계값\r\n" +
                " · RAM 사용률 ≥ 설정 임계값\r\n" +
                " · GPU 사용률 ≥ 설정 임계값\r\n" +
                "\r\n" +
                "──────────────────────────────\r\n" +
                "▣ 주의사항\r\n" +
                "──────────────────────────────\r\n" +
                "\r\n" +
                " · 프로세스 이름은 '.exe' 없이 입력하세요.\r\n" +
                "   (chrome.exe → chrome)\r\n" +
                "\r\n" +
                " · 'System', 'explorer' 같은 시스템 프로세스는\r\n" +
                "   등록하지 마세요. 윈도우가 불안정해질 수 있습니다.\r\n" +
                "\r\n" +
                " · 아래 '활동 로그'에서 자동 종료 내역을\r\n" +
                "   실시간으로 확인할 수 있습니다.\r\n" +
                "\r\n" +
                " · 창을 닫으면 자동 관리는 중지됩니다.\r\n" +
                "\r\n" +
                "──────────────────────────────\r\n" +
                "▣ 자동 종료 추천\r\n" +
                "──────────────────────────────\r\n" +
                "\r\n" +
                "오른쪽 위 '자동 종료 추천' 목록은 현재 메모리를\r\n" +
                "많이 쓰는 프로세스 상위 10개를 5초마다\r\n" +
                "자동으로 새로고침합니다.\r\n" +
                "\r\n" +
                "윈도우 핵심 프로세스(explorer, svchost 등)는\r\n" +
                "자동으로 제외되므로 안전하게 선택하여\r\n" +
                "자동 종료 대상에 바로 추가할 수 있습니다.";
        }

        private void BindManagerEvents()
        {
            _manager.LogGenerated += msg =>
            {
                if (IsDisposed) return;
                BeginInvoke(new Action(() =>
                {
                    txtLog.AppendText(msg + Environment.NewLine);
                }));
            };
        }

        private void InitWatchTimer()
        {
            _watchTimer.Interval = 1000; // 1초 주기 감시
            _watchTimer.Tick += WatchTimer_Tick;
        }

        private void WatchTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                SystemSnapshot snap = _monitor.GetCurrentStatus();
                lblCurrentStatus.Text =
                    $"현재 상태 | CPU {snap.CpuUsage:F0}%  RAM {snap.RamUsage:F0}%  GPU {snap.GpuUsage:F0}%";
                _manager.Evaluate(snap);
            }
            catch { /* 감시 주기에서는 예외 무시 */ }
        }

        // --- 설정 컨트롤 이벤트 ---

        private void nudCpu_ValueChanged(object sender, EventArgs e)
        {
            _manager.CpuThreshold = (float)nudCpu.Value;
            UpdateThresholdSummary();
        }

        private void nudRam_ValueChanged(object sender, EventArgs e)
        {
            _manager.RamThreshold = (float)nudRam.Value;
            UpdateThresholdSummary();
        }

        private void nudGpu_ValueChanged(object sender, EventArgs e)
        {
            _manager.GpuThreshold = (float)nudGpu.Value;
            UpdateThresholdSummary();
        }

        private void UpdateThresholdSummary()
        {
            lblThresholdSummary.Text =
                $"설정된 임계값 | CPU {nudCpu.Value:F0}%  RAM {nudRam.Value:F0}%  GPU {nudGpu.Value:F0}%";
        }

        private void btnAddKill_Click(object sender, EventArgs e)
        {
            string name = ProcessService.NormalizeName(txtKillInput.Text);
            if (string.IsNullOrEmpty(name)) return;
            if (lstKill.Items.Contains(name)) return;

            lstKill.Items.Add(name);
            _manager.AutoKillTargets.Add(name);
            txtKillInput.Clear();
        }

        private void btnRemoveKill_Click(object sender, EventArgs e)
        {
            if (lstKill.SelectedItem is string selected)
            {
                lstKill.Items.Remove(selected);
                _manager.AutoKillTargets.Remove(selected);
            }
        }

        private void btnPickFromProcesses_Click(object sender, EventArgs e)
        {
            using var picker = new ProcessPickerForm();
            if (picker.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(picker.SelectedProcessName))
            {
                string name = picker.SelectedProcessName;
                if (!lstKill.Items.Contains(name))
                {
                    lstKill.Items.Add(name);
                    _manager.AutoKillTargets.Add(name);
                }
            }
        }

        private void btnAddTrigger_Click(object sender, EventArgs e)
        {
            string name = ProcessService.NormalizeName(txtTriggerInput.Text);
            if (string.IsNullOrEmpty(name)) return;
            if (lstTrigger.Items.Contains(name)) return;

            lstTrigger.Items.Add(name);
            _manager.TriggerPrograms.Add(name);
            txtTriggerInput.Clear();
        }

        private void btnRemoveTrigger_Click(object sender, EventArgs e)
        {
            if (lstTrigger.SelectedItem is string selected)
            {
                lstTrigger.Items.Remove(selected);
                _manager.TriggerPrograms.Remove(selected);
            }
        }

        private void chkEnable_CheckedChanged(object sender, EventArgs e)
        {
            _manager.Enabled = chkEnable.Checked;
            if (chkEnable.Checked)
            {
                if (_manager.AutoKillTargets.Count == 0)
                {
                    MessageBox.Show(this,
                        "자동 종료 대상이 비어 있습니다.\r\n대상을 먼저 추가해 주세요.",
                        "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    chkEnable.Checked = false;
                    return;
                }
                _watchTimer.Start();
                lblState.Text = "상태: 감시 중";
                lblState.ForeColor = Color.ForestGreen;
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] 자동 관리 시작" + Environment.NewLine);
            }
            else
            {
                _watchTimer.Stop();
                lblState.Text = "상태: 중지됨";
                lblState.ForeColor = Color.DimGray;
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] 자동 관리 중지" + Environment.NewLine);
            }
        }

        // --- 자동 종료 추천 ---

        private void InitRecommendTimer()
        {
            _recommendTimer.Interval = 5000; // 5초마다 추천 목록 재계산
            _recommendTimer.Tick += (s, e) => LoadRecommendations();
            _recommendTimer.Start();
        }

        private void LoadRecommendations()
        {
            try
            {
                var recommended = _processService.GetRecommendedTargets(10);

                // 선택 상태를 유지하기 위해 현재 선택된 이름들 기억
                var previouslySelected = lvRecommend.SelectedItems
                    .Cast<ListViewItem>()
                    .Select(i => i.Text)
                    .ToHashSet();

                lvRecommend.BeginUpdate();
                lvRecommend.Items.Clear();
                foreach (var r in recommended)
                {
                    var item = new ListViewItem(r.Name);
                    item.SubItems.Add($"{r.TotalMemoryMB:N0}");
                    if (previouslySelected.Contains(r.Name))
                        item.Selected = true;
                    lvRecommend.Items.Add(item);
                }
                lvRecommend.EndUpdate();
            }
            catch { /* 주기 호출이므로 예외 무시 */ }
        }

        private void btnRefreshRecommend_Click(object sender, EventArgs e) => LoadRecommendations();

        private void btnAddRecommend_Click(object sender, EventArgs e) => AddSelectedRecommendations();

        private void lvRecommend_DoubleClick(object sender, EventArgs e) => AddSelectedRecommendations();

        private void AddSelectedRecommendations()
        {
            if (lvRecommend.SelectedItems.Count == 0)
            {
                MessageBox.Show(this,
                    "추천 목록에서 종료 대상을 선택해 주세요.\r\n(Ctrl 또는 Shift 로 여러 개 선택 가능)",
                    "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int added = 0;
            foreach (ListViewItem item in lvRecommend.SelectedItems)
            {
                string name = item.Text;
                if (string.IsNullOrWhiteSpace(name)) continue;
                if (lstKill.Items.Contains(name)) continue;

                lstKill.Items.Add(name);
                _manager.AutoKillTargets.Add(name);
                added++;
            }

            if (added > 0)
            {
                txtLog.AppendText(
                    $"[{DateTime.Now:HH:mm:ss}] 추천 목록에서 {added}개 대상 추가" + Environment.NewLine);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _watchTimer.Stop();
            _recommendTimer.Stop();
            _monitor.Dispose();
            base.OnFormClosing(e);
        }

        private void btnAddKill_Click_1(object sender, EventArgs e)
        {

        }
    }
}
