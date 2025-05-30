
var Formdata = window.userFormPermissions || [];

$(document).ready(function () {


    $('#ddlCompanyName,#ddlCDCompanyName').select2({
        placeholder: 'Select Company',
        width: '100%',
        dropdownAutoWidth: true,
        allowClear: true,
        ajax: {
            url: '/Company/GetCompanyNameList',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {

                return {
                    results: data.map(item => ({
                        id: item.id,
                        text: item.compnyName,
                    }))
                };
            }
        }
    });


    $('#ddlproject').select2({
        placeholder: 'Select Project',
        width: '100%',
        dropdownAutoWidth: true,
        allowClear: true,
        ajax: {
            url: '/Project/GetProjectNameList',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                return {
                    results: data.map(item => ({
                        id: item.id,
                        text: item.projectTitle,
                    }))
                };
            }
        }
    });

    $('#ddlCDVendorName').select2({
        placeholder: 'Select Vendor',
        width: '100%',
        dropdownAutoWidth: true,
        allowClear: true,
        ajax: {
            url: '/ProductMaster/GetVendorsNameList',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                return {
                    results: data.map(item => ({
                        id: item.id,
                        text: item.vendorCompany
                    }))
                };
            },
            error: function (xhr, status, error) {
                console.error("Error fetching vendor list:", error);
            }
        }
    });


    $('#ddlInvoicepaymenttype').select2({
        placeholder: 'Select Payment type',
        width: '100%',
        dropdownAutoWidth: true,
        allowClear: true,
        ajax: {
            url: '/ExpenseMaster/GetPaymentTypeList',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                return {
                    results: data.map(item => ({
                        id: item.id,
                        text: item.type
                    }))
                };
            },
            error: function (xhr, status, error) {
                console.error("Error fetching vendor list:", error);
            }
        }
    });

    $('#ddlpaymentType').select2({
        placeholder: 'Select Type',
        width: '100%',
        dropdownAutoWidth: true,
        allowClear: true,
        ajax: {
            url: '/ExpenseMaster/GetPaymentTypeList',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                return {
                    results: data.map(item => ({
                        id: item.id,
                        text: item.type
                    }))
                };
            },
            error: function (xhr, status, error) {
                console.error("Error fetching vendor list:", error);
            }
        }
    });

    getVendorTransactionList();

    $('#textTransactionCompanyName').on('change', SortCompanyName);

    var creditDebitDetails = JSON.parse(localStorage.getItem('creditDebitDetails'));
    var total = parseFloat(localStorage.getItem('totalCreditAmount'));

    if (creditDebitDetails) {
        var totalAmount = parseFloat($('#txttotalamount').val());
        var totalPendingAmount = totalAmount - total;

        if (!$("#txttotalcreditamount").text().trim()) {
            $("#txttotalcreditamount").text('₹' + total.toFixed(2));
        }

        if (!$("#txttotalpendingamount").text().trim()) {
            $("#txttotalpendingamount").text('₹' + totalPendingAmount.toFixed(2));
        }

        if (!$("#pendingamount").text().trim()) {
            $("#pendingamount").text('₹' + totalPendingAmount.toFixed(2));
        }

        $('#txtcreditdebitamount').off('input').on('input', function () {
            var enteredAmount = parseFloat($(this).val());

            if (!isNaN(enteredAmount)) {
                var pendingAmount = totalPendingAmount - enteredAmount;

                if (enteredAmount > totalPendingAmount) {
                    $('#warningMessage').text('Entered amount cannot exceed pending amount.');
                    $('#txtpendingamount').val('');
                } else {
                    $('#warningMessage').text('');
                    $('#txtpendingamount').val(pendingAmount.toFixed(2));
                }
            } else {
                $('#warningMessage').text('');
                $('#txtpendingamount').val('');
            }
        });
    }


    $('#searchcreditdebitlist').on('keyup', function () {
        var value = $(this).val().toLowerCase();
        var hasVisibleItems = false;

        $('#AllVendorCreditDebitList .transaction-item').filter(function () {
            var isVisible = $(this).text().toLowerCase().indexOf(value) > -1;
            $(this).toggle(isVisible);
            if (isVisible) {
                hasVisibleItems = true;
            }
        });

        if (hasVisibleItems) {
            $('.noresult').hide();
        } else {
            $('.noresult').show();
        }
    });
});

