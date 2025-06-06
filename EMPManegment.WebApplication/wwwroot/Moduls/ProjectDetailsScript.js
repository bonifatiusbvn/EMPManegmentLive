var Formdata = window.userFormPermissions || 0;

$(document).ready(function () {
    GetAllUserProjectDetailsList();

    GetMemberList();

    $('#projectMemberDropdown').select2({
        placeholder: 'Select Members',
        allowClear: true,
        tags: true,
        dropdownCssClass: 'select2-teal',
        dropdownParent: $('#inviteMembersModal'),
        width: '100%'
    });
});

function GetMemberList() {
    $.ajax({
        url: '/Task/GetUserName',
        method: 'GET',
        success: function (response) {

            var $dropdown = $('#projectMemberDropdown');
            $dropdown.empty();

            if (Array.isArray(response)) {
                response.forEach(item => {
                    const fullName = `${item.firstName} ${item.lastName}`;
                    $dropdown.append(new Option(fullName, item.id, false, false));
                });
            }

            $dropdown.trigger('change.select2');
        },
        error: function (xhr, status, error) {
            console.error('Error fetching member list:', error);
        }
    });
}



$(document).ready(function () {
    $('#projectPriority').select2({
        placeholder: 'Select Project Priority',
        width: '100%',
        minimumResultsForSearch: Infinity,
    });
    $('#projectStatus').select2({
        placeholder: 'Select Project Status',
        width: '100%',
        minimumResultsForSearch: Infinity,
    });
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

        formData.append("Area", $("#txtProjectArea").val());
        formData.append("BuildingName", $("#txtBuildingName").val());
        formData.append("State", $("#ProjectState").val());
        formData.append("City", $("#ProjectCity").val());
        formData.append("Country", $("#projectCountry").val());
        formData.append("Pincode", $("#txtProjectPincode").val());
        formData.append("ProjectPath", $("#projectPath").val());
        var fileInput = document.getElementById("fileUpload");
        if (fileInput.files.length > 0) {
            formData.append("ProjectImage", fileInput.files[0]);
        }
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
        },
        errorPlacement: function (error, element) {

            if (element.hasClass("select2-hidden-accessible")) {
                error.insertAfter(element.next('.select2-container'));
            }
            else {
                error.insertAfter(element);
            }
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

    $('.select2').on('change', function () {
        $(this).valid();
    });
});

$(document).ready(function () {
    $('#inviteMembersModal').on('hidden.bs.modal', function () {
        $('#projectMemberDropdown').val(null).trigger('change');
    });
});

function openmemberpop() {
    var ProjectId = $('#projectid').val();
    showMember(ProjectId);
    $("#inviteMembersModal").modal('show');
}

