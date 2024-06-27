
$(document).ready(function () {
    GetAllUserProjectDetailsList();
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
        formData.append("ShortName", $("#projectname").val());
        formData.append("ProjectHead", $("#projectHead").val());
        formData.append("ProjectStartDate", $("#projectStartDate").val());
        formData.append("ProjectEndDate", $("#projectEndDate").val());
        formData.append("ProjectImage", $("#projectImage")[0].files[0]);
        formData.append("Area", $("#txtProjectArea").val());
        formData.append("BuildingName", $("#txtBuildingName").val());
        formData.append("State", $("#ProjectState").val());
        formData.append("City", $("#ProjectCity").val());
        formData.append("Country", $("#projectCountry").val());
        formData.append("Pincode", $("#txtProjectPincode").val());
        formData.append("ProjectPath", $("#projectPath").val());
        $.ajax({
            url: '/Project/CreateProject',
            type: 'Post',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (Result) {
                if (Result.code == 200) {
                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/Project/CreateProject';
                    });
                }
                else {
                    toastr.error(Result.message);
                }               
            }
        })
    }
    else {
        toastr.warning("Kindly fill all datafield");
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
            txtBuildingName: "required",
            txtProjectArea: "required",
            txtProjectPincode: {
                required: true,
                digits: true,
                minlength: 6,
                maxlength: 6
            },
            projectCountry: "required",
            ProjectState: "required",
            ProjectCity: "required",
        },
        messages: {
            projectPriority: "Please Select Project Priority",
            projectTitle: "Please Enter Project Title",
            projectDescription: "Please Enter Project Description",
            projectStatus: "Please Enter Project Status",
            projectdeadline: "Please Enter Deadline Date",
            txtBuildingName: "Please Enter Building Name",
            txtProjectArea: "Please Enter Project Area",
            txtProjectPincode: {
                required: "Please Enter Pin Code",
                digits: "Pin code must contain only digits",
                minlength: "Pin code must be 6 digits long",
                maxlength: "Pin code must be 6 digits long"
            },
            projectCountry: "Please Enter Project Country",
            ProjectState: "Please Enter Project State",
            ProjectCity: "Please Enter Project City",
        }
    });
    $("#frmprojectdetails").validate({
        rules: {
            projectType: "required",
            projectHead: "required",
            projectLocation: "required",
            projectStartDate: "required",
        },
        messages: {
            projectType: "Please Enter Project Type",
            projectHead: "Please Select Project Head",
            projectLocation: "Please Enter Location",
            projectStartDate: "Please Enter Start Date",
        }
    });
    $('#projectDetails').on('click', function () {
        $('#frmprojectdetails').valid();
    });
});

function openmemberpop() {
    showMember()
    $("#inviteMembersModal").modal('show');
}

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
    })
}
function ProjectHeadMemberList() {
    $.ajax({
        url: '/Project/ProjectHeadMemberList',
        type: 'Post',
        dataType: 'json',
        processData: false,
        contentType: false,
        complete: function (Result) {
            $('#dvinvitemember').html(Result.responseText);
        },
    })
}
function SelectProjectHead(button) {

    var memberName = $(button).closest('.mx-n4').find('.fs-13').text().trim();
    $('#projectHead').val(memberName);
    document.getElementById("closebtn").click()
}


$('#SearchBtn').keyup(function () {
    var typeValue = $(this).val().toLowerCase();
    $('.vstack1').each(function () {
        if ($(this).text().toLowerCase().indexOf(typeValue) < 0) {
            $(this).parent().fadeOut();
        }
        else {
            $(this).parent().fadeIn();
        }
    });
});

function invitemember(Id) {

    var protitel = document.getElementById("projecttitle").textContent;
    var proStartDate = $('#projectenddate').val();
    var proStatus = document.getElementById('projectstatus').textContent;
    var proProjectType = document.getElementById('projecttype').textContent;
    var proProjectId = $('#projectid').val();
    var UpdatedBy = $('#memberUpdatedby').val();

    var MemberData = {
        ProjectId: proProjectId,
        UserId: Id,
        ProjectType: proProjectType,
        ProjectTitle: protitel,
        StartDate: proStartDate,
        Status: proStatus,
        UpdatedBy: UpdatedBy,

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
            if (Result.code == 200) {
                Swal.fire({
                    title: Result.message,
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                }).then(function () {
                    window.location = '/Project/ProjectDetails/?Id=' + proProjectId;
                });
            }
            else {
                $("#inviteMembersModal").modal('hide');
                toastr.warning(Result.message);
            }
        },
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
    })
}

