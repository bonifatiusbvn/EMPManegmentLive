$(document).ready(function () {
    GetUserAttendance();
});

function GetUserAttendance() {

    $('#AttendanceTableData').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "Post",
            url: '/UserDetails/GetUserAttendanceList',
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
        url: '/UserDetails/EditUserAttendanceOutTime?attendanceId=' + attandenceId,
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
            url: '/UserDetails/UpdateUserAttendanceOutTime',
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


///* Validate OutTime*/

//$("#outtime").hide();
//let outTimeError = true;
//$("#OutTime").keyup(function () {
//    validateOutTime();
//});

//function validateOutTime() {debugger
//    let outTimeValue = $("#OutTime").val();
//    if (outTimeValue.date!=today.date) {
//            $("#outtime").show();
//        outTimeError = false;
//            return false;
//    } else {
//            $("#outtime").hide();
//        }
//    }
///* Validate Submit Button */
//$("#SubmitButton").click(function () {
//    validateOutTime();
//    if (
//        outTimeError == true
//    ) {
//        return true;
//    } else {
//        return false;
//    }
//});