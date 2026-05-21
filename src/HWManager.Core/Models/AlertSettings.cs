namespace HWManager.Core.Models
{
    /// <summary>
    /// 알림 임계값 설정
    /// !!!중요: 포커스모드의 알림 임계값과 알림 시스템 임계값을 연동 시킬 예정이므로 추후 사용하지 않을 예정
    /// </summary>
    public class AlertSettings
    {
        public float CpuThreshold { get; set; } = 90f;
        public float RamThreshold { get; set; } = 90f;
        public float GpuThreshold { get; set; } = 90f;
    }
}