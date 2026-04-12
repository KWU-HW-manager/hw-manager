using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWManager.Core.Models
{
    public class SystemSnapshot
    {
        public float CpuUsage { get; set; }     // CPU 사용량 (%)
        public double RamUsage { get; set; }    // RAM 사용량 (%)
        public float GpuUsage { get; set; }     // GPU 사용량 (%)
        public DateTime MeasuredAt { get; set; } = DateTime.Now; // 측정 시간
    }
}
