$(document).ready(function () {
    GetUserTotalTask()
    GetAllUserTaskDetail()
});


function showadddetails() {
    ClearTextBox();
    GetTaskType();
    GetUsernameList();
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
    $("#txtdocument").val('');
    $("#contactDescription").val('');
}

function ClearTaskDetails() {
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
    siteloadershow();

    if ($('#frmtaskdetails').valid()) {
        var formData = new FormData();
        formData.append("TaskType", $("#taskType").val());
        formData.append("TaskTitle", $("#dealTitle").val());
        formData.append("UserId", $("#ddlusername").val());
        formData.append("TaskDate", $("#txtdatetime").val());
        formData.append("TaskEndDate", $("#txtenddatetime").val());
        formData.append("TaskDetails", $("#contactDescription").val());
        formData.append("ProjectId", $("#txtprojectid").val());
        var fileInput = document.getElementById("txtdocument");
        if (fileInput.files.length > 0) {
            formData.append("Image", fileInput.files[0]);
        }
        $.ajax({
            url: '/Task/AddTaskDetails',
            type: 'POST',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (Result) {
                siteloaderhide();
                if (Result.code === 200) {
                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/Task/AllTaskDetails';
                    });
                } else {
                    toastr.error(Result.message);
                }
            },
            error: function (xhr, status, error) {
                siteloaderhide();
                toastr.error('An error occurred while processing your request.');
            }
        });
    } else {
        siteloaderhide();
        toastr.warning("Kindly fill all data fields");
    }
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
/*---------InReview------------*/
function btnStatusUpdate(Id) {

    if ($("#tasksListform").valid()) {
        var ReviewStatus = $('#ddlStatusReview' + Id).val();
        if (ReviewStatus != null) {
            var StausChange = {
                TaskStatus: $('#ddlStatusReview' + Id).val(),
                Id: Id,
                UpdatedBy: $("#textTaskUserId").val(),
                UserId: $("#textTaskUserId").val(),
                ProjectHead: $("#textTaskUserName").val(),
                ProjectId: $("#textTaskProjectId").val(),
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
                    if (Result.code == 200) {
                        Swal.fire({
                            title: Result.message,
                            icon: 'success',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'OK',
                        }).then(function () {
                            window.location = '/Task/UserTasks';
                        });
                    }
                    else {
                        toastr.error(Result.message);
                    }
                },
            });
        }
        else {
            siteloaderhide();
            $('#ddlStatusReview' + Id + '-error').text('Kindly select the status').show();
            toastr.warning("Kindly select the status");
        }
    }

    else {
        siteloaderhide();
        toastr.warning("Kindly select the status");
    }
}
/*----ValidateMeassge----*/
$(document).ready(function () {


    $("#tasksListform").validate({
        rules: {
            ddlStatusReview: "required"
        },
        messages: {
            ddlStatusReview: "Please enter status"
        }
    })
    $('#StatusUpdate').on('click', function () {

        $("#tasksListform").validate();
    });
});
/*-------LOWPRIORITY---------*/
function btnStatusUpdateLow(Id) {
    if ($("#tasksListLow").valid()) {

        var StausChange = {
            TaskStatus: $('#ddlStatus' + Id).val(),
            Id: Id,
            UpdatedBy: $("#textTaskUserId").val(),
            UserId: $("#textTaskUserId").val(),
            ProjectHead: $("#textTaskUserName").val(),
            ProjectId: $("#textTaskProjectId").val(),
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
                siteloaderhide();
                GetUserTaskDetails();
                if (Result.code == 200) {

                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/Task/UserTasks';
                    });
                }
                else {
                    toastr.error(Result.message);
                }
            },
            error: function (error, status) {
                toastr.error(error);

            }
        });
    }

    else {
        siteloaderhide();
        toastr.warning("Kindly select the status");
    }
}
/*----ValidateMeassge LowPrority----*/
$(document).ready(function () {


    $("#tasksListLow").validate({
        rules: {
            ddlStatus: "required"
        },
        messages: {
            ddlStatus: "Please enter status"
        }
    })
    $('#StatusUpdate').on('click', function () {

        $("#tasksListLow").validate();
    });
});

