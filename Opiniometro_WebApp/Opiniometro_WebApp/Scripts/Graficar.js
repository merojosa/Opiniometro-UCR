function refrescar(canvas) {
    alert(4);
//    var canvas = document.getElementById("pie_chart2");
    var ctx = canvas.getContext("2d");
    //var myChart = ctx.chart;
    var chartData = ctx.data;
    //myChart.destroy();
    alert(5);
    if (ctx.chart.config.type == 'pie') {
        ctx.chart.destroy();
        var myChart = new Chart(ctx, {
            type: 'horizontalBar',
            data: chartData
        });
    } else {
        ctx.chart.destroy();
        var myChart = new Chart(ctx, {
            type: 'pie',
            data: chartData
        });
    }
}