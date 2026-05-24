using System.Collections.Generic;

namespace HWManager.Client
{
    /// <summary>
    /// 커스텀 자원 관리 화면의 사용자 설정을 JSON으로 저장하기 위한 DTO.
    ///
    /// Form 컨트롤이나 CustomResourceManager를 그대로 직렬화하지 않고,
    /// 실제로 저장이 필요한 값만 분리해서 보관한다.
    /// </summary>
    internal sealed class FocusModeSettings
    {
        public float CpuThreshold { get; set; } = 80f;
        public float RamThreshold { get; set; } = 80f;
        public float GpuThreshold { get; set; } = 80f;
        public bool Enabled { get; set; }
        public List<string> AutoKillTargets { get; set; } = new List<string>();
        public List<string> TriggerPrograms { get; set; } = new List<string>();
    }
}
