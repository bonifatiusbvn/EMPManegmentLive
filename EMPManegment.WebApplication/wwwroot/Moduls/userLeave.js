
function DisplayAddLeaveModel() {
    clearText();
    $('#AddUserLeaveModel').modal('show');
}

function clearText() {
    resetForm();
    $("#LeaveReason").empty();
    $("#LeaveFromDate").val('');
    $("#LeaveDays").val('');
    $("#LeaveToDate").val('');
    $("#LeaveDescription").val('');
    $("#LeaveApproveMemberDropdown").empty();
    $("#LeaveAttachment").val('');
}

function resetForm() {
    if (LeaveApplicationForm) {
        LeaveApplicationForm.resetForm();
    }
}

$(document).ready(function () {

    GetLeaveApproverList();

    $('#LeaveApproveMemberDropdown').select2({
        placeholder: 'Select Members',
        allowClear: true,
        tags: true,
        dropdownCssClass: 'select2-teal',
        width: '100%',
        dropdownParent: $('#AddUserLeaveModel'),
    });

    $('#LeaveReason').select2({
        placeholder: 'Select Reason',
        width: '100%',
        allowClear: true,
        minimumResultsForSearch: Infinity,
        dropdownParent: $('#AddUserLeaveModel'),

        ajax: {
            url: '/UserProfile/GetAllLeaveReasons',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                return {
                    results: data.map(item => ({
                        id: item.id,
                        text: item.leaveReason
                    }))
                };
            }
        }
    });
});

