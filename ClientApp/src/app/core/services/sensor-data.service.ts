import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SensorDataService {
  private baseUrl = 'https://localhost:44342/api'; 

  constructor(private http: HttpClient) {}

  getAggregatedData(): Observable<AggregatedSensorDataModel[]> {
    return this.http.get<AggregatedSensorDataModel[]>(`${this.baseUrl}/Data/`);
  }

  addSensorAndAlarmData(data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/Data/AddSensorAndAlarmData`, data);
  }

  generateData(nrOfRows: number): Observable<any> {
    const params = new HttpParams().set('nrOfRows', nrOfRows.toString());
    return this.http.post(`${this.baseUrl}/Simulator/GenerateData`, {}, { params });
  }
}



interface SensorDataModel {
  timestamp: Date;
  pulseRate: number;
  bodyTemperature: number;
  roomTemperature: number;
  roomHumidity: number;
}

interface PatientModel {
  patientId: string;
  firstName: string;
  lastName: string;
  birthday: Date;
  gender: string;
  address: string;
}

interface SensorModel {
  sensorCode: string;
  sensorName: string;
  manufacturer: string;
}

interface SensorNodeModel {
  nodeId: string;
  nodeName: string;
  batteryPercentage: number;
  hospitalName: string;
  patientId: string;
  sensorCode: string;
}

interface AlarmInsertModel
{
    alarmId: string;
    alarmCause: string;
    alarmCauseValue: number;
    alarmDescription: string;
    sensorNodeId: string;
    timeStamp: Date;
}

interface AggregatedSensorDataModel {
  sensor: SensorModel;
  patient: PatientModel;
  sensorNode: SensorNodeModel;
  sensorData: SensorDataModel[];
  alarmData: AlarmInsertModel[];
}
