// Write your Javascript code.

var selectedThickness = 3;
var hoverThickness = 3;
var unselectedThickness = 2;

/*var chart = new CanvasJS.Chart(id, {
    data: [],
});*/

function createChartFromJSONData(chart, dataObj) {
    var data = $.parseJSON($(dataObj).attr("data-chartJson"));
    var chartPoints = [];
    for (var k in data) {
        if (data.hasOwnProperty(k)) {
            chartPoints.push(data[k]);
        }
    }
    var type = $(dataObj).attr("data-type");
    var id = $(dataObj).attr("data-id");
    plotData(chartPoints, type, id, chart)
}

/*function plot(response, id)
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
}*/


function openInput(id, isAjax)
{
    var url = "/Inputs/Details/" + id;
    if (!isAjax) {        
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

function openOutput(id, isAjax) {
    id = parseInt(id);
    var url = "/Outputs/Details/" + id;
    if (!isAjax) {        
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
        case 2: plotGaussian(chartPoints, id, chart); break;
    }
    switch (type) {
        case "Triangle": plotTriangle(chartPoints, id, chart); break;
        case "Trapeze": plotTrapeze(chartPoints, id, chart); break;
        case "Gaussian": plotGaussian(chartPoints, id, chart); break;
    }

}

function plotTriangle(chartPoints, id, chart) {
    var dataPoints = [];
    if (parseFloat(chartPoints[0]) > parseFloat(chart.options.axisX.minimum)) {
        dataPoints.push({ x: chart.options.axisX.minimum, y: 0 });
    }
    dataPoints.push({ x: chartPoints[0], y: 0 });
    dataPoints.push({ x: chartPoints[1], y: 1 });
    dataPoints.push({ x: chartPoints[2], y: 0 });
    if (parseFloat(chartPoints[2]) < parseFloat(chart.options.axisX.maximum)) {
        dataPoints.push({ x: chart.options.axisX.maximum, y: 0 });
    }
    console.log(dataPoints);
    chart.options.interactivityEnabled = false;
    chart.options.data.push({ type: "line", dataPoints: dataPoints, color: "red", name: id, lineThickness: unselectedThickness, markerType: "none" });
    //chart.render();
}

function plotTrapeze(chartPoints, id, chart) {
    var dataPoints = [];
    if (parseFloat(chartPoints[0]) > parseFloat(chart.options.axisX.minimum)) {
        dataPoints.push({ x: chart.options.axisX.minimum, y: 0 });
    }
    dataPoints.push({ x: chartPoints[0], y: 0 });
    dataPoints.push({ x: chartPoints[1], y: 1 });
    dataPoints.push({ x: chartPoints[2], y: 1 });
    dataPoints.push({ x: chartPoints[3], y: 0 });
    if (parseFloat(chartPoints[3]) < parseFloat(chart.options.axisX.maximum)) {
        dataPoints.push({ x: chart.options.axisX.maximum, y: 0 });
    }
    chart.options.interactivityEnabled = false;
    chart.options.data.push({ type: "line", dataPoints: dataPoints, color: "red", name: id, lineThickness: unselectedThickness, markerType: "none" });
    //chart.render();
}

function plotGaussian(chartPoints, id, chart) {
    var dataPoints = getGaussianDataPoints(chartPoints, chart);
    //console.log(dataPoints);
    chart.options.interactivityEnabled = false;
    chart.options.data.push({ type: "spline", dataPoints: dataPoints, color: "red", name: id, lineThickness: unselectedThickness, markerType: "none" });
}

function getGaussianValue(sigma, c, x) {
    var temp = (-1 * Math.pow(x - c, 2)) / (2 * Math.pow(sigma, 2));
    return Math.pow(Math.E, temp);
}

function getGaussianDataPoints(chartPoints, chart) {
    var dataPoints = [];
    var sigma = chartPoints[0];
    var c = chartPoints[1];
    var min = parseFloat(chart.options.axisX.minimum);
    var max = parseFloat(chart.options.axisX.maximum);
    var jump = (max - min) / 20;
    for (var i = min; i <= max; i += jump) {
        dataPoints.push({ x: i, y: getGaussianValue(sigma, c, i) });
    }
    return dataPoints;
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
    });
    if (chart != null) {
        selectVariableOnChart(clickedId, chart)
    }
}

