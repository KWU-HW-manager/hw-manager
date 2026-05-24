using System;
using System.IO;
using System.Reflection;
using HWManager.Core.Models; // 수집 모델 참조

namespace HWManager.Core.Services
{
    public class OverlayService : IOverlayService
    {
        private object _windowInstance;
        public bool IsOverlayActive { get; set; } = false;

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
                    // WPF 인스턴스 생성 및 출력
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

        public void UpdateHardwareData(SystemSnapshot snapshot)
        {
            if (!IsOverlayActive || _windowInstance == null || snapshot == null) return;

            try
            {
                var windowType = _windowInstance.GetType();
                // 이름으로 오버레이 창의 업데이트 메서드를 정밀 검색합니다
                var updateMethod = windowType.GetMethod("UpdateData");

                if (updateMethod != null)
                {
                    // 오버레이 창이 요구하는 타입(float/double)에 맞춰 데이터를 강제 매칭합니다
                    var parameters = updateMethod.GetParameters();
                    object[] args = new object[3];

                    args[0] = Convert.ChangeType(snapshot.CpuUsage, parameters[0].ParameterType);
                    args[1] = Convert.ChangeType(snapshot.RamUsage, parameters[1].ParameterType);
                    args[2] = Convert.ChangeType(snapshot.GpuUsage, parameters[2].ParameterType);

                    // WPF 창 내부에서 이미 디스패처 처리를 하므로 즉시 안전하게 호출합니다
                    updateMethod.Invoke(_windowInstance, args);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"오버레이 데이터 전송 실패: {ex.Message}");
            }
        }

        // 오버레이 창 투명도 설정 (0.0 ~ 1.0)
        public void SetOpacity(double opacity)
        {
            if (_windowInstance == null) return;
            var type = _windowInstance.GetType();

            // WPF Window의 Opacity 속성을 직접 변경
            type.GetProperty("Opacity")?.SetValue(_windowInstance, opacity);
        }

        // 오버레이 창 크기 설정 (0.5 ~ 1.5)
        public void SetScale(double scale)
        {
            if (_windowInstance == null) return;
            var type = _windowInstance.GetType();

            // WPF 창의 SetScale 메서드를 호출합니다.
            type.GetMethod("SetScale")?.Invoke(_windowInstance, new object[] { scale });
        }
    }
}