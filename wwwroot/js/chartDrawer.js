function drawChart(canvasId, labels, colors, data) {
    var canvas = document.getElementById(canvasId);
    var data = {
        labels: labels,
        datasets: [{
            data: data,
            backgroundColor: colors
        }]
    };

    new Chart(canvas, { type: 'doughnut', data: data, });
}