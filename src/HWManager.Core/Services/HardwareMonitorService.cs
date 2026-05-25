using System;
using System.Linq;
using LibreHardwareMonitor.Hardware;
using HWManager.Core.Models;

namespace HWManager.Core.Services
{
    /// <summary>
    /// LibreHardwareMonitor 라이브러리를 연동하여 시스템 커널 및 센서로부터 
    /// CPU, RAM, GPU 사용량 데이터를 실시간으로 채굴해 내는 백엔드 핵심 서비스 엔진
    /// </summary>
    public class HardwareMonitorService : IDisposable
    {
        // 멀티스레드 환경에서 안전하게 하드웨어 자원에 접근하기 위한 동기화 객체
        private static readonly object SyncRoot = new object();

        // 라이브러리의 핵심 컴퓨터 컨트롤러 객체 (정적 싱글톤 구조로 공유)
        private static Computer? _computer;

        // 현재 이 서비스를 생성하여 사용 중인 인스턴스 참조 카운트
        private static int _instanceCount;

        private bool _disposed;

        public HardwareMonitorService()
        {
            lock (SyncRoot)
            {
                _instanceCount++; // 인스턴스가 생성될 때마다 카운트 증가
            }
        }

        /// <summary>
        /// 하드웨어 센서 데이터를 최신 상태로 업데이트한 후 규격화된 시스템 상태 스냅샷을 반환
        /// </summary>
        public SystemSnapshot GetCurrentStatus()
        {
            // 객체가 이미 해제(Dispose)되었다면 자원에 접근하지 않고 안전하게 빈 상자 반환
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
                bool updateFailed = false; // 센서 읽기 실패 플래그

                // 컴퓨터에 장착된 하드웨어 리스트(CPU, RAM, GPU 등)를 배열로 복사하여 순회
                foreach (IHardware hardware in computer.Hardware.ToArray())
                {
                    try
                    {
                        hardware.Update();
                    }
                    catch
                    {
                        // 백신 프로그램의 차단이나 절전 모드 진입 등으로 센서 읽기가 일시 거부될 때 튕김 방지
                        updateFailed = true;
                        continue;
                    }

                    // 해당 부품이 가지고 있는 수많은 센서 스트림(온도, 전압, 클럭, 부하율 등)을 순회
                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        // 오직 '부하율(Load, %)' 타입의 센서 데이터만 골라내고 나머지는 패스
                        if (sensor.SensorType != SensorType.Load)
                            continue;

                        // 센서 값이 null이면 0으로 안전하게 대체
                        float val = sensor.Value ?? 0;

                        if (hardware.HardwareType == HardwareType.Cpu && sensor.Name.Contains("Total"))
                        {
                            cpu = val;
                        }

                        if (hardware.HardwareType == HardwareType.Memory)
                        {
                            ram = val;
                        }

                        // GPU 점유율 추출 (NVIDIA, AMD 및 인텔 내장 그래픽의 코어/3D 연산량 타깃팅)
                        if (hardware.HardwareType.ToString().Contains("Gpu"))
                        {
                            if (sensor.Name.Contains("GPU Core") || sensor.Name.Contains("D3D") || sensor.Name.Contains("3D"))
                            {
                                if (val >= 0 && val <= 100)
                                {
                                    // 외장/내장 GPU가 동시에 돌 경우, 더 부하가 많이 걸려있는 메인 장치의 값을 채택
                                    if (val > gpu) gpu = val;
                                }
                            }
                        }
                    }
                }

                // 센서 업데이트가 깨졌고 수집된 데이터가 전부 원점이라면 컴퓨터 핸들을 리셋
                if (updateFailed && cpu == 0 && ram == 0 && gpu == 0)
                {
                    CloseComputer();
                }

                // 가공 완료된 데이터 뭉치를 표준 규격 모델에 담아 반환
                return new SystemSnapshot
                {
                    CpuUsage = cpu,
                    RamUsage = ram,
                    GpuUsage = gpu,
                    MeasuredAt = DateTime.Now
                };
            }
        }

        // 정적 공유 컴퓨터 핸들이 열려있는지 확인하고, 닫혀있다면 센서 구동 엔진을 활성화
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
                _computer.Open(); // 최초 1회 운영체제 드라이버 후킹 및 커널 접근 시작
                return _computer;
            }
            catch
            {
                _computer = null;
                return null;
            }
        }

        // 데이터 유실 및 에러 발생 시 프로그램 크래시를 방지하기 위한 빈 스냅샷 생성기
        private static SystemSnapshot CreateEmptySnapshot() => new SystemSnapshot
        {
            CpuUsage = 0,
            RamUsage = 0,
            GpuUsage = 0,
            MeasuredAt = DateTime.Now
        };

        // 로우 레벨 드라이버 커넥션을 닫고 메모리를 비워주는 초기화 루틴
        private static void CloseComputer()
        {
            try
            {
                _computer?.Close();
            }
            catch
            {
                // 해제 단계에서 발생하는 하드웨어 예외는 안정성을 위해 무시
            }
            finally
            {
                _computer = null;
            }
        }

        // 자원 해제 요청 시 바로 드라이버를 닫지 않고, 다른 창에서 사용 중인지 카운트를 체크 후 소멸
        public void Dispose()
        {
            lock (SyncRoot)
            {
                if (_disposed)
                    return;

                _disposed = true;
                _instanceCount--;

                // 아직 다른 화면(예: MainForm 혹은 오버레이)에서 모니터링을 쓰고 있다면 엔진을 유지
                if (_instanceCount > 0)
                    return;

                // 이 서비스를 쓰는 곳이 프로젝트 내에 아무도 없을 때만 하드웨어 최종 셧다운
                _instanceCount = 0;
                CloseComputer();
            }
        }
    }
}
