$(document).ready(function () {
    GetUserTaskDetails();  
    AllTaskDetailsList();
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
            $('#UserName').text(response.userName);
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

function AllTaskDetailsList() {debugger
    $.ajax({
        url: '/Task/GetAllTaskDetailList',
        type: 'Get',
        dataType: 'json',
        contentType: 'application/json;charset=utf-8;',
        success: function (result) {
            var object = '';
            $.each(result, function (index, item) {debugger
                object += '<tr>';
                object += '<td><div class="avatar-group flex-nowrap"><a href="javascript: void(0);" class="avatar-group-item" data-img="/' + item.userProfile + '" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="John Robles"><img src="/' + item.userProfile + '" alt="" class="rounded-circle avatar-xxs"> </a><div class="flex-shrink-0 me-3">' + item.userName + '</div></div></td>';
                /*object += '<td> <a class="fw-medium link-primary">' + item.taskTitle + '</a> </td>';*/
                object += '<td><div class="d-flex"><div class="flex-grow-1 tasks_name">' + item.taskTitle + '</div><div class="flex-shrink-0 ms-4"><ul class="list-inline tasks-list-menu mb-0"><li class="list-inline-item"><a onclick="btnTaskDetails(\'' + item.id + '\')"><i class="ri-eye-fill align-bottom me-2 text-muted"></i></a></li><li class="list-inline-item"><a class="edit-item-btn" href="#showModal" data-bs-toggle="modal"><i class="ri-pencil-fill align-bottom me-2 text-muted"></i></a></li><li class="list-inline-item"><a class="remove-item-btn" data-bs-toggle="modal" href="#deleteOrder"><i class="ri-delete-bin-fill align-bottom me-2 text-muted"></i></a></li></ul></div></div></td>';
                object += '<td>' + item.taskDetails + '</td>';

                //------- Task Type ---------//
                if (item.taskTypeName == "HighPriority") {debugger
                    object += '<td><span class="badge bg-danger-subtle text-danger text-uppercase">' + item.taskTypeName + '</span></td>';
                }
                else if (item.taskTypeName == "MediumPriority") {
                    object += '<td><span class="badge bg-warning-subtle text-warning text-uppercase">' + item.taskTypeName + '</span></td>';
                }
                else {
                    object += '<td><span class="badge bg-success-subtle text-success text-uppercase">' + item.taskTypeName + '</span></td>';
                }

                object += '<td>' + (new Date(item.taskDate)).toLocaleDateString('en-US') + '</td>';
                object += '<td>' + (new Date(item.taskEndDate)).toLocaleDateString('en-US') + '</td>';

                //--------- Task Status ----------//
                if (item.taskStatus == "Working") {
                    object += '<td><a class="badge bg-warning text-uppercase">' + item.taskStatus + '</a></td>';
                }
                else if (item.taskStatus == "Completed") {
                    object += '<td><a class="badge bg-success text-uppercase">' + item.taskStatus + '</a></td>';
                }
                else if (item.taskStatus == "Pending") {
                    object += '<td><a class="badge bg-primary text-uppercase">' + item.taskStatus + '</a></td>';
                }
                else {
                    object += '<td><a class="badge bg-secondary text-uppercase">' + item.taskStatus + '</a></td>';
                }
                object += '</tr>';
            });
            $('#AllTaskDetails').html(object);
        },
        error: function () {
            alert("data can't get");
        }
    });
};