/*/-------MEDIUMPRIORITY------------*/
function btnStatusUpdateMedium(Id) {

    if ($("#tasksListMedium").valid()) {
        var MediumStatus = $('#ddlStatusMedium' + Id).val();
        if (MediumStatus != null) {

            var StausChange = {
                TaskStatus: $('#ddlStatusMedium' + Id).val(),
                Id: Id,
                UpdatedBy: $("#textTaskUserId").val(),
                UserId: $("#textTaskUserId").val(),
                ProjectHead: $("#textTaskUserName").val(),
                ProjectId: $("#textTaskProjectId").val(),
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
                    siteloaderhide();
                    GetUserTaskDetails();
                    if (Result.code == 200) {

                        Swal.fire({
                            title: Result.message,
                            icon: 'success',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'OK',
                        }).then(function () {
                            window.location = '/Task/UserTasks';
                        });
                    }
                    else {
                        toastr.error(Result.message);
                    }
                },
                error: function (error, status) {
                    toastr.error(error);

                }
            });
        } else {
            siteloaderhide();
            $('#ddlStatusMedium' + Id + '-error').text('Kindly select the status').show();
            toastr.warning("Kindly select the status");
        }
    }

    else {
        siteloaderhide();
        toastr.warning("Kindly select the status");
    }
}
/*----ValidateMeassge MediumPrority----*/
$(document).ready(function () {


    $("#tasksListMedium").validate({
        rules: {
            ddlStatusMedium: "required"
        },
        messages: {
            ddlStatusMedium: "Please enter status"
        }
    })
    $('#StatusUpdate').on('click', function () {

        $("#tasksListMedium").validate();
    });
});
/*---------HIGHPRIORITY-----------*/
function btnStatusUpdateHigh(Id) {
    if ($("#tasksListhigh").valid()) {
        var data = $('#ddlStatusHigh' + Id).val();
        if (data != null) {
            var StausChange = {
                TaskStatus: $('#ddlStatusHigh' + Id).val(),
                Id: Id,
                UpdatedBy: $("#textTaskUserId").val(),
                UserId: $("#textTaskUserId").val(),
                ProjectHead: $("#textTaskUserName").val(),
                ProjectId: $("#textTaskProjectId").val(),
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
                    siteloaderhide();
                    GetUserTaskDetails();
                    if (Result.code == 200) {

                        Swal.fire({
                            title: Result.message,
                            icon: 'success',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'OK',
                        }).then(function () {
                            window.location = '/Task/UserTasks';
                        });
                    }
                    else {
                        toastr.error(Result.message);
                    }
                },
                error: function (error, status) {
                    toastr.error(error);
                }
            });
        } else {
            siteloaderhide();
            $('#ddlStatusHigh' + Id + '-error').text('Kindly select the status').show();
            toastr.warning("Kindly select the status");
        }
    }
    else {
        siteloaderhide();
        toastr.warning("Kindly select the status");
    }
}
/*----ValidateMeassge HighPrority----*/
$(document).ready(function () {
    $("#tasksListhigh").validate({
        rules: {
            ddlStatusHigh: "required"
        },
        messages: {
            ddlStatusHigh: "Please enter status"
        }
    })
    $('#StatusUpdate').on('click', function () {

        $("#tasksListhigh").validate();
    });
});

function btnTaskDetails(Id) {
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
            $('#taskstartdate-field').text(moment(response.taskDate).format('DD MMM YY HH:mm'));
            //var enddate = response.taskEndDate;
            //var EndDate = enddate.substr(0, 10);
            $('#taskenddate-field').text(moment(response.taskEndDate).format('DD MMM YY HH:mm'));
            $('#taskpriority-field').text(response.taskTypeName);
            $('#taskstatus-field').text(response.taskStatus);
        },
        error: function () {
            siteloaderhide();
            toastr.error("Can't get Data");
        }

    });
}


