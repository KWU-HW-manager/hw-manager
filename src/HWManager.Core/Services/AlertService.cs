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
        // 리소스별 임계값 분리
        private const float CPU_ALERT_THRESHOLD = 90f;
        private const float RAM_ALERT_THRESHOLD = 90f;
        private const float GPU_ALERT_THRESHOLD = 90f;

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
            CheckResourceUsage("CPU", cpuUsage, CPU_ALERT_THRESHOLD);
            CheckResourceUsage("RAM", (float)ramUsage, RAM_ALERT_THRESHOLD);
            CheckResourceUsage("GPU", gpuUsage, GPU_ALERT_THRESHOLD);
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

        /// <summary>
        /// 임계값 설정 (필요시 동적으로 변경 가능하도록)
        /// </summary>
        public void SetThresholds(float cpuThreshold, float ramThreshold, float gpuThreshold)
        {
            // 나중에 필드로 변경하면 사용 가능
            // 현재는 상수이므로 주석 처리
        }
    }
}