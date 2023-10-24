
function ClearTextBox() {
    $("#dealType").val(null);
    $("#dealTitle").val('');
    $("#ddlusername").val(null);
    $("#txtdatetime").val('');
    $("#txtenddatetime").val('');
    $("contactDescription").val('');
}

function GetTaskType() {

    $.ajax({
        url: '/Home/GetTaskType',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#dealType').append('<Option value=' + data.taskId + '>' + data.taskType + '</Option>')
            });
        }
    });
}

function TaskTypetext(sel) {
    $("#txtDealType").val((sel.options[sel.selectedIndex].text));
}

function btnSaveTaskDetail() {
    debugger
    $("#frmtaskdetails").removeData("validator");
    debugger
    var formData = {};
    debugger

    debugger
    $('#frmtaskdetails').valid()
    { 
    formData = {
        TaskType: $("#dealType").val(),
        TaskTitle: $("#dealTitle").val(),
        UserId: $("#ddlusername").val(),
        TaskDate: $("#txtdatetime").val(),
        TaskEndDate: $("#txtenddatetime").val(),
        TaskDetails: $("#contactDescription").val()
    };
    AddTaskDetail(formData)
    }
}


function AddTaskDetail(formData) {
    debugger
    var form_data = new FormData();
    form_data.append("MEMBERREQUEST", JSON.stringify(formData));
    debugger
    $.ajax({
        url: '/Home/AddTaskDetails',
        type: 'Post',
        data: form_data,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (Result) {
            debugger
            Swal.fire({
                title: Result.message,
                icon: 'success',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK'
            }).then(function () {
                window.location = '/Home/UserHome';
            });
        }
    })

}



function GetUsername() {
    
    $.ajax({
        url: '/Home/GetUserName',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#ddlusername').append('<Option value=' + data.id + '>' + data.userName + '</Option>')
            });
        }
    });
}

function Usernametext(sel) {
    $("#txtguserid").val((sel.options[sel.selectedIndex].text));
}

$(document).ready(function () {
    debugger
    GetTaskType();
    GetUsername();
    ClearTextBox();
});