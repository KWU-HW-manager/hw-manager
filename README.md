# [cite_start]HW-Manager: 실시간 PC자원 모니터링 및 최적화 시스템 [cite: 252, 253]

[cite_start]광운대학교 응용소프트웨어실습 8조 팀 프로젝트입니다. [cite: 254]
[cite_start]기존 작업 관리자의 한계를 보완하여 실시간 모니터링, 사용자 설정 기반 자동 최적화, 데이터 분석, 원격 모니터링 기능을 통합한 스마트 시스템입니다. [cite: 282]

## [cite_start]👨‍💻 Team 8 Members [cite: 254]
* [cite_start]**홍경택 (팀장)**: 하드웨어 자원 수집, 커스텀 기반 자원 관리 (임계값 알림) [cite: 255, 413, 414, 416]
* [cite_start]**이윤성**: Windows Forms UI 설계, 실시간 그래프 시각화, Overlay UI [cite: 256, 417, 419, 420, 421]
* [cite_start]**최상동**: 커스텀 자원 관리 (자동 제어), 프로세스 제어, 형상 관리 [cite: 257, 418, 422, 423, 426]
* [cite_start]**조훈영**: SQLite + ADO.NET DB 구축, 통계 시각화, ASP.NET 웹 대시보드 [cite: 258, 427, 428, 429, 430]

## [cite_start]🛠️ Tech Stack [cite: 389]
* [cite_start]**Language**: C# [cite: 390]
* [cite_start]**Desktop Client**: Windows Forms (.NET) [cite: 391]
* [cite_start]**Web/Server**: ASP.NET Core [cite: 392]
* [cite_start]**Database**: SQLite (ADO.NET) [cite: 393]
* [cite_start]**IDE**: Visual Studio [cite: 395]

## ✨ Core Features
1. [cite_start]**실시간 하드웨어 모니터링**: CPU, RAM, GPU 사용량 통합 시각화 [cite: 328]
2. [cite_start]**스마트 프로세스 제어**: 직관적인 프로세스 목록 제공 및 제어 [cite: 331]
3. [cite_start]**커스텀 기반 자원 관리**: 사용자 설정 조건에 따른 프로세스 자동 제어 및 알림 [cite: 336, 337, 338]
4. [cite_start]**자원 사용 이력 분석**: SQLite 기반 로그 저장 및 일간/주간 패턴 분석 [cite: 346, 347]
5. [cite_start]**웹 대시보드 (원격 모니터링)**: 외부 기기에서 ASP.NET을 통한 PC 자원 상태 확인 [cite: 343]

## 📂 Project Structure
* `HWManager.Client`: 모니터링 및 프로세스 제어를 담당하는 WinForms 데스크톱 애플리케이션
* `HWManager.Server`: 자원 데이터를 수신하고 원격 뷰어를 제공하는 ASP.NET 웹 대시보드
* `HWManager.Core`: 클라이언트와 서버가 공유하는 데이터 모델 및 DB 컨텍스트