function GetAllUserTaskDetail() {
    $.ajax({
        url: '/Task/GetAllUserTaskDetail',
        type: 'Get',
        dataType: 'json',
        contentType: 'application/json;charset=utf-8;',
        success: function (result) {
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

function GetUserTotalTask() {
    $.ajax({
        url: '/Home/GetUserTotalTask',
        type: 'Get',
        dataType: 'json',
        contentType: 'application/json;charset=utf-8;',
        success: function (result) {
            $("#UserTotalTask").text(result.length);
        },
    });
};

var datas = userPermissions
$(document).ready(function () {
    function data(datas) {
        var userPermission = datas;
        AllTaskDetailsList(userPermission);
    }

    function AllTaskDetailsList(userPermission) {
        $('#tasksTableData').DataTable({
            processing: false,
            serverSide: true,
            filter: true,
            "bDestroy": true,
            ajax: {
                type: "POST",
                url: '/Task/GetAllTaskList',
                dataType: 'json',
            },
            columns: [
                {
                    "data": "userName", "name": "UserName",
                },
                {
                    "data": "taskTitle", "name": "TaskTitle"
                },
                {
                    "data": "taskDetails", "name": "TaskDetails",
                    "render": function (data, type, full) {
                        if (type === 'display') {
                            let taskDetails = data;
                            if (data.length > 50) {
                                taskDetails = '<span title="' + data + '">' + data.substr(0, 50) + '...</span>';
                            }
                            if (full.document) {
                                let documentLink = '<div class="d-flex align-items-center">' +
                                    '<div class="flex-grow-1">' + taskDetails + '</div>' +
                                    '<div class="flex-shrink-0 ms-4 task-icons">' +
                                    '<li class="list-inline tasks-list-menu mb-0" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="Document">' +
                                    '<a class="flex-shrink-0 ms-4 task-icons" onclick="DownloadTaskDocument(\'' + full.document + '\')">' +
                                    '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">' +
                                    '<path fill="currentColor" d="M13 12h3l-4 4l-4-4h3V8h2v4Zm2-8H5v16h14V8h-4V4ZM3 2.992C3 2.444 3.447 2 3.999 2H16l5 5v13.993A1 1 0 0 1 20.007 22H3.993A1 1 0 0 1 3 21.008V2.992Z" />' +
                                    '</svg></a></li>' +
                                    '</div>' +
                                    '</div>';
                                return documentLink;
                            }

                            return '<div class="d-flex align-items-center">' +
                                '<div class="flex-grow-1">' + taskDetails + '</div>' +
                                '</div>';
                        }
                        return data;
                    }
                },
                {
                    "data": "taskType", "name": "TaskType",
                    "render": function (data, type, full) {
                        if (full.taskTypeName == "HighPriority") {
                            return '<a class="badge bg-danger-subtle text-danger text-uppercase">' + full.taskTypeName + '</a>';
                        } else if (full.taskTypeName == "MediumPriority") {
                            return '<a class="badge bg-warning-subtle text-warning text-uppercase">' + full.taskTypeName + '</a>';
                        } else {
                            return '<a class="badge bg-success-subtle text-success text-uppercase">' + full.taskTypeName + '</a>';
                        }
                    }
                },
                {
                    "data": "taskDate", "name": "TaskDate",
                    "render": function (data, type, full, meta) {
                        return getCommonDateformat(data);
                    }
                },
                {
                    "data": "taskEndDate", "name": "TaskEndDate",
                    "render": function (data, type, full, meta) {
                        return getCommonDateformat(data);
                    }
                },
                {
                    "data": "taskStatus",
                    "name": "TaskStatus",
                    "render": function (data, type, full) {
                        var badgeClass = 'bg-info';
                        if (full.taskStatus === "Working") {
                            badgeClass = 'bg-warning';
                        } else if (full.taskStatus === "Completed") {
                            badgeClass = 'bg-success';
                        } else if (full.taskStatus === "Pending") {
                            badgeClass = 'bg-secondary';
                        } else if (full.taskStatus === "InReview") {
                            badgeClass = 'bg-orange';
                        } else if (full.taskStatus === "InReview") {
                            badgeClass = 'bg-danger';
                        }

                        return '<a class="badge ' + badgeClass + ' text-uppercase">' + full.taskStatus + '</a>';
                    }
                },
                {
                    "data": "action",
                    "name": "Action",
                    "render": function (data, type, full, meta) {
                        var userPermissionArray = JSON.parse(userPermission);

                        var canEdit = false;
                        var canDelete = false;

                        for (var i = 0; i < userPermissionArray.length; i++) {
                            var permission = userPermissionArray[i];
                            if (permission.formName === "Tasks List") {
                                canEdit = permission.edit;
                                canDelete = permission.delete;
                                break;
                            }
                        }

                        var buttons = '<ul class="list-inline hstack gap-2 mb-0">';

                        buttons += '<li class="list-inline-item" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="View">' +
                            '<a class="text-primary" onclick="btnTaskDetails(\'' + full.id + '\')"><i class="ri-eye-fill fs-16"></i></a></li>';

                        if (canEdit) {
                            buttons += '<li class="btn list-inline-item edit" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="Edit">' +
                                '<a class="text-primary" onclick="EditTaskDetails(\'' + full.id + '\')">' +
                                '<i class="fa-regular fa-pen-to-square"></i></a></li>';
                        }

                        if (canDelete) {
                            buttons += '<li class="list-inline-item" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="Delete">' +
                                '<a class="btn text-danger btndeletedoc" onclick="DeleteTask(\'' + full.id + '\')"><i class="fas fa-trash"></i></a></li>';
                        }

                        buttons += '</ul>';

                        return buttons;
                    }
                }
            ],
            columnDefs: [{
                "defaultContent": "",
                "targets": "_all",
            }]
        });
    }

    data(datas);
});

function DownloadTaskDocument(taskDocument) {
    $.ajax({
        url: '/Task/DownloadTaskDocument?TaskDocument=' + taskDocument,
        type: "get",
        contentType: 'application/json;charset=utf-8',
        dataType: 'json',
        success: function (result) {
            siteloaderhide();

            if (result.fileName && result.memory) {

                var byteCharacters = atob(result.memory);
                var byteNumbers = new Array(byteCharacters.length);
                for (var i = 0; i < byteCharacters.length; i++) {
                    byteNumbers[i] = byteCharacters.charCodeAt(i);
                }
                var byteArray = new Uint8Array(byteNumbers);

                var blob = new Blob([byteArray], { type: result.contentType });

                var link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                link.setAttribute('download', result.fileName);

                document.body.appendChild(link);

                link.click();

                document.body.removeChild(link);
            } else {
                toastr.warning(result.Message || "No document found for selected task");
            }
        },
        error: function () {
            siteloaderhide();
            toastr.error("Can't get Data");
        }
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
            var startDateTime = response.taskDate;
            $('#EditStartDate').val(formatDateTime(startDateTime));
            var endDateTime = response.taskEndDate;
            $('#EditEndDate').val(formatDateTime(endDateTime));
            $('#EditTaskType').val(response.taskTypeName);
            $('#EditTaskTypeId').val(response.taskType);
            $('#EditStatus').val(response.taskStatus);
            CheckValidation();
        },
        error: function () {
            toastr.error("Can't get Data");
        }
    });
}

function UpdateTaskDetails() {
    var objData = {
        UpdatedBy: $("#textUpdatedById").val(),
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
        url: '/Task/UpdateTaskDetails',
        type: 'Post',
        data: objData,
        dataType: 'json',
        success: function (Result) {
            if (Result.code == 200) {
                Swal.fire({
                    title: Result.message,
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                }).then(function () {
                    window.location = '/Task/AllTaskDetails'; 
                })
            } else {
                toastr.error(Result.message);
            }
        },
    });
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
function DeleteTask(Id) {
    Swal.fire({
        title: "Are you sure want to Delete This?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, Delete it!",
        cancelButtonText: "No, cancel!",
        confirmButtonClass: "btn btn-primary w-xs me-2 mt-2",
        cancelButtonClass: "btn btn-danger w-xs mt-2",
        buttonsStyling: false,
        showCloseButton: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Task/DeleteTask?Id=' + Id,
                type: 'POST',
                dataType: 'json',
                success: function (Result) {
                    if (Result.code) {
                        Swal.fire({
                            title: Result.message,
                            icon: 'success',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'OK'
                        }).then(function () {
                            window.location = '/Task/AllTaskDetails';
                        })
                    }
                    else {
                        toastr.error(Result.message);
                    }
                },
                error: function () {
                    Swal.fire({
                        title: "Can't delete task!",
                        icon: 'warning',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/Task/AllTaskDetails';
                    })
                }
            })
        } else if (result.dismiss === Swal.DismissReason.cancel) {

            Swal.fire(
                'Cancelled',
                'Tasks have no changes.!!😊',
                'error'
            );
        }
    });
}