function ResetAllTransaction() {
    window.location = '/Invoice/AllTransaction';
}






let VendorListGridOptions;

$(document).ready(function () {

    const colorClasses = [
        { bgClass: 'bg-primary-subtle', textClass: 'text-primary' },
        { bgClass: 'bg-secondary-subtle', textClass: 'text-secondary' },
        { bgClass: 'bg-success-subtle', textClass: 'text-success' },
        { bgClass: 'bg-info-subtle', textClass: 'text-info' },
        { bgClass: 'bg-warning-subtle', textClass: 'text-warning' },
        { bgClass: 'bg-danger-subtle', textClass: 'text-danger' },
        { bgClass: 'bg-dark-subtle', textClass: 'text-dark' }
    ];

    VendorListGridOptions = {
        columnDefs: [
            {
                headerName: "Vendor Company", field: "vendorCompany", sortable: true, filter: true,
                cellRenderer: function (params) {
                    const full = params.data;
                    if (!full) return '';

                    let profileImageHtml;
                    if (full.vendorCompanyLogo && full.vendorCompanyLogo.trim() !== '') {
                        profileImageHtml = `<img src="/Content/Image/${full.vendorCompanyLogo}"
                style="height: 40px; width: 40px; border-radius: 50%;"
                onmouseover="showIcons(event, this.parentElement)"
                onmouseout="hideIcons(event, this.parentElement)">`;
                    } else {
                        const initials = full.vendorCompany ? full.vendorCompany[0] : '';
                        const randomColor = colorClasses[Math.floor(Math.random() * colorClasses.length)];
                        profileImageHtml = `
                <div class="flex-shrink-0 avatar-xs me-2">
                    <div class="avatar-title ${randomColor.bgClass} ${randomColor.textClass}
                        rounded-circle" style="height: 40px; width: 40px;">
                        ${initials.toUpperCase()}
                    </div>
                </div>`;
                    }

                    return `<a href="#" onclick="GetCreditDebitTotalAmount('${full.vid}')" class="link-primary" 
                    style="display: flex; align-items: center;">
                    ${profileImageHtml}<span style="margin-left: 5px;">${full.vendorCompany}</span>
                </a>`;
                }
            },
            {
                headerName: "Vendor Full Name", field: "gstno", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data) return '';
                    return `${params.data.vendorFirstName || ''} ${params.data.vendorLastName || ''}`;
                }
            },
            { headerName: "Email", field: "vendorEmail", sortable: true, filter: true },
            { headerName: "Phone", field: "vendorPhone", sortable: true, filter: true }
        ],
        defaultColDef: {
            sortable: true,
            filter: true,
            cellClass: 'ag-cell-default-style',
            resizable: true
        },
        rowSelection: 'single',
        rowClassRules: {
            'selected-row': params => params.node.isSelected()
        },
        onGridReady: function (params) {
            VendorListGridOptions.api = params.api;
            VendorListGridOptions.columnApi = params.columnApi;
            params.api.sizeColumnsToFit();
        },
        rowModelType: 'infinite',
        cacheBlockSize: 10,
        datasource: {
            getRows: function (params) {
                const request = {
                    StartRow: params.startRow,
                    PageSize: VendorListGridOptions.cacheBlockSize || 10,
                    SearchType: "",
                    SearchValue: "",
                    SortModel: params.sortModel || [],
                    SortColumn: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].colId : "",
                    SortDirection: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].sort : "",
                    filters: Object.entries(params.filterModel || {}).map(([key, value]) => ({
                        colId: key,
                        filterValue: value.filter
                    })),
                    searchValue: $('#txtCompanySearch').val(),
                };

                $.ajax({
                    url: '/Invoice/GetVendorList',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(request),
                    success: function (response) {
                        params.successCallback(response.rowsThisPage, response.totalRowCount);
                        $('#FooterTotalrecord').text(response.totalRowCount);
                    },
                    error: function () {
                        params.failCallback();
                    }
                });
            }
        }
    };


    const userPermissionArray = Formdata || [];
    let canEdit = false;
    let canDelete = false;

    for (let permission of userPermissionArray) {
        if (permission.formName === "Vendor") {
            canEdit = permission.edit;
            canDelete = permission.delete;
            break;
        }
    }

    if (canEdit || canDelete) {
        VendorListGridOptions.columnDefs.push({
            headerName: "Actions",
            field: "actions",
            sortable: false,
            filter: false,
            cellRenderer: function (params) {
                if (!params.data || !params.data.id) return '';
                let buttons = '';
                if (canEdit) {
                    buttons += `<a title="Edit" onclick="EditCompanyDetails('${params.data.id}')" aria-label="Edit">
                        <i class="fa-solid fa-pen-to-square"></i></a>`;
                }
                if (canDelete) {
                    buttons += `<a href="javascript:;" onclick="DeleteCompanyDetails('${params.data.id}')" 
                                title="Delete" aria-label="Delete" style="margin-left:15px;">
                                <i class="fa-solid fa-trash"></i></a>`;
                }
                return buttons;
            }
        });
    }

    const gridElement = document.querySelector('#VendorTableData');
    agGrid.createGrid(gridElement, VendorListGridOptions);


});