function selectVariableOnChart(clickedId, chart) {
    $.each(chart.options.data, function (i, v) {
        if (v.name == clickedId) {
            v.color = "blue";
            v.lineThickness = selectedThickness;
        }
        else {
            v.color = "red";
            v.lineThickness = unselectedThickness;
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

function createFakeRuleId() {
    var rows = $(".rules-table tbody tr");
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
    if (chart != null) {
        $.each(chart.options.data, function (i, v) {
            if (v.name == clickedId && v.color != "blue") {
                v.color = "green";
                v.lineThickness = hoverThickness;
            }
        })
        chart.render();
    }
}

function cancelHoverVariable(chart) {
    $(".variable-row").each(function () {
        if ($(this).hasClass("panel-success")) {
            $(this).removeClass("panel-success");
            $(this).addClass("panel-default");
        }
    });
    if (chart != null) {
        $.each(chart.options.data, function (i, v) {
            if (v.color == "green") {
                v.color = "red";
                v.lineThickness = unselectedThickness;
            }
        });
        chart.render();
    }
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

function refreshCheckboxes() {
    $("input[type=checkbox]").each(function (i, v) {
        var value = $(v).parents(".panel").find("select").val();
        if (value == -1) {
            $(v).prop("disabled", true);
        } else {
            $(v).prop("disabled", false);
        }
    });
}

function changeRulesButtonsState(isDisabled) {
    $("#btnSave").prop("disabled", isDisabled);
    $("#btnCancel").prop("disabled", isDisabled);
}

function removeInput(obj, isInputsList) {
    var id = $(obj).attr("data-id");
    setTimeout(function () {
        var url = "/Inputs/Remove/" + id;
        $.ajax({
            url: url,
            type: "POST",
            success: function (response) {
                getInputsList($("#SysId").attr("data-system-id"));
                refreshSystemMenu();
                if (isInputsList) {
                    var openedId = $("#InputId").val();
                    if (openedId == id) {
                        $(".content").html("<h2>Wybierz wejście z listy lub stwórz nowe</h2>");
                    }
                }
                if (typeof (systemStateAlerts) !== 'undefined') {
                    systemStateAlerts();
                }
            }
        });
    }, 200);
}

function removeOutput(obj, isOutputsList) {
    var id = $(obj).attr("data-id");
    setTimeout(function () {
        var url = "/Outputs/Remove/" + id;
        $.ajax({
            url: url,
            type: "POST",
            success: function (response) {
                getOutputsList($("#SysId").attr("data-system-id"));
                refreshSystemMenu();
                if (isOutputsList) {
                    var openedId = $("#OutputId").val()
                    if (openedId == id) {
                        $(".content").html("<h2>Wybierz wyjście z listy lub stwórz nowe</h2>");
                    }
                }
                //if ($.isFunction(systemStateAlerts)) {
                if (typeof(systemStateAlerts) !== 'undefined') {
                    systemStateAlerts();
                }
            }
        });
    }, 200);
}

function getInputsList(systemId) {
    var url = "/Inputs/InputsList/?systemId=" + systemId;
    $.ajax({
        url: url,
        type: "GET",
        success: function (response) {
            $(".inputs-list").parent().replaceWith(response);
        }
    })
}

function getOutputsList(systemId) {
    var url = "/Outputs/OutputsList/?systemId=" + systemId;
    $.ajax({
        url: url,
        type: "GET",
        success: function (response) {
            $(".outputs-list").parent().replaceWith(response);
        }
    })
}

function RefreshRules(systemId) {
    var url = "/Rules/SystemRules/?systemId=" + systemId;
    $.ajax({
        url: url,
        type: "GET",
        success: function (response) {
            $(".body-content").html(response);
            printSaveSuccess();
            changeRulesButtonsState(true);
            $("#alert-container").show();
        }
    })
}

function addInput() {
    var systemId = $(".inputs-list").attr("data-id");
    var url = "/Inputs/Add/?systemId=" + systemId;
    $.ajax({
        url: url,
        type: "POST",
        success: function (response) {
            $(".body-content").html(response);
        }
    })
}

function addOutput() {
    var systemId = $(".outputs-list").attr("data-id");
    var url = "/Outputs/Add/?systemId=" + systemId;
    $.ajax({
        url: url,
        type: "POST",
        success: function (response) {
            $(".body-content").html(response);
        }
    })
}

function collectData() {
    var variables = [];
    $(".variable-row").each(function () {
        var id = $(this).attr("id");
        var name = $(this).find("#Name").val();
        var type = $(this).find("#Type").val();
        var variableData = {};
        $(this).find(".chart-data-input").each(function () {
            var pointValue = parseFloat($(this).val().replace(',', '.'));
            var pointName = $(this).attr("id");
            variableData[pointName] = pointValue;
        })
        var variable = {
            Name: name,
            VariableId: id,
            Type: type,
            FunctionData: variableData
        };
        variables.push(variable);
    });

    var VM = {
        Name: $("#InputName").val(),
        //InputId: $("#InputId").val(),
        SystemId: $("#SysId").attr("data-system-id"),
        Variables: variables
    };

    return VM;
}

function collectRulesData() {
    var rules = [];
    $(".rules-table tbody tr").each(function (i, v) {
        var elements = [];
        $(v).find(".element-variable").each(function (i, v) {
            var operation = $(v).next("td").find("select").val();
            if (operation == null) {
                operation = 2;
            }
            var negation = $(v).find("input[type=checkbox]").prop("checked");
            var elem = {
                ElementId: $(v).find(".elementId").val(),
                InputOutputId: $(v).find(".elementInputOutputId").val(),
                Type: $(v).find(".elementType").val(),
                VariableId: $(v).find(".variableId").val(),
                VariableName: "",
                InputOutputName: "",
                NextOperation: operation,
                IsNegation: negation
            }
            elements.push(elem);
        })
        
        var rule = {
            RuleId: $(v).attr("id"),
            SystemId: $("#SysId").attr("data-system-id"),
            RuleElements: elements
        }
        rules.push(rule);
    });
    return rules;
}

function collectCalcData() {
    var values = {};
    $(".inputs-values .form-group").each(function (i, v) {
        var value = $(v).find("input").val();
        var name = $(v).find("label").attr("data-name");
        values[name] = parseFloat(value);
    });
    var vm = {
        InputsValues: values,
        SystemId: $("#SystemIdTemp").val(),
        AndMethod: $("#AndMethod").val(),
        OrMethod: $("#OrMethod").val()
    }
    return vm;
}

function collectPublishData() {
    var vm = {
        SystemId: $("#SysId").attr("data-system-id"),
        Description: $("#descInput").val(),
        Author: $("#authorInput").val(),
        Name: $("#nameInput").val()
    };
    return vm;
}

function printSaveSuccess(){
    $("#alert-container").removeClass("alert-danger");
    $("#alert-container").addClass("alert-success");
    $("#alert-container #alert-message").text("Zapisano pomyślnie!");
}

function printSaveErrors(response){
    $("#alert-container").removeClass("alert-success");
    $("#alert-container").addClass("alert-danger");
    var msg = "";
    $.each(response.errors, function (i, v) {
        if (msg.length > 0) msg += "<br>";
        msg += v;
        msg += ".";

    })
    $("#alert-container #alert-message").html(msg);
}

function numerateRulesRows() {
    $(".rules-table tr").each(function (i, v) {
        $(v).find(".index").text(i++);
    });
}

function refreshSystemMenu() {
    var id = $("#SysId").attr("data-system-id");
    var url = "/SystemOverview/SystemMenu/?systemId=" + id;
    $.ajax({
        url: url,
        type: "GET",
        success: function (response) {
            $(".system-menu").replaceWith(response);
        }
    })
}

function checkReadOnly() {
    var id = $("#SysId").attr("data-system-id");
    var url = "/Systems/IsSystemPublished/?systemId=" + id;
    $.ajax({
        url: url,
        type: "POST",
        success: function (response) {
            console.log(response);
            setReadOnly(response);
        }
    });
}


function setReadOnly() {
        //general
        $("#btnPublishModal").prop("disabled", true);
        //$('#btnPublishModal').bind('click', false);
        //$('#btnPublishModal').addClass('disabled');
        $('.btn-add').bind('click', false);
        $('.btn-add').addClass('disabled');
        $('.remove-input').hide();
        $('.remove-output').hide();
        //$('.remove-input').bind('click', false);
        //$('.remove-output').bind('click', false);
        //inputs/outputs
        $("input, select").prop("disabled", true);
        $("#addVariable").prop("disabled", true);
        $('.remove-variable').hide();
        //$('.remove-variable').bind('click', false);
        //rules
        $('.remove-rule').hide();
        $("#btnAdd").prop("disabled", true);
}
