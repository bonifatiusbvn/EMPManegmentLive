
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

$('#selectSearchAttandanceOption').change(function () {
    if ($("#selectSearchAttandanceOption").val() == "ByUsername") {
        GetUsername();
        $("#usernamebox").show();
        $("#datesbox").hide();
        $("#startdatebox").hide();
        $("#enddatebox").hide();
        cleartextBox();
    }
    if ($("#selectSearchAttandanceOption").val() == "ByDate") {
        $("#usernamebox").hide();
        $("#datesbox").show();
        $("#startdatebox").hide();
        $("#enddatebox").hide();
        cleartextBox();
    }
    if ($("#selectSearchAttandanceOption").val() == "ByDate&ByUsername") {
        GetUsername();
        $("#usernamebox").show();
        $("#datesbox").show();
        $("#startdatebox").hide();
        $("#enddatebox").hide();
        cleartextBox();
    }
    if ($("#selectSearchAttandanceOption").val() == "ByBetweenDates&ByUsername") {
        GetUsername();
        $("#usernamebox").show();
        $("#datesbox").hide();
        $("#startdatebox").show();
        $("#enddatebox").show();
        cleartextBox();
    }
})
$('#SelectAttandance').change(function () {
    if ($("#SelectAttandance").val() == "ByMonth") {
        $("#monthbox").show();
        $("#datebox").hide();
        $("#datebox1").hide();
        CleartextBox();
    }
    if ($("#SelectAttandance").val() == "BetweenDates") {
        $("#monthbox").hide();
        $("#datebox").show();
        $("#datebox1").show();
        CleartextBox();
    }
    if ($("#SelectAttandance").val() == "") {
        $("#monthbox").show();
        $("#datebox").hide();
        $("#datebox1").hide();
        CleartextBox();
    }
})


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
                render: function (data) {
                    var dateObj = new Date(data);
                    var day = ('0' + dateObj.getDate()).slice(-2);
                    var month = ('0' + (dateObj.getMonth() + 1)).slice(-2);
                    var year = dateObj.getFullYear();
                    return day + '-' + month + '-' + year;
                }
            },
            {
                "data": "intime", "name": "InTime",
                render: function (data) {
                    return new Date(data).toLocaleTimeString('en-GB');
                }
            },
            {
                "data": "outTime", "name": "OutTime",
                render: function (data) {
                    return new Date(data).toLocaleTimeString('en-GB');
                }
            },
            {
                "data": "totalHours", "name": "TotalHours",
                render: function (data, type, full) {
                    var userDate = new Date(full.date).toLocaleDateString('en-GB');
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
            ]
        });
    }

    data(datas);
});




function EditUserAttendance(attandenceId) {
    $('#EditTimeModel').modal('show');
    $.ajax({
        url: '/UserProfile/EditUserAttendanceOutTime?attendanceId=' + attandenceId,
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
            url: '/UserProfile/UpdateUserAttendanceOutTime',
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
                var userdate = new Date(item.date).toLocaleDateString('en-US');
                var todate = new Date().toLocaleDateString('en-US');
                object += '<tr>';
                object += '<td>' + item.userName + '</td>';
                var formattedDate = formatDate(new Date(item.date));
                object += '<td>' + formattedDate + '</td>';
                object += '<td>' + (new Date(item.intime)).toLocaleTimeString('en-US') + '</td>';
                //---------OutTime---------//
                if (item.outTime != null) {
                    object += '<td>' +
                        (new Date(item.outTime)).toLocaleTimeString('en-US') + '</td>';
                }
                else if (item.outTime == null && userdate == todate) {

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
                else if (item.totalHours == null && userdate == todate) {
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
                debugger
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
        url: '/UserProfile/EditUserAttendanceOutTime?attendanceId=' + attandenceId,
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
            url: '/UserProfile/UpdateUserAttendanceOutTime',
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
            url: '/UserProfile/UpdateUserAttendanceOutTime',
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

