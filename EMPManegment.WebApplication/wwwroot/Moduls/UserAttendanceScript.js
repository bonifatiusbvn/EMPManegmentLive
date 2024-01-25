$("#monthbox").show();
$("#datebox").hide();
$("#datebox1").hide();
$("#usernamebox").show();
$("#datesbox").hide();
$("#backbtn").hide();
function cleartextBox() {
    $("#ddlusername").find("option").remove().end().append(
        '<option selected value = "">--Select Username--</option>');
    $("#txtdate").val('');
}
$(document).ready(function () {
    GetUserAttendance(); 
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
    if($("#selectSearchAttandanceOption").val() == "ByUsername")
    {
        GetUsername();
        $("#usernamebox").show();
        $("#datesbox").hide();
              cleartextBox();
    }
    if ($("#selectSearchAttandanceOption").val() == "ByDate") {
        $("#usernamebox").hide();
        $("#datesbox").show();
        cleartextBox();
    }
    if ($("#selectSearchAttandanceOption").val() == "ByDate&ByUsername") {
        GetUsername();
        $("#usernamebox").show();
        $("#datesbox").show();
        cleartextBox();
    }
})
$('#SelectAttandance').change(function () {
    if ($("#SelectAttandance").val() == "ByMonth") {
        $("#monthbox").show();
        $("#datebox").hide();
        $("#datebox1").hide();
    }
    if ($("#SelectAttandance").val() == "BetweenDates") {
        $("#monthbox").hide();
        $("#datebox").show();
        $("#datebox1").show();
    }
    if ($("#SelectAttandance").val() == "") {
        $("#monthbox").show();
        $("#datebox").hide();
        $("#datebox1").hide();
    }
})

function GetUserAttendance() {

     $('#attendanceTableData').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "Post",
            url: '/UserProfile/GetUserAttendanceList',
            dataType: 'json'
        },
        columns: [
            { "data": "userName", "name": "UserName" },
            {
                "data": "date", "name": "Date",
                "render": function (data, type, full) {
                    return (new Date(full.date)).toLocaleDateString('en-GB');
                }
            },
            {
                "data": "intime", "name": "InTime",
                "render": function (data, type, full) {
                    return (new Date(full.intime)).toLocaleTimeString('en-GB');
                }
            },
            {
                "data": "outTime", "name": "OutTime",
                "render": function (data, type, full) {
                    var userdate = new Date(full.date).toLocaleDateString('en-GB');
                    var todate = new Date().toLocaleDateString('en-GB');
                    
                     if (full.outTime != null) {
                        return (new Date(full.outTime)).toLocaleTimeString('en-GB');
                    }
                    else if (full.outTime == null && userdate == todate)
                    {
                       return ("Pending..");
                    }
                    else  {
                         return ("Missing");
                    }
                }
            },
            {
                "data": "totalHours", "name": "TotalHours",
                "render": function (data, type, full) {
                    var userdate = new Date(full.date).toLocaleDateString('en-GB');
                    var todate = new Date().toLocaleDateString('en-US');
                    if (full.totalHours != null) {
                        return (full.totalHours?.substr(0, 8)) + ('hr');
                    }
                    else if (full.totalHours == null && userdate == todate) {
                        return ("Pending...");
                    }
                    else {
                        return ("Missing");
                    }
                }
            },
            {
                "render": function (data, type, full) {
                    return '<a class="btn btn-sm btn-primary edit-item-btn" onclick="EditUserAttendance(\'' + full.attendanceId + '\')" >EditTime</a>';
                }
            },
        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all",
        }],
    });
}


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
            alert('Data not Found');
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
                        window.location = '/UserProfile/GetUsersListById';
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
                    object += '<td>' + (new Date(item.date)).toLocaleDateString('en-GB') + '</td>';
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
                $('#TableDataAttendanceList').html(object);
            }      
        }

    });
};

function GetSearchAttendanceList() {

    if ($('#attendanceform').valid()) {
        var form_data = new FormData();
        form_data.append("Date", $('#txtdate').val());
        form_data.append("UserId", $("#ddlusername").val());
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
            title: "Kindly Fill the Status",
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
            alert('Data not Found');
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
                        window.location = '/UserProfile/GetUsersListById';
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
                        window.location = '/UserProfile/GetUsersListById';
                    });

                }

            },

        })
    }
}

//function exportToExcel()
//{
//    
//    $.ajax({
//        url: '/UserProfile/ExportToExcel',
//        type: 'Get',
//        datatype: 'json',
//        processData: false,
//        contentType: false,
//        success: function (Result) {
//            
//            var bytes = new Uint8Array(Result.fileContents);
//            var blob = new Blob([bytes], { type: "application/vnd.openxmlformats-officedocuments.spreadsheetml.sheet" });
//            
//            var link = document.createElement('a');
//            link.href = window.URL.createObjectURL(blob);
//            link.download = Result.fileDownloadName;
//            link.click();
//            Swal.fire({
//                title: 'Successfully Download',
//                icon: 'success',
//                confirmButtonColor: '#3085d6',
//                confirmButtonText: 'OK',
//            }).then(function () {
//                window.location = '/UserProfile/GetAttendance';
//            });

//        }
//    });
//}