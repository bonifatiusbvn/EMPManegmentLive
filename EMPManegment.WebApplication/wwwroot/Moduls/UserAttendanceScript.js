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
                object += '<tr>';
                object += '<td>' + item.userName + '</td>';
                object += '<td>' + (new Date(item.date)).toLocaleDateString('en-US') + '</td>';
                object += '<td>' + (new Date(item.intime)).toLocaleTimeString('en-US') + '</td>';
                object += '<td>' + (new Date(item.outTime)).toLocaleTimeString('en-US') + '</td>';
                object += '<td>' + (item.totalHours.substr(0, 8)) + ('hr') + '</td>';
                object += '<td><a class="btn btn-sm btn-primary edit-item-btn" onclick="EditUserAttendance(\'' + item.attendanceId + '\')">EditTime</a></td>';
                object += '</tr>';
            });
            $('#TableDataAttendance').html(object);
        },
    });
};

function EditUserAttendance(attandenceId) {
    debugger
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
                $('#Date').val(item.date);
                $('#Intime').val(item.intime);
                $('#OutTime').val(item.outTime);
                
            });
        },
        error: function () {
            alert('Data not Found');
        }
    })
}


function UpdateUserAttendance() {
    debugger
    var objData = {
        OutTime: $("#OutTime").val(),
    }

    $.ajax({
        url: '/UserDetails/UpdateUserAttendanceOutTime',
        type: 'Post',
        data: objData,
        dataType: 'json',
        success: function () {
            alert("Data Successfully Updated!");
            window.location.reload();
        },
        error: function () {
            alert('There is some problem in your request.');
        }
    })
}
