function BindDropDownWithSelectAsNull(data, id) {
    var districtHtml = "<option value=''>---Select---</option>";

    $.each(data, function (index, District) {
        districtHtml += "<option value=" + District.value + ">" + District.text + "</option>";

    });
    $(id).html(districtHtml);
}

function BootStrapCalenderDesp(Id) {
    var date_input = $(Id); //our date input has the name "date"
    var container = $('.bootstrap-iso form').length > 0 ? $('.bootstrap-iso form').parent() : "body";
    var options = {
        format: 'dd-mm-yyyy',
        container: container,
        todayHighlight: true,
        autoclose: true,
    };
    date_input.datepicker(options);
}

function BindGridWithIdCol(ExcelColumns, PdfColumns, Id, title) {
    if ($(Id).DataTable().context.length > 0) {
        var table = $(Id).DataTable();
        var tableArray = [
            {
                extend: 'excel',
                title: title,
                text: '<img src="../images/excel.png" style="width:20px; heigth:10px;">',
                exportOptions: {
                    columns: ExcelColumns
                }
            },
            {
                extend: 'pdfHtml5',
                title: title,
                text: '<img src="../images/pdf.png" style="width:20px; heigth:10px;">',
                exportOptions: {
                    columns: PdfColumns
                }
            },
            {//To be unable column visibility
                extend: 'colvis',
                columns: ':not(.noVis)'
            }
        ]
        new $.fn.dataTable.Buttons(table, {
            buttons: tableArray
        });

        $("select[name='HrmsGrid_length']").css("height", "37");

        table.buttons(0, null).container().prependTo(
            table.table().container()
        )
    }
}




function GetCurrentDateTime() {
    var now = new Date();
    var utcString = now.toISOString().substring(0, 19);
    var year = now.getFullYear();
    var month = now.getMonth() + 1;
    var day = now.getDate();
    var hour = now.getHours();
    var minute = now.getMinutes();
    var second = now.getSeconds();
    var localDatetime = year + "-" +
        (month < 10 ? "0" + month.toString() : month) + "-" +
        (day < 10 ? "0" + day.toString() : day) + "T" +
        (hour < 10 ? "0" + hour.toString() : hour) + ":" +
        (minute < 10 ? "0" + minute.toString() : minute) +
        utcString.substring(16, 19);
    return(localDatetime);
}


function DateJSONtoJS(date) {
    var dt = new Date(date);
    var formatted_date = dt.getDate();
    var formatted_month = (dt.getMonth() + 1).toString();
    var formatted_year = dt.getFullYear();
    return (formatted_date.toString().padStart(2, '0') + "-" + formatted_month.toString().padStart(2, '0') + "-" + formatted_year);
}

function DateJSONtoHTML(date, vformat) {
    //specify Format in vformat :
    // 1. For <input type="date"> -> "YYYY-MM-DD"       
    // 2. For ShortDate : "DD-MM-YYYY"
    // 3. For LongDate with timestamp : "MM/dd/yyyy HH:mm:ss"
    var dt3 = moment(date).format(vformat);
    return (dt3);
} 

function ValuetoDecimal(val, decimal) {
    debugger;
    var dt3 = null;
    if (decimal == null) {
        decimal = 3;
    }
    if (val != null) {
        //dt3 = (Math.round(val * 100) / 100).toFixed(decimal); // using ToFixed
        dt3 = Math.round(val * Math.pow(10, decimal)) / Math.pow(10, decimal);
    } else {
        dt3 = '';
    }  
    return (dt3);
}

function ValuetoDecimalCeil(val, decimal) {
    debugger;
    var dt3 = null;
    if (decimal == null) {
        decimal = 3;
    }
    if (val != null) {
        dt3 = Math.ceil(val * Math.pow(10, decimal)) / Math.pow(10, decimal);
    } else {
        dt3 = '';
    }
    return (dt3);
}

function ValuetoDecimalFloor(val, decimal) {
    debugger;
    var dt3 = null;
    if (decimal == null) {
        decimal = 3;
    }
    if (val != null) {
        dt3 = Math.floor(val * Math.pow(10, decimal)) / Math.pow(10, decimal);
    } else {
        dt3 = '';
    }
    return (dt3);
}

function DespatchDivConfirm(message, url, fn) {
    debugger;
    CommonAlert('Alert', message, FnConfirm, { 'Url': url, 'Fn': fn }, 'alert', "confirmation");    
    
}

function FnConfirm(Data) {
    debugger;
    $(".modalLoader").css("display", "block");
    //CommonAjax(Data.Url, "Get", false, "application/json", false, ConfirmSuccess);
    $.ajax({
        url: Data.Url,
        type: "Get",
        async: false,
        contentType: "application/json",
        cache: false,
        success: function (response) {
            $("#commonModalbinder").css("display", "none");
            if (response.errorMessage != "") {
                CommonAlert(data.alert, response.errorMessage, null, null, "error");
            } else {
                if (response.alert != null) {
                    CallBack(Data.fn);
                    CommonAlert(response.alert, response.message, null, null, "success", response.alert.toLowerCase());
                    if (response.report != null && response.report != '') {
                        ReportInNewTab(response.report);
                    }
                }
            }
        },
        error: function (response) {
            CommonAlert("Error", response.statusText, null, null, "error");
            //alert(response.responseText);
        },
        failure: function (response) {
            CommonAlert("Failure", response.statusText, null, null, "error");
            //alert(response.responseText);
        }
    });
}