let vendorAllTranGridOptions = {};
let startDate = null;
let endDate = null;

$(document).ready(function () {

    initializeVendorTransactionGrid();
    function initializeVendorTransactionGrid() {
        const columnDefs = [
            {
                headerName: "",
                field: "type",
                cellRenderer: function (params) {

                    const type = params.data?.type || '';
                    if (type.toLowerCase() === "credit") {
                        return `<div class="agavatar-title bg-success-subtle text-success agrounded-circle agfs-16">
                                <i class="ri-arrow-left-down-fill"></i>
                            </div>`;
                    } else {

                        return `<div class="agavatar-title bg-danger-subtle text-danger agrounded-circle agfs-16">
                                <i class="ri-arrow-right-up-fill"></i>
                            </div>`;
                    }
                }
            },
            {
                headerName: "Vendor Name",
                field: "vendorCompany",
                sortable: true,
                filter: true
            },
            {
                headerName: "Date",
                field: "date",
                sortable: true,
                filter: true,
                cellRenderer: function (params) {
                    return params.value ? getCommonDateformat(params.value) : '';
                }
            },
            {
                headerName: "Project",
                field: "project",
                sortable: true,
                filter: true
            },
            {
                headerName: "Credit/Debit Amount",
                field: "creditDebitAmount",
                sortable: true,
                filter: true,
                cellRenderer: function (params) {
                    const type = params.data?.type || '';
                    const amount = params.value || '';

                    if (type.toLowerCase() === "credit") {
                        return `<h6 class="text-success mb-1 amount">${amount}</h6>`;
                    } else {
                        return `<h6 class="text-danger mb-1 amount">${amount}</h6>`;
                    }
                }
            },
            {
                headerName: "Staus",
                field: "paymentTypeName",
                sortable: true,
                filter: true,
                cellRenderer: function (params) {
                    const paymentType = params.value ? params.value.toLowerCase() : '';
                    let badgeClass = '';
                    let textColor = '';
                    let icon = '';
                    let text = params.value || '';

                    switch (paymentType) {
                        case 'paid':
                            badgeClass = 'bg-success-subtle';
                            textColor = 'text-success';
                            icon = '<i class="ri-checkbox-circle-line align-bottom"></i>';
                            break;
                        case 'cancel':
                            badgeClass = 'bg-danger-subtle';
                            textColor = 'text-danger';
                            icon = '<i class="ri-close-circle-line align-bottom"></i>';
                            break;
                        case 'unpaid':
                            badgeClass = 'bg-primary-subtle';
                            textColor = 'text-warning';
                            icon = '<i class="ri-time-line align-bottom"></i>';
                            break;
                        case 'refund':
                            badgeClass = 'bg-primary-subtle';
                            textColor = 'text-primary';
                            icon = '<i class="ri-time-line align-bottom"></i>';
                            break;
                        default:
                            badgeClass = 'bg-secondary-subtle';
                            textColor = 'text-secondary';
                            icon = '<i class="ri-question-line align-bottom"></i>';
                    }

                    return `<span class="badge ${badgeClass} ${textColor} fs-11">${icon} ${text}</span>`;
                }
            },
        ];

        let canEdit = false;
        let canDelete = false;

        for (let i = 0; i < Formdata.length; i++) {
            const permission = Formdata[i];
            if (permission.formName === "Vendor List") {
                canEdit = permission.edit;
                canDelete = permission.delete;
                break;
            }
        }

        if (canEdit || canDelete) {
            columnDefs.push({
                headerName: "Actions",
                field: "actions",
                sortable: false,
                filter: false,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';

                    let buttons = '<ul class="list-inline mb-0">';
                    if (canEdit) {
                        buttons += `<li class="list-inline-item">
                                    <a href="/PurchaseOrderMaster/CreatePurchaseOrder?id=${params.data.id}">
                                        <i class="fa-regular fa-pen-to-square"></i>
                                    </a>
                                </li>`;
                    }
                    if (canDelete) {
                        buttons += `<li class="list-inline-item">
                                    <a class="btn text-danger" onclick="DeleteTransaction('${params.data.id}')">
                                        <i class="fas fa-trash"></i>
                                    </a>
                                </li>`;
                    }
                    buttons += '</ul>';
                    return buttons;
                }
            });
        }

        vendorAllTranGridOptions = {
            rowHeight: 50,
            columnDefs,
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
                vendorAllTranGridOptions.api = params.api;
                vendorAllTranGridOptions.columnApi = params.columnApi;
                vendorAllTranGridOptions.api.sizeColumnsToFit();
                createEnhancedPagination(params.api);
            },
            rowModelType: 'infinite',
            cacheBlockSize: 20,
            pagination: true,
            paginationPageSize: 20,
            suppressPaginationPanel: true,
            datasource: getVendorTransactionDatasource()
        };
    }

    function getVendorTransactionDatasource() {
        return {
            getRows: function (params) {
                const request = {
                    StartRow: params.startRow,
                    PageSize: vendorAllTranGridOptions.cacheBlockSize || 10,
                    SearchType: "",
                    SearchValue: "",
                    SortModel: params.sortModel || [],
                    SortColumn: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].colId : "",
                    SortDirection: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].sort : "",
                    filters: Object.entries(params.filterModel || {}).map(([key, value]) => ({
                        colId: key,
                        filterValue: value.filter
                    })),
                    SearchValue: $('#txttransactionSearch').val() || "",
                    CompanyFilter: $('#ddlCDCompanyName').val() || null,
                    PaymentType: $('#ddlpaymentType').val() || null,
                    VendorFilter: $('#ddlCDVendorName').val() || null,
                    StartDate: startDate,
                    EndDate: endDate

                };

                $.ajax({
                    url: '/Invoice/AllVendorTransaction',
                    method: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(request),
                    success: function (response) {

                        if (response && Array.isArray(response.rowsThisPage)) {
                            params.successCallback(response.rowsThisPage, response.totalCount);
                        } else {
                            params.failCallback();
                        }
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

        const gridElement = document.querySelector('#vendorAllTransaction');
        if (gridElement) {
            const paginationEl = document.createElement('div');
            paginationEl.className = 'ag-paging-panel enhanced';
            paginationEl.appendChild(paginationContainer);
            gridElement.appendChild(paginationEl);
        }

        const pageSizeSelector = pageSizeContainer.querySelector('.page-size-selector');
        if (pageSizeSelector) {
            pageSizeSelector.addEventListener('change', function () {
                const newPageSize = Number(this.value);
                const gridElement = document.querySelector('#vendorAllTransaction');

                if (gridElement) {
                    vendorAllTranGridOptions = {
                        ...vendorAllTranGridOptions,
                        cacheBlockSize: newPageSize,
                        paginationPageSize: newPageSize,
                        datasource: getAllVendorTransactions(),
                    };

                    gridElement.innerHTML = '';
                    agGrid.createGrid(gridElement, vendorAllTranGridOptions);
                }
            });
        }

        updateEnhancedPagination(gridApi, rangeDisplay);
    }

    function updateEnhancedPagination(gridApi, rangeDisplay) {
        if (!gridApi) return;

        const currentPage = gridApi.paginationGetCurrentPage() + 1;
        const totalPages = gridApi.paginationGetTotalPages();
        const totalRows = gridApi.paginationGetRowCount();
        const pageSize = vendorAllTranGridOptions.paginationPageSize;

        const startRow = totalRows > 0 ? ((currentPage - 1) * pageSize + 1) : 0;
        const endRow = totalRows > 0 ? Math.min(currentPage * pageSize, totalRows) : 0;

        if (rangeDisplay) {
            rangeDisplay.textContent = totalRows > 0 ? `${startRow} to ${endRow} of ${totalRows}` : '0 to 0 of 0';
        }

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
        if (prevButton) prevButton.disabled = currentPage === 1;
        if (nextButton) nextButton.disabled = currentPage === totalPages || totalPages === 0;

        const pageSizeSelector = document.querySelector('.page-size-selector');
        if (pageSizeSelector) pageSizeSelector.value = pageSize;
    }


    const gridElement = document.querySelector('#vendorAllTransaction');
    agGrid.createGrid(gridElement, vendorAllTranGridOptions);


    $('#btntranssearch').on('click', function () {
        if (vendorAllTranGridOptions.api) {
            vendorAllTranGridOptions.api.refreshInfiniteCache();
        }
    });

    $('#txttransactionSearch').on('keypress', function (e) {
        if (e.which === 13) {
            $('#btntranssearch').click();
        }
    });

    $('#txttransactionSearch').on('input', function () {
        var searchText = $(this).val().trim();

        if (searchText === '') {
            if (vendorAllTranGridOptions.api) {
                vendorAllTranGridOptions.api.refreshInfiniteCache();
            }
        }
    });


    $('#ddlCDVendorName').change(() => {
        if (vendorAllTranGridOptions.api) {
            vendorAllTranGridOptions.api.refreshInfiniteCache();
        }
    });

    $('#ddlpaymentType').change(() => {
        if (vendorAllTranGridOptions.api) {
            vendorAllTranGridOptions.api.refreshInfiniteCache();
        }
    });

    $('#toggletraDateFilter').click(e => {
        e.stopPropagation();
        $('#dateFilterContainer').toggle();
    });

    $('#applyFilters').click(() => {
        startDate = $('#txtstartdatebox').val() || null;
        endDate = $('#txtenddatebox').val() || null;

        if (startDate && endDate && new Date(startDate) > new Date(endDate)) {
            alert('End date must be after start date');
            return;
        }

        if (vendorAllTranGridOptions.api) {
            vendorAllTranGridOptions.api.refreshInfiniteCache();
        }
        $('#dateFilterContainer').hide();
    });

    $("#resetPendingDateFilters").click(function () {
        $("#fromDate").val('');
        $("#toDate").val('');
        startDate = null;
        endDate = null;
        vendorAllTranGridOptions.api.setFilterModel(null);
        vendorAllTranGridOptions.api.onFilterChanged();
        $('#dateFilterContainer').hide();
    });

});

function InsertCreditDebitDetails() {

    var value = $('#txtcreditdebitamount').val();
    if (value.trim() === '') {

        $('#warningMessage').text('Please enter value!!');
        toastr.warning("Kindly fill all datafield");
    }
    else {
        var VendorId = document.getElementById("txtvendorid").textContent;

        var objData = {
            VendorId: document.getElementById("txtvendorid").textContent,
            InvoiceNo: document.getElementById("txtinvoiceno").textContent,
            CompanyId: $("#ddlCompanyName").val(),
            Type: $("#ddltypeonly").val(),
            ProjectId: $("#ddlproject").val(),
            PaymentType: $("#drpcreditdebitpaymenttype").val(),
            PaymentMethod: $("#drpcreditdebitpaymentmethod").val(),
            CreditDebitAmount: $("#txtcreditdebitamount").val(),
            PendingAmount: $("#txtpendingamount").val(),
            TotalAmount: $("#txttotalamount").val(),
            CreatedBy: $("#txtuserid").val(),
            Date: $("#txtpaydatevendor").val(),
        };

        var form_data = new FormData();
        form_data.append("CREDITDEBITDETAILS", JSON.stringify(objData));

        $.ajax({
            url: '/Invoice/InsertCreditDebitDetails',
            type: 'POST',
            data: form_data,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.code == 200) {
                    Swal.fire({
                        title: result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/Invoice/PayVendors?Vid=' + VendorId;
                        GetCreditDebitTotalAmount(VendorId);
                    });
                }
                else {
                    toastr.error(Result.message);
                }
            },
            error: function () {
                toastr.error('An error occurred while processing your request.');
            }
        });
    }
}
function GetCreditDebitTotalAmount(Vid) {
    $.ajax({
        url: '/Invoice/GetCreditDebitDetailsByVendorId?VendorId=' + Vid,
        type: 'POST',
        dataType: 'json',
        success: function (result) {

            var total = 0;
            result.forEach(function (obj) {
                if (obj.creditDebitAmount) {
                    total += obj.creditDebitAmount;
                }
            });

            localStorage.setItem('creditDebitDetails', JSON.stringify(result));
            localStorage.setItem('totalCreditAmount', total);
            window.location = '/Invoice/PayVendors?Vid=' + Vid;

        },
        error: function (xhr, status, error) {
            toastr.error("Error in AJAX request:", status, error);
        }
    });
}
function getLastTransaction(Vid) {

    $.ajax({
        url: '/Invoice/GetLastTransactionByVendorId',
        type: 'GET',
        dataType: 'html',
        data: { Vid: Vid },
        success: function (response) {
            $("#lasttenTransaction").html(response);
            $("#zoomInModal").modal('show');
        },
    });
}
function DeleteTransaction(Transaction, VendorId, that) {
    Swal.fire({
        title: "Are you sure you want to delete this?",
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
                url: '/Invoice/DeleteTransaction?Id=' + VendorId,
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
                            if (Transaction == "AllTransaction") {
                                window.location = '/Invoice/AllTransaction';
                            } else if (Transaction == "VendorTransactions") {
                                window.location = '/Invoice/VendorAllTransaction?Vid=' + Vid;
                            }
                        });
                    } else {
                        toastr.error(Result.message);
                    }
                },
                error: function () {
                    Swal.fire({
                        title: "Can't delete transaction!",
                        icon: 'warning',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        if (Transaction == "AllTransaction") {
                            window.location = '/Invoice/AllTransaction';
                        } else if (Transaction == "VendorTransactions") {
                            window.location = '/Invoice/VendorAllTransaction?Vid=' + Vid;
                        }
                    });
                }
            });
        } else if (result.dismiss === Swal.DismissReason.cancel) {
            Swal.fire(
                'Cancelled',
                'Transactions have no changes.!!😊',
                'error'
            );
        }
    });
}
