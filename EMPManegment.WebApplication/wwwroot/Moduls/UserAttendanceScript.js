
var Formdata = window.userFormPermissions || 0;

var selectedUserName = null;
var selectedDate = null;
var selectedStartDate = null;
var selectedEndDate = null;
var selectedMonth = null;
function clearTextBox() {
    $('#drpAttusername').find('option').not(':first').remove();
    $('#ddlmyattendanceser').find('option').not(':first').remove();
    $('#txtdate').val('');
    $('#txtstartdatebox').val('');
    $('#txtenddatebox').val('');
    $('#txtstartdate').val('');
    $('#txtenddate').val('');
    $('#txtmonth').val('');
}



let AllUserAttendanceGridOptions = [];
let startDate = null;
let endDate = null;

$(document).ready(function () {

    let startDate = null;
    let endDate = null;

    AllUserAttendanceGridOptions = {
        rowHeight: 50,
        columnDefs: [
            {
                headerName: "Employee Name", field: "firstName", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.attendanceId) {
                        return '';
                    }
                    return params.data.firstName + ' ' + params.data.lastName;
                }
            },
            {
                headerName: "Date", field: "date", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.attendanceId) {
                        return '';
                    }
                    return getCommonDateformat(params.data.date);
                }
            },
            {
                headerName: "Intime", field: "intime", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.attendanceId) {
                        return '';
                    }
                    return new Date(params.data.intime).toLocaleTimeString('en-US');
                }
            },
            {
                headerName: "Outtime", field: "outTime", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.attendanceId) {
                        return '';
                    }
                    var userDate = new Date(params.data.date).toLocaleDateString('en-US');
                    var todayDate = new Date().toLocaleDateString('en-US');
                    if (params.data.outTime != null) {
                        return new Date(params.data.outTime).toLocaleTimeString('en-US');
                    }
                    else if (params.data.outTime == null && userDate == todayDate) {
                        return "Pending...";
                    }
                    else {
                        return "Missing";
                    }
                }
            },
            {
                headerName: "Total Hours", field: "totalHours", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.attendanceId) {
                        return '';
                    }
                    var userDate = new Date(params.data.date).toLocaleDateString('en-US');
                    var todayDate = new Date().toLocaleDateString('en-US');
                    if (params.data.totalHours != null) {
                        return params.data.totalHours.substr(0, 8) + ' hr';
                    } else if (params.data.totalHours == null && userDate === todayDate) {
                        return "Pending...";
                    } else {
                        return "Missing";
                    }
                }
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
            AllUserAttendanceGridOptions.api = params.api;
            AllUserAttendanceGridOptions.columnApi = params.columnApi;
            AllUserAttendanceGridOptions.api.sizeColumnsToFit();

            createEnhancedPagination(params.api);
        },
        rowModelType: 'infinite',
        cacheBlockSize: 10,
        pagination: true,
        paginationPageSize: 20,
        suppressPaginationPanel: true,
        datasource: {
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
                    searchValue: $('#txtAllUserAttendanceSearch').val(),
                    UserFilter: $('#drpAttusername').val(),
                    StartDate: startDate,
                    EndDate: endDate,
                };

                $.ajax({
                    url: '/UserProfile/GetUserAttendanceList',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(request),
                    success: function (response) {
                        params.successCallback(response.rowsThisPage, response.totalRowCount);
                        const rangeDisplay = document.querySelector('#AllUserAttendanceTable .range-display');
                        if (rangeDisplay) {
                            updateEnhancedPagination(AllUserAttendanceGridOptions.api, rangeDisplay);
                        }
                    },
                    error: function () {
                        params.failCallback();
                    }
                });
            }
        }
    };

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


        const eGui = document.querySelector('#AllUserAttendanceTable');
        const paginationEl = document.createElement('div');
        paginationEl.className = 'ag-paging-panel enhanced';
        paginationEl.appendChild(paginationContainer);
        eGui.appendChild(paginationEl);


        const pageSizeSelector = pageSizeContainer.querySelector('.page-size-selector');
        pageSizeSelector.addEventListener('change', function () {
            const newPageSize = Number(this.value);


            AllUserAttendanceGridOptions.paginationPageSize = newPageSize;


            gridApi.setDatasource(AllUserAttendanceGridOptions.datasource);


            gridApi.paginationGoToPage(0);


            updateEnhancedPagination(gridApi, rangeDisplay);
        });

        updateEnhancedPagination(gridApi, rangeDisplay);
    }

    function updateEnhancedPagination(gridApi, rangeDisplay) {
        const currentPage = gridApi.paginationGetCurrentPage() + 1;
        const totalPages = gridApi.paginationGetTotalPages();
        const totalRows = gridApi.paginationGetRowCount();
        const pageSize = AllUserAttendanceGridOptions.paginationPageSize;


        const startRow = totalRows > 0 ? ((currentPage - 1) * pageSize + 1) : 0;
        const endRow = totalRows > 0 ? Math.min(currentPage * pageSize, totalRows) : 0;


        rangeDisplay.textContent = totalRows > 0 ? `${startRow} to ${endRow} of ${totalRows}` : '0 to 0 of 0';


        const pageInfo = document.querySelector('.page-info');
        if (pageInfo) {
            pageInfo.textContent = `Page ${currentPage} of ${totalPages || 1}`;
        }


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

        if (prevButton) {
            prevButton.disabled = currentPage === 1;
        }
        if (nextButton) {
            nextButton.disabled = currentPage === totalPages || totalPages === 0;
        }


        const pageSizeSelector = document.querySelector('.page-size-selector');
        if (pageSizeSelector) {
            pageSizeSelector.value = pageSize;
        }
    }


    const userFormPermissionArray = Formdata;
    let canEdit = false;

    for (let i = 0; i < userFormPermissionArray.length; i++) {
        const permission = userFormPermissionArray[i];
        if (permission.formName === "Users Attendance") {
            canEdit = permission.edit;
            break;
        }
    }

    if (canEdit) {
        AllUserAttendanceGridOptions.columnDefs.push({
            headerName: "Action",
            field: "actions",
            sortable: false,
            filter: false,
            cellRenderer: function (params) {
                if (!params.data || !params.data.attendanceId) {
                    return '';
                }

                let buttons = '';
                if (canEdit) {
                    buttons += `
                         <li class="list-inline-item"><a onclick="EditUserAttendance('${params.data.attendanceId}')"><i class="fa-regular fa-pen-to-square"></i></a></li>`;
                }
                return buttons;
            }
        });
    }

    const myGridElement = document.querySelector('#AllUserAttendanceTable');
    agGrid.createGrid(myGridElement, AllUserAttendanceGridOptions);

    $('#txtAllUserAttendanceSearch').on('change keyup', function () {
        AllUserAttendanceGridOptions.api.onFilterChanged();
    });
    $('#drpAttusername').change(() => {
        const userText = $("#drpAttusername option:selected").text();
        $("#txtUserName").val(userText === 'All User' ? '' : userText);
        if (AllUserAttendanceGridOptions.api) {
            AllUserAttendanceGridOptions.api.onFilterChanged();
        }
    });

    $('#toggleDateFilter').click(e => {
        e.stopPropagation();
        $('#dateFilterContainer').toggle();
    });

    $('#applyFilters').click(() => {
        startDate = $('#txtstartdatebox').val() || null;
        endDate = $('#txtenddatebox').val() || null;
        if (AllUserAttendanceGridOptions.api) {
            AllUserAttendanceGridOptions.api.onFilterChanged();
        }
    });

    $('#drpAttusername').select2({
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
                        text: item.firstName + ' ' + item.lastName + ' ( ' + item.userName + ' ) ',
                    }))
                };
            }
        }
    });
    $('#AddUserAttendanceModel').on('shown.bs.modal', function () {
        $('#ddlusername').select2({
            placeholder: 'Select User',
            width: '100%',
            dropdownParent: $('#AddUserAttendanceModel'),
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
                            text: item.firstName + ' ' + item.lastName + ' ( ' + item.userName + ' ) ',
                        }))
                    };
                }
            }
        });
    });
});