function showMember(ProjectId) {
    var formData = new FormData();
    formData.append("ProjectId", ProjectId);
    $.ajax({
        url: '/Project/GetMemberList',
        type: 'Post',
        data: formData,
        processData: false,
        contentType: false,
        dataType: 'html',
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
    var memberName = $(button).closest('.mx-n4').find('.full_name').text().trim();
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

function invitemember() {
    var selectElement = document.getElementById('projectMemberDropdown');
    var MemberDetails = [];

    $(selectElement.options).each(function () {
        var option = this;
        if (option.selected) {
            var memberName = option.text.trim();

            var objData = {
                Fullname: memberName
            };
            MemberDetails.push(objData);
        }
    });
    if (MemberDetails.length > 0) {
        var proProjectId = $('#projectid').val();
        var UpdatedBy = $('#memberUpdatedby').val();

        var MemberData = {
            ProjectId: proProjectId,
            UpdatedBy: UpdatedBy,
            ProjectMemberList: MemberDetails,
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
                    toastr.warning(Result.message);
                    $("#inviteMembersModal").modal('hide');
                    showProjectMembers(proProjectId)
                }
            },
        })
    } else {
        toastr.warning("Select member you want to add!");
    }
}
$(document).ready(function () {
    var ShowProjectMemberProjectId = $('#ShowProjectMemberProjectId').val();
    if (ShowProjectMemberProjectId) {
        showProjectMembers(ShowProjectMemberProjectId)
        showProjectDocuments(ShowProjectMemberProjectId)
    }
})
function showProjectMembers(ProjectId) {
    debugger
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

let isProjectGridInitialized = false;

function showTeamsPagination(ProjectId) {
    const myGridElement = document.querySelector('#ProjectMembersTable');

    if (isProjectGridInitialized) {
        return;
    }

    let ProjectMemberGridOptions = {
        rowHeight: 50,
        columnDefs: [
            {
                headerName: "User Name",
                field: "firstName",
                sortable: true,
                filter: true,
                cellRenderer: function (params) {
                    if (!params.data?.id) return '';
                    const colors = [
                        { bg: 'bg-primary-subtle', text: 'text-primary' },
                        { bg: 'bg-secondary-subtle', text: 'text-secondary' },
                        { bg: 'bg-success-subtle', text: 'text-success' },
                        { bg: 'bg-info-subtle', text: 'text-info' },
                        { bg: 'bg-warning-subtle', text: 'text-warning' },
                        { bg: 'bg-danger-subtle', text: 'text-danger' },
                        { bg: 'bg-dark-subtle', text: 'text-dark' }
                    ];

                    let profileHtml;
                    if (params.data.image?.trim()) {
                        profileHtml = `<img src="/${params.data.image}" style="height: 40px; width: 40px; border-radius: 50%;">`;
                    } else {
                        const initials = `${params.data.firstName?.[0] || ''}${params.data.lastName?.[0] || ''}`.toUpperCase();
                        const color = colors[Math.floor(Math.random() * colors.length)];
                        profileHtml = `<div class="flex-shrink-0 avatar-xs me-2">
                            <div class="avatar-title ${color.bg} ${color.text} rounded-circle fs-13" style="height: 40px; width: 40px;">${initials}</div>
                        </div>`;
                    }

                    return `<div class="d-flex align-items-center">${profileHtml}
                        <div class="flex-grow-1 tasks_name ml-2" style="color: #16989A !important; margin-left: 10px">${params.data.firstName} ${params.data.lastName}</div>
                    </div>`;
                }
            },
            {
                headerName: "Designation",
                field: "designation",
                sortable: true,
                filter: true
            }
        ],
        defaultColDef: {
            sortable: true,
            filter: true,
            cellClass: 'ag-cell-default-style',
            width: 175,
        },
        rowSelection: 'single',
        rowClassRules: {
            'selected-row': params => params.node.isSelected()
        },
        onGridReady: function (params) {
            params.api.sizeColumnsToFit();
        },
        rowModelType: 'infinite',
        cacheBlockSize: 10,
        maxBlocksInCache: 2,
        datasource: {
            getRows: function (params) {
                const request = {
                    StartRow: params.startRow,
                    PageSize: ProjectMemberGridOptions.cacheBlockSize || 10,
                    SearchType: "",
                    SearchValue: "",
                    SortModel: params.sortModel || [],
                    SortColumn: (params.sortModel?.length > 0) ? params.sortModel[0].colId : "",
                    SortDirection: (params.sortModel?.length > 0) ? params.sortModel[0].sort : "",
                    filters: Object.entries(params.filterModel || {}).map(([key, value]) => ({
                        colId: key,
                        filterValue: value.filter
                    })),
                    ProjectFilter: ProjectId,
                };

                $.ajax({
                    url: '/Project/ShowTeam',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(request),
                    success: function (response) {
                        params.successCallback(response.rowsThisPage, response.totalRowCount);
                    },
                    error: function () {
                        params.failCallback();
                    }
                });
            }
        }
    };

    const userPermissionArray = Formdata;
    let canDelete = false;

    for (let i = 0; i < userPermissionArray.length; i++) {
        const permission = userPermissionArray[i];
        if (permission.formName === "Projec tDetails") {
            canDelete = permission.delete;
            break;
        }
    }

    if (canDelete) {
        ProjectMemberGridOptions.columnDefs.push({
            headerName: "Action",
            field: "action",
            sortable: false,
            filter: false,
            cellRenderer: function (params) {
                if (!params.data || !params.data.id) {
                    return '';
                }
                return `<a class="btn" onclick="EditProjectMemberDesignation('${params.data.id}')"><i class="fa-regular fa-pen-to-square" style="color: #16989A;"></i></a><a class="btn" onclick="deleteProjectMember('${params.data.userId}')"><i class="fas fa-trash" style="color: #16989A;"></i></a>`;
            }
        });
    }

    agGrid.createGrid(myGridElement, ProjectMemberGridOptions);

    isProjectGridInitialized = true;
}

function EditProjectMemberDesignation(projectMemberId) {
    $.ajax({
        url: '/Project/EditProjectMemberDesignation?ProjectMemberId=' + projectMemberId,
        type: 'GET',
        dataType: 'json',
        success: function (result) {
            if (result) {
                $("#UpdateMembersDesignationModal").modal('show');
                $('#ProjectMemberDesignation').val(result.projectDesignation);
                $('#ProjectUserId').val(result.userId);
                $('#ProjectId').val(result.projectId);
            } else {
                alert("Failed to load project member details.");
            }
        },
        error: function () {
            alert("An error occurred while fetching data.");
        }
    });
}


function UpdateProjectMemberDesignation() {
    if ($("#UpdateMembersDesignationForm").valid()) {
        var ProjectId = $('#ProjectId').val()
        var UpdateDesignationdata = {
            ProjectId: ProjectId,
            UserId: $('#ProjectUserId').val(),
            ProjectDesignation: $('#ProjectMemberDesignation').val(),
            UpdatedBy: $('#ProjectUpdatedBy').val(),
        }
        var form_data = new FormData();
        form_data.append("UpdateDesignation", JSON.stringify(UpdateDesignationdata));

        $.ajax({
            url: '/Project/UpdateProjectMemberDesignation',
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
                        window.location = '/Project/ProjectDetails/?Id=' + ProjectId;
                    });
                }
                else {
                    toastr.warning(Result.message);
                    $("#UpdateMembersDesignationModal").modal('hide');
                }
            },
        })
    }
}
$(document).ready(function () {

    $("#UpdateMembersDesignationForm").validate({
        rules: {
            ProjectMemberDesignation: "required",
        },
        messages: {
            ProjectMemberDesignation: "Please Enter Project Designation",
        },
    })
});

