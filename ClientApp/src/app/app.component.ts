import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SensorDataService } from './core/services/sensor-data.service';
import { BarChartComponent } from "../bar-chart/bar-chart.component";
import { LineChartComponent } from "../line-chart/line-chart.component";
import { PieChartComponent } from "../pie-chart/pie-chart.component";
import { PolarChartComponent } from "../polar-chart/polar-chart.component";
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MasterData } from '../master-data';
import { ApiService } from './core/services/api.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, BarChartComponent, LineChartComponent, PieChartComponent, PolarChartComponent,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatMenuModule,
    MatSelectModule,
    MatFormFieldModule,
    CommonModule,
    FormsModule,],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  pulseRates: number[] = [];
  bodyTemperatures: number[] = [];
  timestamps: string[] = [];

  sensors: AggregatedSensorDataModel[] = [];
  selectedSensor?: AggregatedSensorDataModel;

  allSensors = MasterData.getSensors();
  allPatients = MasterData.getPatients();
  allSensorNodes = MasterData.getSensorNodes();

  sensorNames: string[] = [];
  sensorNodeCounts: number[] = [];

  sensorNodes: string[] = [];
  sensorNodeDataCount: number[] = [];

  pulseAIResponse: string = "";
  bodyTempAIResponse: string = "";

  constructor(private sensorDataService: SensorDataService, private apiService: ApiService) {}

  ngOnInit(): void {
    this.sensorDataService.getAggregatedData().subscribe(data => {
      this.sensors = data;

      this.sensorNames = [...new Set(data.map(e => e.sensor.sensorName))];
      this.sensorNodeCounts = data.map(e => data.filter(x => x.sensor.sensorName == e.sensor.sensorName).length);

      this.sensorNodes = [...new Set(data.map(e => e.sensorNode.nodeName))];
      this.sensorNodeDataCount = data.map(e => e.sensorData.length);

    });
  }

  onSelectSensorNode(index: number): void {
    let item: AggregatedSensorDataModel = this.sensors[index];
    this.selectedSensor = item ?? null;
    this.processLinePulseAndTempData(item);
    this.processRoomData(item);
  }

  public getPulseRateDistribution(): number[] {
    const normal = this.pulseRates.filter(rate => rate < 100).length;
    const elevated = this.pulseRates.filter(rate => rate >= 100 && rate < 120).length;
    const high = this.pulseRates.filter(rate => rate >= 120).length;
  
    return [normal, elevated, high];
  }

  public labels: string[] = [];
  public avgPulseRate: number[] = [];
  public avgBodyTemp: number[] = [];

  public roomLabels: string[] = [];
  public avgRoomTemp: number[] = [];
  public avgRoomHumidity: number[] = [];

  public processLinePulseAndTempData(sensorData: AggregatedSensorDataModel) {
    this.labels = [];
    this.avgPulseRate = [];
    this.avgBodyTemp = [];

    const sortedData = sensorData.sensorData.sort((a: any, b: any) => new Date(a.timestamp).getTime() - new Date(b.timestamp).getTime());
    const dataByTime: { [key: string]: { pulseRates: number[], bodyTemperatures: number[] } } = {};

    sortedData.forEach((data: any) => {
      const time = data.timestamp.split('T')[1].split(':')[0] + ':' + data.timestamp.split('T')[1].split(':')[1];
      if (!dataByTime[time]) {
        dataByTime[time] = { pulseRates: [], bodyTemperatures: [] };
      }
      dataByTime[time].pulseRates.push(data.pulseRate);
      dataByTime[time].bodyTemperatures.push(data.bodyTemperature);
    });

    Object.keys(dataByTime).forEach((time) => {
      const pulseRates = dataByTime[time].pulseRates;
      const bodyTemperatures = dataByTime[time].bodyTemperatures;

      const avgPulseRate = this.calculateAverage(pulseRates);
      const avgBodyTemperature = this.calculateAverage(bodyTemperatures);

      this.labels.push(time);
      this.avgPulseRate.push(avgPulseRate);
      this.avgBodyTemp.push(avgBodyTemperature);
    });
  }

  private calculateAverage(data: number[]): number {
    const total = data.reduce((sum, value) => sum + value, 0);
    return total / data.length || 0;
  }

  public processRoomData(sensorData: AggregatedSensorDataModel) {
    this.roomLabels = [];
    this.avgRoomHumidity = [];
    this.avgRoomTemp = [];
    
    const sortedData = sensorData.sensorData.sort((a: any, b: any) => new Date(a.timestamp).getTime() - new Date(b.timestamp).getTime());

    const dataByTime: { [key: string]: { humidities: number[], temperatures: number[] } } = {};

    sortedData.forEach((data: any) => {
      const time = data.timestamp.split('T')[1].split(':')[0] + ':' + data.timestamp.split('T')[1].split(':')[1];
      if (!dataByTime[time]) {
        dataByTime[time] = { humidities: [], temperatures: [] };
      }
      dataByTime[time].humidities.push(data.roomHumidity);
      dataByTime[time].temperatures.push(data.roomTemperature);
    });

    Object.keys(dataByTime).forEach((time) => {
      const pulseRates = dataByTime[time].humidities;
      const bodyTemperatures = dataByTime[time].temperatures;

      const avgPulseRate = this.calculateAverage(pulseRates);
      const avgBodyTemperature = this.calculateAverage(bodyTemperatures);

      this.roomLabels.push(time);
      this.avgRoomHumidity.push(avgPulseRate);
      this.avgRoomTemp.push(avgBodyTemperature);
    });
  }

  public getAISuggestionForPulseRates() {
    this.pulseAIResponse = "...";
    // const uniquePulseRates = Array.from(new Set(this.avgPulseRate));
    // const uniquePulseRateString = uniquePulseRates.slice(0, 5).join(",");

    this.apiService.generateResponse("What are some normal pulse rates for a person?")
      .subscribe(
        response => {
          const parsedResponse = JSON.parse(response);
          
          this.pulseAIResponse = Object.entries(parsedResponse)
            .map(([key, value]) => `${key}: ${value}`)
            .join(', '); 
        },
        error => {
          this.pulseAIResponse = "";
          console.error('Error:', error);
        }
      );
  }

  public getAISuggestionForBodyTemp() {
    this.bodyTempAIResponse = "...";
    
    this.apiService.generateResponse("What is the normal body temperature range of a person?")
      .subscribe(
        response => {
          const parsedResponse = JSON.parse(response);
          
          this.bodyTempAIResponse = Object.entries(parsedResponse)
            .map(([key, value]) => `${key}: ${value}`)
            .join(', '); 
        },
        error => {
          this.bodyTempAIResponse = "";
          console.error('Error:', error);
        }
      );
  }
  
}

interface AggregatedSensorDataModel {
  sensor: SensorModel;
  patient: PatientModel;
  sensorNode: SensorNodeModel;
  sensorData: SensorDataModel[];
  alarmData: AlarmInsertModel[];
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