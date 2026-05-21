using System;
using System.Drawing;
using System.Windows.Forms;

namespace HWManager.Client
{
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();
            ApplyModernStyle();
        }

        /// <summary>
        /// 현대적인 UI 스타일 적용
        /// </summary>
        private void ApplyModernStyle()
        {
            this.BackColor = Color.White;
            this.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point, 129);
        }

        /// <summary>
        /// 저장 버튼 클릭 이벤트
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 설정값 저장 로직 (팀원들이 구현)
                SaveSettings();

                MessageBox.Show("설정이 저장되었습니다.", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"설정 저장 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 취소 버튼 클릭 이벤트
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 설정값 저장 (팀원들이 구현할 부분)
        /// </summary>
        private void SaveSettings()
        {
            // TODO: 팀원들이 여기에 설정 저장 로직을 구현할 예정
            // 예: 데이터베이스 저장, 파일 저장, Properties.Settings 저장 등
        }

        /// <summary>
        /// 폼 로드 이벤트
        /// </summary>
        private void ConfigForm_Load(object sender, EventArgs e)
        {
            // TODO: 팀원들이 여기에 설정값 로드 로직을 구현할 예정
        }

        /// <summary>
        /// 폼 닫기 이벤트
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
        }
    }
}