using System;
using System.Collections.Generic;
using HWManager.Core.Models;

namespace HWManager.Core.Services
{
    /// <summary>
    /// 알림 이벤트 인자
    /// </summary>
    public class AlertEventArgs : EventArgs
    {
        public AlertRecord AlertRecord { get; set; }
    }

    public class AlertService
    {
        private const int ALERT_COOLDOWN_SECONDS = 60; // 고정된 중복 방지 시간
        private AlertSettings _settings;
        private List<AlertRecord> _alertHistory = new List<AlertRecord>();

        /// <summary>
        /// 알림 발생 이벤트
        /// </summary>
        public event EventHandler<AlertEventArgs> AlertTriggered;

        public AlertService(AlertSettings settings = null)
        {
            // 기본값 또는 외부에서 전달된 설정 사용
            _settings = settings ?? new AlertSettings();
        }

        /// <summary>
        /// 사용량을 확인하고 임계값 초과 시 알림 발생
        /// </summary>
        public void CheckAndAlert(float cpuUsage, double ramUsage, float gpuUsage)
        {
            CheckResourceUsage("CPU", cpuUsage, _settings.CpuThreshold);
            CheckResourceUsage("RAM", (float)ramUsage, _settings.RamThreshold);
            CheckResourceUsage("GPU", gpuUsage, _settings.GpuThreshold);
        }

        private void CheckResourceUsage(string resourceType, float usage, float threshold)
        {
            if (usage >= threshold)
            {
                var record = new AlertRecord
                {
                    ResourceType = resourceType,
                    UsagePercentage = usage,
                    AlertTime = DateTime.Now,
                    Details = $"{resourceType} 사용량이 {usage:F1}%에 도달했습니다. (임계값: {threshold}%)"
                };

                _alertHistory.Add(record);
                OnAlertTriggered(record);
            }
        }

        /// <summary>
        /// 알림 이벤트 발생
        /// </summary>
        protected virtual void OnAlertTriggered(AlertRecord record)
        {
            AlertTriggered?.Invoke(this, new AlertEventArgs { AlertRecord = record });
        }

        /// <summary>
        /// 임계값 설정 변경
        /// </summary>
        public void UpdateSettings(AlertSettings settings)
        {
            if (settings != null)
            {
                _settings = settings;
            }
        }

        /// <summary>
        /// 현재 설정 조회
        /// </summary>
        public AlertSettings GetSettings()
        {
            return _settings;
        }

        /// <summary>
        /// 알림 기록 조회
        /// </summary>
        public List<AlertRecord> GetAlertHistory()
        {
            return new List<AlertRecord>(_alertHistory);
        }

        /// <summary>
        /// 알림 기록 초기화
        /// </summary>
        public void ClearHistory()
        {
            _alertHistory.Clear();
        }
    }
}