$(document).click(function (event) {
    const target = $(event.target);
    if (
        !target.closest('#dateFilterContainer').length &&
        !target.closest('#toggleDateFilter').length
    ) {
        $('#dateFilterContainer').hide();
    }
});

function ResetAllUserAttendanceData() {
    $('#drpAttusername').empty();
    $('#txtstartdatebox').val('');
    $('#txtenddatebox').val('');
    startDate = null;
    endDate = null;
    $('#dateFilterContainer').hide();
    AllUserAttendanceGridOptions.api.setFilterModel(null);
    AllUserAttendanceGridOptions.api.onFilterChanged();
}


function formatDateToLocal(date) {
    var year = date.getFullYear();
    var month = (date.getMonth() + 1).toString().padStart(2, '0');
    var day = date.getDate().toString().padStart(2, '0');
    var hours = date.getHours().toString().padStart(2, '0');
    var minutes = date.getMinutes().toString().padStart(2, '0');
    return `${year}-${month}-${day}T${hours}:${minutes}`;
}

function EditUserAttendance(attandenceId) {
    $('#EditTimeModel').modal('show');
    $.ajax({
        url: '/UserProfile/EditOutTime?attendanceId=' + attandenceId,
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            $.each(response, function (index, item) {
                $('#AttandanceId').val(item.attendanceId);
                $('#UserName').val(item.userName);
                $('#Date').val(getCommonDateformat(item.date));

                var intime = item.intime ? getCommonDatetime(item.date, item.intime) : '';
                $('#Intime').val(intime);

                var outTime = item.outTime ? getCommonDatetime(item.date, item.outTime) : '';
                $('#OutTime').val(outTime);
            });
        },
        error: function () {
            toastr.error("Can't get Data");
        }
    });
}

