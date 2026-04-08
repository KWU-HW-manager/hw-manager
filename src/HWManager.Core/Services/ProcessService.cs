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

        // 프로세스 종료
        public bool KillProcess(int pid)
        {
            try
            {
                var target = Process.GetProcessById(pid);
                target.Kill();
                return true;
            }
            catch { return false; }
        }
    }
}