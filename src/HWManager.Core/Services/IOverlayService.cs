using HWManager.Core.Models;
using System;

namespace HWManager.Core.Services
{
    public interface IOverlayService
    {
        // 오버레이 창 활성화 여부
        bool IsOverlayActive { get; set; }

        // 오버레이 창을 화면에 표시
        void ShowOverlay();

        // 오버레이 창을 화면에서 숨김
        void HideOverlay();

        // 메인 프로그램에서 수집한 하드웨어 데이터를 오버레이로 전달
        void UpdateHardwareData(SystemSnapshot snapshot);
    }
}