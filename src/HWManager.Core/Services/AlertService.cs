using System;
using System.Collections.Generic;
using HWManager.Core.Models;

namespace HWManager.Core.Services
{
    /// <summary>
    /// 알림 이벤트 args
    /// </summary>
    public class AlertEventArgs : EventArgs
    {
        public AlertRecord AlertRecord { get; set; }
    }

    public class AlertService
    {
        private AlertSettings _settings;
        private List<AlertRecord> _alertHistory = new List<AlertRecord>();
        private Dictionary<string, DateTime> _lastAlertTime = new Dictionary<string, DateTime>();

        /// <summary>
        /// 알림 발생 이벤트
        /// </summary>
        public event EventHandler<AlertEventArgs> AlertTriggered;

        public AlertService(AlertSettings settings = null)
        {
            // 기본값 또는 외부에서 주입된 설정 사용
            _settings = settings ?? new AlertSettings();
        }

        /// <summary>
        /// 자원량을 확인하고 임계값 넘으면 알림 발생
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
                // 알림 간격 확인 (쿨다운 적용)
                string alertKey = resourceType;
                if (_lastAlertTime.ContainsKey(alertKey))
                {
                    var timeSinceLastAlert = DateTime.Now - _lastAlertTime[alertKey];
                    if (timeSinceLastAlert.TotalSeconds < _settings.AlertInterval)
                    {
                        return;
                    }
                }

                _lastAlertTime[alertKey] = DateTime.Now;

                var record = new AlertRecord
                {
                    ResourceType = resourceType,
                    UsagePercentage = usage,
                    AlertTime = DateTime.Now,
                    Details = $"{resourceType} 사용률이 {usage:F1}%에 도달했습니다. (임계값: {threshold}%)"
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
        /// 임계값 설정 업데이트
        /// </summary>
        public void UpdateSettings(AlertSettings settings)
        {
            if (settings != null)
            {
                _settings = settings;
            }
        }

        /// <summary>
        /// 현재 설정 반환
        /// </summary>
        public AlertSettings GetSettings()
        {
            return _settings;
        }

        /// <summary>
        /// 알림 기록 반환
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
            _lastAlertTime.Clear();
        }
    }
}