using System;
using System.Linq;
using LibreHardwareMonitor.Hardware; // 라이브러리 추가
using HWManager.Core.Models;

namespace HWManager.Core.Services
{
    public class HardwareMonitorService
    {
        private Computer _computer;

        public HardwareMonitorService()
        {
            // 하드웨어 모니터 객체 초기화
            _computer = new Computer
            {
                IsCpuEnabled = true,
                IsMemoryEnabled = true,
                IsGpuEnabled = true
            };
            _computer.Open();
        }

        public SystemSnapshot GetCurrentStatus()
        {
            float cpu = 0;
            float ram = 0;
            float gpu = 0;

            foreach (IHardware hardware in _computer.Hardware)
            {
                hardware.Update();

                foreach (ISensor sensor in hardware.Sensors)
                {

                    if (sensor.SensorType == SensorType.Load)
                    {
                        float val = sensor.Value ?? 0;

                        if (hardware.HardwareType == HardwareType.Cpu && sensor.Name.Contains("Total"))
                        {
                            cpu = val;
                        }

                        if (hardware.HardwareType == HardwareType.Memory)
                        {
                            ram = val;
                        }

                        if (hardware.HardwareType.ToString().Contains("Gpu"))
                        {

                            if (sensor.Name.Contains("GPU Core") || sensor.Name.Contains("D3D") || sensor.Name.Contains("3D"))
                            {

                                if (val >= 0 && val <= 100)
                                {

                                    if (val > gpu) gpu = val;
                                }
                            }
                        }
                    }
                }
            }

            return new SystemSnapshot
            {
                CpuUsage = cpu,
                RamUsage = (double)ram,
                GpuUsage = gpu,
                MeasuredAt = DateTime.Now
            };
        }

        public void Dispose() => _computer.Close(); // 리소스 해제
    }
} 