function UpdateUserAttendance() {
    var objData = {
        AttendanceId: $("#AttandanceId").val(),
        OutTime: $("#OutTime").val(),
        Intime: $("#Intime").val(),
        UserName: $("#UserName").val(),
        Date: $("#Date").val(),
    }


    if (objData.OutTime == "") {
        $("#OutTime").css('border-color', 'red');
        $("#OutTime").focus();
    }

    else {
        $("#OutTime").css('border-color', 'lightgray');
        $.ajax({
            url: '/UserProfile/UpdateOutTime',
            type: 'Post',
            data: objData,
            dataType: 'json',
            success: function (Result) {

                var ricon = "warning";

                if (Result.icone == ricon) {

                    Swal.fire({
                        title: Result.message,
                        icon: Result.icone,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/UserProfile/UsersAttendance';
                    });
                }
            },
        })
    }
}

function formatDate(date) {
    var day = String(date.getDate()).padStart(2, '0');
    var month = String(date.getMonth() + 1).padStart(2, '0');
    var year = date.getFullYear();

    return day + '-' + month + '-' + year;
}

function GetSearchAttendanceList() {
    var UserPermissionData = datas;
    $.ajax({
        url: '/UserProfile/GetSearchAttendanceList',
        type: 'GET',
        success: function (result) {

            $("#attendancedt").hide();
            $("#dvattendancelist").html(result);
            fn_SearchUserAttendanceList(UserPermissionData);
        },
        error: function () {
            alert('Error loading attendance list. Please try again.');
        }
    });
}

function fn_SearchUserAttendanceList(UserPermissionData) {
    var selectedValue = $('#ddlatendanceser').data('value');
    var isValid = true;
    var errorMessage = "Kindly fill all required fields";

    if (typeof selectedValue === "undefined") {
        isValid = false;
        errorMessage = "Please select a search criteria";
    } else if (selectedValue === "ByUsername" && $("#drpAttusername").val() === "") {
        isValid = false;
        errorMessage = "Please select a Username";
    } else if (selectedValue === "ByDate" && $("#txtdate").val() === "") {
        isValid = false;
        errorMessage = "Please select a Date";
    } else if (selectedValue === "ByDateAndUser" && ($("#drpAttusername").val() === "" || $("#txtdate").val() === "")) {
        isValid = false;
        errorMessage = "Please select both Username and Date";
    } else if (selectedValue === "ByDatesAndUser" && ($("#drpAttusername").val() === "" || $("#txtstartdatebox").val() === "" || $("#txtenddatebox").val() === "")) {
        isValid = false;
        errorMessage = "Please select Username, Start Date, and End Date";
    }

    if (isValid) {
        selectedUserName = $("#drpAttusername").val();
        selectedDate = $('#txtdate').val();
        selectedStartDate = $("#txtstartdatebox").val();
        selectedEndDate = $("#txtenddatebox").val();
        var FilterData = {
            Date: selectedDate,
            UserId: selectedUserName,
            StartDate: selectedStartDate,
            EndDate: selectedEndDate
        };
        GetUserSearchAttendanceList(FilterData, UserPermissionData);
    } else {
        $("#backbtn").hide();
        toastr.warning(errorMessage);
    }
}

