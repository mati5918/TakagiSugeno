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

function createCharts(chart) {
    var rows = $(".variable-row");
    $.each(rows, function (index, value) {
        var id = $(value).attr("id");
        //console.log(id);
        var chartPoints = [];
        var type = $(value).find(".select-type").first().val();
        $(value).find(".chart-data-input").each(function () {
            var value = parseFloat($(this).val());
            chartPoints.push(value);
        })
        plotData(chartPoints, type, id, chart);
    })
}

function plotData(chartPoints, type, id, chart) {
    switch (parseInt(type)) {
        case 0: plotTriangle(chartPoints, type, id, chart); break;
        case 1: plotTrapeze(chartPoints, type, id, chart); break;
    }
}

function plotTriangle(chartPoints, type, id, chart) {
    var dataPoints = [];
    dataPoints.push({ x: chartPoints[0], y: 0 });
    dataPoints.push({ x: chartPoints[1], y: 1 });
    dataPoints.push({ x: chartPoints[2], y: 0 });
    chart.options.data.push({ type: "line", dataPoints: dataPoints, color: "red", name: id });
    chart.render();
}

function plotTrapeze(chartPoints, type, id, chart) {
    var dataPoints = [];
    dataPoints.push({ x: chartPoints[0], y: 0 });
    dataPoints.push({ x: chartPoints[1], y: 1 });
    dataPoints.push({ x: chartPoints[2], y: 1 });
    dataPoints.push({ x: chartPoints[3], y: 0 });
    chart.options.data.push({ type: "line", dataPoints: dataPoints, color: "red", name: id });
    chart.render();
}

