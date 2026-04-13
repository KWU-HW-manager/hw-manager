using System;
using System.Windows.Forms;

namespace HWManager.Client
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            ApplicationConfiguration.Initialize();

            // DatabaseHelper.cs: 초기화 메서드 호출
            DatabaseHelper.Initialize();

            Application.Run(new MainForm());
        }
    }
}