
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
            title: "Kindly fill all datafield",
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

    var MemberData = {
        ProjectId: proProjectId,
        UserId: Id,
        ProjectType: proProjectType,
        ProjectTitle: protitel,
        StartDate: proStartDate,
        Status: proStatus

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
                    window.location = '/Project/GetProjectDetails/?Id=' + proProjectId;
                });
            }
            else {
                Swal.fire({
                    title: Result.message,
                    icon: 'warning',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                }).then(function () {
                    window.location = '/Project/GetProjectDetails/?Id=' + proProjectId;
                });
            }
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
                window.location = '/Project/GetProjectDetails/?Id=' + data6;
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

function GetAllUserProjectDetailsList(page) {

    var searchBy = $("#inputSearch").val();
    var searchFor = $("#inputsearch").val();

    $.get("/Project/GetAllUserProjectList", { searchby: searchBy, searchfor: searchFor, page: page })
        .done(function (result) {

            $("#getallprojectlist").html(result);
        })
        .fail(function (error) {
            console.error(error);
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
    var MemberData = {
        ProjectId: proId,
        UserId: userId,
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
            Swal.fire({
                title: Result.message,
                icon: 'success',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK'
            }).then(function () {
                window.location = '/Project/ProjectDetails/?Id=' + proId;
            })
        },
        error: function () {
            Swal.fire({
                title: "Can't remove member!",
                icon: 'warning',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK',
            })
        }
    })
}


$(document).ready(function () {

    GetProjectCountry();

    $('#dropProjectState').change(function () {

        var Text = $("#dropProjectState Option:Selected").text();
        var txtProjectid = $(this).val();
        $("#txtProjectstate").val(txtProjectid);
    });

    $('#ProjectCity').change(function () {

        var Text = $("#projectCity Option:Selected").text();
        var txtProjectcity = $(this).val();
        $("#txtProjectCity").val(txtProjectcity);
    });

});

function fn_getProjectState(drpProjectstate, countryId, that) {
    var cid = countryId;
    if (cid == undefined || cid == null) {
        var cid = $(that).val();
    }


    $('#' + drpProjectstate).empty();
    $('#' + drpProjectstate).append('<Option >--Select State--</Option>');
    $.ajax({
        url: '/Authentication/GetState?StateId=' + cid,
        success: function (result) {

            $.each(result, function (i, data) {
                $('#' + drpProjectstate).append('<Option value=' + data.id + '>' + data.stateName + '</Option>')
            });
        }
    });
}

function fn_getProjectcitiesbystateId(drpProjectcity, stateid, that) {

    var sid = stateid;
    if (sid == undefined || sid == null) {
        var sid = $(that).val();
    }


    $('#' + drpProjectcity).empty();
    $('#' + drpProjectcity).append('<Option >--Select City--</Option>');
    $.ajax({
        url: '/Authentication/GetCity?CityId=' + sid,
        success: function (result) {

            $.each(result, function (i, data) {
                $('#' + drpProjectcity).append('<Option value=' + data.id + '>' + data.cityName + '</Option>');

            });
        }
    });
}

function GetProjectCountry() {
    $.ajax({
        url: '/Authentication/GetCountrys',
        success: function (result) {
            $.each(result, function (i, data) {

                $('#projectCountry').append('<Option value=' + data.id + '>' + data.countryName + '</Option>')

            });
        }
    });
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