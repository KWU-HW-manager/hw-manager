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
        private const float ALERT_THRESHOLD = 90f;
        private List<AlertRecord> _alertHistory = new List<AlertRecord>();

        /// <summary>
        /// 알림 발생 이벤트
        /// </summary>
        public event EventHandler<AlertEventArgs> AlertTriggered;

        /// <summary>
        /// 사용량을 확인하고 임계값 초과 시 알림 발생
        /// </summary>
        public void CheckAndAlert(float cpuUsage, double ramUsage, float gpuUsage)
        {
            CheckResourceUsage("CPU", cpuUsage);
            CheckResourceUsage("RAM", (float)ramUsage);
            CheckResourceUsage("GPU", gpuUsage);
        }

        private void CheckResourceUsage(string resourceType, float usage)
        {
            if (usage >= ALERT_THRESHOLD)
            {
                var record = new AlertRecord
                {
                    ResourceType = resourceType,
                    UsagePercentage = usage,
                    AlertTime = DateTime.Now,
                    Details = $"{resourceType} 사용량이 {usage:F1}%에 도달했습니다."
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