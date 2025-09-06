# TrackSense.Utils.SensorDataEmulator
Scripts for sensor data emulation

Сервис эмулирует работу датчика, отправляя данные на Endpoint переодичностью раз в минуту

## Запуск 

Для WIndows: запустить start_windows.bat

Для Linux: запустить start_linux.bat

## Схема

/sensor-data - основной Endpoint

## Пример ответа

{
  "GPS": "-89.64591286602015,86.41263478479118",
  "sensor1": 0.17,
  "sensor2": 0.34,
  "sensor3": 0.17,
  "g_sensor": 0.69,
  "volt": 12.27,
  "temp": 271,
  "hum": "18",
  "barometer": 1030.36
  "time":"2024-06-09T10:19:38.389036"
}


## Типы данных 

GPS: str

sensor1: float

sensor2: float

sensor3: float

g_sensor: float

volt: float

temp: unsigned short

hum: bytes

barometer: float

time: datetime
