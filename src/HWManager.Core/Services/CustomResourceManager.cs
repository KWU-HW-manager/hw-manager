using System;
using System.Collections.Generic;
using System.Linq;
using HWManager.Core.Models;

namespace HWManager.Core.Services
{
    // 커스텀 기반 자원 관리: 임계값/트리거 프로그램 조건을 감시해서
    // 사용자가 지정한 대상 프로세스를 자동으로 종료하는 로직
    public class CustomResourceManager
    {
        // 임계값 (%) - 해당 값 이상이면 자원 부족 상황으로 판단
        public float CpuThreshold { get; set; } = 80f;
        public float RamThreshold { get; set; } = 80f;
        public float GpuThreshold { get; set; } = 80f;

        // 자원 부족/트리거 발생 시 자동 종료할 프로세스 이름 목록
        public List<string> AutoKillTargets { get; } = new List<string>();

        // 이 프로그램들이 실행 중이면 "고사양 작업" 으로 간주, 즉시 정리
        public List<string> TriggerPrograms { get; } = new List<string>();

        // 자동 제어 기능 ON/OFF
        public bool Enabled { get; set; } = false;

        // 자동 종료 쿨다운.
        // 조건이 계속 유지되는 상황(CPU/RAM/GPU 임계값 초과 등)에서 매초 종료 시도가 반복되는 것을 막는다.
        public TimeSpan Cooldown { get; set; } = TimeSpan.FromSeconds(30);

        // 마지막으로 자동 종료를 시도한 시각.
        // 실제 종료 성공 여부와 관계없이 "종료 시도" 자체를 기준으로 쿨다운을 적용한다.
        private DateTime? _lastKillAttemptAt;

        // 이벤트: UI에 활동 로그를 실시간 전달 ("[13:02:11] Discord 자동 종료" 형식)
        public event Action<string>? LogGenerated;

        private readonly ProcessService _processService = new ProcessService();

        // Tick 한 번 = 조건 평가 한 번
        // 호출자 쪽에서 Timer로 주기적으로 호출해 준다.
        public void Evaluate(SystemSnapshot snapshot)
        {
            if (!Enabled) return;
            if (AutoKillTargets.Count == 0) return;

            string? reason = CheckTrigger(snapshot);
            if (reason == null) return;

            if (IsCooldownActive())
                return;

            _lastKillAttemptAt = DateTime.Now;
            KillTargets(reason);
        }

        // 마지막 종료 시도 후 Cooldown 시간이 지나지 않았으면 true.
        private bool IsCooldownActive()
        {
            if (_lastKillAttemptAt == null)
                return false;

            return DateTime.Now - _lastKillAttemptAt.Value < Cooldown;
        }

        // 조건을 만족하는 첫 번째 사유를 반환, 없으면 null
        private string? CheckTrigger(SystemSnapshot snapshot)
        {
            // 1. 트리거 프로그램 실행 감지
            foreach (string trigger in TriggerPrograms)
            {
                if (ProcessService.IsRunning(trigger))
                    return $"트리거 프로그램 '{trigger}' 실행 감지";
            }

            // 2. 임계값 초과 감지
            if (snapshot.CpuUsage >= CpuThreshold)
                return $"CPU 사용률 {snapshot.CpuUsage:F0}% (임계값 {CpuThreshold:F0}%) 초과";
            if (snapshot.RamUsage >= RamThreshold)
                return $"RAM 사용률 {snapshot.RamUsage:F0}% (임계값 {RamThreshold:F0}%) 초과";
            if (snapshot.GpuUsage >= GpuThreshold)
                return $"GPU 사용률 {snapshot.GpuUsage:F0}% (임계값 {GpuThreshold:F0}%) 초과";

            return null;
        }

        private void KillTargets(string reason)
        {
            Log($"[{DateTime.Now:HH:mm:ss}] 조건 충족 → {reason}");

            foreach (string target in AutoKillTargets.ToList())
            {
                if (!ProcessService.IsRunning(target))
                    continue;

                bool ok = _processService.KillProcessesByName(target);
                Log(ok
                    ? $"  └ '{target}' 종료 완료"
                    : $"  └ '{target}' 종료 실패");
            }
        }

        private void Log(string message) => LogGenerated?.Invoke(message);
    }
}
