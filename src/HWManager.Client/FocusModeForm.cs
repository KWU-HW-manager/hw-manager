using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HWManager.Core.Models;
using HWManager.Core.Services;

namespace HWManager.Client
{
    public partial class FocusModeForm : Form
    {
        private readonly CustomResourceManager _manager = new CustomResourceManager();
        private readonly CustomResourceSettingsStore _settingsStore = new CustomResourceSettingsStore();
        private readonly HardwareMonitorService _monitor = new HardwareMonitorService();
        private readonly ProcessService _processService = new ProcessService();
        private readonly System.Windows.Forms.Timer _watchTimer = new System.Windows.Forms.Timer();
        private readonly System.Windows.Forms.Timer _recommendTimer = new System.Windows.Forms.Timer();
        // 개인 설정 저장/불러오기 UI는 Designer가 아니라 런타임에 구성한다.
        // 디자이너가 복잡한 TableLayoutPanel 설정을 자주 깨뜨려서, 의도한 크기/이벤트를 코드로 고정한다.
        private GroupBox? _grpProfiles;
        private TextBox? _txtProfileName;
        private ComboBox? _cboProfiles;
        private Button? _btnSaveProfile;
        private Button? _btnLoadProfile;
        private Button? _btnRefreshProfiles;
        private Button? _btnDeleteProfile;
        private Button? _btnExportCurrent;
        private Button? _btnImportProfile;
        private bool _loadingSettings;

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
            LoadActiveSettings();
            RefreshProfileList();
            LoadRecommendations();
        }

        // 디자이너가 지워버리는 속성/이벤트를 코드비하인드에서 복구.
        // Designer.cs 가 어떻게 망가져도 이 메서드만 실행되면 UI 가 정상화됨.
        private void ConfigureRuntimeLayout()
        {
            rootLayout.AutoScroll = true;

            // DPI/폰트 배율이 다른 PC에서 버튼, 입력칸, 상태 라벨이 세로로 잘리는 문제를 방지한다.
            // Designer.cs의 절대 높이(60px 등)에만 의존하지 않고 런타임에 최소 높이를 보정한다.
            EnsureReadableRuntimeSizes();
            ConfigureProfileLayout();

            grpThreshold.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            grpKill.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            grpTrigger.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            grpRecommend.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);

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
            lblThresholdSummary.Font = new Font("맑은 고딕", 8.5F);
            lblThresholdSummary.ForeColor = Color.FromArgb(90, 90, 90);
            lblThresholdSummary.Text = "설정된 임계값 | CPU 80%  RAM 80%  GPU 80%";

            // 입력 상자
            SetupInput(txtKillInput, "프로세스 이름 (예: chrome)");
            SetupInput(txtTriggerInput, "트리거 프로그램 (예: notion)");

            // ListBox — Dock/Font 복구
            SetupListBox(lstKill);
            SetupListBox(lstTrigger);

            // 컨트롤 패널 라벨/체크박스 (Text 는 디자이너가 유지해주지만 Dock 이 날아갈 수 있음)
            chkEnable.Dock = DockStyle.Fill;
            chkEnable.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            lblState.Dock = DockStyle.Fill;
            lblState.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            lblState.TextAlign = ContentAlignment.MiddleLeft;
            lblCurrentStatus.Dock = DockStyle.Fill;
            lblCurrentStatus.Font = new Font("맑은 고딕", 9F);
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
            btnAddRecommend.MinimumSize = new Size(260, 44);
            btnAddRecommend.Margin = new Padding(3, 8, 3, 8);

