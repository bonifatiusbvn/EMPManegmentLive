﻿$(document).ready(function () {
    GetUserTaskDetails();  
});


function showadddetails() {
 
    ClearTextBox();
    GetTaskType();
    GetUsername();
    $('#adddeals').modal('show');
}


function ClearTextBox() {

    $("#taskType").find("option").remove().end().append(
        '<option selected disabled value = "">--Select Task Type--</option>');
    $("#dealTitle").val('');
    $("#ddlusername").find("option").remove().end().append(
        '<option selected disabled value = "">--Select Username--</option>');
    $("#txtdatetime").val('');
    $("#txtenddatetime").val('');
    $("#contactDescription").val('');
}

function ClearTaskDetails()
{
    $("#txttaskStatus").val("");
}

function GetTaskType() {

    $.ajax({
        url: '/Task/GetTaskType',
        success: function (result) {

            $.each(result, function (i, data) {
                $('#taskType').append('<Option value=' + data.taskId + '>' + data.taskType + '</Option>')
            });
        }
    });
}

function TaskTypetext(sel) {
    $("#txtTaskType").val((sel.options[sel.selectedIndex].text));
}

function btnSaveTaskDetail() {

    if ($('#frmtaskdetails').valid()) {
        var formData = new FormData();
        formData.append("TaskType", $("#taskType").val());
        formData.append("TaskTitle", $("#dealTitle").val());
        formData.append("UserId", $("#ddlusername").val());
        formData.append("TaskDate", $("#txtdatetime").val());
        formData.append("TaskEndDate", $("#txtenddatetime").val());
        formData.append("TaskDetails", $("#contactDescription").val());
        $.ajax({
            url: '/Task/AddTaskDetails',
            type: 'Post',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (Result) {

                if (Result.message != null) {
                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/Task/UserTasks';
                    });
                }
            }
        })
    }
    else {
        Swal.fire({
            title: "Kindly Fill All Datafield",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        })
    }
}

function GetUsername() {
    
    $.ajax({
        url: '/Task/GetUserName',
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
    $("#frmtaskdetails").validate({
        rules: {
            taskType: "required",
            dealTitle: "required",
            ddlusername: "required",
            txtdatetime: "required",
            txtenddatetime: "required",
            contactDescription: "required",
        },
        messages: {
            taskType: "Please Enter Deal Type",
            dealTitle: "Please Enter Deal Title",
            ddlusername: "Please Select UserName",
            txtdatetime: "Please Enter Start Date",
            txtenddatetime: "Please Enter End Date",
            contactDescription: "Please Enter Description",
        }
    })
    $('#taskDetails').on('click', function () {
        TaskType1 = $('#taskType').val();
        if (TaskType1 == "") { 
            $('#taskType').attr("aria-invalid", "true");
            $("label[for='taskType']").addClass('failed');
        }
        UserId1 = $('#ddlusername').val();
        if (UserId1 == "") {
            $('#ddlusername').attr("aria-invalid", "true");
            $("label[for='ddlusername']").addClass('failed');
        }
    });
});


function GetUserTaskDetails() {
    $.ajax({
        url: '/Task/GetAllUserTaskDetail',
        type: 'Get',
        dataType: 'json',
        processData: false,
        contentType: false,
        complete: function (Result) {
            $('#dvtskdetail').html(Result.responseText);
            ClearTaskDetails();
        }
    })
}

function btnStatusUpdate(Id) { 

    var StausChange = {
        TaskStatus: $('#ddlStatus' + Id).val(),
        Id : Id
    }
    var form_data = new FormData();
    form_data.append("STATUSUPDATE", JSON.stringify(StausChange));

    $.ajax({
        url: '/Task/UpdateUserTaskStatus',
        type: 'Post',
        data: form_data,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (Result) {
            GetUserTaskDetails();
            Swal.fire({
                title: Result.message,
                icon: 'success',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK',
            })
        },
        error: function (error, status) {
            alert(error);
        }
    })
}

function btnTaskDetails(Id)
{
    $.ajax({
        url: '/Task/GetTaskDetailsById?Id=' + Id,
        type: "Get",
        contentType: 'application/json;charset=utf-8;',
        dataType: 'json',
        success: function (response) {

            $('#showDetailsModal').modal('show');
            $('#taskTitle-field').text(response.taskTitle);
            $('#taskDescription-field').text(response.taskDetails);
            var startdate = response.taskDate;
            var StartDate = startdate.substr(0, 10);
            $('#taskstartdate-field').text(StartDate);
            var enddate = response.taskEndDate;
            var EndDate = enddate.substr(0, 10);
            $('#taskenddate-field').text(EndDate);
            $('#taskpriority-field').text(response.taskTypeName);
            $('#taskstatus-field').text(response.taskStatus); 
        },
        error: function () {
            alert('Data not found');
        }
    });
}