function GetUserSearchAttendanceList(FilterData, UserPermissionData) {
    var userPermissionArray = JSON.parse(UserPermissionData);
    var canEdit = userPermissionArray.some(permission => permission.formName === "Users Attendance" && permission.edit);
    var columns = [
        { "data": "userName", "name": "UserName" },
        {
            "data": "date", "name": "Date",
            "render": function (data) {
                return getCommonDateformat(data);
            }
        },
        {
            "data": "intime", "name": "InTime",
            "render": function (data) {
                return new Date(data).toLocaleTimeString('en-US');
            }
        },
        {
            "data": "outTime", "name": "OutTime",
            "render": function (data, type, full) {
                var userDate = new Date(full.date).toLocaleDateString('en-US');
                var todayDate = new Date().toLocaleDateString('en-US');
                if (data) {
                    return new Date(data).toLocaleTimeString('en-US');
                } else if (userDate === todayDate) {
                    return "Pending...";
                } else {
                    return "Missing";
                }
            }
        },
        {
            "data": "totalHours", "name": "TotalHours",
            "render": function (data, type, full) {
                var userDate = new Date(full.date).toLocaleDateString('en-US');
                var todayDate = new Date().toLocaleDateString('en-US');
                if (full.totalHours) {
                    return full.totalHours.substr(0, 8) + ' hr';
                } else if (userDate === todayDate) {
                    return "Pending...";
                } else {
                    return "Missing";
                }
            }
        },
    ];

    if (canEdit) {
        columns.push({
            "data": null,
            "orderable": false,
            "searchable": false,
            "render": function (data, type, full) {
                return '<a onclick="editUserAttendanceSrc(\'' + full.attendanceId + '\')" class="btn text-info">' +
                    '<i class="fa-regular fa-pen-to-square"></i></a>';
            }
        });
    }

    $('#FilterAttendanceTable').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        destroy: true,
        pageLength: 30,
        lengthMenu: [[10, 25, 30, 50, -1], [10, 25, 30, 50, "All"]],
        ajax: {
            type: "POST",
            url: '/UserProfile/GetUserSearchAttendanceList',
            dataType: 'json',
            data: FilterData,
        },
        columns: columns,
        scrollY: 400,
        scrollX: true,
        scrollCollapse: true,
        fixedHeader: {
            header: true,
            footer: true
        },
        autoWidth: false,
        columnDefs: [
            {
                targets: '_all', width: 'auto'
            }
        ],
        order: [[1, 'asc']]
    });
}
$(document).ready(function () {
    function data(datas) {
        var userPermission = datas;
        GetMyAttendanceList(userPermission);
    }

    function GetMyAttendanceList(userPermission) {
        var userPermissionArray = JSON.parse(userPermission);
        var canEdit = userPermissionArray.some(permission => permission.formName === "Users Attendance" && permission.edit);
        var columns = [
            { "data": "userName", "name": "UserName" },
            {
                "data": "date", "name": "Date",
                "render": function (data, type, full, meta) {
                    return getCommonDateformat(data);
                }
            },
            {
                "data": "intime", "name": "InTime",
                render: function (data) {
                    return new Date(data).toLocaleTimeString('en-US');
                }
            },
            {
                "data": "outTime", "name": "OutTime",
                render: function (data, type, full) {
                    var userDate = new Date(full.date).toLocaleDateString('en-US');
                    var todayDate = new Date().toLocaleDateString('en-US');
                    if (data != null) {
                        return new Date(data).toLocaleTimeString('en-US');
                    }
                    else if (data == null && userDate == todayDate) {
                        return "Pending...";
                    }
                    else {
                        return "Missing";
                    }
                }
            },
            {
                "data": "totalHours", "name": "TotalHours",
                render: function (data, type, full) {
                    var userDate = new Date(full.date).toLocaleDateString('en-US');
                    var todayDate = new Date().toLocaleDateString('en-US');
                    if (full.totalHours != null) {
                        return full.totalHours.substr(0, 8) + ' hr';
                    } else if (full.totalHours == null && userDate === todayDate) {
                        return "Pending...";
                    } else {
                        return "Missing";
                    }
                }
            },
        ];

        if (canEdit) {
            columns.push({
                "data": null,
                "orderable": false,
                "searchable": false,
                "render": function (data, type, full) {
                    return '<a onclick="editMyAttendance(\'' + full.attendanceId + '\')" class="btn text-info">' +
                        '<i class="fa-regular fa-pen-to-square"></i></a>';
                }
            });
        }

        $('#MyAttendanceData').DataTable({
            processing: false,
            serverSide: true,
            filter: true,
            destroy: true,
            pageLength: 30,
            lengthMenu: [[10, 25, 30, 50, -1], [10, 25, 30, 50, "All"]],
            ajax: {
                type: "POST",
                url: '/UserProfile/GetAttendanceList',
                dataType: 'json'
            },
            columns: columns,
            scrollY: 400,
            scrollX: true,
            scrollCollapse: true,
            fixedHeader: {
                header: true,
                footer: true
            },
            autoWidth: false,
            columnDefs: [
                {
                    targets: '_all', width: 'auto'
                }
            ],
            order: [[1, 'asc']]
        });
    }


    data(datas);
});