function GetLeaveApproverList() {
    debugger
    $.ajax({
        url: '/Task/GetUserName',
        method: 'GET',
        success: function (response) {
            debugger
            var $dropdown = $('#LeaveApproveMemberDropdown');
            $dropdown.empty();

            if (Array.isArray(response)) {
                debugger
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
    // Always disable To Date
    $("#LeaveToDate").prop("disabled", true);

    // Restrict From Date to today or later
    let today = new Date();
    let yyyy = today.getFullYear();
    let mm = String(today.getMonth() + 1).padStart(2, '0');
    let dd = String(today.getDate()).padStart(2, '0');
    let minDate = `${yyyy}-${mm}-${dd}`;
    $("#LeaveFromDate").attr("min", minDate);

    // Update To Date automatically
    $("#LeaveFromDate, #LeaveDays").on("input change", function () {
        let fromDateVal = $("#LeaveFromDate").val();
        let daysVal = parseFloat($("#LeaveDays").val());

        if (fromDateVal && daysVal > 0) {
            let fromDate = new Date(fromDateVal);
            fromDate.setDate(fromDate.getDate() + (daysVal - 1));

            let isoDate = fromDate.toISOString().split("T")[0];
            $("#LeaveToDate").val(isoDate);
        } else {
            $("#LeaveToDate").val("");
        }
    });
});

function AddUserLeaveApplication() {

  if ($("#LeaveApplicationForm").valid()) {
        var formData = new FormData();

        formData.append("Reason", $("#LeaveReason").val());
        formData.append("FromDate", $("#LeaveFromDate").val());
        formData.append("ToDate", $("#LeaveToDate").val());
        formData.append("Days", $("#LeaveDays").val());
        formData.append("Description", $("#LeaveDescription").val());
        formData.append("Approvers", ($("#LeaveApproveMemberDropdown").val() || []).join(","));

        var file = $("#LeaveAttachment")[0].files[0];
        if (file) {
            formData.append("LeaveAttachment", file);
        }

        $.ajax({
            url: '/UserProfile/AddUserLeaveApplication',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (Result) {
                if (Result.code === 200) {
                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(() => {
                        $("#LeaveApplicationForm")[0].reset();
                        $("#LeaveApproveMemberDropdown").val(null).trigger("change");
                    });
                } else {
                    toastr.error(Result.message);
                }
            },
            error: function () {
                toastr.error("Something went wrong. Please try again.");
            }
        });
    } else {
        toastr.error("Kindly fill all required details");
    }
}

var LeaveApplicationForm;
$(document).ready(function () {

    LeaveApplicationForm = $("#LeaveApplicationForm").validate({
        rules: {
            LeaveReason: { required: true },
            LeaveFromDate: { required: true },
            LeaveDays: { required: true, number: true, min: 0.5 },
            LeaveToDate: { required: true },
            Approvers: { required: true }
        },
        messages: {
            LeaveReason: "Please select a reason",
            LeaveFromDate: "Please select a start date",
            LeaveDays: {
                required: "Please enter number of days",
                number: "Please enter a valid number",
                min: "Minimum 0.5 days"
            },
            LeaveToDate: "End date is required",
            Approvers: "Please select at least one approver"
        },
        errorElement: "span",
        errorClass: "text-danger",
        highlight: function (element) {
            $(element).addClass("is-invalid");
        },
        unhighlight: function (element) {
            $(element).removeClass("is-invalid");
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
    $('.select2').on('change', function () {
        $(this).valid();
    });
});



let UserLeaveGridOptions = [];


$(document).ready(function () {

    UserLeaveGridOptions = {
        rowHeight: 50,
        columnDefs: [

            {
                headerName: "Applied on", field: "createdOn", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) {
                        return '';
                    }
                    return getCommonDateformat(params.data.createdOn);
                }
            },
            {
                headerName: "Status", field: "isApproved", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) {
                        return '';
                    }

                    if (params.data.isApproved === true) {
                        return '<span class="badge bg-success">Approved</span>';
                    }
                    else if (params.data.isApproved === false) {
                        return '<span class="badge bg-danger">Rejected</span>';
                    }
                    else {
                        return '<span class="badge bg-warning text-dark">Pending</span>';
                    }
                }
            },
            {
                headerName: "Approved By", field: "approvedByName", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) {
                        return '';
                    }
                    if (!params.data.approvedByName) {
                        return '<span>-----</span>';
                    }

                    return params.data.approvedByName;
                }
            },
            {
                headerName: "Approved On",
                field: "approveOn",
                sortable: true,
                filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) {
                        return '';
                    }

                    if (!params.data.approveOn) {
                        return '<span>-----</span>';
                    }

                    return getCommonDateformat(params.data.approveOn);
                }
            },

            {
                headerName: "Reason", field: "reasonName", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) {
                        return '';
                    }
                    return params.data.reasonName;
                }
            },

            {
                headerName: "FromDate", field: "fromDate", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) {
                        return '';
                    }
                    return getCommonDateformat(params.data.fromDate);
                }
            },
            {
                headerName: "ToDate", field: "toDate", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) {
                        return '';
                    }
                    return getCommonDateformat(params.data.toDate);

                }
            },
            {
                headerName: "Days", field: "days", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) {
                        return '';
                    }

                    return params.data.days;

                }
            },
            {
                headerName: "Approvers", field: "approverName", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) {
                        return '';
                    }

                    return params.data.approverName;

                }
            },
            {
                headerName: "Description", field: "description", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) {
                        return '';
                    }
                    return params.data.description;
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
            UserLeaveGridOptions.api = params.api;
            UserLeaveGridOptions.columnApi = params.columnApi;
            UserLeaveGridOptions.api.sizeColumnsToFit();
            createUserLeaveEnhancedPagination(params.api);
        },

        rowModelType: 'infinite',
        cacheBlockSize: 20,
        pagination: true,
        paginationPageSize: 20,
        suppressPaginationPanel: true,
        datasource: getUserLeaveDatasource()
    };
    function getUserLeaveDatasource() {
        return {
            getRows: function (params) {
                const request = {
                    StartRow: params.startRow,
                    PageSize: UserLeaveGridOptions.cacheBlockSize || 10,
                    SearchType: "",
                    SearchValue: "",
                    SortModel: params.sortModel || [],
                    SortColumn: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].colId : "",
                    SortDirection: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].sort : "",
                    filters: Object.entries(params.filterModel || {}).map(([key, value]) => ({
                        colId: key,
                        filterValue: value.filter
                    })),
                    searchValue: $('#txtUserLeaveSearch').val(),
                };

                $.ajax({
                    url: '/UserProfile/GetUserLeaveList',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(request),
                    success: function (response) {
                        params.successCallback(response.rowsThisPage, response.totalRowCount);
                        const rangeDisplay = document.querySelector('#UserLeaveTable .range-display');
                        if (rangeDisplay) updateUserLeaveEnhancedPagination(UserLeaveGridOptions.api, rangeDisplay);
                    },
                    error: function () {
                        params.failCallback();
                    }
                });
            }
        };
    }

    function createUserLeaveEnhancedPagination(gridApi) {
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
            updateUserLeaveEnhancedPagination(gridApi, rangeDisplay);
        });

        const nextButton = document.createElement('button');
        nextButton.className = 'pagination-button';
        nextButton.innerHTML = 'Next <i class="ri-arrow-right-s-line"></i>';
        nextButton.addEventListener('click', () => {
            gridApi.paginationGoToNextPage();
            updateUserLeaveEnhancedPagination(gridApi, rangeDisplay);
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

        const eGui = document.querySelector('#UserLeaveTable');
        const paginationEl = document.createElement('div');
        paginationEl.className = 'ag-paging-panel enhanced';
        paginationEl.appendChild(paginationContainer);
        eGui.appendChild(paginationEl);

        const pageSizeSelector = pageSizeContainer.querySelector('.page-size-selector');
        pageSizeSelector.addEventListener('change', function () {
            const newPageSize = Number(this.value);

            // Destroy and recreate grid with new block size
            const gridDiv = document.querySelector('#UserLeaveTable');

            UserLeaveGridOptions = {
                ...UserLeaveGridOptions,
                cacheBlockSize: newPageSize,
                paginationPageSize: newPageSize,
                datasource: getUserLeaveDatasource(),
            };

            // Clear old grid and re-init
            gridDiv.innerHTML = '';
            agGrid.createGrid(gridDiv, UserLeaveGridOptions);
        });

        updateUserLeaveEnhancedPagination(gridApi, rangeDisplay);
    }

    function updateUserLeaveEnhancedPagination(gridApi, rangeDisplay) {
        const currentPage = gridApi.paginationGetCurrentPage() + 1;
        const totalPages = gridApi.paginationGetTotalPages();
        const totalRows = gridApi.paginationGetRowCount();
        const pageSize = UserLeaveGridOptions.paginationPageSize;

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
                updateUserLeaveEnhancedPagination(gridApi, rangeDisplay);
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

    const myGridElement = document.querySelector('#UserLeaveTable');
    agGrid.createGrid(myGridElement, UserLeaveGridOptions);

    $('#txtUserLeaveSearch').on('change keyup', function () {
        UserLeaveGridOptions.api.onFilterChanged();
    });
});


