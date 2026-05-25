using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWManager.Core.Models
{
    // 시스템 전체 자원(CPU, RAM, GPU)의 실시간 사용량 메트릭 표준 데이터 모델

    public class SystemSnapshot
    {
        public float CpuUsage { get; set; }     // CPU 사용량 (%)
        public double RamUsage { get; set; }    // RAM 사용량 (%)
        public float GpuUsage { get; set; }     // GPU 사용량 (%)
        public DateTime MeasuredAt { get; set; } = DateTime.Now; // 측정 시간
    }
}
