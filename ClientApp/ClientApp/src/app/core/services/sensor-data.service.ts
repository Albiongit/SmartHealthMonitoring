import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

// Define your model here
interface SensorDataModel {
  timestamp: Date;
  pulseRate: number;
  bodyTemperature: number;
  roomTemperature: number;
  roomHumidity: number;
}

interface AggregatedSensorDataModel {
  sensor: {
    sensorCode: string;
    sensorName: string;
    manufacturer: string;
  };
  patient: {
    patientId: string;
    firstName: string;
    lastName: string;
    birthday: Date;
    gender: string;
    address: string;
  };
  sensorNode: {
    nodeId: string;
    nodeName: string;
    batteryPercentage: number;
    hospitalName: string;
    patientId: string;
    sensorCode: string;
  };
  sensorData: SensorDataModel[];
}

@Injectable({
  providedIn: 'root',
})
export class SensorDataService {
  private apiUrl = 'https://localhost:5001/api/aggregatedsensordata'; // Update with your actual API URL

  constructor(private http: HttpClient) {}

  // Fetch the aggregated sensor data by sensor node id
  getAggregatedSensorData(sensorNodeId: string): Observable<AggregatedSensorDataModel> {
    return this.http.get<AggregatedSensorDataModel>(`${this.apiUrl}/${sensorNodeId}`);
  }
}