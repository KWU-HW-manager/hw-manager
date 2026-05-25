using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWManager.Core.Models
{
    // 개별 프로세스의 자원 점유율(메모리) 및 식별 정보 표준 데이터 모델

    public class ProcessInfo 
    {
        public int Id { get; set; }            // 프로세스 ID (PID)
        public string Name { get; set; }       // 프로세스 이름
        public double MemoryUsageMB { get; set; } // 메모리 사용량 (MB 단위)
    }
}