function GetMySearchAttendanceList() {
    var UserPermissionData = datas;
    $.ajax({
        url: '/UserProfile/GetSearchAttendanceList',
        type: 'GET',
        success: function (result) {

            $("#attendancedt").hide();
            $("#GetMyAttendanceList").html(result);
            fn_SearchMyAttendanceList(UserPermissionData);
        },
        error: function () {
            alert('Error loading attendance list. Please try again.');
        }
    });
}
function fn_SearchMyAttendanceList(UserPermissionData) {
    if ($('#txtmonth').val() == "" && $("#txtstartdate").val() == "" && $("#txtenddate").val() == "") {
        toastr.warning("Select the Month or UserName");
    } else {
        selectedMonth = $('#txtmonth').val();
        selectedStartDate = $("#txtstartdate").val();
        selectedEndDate = $("#txtenddate").val();
        var FilterData = {
            Cmonth: selectedMonth,
            StartDate: selectedStartDate,
            EndDate: selectedEndDate
        };
        MySearchAttendanceList(UserPermissionData, FilterData);
    }
}

function MySearchAttendanceList(UserPermissionData, FilterData) {
    var userPermissionArray = JSON.parse(UserPermissionData);
    var canEdit = userPermissionArray.some(permission => permission.formName === "Users Attendance" && permission.edit);
    var columns = [
        { "data": "userName", "name": "UserName" },
        {
            "data": "date", "name": "Date",
            "render": function (data) {
                return getCommonDateformat(data);
            }
        },
        {
            "data": "intime", "name": "InTime",
            "render": function (data) {
                return new Date(data).toLocaleTimeString('en-US');
            }
        },
        {
            "data": "outTime", "name": "OutTime",
            "render": function (data, type, full) {
                var userDate = new Date(full.date).toLocaleDateString('en-US');
                var todayDate = new Date().toLocaleDateString('en-US');
                if (data) {
                    return new Date(data).toLocaleTimeString('en-US');
                } else if (userDate === todayDate) {
                    return "Pending...";
                } else {
                    return "Missing";
                }
            }
        },
        {
            "data": "totalHours", "name": "TotalHours",
            "render": function (data, type, full) {
                var userDate = new Date(full.date).toLocaleDateString('en-US');
                var todayDate = new Date().toLocaleDateString('en-US');
                if (full.totalHours) {
                    return full.totalHours.substr(0, 8) + ' hr';
                } else if (userDate === todayDate) {
                    return "Pending...";
                } else {
                    return "Missing";
                }
            }
        },
    ];

    if (canEdit) {
        columns.push({
            "data": null,
            "orderable": false,
            "searchable": false,
            "render": function (data, type, full) {
                return '<a onclick="editMyAttendance(\'' + full.attendanceId + '\')" class="btn text-info">' +
                    '<i class="fa-regular fa-pen-to-square"></i></a>';
            }
        });
    }

    $('#FilterAttendanceTable').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        destroy: true,
        pageLength: 30,
        lengthMenu: [[10, 25, 30, 50, -1], [10, 25, 30, 50, "All"]],
        ajax: {
            type: "POST",
            url: '/UserProfile/GetAttendanceList',
            dataType: 'json',
            data: FilterData,
        },
        columns: columns,
        scrollY: 400,
        scrollX: true,
        scrollCollapse: true,
        fixedHeader: {
            header: true,
            footer: true
        },
        autoWidth: false,
        columnDefs: [
            {
                targets: '_all', width: 'auto'
            }
        ],
        order: [[1, 'asc']],
        drawCallback: function (settings) {
            var api = this.api();
            var count = api.data().count();
            if (count > 0) {
                $("#attendancepdfexcel").show();
            } else {
                $("#attendancepdfexcel").hide();
            }
        }
    });
}

