$(document).ready(function () {
    GetUserTotalTask()
    GetAllUserTaskDetail()
});


function showadddetails() {
    ClearTextBox();
    GetTaskType();

    var ProjectName = $("#drpProjectName").val();
    if (ProjectName == "All Project") {
        Swal.fire({
            title: "Kindly select project on dashboard.",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        });
    }
    else {
        $('#addtasks').modal('show');
    }
}

$(document).ready(function () {

    $('#ddlusername').select2({
        placeholder: 'Select Employee',
        width: '100%',
        dropdownParent: $('#addtasks'),
        ajax: {
            url: '/Task/GetUserName',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                
                return {
                    results: data.map(item => ({
                        id: item.id,
                        text: item.firstName + ' ' + item.lastName + ' (' + item.userName + ')',
                    }))
                };
            },
            error: function (xhr, status, error) {
                console.error("Error fetching vendor list:", error);
            }
        }
    });
});

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

var Formdata = window.userFormPermissions || 0;

let TaskGridOptions = [];
let startDate = null;
let endDate = null;

$(document).ready(function () {
    TaskGridOptions = {
        rowHeight: 50,
        columnDefs: [
            {
                headerName: "User Id", field: "userName", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';
                    return `<span style="color: #16989A !important;">` + params.data.userName + `</span>`;
                }
},
            { headerName: "Task Title", field: "taskTitle", sortable: true, filter: true },
            {
                headerName: "Task Details", field: "taskDetails", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';
                    if (params.data.type === 'display') {
                        let taskDetails = params.data.taskDetails;
                        if (params.data.length > 50) {
                            taskDetails = '<span title="' + params.data.taskDetails + '">' + data.substr(0, 50) + '...</span>';
                        }
                        if (params.data.document) {
                            let documentLink = '<div class="d-flex align-items-center">' +
                                '<div class="flex-grow-1">' + taskDetails + '</div>' +
                                '<div class="flex-shrink-0 ms-4 task-icons">' +
                                '<li class="list-inline tasks-list-menu mb-0" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="Document">' +
                                '<a class="flex-shrink-0 ms-4 task-icons" onclick="DownloadTaskDocument(\'' + params.data.document + '\')">' +
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
                    return params.data.taskDetails;
                }
            },
            {
                headerName: "Task Type", field: "taskType", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';
                    if (params.data.taskTypeName == "HighPriority") {
                        return '<a class="badge bg-danger-subtle text-danger text-uppercase">' + params.data.taskTypeName + '</a>';
                    } else if (params.data.taskTypeName == "MediumPriority") {
                        return '<a class="badge bg-warning-subtle text-warning text-uppercase">' + params.data.taskTypeName + '</a>';
                    } else {
                        return '<a class="badge bg-success-subtle text-success text-uppercase">' + params.data.taskTypeName + '</a>';
                    }                }
            },
            {
                headerName: "Task Date", field: "taskDate", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';
                    return getCommonDateformat(params.data.taskDate);
                }
            },
            {
                headerName: "Task End Date", field: "taskEndDate", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';
                    return getCommonDateformat(params.data.taskEndDate);
                }
            },
            {
                headerName: "Task Status", field: "taskStatus", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';
                    var badgeClass = 'bg-info';
                    if (params.data.taskStatus === "Working") {
                        badgeClass = 'bg-warning';
                    } else if (params.data.taskStatus === "Completed") {
                        badgeClass = 'bg-success';
                    } else if (params.data.taskStatus === "Pending") {
                        badgeClass = 'bg-secondary';
                    } else if (params.data.taskStatus === "InReview") {
                        badgeClass = 'bg-orange';
                    } else if (params.data.taskStatus === "InReview") {
                        badgeClass = 'bg-danger';
                    }

                    return '<a class="badge ' + badgeClass + ' text-uppercase">' + params.data.taskStatus + '</a>';
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
            TaskGridOptions.api = params.api;
            TaskGridOptions.columnApi = params.columnApi;
            TaskGridOptions.api.sizeColumnsToFit();
            createEnhancedPagination(params.api);
        },
        rowModelType: 'infinite',
        cacheBlockSize: 20,
        pagination: true,
        paginationPageSize: 20,
        suppressPaginationPanel: true,
        datasource: getTaskDatasource()
    };

    function getTaskDatasource() {
        return {
            getRows: function (params) {
                const request = {
                    StartRow: params.startRow,
                    PageSize: params.endRow - params.startRow,
                    SearchType: "",
                    SearchValue: "",
                    SortModel: params.sortModel || [],
                    SortColumn: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].colId : "",
                    SortDirection: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].sort : "",
                    filters: Object.entries(params.filterModel || {}).map(([key, value]) => ({
                        colId: key,
                        filterValue: value.filter
                    })),
                    searchValue: $('#txtTaskSearch').val(),
                    UserFilter: $('#txtTaskUserName').val(),
                    ProjectFilter: $('#txtTaskProjectName').val(),
                    TasktypeFilter: $('#txtTaskTypeName').val(),
                   TaskStatusFilter: $('#txtTaskStatusName').val(),
                    StartDate: startDate,
                    EndDate: endDate,
                };

                $.ajax({
                    url: '/Task/GetAllTaskList',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(request),
                    success: function (response) {
                        params.successCallback(response.rowsThisPage, response.totalRowCount);
                        const rangeDisplay = document.querySelector('#TaskTable .range-display');
                        if (rangeDisplay) updateEnhancedPagination(TaskGridOptions.api, rangeDisplay);
                    },
                    error: function () {
                        params.failCallback();
                    }
                });
            }
        };
    }

    function createEnhancedPagination(gridApi) {
        const paginationContainer = document.createElement('div');
        paginationContainer.className = 'enhanced-pagination-container';

        const pageSizeContainer = document.createElement('div');
        pageSizeContainer.className = 'page-size-container';
        pageSizeContainer.innerHTML = `
            <span>Page Size: </span>
            <select class="page-size-selector">
                <option value="10">10</option>
                <option value="20" selected>20</option>
                <option value="50">50</option>
                <option value="100">100</option>
            </select>
        `;

        const rangeDisplay = document.createElement('div');
        rangeDisplay.className = 'range-display';

        const navContainer = document.createElement('div');
        navContainer.className = 'navigation-container';

        const prevButton = document.createElement('button');
        prevButton.className = 'pagination-button';
        prevButton.innerHTML = '<i class="ri-arrow-left-s-line"></i> Previous';
        prevButton.addEventListener('click', () => {
            gridApi.paginationGoToPreviousPage();
            updateEnhancedPagination(gridApi, rangeDisplay);
        });

        const nextButton = document.createElement('button');
        nextButton.className = 'pagination-button';
        nextButton.innerHTML = 'Next <i class="ri-arrow-right-s-line"></i>';
        nextButton.addEventListener('click', () => {
            gridApi.paginationGoToNextPage();
            updateEnhancedPagination(gridApi, rangeDisplay);
        });

        const pageButtonsContainer = document.createElement('div');
        pageButtonsContainer.className = 'page-buttons';

        navContainer.appendChild(prevButton);
        navContainer.appendChild(pageButtonsContainer);
        navContainer.appendChild(nextButton);

        const pageInfo = document.createElement('div');
        pageInfo.className = 'page-info';

        paginationContainer.appendChild(pageSizeContainer);
        paginationContainer.appendChild(rangeDisplay);
        paginationContainer.appendChild(navContainer);
        paginationContainer.appendChild(pageInfo);

        const eGui = document.querySelector('#TaskTable');
        const paginationEl = document.createElement('div');
        paginationEl.className = 'ag-paging-panel enhanced';
        paginationEl.appendChild(paginationContainer);
        eGui.appendChild(paginationEl);

        const pageSizeSelector = pageSizeContainer.querySelector('.page-size-selector');
        pageSizeSelector.addEventListener('change', function () {
            const newPageSize = Number(this.value);

            // Destroy and recreate grid with new block size
            const gridDiv = document.querySelector('#TaskTable');

            TaskGridOptions = {
                ...TaskGridOptions,
                cacheBlockSize: newPageSize,
                paginationPageSize: newPageSize,
                datasource: getTaskDatasource(),
            };

            // Clear old grid and re-init
            gridDiv.innerHTML = '';
            agGrid.createGrid(gridDiv, TaskGridOptions);
        });

        updateEnhancedPagination(gridApi, rangeDisplay);
    }

    function updateEnhancedPagination(gridApi, rangeDisplay) {
        const currentPage = gridApi.paginationGetCurrentPage() + 1;
        const totalPages = gridApi.paginationGetTotalPages();
        const totalRows = gridApi.paginationGetRowCount();
        const pageSize = TaskGridOptions.paginationPageSize;

        const startRow = totalRows > 0 ? ((currentPage - 1) * pageSize + 1) : 0;
        const endRow = totalRows > 0 ? Math.min(currentPage * pageSize, totalRows) : 0;

        rangeDisplay.textContent = totalRows > 0 ? `${startRow} to ${endRow} of ${totalRows}` : '0 to 0 of 0';

        const pageInfo = document.querySelector('.page-info');
        if (pageInfo) pageInfo.textContent = `Page ${currentPage} of ${totalPages || 1}`;

        const pageButtonsContainer = document.querySelector('.page-buttons');
        if (!pageButtonsContainer) return;

        pageButtonsContainer.innerHTML = '';

        const startPage = Math.max(1, currentPage - 1);
        const endPage = Math.min(totalPages, currentPage + 1);

        for (let i = startPage; i <= endPage; i++) {
            const pageButton = document.createElement('button');
            pageButton.className = `pagination-button ${i === currentPage ? 'active' : ''}`;
            pageButton.textContent = i;
            pageButton.addEventListener('click', () => {
                gridApi.paginationGoToPage(i - 1);
                updateEnhancedPagination(gridApi, rangeDisplay);
            });
            pageButtonsContainer.appendChild(pageButton);
        }

        const prevButton = document.querySelector('.navigation-container .pagination-button:first-child');
        const nextButton = document.querySelector('.navigation-container .pagination-button:last-child');
        if (prevButton) prevButton.disabled = currentPage === 1;
        if (nextButton) nextButton.disabled = currentPage === totalPages || totalPages === 0;

        const pageSizeSelector = document.querySelector('.page-size-selector');
        if (pageSizeSelector) pageSizeSelector.value = pageSize;
    }

    const userFormPermissionArray = Formdata;
    let canEdit = false;
    let canDelete = false;
    for (let i = 0; i < userFormPermissionArray.length; i++) {
        if (userFormPermissionArray[i].formName === "Tasks List") {
            canEdit = userFormPermissionArray[i].edit;
            canDelete = userFormPermissionArray[i].delete;
            break;
        }
    }
    
    if (canEdit || canDelete) {
        TaskGridOptions.columnDefs.push({
            headerName: "Action",
            field: "actions",
            sortable: false,
            filter: false,
            cellRenderer: function (params) {
                if (!params.data || !params.data.id) return '';
                let buttons = '<div style="display: flex; align-items: center; gap: 10px;">';

                buttons += `
    <a onclick="btnTaskDetails('${params.data.id}')" title="View">
        <i class="ri-eye-fill fs-16"></i>
    </a>`;

                if (canEdit) {
                    buttons += `
        <a onclick="EditTaskDetails('${params.data.id}')" title="Edit">
            <i class="fa-regular fa-pen-to-square"></i>
        </a>`;
                }

                if (canDelete) {
                    buttons += `
        <a onclick="DeleteTask('${params.data.id}')" title="Delete">
            <i class="fas fa-trash"></i>
        </a>`;
                }

                buttons += '</div>';
                return buttons;

            }
        });
    }

    const myGridElement = document.querySelector('#TaskTable');
    agGrid.createGrid(myGridElement, TaskGridOptions);

    $('#txtTaskSearch').on('change keyup', function () {
        TaskGridOptions.api.onFilterChanged();
    });

    $('#txtTaskUserName').change(() => {
        const userText = $("#txtTaskUserName option:selected").text();
        $("#txtUserName").val(userText === 'All User' ? '' : userText);
        if (TaskGridOptions.api) {
            TaskGridOptions.api.onFilterChanged();
        }
    });

    $('#txtTaskProjectName').change(() => {
        const projectText = $("#txtTaskProjectName option:selected").text();
        $("#txtProjectName").val(projectText === 'All Project' ? '' : projectText);
        if (TaskGridOptions.api) {
            TaskGridOptions.api.onFilterChanged();
        }
    });

    $('#txtTaskTypeName').change(() => {
        const tasktypeText = $("#txtTaskTypeName option:selected").text();
        $("#txtTypeName").val(tasktypeText === 'All Type' ? '' : tasktypeText);
        if (TaskGridOptions.api) {
            TaskGridOptions.api.onFilterChanged();
        }
    });

    $('#txtTaskStatusName').change(() => {
        const userText = $("#txtTaskStatusName option:selected").text();
        $("#txtStatusName").val(userText === 'All Status' ? '' : userText);
        if (TaskGridOptions.api) {
            TaskGridOptions.api.onFilterChanged();
        }
    });

    $('#toggleDateFilter').click(e => {
        e.stopPropagation();
        $('#dateFilterContainer').toggle();
    });

    $('#applyFilters').click(() => {
        startDate = $('#txtstartdatebox').val() || null;
        endDate = $('#txtenddatebox').val() || null;
        if (TaskGridOptions.api) {
            TaskGridOptions.api.onFilterChanged();
        }
    });

    $('#txtTaskUserName').select2({
        placeholder: 'Select Employee',
        width: '100%',
        dropdownAutoWidth: true,
        allowClear: true,
        ajax: {
            url: '/Task/GetUserName',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                return {
                    results: data.map(item => ({
                        id: item.id,
                        text: `${item.firstName} ${item.lastName} ( ${item.userName} )`,
                    }))
                };
            }
        }
    });

    $('#txtTaskProjectName').select2({
        placeholder: 'Select Project',
        width: '100%',
        dropdownAutoWidth: true,
        allowClear: true,
        ajax: {
            url: '/Project/GetProjectsList',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                return {
                    results: data.map(item => ({
                        id: item.projectId,
                        text: item.projectTitle +' (' +  item.shortName + ')'
                    }))
                };
            },
        }
    });

    $('#txtTaskTypeName').select2({
        placeholder: 'Select Task type',
        width: '100%',
        dropdownAutoWidth: true,
        allowClear: true,
        ajax: {
            url: '/Task/GetTaskType',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                return {
                    results: data.map(item => ({
                        id: item.taskId,
                        text: item.taskType
                    }))
                };
            },
        }
    });
    $('#txtTaskStatusName').select2({
        placeholder: 'Select status',
        width: '100%',
        allowClear: true
    });
});
function ResetTaskFilterData() {
    window.location = '/Task/AllTaskDetails';
}
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