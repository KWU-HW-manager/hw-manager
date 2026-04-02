# 💻 HW-Manager: 실시간 PC자원 모니터링 및 최적화 시스템

광운대학교 응용소프트웨어실습 8조 팀 프로젝트입니다.
기존 Windows 작업 관리자의 한계를 보완하여 실시간 모니터링, 사용자 설정 기반 자동 최적화, 데이터 분석, 그리고 원격 모니터링 기능을 통합한 스마트 시스템을 개발합니다.

## ✨ Core Features
* **실시간 하드웨어 모니터링**: CPU, 메모리, GPU 등의 자원 사용량을 수집하고 통합 시각화
* **스마트 프로세스 제어**: 직관적인 프로세스 목록 제공 및 개별 제어
* **커스텀 기반 자원 관리 (Focus Mode)**: 임계값 기준 알림 및 사용자 설정 조건에 따른 프로세스 자동 제어/종료
* **자원 사용 이력 분석**: SQLite 기반 로그 저장 및 일간/주간 사용 패턴 통계 시각화
* **원격 웹 대시보드**: ASP.NET 기반 웹 페이지를 통해 외부 기기에서 PC 자원 상태 모니터링
* **플로팅 위젯 (Overlay)**: 게임 및 전체 화면 환경을 위한 반투명 최소화 UI

## 🛠️ Tech Stack
* **Language**: C#
* **UI Framework**: Windows Forms (.NET)
* **Backend / Web**: ASP.NET Core
* **Database**: SQLite (ADO.NET)
* **IDE**: Visual Studio
* **Version Control**: GitHub

## 👨‍💻 Team 8 Members & Roles
| 이름 | 주요 역할 및 담당 업무 |
|:---:|---|
| **홍경택**<br>(팀장) | • C# 기반 하드웨어 자원 수집 로직 구현 (CPU, RAM, GPU)<br>• 실시간 데이터 수집 로직 구현<br>• 커스텀 기반 자원 관리 기능 구현 (임계값 기준 알림 발생) |
| **이윤성** | • Windows Forms 기반 UI 설계 및 화면 구성<br>• 실시간 자원 상태 그래프 시각화 기능 구현<br>• Overlay(플로팅 위젯) UI 구현 |
| **최상동** | • 커스텀 기반 자원 관리 기능 구현 (프로세스 자동 제어 로직)<br>• C# 기반 프로세스 제어 기능 구현<br>• 사용자 프로필 기능 구현<br>• GitHub Repository 생성 및 형상 관리 |
| **조훈영** | • SQLite + ADO.NET 기반 자원 로그 저장 기능 구현<br>• 통계 데이터 처리 및 시각화 화면 구현<br>• ASP.NET 기반 웹 대시보드 구현 |

## 📂 Project Structure
```text
HW-Manager/
│
├── src/                         
│   ├── HWManager.Client/        # [WinForms] 로컬 모니터링 UI 및 프로세스 제어 앱
│   ├── HWManager.Server/        # [ASP.NET] 원격 대시보드 제공 및 외부 통신 웹 서버
│   └── HWManager.Core/          # [ClassLib] 공통 데이터 모델 및 DB Context
│
└── docs/                        # 기획서, 제안서 및 결과 보고서 폴더