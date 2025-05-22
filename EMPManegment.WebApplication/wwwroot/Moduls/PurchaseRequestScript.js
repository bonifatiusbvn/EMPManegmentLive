var datas = userPermissions
$(document).ready(function () {
    fn_updatePRProductAmount();
    GetPurchaseRequestList();
    fn_updatePRTotals();
    CountCartTotalItems();
    $(document).on('input', '.product-quantity', function () {
        fn_updatePRProductAmount()
    });

    $(document).on('keydown', '.product-quantity', function (event) {
        if (event.key === 'Enter') {
            $(this).blur();
        }
    });

    $(document).on('keydown', '#txtproductamount', function (event) {
        if (event.key === 'Enter') {
            $(this).blur();
        }
    });

    $(document).on('focusout', '.product-quantity', function () {
        $(this).trigger('input');
    });
});

var Formdata = window.userFormPermissions || 0;
let PurchaseRequestGridOptions = [];

$(document).ready(function () {
    PurchaseRequestGridOptions = {
        rowHeight: 50,
        columnDefs: [
            {
                headerName: "PR No.", field: "prNo", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.prId) return '';
                    return '<h5 class="fs-15"><a href="/PurchaseRequest/PurchaseRequestDetails?prNo=' + params.data.prNo + '" style="color: #16989A !important;" >' + params.data.prNo + '</a></h5>';

                }
            },
            {
                headerName: "User Name", field: "firstName", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.prId) return '';
                    return params.data.firstName + ' ' + params.data.lastName + ' ( ' + params.data.userName + ' )';

                }
            },
            { headerName: "Project Name", field: "projectName", sortable: true, filter: true },
            { headerName: "Product Name", field: "productName", sortable: true, filter: true },
            { headerName: "Quantity", field: "quantity", sortable: true, filter: true },
            {
                headerName: "Approve",
                field: "isApproved",
                sortable: true,
                filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.prId) return '';

                    const isChecked = params.data.isApproved;
                    const checkboxId = 'chk_child_' + params.data.prId;

                    return `
                        <div class="custom-control custom-checkbox">
                        <input type="checkbox"
                        class="custom-control-input custom-control-input-teal"
                        id="${checkboxId}"
                        data-id="${params.data.prId}"
                        data-approved="${isChecked}"
                        ${isChecked ? 'checked' : ''}>
                        <label class="custom-control-label" for="${checkboxId}" style="margin-top: 11px;"></label>
                        </div>
                `;
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
            PurchaseRequestGridOptions.api = params.api;
            PurchaseRequestGridOptions.columnApi = params.columnApi;
            PurchaseRequestGridOptions.api.sizeColumnsToFit();
            createEnhancedPagination(params.api);
        },
        rowModelType: 'infinite',
        cacheBlockSize: 20,
        pagination: true,
        paginationPageSize: 20,
        suppressPaginationPanel: true,
        datasource: getPRDatasource()
    };

    function getPRDatasource() {
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
                    searchValue: $('#txtPurchaseRequestSearch').val(),
                };

                $.ajax({
                    url: '/PurchaseRequest/GetPRList',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(request),
                    success: function (response) {
                        params.successCallback(response.rowsThisPage, response.totalRowCount);
                        const rangeDisplay = document.querySelector('#PurchaseRequestTable .range-display');
                        if (rangeDisplay) updateEnhancedPagination(PurchaseRequestGridOptions.api, rangeDisplay);
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

        const eGui = document.querySelector('#PurchaseRequestTable');
        const paginationEl = document.createElement('div');
        paginationEl.className = 'ag-paging-panel enhanced';
        paginationEl.appendChild(paginationContainer);
        eGui.appendChild(paginationEl);

        const pageSizeSelector = pageSizeContainer.querySelector('.page-size-selector');
        pageSizeSelector.addEventListener('change', function () {
            const newPageSize = Number(this.value);

            // Destroy and recreate grid with new block size
            const gridDiv = document.querySelector('#PurchaseRequestTable');

            PurchaseRequestGridOptions = {
                ...PurchaseRequestGridOptions,
                cacheBlockSize: newPageSize,
                paginationPageSize: newPageSize,
                datasource: getPRDatasource(),
            };

            // Clear old grid and re-init
            gridDiv.innerHTML = '';
            agGrid.createGrid(gridDiv, PurchaseRequestGridOptions);
        });

        updateEnhancedPagination(gridApi, rangeDisplay);
    }

    function updateEnhancedPagination(gridApi, rangeDisplay) {
        const currentPage = gridApi.paginationGetCurrentPage() + 1;
        const totalPages = gridApi.paginationGetTotalPages();
        const totalRows = gridApi.paginationGetRowCount();
        const pageSize = PurchaseRequestGridOptions.paginationPageSize;

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
        if (userFormPermissionArray[i].formName === "Create Purchase Request") {
            canEdit = userFormPermissionArray[i].edit;
            canDelete = userFormPermissionArray[i].delete;
            break;
        }
    }

    if (canEdit || canDelete) {
        PurchaseRequestGridOptions.columnDefs.push({
            headerName: "Action",
            field: "actions",
            sortable: false,
            filter: false,
            cellRenderer: function (params) {
                if (!params.data || !params.data.prId) return '';

                let buttons = '';
                if (canEdit) {
                    buttons += `
                    <li class="list-inline-item">
                        <a href="/PurchaseRequest/CreatePurchaseRequest?id=${params.data.prNo}">
                            <i class="fa-regular fa-pen-to-square"></i>
                        </a>
                    </li>`;
                }
                if (canDelete) {
                    buttons += `
                    <li class="list-inline-item">
                        <a onclick="DeletePurchaseRequest('${params.data.prNo}')">
                            <i class="fas fa-trash"></i>
                        </a>
                    </li>`;
                }
                return buttons;
            }
        });
    }

    const myGridElement = document.querySelector('#PurchaseRequestTable');
    agGrid.createGrid(myGridElement, PurchaseRequestGridOptions);

    $('#txtPurchaseRequestSearch').on('change keyup', function () {
        PurchaseRequestGridOptions.api.onFilterChanged();
    });
});

