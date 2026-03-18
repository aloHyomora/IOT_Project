# Smart Sign Language Control (MR-Embedded System Integration)

## 📌 Project Overview
본 프로젝트는 **임베디드시스템프로그래밍** 전공 강의의 텀 프로젝트로 진행되었습니다. 기존 스마트 홈이나 IoT 기기 제어 방식이 단순한 모바일 앱의 터치 인터페이스에 의존하는 한계를 극복하고, **사용자의 수어(Sign Language)를 인식하여 문장 단위의 복잡한 명령을 직관적으로 제어**하는 시스템을 구축하는 것을 목표로 합니다.

## 🔗 Demo Video
[시연 영상 보기 (YouTube)](https://youtu.be/ipGFO8HEp-Y?si=gFeLlU8ZZIsDIp3n)

## 🚀 Key Features
- **문장 단위 제어:** 단순한 On/Off를 넘어 수어 조합을 통한 복잡한 문장형 명령 수행.
- **MR(Mixed Reality) 환경:** 가상 인터페이스와 실제 사물을 결합한 몰입형 제어 환경 제공.
- **실시간 핸드 트래킹:** Meta SDK를 활용하여 정교한 수어 데이터 추출 및 인식.
- **신뢰성 있는 데이터 통신:** 바이너리 패킷 구조를 활용한 데이터 손실 방지 및 무결성 보장.
- **MCU 하드웨어 제어:** 수신된 명령에 따라 실제 LED, 모터 등 하드웨어 액추에이터 구동.

## 🛠 Tech Stack
- **Software:** Meta SDK (Unity 기반 MR 환경 구축), Oculus Hand Tracking SDK (커스텀 수어 인식 로직)
- **Embedded:** Mbed OS (or 관련 MCU 프레임워크), C/C++
- **Communication:** Serial/Socket 통신 (명령 데이터 송수신), 바이너리 패킷화 프로토콜 구현

## 🔌 System Architecture
### 1. Gesture Recognition & Customization
Meta SDK의 핸드 트래킹 기능을 커스터마이징하여 특정 손동작을 수어 데이터로 변환합니다. 개별 손가락의 관절(Joint) 데이터를 기반으로 수어 패턴을 정의합니다.

### 2. Robust Communication Protocol
데이터 전송 시 발생할 수 있는 손실과 오류를 방지하기 위해 독자적인 패킷 프로토콜을 설계했습니다.
- **Start Binary:** 데이터의 시작을 알리는 특정 바이너리 값 (Frame Sync)
- **Data Payload:** 해석된 수어 명령 데이터
- **End Binary:** 데이터의 끝을 명시하여 패킷의 무결성 확인

### 3. MCU Logic & Actuator Control
수신된 바이너리 데이터를 MCU에서 파싱(Parsing)하여 사전에 정의된 명령셋에 따라 출력 포트를 제어합니다.
- 예: "Light" + "On" (수어) -> MCU 수신 -> 특정 핀 High 출력

## 📝 Project Takeaways
- **데이터 흐름 최적화:** 고성능 MR 헤드셋과 저전력 MCU 간의 효율적인 데이터 송수신 로직을 설계하며 시스템 아키텍처 역량을 강화함.
- **실패 시나리오 고려:** 통신 노이즈로 인한 비정상 동작을 방지하기 위해 시작/끝 바이너리 패킷 구조를 도입하여 시스템의 신뢰성을 높임.
- **UI/UX 확장성:** 물리적 접촉이 없는 제어 방식을 통해 거동이 불편한 사용자나 산업 현장에서의 비접촉 제어 가능성을 확인.
