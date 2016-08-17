// Write your Javascript code.

function createChart(id)
{
    var url = "/Charts/GetChartData/?inputId=" + id;
    $.ajax({
        url: url,
        type: "GET",
        success: function (response) {
            plot(response, id);
        }
    });
}

function plot(response, id)
{
    var chart = new CanvasJS.Chart(id.toString(), {
        data: [],
        interactivityEnabled: false
    });
    var dataPoints = [];
    for (var i = 0; i < response.length; i++) {
        for (var j = 0; j < response[i].data.length; j++){
            dataPoints.push({ x: response[i].data[j].x, y: response[i].data[j].y });

        }
        chart.options.data.push({type: "line", dataPoints: dataPoints});
        dataPoints = [];
    }
    chart.render();
}

function openInput(id, title)
{
    if (title === "Przegląd systemu") {
        alert(title);
    }
}