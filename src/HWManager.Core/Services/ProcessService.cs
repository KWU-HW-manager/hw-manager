using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using HWManager.Core.Models;

namespace HWManager.Core.Services
{
    public class ProcessService
    {
        // 프로세스 목록 가져오기 및 메모리순 정렬
        public List<ProcessInfo> GetProcesses()
        {
            var list = new List<ProcessInfo>();
            var allProcs = Process.GetProcesses();

            foreach (var p in allProcs)
            {
                try
                {
                    list.Add(new ProcessInfo
                    {
                        Id = p.Id,
                        Name = p.ProcessName,
                        // MB 단위로 변환하여 저장
                        MemoryUsageMB = p.WorkingSet64 / 1024.0 / 1024.0
                    });
                }
                catch { continue; } // 접근 권한 없는 프로세스 무시
            }

            return list.OrderByDescending(x => x.MemoryUsageMB).ToList();
        }

        // 이름으로 관련된 모든 프로세스 종료
        public bool KillProcessesByName(string processName)
        {
            try
            {
                // 1. 해당 이름을 가진 모든 프로세스 찾기 (예: "Discord")
                var targets = Process.GetProcessesByName(processName);

                foreach (var p in targets)
                {
                    try
                    {
                        // 2. 각 프로세스와 그 자식들까지 싹 다 종료
                        p.Kill(true);
                    }
                    catch { continue; }
                }
                return true;
            }
            catch { return false; }
        }
    }
}