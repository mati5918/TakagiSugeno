// Write your Javascript code.

function createChart(inputId, containerId)
{
    var url = "/Charts/GetChartData/?inputId=" + inputId;
    $.ajax({
        url: url,
        type: "GET",
        success: function (response) {
            plot(response, containerId);
        }
    });
}

function plot(response, id)
{
    var chart = new CanvasJS.Chart(id, {
        data: [],
        interactivityEnabled: false
    });
    var dataPoints = [];
    for (var i = 0; i < response.length; i++) {
        for (var j = 0; j < response[i].data.length; j++){
            dataPoints.push({ x: response[i].data[j].x, y: response[i].data[j].y });

        }
        //chart.options.data.push({ type: "line", dataPoints: dataPoints, name: response[i].variableId, click: test, markerSize: 5 });
        chart.options.data.push({ type: "line", dataPoints: dataPoints});
        dataPoints = [];
    }
    chart.render();
}

function test(e)
{
    alert("adada");
    //alert(e.dataSeries.name);
}

function openInput(id, title)
{
    var url = "/Inputs/Details/" + id;
    if (title === "Przegląd systemu") {        
        window.location.href = url;
    }
    else {
        $.ajax({
            url: url,
            type: "GET",
            success: function (response) {
                $(".body-content").html(response);
            }
        });
    }
}
