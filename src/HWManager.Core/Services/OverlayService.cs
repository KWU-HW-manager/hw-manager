using System;
using System.IO;
using System.Reflection;
using HWManager.Core.Models;

namespace HWManager.Core.Services
{
    /// <summary>
    /// WinForms 프로젝트와 독립된 WPF 오버레이 DLL을 런타임에 동적으로 로드하고
    /// 리플렉션(Reflection)을 이용해 창의 상태와 데이터를 원격 중계하는 서비스
    /// </summary>
    public class OverlayService : IOverlayService
    {
        private object _windowInstance; // 동적으로 생성된 WPF MainWindow 인스턴스 저장 객체
        public bool IsOverlayActive { get; set; } = false;

        // 실행 파일 경로에서 오버레이 DLL을 찾아 로드한 뒤 WPF 창 인스턴스 생성 및 출력
        public void ShowOverlay()
        {
            if (_windowInstance != null) return;

            try
            {
                // 실행 폴더에서 오버레이 dll 파일 탐색 및 로드
                string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HWManager.Overlay.dll");
                if (!File.Exists(dllPath)) return;

                Assembly assembly = Assembly.LoadFrom(dllPath);
                Type windowType = assembly.GetType("HWManager.Overlay.MainWindow");

                if (windowType != null)
                {
                    // 리플렉션으로 WPF 인스턴스 생성 및 Show 메서드 동적 호출
                    _windowInstance = Activator.CreateInstance(windowType);
                    windowType.GetMethod("Show")?.Invoke(_windowInstance, null);
                    IsOverlayActive = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"오버레이 켜기 실패: {ex.Message}");
            }
        }

        // 실행 중인 WPF 오버레이 창을 닫고 할당된 인스턴스 메모리 해제
        public void HideOverlay()
        {
            if (_windowInstance == null) return;

            try
            {
                var windowType = _windowInstance.GetType();
                windowType.GetMethod("Close")?.Invoke(_windowInstance, null);
            }
            catch { }
            finally
            {
                _windowInstance = null;
                IsOverlayActive = false;
            }
        }

        // 수집된 자원 스냅샷 데이터를 매개변수 타입에 맞춰 변환 후 WPF의 UpdateData 메서드로 원격 전달
        public void UpdateHardwareData(SystemSnapshot snapshot)
        {
            if (!IsOverlayActive || _windowInstance == null || snapshot == null) return;

            try
            {
                var windowType = _windowInstance.GetType();
                var updateMethod = windowType.GetMethod("UpdateData");

                if (updateMethod != null)
                {
                    // 오버레이 창 메서드 파라미터 규격(float/double)에 맞춰 타입 동적 변환
                    var parameters = updateMethod.GetParameters();
                    object[] args = new object[3];

                    args[0] = Convert.ChangeType(snapshot.CpuUsage, parameters[0].ParameterType);
                    args[1] = Convert.ChangeType(snapshot.RamUsage, parameters[1].ParameterType);
                    args[2] = Convert.ChangeType(snapshot.GpuUsage, parameters[2].ParameterType);

                    // WPF 내부 디스패처 안전 구동 보장 하에 원격 인보크 실행
                    updateMethod.Invoke(_windowInstance, args);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"오버레이 데이터 전송 실패: {ex.Message}");
            }
        }

        // 리플렉션 프로퍼티 조작을 통해 WPF 내장 Window.Opacity 속성을 다이렉트로 수정 (0.0 ~ 1.0)
        public void SetOpacity(double opacity)
        {
            if (_windowInstance == null) return;
            var type = _windowInstance.GetType();

            // WPF Window의 Opacity 속성을 직접 변경
            type.GetProperty("Opacity")?.SetValue(_windowInstance, opacity);
        }

        // WPF 오버레이 내부 레이아웃스케일러 및 창 크기 강제 변경 메서드 동적 호출 (0.5 ~ 1.5)
        public void SetScale(double scale)
        {
            if (_windowInstance == null) return;
            var type = _windowInstance.GetType();

            // WPF 창의 SetScale 메서드를 호출합니다.
            type.GetMethod("SetScale")?.Invoke(_windowInstance, new object[] { scale });
        }
    }
}