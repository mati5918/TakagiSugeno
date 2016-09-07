// Write your Javascript code.

var selectedThickness = 3 ;
var hoverThickness = 3;
var unselectedThickness = 2;

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
        case 0: plotTriangle(chartPoints, id, chart); break;
        case 1: plotTrapeze(chartPoints, id, chart); break;
    }
}

function plotTriangle(chartPoints, id, chart) {
    var dataPoints = [];
    dataPoints.push({ x: chartPoints[0], y: 0 });
    dataPoints.push({ x: chartPoints[1], y: 1 });
    dataPoints.push({ x: chartPoints[2], y: 0 });
    chart.options.data.push({ type: "line", dataPoints: dataPoints, color: "red", name: id, lineThickness: unselectedThickness });
    chart.render();
}

function plotTrapeze(chartPoints, id, chart) {
    var dataPoints = [];
    dataPoints.push({ x: chartPoints[0], y: 0 });
    dataPoints.push({ x: chartPoints[1], y: 1 });
    dataPoints.push({ x: chartPoints[2], y: 1 });
    dataPoints.push({ x: chartPoints[3], y: 0 });
    chart.options.data.push({ type: "line", dataPoints: dataPoints, color: "red", name: id, lineThickness: unselectedThickness });
    chart.render();
}

function selectVariable(clickedId, chart) {
    $(".variable-row").each(function () {
        var currId = $(this).attr("id");
        if (clickedId == currId) {
            $(this).removeClass("panel-default");
            $(this).removeClass("panel-success");
            $(this).addClass("panel-primary");
        } else {
            $(this).removeClass("panel-primary");
            $(this).addClass("panel-default");
        }
    })
    $.each(chart.options.data, function (i,v) {
        if (v.name == clickedId) {
            v.color = "blue";
            v.lineThickness = selectedThickness
        }
        else {
            v.color = "red";
            v.lineThickness = unselectedThickness
        }
    })
    chart.render();
}

function createFakeId() {
    var rows = $(".variable-row");
    var min = 0;
    $.each(rows, function (index, value) {
        var id = parseInt($(value).attr("id"));
        if (id < min) min = id;
    });
    return min - 1;
}

function removeSerieFromChart(chart, id) {
    chart.options.data = $.grep(chart.options.data, function (e) {
        return e.name != id;
    })
    chart.render();
}

function addNewSerieToChart(chart, id) {
    plotTriangle(Array.of(0,0,0), id, chart);
}

function refreshSerie(id, chart, type) {
    removeSerieFromChart(chart, id);
    var row = $("#" + id);
    var chartPoints = [];
    //var type = $(row).find("#Type").val();
    //console.log(type);
    $(row).find(".chart-data-input").each(function () {
        var value = parseFloat($(this).val());
        chartPoints.push(value);
    })
    plotData(chartPoints, type, id, chart);

}

function hoverVariable(clickedId, chart) {
    $(".variable-row").each(function () {
        var currId = $(this).attr("id");
        if (clickedId == currId && !$(this).hasClass("panel-primary")) {
            $(this).removeClass("panel-default");
            $(this).addClass("panel-success");
        }
    })
    $.each(chart.options.data, function (i,v) {
        if (v.name == clickedId && v.color != "blue") {
            v.color = "green";
            v.lineThickness = hoverThickness
        }
    })
    chart.render();
}

function cancelHoverVariable(chart) {
    $(".variable-row").each(function () {
        if ($(this).hasClass("panel-success")) {
            $(this).removeClass("panel-success");
            $(this).addClass("panel-default");
        }
    })
    $.each(chart.options.data, function (i, v) {
        if (v.color == "green") {
            v.color = "red";
            v.lineThickness = unselectedThickness
        }
    })
    chart.render();
}

function hideAlert() {
    $(".alert").hide();
}

function refreshButtonsState() {
    if ($(".has-error").length > 0) {
        $("#btnSave").prop("disabled", true);
    }
    else {
        $("#btnSave").prop("disabled", false);
    }
    $("#btnCancel").prop("disabled", false);
}