$('#backbtn').on('click', function (event) {
    window.location = '/UserProfile/UsersAttendance';
});

function editUserAttendanceSrc(attandenceId) {
    $.ajax({
        url: '/UserProfile/EditOutTime?attendanceId=' + attandenceId,
        type: 'Get',
        dataType: 'json',
        processData: false,
        contentType: false,
        success: function (response) {
            $.each(response, function (index, item) {
                $('#srcAttandanceId').val(item.attendanceId);
                $('#srcUserName').val(item.userName);
                $('#srcDate').val(getCommonDateformat(item.date));
                function formatDateToLocal(date) {
                    var yyyy = date.getFullYear();
                    var mm = (date.getMonth() + 1).toString().padStart(2, '0');
                    var dd = date.getDate().toString().padStart(2, '0');
                    var hh = date.getHours().toString().padStart(2, '0');
                    var mi = date.getMinutes().toString().padStart(2, '0');
                    var ss = date.getMinutes().toString().padStart(2, '0');

                    return `${yyyy}-${mm}-${dd}T${hh}:${mi}:${ss}`;
                }
                function setDateAttributes(selector, date, time) {
                    var minDate = new Date(date);
                    var maxDate = new Date(minDate);
                    maxDate.setDate(minDate.getDate() + 7);

                    var formattedMinDate = formatDateToLocal(minDate);
                    var formattedMaxDate = formatDateToLocal(maxDate);

                    $(selector).attr('min', formattedMinDate);
                    $(selector).attr('max', formattedMaxDate);

                    if (time) {
                        var timeDate = new Date(time);
                        var formattedTime = formatDateToLocal(timeDate);
                        $(selector).val(formattedTime);
                    } else {
                        $(selector).val(formattedMinDate);
                    }
                }
                if (item.intime == null) {
                    setDateAttributes('#srcIntime', item.date, null);
                } else {
                    setDateAttributes('#srcIntime', item.date, item.intime);
                }

                if (item.outTime == null) {
                    setDateAttributes('#srcOutTime', item.date, null);
                } else {
                    setDateAttributes('#srcOutTime', item.date, item.outTime);
                }

            });
            $('#editTimeModelsearch').modal('show');
        },
        error: function () {
            toastr.error("Can't get Data");
        }
    })
}