function fn_SearchItemDetailsById(Id) {
    $.ajax({
        url: '/PurchaseRequest/DisplayProductDetailsListById?ProductId=' + Id,
        type: 'Post',
        datatype: 'json',
        processData: false,
        contentType: false,
        complete: function (Result) {
            fn_AddNewPRRow(Result.responseText);
        }
    });
}

var count = 0;
function fn_AddNewPRRow(Result) {
    var newProductRow = $(Result);
    var newProductId = newProductRow.find('.card-body').data('product-id');
    var isDuplicate = false;

    $('#displayPurchaseRequest .products .card-body').each(function () {
        var existingProductRow = $(this);
        var existingProductId = existingProductRow.data('product-id');
        if (existingProductId === newProductId) {
            isDuplicate = true;
            return false;
        }
    });

    if (!isDuplicate) {
        count++;
        $("#displayPurchaseRequest").append(Result);
        fn_updatePRProductAmount();
        fn_updatePRTotals();
        showHidePRCreatebtn();
        CountCartTotalItems();
    } else {
        toastr.warning("Product already added!");
    }
}
function fn_updatePRProductAmount() {

    $(".products").each(function () {
        var row = $(this);
        var subtotal = parseFloat(row.find("#dspperunitprice").text().replace('₹', ''));
        var totalquantity = parseFloat(row.find("#txtproductquantity").val());
        var gstAmount = parseFloat(row.find("#txtProductGstAmount").text().replace('₹', ''));
        var totalGstamount = gstAmount * totalquantity;
        var Amount = subtotal * totalquantity;
        var TotalAmount = totalGstamount + Amount;

         var formattedTotalAmount = TotalAmount.toLocaleString('en-IN', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
        row.find("#dsptotalAmount").text('₹' + formattedTotalAmount);

    });
    fn_updatePRTotals();
}
function fn_updatePRTotals() {

    var cartSubtotal = 0;
    var cartGst = 0;
    var cartTotalamount = 0;

    $(".products").each(function () {
        var row = $(this);

        var subtotal = parseFloat(row.find("#dspperunitprice").text().replace('₹', ''));
        var gst = parseFloat(row.find("#txtProductGstAmount").text().replace('₹', ''));
        var quantity = parseFloat(row.find("#txtproductquantity").val());
        var totalGstamount = gst * quantity;
        var Amount = subtotal * quantity;

        cartSubtotal += Amount;
        cartGst += totalGstamount;
        cartTotalamount = cartSubtotal + cartGst;
    });

    $("#cart-subtotal").html('₹' + cartSubtotal.toFixed(2));
    $("#cart-Gst").html('₹' + cartGst.toFixed(2));
    $("#cart-total").html('₹' + cartTotalamount.toFixed(2));
}
function CountCartTotalItems() {
    var totalItems = $('.products').length;
    $("#cartTotalItemCount").html('Your Select ' + '(' + totalItems + ' ' + 'items)');
};
function removeProduct(btn) {
    $(btn).closest(".products").remove();
    fn_updatePRProductAmount();
    fn_updatePRTotals();
    CountCartTotalItems();
    showHidePRCreatebtn();
}
function showHidePRCreatebtn() {
    var totalAmount = $("#dsptotalAmount").text();
    if (totalAmount != "") {
        $("#btnpurchaserequest").show();
    } else {
        $("#btnpurchaserequest").hide();
    }
}
function preventEmptyValue(input) {

    if (input.value === "") {

        input.value = 1;
    }
}
function ApproveUnapprovePR() {
    Swal.fire({
        title: "Are you sure you want to approve this purchase request?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, enter it!",
        cancelButtonText: "No, cancel!",
        confirmButtonClass: "btn btn-primary w-xs me-2 mt-2",
        cancelButtonClass: "btn btn-danger w-xs mt-2",
        buttonsStyling: false,
        showCloseButton: true
    }).then((result) => {
        if (result.isConfirmed) {
            let selectedIds = [];
            $("input[name=chk_child]").each(function () {
                selectedIds.push({
                    PrId: $(this).attr("data-id"),
                    IsApproved: $(this).is(":checked")
                });
            });


            var PRDetails = {
                PRList: selectedIds
            };

            var form_data = new FormData();
            form_data.append("PRIsApproved", JSON.stringify(PRDetails));

            $.ajax({
                url: '/PurchaseRequest/ApproveUnapprovePR',
                type: 'POST',
                processData: false,
                contentType: false,
                data: form_data,
                success: function (Result) {
                    if (Result.code == 200) {
                        Swal.fire({
                            title: Result.message,
                            icon: "success",
                            confirmButtonClass: "btn btn-primary w-xs mt-2",
                            buttonsStyling: false
                        }).then(function () {
                            window.location = '/PurchaseRequest/PurchaseRequests';
                        });
                    } else {
                        toastr.error(Result.message);
                    }
                }
            });
        } else if (result.dismiss === Swal.DismissReason.cancel) {
            Swal.fire(
                'Cancelled',
                'User has no changes.😊',
                'error'
            ).then(function () {
                window.location = '/PurchaseRequest/PurchaseRequests';
            });
        }
    });
}

function GetPurchaseRequestList() {
    $.ajax({
        url: '/PurchaseRequest/GetPurchaseRequestList',
        type: 'Post',
        dataType: 'json',
        processData: false,
        contentType: false,
        complete: function (result) {
            $('#addNewlink').html(result.responseText);
        },
    })
}
function CreatePurchaseRequest() {
    var TotalAmount = $("#dsptotalAmount").text();
    if (TotalAmount != "") {
        var purchaseRequests = [];
        $(".products").each(function () {
            var orderRow = $(this);
            var objData = {
                UserId: orderRow.find("#txtuserId").val(),
                ProjectId: orderRow.find("#txtprojectId").val(),
                ProductId: orderRow.find("#txtproductId").val(),
                ProductName: orderRow.find("#txtProductName").val(),
                ProductTypeId: orderRow.find("#txtproducttype").val(),
                Quantity: orderRow.find("#txtproductquantity").val(),
                CreatedBy: $('#txtuserId').val(),
                PrNo: $('#prNo').val(),
                PrDate: $('#txtPrDate').val(),
            };
            purchaseRequests.push(objData);
        });

        var data = {
            PRList: purchaseRequests,
        }

        var form_data = new FormData();
        form_data.append("InsertPRDetails", JSON.stringify(data));

        $.ajax({
            url: '/PurchaseRequest/CreateMutiplePurchaseRequest',
            type: 'POST',
            data: form_data,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (Result) {
                if (Result.code == 200) {
                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/PurchaseRequest/PurchaseRequests';
                    });
                }
                else {
                    toastr.error(Result.message);
                }
            },
            error: function () {
                toastr.error("An error occurred while creating the purchase request.");
            }
        });
    } else {
        toastr.warning("Kindly add the products");
    }
}
function UpdatePurchaseRequestDetails() {

    var TotalAmount = $("#dsptotalAmount").text();
    if (TotalAmount != "") {
        var purchaseRequests = [];
        $(".products").each(function () {
            var orderRow = $(this);
            var objData = {
                UserId: orderRow.find("#txtuserId").val(),
                ProjectId: $("#textModelProjectIdPR").val(),
                ProductId: orderRow.find("#txtproductId").val(),
                ProductName: orderRow.find("#txtProductName").val(),
                ProductTypeId: orderRow.find("#txtproducttype").val(),
                Quantity: orderRow.find("#txtproductquantity").val(),
                UpdatedBy: $('#txtuserId').val(),
                PrNo: $('#prNo').val(),
                PrDate: $('#txtUpdatePrDate').val(),
            };
            purchaseRequests.push(objData);
        });

        var data = {
            PRList: purchaseRequests,
            PrNo: $('#prNo').val(),
        }

        var form_data = new FormData();
        form_data.append("UpdatePRDetails", JSON.stringify(data));

        $.ajax({
            url: '/PurchaseRequest/UpdatePurchaseRequestDetails',
            type: 'Post',
            data: form_data,
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
                        window.location = '/PurchaseRequest/PurchaseRequests';
                    });
                }
                else {
                    toastr.error(Result.message);
                }
            }
        })
    }
    else {
        toastr.warning("Please Add Product!");
    }

}
function DeletePurchaseRequest(PrNo) {

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
                url: '/PurchaseRequest/DeletePurchaseRequest?PrNo=' + PrNo,
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
                            GetPRData();
                        })
                    }
                    else {
                        toastr.error(Result.message);
                    }
                },
                error: function () {
                    Swal.fire({
                        title: "Can't delete order!",
                        icon: 'warning',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        GetPRData();
                    })
                }
            })
        } else if (result.dismiss === Swal.DismissReason.cancel) {

            Swal.fire(
                'Cancelled',
                'Order have no changes.!!😊',
                'error'
            );
        }
    });
}
function createPR() {
    if ($("#txtProjectId").val() == "") {
        Swal.fire({
            title: "Kindly select project on dashboard.",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        });
    }
    else {
        window.location = '/PurchaseRequest/CreatePurchaseRequest';
    }
}






