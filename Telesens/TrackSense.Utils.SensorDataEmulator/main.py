from fastapi import FastAPI
from pydantic import BaseModel
import random
from typing import Dict
from fastapi_utils.tasks import repeat_every
import ctypes
from datetime import datetime

app = FastAPI(
    title="Robot marine simalation service",
    description="Kto eto voovbche chiaet?",
    version="1.0.0"
)

class SensorData(BaseModel):
    DateTime: datetime
    GPS: str
    Sensor1: float
    Sensor2: float
    Sensor3: float
    G_sensor: float
    Volt: float
    Temperature: int
    Humidity: bytes
    Barometer: int

def generate_sensor_data() -> Dict[str, any]:
    latitude = random.uniform(-90.0, 90.0)
    longitude = random.uniform(-180.0, 180.0)
    GPS = f"{latitude},{longitude}"
    Sensor1 = round(random.uniform(0.1, 1.0), 2)
    Sensor2 = round(random.uniform(0.1, 1.0), 2)
    Sensor3 = round(random.uniform(0.1, 1.0), 2)
    G_sensor = round(random.uniform(0.1, 1.0), 2)
    Volt = round(random.uniform(9.0, 12.5), 2)
    Temperature = ctypes.c_ushort(random.randint(203, 340)).value
    Humidity = random.randint(0, 100)
    Barometer = int(random.uniform(950.0, 1050.0)) % 256
    DateTime = datetime.utcnow()

    return {
        "DateTime": DateTime,
        "GPS": GPS,
        "Sensor1": Sensor1,
        "Sensor2": Sensor2,
        "Sensor3": Sensor3,
        "G_sensor": G_sensor,
        "Volt": Volt,
        "Temperature": Temperature,
        "Humidity": Humidity,
        "Barometer": Barometer
    }

@app.get("/sensor-data", response_model=SensorData, summary="Get Sensor Data", description="Returns the current sensor data.")
async def get_sensor_data():
    data = generate_sensor_data()
    return data

@app.on_event("startup")
@repeat_every(seconds=60)
async def periodic_sensor_data_generation():
    data = generate_sensor_data()
    print(data)