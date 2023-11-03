$(document).ready(function () {

    //GetUserTaskDetails();  
});


function showadddetails() {
    debugger
    ClearTextBox();
    GetTaskType();
    GetUsername();
    $('#adddeals').modal('show');
}


function ClearTextBox() {

    $("#dealType option").remove();
    $("#dealType")[0];
    $("#dealTitle").val('');
    $("#ddlusername").val("");
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

    var formData = new FormData();
    formData.append("TaskType", $("#dealType").val());
    formData.append("TaskTitle", $("#dealTitle").val());
    formData.append("UserId", $("#ddlusername").val());
    formData.append("TaskDate", $("#txtdatetime").val());
    formData.append("TaskEndDate", $("#txtenddatetime").val());
    formData.append("TaskDetails", $("#contactDescription").val());

    $.ajax({
        url: '/Home/AddTaskDetails',
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
                    window.location = '/Home/UserHome';
                });
            } else {
                Swal.fire({
                    title: "Kindly Fill All Datafield",
                    icon: 'error',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                })
            }
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
    $("#frmtaskdetails").validate({
        rules: {
            dealType: "required",
            dealTitle: "required",
            ddlusername: "required",
            txtdatetime: "required",
            txtenddatetime: "required",
            contactDescription: "required",
        },
        messages: {
            dealType: "Please Enter Deal Type",
            dealTitle: "Please Enter Deal Title",
            ddlusername: "Please Select UserName",
            txtdatetime: "Please Enter Start Date",
            txtenddatetime: "Please Enter End Date",
            contactDescription: "Please Enter Description",
        }
    })
    $('#taskDetails').on('click', function () {
        $("#frmtaskdetails").valid();
    });
});


function GetUserTaskDetails() {
    $.ajax({
        url: '/Home/GetUserTaskDetail',
        type: 'Post',
        dataType: 'json',
        processData: false,
        contentType: false,
        complete: function (Result) {
            $('#dvtskdetail').html(Result.responseText);
            ClearTaskDetails();
        }
    })
}

function btnStatusUpdate(Id)
{
    var StausChange = {
        TaskStatus: $('#ddlStatus' + Id).val(),
        Id : Id
    }
    var form_data = new FormData();
    form_data.append("STATUSUPDATE", JSON.stringify(StausChange));

    $.ajax({
        url: '/Home/UpdateUserDealStatus',
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