function showTeamsPagination(ProjectId) {
    showTeams(ProjectId, 1);
}

function showTeams(ProjectId, page) {
    var formData = new FormData();
    formData.append("ProjectId", ProjectId);
    formData.append("page", page);
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
            if (Result.code == 200) {
                Swal.fire({
                    title: Result.message,
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                }).then(function () {
                    window.location = '/Project/ProjectDetails/?Id=' + data6;
                })
            }
            else {
                toastr.error(Result.message);
            }
        },
        error: function () {
            toastr.error("Can't get Data");
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
    })
}

function GetAllUserProjectDetailsList(page) {

    var searchBy = $("#inputSearch").val();
    var searchFor = $("#inputsearch").val();

    $.get("/Project/GetAllUserProjectList", { searchby: searchBy, searchfor: searchFor, page: page })
        .done(function (result) {

            $("#getallprojectlist").html(result);
        })
        .fail(function (error) {
            toastr.error(error);
        });
}

GetAllUserProjectDetailsList(1);

$(document).on("click", ".pagination a", function (e) {
    e.preventDefault();
    var page = $(this).text();
    GetAllUserProjectDetailsList(page);
    var ProjectId = $("#projectid").val();
    showTeams(ProjectId, page);
});


$(document).on("click", "#backButton", function (e) {
    e.preventDefault();
    var page = $(this).text();
    GetAllUserProjectDetailsList(page);
    var ProjectId = $("#projectid").val();
    showTeams(ProjectId, page);
});


function searchproject() {

    GetAllUserProjectDetailsList(1);
}

$(document).ready(function () {
    $(document).on('click', '.btndeletedoc', function () {
        var userId = $(this).data("user-id");
        opendeletpop(userId);
    });

    $("#delete-product").click(function () {
        var userId = $(this).data("user-id");
        deleteProjectMember(userId);
    });
});


function opendeletpop(userId) {
    $("#delete-product").data("user-id", userId);
    $("#deleteOrderModal").modal('show');
}

function docmodalopen() {
    $("#documentUploadModal").modal('show');
}
function deleteProjectMember(userId) {
    var proId = $('#projectid').val();
    var updatedby = $('#txtUpdatedBy').val();
    var MemberData = {
        ProjectId: proId,
        UserId: userId,
        UpdatedBy: updatedby,
    }
    var form_data = new FormData();
    form_data.append("InviteMember", JSON.stringify(MemberData));
    $.ajax({
        url: '/Project/IsDeletedMember',
        type: 'POST',
        dataType: 'json',
        data: form_data,
        processData: false,
        contentType: false,
        success: function (Result) {
            if (Result.code == 200) {
                Swal.fire({
                    title: Result.message,
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK'
                }).then(function () {
                    window.location = '/Project/ProjectDetails/?Id=' + proId;
                })
            }
            else {
                toastr.error(Result.message);
            }
           
        },
        error: function () {
            toastr.error("Can't remove member");
        }
    })
}

$(document).ready(function () {
    var ProId = $('#txtprojectid').val();
    projectActivity(ProId);
})

function projectActivity(ProId) {

    $.ajax({
        url: '/Project/GetInvoiceActivity?ProId=' + ProId,
        type: 'Get',
        dataType: 'json',
        processData: false,
        contentType: false,
        complete: function (Result) {

            $('#projectActivity').html(Result.responseText);
            $('#projectActivityinoverview').html(Result.responseText);
            projectinvoiceActivity(ProId)
        },
    })
}

function projectinvoiceActivity(ProId) {

    $.ajax({
        url: '/Task/GetProjectActivity?ProId=' + ProId,
        type: 'Get',
        dataType: 'json',
        processData: false,
        contentType: false,
        complete: function (Result) {

            $('#invoiceactivity').html(Result.responseText);
        },
    })
}