function addProjectDocument() {
    var document = $("#txtDocumentName")[0].files[0];
    if (document != undefined) {
        var formData = new FormData();
        var ProjectId = $('#projectid').val();
        formData.append("ProjectId", ProjectId);
        formData.append("DocumentName", $("#txtDocumentName")[0].files[0]);

        $.ajax({
            url: '/Project/AddDocumentToProject',
            type: 'POST',
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
                        window.location = '/Project/ProjectDetails/?Id=' + ProjectId;
                    });
                } else {
                    toastr.error(Result.message);
                }
            },
            error: function () {
                toastr.error("Can't get Data");
            }
        });
    }
    else {
        toastr.warning('Please select document!');
    }
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

let isProjectDocumentGridInitialized = false;

function showuploadDocuments(ProjectId) {
    const myProjectDocumentGridElement = document.querySelector('#ProjectDocumentTable');

    if (isProjectDocumentGridInitialized) {
        return;
    }

    let ProjectDocumentGridOptions = {
        rowHeight: 50,
        columnDefs: [
            {
                headerName: "Document Name",
                field: "documentName",
                sortable: true,
                filter: true,
                cellRenderer: function (params) {
                    if (!params.data?.id) return '';

                    return params.data.documentName.substring(37);
                }
            },
            {
                headerName: "User Name",
                field: "firstName",
                sortable: true,
                filter: true,
                cellRenderer: function (params) {
                    if (!params.data?.id) return '';

                    return params.data.firstName + " " + params.data.lastName;
                }
            },
            {
                headerName: "Date",
                field: "date",
                sortable: true,
                filter: true,
                cellRenderer: function (params) {
                    if (!params.data?.id) return '';

                    return getCommonDateformat(params.data.date);
                }
            },
        ],
        defaultColDef: {
            sortable: true,
            filter: true,
            cellClass: 'ag-cell-default-style',
            width: 175,
        },
        rowSelection: 'single',
        rowClassRules: {
            'selected-row': params => params.node.isSelected()
        },
        onGridReady: function (params) {
            params.api.sizeColumnsToFit();
        },
        rowModelType: 'infinite',
        cacheBlockSize: 10,
        maxBlocksInCache: 2,
        datasource: {
            getRows: function (params) {
                const request = {
                    StartRow: params.startRow,
                    PageSize: ProjectDocumentGridOptions.cacheBlockSize || 10,
                    SearchType: "",
                    SearchValue: "",
                    SortModel: params.sortModel || [],
                    SortColumn: (params.sortModel?.length > 0) ? params.sortModel[0].colId : "",
                    SortDirection: (params.sortModel?.length > 0) ? params.sortModel[0].sort : "",
                    filters: Object.entries(params.filterModel || {}).map(([key, value]) => ({
                        colId: key,
                        filterValue: value.filter
                    })),
                    ProjectFilter: ProjectId,
                };

                $.ajax({
                    url: '/Project/ShowUploadedDocuments',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(request),
                    success: function (response) {
                        params.successCallback(response.rowsThisPage, response.totalRowCount);
                    },
                    error: function () {
                        params.failCallback();
                    }
                });
            }
        }
    };

    const userPermissionArray = Formdata;
    let canDelete = false;

    for (let i = 0; i < userPermissionArray.length; i++) {
        const permission = userPermissionArray[i];
        if (permission.formName === "Projec tDetails") {
            canDelete = permission.delete;
            break;
        }
    }

    if (canDelete) {
        ProjectDocumentGridOptions.columnDefs.push({
            headerName: "Action",
            field: "action",
            sortable: false,
            filter: false,
            cellRenderer: function (params) {
                if (!params.data || !params.data.id) {
                    return '';
                }
                return `<a onclick="DownloadProjectDocument('${params.data.documentName}')"><i class="fas fa-cloud-download-alt" style="color: #16989A;"></i></a><a onclick="deleteProjectDocument('${params.data.id}')"><i class="fas fa-trash" style="color: #16989A;margin-left:10px;"></i></a>`;
            }
        });
    }

    agGrid.createGrid(myProjectDocumentGridElement, ProjectDocumentGridOptions);

    isProjectDocumentGridInitialized = true;
}
function GetAllUserProjectDetailsList(page) {
    const projectStatus = $("#ddlProjectStatus").val();
    const projectpriority = $("#ddlProjectPriority").val();
    const fromDate = $("#txtStartDate").val();
    const toDate = $("#txtEndDate").val();
    const searchValue = $("#txtProjectSearch").val();

    const requestData = {
        ProjectStatus: (projectStatus === "-- Status --" || projectStatus === "All" || projectStatus === "") ? null : projectStatus,
        Projectpriority: (projectpriority === "-- Priority --" || projectpriority === "All" || projectpriority === "") ? null : projectpriority,
        FromDate: fromDate || null,
        ToDate: toDate || null,
        SearchValue: searchValue?.trim() || null,
        Page: page || 1
    };

    $.ajax({
        url: '/Project/GetAllUserProjectList',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(requestData),
        success: function (result) {
            $("#getallprojectlist").html(result);
        },
        error: function () {
            toastr.error("Failed to load project list.");
        }
    });
}

