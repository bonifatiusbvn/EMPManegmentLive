
$(document).ready(function () {
      document.getElementById("showProjectMembers").click()
      document.getElementById("showProjectDocuments").click()
});


function btnCreateProjectDetail() {

    if ($('#formprojectdetails').valid()) {

        var formData = new FormData();
        formData.append("ProjectTitle", $("#projectTitle").val());
        formData.append("ProjectPriority", $("#projectPriority").val());
        formData.append("ProjectDescription", $("#projectDescription").val());
        formData.append("ProjectStatus", $("#projectStatus").val());
        formData.append("ProjectDeadline", $("#projectdeadline").val());
        formData.append("ProjectType", $("#projectType").val());
        formData.append("ProjectHead", $("#projectHead").val());
        formData.append("ProjectLocation", $("#projectLocation").val());
        formData.append("ProjectStartDate", $("#projectStartDate").val());
        formData.append("ProjectEndDate", $("#projectEndDate").val());
        $.ajax({
            url: '/Project/CreateProject',
            type: 'Post',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (Result) {

                Swal.fire({
                    title: Result.message,
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                }).then(function () {
                    window.location = '/Project/CreateProject';
                });     
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

$(document).ready(function () {

    $("#formprojectdetails").validate({
        rules: {
            projectPriority: "required",
            projectTitle: "required",
            projectDescription: "required",
            projectStatus: "required",
            projectdeadline: "required",
        },
        messages: {
            projectPriority: "Please Select Project Priority",
            projectTitle: "Please Enter Project Title",
            projectDescription: "Please Enter Project Description",
            projectStatus: "Please Enter Project Status",
            projectdeadline: "Please Enter Deadline Date",
        }
    });
    $("#frmprojectdetails").validate({
        rules: {
            projectType: "required",
            projectHead: "required",
            projectLocation: "required",
            projectStartDate: "required",
            projectEndDate: "required",
        },
        messages: {
            projectType: "Please Enter Project Type",
            projectHead: "Please Select Project Head",
            projectLocation: "Please Enter Location",
            projectStartDate: "Please Enter Start Date",
            projectEndDate: "Please Enter End Date",
        }
    });
    $('#projectDetails').on('click', function () {
        $('#frmprojectdetails').valid();
    });
});

function showMember() {
    $.ajax({
        url: '/Project/GetMemberList',
        type: 'Post',
        dataType: 'json',
        processData: false,
        contentType: false,
        complete: function (Result) {
            $('#dvinvitemember').html(Result.responseText);
        },
        Error: function () {
            Swal.fire({
                title: "Can't get data!",
                icon: 'warning',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK',
            })
        }
    })
}
function invitemember(Id)
{
    const data1 = document.getElementById('projecttitle');
    var result1 = data1.outerText;
    const data2 = document.getElementById('projectstartdate');
    var result2 = data2.outerText;
    const data4 = document.getElementById('projectstatus');
    var result4 = data4.outerText;
    const data5 = document.getElementById('projecttype');
    var result5 = data5.outerText;
    const data6 = document.getElementById('projectid').value;

    var MemberData = {
        ProjectType: result5,
        Status: result4,
        StartDate: result2,
        ProjectTitle: result1,
        ProjectId: data6,
        UserId: Id
    }
    var form_data = new FormData();
    form_data.append("InviteMember", JSON.stringify(MemberData));

        $.ajax({
        url: '/Project/InviteMemberToProject',
        type: 'Post',
        data: form_data,
        dataType: 'json',
        processData: false,
        contentType: false,
            success: function (Result) {
                Swal.fire({
                    title: Result.message,
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                }).then(function () {
                    window.location = '/Project/AddProjectMember/?Id=' + data6;
                }); 
            },
        Error: function () {
            Swal.fire({
                title: "Can't get data!",
                icon: 'warning',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK',
            })
        }
    })
}

function showProjectMembers(ProjectId) {
    var formData = new FormData();
    formData.append("ProjectId", ProjectId);
    $.ajax({
        url: '/Project/ShowProjectMembers',
        type: 'Post',
        dataType: 'json',
        data: formData,
        processData: false,
        contentType: false,
        complete: function (Result) {
            $('#dvshowmembers').html(Result.responseText);
        },
        Error: function () {
            Swal.fire({
                title: "Can't get data!",
                icon: 'warning',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK',
            })
        }
    })
}

function showTeams(ProjectId) {
    var formData = new FormData();
    formData.append("ProjectId", ProjectId);
    $.ajax({
        url: '/Project/ShowTeam',
        type: 'Post',
        dataType: 'json',
        data: formData,
        processData: false,
        contentType: false,
        complete: function (Result) {
            $('#dvshowteam').html(Result.responseText);
        },
        Error: function () {
            Swal.fire({
                title: "Can't get data!",
                icon: 'warning',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK',
            })
        }
    })
}


function addProjectDocument() {

    var formData = new FormData();
    const data6 = document.getElementById('projectid').value;
    formData.append("ProjectId", data6);
    formData.append("DocumentName", $("#txtDocumentName")[0].files[0]);

    $.ajax({
        url: '/Project/AddDocumentToProject',
        type: 'Post',
        dataType: 'json',
        data: formData,
        processData: false,
        contentType: false,
        success: function (Result) {
            Swal.fire({
                title: Result.message,
                icon: 'success',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK',
            }).then(function () {
                window.location = '/Project/AddProjectMember/?Id=' + data6;
            })
        },
        Error: function () {
            Swal.fire({
                title: "Can't get data!",
                icon: 'warning',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK',
            })
        }
    })
}

function showProjectDocuments(ProjectId) {
    var formData = new FormData();
    formData.append("ProjectId", ProjectId);
    $.ajax({
        url: '/Project/ShowProjectDocuments',
        type: 'Post',
        dataType: 'json',
        data: formData,
        processData: false,
        contentType: false,
        complete: function (Result) {
            $('#dvshowdocuments').html(Result.responseText);
        },
        Error: function () {
            Swal.fire({
                title: "Can't get data!",
                icon: 'warning',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK',
            })
        }
    })
}

function showuploadDocuments(ProjectId) {
    var formData = new FormData();
    formData.append("ProjectId", ProjectId);
    $.ajax({
        url: '/Project/ShowUploadedDocuments',
        type: 'Post',
        dataType: 'json',
        data: formData,
        processData: false,
        contentType: false,
        complete: function (Result) {
            $('#dvuploadDocuments').html(Result.responseText);
        },
        Error: function () {
            Swal.fire({
                title: "Can't get data!",
                icon: 'warning',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK',
            })
        }
    })
}
