#include "BluetoothSerial.h"

String device_name = "ESP32-BT-Slave";

// Check if Bluetooth is available
#if !defined(CONFIG_BT_ENABLED) || !defined(CONFIG_BLUEDROID_ENABLED)
#error Bluetooth is not enabled! Please run `make menuconfig` to and enable it
#endif

// Check Serial Port Profile
#if !defined(CONFIG_BT_SPP_ENABLED)
#error Serial Port Profile for Bluetooth is not available or not enabled. It is only available for the ESP32 chip.
#endif

BluetoothSerial BTSerial;

// LED 핀 설정
const int ledRedPin = 9;
const int ledYellowPin = 10;
const int ledGreenPin = 11;
char receivedChar;

void setup() {
  // 시리얼 통신 시작
  Serial.begin(115200);
  BTSerial.begin(device_name);  //Bluetooth device name
  //BTSerial.deleteAllBondedDevices(); // Uncomment this to delete paired devices; Must be called after begin
  Serial.printf("The device with name \"%s\" is started.\nNow you can pair it with Bluetooth!\n", device_name.c_str());

  // LED 핀 출력 모드 설정
  pinMode(ledRedPin, OUTPUT);
  pinMode(ledYellowPin, OUTPUT);
  pinMode(ledGreenPin, OUTPUT);

  Serial.println("Bluetooth 연결 대기 중...");
}

void loop() {
  // 블루투스 모듈에서 데이터를 수신하면
  if (BTSerial.available()) {
    char received = BTSerial.read();        // 데이터를 한 글자씩 읽어옴
    Serial.print("수신된 데이터: ");
    Serial.println(received);
    LedON(ledYellowPin);                    // 블루투스 연결 상태이면 yellow


    // 특정 문자가 수신되면 LED를 켬
    if (received == 'A') {                  // 'C'를 수신하면 연결이 성공한 것으로 간주
      LedON(ledGreenPin);                   // A 받으면 green
      delay(1000);                          // 1초 동안 LED 유지
      digitalWrite(ledGreenPin, LOW);       // LED 끄기
      Serial.println("Bluetooth 연결 성공, 1초 동안 LED가 켜졌다가 꺼졌습니다.");
    }
  }
  else{
    LedON(ledRedPin);                       // 연결안되면 red
  }

  //  ====== DEBUG ======
  if (Serial.available()){
    char c = Serial.read();
    if(c == '1'){
      LedON(ledRedPin);
      delay(1000); 
    }else if(c == '2'){
      LedON(ledYellowPin);
      delay(1000); 
    }else if(c == '3'){
      LedON(ledGreenPin);
      delay(1000); 
    }    
  }
}

void LedON(int pinNum){
  digitalWrite(ledRedPin, LOW); // ledRedPin 끄기
  digitalWrite(ledYellowPin, LOW); // ledYellowPin 끄기
  digitalWrite(ledGreenPin, LOW); // ledGreenPin 끄기

  digitalWrite(pinNum, HIGH); // LED 켜기
}