// Trigger on filter changes
$('#ddlProjectStatus').change(() => GetAllUserProjectDetailsList(1));
$('#ddlProjectPriority').change(() => GetAllUserProjectDetailsList(1));

// Trigger on search (Enter key)
$(document).on("keyup", "#txtProjectSearch", function (e) {
    if (e.key === "Enter") {
        GetAllUserProjectDetailsList(1);
    }
});

// Trigger on pagination
$(document).on("click", ".pagination a", function (e) {
    e.preventDefault();
    const page = $(this).data("page") || $(this).text();
    GetAllUserProjectDetailsList(parseInt(page));
});

// Apply date filters
$(document).on("click", "#applyDateFilters", function (e) {
    e.preventDefault();
    GetAllUserProjectDetailsList(1);
});

// Toggle date filter container
$(document).on("click", "#toggleDateFilter", function (e) {
    e.preventDefault();
    $("#dateFilterContainer").toggle();
});

// Reset all filters
$(document).on("click", "#btnResetFilters", function () {
    $("#ddlProjectStatus").val("-- Status --").trigger('change');
    $("#ddlProjectPriority").val("-- Priority --").trigger('change');
    $("#txtStartDate").val("");
    $("#txtEndDate").val("");
    $("#txtProjectSearch").val("");
    GetAllUserProjectDetailsList(1);
});