$(document).ready(function () {
    loadLeaveRequests();
});

function loadLeaveRequests() {
    $.ajax({
        url: '/UserProfile/UserLeaveApproveRequest',
        type: 'GET',
        contentType: 'application/json',
        success: function (data) {
            var tbody = $("#leaveTable tbody");
            tbody.empty();

            $.each(data, function (i, item) {
                var status = item.isApproved === true ? "Approved" :
                    item.isApproved === false ? "Rejected" : "Pending";

                var row = `
                            <tr>
                                <td>${item.userName}</td>
                                <td>${item.reasonName}</td>
                                <td>${formatDate(item.fromDate)}</td>
                                <td>${formatDate(item.toDate)}</td>
                                <td>${item.days}</td>
                                <td>${item.description ?? ''}</td>
                                <td>${item.approverName ?? ''}</td>
                                <td>${status}</td>
                                <td>
    ${item.attachment
                        ? `<img src="/Content/LeaveDocuments/${item.attachment}" 
                 alt="Attachment" 
                 style="width:80px;height:80px;object-fit:cover;cursor:pointer;" 
                 onclick="window.open('/Content/LeaveDocuments/${item.attachment}', '_blank')">`
                        : 'No File'}
</td>
<td class="text-center">
    <i class="fa-solid fa-circle-check text-success fs-4 me-3"
       style="cursor:pointer;"
       title="Approve"
       onclick="ApproveRejectLeaveRequest('${item.id}','True')"></i>

    <i class="fa-solid fa-circle-xmark text-danger fs-4" 
       style="cursor:pointer;" 
       title="Reject"
       onclick="ApproveRejectLeaveRequest('${item.id}','False')"></i>
</td>

                            </tr>`;
                tbody.append(row);
            });
        },
        error: function (err) {
            console.log("Error fetching leave requests", err);
        }
    });
}

function formatDate(dateStr) {
    if (!dateStr) return '';
    let d = new Date(dateStr);
    return d.toLocaleDateString();
}

function ApproveRejectLeaveRequest(leaveId, isApproved) {
    debugger
    $.ajax({
        url: '/UserProfile/ApproveUserLeaveApplication?Id=' + leaveId + '&isApproved=' + isApproved,
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.code === 200) {
                Swal.fire({
                    title: result.message,
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK'
                }).then(() => {
                    window.location.href = "/UserProfile/UserLeave";  // ✅ redirect
                });
            } else {
                toastr.error(result.message);
            }
        },
        error: function () {
            toastr.error("Something went wrong. Please try again.");
        }
    });
}