            // ListView 컬럼 헤더 Text 가 유지되는지 방어
            colRecName.Text = "프로세스 이름";
            colRecMem.Text = "메모리 (MB)";
            lvRecommend.Dock = DockStyle.Fill;
            lvRecommend.Font = new Font("맑은 고딕", 9F);
            lvRecommend.FullRowSelect = true;
            lvRecommend.MultiSelect = true;
            lvRecommend.View = View.Details;
        }

        // 개인 설정 영역을 화면 최상단 전체 폭에 배치한다.
        // 오른쪽 패널 안에 넣으면 추천/도움말과 공간을 나눠 글씨가 잘렸기 때문에,
        // rootLayout을 2행 구조로 바꿔 상단은 저장/가져오기 전용, 하단은 기존 관리 UI로 사용한다.
        private void ConfigureProfileLayout()
        {
            if (_grpProfiles != null)
                return;

            rootLayout.SuspendLayout();

            // 상단 150px은 2줄짜리 프로필 UI가 DPI 125% 이상에서도 잘리지 않도록 확보한 고정 영역.
            rootLayout.RowStyles.Clear();
            rootLayout.RowCount = 2;
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // 기존 좌/우 본문 레이아웃은 새로 추가한 두 번째 행으로 내린다.
            rootLayout.SetColumn(settingsLayout, 0);
            rootLayout.SetRow(settingsLayout, 1);
            rootLayout.SetColumn(sideLayout, 1);
            rootLayout.SetRow(sideLayout, 1);

            _grpProfiles = new GroupBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("맑은 고딕", 9F, FontStyle.Bold),
                Margin = new Padding(3, 3, 3, 6),
                Text = "개인 설정 저장/불러오기"
            };

            // 2행 구성: 1행은 저장/내보내기, 2행은 불러오기/삭제/가져오기.
            // 버튼 영역을 넓게 잡아 한글 버튼 텍스트가 DPI 배율에 따라 잘리는 문제를 방지한다.
            var profileLayout = new TableLayoutPanel
            {
                ColumnCount = 3,
                Dock = DockStyle.Fill,
                Padding = new Padding(10, 8, 10, 8),
                RowCount = 2
            };
            profileLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 88F));
            profileLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            profileLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 620F));
            profileLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            profileLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            var lblSaveName = CreateProfileLabel("저장 이름");
            _txtProfileName = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Font = new Font("맑은 고딕", 9F),
                PlaceholderText = "예: 게임용, 작업용"
            };
            _btnSaveProfile = new Button();
            SetupProfileButton(_btnSaveProfile, "현재 설정 저장", btnSaveProfile_Click);

            var lblLoadName = CreateProfileLabel("저장 목록");
            _cboProfiles = new ComboBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("맑은 고딕", 9F)
            };
            _btnLoadProfile = new Button();
            _btnRefreshProfiles = new Button();
            _btnDeleteProfile = new Button();
            SetupProfileButton(_btnLoadProfile, "불러오기", btnLoadProfile_Click);
            SetupProfileButton(_btnRefreshProfiles, "목록 새로고침", btnRefreshProfiles_Click);
            SetupProfileButton(_btnDeleteProfile, "삭제", btnDeleteProfile_Click);

            _btnExportCurrent = new Button();
            _btnImportProfile = new Button();
            SetupProfileButton(_btnExportCurrent, "현재 설정 내보내기", btnExportCurrent_Click);
            SetupProfileButton(_btnImportProfile, "파일 가져오기", btnImportProfile_Click);

            profileLayout.Controls.Add(lblSaveName, 0, 0);
            profileLayout.Controls.Add(_txtProfileName, 1, 0);
            profileLayout.Controls.Add(CreateButtonFlow(_btnSaveProfile, _btnExportCurrent), 2, 0);
            profileLayout.Controls.Add(lblLoadName, 0, 1);
            profileLayout.Controls.Add(_cboProfiles, 1, 1);
            profileLayout.Controls.Add(CreateButtonFlow(_btnLoadProfile, _btnRefreshProfiles, _btnDeleteProfile, _btnImportProfile), 2, 1);

            _grpProfiles.Controls.Add(profileLayout);
            rootLayout.Controls.Add(_grpProfiles, 0, 0);
            rootLayout.SetColumnSpan(_grpProfiles, 2);
            rootLayout.ResumeLayout(false);
        }

        private static Label CreateProfileLabel(string text)
        {
            return new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("맑은 고딕", 9F),
                Text = text,
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        // 같은 셀에 여러 버튼을 가로로 놓기 위한 헬퍼.
        // TableLayoutPanel에 버튼을 직접 여러 개 넣으면 AutoSize 계산이 흔들려 FlowLayoutPanel로 묶는다.
        private static FlowLayoutPanel CreateButtonFlow(params Button[] buttons)
        {
            var flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0),
                Margin = new Padding(0),
                WrapContents = false
            };

            foreach (Button button in buttons)
                flow.Controls.Add(button);

            return flow;
        }

        // 개인 설정 영역 전용 버튼 크기 보정.
        // TextRenderer로 실제 한글 폭을 계산하고 여유 폭을 더해 다른 PC/DPI에서도 글씨가 잘리지 않게 한다.
        private static void SetupProfileButton(Button btn, string text, EventHandler onClick)
        {
            var buttonFont = new Font("맑은 고딕", 8.5F, FontStyle.Bold);
            Size textSize = TextRenderer.MeasureText(text, buttonFont);

            btn.Text = text;
            btn.Font = buttonFont;
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.UseCompatibleTextRendering = true;
            btn.AutoSize = false;
            btn.MinimumSize = new Size(Math.Max(112, textSize.Width + 36), 40);
            btn.Size = btn.MinimumSize;
            btn.Margin = new Padding(4, 5, 4, 5);
            btn.Padding = new Padding(10, 4, 10, 4);
            btn.Click -= onClick;
            btn.Click += onClick;
        }

        private void EnsureReadableRuntimeSizes()
        {
            // 입력칸 + 버튼이 들어가는 하단 행은 60px로는 125%/150% 배율에서 글자가 잘릴 수 있다.
            // 버튼 높이 44px + 위/아래 Margin을 안정적으로 수용하도록 82px로 넉넉하게 확보한다.
            if (killLayout.RowStyles.Count > 1)
                killLayout.RowStyles[1] = new RowStyle(SizeType.Absolute, 82F);

            if (triggerLayout.RowStyles.Count > 1)
                triggerLayout.RowStyles[1] = new RowStyle(SizeType.Absolute, 82F);

            if (recommendLayout.RowStyles.Count > 1)
                recommendLayout.RowStyles[1] = new RowStyle(SizeType.Absolute, 82F);

            // 활성화 체크/상태 표시 영역도 두 줄이 들어가므로 최소 높이를 보장한다.
            controlLayout.MinimumSize = new Size(0, 96);

            // 전체 창을 너무 작게 줄였을 때 글자가 먼저 잘리지 않고 스크롤이 생기도록 최소 크기를 조금 키운다.
            MinimumSize = new Size(1280, 860);
        }

        private static void SetupThresholdLabel(Label lbl, string text)
        {
            lbl.Text = text;
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            lbl.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        }

        private static void SetupThresholdUnit(Label lbl)
        {
            lbl.Text = "%";
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            lbl.Font = new Font("맑은 고딕", 9F);
        }

        private static void SetupNud(NumericUpDown nud)
        {
            nud.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            nud.Minimum = 10;
            nud.Maximum = 100;
            nud.Increment = 5;
            nud.TextAlign = HorizontalAlignment.Right;
            nud.Font = new Font("맑은 고딕", 9F);
            nud.Margin = new Padding(3, 2, 3, 2);
        }

        private static void SetupInput(TextBox tb, string placeholder)
        {
            tb.Dock = DockStyle.Fill;
            tb.Font = new Font("맑은 고딕", 9F);
            tb.PlaceholderText = placeholder;
            tb.Margin = new Padding(3, 3, 3, 3);
        }

        private static void SetupListBox(ListBox lb)
        {
            lb.Dock = DockStyle.Fill;
            lb.Font = new Font("맑은 고딕", 9F);
            lb.IntegralHeight = false;
        }

        // 멱등(idempotent) 핸들러 연결: 중복 등록을 막기 위해 -= 후 += 수행.
        // AutoSize 로 두면 텍스트가 길거나 폰트가 커져도 가로로 잘리지 않고
        // TableLayoutPanel 의 AutoSize 칸이 버튼 너비에 맞춰 늘어남.
        // 세로는 부모 행이 60px 절대값이므로 Dock=Fill 로 셀을 가득 채워 가운데 정렬 효과.
        private void SetupButton(Button btn, string text, EventHandler onClick)
        {
            var buttonFont = new Font("맑은 고딕", 9F, FontStyle.Bold);

            btn.Text = text;
            btn.Font = buttonFont;
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.UseCompatibleTextRendering = true;

            // AutoSize에 맡기면 DPI 배율/한글 렌더링 차이 때문에 버튼 내부 글자가 잘리는 경우가 있다.
            // 그래서 텍스트 폭을 직접 계산하고 여유 폭을 크게 더한 뒤 고정 크기로 배치한다.
            btn.AutoSize = false;
            Size textSize = TextRenderer.MeasureText(text, buttonFont);
            int width = Math.Max(92, textSize.Width + 56);
            btn.MinimumSize = new Size(width, 44);
            btn.Size = new Size(width, 44);
            btn.Padding = new Padding(14, 6, 14, 6);
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
                btnRefreshRecommend, btnAddRecommend,
                _btnSaveProfile, _btnLoadProfile, _btnRefreshProfiles,
                _btnDeleteProfile, _btnExportCurrent, _btnImportProfile
            }.Where(btn => btn != null).Cast<Button>();
            foreach (var btn in buttons)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = Color.White;
                btn.Cursor = Cursors.Hand;
                btn.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
                btn.TextAlign = ContentAlignment.MiddleCenter;
                btn.UseCompatibleTextRendering = true;
                btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(235, 235, 235);
                btn.MouseLeave += (s, e) => btn.BackColor = Color.White;
            }
        }

        private void InitManualText()
        {
            rtbManual.ReadOnly = true;
            rtbManual.BackColor = Color.White;
            rtbManual.BorderStyle = BorderStyle.FixedSingle;
            rtbManual.Font = new Font("맑은 고딕", 9.5F);
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

        // 화면 최초 진입 시 마지막으로 사용하던 커스텀 자원 관리 설정을 복원한다.
        // 실제 파일 접근/구버전 파일 호환 처리는 Core의 CustomResourceSettingsStore에 위임한다.
        private void LoadActiveSettings()
        {
            try
            {
                ApplySettingsToUi(_settingsStore.LoadActiveSettings());
            }
            catch (Exception ex)
            {
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] 설정 로드 실패: {ex.Message}" + Environment.NewLine);
            }
        }

        // 화면에서 값이 바뀔 때마다 현재 상태를 자동 저장한다.
        // _loadingSettings 중에는 UI 값 세팅으로 ValueChanged가 발생하므로 중복 저장을 막는다.
        private void SaveActiveSettings()
        {
            if (_loadingSettings)
                return;

            try
            {
                _settingsStore.SaveActiveSettings(BuildCurrentSettings());
            }
            catch (Exception ex)
            {
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] 설정 저장 실패: {ex.Message}" + Environment.NewLine);
            }
        }

        // UI 컨트롤 값을 Manager에 먼저 동기화한 뒤 저장용 DTO로 만든다.
        // 이렇게 하면 수동 입력/체크 상태와 Manager 내부 상태가 어긋나지 않는다.
        private CustomResourceSettings BuildCurrentSettings(string? profileName = null)
        {
            _manager.CpuThreshold = (float)nudCpu.Value;
            _manager.RamThreshold = (float)nudRam.Value;
            _manager.GpuThreshold = (float)nudGpu.Value;
            _manager.Enabled = chkEnable.Checked;
            return _manager.CreateSettings(profileName);
        }

        // 저장된 설정을 UI와 Manager 양쪽에 동시에 반영한다.
        // 자동 관리 활성화는 종료 대상이 있을 때만 복원해서 빈 목록 상태의 오동작을 막는다.
        private void ApplySettingsToUi(CustomResourceSettings settings)
        {
            _loadingSettings = true;

            try
            {
                _manager.ApplySettings(settings);

                nudCpu.Value = ClampToNumericRange(nudCpu, _manager.CpuThreshold);
                nudRam.Value = ClampToNumericRange(nudRam, _manager.RamThreshold);
                nudGpu.Value = ClampToNumericRange(nudGpu, _manager.GpuThreshold);

                lstKill.Items.Clear();
                foreach (string name in _manager.AutoKillTargets)
                    lstKill.Items.Add(name);

                lstTrigger.Items.Clear();
                foreach (string name in _manager.TriggerPrograms)
                    lstTrigger.Items.Add(name);

                UpdateThresholdSummary();

                bool canEnable = settings.Enabled && _manager.AutoKillTargets.Count > 0;
                chkEnable.Checked = canEnable;
                _manager.Enabled = canEnable;
            }
            finally
            {
                _loadingSettings = false;
            }
        }

        private static decimal ClampToNumericRange(NumericUpDown nud, float value)
        {
            decimal decimalValue = (decimal)value;
            if (decimalValue < nud.Minimum) return nud.Minimum;
            if (decimalValue > nud.Maximum) return nud.Maximum;
            return decimalValue;
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
            SaveActiveSettings();
        }

        private void nudRam_ValueChanged(object sender, EventArgs e)
        {
            _manager.RamThreshold = (float)nudRam.Value;
            UpdateThresholdSummary();
            SaveActiveSettings();
        }

        private void nudGpu_ValueChanged(object sender, EventArgs e)
        {
            _manager.GpuThreshold = (float)nudGpu.Value;
            UpdateThresholdSummary();
            SaveActiveSettings();
        }

        private void UpdateThresholdSummary()
        {
            lblThresholdSummary.Text =
                $"설정된 임계값 | CPU {nudCpu.Value:F0}%  RAM {nudRam.Value:F0}%  GPU {nudGpu.Value:F0}%";
        }

        // 프로필 콤보박스를 디스크의 실제 JSON 파일 목록과 다시 맞춘다.
        // 저장/삭제/가져오기 후에도 사용자가 보던 항목을 가능하면 다시 선택한다.
        private void RefreshProfileList(string? selectName = null)
        {
            if (_cboProfiles == null)
                return;

            string? current = selectName ?? _cboProfiles.SelectedItem as string;
            var names = _settingsStore.GetProfileNames();

            _cboProfiles.BeginUpdate();
            _cboProfiles.Items.Clear();
            foreach (string name in names)
                _cboProfiles.Items.Add(name);
            _cboProfiles.EndUpdate();

            if (!string.IsNullOrWhiteSpace(current) && names.Contains(current, StringComparer.OrdinalIgnoreCase))
                _cboProfiles.SelectedItem = names.First(name => string.Equals(name, current, StringComparison.OrdinalIgnoreCase));
            else if (_cboProfiles.Items.Count > 0)
                _cboProfiles.SelectedIndex = 0;
        }

        // 현재 화면 설정을 이름 있는 개인 설정으로 보관한다.
        // 이후 목록에서 선택해 불러오거나 다른 PC로 내보낼 수 있다.
        private void btnSaveProfile_Click(object? sender, EventArgs e)
        {
            try
            {
                string profileName = _settingsStore.SaveProfile(_txtProfileName?.Text ?? string.Empty, BuildCurrentSettings());
                RefreshProfileList(profileName);
                if (_txtProfileName != null)
                    _txtProfileName.Text = profileName;
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] 개인 설정 '{profileName}' 저장" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "개인 설정 저장 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 목록에서 선택한 개인 설정을 현재 화면에 적용하고 활성 설정으로도 저장한다.
        // 창을 닫았다가 열어도 방금 불러온 설정이 유지되도록 하기 위함이다.
        private void btnLoadProfile_Click(object? sender, EventArgs e)
        {
            if (_cboProfiles?.SelectedItem is not string profileName)
            {
                MessageBox.Show(this, "불러올 개인 설정을 선택해 주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                CustomResourceSettings settings = _settingsStore.LoadProfile(profileName);
                ApplySettingsToUi(settings);
                SaveActiveSettings();
                if (_txtProfileName != null)
                    _txtProfileName.Text = settings.ProfileName;
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] 개인 설정 '{profileName}' 불러오기" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "개인 설정 불러오기 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRefreshProfiles_Click(object? sender, EventArgs e) => RefreshProfileList();

        // 목록에서 더 이상 필요 없는 개인 설정 파일을 삭제한다.
        // 실수로 지우는 것을 막기 위해 삭제 전 확인 메시지를 띄운다.
        private void btnDeleteProfile_Click(object? sender, EventArgs e)
        {
            if (_cboProfiles?.SelectedItem is not string profileName)
            {
                MessageBox.Show(this, "삭제할 개인 설정을 선택해 주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(this,
                $"개인 설정 '{profileName}'을(를) 삭제할까요?",
                "개인 설정 삭제",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                _settingsStore.DeleteProfile(profileName);
                RefreshProfileList();
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] 개인 설정 '{profileName}' 삭제" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "개인 설정 삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 현재 설정을 사용자가 지정한 JSON 파일로 내보낸다.
        // 이 파일을 USB/메신저 등으로 다른 PC에 옮긴 뒤 '파일 가져오기'로 복원할 수 있다.
        private void btnExportCurrent_Click(object? sender, EventArgs e)
        {
            using var dialog = new SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = "json",
                FileName = "custom_resource_settings.json",
                Filter = "JSON 설정 파일 (*.json)|*.json|모든 파일 (*.*)|*.*",
                Title = "커스텀 자원 관리 설정 내보내기"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;

            try
            {
                string profileName = string.IsNullOrWhiteSpace(_txtProfileName?.Text) ? "" : _txtProfileName.Text.Trim();
                _settingsStore.ExportSettings(BuildCurrentSettings(profileName), dialog.FileName, profileName);
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] 설정 파일 내보내기 완료: {dialog.FileName}" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "설정 내보내기 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 다른 PC에서 가져온 JSON 설정 파일을 개인 설정으로 등록하고 즉시 현재 화면에 적용한다.
        // 가져온 뒤 활성 설정도 저장해서 다음 실행 때도 동일한 값이 유지된다.
        private void btnImportProfile_Click(object? sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = "JSON 설정 파일 (*.json)|*.json|모든 파일 (*.*)|*.*",
                Title = "커스텀 자원 관리 설정 가져오기"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;

            try
            {
                string profileName = _settingsStore.ImportProfile(dialog.FileName);
                CustomResourceSettings settings = _settingsStore.LoadProfile(profileName);
                ApplySettingsToUi(settings);
                SaveActiveSettings();
                RefreshProfileList(profileName);
                if (_txtProfileName != null)
                    _txtProfileName.Text = profileName;
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] 설정 파일 가져오기 완료: {profileName}" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "설정 가져오기 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAddKill_Click(object sender, EventArgs e)
        {
            string name = ProcessService.NormalizeName(txtKillInput.Text);
            if (string.IsNullOrEmpty(name)) return;
            if (lstKill.Items.Contains(name)) return;

            lstKill.Items.Add(name);
            _manager.AutoKillTargets.Add(name);
            txtKillInput.Clear();
            SaveActiveSettings();
        }

        private void btnRemoveKill_Click(object sender, EventArgs e)
        {
            if (lstKill.SelectedItem is string selected)
            {
                lstKill.Items.Remove(selected);
                _manager.AutoKillTargets.Remove(selected);
                SaveActiveSettings();
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
                    SaveActiveSettings();
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
            SaveActiveSettings();
        }

        private void btnRemoveTrigger_Click(object sender, EventArgs e)
        {
            if (lstTrigger.SelectedItem is string selected)
            {
                lstTrigger.Items.Remove(selected);
                _manager.TriggerPrograms.Remove(selected);
                SaveActiveSettings();
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

            SaveActiveSettings();
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
                SaveActiveSettings();
                txtLog.AppendText(
                    $"[{DateTime.Now:HH:mm:ss}] 추천 목록에서 {added}개 대상 추가" + Environment.NewLine);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveActiveSettings();
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