// Initial load
$(document).ready(function () {
    GetAllUserProjectDetailsList(1);
});



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
    projectInvoiceActivity(ProId);
})

function projectInvoiceActivity(ProId) {

    $.ajax({
        url: '/Project/GetInvoiceActivity?ProId=' + ProId,
        type: 'Get',
        dataType: 'json',
        processData: false,
        contentType: false,
        complete: function (Result) {

            $('#projectActivityinoverview').html(Result.responseText);
        },
    })
}

//function projectinvoiceActivity(ProId) {

//    $.ajax({
//        url: '/Task/GetProjectActivity?ProId=' + ProId,
//        type: 'Get',
//        dataType: 'json',
//        processData: false,
//        contentType: false,
//        complete: function (Result) {

//            $('#invoiceactivity').html(Result.responseText);
//        },
//    })
//}


function projectActivity(ProjectId) {

    $.ajax({
        url: '/Project/GetProjectActivityDetails?ProjectId=' + ProjectId,
        type: 'Post',
        dataType: 'json',
        processData: false,
        contentType: false,
        complete: function (Result) {
            $('#projectActivity').html(Result.responseText);
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

function fn_UpdateProjectDetail() {
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

        var fileInput = document.getElementById("fileUpload");
        var existingImageName = $("#currentProjectImageName").text().trim();

        if (fileInput.files.length > 0) {
            formData.append("ProjectImage", fileInput.files[0]);
        } else {
            formData.append("ProjectImageName", existingImageName);
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

function DownloadProjectDocument(documentName) {
    $.ajax({
        url: '/Project/DownloadProjectDocument?DocumentName=' + documentName,
        type: "GET",
        dataType: 'json',
        success: function (result) {
            siteloaderhide();

            if (result && result.memory) {
                try {
                    var binaryString = window.atob(result.memory);
                    var length = binaryString.length;
                    var bytes = new Uint8Array(length);

                    for (var i = 0; i < length; i++) {
                        bytes[i] = binaryString.charCodeAt(i);
                    }

                    var blob = new Blob([bytes], { type: result.contentType });

                    var link = document.createElement('a');
                    link.href = window.URL.createObjectURL(blob);
                    link.setAttribute('download', result.fileName);

                    document.body.appendChild(link);
                    link.click();

                    document.body.removeChild(link);
                } catch (e) {
                    toastr.error("Error decoding file: " + e.message);
                }
            } else {
                toastr.warning(result.Message || "No document found for selected");
            }
        },
        error: function () {
            siteloaderhide();
            toastr.error("Can't get data");
        }
    });
}
