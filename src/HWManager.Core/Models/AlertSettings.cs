namespace HWManager.Core.Models
{
    /// <summary>
    /// 알림 임계값 및 간격 설정
    /// </summary>
    public class AlertSettings
    {
        public float CpuThreshold { get; set; } = 90f;
        public float RamThreshold { get; set; } = 90f;
        public float GpuThreshold { get; set; } = 90f;
        public int AlertInterval { get; set; } = 60; // 초 단위
        public bool IsEnabled { get; set; } = true;
    }
}