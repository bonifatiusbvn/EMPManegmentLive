$(document).ready(function () {
    
    GetAllUserTaskDetail()
    AllTaskDetailsList()
});


function showadddetails() { 
    ClearTextBox();
    GetTaskType();
    GetUsername();
    $('#addtasks').modal('show');
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
                        window.location = '/Task/AllTaskDetails';
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
                $('#ddlusername').append('<Option value=' + data.id + '>'  + data.firstName + " " + data.lastName + " " + "(" + data.userName + ")" + '</Option>')
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
            }).then(function () {
                window.location = '/Task/UserTasks';
            });
        },
        error: function (error, status) {
            alert(error);
        
        }
    })
}

function btnTaskDetails(Id){
    $.ajax({
        url: '/Task/GetTaskDetailsById?Id=' + Id,
        type: "get",
        contentType: 'application/json;charset=utf-8;',
        dataType: 'json',
        success: function (response) {
            $('#showDetailsModal').modal('show');
            $('#UserName').text(response.userName);
            $('#taskTitle-field').text(response.taskTitle);
            $('#taskDescription-field').text(response.taskDetails);
            //var startdate = response.taskDate;
            //var StartDate = startdate.substr(0, 10);
            $('#taskstartdate-field').text(response.taskDate);
            //var enddate = response.taskEndDate;
            //var EndDate = enddate.substr(0, 10);
            $('#taskenddate-field').text(response.taskEndDate);
            $('#taskpriority-field').text(response.taskTypeName);
            $('#taskstatus-field').text(response.taskStatus); 
        },
        error: function () {
            alert('Data not found');
        }
    });
}


function GetAllUserTaskDetail() {
    $.ajax({
        url: '/Task/GetAllUserTaskDetail',
        type: 'Get',
        dataType: 'json',
        contentType: 'application/json;charset=utf-8;',
        success: function (result)
        {
            var object = '';
            var pendingTask = result.filter(function (obj) {
                return (obj.taskStatus == "Pending");
            });
            
            var nullTask = result.filter(function (obj) {
                    return (obj.taskStatus == null);
            });

            
            var totalPending = parseInt(pendingTask.length) + parseInt(nullTask.length);

            $("#Pendingtask").text(totalPending);

            var workingTask = result.filter(function (obj) {
                return (obj.taskStatus == "Working");
            });
            $("#Workingtask").text(workingTask.length);

            var completeTask = result.filter(function (obj) {
                return (obj.taskStatus == "Completed");
            });
            $("#Completetask").text(completeTask.length);

            $("#Totaltask").text(result.length);
        }, 
    });
};

function AllTaskDetailsList() {
    $('#tasksTableData').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "Post",
            url: '/Task/TaskDetailsDataTable',
            dataType: 'json',
        },
        columns: [
            {
                "data": "userName", "name": "UserName",
            },
            {
                "data": "taskTitle", "name": "TaskTitle"
            },
            { "data": "taskDetails", "name": "TaskDetails" },
            {
                "data": "taskType", "name": "TaskType",
                "render": function (data, type, full) {

                    if (full.taskTypeName == "HighPriority") {
                        return '<a class="badge bg-danger-subtle text-danger text-uppercase">' + full.taskTypeName + '</a>';
                    }
                    else if (full.taskTypeName == "MediumPriority") {
                        return '<a class="badge bg-warning-subtle text-warning text-uppercase">' + full.taskTypeName + '</a>';
                    }
                    else {
                        return '<a class="badge bg-success-subtle text-success text-uppercase">' + full.taskTypeName + '</a>';
                    }
                }
            },
            {
                "data": "taskDate", "name": "TaskDate",
                "render": function (data, type, full) {
                    return (new Date(full.taskDate)).toLocaleDateString('en-US');
                }
            },
            {
                "data": "taskEndDate", "name": "TaskEndDate",
                "render": function (data, type, full) {
                    return (new Date(full.taskEndDate)).toLocaleDateString('en-US');
                }
            },
            {
                "data": "taskStatus", "name": "TaskStatus",
                "render": function (data, type, full) {

                    if (full.taskStatus == "Working") {
                        return ('<a class="badge bg-warning text-uppercase">' + full.taskStatus + '</a>' + '<a onclick="btnTaskDetails(\'' + full.id + '\')"><i class="ri-eye-fill align-bottom me-2 text-muted"></i></a>' + '<a onclick="EditTaskDetails(\'' + full.id + '\')"><i class="ri-pencil-fill align-bottom me-2 text-muted"></i></a>');
                    }
                    else if (full.taskStatus == "Completed") {
                        return ('<a class="badge bg-success text-uppercase">' + full.taskStatus + '</a>' + '<a onclick="btnTaskDetails(\'' + full.id + '\')"><i class="ri-eye-fill align-bottom me-2 text-muted"></i></a>' + '<a onclick="EditTaskDetails(\'' + full.id + '\')"><i class="ri-pencil-fill align-bottom me-2 text-muted"></i></a>');
                    }
                    else if (full.taskStatus == "Pending") {
                        return ('<a class="badge bg-primary text-uppercase">' + full.taskStatus + '</a>' + '<a onclick="btnTaskDetails(\'' + full.id + '\')"><i class="ri-eye-fill align-bottom me-2 text-muted"></i></a>' + '<a onclick="EditTaskDetails(\'' + full.id + '\')"><i class="ri-pencil-fill align-bottom me-2 text-muted"></i></a>');
                    }
                    else {
                        return ('<a class="badge bg-secondary text-uppercase">' + full.taskStatus + '</a>' + '<a onclick="btnTaskDetails(\'' + full.id + '\')"><i class="ri-eye-fill align-bottom me-2 text-muted"></i></a>' + '<a onclick="EditTaskDetails(\'' + full.id + '\')"><i class="ri-pencil-fill align-bottom me-2 text-muted"></i></a>');
                    }
                }
            },
        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all",
        }]
    });
}


