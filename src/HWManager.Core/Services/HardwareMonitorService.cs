using System;
using System.Collections.Generic; // List를 쓰기 위해 필요
using System.Diagnostics;         // PerformanceCounter를 쓰기 위해 필요
using HWManager.Core.Models;      // SystemSnapshot을 쓰기 위해 필요


namespace HWManager.Core.Services
{
    public class HardwareMonitorService
    {
        private PerformanceCounter _cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private PerformanceCounter _ram = new PerformanceCounter("Memory", "% Committed Bytes In Use");
        private List<PerformanceCounter> _gpus = new List<PerformanceCounter>();

        public HardwareMonitorService()
        {
            InitGpuCounters();
        }

        private void InitGpuCounters()
        {
            try
            {
                var category = new PerformanceCounterCategory("GPU Engine");
                foreach (var instance in category.GetInstanceNames())
                {
                    if (instance.EndsWith("engtype_3D"))
                    {
                        _gpus.Add(new PerformanceCounter("GPU Engine", "Utilization Percentage", instance));
                    }
                }
            }
            catch { }
        }

        public SystemSnapshot GetCurrentStatus()
        {
            // CPU 수집
            float cpuVal = _cpu.NextValue();

            // RAM 수집
            float ramVal = _ram.NextValue();

            // GPU 수집 (기존 합산 방식)
            float gpuVal = 0;
            foreach (var g in _gpus)
            {
                try
                {
                    float currentVal = g.NextValue();

                    // 여러 엔진 중 가장 높은 사용량 하나만 선택 (합산 방지)
                    if (currentVal > gpuVal)
                    {
                        gpuVal = currentVal;
                    }
                }
                catch { }
            }

            return new SystemSnapshot
            {
                CpuUsage = cpuVal,
                RamUsage = (double)ramVal,
                GpuUsage = gpuVal,
                MeasuredAt = DateTime.Now
            };
        }
    }
}