using System;
using System.Windows;

namespace HWManager.Overlay
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 수집기 모델 타입(float, double, float)에 맞춰 매개변수 선언
        public void UpdateData(float cpu, double ram, float gpu)
        {
            // UI 스레드 안전성을 확보하면서 텍스트와 게이지 바를 동시에 갱신
            Dispatcher.Invoke(() =>
            {
                // 1. 텍스트 수치 업데이트
                if (txtCpu != null) txtCpu.Text = $"{cpu:F1}%";
                if (txtRam != null) txtRam.Text = $"{ram:F1}%";
                if (txtGpu != null) txtGpu.Text = $"{gpu:F1}%";

                // 2. 가로형 미니 게이지 바 위치 업데이트
                if (progressCpu != null) progressCpu.Value = cpu;
                if (progressRam != null) progressRam.Value = ram; // double 타입 자동 형변환 대입
                if (progressGpu != null) progressGpu.Value = gpu;
            });
        }
    }
}