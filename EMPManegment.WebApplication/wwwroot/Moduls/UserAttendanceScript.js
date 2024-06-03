
var datas = userPermissions

$("#monthbox").show();
$("#datebox").hide();
$("#datebox1").hide();
$("#usernamebox").show();
$("#datesbox").hide();
$("#backbtn").hide();
$("#startdatebox").hide();
$("#enddatebox").hide();
function cleartextBox() {
    $("#ddlusername").find("option").remove().end().append(
        '<option selected value = "">--Select Username--</option>');
    $("#txtdate").val('');
}

function CleartextBox() {
    $("#txtmonth").val('');
    $("#txtstartdate").val('');
    $("#txtenddate").val('');
}
$(document).ready(function () {
    GetAttendance();
    $("#attendanceform").validate({
        rules: {
            ddlusername: "required",
            txtdate: "required"
        },
        messages: {
            ddlusername: "Please Enter UserName",
            txtdate: "Please Enter Date"
        }
    })
    $('#searchattendanceform').on('click', function () {
        $("#attendanceform").validate();
    });
});

$('.dropdown-item').click(function () {
    var selectedValue = $(this).attr('data-value');

    if (selectedValue === "ByUsername") {
        GetUsername();
        $("#usernamebox").show();
        $("#datesbox").hide();
        $("#startdatebox").hide();
        $("#enddatebox").hide();
        cleartextBox();
    }
    if (selectedValue === "ByDate") {
        $("#usernamebox").hide();
        $("#datesbox").show();
        $("#startdatebox").hide();
        $("#enddatebox").hide();
        cleartextBox();
    }
    if (selectedValue === "ByDate&ByUsername") {
        GetUsername();
        $("#usernamebox").show();
        $("#datesbox").show();
        $("#startdatebox").hide();
        $("#enddatebox").hide();
        cleartextBox();
    }
    if (selectedValue === "ByBetweenDates&ByUsername") {
        GetUsername();
        $("#usernamebox").show();
        $("#datesbox").hide();
        $("#startdatebox").show();
        $("#enddatebox").show();
        cleartextBox();
    }
});
$(document).ready(function () {
    $('.dropdown-item').on('click', function () {
        var selectedValue = $(this).data('value');
        $('#SelectAttandance').val(selectedValue);

        if (selectedValue == "ByMonth") {
            $("#monthbox").show();
            $("#datebox").hide();
            $("#datebox1").hide();
            CleartextBox();
        } else if (selectedValue == "BetweenDates") {
            $("#monthbox").hide();
            $("#datebox").show();
            $("#datebox1").show();
            CleartextBox();
        } else {
            $("#monthbox").show();
            $("#datebox").hide();
            $("#datebox1").hide();
            CleartextBox();
        }


        $('.btn-group .dropdown-toggle').text($(this).text());
    });

    function CleartextBox() {

        $('#monthbox').val('');
        $('#datebox').val('');
        $('#datebox1').val('');
    }
});



