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
        private DateTime _lastUpdate = DateTime.MinValue;

        public HardwareMonitorService()
        {
            InitGpuCounters();
        }

        private void InitGpuCounters()
        {
            try
            {
                foreach (var g in _gpus) g.Dispose();
                _gpus.Clear();

                var category = new PerformanceCounterCategory("GPU Engine");
                foreach (var instance in category.GetInstanceNames())
                {
                    if (instance.Contains("engtype_3D"))
                    {
                        var counter = new PerformanceCounter("GPU Engine", "Utilization Percentage", instance);

                        // [핵심] 생성 직후 한 번 호출해서 '이전 시점' 데이터를 만들어둡니다.
                        // 이렇게 하면 GetCurrentStatus에서 호출할 때 바로 실제 값이 나옵니다.
                        try { counter.NextValue(); } catch { }

                        _gpus.Add(counter);
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
                    if ((DateTime.Now - _lastUpdate).TotalSeconds > 5)
                    {
                        InitGpuCounters();
                        _lastUpdate = DateTime.Now;
                    }

                    float currentVal = g.NextValue();

                    // [핵심 로직] 값이 0보다 크고 100 이하인 "정상적인 백분율"만 취합니다.
                    // 노트북에서 수백만이 찍히는 '가짜 값'은 여기서 걸러집니다.
                    if (currentVal > 0 && currentVal <= 100)
                    {
                        if (currentVal > gpuVal)
                        {
                            gpuVal = currentVal;
                        }
                    }
                }
                catch { }
            }

            if (gpuVal > 100) gpuVal = 100;
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