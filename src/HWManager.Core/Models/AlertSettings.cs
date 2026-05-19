namespace HWManager.Core.Models
{
    /// <summary>
    /// 알림 임계값 설정
    /// </summary>
    public class AlertSettings
    {
        public float CpuThreshold { get; set; } = 90f;
        public float RamThreshold { get; set; } = 90f;
        public float GpuThreshold { get; set; } = 90f;
    }
}