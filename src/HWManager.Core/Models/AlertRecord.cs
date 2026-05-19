using System;

namespace HWManager.Core.Models
{
    /// <summary>
    /// 90% 이상 사용량 초과 시 기록되는 알림 정보
    /// </summary>
    public class AlertRecord
    {
        public int Id { get; set; }
        public string ResourceType { get; set; } // "CPU", "RAM", "GPU"
        public float UsagePercentage { get; set; } // 실제 사용량 (%)
        public DateTime AlertTime { get; set; } = DateTime.Now;
        public string Details { get; set; } // 추가 정보
    }
}