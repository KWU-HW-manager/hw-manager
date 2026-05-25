using System.Collections.Generic;

namespace HWManager.Core.Models
{
    /// <summary>
    /// 커스텀 자원 관리에서 저장/불러오기가 필요한 사용자 설정 값.
    /// </summary>
    public sealed class CustomResourceSettings
    {
        // 개인 설정 목록에 표시할 이름. 활성 설정 파일에서는 비어 있을 수 있다.
        public string ProfileName { get; set; } = string.Empty;

        // 자원 부족 판정에 사용하는 임계값. 저장소에서 로드할 때 10~100 범위로 보정한다.
        public float CpuThreshold { get; set; } = 80f;
        public float RamThreshold { get; set; } = 80f;
        public float GpuThreshold { get; set; } = 80f;

        // 창을 다시 열었을 때 자동 관리 활성화 상태를 복원하기 위한 값.
        public bool Enabled { get; set; }

        // 조건 충족 시 종료할 프로세스 이름 목록과, 실행되면 즉시 정리할 트리거 목록.
        public List<string> AutoKillTargets { get; set; } = new List<string>();
        public List<string> TriggerPrograms { get; set; } = new List<string>();
    }
}