function EditTaskDetails(Id) {
    $.ajax({
        url: '/Task/GetTaskDetailsById?Id=' + Id,
        type: "Get",
        contentType: 'application/json;charset=utf-8;',
        dataType: 'json',
        success: function (response) {
            $('#UpdateTaskDetails').modal('show');
            $('#EditId').val(response.id);
            $('#EditUserName').val(response.userName);
            $('#EditTaskTitle').val(response.taskTitle);
            $('#EditDescription').val(response.taskDetails);
            var startdate = response.taskDate;
            var StartDate = startdate.substr(0, 10);
            $('#EditStartDate').val(StartDate);
            var enddate = response.taskEndDate;
            var EndDate = enddate.substr(0, 10);
            $('#EditEndDate').val(EndDate);
            $('#EditTaskType').val(response.taskTypeName);
            $('#EditTaskTypeId').val(response.taskType);
            $('#EditStatus').val(response.taskStatus);
            CheckValidation();
        },
        error: function () {
            alert('Data not found');
        }
    });
}

function UpdateTaskDetails() {
    
    var objData = {
        Id: $("#EditId").val(),
        UserName: $("#EditUserName").val(),
        TaskTitle: $("#EditTaskTitle").val(),
        TaskDetails: $("#EditDescription").val(),
        TaskDate: $("#EditStartDate").val(),
        TaskEndDate: $("#EditEndDate").val(),
        TaskTypeName: $("#EditTaskType").val(),
        TaskType: $("#EditTaskTypeId").val(),
        TaskStatus: $("#EditStatus").val(),
    }
    $.ajax({
        url: '/Task/UpdateUserTaskDetails',
        type: 'Post',
        data: objData,
        dataType: 'json',
        success: function (Result) {
            Swal.fire({
                title: Result.message,
                icon: 'success',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK',
            }).then(function () {
                window.location = '/Task/AllTaskDetails';
            })
        },
    })
}

$('#UpdateDetailsForm').on('change', function () {
    CheckValidation();
}); 

//-----------------Validation-----------------//

function CheckValidation() {
    
    var isValid = true;
    taskTitle = $("#EditTaskTitle").val();
    taskDetails = $("#EditDescription").val();
    taskDate = $("#EditStartDate").val();
    taskEndDate = $("#EditEndDate").val();


    //taskTitle
    if (taskTitle == "") {
        $('#ValidateTaskTitle').text('Please Enter Task Tittle');
        $('#EditTaskTitle').css('border-color', 'red');
        $('#EditTaskTitle').focus();
        isValid = false;
    }
    else {
        $('#ValidateTaskTitle').text('');
        $('#EditTaskTitle').css('border-color', 'lightgray');

    }

    //taskDetails
    if (taskDetails == "") {
        $('#ValidateDescription').text('Please Enter Task Details');
        $('#EditDescription').css('border-color', 'red');
        $('#EditDescription').focus();
        isValid = false;
    }

    else {
        $('#ValidateDescription').text('');
        $('#EditDescription').css('border-color', 'lightgray');

    }

    //taskDate
    if (taskDate == "") {
        $('#ValidateStartDate').text('Please Enter Task Start Date');
        $('#EditStartDate').css('border-color', 'red');
        $('#EditStartDate').focus();
        isValid = false;
    }

    else {
        $('#ValidateStartDate').text('');
        $('#EditStartDate').css('border-color', 'lightgray');
    }

    //taskEndDate
    if (taskEndDate == "") {
        $('#ValidateEndDate').text('Please Enter Task End Date');
        $('#EditEndDate').css('border-color', 'red');
        $('#EditEndDate').focus();
        isValid = false;
    }

    else {
        $('#ValidateEndDate').text('');
        $('#EditEndDate').css('border-color', 'lightgray');
    }
}