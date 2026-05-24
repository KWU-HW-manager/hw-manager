using System;
using System.Drawing;
using System.Windows.Forms;
using HWManager.Core.Models;

namespace HWManager.Client
{
    public partial class ConfigForm : Form
    {
        private AlertSettings _alertSettings;
        private bool _overlayEnabled;
        private MainForm _mainForm;
        private bool _originalOverlayEnabled;
        private double _originalOpacity;
        private double _originalScale;

        public ConfigForm(MainForm mainForm = null)
        {
            InitializeComponent();
            _mainForm = mainForm;
            ApplyModernStyle();
            LoadSettings();
        }

        /// <summary>
        /// 현대적 UI 스타일 적용
        /// </summary>
        private void ApplyModernStyle()
        {
            this.BackColor = Color.White;
            this.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point, 129);

            var buttons = new[] { btnSave, btnCancel };
            foreach (var btn in buttons)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Cursor = Cursors.Hand;
                btn.MouseEnter += (s, e) => btn.BackColor = btn == btnSave
                    ? Color.FromArgb(65, 105, 165)
                    : Color.FromArgb(200, 200, 200);
                btn.MouseLeave += (s, e) => btn.BackColor = btn == btnSave
                    ? Color.FromArgb(79, 129, 189)
                    : Color.LightGray;
            }
        }

        /// <summary>
        /// 설정값 로드 (데이터베이스에서)
        /// </summary>
        private void LoadSettings()
        {
            try
            {
                // 데이터베이스에서 설정값 로드
                _alertSettings = DatabaseHelper.LoadAlertSettings();
                _overlayEnabled = DatabaseHelper.LoadOverlaySettings();

                // UI에 값 반영
                nudAlertCpu.Value = (decimal)_alertSettings.CpuThreshold;
                nudAlertRam.Value = (decimal)_alertSettings.RamThreshold;
                nudAlertGpu.Value = (decimal)_alertSettings.GpuThreshold;
                nudAlertInterval.Value = _alertSettings.AlertInterval;
                chkEnableAlert.Checked = _alertSettings.IsEnabled;
                chkEnableOverlay.Checked = _overlayEnabled;

                if (_mainForm != null)
                {
                    _originalOverlayEnabled = _mainForm.IsOverlayEnabled;
                    _originalOpacity = _mainForm.OverlayOpacity;
                    _originalScale = _mainForm.OverlayScale;
                    tbOpacity.Value = (int)(_mainForm.OverlayOpacity * 10);
                    tbScale.Value = (int)(_mainForm.OverlayScale * 10);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"설정 로드 중 오류가 발생했습니다: {ex.Message}", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 저장 버튼 클릭 이벤트
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 설정값 수집
                _alertSettings.CpuThreshold = (float)nudAlertCpu.Value;
                _alertSettings.RamThreshold = (float)nudAlertRam.Value;
                _alertSettings.GpuThreshold = (float)nudAlertGpu.Value;
                _alertSettings.AlertInterval = (int)nudAlertInterval.Value;
                _alertSettings.IsEnabled = chkEnableAlert.Checked;
                _overlayEnabled = chkEnableOverlay.Checked;

                SaveSettings();

                // MainForm의 AlertService 업데이트
                if (_mainForm != null)
                {
                    _mainForm.RefreshAlertSettings();
                    _mainForm.ApplyOverlaySettings();
                }

                MessageBox.Show("설정이 저장되었습니다.", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"설정 저장 중 오류가 발생했습니다: {ex.Message}", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 취소 버튼 클릭 이벤트
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_mainForm != null)
            {
                // 백업해뒀던 처음 값으로 원상복구
                _mainForm.IsOverlayEnabled = _originalOverlayEnabled;
                _mainForm.OverlayOpacity = _originalOpacity;
                _mainForm.OverlayScale = _originalScale;

                // 원상복구된 값을 오버레이에 즉시 반영
                _mainForm.ApplyOverlaySettings();
            }
            this.DialogResult = DialogResult.Cancel;
            this.Close(); ;
        }

        /// <summary>
        /// 알림 기능 체크 변경 이벤트
        /// </summary>
        private void chkEnableAlert_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = chkEnableAlert.Checked;
            nudAlertCpu.Enabled = enabled;
            nudAlertRam.Enabled = enabled;
            nudAlertGpu.Enabled = enabled;
            nudAlertInterval.Enabled = enabled;
            lblAlertCpu.Enabled = enabled;
            lblAlertRam.Enabled = enabled;
            lblAlertGpu.Enabled = enabled;
            lblAlertInterval.Enabled = enabled;
        }

        /// <summary>
        /// 설정값 저장 (데이터베이스에)
        /// </summary>
        private void SaveSettings()
        {
            DatabaseHelper.SaveAlertSettings(_alertSettings);
            DatabaseHelper.SaveOverlaySettings(_overlayEnabled);

            // 투명도, 크기도 함께 저장
            if (_mainForm != null)
            {
                DatabaseHelper.SaveOverlayVisuals(_mainForm.OverlayOpacity, _mainForm.OverlayScale);
            }
        }

        /// <summary>
        /// 폼 로드 이벤트
        /// </summary>
        private void ConfigForm_Load(object sender, EventArgs e)
        {
            // 추가 초기화 필요시 여기에 작성
        }

        /// <summary>
        /// 폼 닫기 이벤트
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
        }

        private void tbOpacity_Scroll(object sender, EventArgs e)
        {
            if (_mainForm != null)
            {
                // 슬라이더 값(2~10)을 소수점(0.2~1.0)으로 변환합니다.
                _mainForm.OverlayOpacity = tbOpacity.Value / 10.0;

                // 메인창에 오버레이 상태를 즉시 갱신하라고 명령합니다.
                _mainForm.ApplyOverlaySettings();
            }
        }

        private void tbScale_Scroll(object sender, EventArgs e)
        {
            if (_mainForm != null)
            {
                _mainForm.OverlayScale = tbScale.Value / 10.0;
                _mainForm.ApplyOverlaySettings(); // 즉시 반영
            }
        }

        private void chkEnableOverlay_CheckedChanged(object sender, EventArgs e)
        {
            if (_mainForm != null)
            {
                // 체크박스 상태를 메인창 변수에 실시간 동기화 후 즉시 반영
                _mainForm.IsOverlayEnabled = chkEnableOverlay.Checked;
                _mainForm.ApplyOverlaySettings();
            }
        }
    }
}