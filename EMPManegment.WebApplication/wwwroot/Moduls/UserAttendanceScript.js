$(document).ready(function () {
    GetUserAttendance();
});
function GetUserAttendance() {
    
    $.ajax({
        url: '/UserDetails/GetUserAttendanceList',
        type: 'Get',
        dataType: 'json',
        processData: false,
        contentType: false,
        success: function (result, status, xhr) {
            
            var object = '';
            $.each(result, function (index, item) {
                
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
                
                else if (item.outTime == null && userdate == todate)
                {
                    object += '<td>' +
                        ("Pending") + '</td>';

                }


                else {
                    object += '<td>' +
                        ("Missing") + '</td>';
                }
                //---------TotalHours--------//
                debugger
                if (item.totalHours != null) {
                    object += '<td>' +
                        (item.totalHours?.substr(0, 8)) + ('hr') + '</td>';
                }

                else if (item.totalHours == null && userdate == today) {
                    object += '<td>' +
                        ("Pending") + '</td>';

                }
                else {
                    object += '<td>' +
                        ("Missing") + '</td>';
                }
                object += '<td><a class="btn btn-sm btn-primary edit-item-btn" onclick="EditUserAttendance(\'' + item.attendanceId + '\')">EditTime</a></td>';
                object += '</tr>';
            });
            $('#TableDataAttendance').html(object);
        },
    });
};

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
        AttendanceId: $('#AttandanceId').val(),
        UserName: $('#UserName').val(),
        Date: $('#Date').val(),
        Intime: $('#Intime').val(),
        OutTime: $("#OutTime").val(),
        UserId: $("#UserId").val(),
    }

    $.ajax({
        url: '/UserDetails/UpdateUserAttendanceOutTime',
        type: 'Post',
        data: objData,
        dataType: 'json',
        success: function (Result) {
            Swal.fire({
                title: Result.message,
                icon: 'success',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK'
            }).then(function () {
                window.location = '/UserDetails/GetUsersListById';
            }); 
        },
        error: function () {
            alert('There is some problem in your request.');
        }
    })
}
