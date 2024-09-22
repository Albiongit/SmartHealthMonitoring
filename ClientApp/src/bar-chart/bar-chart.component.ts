import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Chart } from 'chart.js';

@Component({
  selector: 'app-bar-chart',
  standalone: true,
  imports: [],
  templateUrl: './bar-chart.component.html',
  styleUrl: './bar-chart.component.scss'
})
export class BarChartComponent implements OnInit, OnChanges {
  @Input() labels: string[] = [];
  @Input() first: number[] = [];
  @Input() second: number[] = [];
  @Input() labelNames: string[] = [];

  public chart: any;

  ngOnInit(): void {
    this.createChart();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.chart && (changes['labels'] || changes['first'] || changes['second'])) {
      this.updateChart();
    }
  }

  createChart(): void {
    this.chart = new Chart("MyChart", {
      type: 'bar',
      data: {
        labels: this.labels,
        datasets: [
          {
            label: this.labelNames[0],
            data: this.first,
            backgroundColor: 'blue'
          },
          {
            label: this.labelNames[1],
            data: this.second,
            backgroundColor: 'limegreen'
          }
        ]
      },
      options: {
        aspectRatio: 2.5
      }
    });
  }

  updateChart(): void {
    if (this.chart) {
      this.chart.data.labels = this.labels;
      this.chart.data.datasets[0].data = this.first;
      this.chart.data.datasets[1].data = this.second;
      this.chart.update();
    }
  }

}
