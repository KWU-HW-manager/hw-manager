using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace HWManager.Overlay
{
    /// <summary>
    /// 화면 최상단에 상주하는 하드웨어 가속 반투명 오버레이 창의 비하인드 코드
    /// 마우스 관통(유령화), 디스플레이 배율 대응 휠 드래그 이동 및 가변 스케일링을 연산
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TRANSPARENT = 0x00000020;
        private double _initWidth = 420;
        private double _initHeight = 50;

        // 온오프 토글 시 창 위치 유실 버그를 방지하기 위해 프로그램 가동 동안 위치를 유지하는 정적 변수
        private static double _lastX = -1;
        private static double _lastY = -1;

        // Win32 OS 전역 마우스 관통 스타일 및 키 상태 조회를 위한 로우 레벨 API 인터페이스
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

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

            // 이전에 기억된 정상 좌표가 있다면 생성 시점에 강제 배치
            if (_lastX >= 0 && _lastY >= 0)
            {
                this.WindowStartupLocation = WindowStartupLocation.Manual;
                this.Left = _lastX;
                this.Top = _lastY;
            }

            // 창이 최초로 화면에 로드 완료되었을 때 초기 위치 기억
            this.Loaded += (s, e) =>
            {
                if (_lastX < 0 || _lastY < 0)
                {
                    _lastX = this.Left;
                    _lastY = this.Top;
                }
            };
        }

        // 0.1초마다 글로벌 마우스 및 Shift 상태 감시
        private void InitGhostTimer()
        {
            _ghostTimer = new DispatcherTimer();
            _ghostTimer.Interval = TimeSpan.FromMilliseconds(100);
            _ghostTimer.Tick += GhostTimer_Tick;
            _ghostTimer.Start();
        }

        // 0.1초마다 글로벌 마우스 및 Shift 상태 감시하여 유령화 토글
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

            // 마우스 포인터가 오버레이 물리 영역 내부에 진입했는지 검사
            bool isMouseOver = mousePos.X >= windowRect.Left && mousePos.X <= windowRect.Right &&
                               mousePos.Y >= windowRect.Top && mousePos.Y <= windowRect.Bottom;

            // 마우스가 창 위에 있고 Shift를 누르면 실체화(조작 가능)
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

        // 휠 드래그 중 윈도우 디스플레이 배율(DPI) 변동 오차를 정밀 연산하여 이동 처리
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

                // 마우스로 정당하게 움직이고 있을 때만 실시간 좌표 갱신
                _lastX = this.Left;
                _lastY = this.Top;
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

        // WinForms 백엔드 교량에서 밀어준 스냅샷 원시 데이터를 UI 스레드 안전 구역(Dispatcher)에서 비동기 반영
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

        // 가변 스케일링 슬라이더 연동 제어 함수 (컨텐츠 벡터 배율 및 창 외곽 크기 동시 처리)
        public void SetScale(double scale)
        {
            Dispatcher.Invoke(() =>
            {
                // 내부 UI 컨텐츠 스케일 변경
                if (this.Content is System.Windows.FrameworkTemplate) return;
                if (this.Content is System.Windows.FrameworkElement element)
                {
                    element.LayoutTransform = new System.Windows.Media.ScaleTransform(scale, scale);
                }

                // 정확한 원본 크기에 배율을 곱해서 창 크기를 강제 조정
                this.Width = _initWidth * scale;
                this.Height = _initHeight * scale;
            });
        }
    }
}