import { Component, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { Chart, registerables } from 'chart.js';

@Component({
  selector: 'app-polar-chart',
  standalone: true,
  imports: [],
  templateUrl: './polar-chart.component.html',
  styleUrls: ['./polar-chart.component.scss']
})
export class PolarChartComponent implements OnInit, OnChanges {
  @Input() labels: string[] = [];
  @Input() data: number[] = [];
  @Input() backgroundColors: string[] = [];

  public chart: any;
  constructor() {
  }

  ngOnInit(): void {
    this.createChart();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.chart && (changes['labels'] || changes['data'] || changes['backgroundColors'])) {
      this.updateChart();
    }
  }

  createChart(): void {
    this.chart = new Chart('MyChart3', {
      type: 'polarArea',
      data: {
        labels: this.labels,
        datasets: [{
          label: '',
          data: this.data,
          backgroundColor: this.backgroundColors,
        }]
      },
      options: {
        aspectRatio: 2.5
      }
    });
  }

  updateChart(): void {
    if (this.chart) {
      this.chart.data.labels = this.labels;
      this.chart.data.datasets[0].data = this.data;
      this.chart.data.datasets[0].backgroundColor = this.backgroundColors;
      this.chart.update();
    }
  }
}