function editMyAttendance(attandenceId) {
    $.ajax({
        url: '/UserProfile/EditOutTime?attendanceId=' + attandenceId,
        type: 'Get',
        dataType: 'json',
        processData: false,
        contentType: false,
        success: function (response) {
            $.each(response, function (index, item) {
                $('#txtmyAttandanceId').val(item.attendanceId);
                $('#txtmyUserName').val(item.userName);
                $('#txtmyDate').val(getCommonDateformat(item.date));
                function formatDateToLocal(date) {
                    var yyyy = date.getFullYear();
                    var mm = (date.getMonth() + 1).toString().padStart(2, '0');
                    var dd = date.getDate().toString().padStart(2, '0');
                    var hh = date.getHours().toString().padStart(2, '0');
                    var mi = date.getMinutes().toString().padStart(2, '0');
                    var ss = date.getMinutes().toString().padStart(2, '0');

                    return `${yyyy}-${mm}-${dd}T${hh}:${mi}:${ss}`;
                }
                function setDateAttributes(selector, date, time) {
                    var minDate = new Date(date);
                    var maxDate = new Date(minDate);
                    maxDate.setDate(minDate.getDate() + 7);

                    var formattedMinDate = formatDateToLocal(minDate);
                    var formattedMaxDate = formatDateToLocal(maxDate);

                    $(selector).attr('min', formattedMinDate);
                    $(selector).attr('max', formattedMaxDate);

                    if (time) {
                        var timeDate = new Date(time);
                        var formattedTime = formatDateToLocal(timeDate);
                        $(selector).val(formattedTime);
                    } else {
                        $(selector).val(formattedMinDate);
                    }
                }
                if (item.intime == null) {
                    setDateAttributes('#txtmyIntime', item.date, null);
                } else {
                    setDateAttributes('#txtmyIntime', item.date, item.intime);
                }

                if (item.outTime == null) {
                    setDateAttributes('#txtmyOutTime', item.date, null);
                } else {
                    setDateAttributes('#txtmyOutTime', item.date, item.outTime);
                }

            });
            $('#editMyAttendanceTime').modal('show');
        },
        error: function () {
            toastr.error("Can't get Data");
        }
    })
}

var UserPermissionData = userPermissions;
function UpdateUserAttendanceSrc() {
    var objData = {
        AttendanceId: $("#srcAttandanceId").val(),
        OutTime: $("#srcOutTime").val(),
        Intime: $("#srcIntime").val(),
        UserName: $("#srcUserName").val(),
        Date: $("#srcDate").val(),
    }


    if (objData.OutTime == "") {
        $("#OutTime").css('border-color', 'red');
        $("#OutTime").focus();
    }

    else {
        $("#OutTime").css('border-color', 'lightgray');
        $.ajax({
            url: '/UserProfile/UpdateOutTime',
            type: 'Post',
            data: objData,
            dataType: 'json',
            success: function (Result) {
                if (Result.code != 200) {
                    toastr.warning(Result.message);
                }

                else {
                    Swal.fire({
                        title: Result.message,
                        icon: "success",
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        $('#editTimeModelsearch').modal('hide');

                        var FilterData = {
                            Date: selectedDate,
                            UserId: selectedUserName,
                            StartDate: selectedStartDate,
                            EndDate: selectedEndDate
                        };
                        GetUserSearchAttendanceList(FilterData, UserPermissionData);
                    });
                }
            },
        })
    }
}

function updateUserAttendance() {
    var date = $("#Date").val();
    var intime = $("#Intime").val();
    var outTime = $("#OutTime").val();

    var objData = {
        AttendanceId: $("#AttandanceId").val(),
        Intime: moment(intime).format('YYYY-MM-DD HH:mm:ss'),
        OutTime: outTime ? moment(outTime).format('YYYY-MM-DD HH:mm:ss') : null,
        UserName: $("#UserName").val(),
        Date: moment(date).format('YYYY-MM-DD'),
        UpdatedBy: $("#textUpdatedById").val(),
    };

    if (!objData.Intime) {
        $("#Intime").css('border-color', 'red');
        $("#Intime").focus();
    } else {
        $("#Intime").css('border-color', 'lightgray');
        $.ajax({
            url: '/UserProfile/UpdateOutTime',
            type: 'Post',
            data: objData,
            dataType: 'json',
            success: function (Result) {
                if (Result.code == 200) {
                    Swal.fire({
                        title: Result.message,
                        icon: "success",
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/UserProfile/UsersAttendance';
                    });
                } else {
                    toastr.warning(Result.message);
                }
            },
            error: function () {
                toastr.error("An error occurred while updating the attendance.");
            }
        });
    }
}

function updateMyAttendance() {
    var objData = {
        AttendanceId: $("#txtmyAttandanceId").val(),
        OutTime: $("#txtmyOutTime").val(),
        Intime: $("#txtmyIntime").val(),
        UserName: $("#txtmyUserName").val(),
        Date: $("#txtmyDate").val(),
    }

    if (objData.OutTime == "") {
        $("#OutTime").css('border-color', 'red');
        $("#OutTime").focus();
    }

    else {
        $("#OutTime").css('border-color', 'lightgray');
        $.ajax({
            url: '/UserProfile/UpdateOutTime',
            type: 'Post',
            data: objData,
            dataType: 'json',
            success: function (Result) {
                if (Result.code != 200) {
                    toastr.warning(Result.message);
                }
                else {
                    Swal.fire({
                        title: Result.message,
                        icon: "success",
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/UserProfile/MyAttendance';
                    });
                }
            },
        })
    }
}

