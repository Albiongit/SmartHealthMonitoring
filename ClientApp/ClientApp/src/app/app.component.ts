import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ChartData, ChartOptions } from 'chart.js';
import { SensorDataService } from './core/services/sensor-data.service';
import { BaseChartDirective } from 'ng2-charts';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, BaseChartDirective],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  public pulseRateData: ChartData<'line'> | undefined;
  public bodyTemperatureData: ChartData<'line'> | undefined;
  public roomTemperatureData: ChartData<'line'> | undefined;
  public roomHumidityData: ChartData<'line'> | undefined;

  public chartOptions: ChartOptions = {
    responsive: true,
  };

  constructor(private sensorDataService: SensorDataService) {}

  ngOnInit(): void {
    const sensorNodeId = 'your-sensor-node-id'; // Replace with actual sensor node ID
    this.sensorDataService.getAggregatedSensorData(sensorNodeId).subscribe(data => {
      this.prepareCharts(data.sensorData);
    });
  }

  prepareCharts(sensorData: any[]): void {
    const timestamps = sensorData.map(d => new Date(d.timestamp).toLocaleTimeString());

    this.pulseRateData = {
      labels: timestamps,
      datasets: [
        {
          data: sensorData.map(d => d.pulseRate),
          label: 'Pulse Rate',
          borderColor: '#3e95cd',
          fill: false,
        },
      ],
    };

    this.bodyTemperatureData = {
      labels: timestamps,
      datasets: [
        {
          data: sensorData.map(d => d.bodyTemperature),
          label: 'Body Temperature',
          borderColor: '#8e5ea2',
          fill: false,
        },
      ],
    };

    this.roomTemperatureData = {
      labels: timestamps,
      datasets: [
        {
          data: sensorData.map(d => d.roomTemperature),
          label: 'Room Temperature',
          borderColor: '#3cba9f',
          fill: false,
        },
      ],
    };

    this.roomHumidityData = {
      labels: timestamps,
      datasets: [
        {
          data: sensorData.map(d => d.roomHumidity),
          label: 'Room Humidity',
          borderColor: '#e8c3b9',
          fill: false,
        },
      ],
    };
  }
}