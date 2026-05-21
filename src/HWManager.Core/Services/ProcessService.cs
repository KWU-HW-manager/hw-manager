using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HWManager.Core.Models;

namespace HWManager.Core.Services
{
    public class ProcessService
    {
        // 자동 종료 추천에서 제외할 시스템/필수 프로세스.
        // 이들을 죽이면 윈도우가 불안정해지거나 블루스크린이 난다.
        private static readonly HashSet<string> SystemBlacklist =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "System", "Idle", "Registry", "svchost", "csrss", "smss",
                "wininit", "winlogon", "services", "lsass", "dwm",
                "explorer", "fontdrvhost", "LogonUI", "RuntimeBroker",
                "SearchHost", "SearchApp", "ShellExperienceHost",
                "StartMenuExperienceHost", "TextInputHost", "ctfmon",
                "sihost", "taskhostw", "WmiPrvSE", "spoolsv", "conhost",
                "dllhost", "audiodg", "MemCompression",
                "SecurityHealthService", "SecurityHealthSystray",
                "HWManager.Client"
            };

        // 프로세스 이름 정규화: 양끝 공백 제거, '.exe' 꼬리 제거
        public static string NormalizeName(string? raw)
        {
            string s = (raw ?? string.Empty).Trim();
            if (s.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                s = s.Substring(0, s.Length - 4);
            return s;
        }

        // 단일 프로세스 목록 (PID 단위). 기존 호환성 유지.
        public List<ProcessInfo> GetProcesses()
        {
            var list = new List<ProcessInfo>();
            foreach (var p in Process.GetProcesses())
            {
                try
                {
                    list.Add(new ProcessInfo
                    {
                        Id = p.Id,
                        Name = p.ProcessName,
                        MemoryUsageMB = p.WorkingSet64 / 1024.0 / 1024.0
                    });
                }
                catch { continue; }
            }
            return list.OrderByDescending(x => x.MemoryUsageMB).ToList();
        }

        // 이름별 집계: 같은 이름 프로세스들을 묶어서 메모리 합산 + 개수.
        // excludeSystem=true 이면 윈도우 핵심 프로세스는 결과에서 제외.
        public List<ProcessGroupInfo> GetProcessSummary(bool excludeSystem = false)
        {
            return Process.GetProcesses()
                .Where(p => !string.IsNullOrWhiteSpace(p.ProcessName))
                .Where(p => !excludeSystem || !SystemBlacklist.Contains(p.ProcessName))
                .GroupBy(p => p.ProcessName)
                .Select(BuildGroup)
                .OrderByDescending(g => g.TotalMemoryMB)
                .ToList();
        }

        // 자동 종료 대상 후보 추천: 시스템 제외 + 메모리 50MB 이상 + 상위 N개.
        public List<ProcessGroupInfo> GetRecommendedTargets(int top = 10)
        {
            return GetProcessSummary(excludeSystem: true)
                .Where(g => g.TotalMemoryMB >= 50.0)
                .Take(top)
                .ToList();
        }

        // 이름 기준으로 관련된 모든 프로세스(자식 포함) 종료
        public bool KillProcessesByName(string processName)
        {
            try
            {
                var targets = Process.GetProcessesByName(processName);
                foreach (var p in targets)
                {
                    try { p.Kill(true); }
                    catch { continue; }
                }
                return true;
            }
            catch { return false; }
        }

        // 해당 이름의 프로세스가 현재 실행 중인지 확인
        public static bool IsRunning(string processName)
        {
            try { return Process.GetProcessesByName(processName).Length > 0; }
            catch { return false; }
        }

        // --- 내부 구현 ---

        private static ProcessGroupInfo BuildGroup(IGrouping<string, Process> g)
        {
            double memMb = 0;
            int count = 0;
            foreach (var p in g)
            {
                try { memMb += p.WorkingSet64 / 1024.0 / 1024.0; count++; }
                catch { continue; }
            }
            return new ProcessGroupInfo
            {
                Name = g.Key,
                TotalMemoryMB = memMb,
                InstanceCount = count
            };
        }
    }
}
