import { Component, Input, OnInit, SimpleChanges, OnChanges } from '@angular/core';
import { Chart, registerables } from 'chart.js';

@Component({
  selector: 'app-line-chart',
  standalone: true,
  imports: [],
  templateUrl: './line-chart.component.html',
  styleUrls: ['./line-chart.component.scss']
})
export class LineChartComponent implements OnInit, OnChanges {
  @Input() labels: string[] = [];
  @Input() firstLine: number[] = [];
  @Input() lineLabel: string = "";
  @Input() color: string = "blue";
  @Input() chartId: string = "";

  public chart: Chart | undefined;

  constructor() {
    Chart.register(...registerables);
  }

  ngOnInit(): void {
    setTimeout(() => {
      this.createChart();
    }, 100);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.chart && (changes['labels'] || changes['firstLine'])) {
      this.updateChart();
    }
  }

  createChart(): void {
    if (!this.chartId) {
      console.error('Chart ID is required');
      return;
    }
    
    this.chart = new Chart(this.chartId, {
      type: 'line',
      data: {
        labels: this.labels,
        datasets: [
          {
            label: this.lineLabel,
            data: this.firstLine,
            borderColor: this.color,
            fill: false,
            tension: 0.1
          },
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
      this.chart.data.datasets[0].data = this.firstLine;
      this.chart.update();
    }
  }
}
