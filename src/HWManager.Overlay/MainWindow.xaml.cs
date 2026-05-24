using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace HWManager.Overlay
{
    public partial class MainWindow : Window
    {
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TRANSPARENT = 0x00000020;

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        // 윈도우 전역 키 상태를 직접 조회하는 API
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        public struct POINT { public int X; public int Y; }
        public struct RECT { public int Left; public int Top; public int Right; public int Bottom; }

        private DispatcherTimer _ghostTimer = null!;
        private POINT _dragStartMousePhysical;
        private Point _dragStartWindowLogical;

        public MainWindow()
        {
            InitializeComponent();
            InitGhostTimer();
        }

        // 0.1초마다 글로벌 마우스 및 Shift 상태 감시
        private void InitGhostTimer()
        {
            _ghostTimer = new DispatcherTimer();
            _ghostTimer.Interval = TimeSpan.FromMilliseconds(100);
            _ghostTimer.Tick += GhostTimer_Tick;
            _ghostTimer.Start();
        }

        private void GhostTimer_Tick(object? sender, EventArgs e)
        {
            if (!IsLoaded) return;

            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            if (hwnd == IntPtr.Zero) return;

            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);

            GetCursorPos(out POINT mousePos);
            GetWindowRect(hwnd, out RECT windowRect);

            // 창 포커스와 무관하게 윈도우 전역 Shift 키 실시간 감지 (0x10 = VK_SHIFT)
            bool isShiftDown = (GetAsyncKeyState(0x10) & 0x8000) != 0;

            // 물리 화면 기준 좌표 내에 마우스가 들어왔는지 검사
            bool isMouseOver = mousePos.X >= windowRect.Left && mousePos.X <= windowRect.Right &&
                               mousePos.Y >= windowRect.Top && mousePos.Y <= windowRect.Bottom;

            // 마우스가 창 위에 있고 Shift를 누르면 실체화
            if (isMouseOver && isShiftDown)
            {
                SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle & ~WS_EX_TRANSPARENT); // 관통 해제
            }
            else if (!this.IsMouseCaptured) // 드래그 중이 아닐 때만 다시 유령화
            {
                SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT); // 관통 설정
            }
        }

        // 마우스 휠 클릭 시 물리 좌표 및 논리 윈도우 위치 저장
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                GetCursorPos(out _dragStartMousePhysical);
                _dragStartWindowLogical = new Point(this.Left, this.Top);
                this.CaptureMouse(); // 마우스 제어권 강제 고정
            }
        }

        // 휠 드래그 중 윈도우 화면 배율(DPI)을 완벽하게 계산하여 이동
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.IsMouseCaptured && e.MiddleButton == MouseButtonState.Pressed)
            {
                GetCursorPos(out POINT currentMousePhysical);

                int deltaX = currentMousePhysical.X - _dragStartMousePhysical.X;
                int deltaY = currentMousePhysical.Y - _dragStartMousePhysical.Y;

                // 윈도우 디스플레이 배율 자동 계산 (125%, 150% 등 대응)
                var dpi = VisualTreeHelper.GetDpi(this);

                // 배율 오차 없는 정확한 위치로 이동
                this.Left = _dragStartWindowLogical.X + (deltaX / dpi.DpiScaleX);
                this.Top = _dragStartWindowLogical.Y + (deltaY / dpi.DpiScaleY);
            }
        }

        // 휠 클릭 해제 시 제어 해제
        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                this.ReleaseMouseCapture();
            }
        }

        public void UpdateData(float cpu, double ram, float gpu)
        {
            Dispatcher.Invoke(() =>
            {
                if (txtCpu != null) txtCpu.Text = $"{cpu:F1}%";
                if (txtRam != null) txtRam.Text = $"{ram:F1}%";
                if (txtGpu != null) txtGpu.Text = $"{gpu:F1}%";

                if (progressCpu != null) progressCpu.Value = cpu;
                if (progressRam != null) progressRam.Value = ram;
                if (progressGpu != null) progressGpu.Value = gpu;
            });
        }
    }
}