function ConfirmSuccess(response, fn) {    
    debugger;
    $("#commonModalbinder").css("display", "none");
    if (response.errorMessage != "") {
        CommonAlert(response.alert, response.errorMessage, null, null, "error");
    } else {
        if (response.alert != null) {
            fn();
            CommonAlert(response.alert, response.message, null, null, "success", response.alert.toLowerCase());
            if (response.report != null && response.report != '') {
                ReportInNewTab(response.report);
            }
        }
    }
}


function SuccessWithBootBox(response) {
    debugger;
    if (response.isAlertBox) {
        bootbox.alert({
            title: "<b>" + response.alert + "</b>",
            message: response.message,
            backdrop: true
        })
        if (response.report != "" && response.report != null) { ReportInNewTab(response.report); }
    }
    $(".modalLoader").hide();
}

function AlertWithBootBox(title, message) {
    debugger;   
    bootbox.alert({
        title: "<b>" + title + "</b>",
        message: message,
        backdrop: true
    })          
    $(".modalLoader").hide();
}


function DateJSONtoJSWithSlash(date) {
    var dt = new Date(date);
    var formatted_date = dt.getDate();
    var formatted_month = (dt.getMonth() + 1).toString();
    var formatted_year = dt.getFullYear();
    return (formatted_date.toString().padStart(2, '0') + "/" + formatted_month.toString().padStart(2, '0') + "/" + formatted_year);
}

function CommonReportGenerateJS(url, data) {
    debugger;
    $(".modalLoader").css("display", "block");
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            error: function (xhr, status, error) {

                CommonAlert(status, error, SubmitPopup, url, "error");
            },
            success: function (response) {
                var contentId = "/" + response.areaName + "/" + response.selectedMenu + "/GenerateReport";
                url = window.location.origin + contentId;

                if (response.errorMessage != null && response.errorMessage != "") {
                    CommonAlert(response.alert, response.errorMessage, null, null, "error");
                }
                $(".modalLoader").hide();
                if (response.report != null) {
                    var x = window.open('', '_blank');
                    x.document.write('<body></body>');
                    x.location.hash = response.selectedMenu;
                    var embedtag = x.document.createElement('embed');
                    embedtag.id = 'reportEmbed';
                    embedtag.src = response.report;
                    embedtag.style = "width:100%; height:100%;";
                    embedtag.alt = "pdf";
                    embedtag.title = "Report";
                    embedtag.type = "application/pdf";
                    embedtag.pluginspage = "http://www.adobe.com/products/acrobat/readstep2.html";
                    x.document.body.appendChild(embedtag);
                    x.document.title = response.selectedMenu;
                }
                

            }
        });                   
}

function ReportInNewTab(response) {
    var x = window.open('', '_blank');
    x.document.write('<body></body>');
    x.location.hash = response.selectedMenu;
    var embedtag = x.document.createElement('embed');
    embedtag.id = 'reportEmbed';
    embedtag.src = response;
    embedtag.style = "width:100%; height:100%;";
    embedtag.alt = "pdf";
    embedtag.title = "Report";
    embedtag.type = "application/pdf";
    embedtag.pluginspage = "http://www.adobe.com/products/acrobat/readstep2.html";
    x.document.body.appendChild(embedtag);
    x.document.title = response.selectedMenu;

}

$.fn.rowCount = function () {
    var length = $('tr', $(this).find('tbody').eq(0)).length;
    var arr = new Array(length);
    for (var i = 0; i < length; i++) {
        arr[i] = i;
    }
};

function ReturnDropDownWithoutSelect(data, id) {
    debugger;
    var districtHtml = "<option value=''>---Select---</option>";

    $.each(data, function (index, District) {
        districtHtml += "<option value=" + District.value + ">" + District.text + "</option>";

    });
    return districtHtml;
}

$.fn.columnCount = function () {
    debugger;
    var length = $('th', $(this).find('thead').eq(0)).length;
    var arr = new Array(length);
    for (var i = 0; i < length; i++) {
        arr[i] = i;
    }
    return arr;
};

function CommonSubmitWithReport(data, actionUrl, e) {
    
    e.preventDefault();

    $(".modalLoader").css("display", "block");
    var $form = $(this).parents("form");
    $.ajax({
        type: "POST",
        url: actionUrl,
        data: data,
        //processData: false,//Add for uploading content like pdf
        //contentType: false,//Add for uploading content like pdf
        error: function (xhr, response, error) {
            CommonAlert("Error", response, null, null, "error");
        },
        success: function (response) {
            debugger;
            var url = window.location.origin + "/" + response.areaName + "/" + response.selectedMenu + "/" + response.selectedAction;

            if (response.isAlertBox) {
                if (response.errorMessage != "") {
                    CommonAlert(response.alert, response.message, SubmitPopup, url, "error");
                } else {
                    CommonAlert(response.alert, response.message, SubmitPopup, url, "alert", "create");
                    if (response.report != "") { ReportInNewTab(response.report); }
                }
            } else {
                $(".modalLoader").hide();
                GetIndex(url);
            }

        }
    });
}

function DateDifference(fromdt, todt) {
    var startTime = moment(fromdt);    
    var endTime = moment(todt);
    var duration = moment.duration(startTime.diff(endTime));    
    var hours = parseInt(duration.asHours());
    var minutes = parseInt(duration.asMinutes()) % 60;
    var str = hours.toString().padStart(2, '0') + ":" + minutes.toString().padStart(2, '0');
    return str

}

function isTodayOrFuture(date) {
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    return date > today;
}

function isInTheFuture(date) {
    const today = new Date();
    today.setHours(23, 59, 59, 998);

    return date > today;
}


