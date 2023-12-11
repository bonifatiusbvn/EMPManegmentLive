$(document).ready(function () {
    GetUserAttendance();
    GetAttendance();
});
function GetUserAttendance() {
    siteloadershow();
    $('#AttendanceTableData').DataTable({
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
                    return (new Date(full.date)).toLocaleDateString('en-US');
                }
            },
            {
                "data": "intime", "name": "intime",
                "render": function (data, type, full) {
                    return (new Date(full.intime)).toLocaleTimeString('en-US');
                }
            },
            {
                "data": "outTime", "name": "OutTime",
                "render": function (data, type, full) {
                    var userdate = new Date(full.date).toLocaleDateString('en-US');
                    var todate = new Date().toLocaleDateString('en-US');
                    if (full.outTime != null) {
                        return (new Date(full.outTime)).toLocaleTimeString('en-US');
                    }
                    else if (full.outTime == null && userdate == todate)
                    {
                       return ("Pending..");
                    }
                    else {
                        return ("Missing");
                    }
                }
            },
            {
                "data": "totalHours", "name": "totalHours",
                "render": function (data, type, full) {
                    var userdate = new Date(full.date).toLocaleDateString('en-US');
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
                    return '<a class="btn btn-sm btn-primary edit-item-btn" onclick="EditUserAttendance(\'' + full.attendanceId + '\')">EditTime</a>';
                }
            },

        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all",
        }]
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
                $('#Date').val((new Date(item.date)).toLocaleDateString('en-US'));
                $('#Intime').val((new Date(item.intime)).toLocaleTimeString('en-US'));
                $('#OutTime').val(item.outTime);
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
                        window.location = '/UserDetails/GetUsersListById';
                    });

                }
                
            },

        })

    }


   
}

function GetAttendance() {
    
    var month = $('#txtmonth').val();
    var form_data = new FormData();
    form_data.append("FINDBYMONTH", JSON.stringify(month));
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
                    object += '<td>' + (new Date(item.date)).toLocaleDateString('en-US') + '</td>';
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