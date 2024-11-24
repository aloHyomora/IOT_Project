// This example code is in the Public Domain (or CC0 licensed, at your option.)
// By Evandro Copercini - 2018
//
// This example creates a bridge between Serial and Classical Bluetooth (SPP)
// and also demonstrate that SerialBT have the same functionalities of a normal Serial
// Note: Pairing is authenticated automatically by this device

#include "BluetoothSerial.h"
#include "esp_bt_main.h"
#include "esp_bt_device.h"
#include "esp_gap_bt_api.h"
#include "esp_spp_api.h"

String device_name = "ESP32-BT-Slave";

// Check if Bluetooth is available
#if !defined(CONFIG_BT_ENABLED) || !defined(CONFIG_BLUEDROID_ENABLED)
#error Bluetooth is not enabled! Please run `make menuconfig` to and enable it
#endif

// Check Serial Port Profile
#if !defined(CONFIG_BT_SPP_ENABLED)
#error Serial Port Profile for Bluetooth is not available or not enabled. It is only available for the ESP32 chip.
#endif

BluetoothSerial SerialBT;

void btAppGapCb(esp_bt_gap_cb_event_t event, esp_bt_gap_cb_param_t *param) {
    switch (event) {
        case ESP_BT_GAP_READ_REMOTE_NAME_EVT:
            if (param->read_rmt_name.stat == ESP_BT_STATUS_SUCCESS) {
                Serial.printf("연결된 기기 이름: %s\n", param->read_rmt_name.rmt_name);
            } else {
                Serial.println("원격 기기 이름을 읽는 데 실패했습니다.");
            }
            break;
        default:
            break;
    }
}

void btCallback(esp_spp_cb_event_t event, esp_spp_cb_param_t *param) {
    if (event == ESP_SPP_SRV_OPEN_EVT) {
        // 연결이 성공적으로 이루어졌을 때
        esp_err_t result = esp_bt_gap_read_remote_name(param->srv_open.rem_bda);
        if (result == ESP_OK) {
            Serial.println("원격 이름 요청을 성공적으로 시작했습니다.");
        } else {
            Serial.printf("원격 이름 요청을 시작하는 데 실패했습니다: %s\n", esp_err_to_name(result));
        }
    } else if (event == ESP_SPP_CLOSE_EVT) {
        // 연결이 종료되었을 때
        Serial.println("블루투스 연결이 종료되었습니다.");
    }
}

void setup() {
  Serial.begin(115200);
  esp_bt_gap_register_callback(btAppGapCb);
  SerialBT.register_callback(btCallback);
  SerialBT.begin(device_name);  //Bluetooth device name
  //SerialBT.deleteAllBondedDevices(); // Uncomment this to delete paired devices; Must be called after begin
  Serial.printf("The device with name \"%s\" is started.\nNow you can pair it with Bluetooth!\n", device_name.c_str());
}

void loop() {
  if (Serial.available()) {
    SerialBT.write(Serial.read());
  }
  if (SerialBT.available()) {
    Serial.write(SerialBT.read());
  }
  delay(20);
}