$(document).ready(function () {
    function data(datas) {
        var userPermission = datas;
        GetUserAttendance(userPermission);
    }

    function GetUserAttendance(userPermission) {
        var userPermissionArray = JSON.parse(userPermission);
        var canEdit = userPermissionArray.some(permission => permission.formName === "Users Attendance" && permission.edit);
        var columns = [
            { "data": "userName", "name": "UserName" },
            {
                "data": "date", "name": "Date",
                "render": function (data, type, full, meta) {
                    return getCommonDateformat(data);
                }
            },
            {
                "data": "intime", "name": "InTime",
                render: function (data) {
                    return new Date(data).toLocaleTimeString('en-US');
                }
            },
            {
                "data": "outTime", "name": "OutTime",
                render: function (data, type, full) {
                    var userDate = new Date(full.date).toLocaleDateString('en-US');
                    var todayDate = new Date().toLocaleDateString('en-US');
                    if (data != null) {
                        return new Date(data).toLocaleTimeString('en-US');
                    }
                    else if (data == null && userDate == todayDate) {
                        return "Pending...";
                    }
                    else {
                        return "Missing";
                    }
                }
            },
            {
                "data": "totalHours", "name": "TotalHours",
                render: function (data, type, full) {
                    var userDate = new Date(full.date).toLocaleDateString('en-US');
                    var todayDate = new Date().toLocaleDateString('en-US');
                    if (full.totalHours != null) {
                        return full.totalHours.substr(0, 8) + ' hr';
                    } else if (full.totalHours == null && userDate === todayDate) {
                        return "Pending...";
                    } else {
                        return "Missing";
                    }
                }
            },
        ];

        if (canEdit) {
            columns.push({
                "data": null,
                "orderable": false,
                "searchable": false,
                "render": function (data, type, full) {
                    return '<a onclick="EditUserAttendance(\'' + full.attendanceId + '\')" class="btn text-primary btndeletedoc">' +
                        '<i class="fa-regular fa-pen-to-square"></i></a>';
                }
            });
        }

        $('#attendanceTableData').DataTable({
            processing: false,
            serverSide: true,
            filter: true,
            destroy: true,
            pageLength: 30,
            ajax: {
                type: "POST",
                url: '/UserProfile/GetUserAttendanceList',
                dataType: 'json'
            },
            columns: columns,
            scrollY: 400,
            scrollX: true,
            scrollCollapse: true,
            fixedHeader: {
                header: true,
                footer: true
            },
            autoWidth: false,
            columnDefs: [
                {
                    targets: '_all', width: 'auto'
                }
            ],
            order: [[1, 'asc']]
        });
    }


    data(datas);
});




function EditUserAttendance(attandenceId) {
    $('#EditTimeModel').modal('show');
    $.ajax({
        url: '/UserProfile/EditOutTime?attendanceId=' + attandenceId,
        type: 'Get',
        dataType: 'json',
        processData: false,
        contentType: false,
        success: function (response) {
            $.each(response, function (index, item) {
                $('#AttandanceId').val(item.attendanceId);
                $('#UserName').val(item.userName);
                $('#Date').val((new Date(item.date)).toLocaleDateString('en-GB'));
                $('#Intime').val((new Date(item.intime)).toLocaleTimeString('en-GB'));
                if (item.outTime == null) {
                    $('#OutTime').val(item.date);
                }
                else {
                    $('#OutTime').val(item.outTime);
                }
            });
        },
        error: function () {
            alert('Data not found');
        }
    })
}

function UpdateUserAttendance() {
    var objData = {
        AttendanceId: $("#AttandanceId").val(),
        OutTime: $("#OutTime").val(),
        Intime: $("#Intime").val(),
        UserName: $("#UserName").val(),
        Date: $("#Date").val(),
    }


    if (objData.OutTime == "") {
        $("#OutTime").css('border-color', 'red');
        $("#OutTime").focus();
    }

    else {
        $("#OutTime").css('border-color', 'lightgray');
        $.ajax({
            url: '/UserProfile/UpdateOutTime',
            type: 'Post',
            data: objData,
            dataType: 'json',
            success: function (Result) {

                var ricon = "warning";

                if (Result.icone == ricon) {

                    Swal.fire({
                        title: Result.message,
                        icon: Result.icone,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    })
                }

                else {

                    Swal.fire({
                        title: Result.message,
                        icon: Result.icone,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/UserProfile/GetAllUsersAttendanceList';
                    });
                }
            },
        })
    }
}


