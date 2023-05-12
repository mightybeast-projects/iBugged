var fontFamily = 'system-ui,-apple-system,"Segoe UI",Roboto,"Helvetica Neue",Arial,"Noto Sans","Liberation Sans",sans-serif,"Apple Color Emoji","Segoe UI Emoji","Segoe UI Symbol","Noto Color Emoji"';

function drawChart(canvasId, chartName, labels, colors, data) {
    var canvas = document.getElementById(canvasId);
    var data = {
        labels: labels,
        datasets: [{
            data: data,
            backgroundColor: colors
        }]
    };
    var options = {
        plugins: {
            title: {
                color : "rgba(0, 0, 0)",
                display: true,
                text: chartName,
                font: {
                    size: 16,
                    family : fontFamily
                },
                padding: {
                    top: 10,
                    bottom: 30
                }
            }
        }
    }

    new Chart(canvas, { type: 'doughnut', data: data, options : options });
}