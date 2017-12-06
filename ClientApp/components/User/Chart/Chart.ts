import { Vue, Component, Lifecycle, Prop, p, Watch } from "av-ts";
import Chart from 'chart.js';
import axios from "axios";

@Component()
export default class ChartApp extends Vue {
    myChart: any;
    @Prop ChartData = p({
        type: Array,
        required: true,
        default() {
            return []
        }
    })

    @Prop ChartLabel = p({
        type: Array,
        required: true,
        default() {
            return []
        }
    })

    @Prop ChartTitle = p({
        type: String,
        required: true,
        default() {
            return ""
        }
    })

    @Prop yAxis = p({
        type: String,
        required: true,
        default() {
            return ""
        }
    })

    @Prop xAxis = p({
        type: String,
        required: true,
        default() {
            return ""
        }
    })


    DrawChart() {
        var ctx = document.getElementById("myChart");
        this.myChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: this.ChartLabel,
                datasets: [{
                    label: this.ChartTitle,
                    data: this.ChartData,
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.2)',
                        'rgba(54, 162, 235, 0.2)',
                        'rgba(255, 206, 86, 0.2)',
                        'rgba(75, 192, 192, 0.2)',
                        'rgba(153, 102, 255, 0.2)',
                        'rgba(255, 159, 64, 0.2)'
                    ],
                    borderColor: [
                        'rgba(255,99,132,1)',
                        'rgba(54, 162, 235, 1)',
                        'rgba(255, 206, 86, 1)',
                        'rgba(75, 192, 192, 1)',
                        'rgba(153, 102, 255, 1)',
                        'rgba(255, 159, 64, 1)'
                    ],
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    yAxes: [{
                        scaleLabel: {
                            display: true,
                            labelString: this.yAxis
                        },
                        ticks: {
                            beginAtZero: true
                        }
                    }],
                    xAxes:[{
                        scaleLabel: {
                            display: true,
                            labelString: this.xAxis
                        },
                    }]
                }
            }
        });
    }

    @Lifecycle mounted() {
        this.DrawChart();
    }

    @Watch('ChartData')
    ChartDatahandler(newVal, oldVal) {
        this.myChart.data.datasets[0].data = newVal;
        this.myChart.update();
    };

    @Watch('ChartLabel')
    ChartLabelhandler(newVal, oldVal) {
        this.myChart.data.labels = newVal;
        this.myChart.update();
    };

    @Watch('ChartTitle')
    ChartTitlehandler(newVal, oldVal) {
        this.myChart.data.datasets[0].label = newVal;
        this.myChart.update();
    };

    @Watch('yAxis')
    yAxishandler(newVal, oldVal) {
        this.myChart.options.scales.yAxes[0].scaleLabel.labelString = newVal;
        this.myChart.update();
    };

    @Watch('xAxis')
    xAxishandler(newVal, oldVal) {
        this.myChart.options.scales.xAxes[0].scaleLabel.labelString = newVal;
        this.myChart.update();
    };
}