function GetAttendance() {

    var form_data = new FormData();
    form_data.append("Cmonth", $('#txtmonth').val());
    form_data.append("StartDate", $("#txtstartdate").val());
    form_data.append("EndDate", $("#txtenddate").val());
    $.ajax({
        url: '/UserProfile/GetAttendanceList',
        type: 'Post',
        datatype: 'json',
        data: form_data,
        processData: false,
        contentType: false,
        success: function (Result, status, xhr) {
            var object = '';
            $.each(Result, function (index, item) {
                var formattedDate = getCommonDateformat(item.date);
                var todate = new Date().toLocaleDateString('en-US');
                var date = (new Date(item.date)).toLocaleDateString('en-US')
                object += '<tr>';
                object += '<td>' + item.userName + '</td>';
                object += '<td>' + formattedDate + '</td>';
                object += '<td>' + (new Date(item.intime)).toLocaleTimeString('en-US') + '</td>';
                //---------OutTime---------//
                if (item.outTime != null) {
                    object += '<td>' +
                        (new Date(item.outTime)).toLocaleTimeString('en-US') + '</td>';
                }
                else if (item.outTime == null && date == todate) {

                    object += '<td>' +
                        ("Pending") + '</td>';
                }
                else {
                    object += '<td>' +
                        ("Missing") + '</td>';
                }
                //---------TotalHours--------//
                if (item.totalHours != null) {
                    object += '<td>' +
                        (item.totalHours?.substr(0, 8)) + ('hr') + '</td>';
                }
                else if (item.totalHours == null && date == todate) {
                    object += '<td>' +
                        ("Pending") + '</td>';
                }
                else {
                    object += '<td>' +
                        ("Missing") + '</td>';
                }
            });
            if (Result.message != null) {
                var msg = '';
                msg += '<td>' +
                    (Result.message) + '</td>'; msg += '<td>' +
                        (Result.message) + '</td>'; msg += '<td>' +
                            (Result.message) + '</td>'; msg += '<td>' +
                                (Result.message) + '</td>';
                msg += '<td>' +
                    (Result.message) + '</td>';
                $('#TableDataAttendanceList').html(msg);
                return

            }
            else {

                $('#TableDataAttendanceList').html(object);
            }
        }

    });
};
function formatDate(date) {
    var day = String(date.getDate()).padStart(2, '0');
    var month = String(date.getMonth() + 1).padStart(2, '0');
    var year = date.getFullYear();

    return day + '-' + month + '-' + year;
}

function GetSearchAttendanceList() {

    if ($('#attendanceform').valid()) {
        var form_data = new FormData();
        form_data.append("Date", $('#txtdate').val());
        form_data.append("UserId", $("#ddlusername").val());
        form_data.append("StartDate", $("#txtstartdatebox").val());
        form_data.append("EndDate", $("#txtenddatebox").val());
        $.ajax({
            url: '/UserProfile/GetSearchAttendanceList',
            type: 'Post',
            datatype: 'json',
            data: form_data,
            processData: false,
            contentType: false,
            complete: function (Result) {
                $("#attendancedt").hide();
                $("#backbtn").show();
                if (Result.responseText != '{\"code\":400}') {
                    $("#errorMessage").hide();
                    $("#dvattendancelist").show();
                    $("#dvattendancelist").html(Result.responseText);
                } else {
                    var message = "No Data Found On Selected Username Or Dates!!";
                    $("#errorMessage").show();
                    $("#errorMessage").text(message);
                    $("#dvattendancelist").hide();
                }
            }
        });
    } else {
        Swal.fire({
            title: "Kindly fill the status",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        })
    }
};

function editUserAttendanceSrc(attandenceId) {
    $.ajax({
        url: '/UserProfile/EditOutTime?attendanceId=' + attandenceId,
        type: 'Get',
        dataType: 'json',
        processData: false,
        contentType: false,
        success: function (response) {
            $.each(response, function (index, item) {
                $('#srcAttandanceId').val(item.attendanceId);
                $('#srcUserName').val(item.userName);
                $('#srcDate').val((new Date(item.date)).toLocaleDateString('en-GB'));
                $('#srcIntime').val((new Date(item.intime)).toLocaleTimeString('en-GB'));
                if (item.outTime == null) {
                    $('#srcOutTime').val(item.date);
                }
                else {
                    $('#srcOutTime').val(item.outTime);
                }
            });
            $('#editTimeModelsearch').modal('show');
        },
        error: function () {
            alert('Data not found');
        }
    })
}


function UpdateUserAttendanceSrc() {
    var objData = {
        AttendanceId: $("#srcAttandanceId").val(),
        OutTime: $("#srcOutTime").val(),
        Intime: $("#srcIntime").val(),
        UserName: $("#srcUserName").val(),
        Date: $("#srcDate").val(),
    }


    if (objData.OutTime == "") {
        $("#OutTime").css('border-color', 'red');
        $("#OutTime").focus();
    }

    else {
        $("#OutTime").css('border-color', 'lightgray');
        $.ajax({
            url: '/UserProfile/UpdateOutTime',
            type: 'Post',
            data: objData,
            dataType: 'json',
            success: function (Result) {

                var ricon = "warning";

                if (Result.icone == ricon) {

                    Swal.fire({
                        title: Result.message,
                        icon: Result.icone,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    })
                }

                else {

                    Swal.fire({
                        title: Result.message,
                        icon: Result.icone,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/UserProfile/GetAllUsersAttendanceList';
                    });
                }
            },
        })
    }
}