function ExportToExcel() {
    siteloadershow();
    var objData = {
        Cmonth: selectedMonth,
        StartDate: selectedStartDate,
        EndDate: selectedEndDate,
        UserId: $("#AttandanceUserId").val()
    };

    $.ajax({
        url: '/UserProfile/ExportToExcel',
        type: 'GET',
        data: objData,
        success: function (data, status, xhr) {
            siteloaderhide();
            var filename = "";
            var disposition = xhr.getResponseHeader('Content-Disposition');
            if (disposition && disposition.indexOf('attachment') !== -1) {
                var matches = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/.exec(disposition);
                if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, '');
            }

            var type = xhr.getResponseHeader('Content-Type');
            var blob = new Blob([data], { type: type });

            if (typeof window.navigator.msSaveBlob !== 'undefined') {
                window.navigator.msSaveBlob(blob, filename);
            } else {
                var URL = window.URL || window.webkitURL;
                var downloadUrl = URL.createObjectURL(blob);

                if (filename) {
                    var a = document.createElement("a");
                    if (typeof a.download === 'undefined') {
                        window.location = downloadUrl;
                    } else {
                        a.href = downloadUrl;
                        a.download = filename;
                        document.body.appendChild(a);
                        a.click();
                    }
                } else {
                    window.location = downloadUrl;
                }

                setTimeout(function () { URL.revokeObjectURL(downloadUrl); }, 100);
            }
        },
        error: function (xhr, status, error) {
            siteloaderhide();
            toastr.warning("No data for selected month or date");
        },
        xhrFields: {
            responseType: 'blob'
        }
    });
}

function ExportToPDF() {
    siteloadershow();
    var objData = {
        Cmonth: selectedMonth,
        StartDate: selectedStartDate,
        EndDate: selectedEndDate,
        UserId: $("#AttandanceUserId").val()
    };

    $.ajax({
        url: '/UserProfile/ExportToPdf',
        type: 'POST',
        data: objData,
        success: function (data, status, xhr) {
            siteloaderhide();
            var filename = "";
            var disposition = xhr.getResponseHeader('Content-Disposition');
            if (disposition && disposition.indexOf('attachment') !== -1) {
                var matches = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/.exec(disposition);
                if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, '');
            }

            var type = xhr.getResponseHeader('Content-Type');
            var blob = new Blob([data], { type: type });

            if (typeof window.navigator.msSaveBlob !== 'undefined') {
                window.navigator.msSaveBlob(blob, filename);
            } else {
                var URL = window.URL || window.webkitURL;
                var downloadUrl = URL.createObjectURL(blob);

                if (filename) {
                    var a = document.createElement("a");
                    if (typeof a.download === 'undefined') {
                        window.location = downloadUrl;
                    } else {
                        a.href = downloadUrl;
                        a.download = filename;
                        document.body.appendChild(a);
                        a.click();
                    }
                } else {
                    window.location = downloadUrl;
                }

                setTimeout(function () { URL.revokeObjectURL(downloadUrl); }, 100);
            }
        },
        error: function (xhr, status, error) {
            siteloaderhide();
            toastr.warning("No data for selected month or date");
        },
        xhrFields: {
            responseType: 'blob'
        }
    });
}

function DisplayAddUserModel() {
    GetUsernameList();
    $('#AddUserAttendanceModel').modal('show');
}

function btnSaveUserAttendance() {
    siteloadershow();

    if ($('#frmadduserdetails').valid()) {
        var formData = new FormData();
        formData.append("UserId", $("#ddlusername").val());
        formData.append("Date", $("#txtDate").val());
        formData.append("Intime", $("#txtIntime").val());
        formData.append("OutTime", $("#txtOutTime").val());
        $.ajax({
            url: '/UserProfile/AddUserAttendance',
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
                        window.location = '/UserProfile/UsersAttendance';
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