function deleteProjectDocument(DocumentId) {
    var proId = $('#txtProjectId').val();
    Swal.fire({
        title: "Are you sure want to delete this?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, delete it!",
        cancelButtonText: "No, cancel!",
        confirmButtonClass: "btn btn-primary w-xs me-2 mt-2",
        cancelButtonClass: "btn btn-danger w-xs mt-2",
        buttonsStyling: false,
        showCloseButton: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Project/DeleteProjectDocument?DocumentId=' + DocumentId,
                type: 'POST',
                dataType: 'json',
                success: function (Result) {
                    if (Result.code == 200) {
                        Swal.fire({
                            title: Result.message,
                            icon: 'success',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'OK'
                        }).then(function () {
                            window.location = '/Project/ProjectDetails/?Id=' + proId;
                        })
                    } else {
                        Swal.fire({
                            title: Result.message,
                            icon: 'success',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'OK'
                        }).then(function () {
                            window.location = '/Project/ProjectDetails/?Id=' + proId;
                        })
                    }
                },
            })
        } else if (result.dismiss === Swal.DismissReason.cancel) {

            Swal.fire(
                'Cancelled',
                'Project document have no changes.!!😊',
                'error'
            );
        }
    });
}

function fn_UpdateProjectDetail()
{
    if ($('#formprojectdetails').valid()) {
        var formData = new FormData();
        formData.append("ProjectTitle", $("#projectTitle").val());
        formData.append("ProjectId", $("#textprojectId").val());
        formData.append("ProjectPriority", $("#projectPriority").val());
        formData.append("ProjectDescription", $("#projectDescription").val());
        formData.append("ProjectStatus", $("#projectStatus").val());
        formData.append("ProjectDeadline", $("#projectdeadline").val());
        formData.append("ProjectType", $("#projectType").val());
        formData.append("ShortName", $("#projectname").val());
        formData.append("ProjectHead", $("#projectHead").val());
        formData.append("ProjectStartDate", $("#projectStartDate").val());
        formData.append("ProjectEndDate", $("#projectEndDate").val());
        formData.append("Area", $("#txtProjectArea").val());
        formData.append("BuildingName", $("#txtBuildingName").val());
        formData.append("State", $("#ProjectState").val());
        formData.append("City", $("#ProjectCity").val());
        formData.append("Country", $("#projectCountry").val());
        formData.append("Pincode", $("#txtProjectPincode").val());
        formData.append("ProjectPath", $("#projectPath").val());
        formData.append("UpdatedBy", $("#textProjectUserId").val());
        var imageName = $("#currentImageName").text().trim();
        var imageFile = $("#projectImage")[0].files[0];
        if (imageName && !imageFile) {
            formData.append("ProjectImageName", imageName);
        } else if (imageFile) {
            formData.append("ProjectImage", imageFile);
        }

        $.ajax({
            url: '/Project/UpdateProjectDetails',
            type: 'Post',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (Result) {
                if (Result.code == 200) {
                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/Project/ProjectList';
                    });
                }
                else {
                    toastr.error(Result.message);
                }
            }
        })
    }
    else {
        toastr.warning("Kindly fill all datafield");
    }
}
$(document).ready(function () {
    function toggleImagePreview(show) {
        if (show) {
            $('#imagePreviewContainer').show();
        } else {
            $('#imagePreviewContainer').hide();
        }
    }

    $('#deleteImageButton').click(function () {
        $('#projectImagePreview').attr('src', '');
        $('#projectImage').val('');
        $("#currentImageName").text('');
        toggleImagePreview(false);
    });
    $('#projectImage').change(function () {
        var input = this;
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#projectImagePreview').attr('src', e.target.result);
                toggleImagePreview(true);
            }
            reader.readAsDataURL(input.files[0]);
        } else {
            toggleImagePreview(false);
        }
    });

    if ($('#projectImagePreview').attr('src') === '' || $('#projectImagePreview').attr('src') === '#') {
        toggleImagePreview(false);
    } else {
        toggleImagePreview(true);
    }
});