function updateUserAttendance() {
    var objData = {
        AttendanceId: $("#AttandanceId").val(),
        OutTime: $("#OutTime").val(),
        Intime: $("#Intime").val(),
        UserName: $("#UserName").val(),
        Date: $("#Date").val(),
    }


    if (objData.OutTime == null) {
        $("#OutTime").css('border-color', 'red');
        $("#OutTime").focus();
    }

    else {
        $("#OutTime").css('border-color', 'lightgray');
        $.ajax({
            url: '/UserProfile/UpdateOutTime',
            type: 'Post',
            data: objData,
            dataType: 'json',
            success: function (Result) {
                var ricon = "warning";
                if (Result.icone == ricon) {
                    Swal.fire({
                        title: Result.message,
                        icon: Result.icone,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    })
                }
                else {
                    Swal.fire({
                        title: Result.message,
                        icon: Result.icone,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/UserProfile/GetAllUsersAttendanceList';
                    });

                }

            },

        })
    }
}

function ExportToExcel() {
    siteloadershow();
    $.ajax({
        url: '/UserProfile/ExportToExcel',
        type: 'GET',
        success: function (data, status, xhr) {
            siteloaderhide();
            var filename = "";
            var disposition = xhr.getResponseHeader('Content-Disposition');
            if (disposition && disposition.indexOf('attachment') !== -1) {
                var matches = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/.exec(disposition);
                if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, '');
            }

            var type = xhr.getResponseHeader('Content-Type');
            var blob = new Blob([data], { type: type });

            if (typeof window.navigator.msSaveBlob !== 'undefined') {
                window.navigator.msSaveBlob(blob, filename);
            } else {
                var URL = window.URL || window.webkitURL;
                var downloadUrl = URL.createObjectURL(blob);

                if (filename) {
                    var a = document.createElement("a");
                    if (typeof a.download === 'undefined') {
                        window.location = downloadUrl;
                    } else {
                        a.href = downloadUrl;
                        a.download = filename;
                        document.body.appendChild(a);
                        a.click();
                    }
                } else {
                    window.location = downloadUrl;
                }

                setTimeout(function () { URL.revokeObjectURL(downloadUrl); }, 100);
            }
        },
        error: function (xhr, status, error) {
            alert("Error: " + error);
        },
        xhrFields: {
            responseType: 'blob'
        }
    });
}

function ExportToPDF() {
    siteloadershow();
    $.ajax({
        url: '/UserProfile/ExportToPdf',
        type: 'POST',
        success: function (data, status, xhr) {
            siteloaderhide();
            var filename = "";
            var disposition = xhr.getResponseHeader('Content-Disposition');
            if (disposition && disposition.indexOf('attachment') !== -1) {
                var matches = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/.exec(disposition);
                if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, '');
            }

            var type = xhr.getResponseHeader('Content-Type');
            var blob = new Blob([data], { type: type });

            if (typeof window.navigator.msSaveBlob !== 'undefined') {
                window.navigator.msSaveBlob(blob, filename);
            } else {
                var URL = window.URL || window.webkitURL;
                var downloadUrl = URL.createObjectURL(blob);

                if (filename) {
                    var a = document.createElement("a");
                    if (typeof a.download === 'undefined') {
                        window.location = downloadUrl;
                    } else {
                        a.href = downloadUrl;
                        a.download = filename;
                        document.body.appendChild(a);
                        a.click();
                    }
                } else {
                    window.location = downloadUrl;
                }

                setTimeout(function () { URL.revokeObjectURL(downloadUrl); }, 100);
            }
        },
        error: function (xhr, status, error) {
            alert("Error: " + error);
        },
        xhrFields: {
            responseType: 'blob'
        }
    });
}
