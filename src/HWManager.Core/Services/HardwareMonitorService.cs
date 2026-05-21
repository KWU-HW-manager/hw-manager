using System;
using System.Linq;
using LibreHardwareMonitor.Hardware;
using HWManager.Core.Models;

namespace HWManager.Core.Services
{
    public class HardwareMonitorService : IDisposable
    {
        private static readonly object SyncRoot = new object();
        private static Computer? _computer;
        private static int _instanceCount;

        private bool _disposed;

        public HardwareMonitorService()
        {
            lock (SyncRoot)
            {
                _instanceCount++;
            }
        }

        public SystemSnapshot GetCurrentStatus()
        {
            if (_disposed)
                return CreateEmptySnapshot();

            lock (SyncRoot)
            {
                Computer? computer = EnsureComputerOpened();
                if (computer == null)
                    return CreateEmptySnapshot();

                float cpu = 0;
                float ram = 0;
                float gpu = 0;
                bool updateFailed = false;

                foreach (IHardware hardware in computer.Hardware.ToArray())
                {
                    try
                    {
                        hardware.Update();
                    }
                    catch
                    {
                        // 일부 PC에서는 기능 화면을 닫은 뒤 이전 하드웨어 핸들이 일시적으로
                        // 실패할 수 있다. 한 장치 업데이트 실패가 전체 프로그램 종료로 이어지지 않게 건너뛴다.
                        updateFailed = true;
                        continue;
                    }

                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType != SensorType.Load)
                            continue;

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

                if (updateFailed && cpu == 0 && ram == 0 && gpu == 0)
                {
                    CloseComputer();
                }

                return new SystemSnapshot
                {
                    CpuUsage = cpu,
                    RamUsage = ram,
                    GpuUsage = gpu,
                    MeasuredAt = DateTime.Now
                };
            }
        }

        private static Computer? EnsureComputerOpened()
        {
            if (_computer != null)
                return _computer;

            try
            {
                _computer = new Computer
                {
                    IsCpuEnabled = true,
                    IsMemoryEnabled = true,
                    IsGpuEnabled = true
                };
                _computer.Open();
                return _computer;
            }
            catch
            {
                _computer = null;
                return null;
            }
        }

        private static SystemSnapshot CreateEmptySnapshot() => new SystemSnapshot
        {
            CpuUsage = 0,
            RamUsage = 0,
            GpuUsage = 0,
            MeasuredAt = DateTime.Now
        };

        private static void CloseComputer()
        {
            try
            {
                _computer?.Close();
            }
            catch
            {
                // 종료 중 하드웨어 핸들 정리 실패는 무시
            }
            finally
            {
                _computer = null;
            }
        }

        public void Dispose()
        {
            lock (SyncRoot)
            {
                if (_disposed)
                    return;

                _disposed = true;
                _instanceCount--;

                if (_instanceCount > 0)
                    return;

                _instanceCount = 0;
                CloseComputer();
            }
        }
    }
}
