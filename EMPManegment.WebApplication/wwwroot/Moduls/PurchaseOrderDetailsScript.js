var Formdata = window.userFormPermissions || 0;

let PurchaseOrderGridOptions = [];
let startDate = null;
let endDate = null;

$(document).ready(function () {
    PurchaseOrderGridOptions = {
        rowHeight: 50,
        columnDefs: [
            {
                headerName: "Order Id", field: "firstName", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';
                    return `<a href="/PurchaseOrderMaster/PurchaseOrderDetails/?OrderId=${params.data.orderId}"><span style="color: #16989A !important;">` + params.data.orderId + `</span></a>`;
                }
            },
            { headerName: "Company Name", field: "companyName", sortable: true, filter: true },
            {
                headerName: "Date", field: "date", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';
                    return getCommonDateformat(params.data.orderDate);
                }
            },
            {
                headerName: "Total amount", field: "totalamount", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';
                    return '₹' + params.data.totalAmount;
                }
            },
            { headerName: "Payment Method", field: "paymentMethodName", sortable: true, filter: true },
            { headerName: "Delivery Status", field: "deliveryStatus", sortable: true, filter: true },
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
            PurchaseOrderGridOptions.api = params.api;
            PurchaseOrderGridOptions.columnApi = params.columnApi;
            PurchaseOrderGridOptions.api.sizeColumnsToFit();
            createEnhancedPagination(params.api);
        },
        rowModelType: 'infinite',
        cacheBlockSize: 20,
        pagination: true,
        paginationPageSize: 20,
        suppressPaginationPanel: true,
        datasource: getPurchaseOrderDatasource()
    };

    function getPurchaseOrderDatasource() {
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
                    searchValue: $('#txtPurchaseOrderSearch').val(),
                    ComapnyFilter: $('#txtPOCompanyName').val(),
                    StartDate: startDate,
                    EndDate: endDate,
                };

                $.ajax({
                    url: '/PurchaseOrderMaster/GetPurchaseOrderList',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(request),
                    success: function (response) {
                        params.successCallback(response.rowsThisPage, response.totalRowCount);
                        const rangeDisplay = document.querySelector('#PurchaseOrderTable .range-display');
                        if (rangeDisplay) updateEnhancedPagination(PurchaseOrderGridOptions.api, rangeDisplay);
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

        const eGui = document.querySelector('#PurchaseOrderTable');
        const paginationEl = document.createElement('div');
        paginationEl.className = 'ag-paging-panel enhanced';
        paginationEl.appendChild(paginationContainer);
        eGui.appendChild(paginationEl);

        const pageSizeSelector = pageSizeContainer.querySelector('.page-size-selector');
        pageSizeSelector.addEventListener('change', function () {
            const newPageSize = Number(this.value);

            // Destroy and recreate grid with new block size
            const gridDiv = document.querySelector('#PurchaseOrderTable');

            PurchaseOrderGridOptions = {
                ...PurchaseOrderGridOptions,
                cacheBlockSize: newPageSize,
                paginationPageSize: newPageSize,
                datasource: getPurchaseOrderDatasource(),
            };

            // Clear old grid and re-init
            gridDiv.innerHTML = '';
            agGrid.createGrid(gridDiv, PurchaseOrderGridOptions);
        });

        updateEnhancedPagination(gridApi, rangeDisplay);
    }

    function updateEnhancedPagination(gridApi, rangeDisplay) {
        const currentPage = gridApi.paginationGetCurrentPage() + 1;
        const totalPages = gridApi.paginationGetTotalPages();
        const totalRows = gridApi.paginationGetRowCount();
        const pageSize = PurchaseOrderGridOptions.paginationPageSize;

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
        if (userFormPermissionArray[i].formName === "Create Purchase Order") {
            canEdit = userFormPermissionArray[i].edit;
            canDelete = userFormPermissionArray[i].delete;
            break;
        }
    }

    if (canEdit || canDelete) {
        PurchaseOrderGridOptions.columnDefs.push({
            headerName: "Action",
            field: "actions",
            sortable: false,
            filter: false,
            cellRenderer: function (params) {
                if (!params.data || !params.data.id) return '';
                let buttons = '';
                if (canEdit) {
                    buttons += `
                         <li class="list-inline-item"><a href="/PurchaseOrderMaster/CreatePurchaseOrder?id=${params.data.id}"><i class="fa-regular fa-pen-to-square"></i></a></li>`;
                }

                if (canDelete) {
                    buttons += `
                    <a class="btn text-danger" onclick="deletePurchaseOrderDetails('${params.data.id}')"><i class="fas fa-trash"></i></a>`;
                }
                return buttons;
            }
        });
    }

    const myGridElement = document.querySelector('#PurchaseOrderTable');
    agGrid.createGrid(myGridElement, PurchaseOrderGridOptions);

    $('#txtPurchaseOrderSearch').on('change keyup', function () {
        PurchaseOrderGridOptions.api.onFilterChanged();
    });

    $('#txtPOCompanyName').change(() => {
        const companyText = $("#txtPOCompanyName option:selected").text();
        $("#txtCompanyName").val(companyText === 'All Company' ? '' : companyText);
        if (PurchaseOrderGridOptions.api) {
            PurchaseOrderGridOptions.api.onFilterChanged();
        }
    });

    $('#toggleDateFilter').click(e => {
        e.stopPropagation();
        $('#dateFilterContainer').toggle();
    });

    $('#applyFilters').click(() => {
        startDate = $('#txtstartdatebox').val() || null;
        endDate = $('#txtenddatebox').val() || null;
        if (PurchaseOrderGridOptions.api) {
            PurchaseOrderGridOptions.api.onFilterChanged();
        }
    });

    $('#txtPOCompanyName').select2({
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

    $('#txtPOpaymentmethod').select2({
        placeholder: 'Select Payment Method',
        width: '100%',
        dropdownAutoWidth: true,
        allowClear: true,
        ajax: {
            url: '/PurchaseOrderMaster/GetPaymentMethodList',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                return {
                    results: data.map(item => ({
                        id: item.id,
                        text: item.paymentMethod
                    }))
                };
            },
            error: function (xhr, status, error) {
                console.error("Error fetching vendor list:", error);
            }
        }
    });

    $('#txtPOpaymenttype').select2({
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
});

function ResetPurchaseOrderData() {
    $('#txtPOCompanyName').empty();
    $('#txtstartdatebox').val('');
    $('#txtenddatebox').val('');
    startDate = null;
    endDate = null;
    $('#dateFilterContainer').hide();
    PurchaseOrderGridOptions.api.setFilterModel(null);
    PurchaseOrderGridOptions.api.onFilterChanged();
}

$(document).ready(function () {

    fn_GetPOVendorNameList();
    fn_GetPOCompanyNameList();
    fn_updatePOTotals();
    $('#txtvendorname').change(function () {
        var Text = $("#txtvendorname Option:Selected").text();
        var ProductId = $(this).val();
        $("#txtvendornameid").val(Text);
        $('#productname').empty();
        $('#productname').append('<Option >--Select Product--</Option>');
        $.ajax({
            url: '/ProductMaster/GetProductById?ProductId=' + ProductId,
            success: function (result) {

                $.each(result, function (i, data) {
                    $('#productname').append('<Option value=' + data.id + '>' + data.productType + '</Option>')
                });
            }
        });
    });
    $("#CreatePOForm").validate({
        rules: {
            textVendorName: "required",
            textCompanyName: "required",
            txtPOpaymentmethod: "required",
            textDescription: "required",
            textDeliveryStatus: "required",
        },
        highlight: function (element) {
            if (element.id === "txtPOpaymentmethod" || element.id === "textDeliveryStatus") {
                $(element).addClass('is-invalid');
            }
        },
        unhighlight: function (element) {
            if (element.id === "txtPOpaymentmethod" || element.id === "textDeliveryStatus") {
                $(element).removeClass('is-invalid');
            }
        },
        errorPlacement: function (error, element) {
            if (element.attr("name") === "textVendorName" ||
                element.attr("name") === "textCompanyName" ||
                element.attr("name") === "textDescription") {
                error.insertAfter(element);
            }
        },
        messages: {
            textVendorName: "Select Vendor Name",
            textCompanyName: "Select Company Name",
            txtPOpaymentmethod: "",
            textDescription: "Please Enter Description",
            textDeliveryStatus: "",
        }
    });



    $("#UpdateOrderDetailsForm").validate({
        rules: {
            txtorderdate: "required",
            txtcompanyname: "required",
            txtorderdetails: "required",
            txtamount: "required",
            txtPOpaymentmethod: "required",
            txtdeliverystatus: "required",
            txtorderstatus: "required"
        },
        messages: {
            txtorderdate: "Please emter order date",
            txtcompanyname: "Please enter company name",
            txtorderdetails: "Please enter order details",
            txtamount: "Please enter order Amount",
            txtPOpaymentmethod: "Please Enter Payment method",
            txtdeliverystatus: "Please Enter Delivery status",
            txtorderstatus: "Plese enter orderstatus"
        }
    })
    $("#updatedetailbtn").on('click', function () {
        $("#UpdateOrderDetailsForm").validate();
    });
    $('#txtProducts').change(function () {
        var Text = $("#txtProducts Option:Selected").text();
        var ProductTypeId = $(this).val();
        var VendorTypeId = $("#txtvendorname").val();
        var Productid = $("#txtProductid").val(ProductTypeId);

        $("#txtProductTypeid").val(Text);
        $('#searchproductname').empty();
        $('#searchproductname').append('<Option >--Select ProductName--</Option>');
        $.ajax({
            url: '/ProductMaster/SerchProductByVendor?ProductId=' + ProductTypeId + '&VendorId=' + VendorTypeId,
            type: 'Post',
            success: function (result) {
                $.each(result, function (i, data) {
                    $('#searchproductname').append('<Option value=' + data.id + '>' + data.productName + '</Option>');
                    $('#txtvendorname').prop('disabled', true);
                    $('#txtProducts').prop('disabled', true);
                });
            }
        });
    });
    $('#createorder').on('click', function () {
        $("#createOrderForm").validate();
    });

    $("#statusform").validate({
        rules: {
            idStatus: "required"
        },
        messages: {
            idStatus: "Please enter delivered status"
        }
    })
    $('#statussearch').on('click', function () {
        $("#statusform").validate();
    });

    $('#textVendorNameHidden').change(function () {
        fn_getPOVendorDetail($(this).val());
    });
    $('#textCompanyNameHidden').change(function () {
        fn_getPOCompanyDetail($(this).val());
    });
    function handleFocus(event, selector) {
        if (event.keyCode == 13 || event.keyCode == 9) {
            event.preventDefault();
            $(selector).focus();
        }
    }
    $(document).on('input', '.product-quantity', function () {
        var row = $(this).closest('.product');
        fn_updatePOProductAmount(row);
        fn_updatePOTotals();
    }).on('keydown', '.product-quantity', function (event) {
        var row = $(this).closest(".product");
        var productFocus = row.find('#txtproductamount');
        handleFocus(event, productFocus);
    });
    function debounce(func, delay) {
        let timer;
        return function (...args) {
            clearTimeout(timer);
            timer = setTimeout(() => func.apply(this, args), delay);
        };
    }
    $(document).on('input', '#txtproductamount', function () {
        var productRow = $(this).closest(".product");
        var productAmount = parseFloat($(this).val());

        productRow.find("#productamount").val(productAmount.toFixed(2));
        fn_updatePOProductAmount(productRow);
        fn_updatePOTotals();

    }).on('keydown', '#txtproductamount', function (event) {
        var productRow = $(this).closest(".product");
        var gstFocus = productRow.find('#txtgst');
        handleFocus(event, gstFocus);
    });

    $(document).on('input', '#txtgstPercentage', function () {
        var row = $(this).closest('.product');
        fn_updatePOProductAmount(row);
        fn_updatePOTotals();
    }).on('keydown', '#txtgstPercentage', function (event) {
        if (event.key === 'Enter') {
            $(this).blur();
        }
    });
    $(document).on('focusout', '.product-quantity', function () {
        $(this).trigger('input');
    });
});
function preventPOEmptyValue(input) {

    if (input.value === "") {

        input.value = 1;
    }
}

function showPaymentDetails() {
    $("#PaymentDetails").modal("show")
}


function fn_GetPOVendorNameList() {
    $.ajax({
        url: '/ProductMaster/GetVendorsNameList',
        method: 'GET',
        success: function (result) {
            var vendorTypes = result.map(function (data) {
                return {
                    label: data.vendorCompany,
                    value: data.id
                };
            });


            $("#textVendorName").autocomplete({
                source: vendorTypes,
                minLength: 0,
                select: function (event, ui) {

                    event.preventDefault();
                    $("#textVendorName").val(ui.item.label);
                    $("#textVendorNameHidden").val(ui.item.value);

                    $("#textVendorNameHidden").trigger('change');
                }
            }).focus(function () {
                $(this).autocomplete("search");
            });
        },
        error: function (err) {
            console.error("Failed to fetch vendor types: ", err);
        }
    });
}
function fn_getPOVendorDetail(VendorId) {
    $.ajax({
        url: '/Vendor/GetVendorDetailsById?vendorId=' + VendorId,
        type: 'GET',
        contentType: 'application/json;charset=utf-8',
        dataType: 'json',
        success: function (response) {
            if (response) {
                $('#textVendorMobile').val(response.vendorPhone);
                $('#textVendorGSTNumber').val(response.vendorGstnumber);
                $('#textVendorAddress').val(response.vendorAddress);
            } else {
                console.log('Empty response received.');
            }
        },
    });
}
function selectvendorId() {
    document.getElementById("txtvendorTypeid").value = document.getElementById("txtvendorname").value;
}
var companyMap = {};

function fn_GetPOCompanyNameList() {
    $.ajax({
        url: '/Company/GetCompanyNameList',
        method: 'GET',
        success: function (result) {
            var companyTypes = result.map(function (data) {
                return {
                    label: data.compnyName,
                    value: data.id
                };
            });


            $("#textCompanyName").autocomplete({
                source: companyTypes,
                minLength: 0,
                select: function (event, ui) {

                    event.preventDefault();
                    $("#textCompanyName").val(ui.item.label);
                    $("#textCompanyNameHidden").val(ui.item.value);

                    $("#textCompanyNameHidden").trigger('change');
                }
            }).focus(function () {
                $(this).autocomplete("search");
            });
        },
        error: function (err) {
            console.error("Failed to fetch company types: ", err);
        }
    });
}

function fn_getPOCompanyDetail(CompanyName) {
    var CompanyId = CompanyName;
    $.ajax({
        url: '/Company/GetCompanyDetailsById',
        type: 'GET',
        contentType: 'application/json;charset=utf-8',
        dataType: 'json',
        data: { CompanyId: CompanyId },
        success: function (response) {
            if (response) {
                $('#textCompanyGstNo').val(response.gst);
                $('#textCompanyBillingAddress').val(response.fullAddress);
            } else {
                toastr.error('Empty response received.');
            }
        },
    });
}
function fn_GetPOProductsList() {
    $.ajax({
        url: '/ProductMaster/GetProduct',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#txtProducts').append('<Option value=' + data.id + '>' + data.productName + '</Option>')
            });
        }
    });
}

function fn_POProductTypeDropdown(productId) {

    if ($('#txtPOProductType_' + productId + ' option').length > 1) {
        return
    }
    $.ajax({
        url: '/ProductMaster/GetProduct',
        success: function (result) {
            $('#txtPOProductType_' + productId).empty();

            $.each(result, function (i, data) {
                $('#txtPOProductType_' + productId).append('<option value="' + data.id + '">' + data.productName + '</option>');

            });

            $('#txtPOProductType_' + productId).val($("#txtunittype_" + productId).val())


        }
    });
}

function fn_SearchItemDetailsById(ProductId) {
    var GetProductId = {
        ProductId: ProductId,
    }
    var form_data = new FormData();
    form_data.append("ProductId", JSON.stringify(GetProductId));

    $.ajax({
        url: '/PurchaseOrderMaster/DisplayPOProductDetailsListById',
        type: 'Post',
        datatype: 'json',
        data: form_data,
        processData: false,
        contentType: false,
        complete: function (Result) {

            if (Result.statusText === "success") {
                fn_AddNewPODetailsRow(Result.responseText);
            }
            else {
                var GetProductId = $('#searchProductname').val();
                if (GetProductId === "Select ProductName" || GetProductId === null) {
                    $('#searchvalidationMessage').text('Please select ProductName!!');
                }
                else {
                    $('#searchvalidationMessage').text('');
                }
            }
        }
    });
}

function deletePurchaseOrderDetails(Id) {
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
                url: '/PurchaseOrderMaster/DeletePurchaseOrderDetails?Id=' + Id,
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
                            window.location = '/PurchaseOrderMaster/PurchaseOrders';
                        })
                    } else {
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
                        window.location = '/PurchaseOrderMaster/PurchaseOrders';
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
function fn_UpdatePurchaseOrderDetails() {
    if ($("#CreatePOForm").valid()) {
        if ($('#addnewproductlink tr').length >= 1) {
            var ProductDetails = [];
            $(".product").each(function () {
                var orderRow = $(this);
                var productName = orderRow.find("#textProductName").text().trim();
                var productId = orderRow.find("#textProductId").val().trim();
                var objData = {
                    Product: productName,
                    ProductId: productId,
                    ProductType: orderRow.find("#textProductType").val(),
                    Quantity: orderRow.find("#txtproductquantity").val(),
                    Price: orderRow.find("#txtproductamount").val(),
                    Gstamount: orderRow.find("#txtgstAmount").val(),
                    Gstper: orderRow.find("#txtgstPercentage").val(),
                    Hsn: orderRow.find("#txtHSNcode").val(),
                    ProductTotal: orderRow.find("#txtproducttotalamount").val(),
                };
                ProductDetails.push(objData);
            });
            var PONumber = $("#textPoId").val();
            var PODetails = {
                Id: $("#txtID").val(),
                ProjectId: $("#txtPOProjectId").val(),
                OrderId: $("#textPoId").val(),
                VendorId: $("#textVendorNameHidden").val(),
                CompanyId: $("#textCompanyNameHidden").val(),
                TotalGst: $("#totalgst").val(),
                SubTotal: $("#cart-subtotal").val(),
                TotalAmount: $("#cart-total").val(),
                DeliveryDate: $("#UnitTypeId").val(),
                OrderDate: $("#textOrderDate").val(),
                OrderStatus: $("#UnitTypeId").val(),
                PaymentMethod: $("#txtPOpaymentmethod").val(),
                PaymentStatus: $("#txtPOpaymenttype").val(),
                DeliveryStatus: $("#textDeliveryStatus").val(),
                CreatedBy: $("#textCreatedByVal").val(),
                CreatedOn: $("#textCreatedOnId").val(),
                UpdatedBy: $("#textCreatedById").val(),
                Address: $('#hideShippingAddress').is(':checked') ? $('#textCompanyBillingAddress').val() : $('#textShippingAddress').val(),
                ProductList: ProductDetails,
                DollarPrice: $('#PODollarAmount').val(),
            }

            var form_data = new FormData();
            form_data.append("PurchaseOrder", JSON.stringify(PODetails));
            $.ajax({
                url: '/PurchaseOrderMaster/UpdatePurchaseOrderDetails',
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
                            window.location = '/PurchaseOrderMaster/PurchaseOrderDetails/?OrderId=' + PONumber;
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
        else {
            if ($('#addnewproductlink tr').length == 0) {
                $('#AddVendorModelButton').addClass('error-border');
                toastr.warning("Please select product!");
            }
        }
    }
    else {
        toastr.warning("Kindly fill all data fields");
    }
}
$("#txtProducts").change(function () {
    ProductDetailsByProductTypeId()
})
function ProductDetailsByProductTypeId() {
    var form_data = new FormData();
    form_data.append("ProductId", $('#txtProducts').val());
    $.ajax({
        url: '/ProductMaster/GetProductById',
        type: 'Post',
        datatype: 'json',
        data: form_data,
        processData: false,
        contentType: false,
        complete: function (Result) {
            $("#table-product-list-all").hide();
            $("#ProductdetailsPartial").html(Result.responseText);
        }
    });
}
var count = 0;
function fn_AddNewPODetailsRow(Result) {

    var newProductRow = $(Result);
    var productId = newProductRow.data('product-id');
    fn_POProductTypeDropdown(productId);
    var newProductId = newProductRow.attr('data-product-id');
    var isDuplicate = false;
    $('#addnewproductlink .product').each(function () {
        var existingProductRow = $(this);
        var existingProductId = existingProductRow.attr('data-product-id');
        if (existingProductId === newProductId) {
            isDuplicate = true;
            return false;
        }
    });
    if (!isDuplicate) {
        count++;
        $("#addnewproductlink").append(Result);
        fn_updatePOTotals();
        fn_updatePORowNumbers();
    } else {
        Swal.fire({
            title: "Product already added!",
            text: "The selected product is already added.",
            icon: "warning",
            confirmButtonColor: "#3085d6",
            confirmButtonText: "OK"
        });
    }
}
function fn_updatePORowNumbers() {
    $(".product-id").each(function (index) {
        $(this).text(index + 1);
    });
}
function fn_updatePOProductAmount(that) {
    var row = $(that);
    var productPrice = parseFloat(row.find("#txtproductamount").val());
    var quantity = parseInt(row.find("#txtproductquantity").val());
    var gst = parseFloat(row.find("#txtgstPercentage").val());
    var totalGst = (productPrice * quantity * gst) / 100;
    var totalAmount = productPrice * quantity + totalGst;

    row.find("#txtgstAmount").val(totalGst.toFixed(2));
    row.find("#txtproducttotalamount").val(totalAmount.toFixed(2));
}
function fn_updatePOTotals() {

    var totalSubtotal = 0;
    var totalGst = 0;
    var totalAmount = 0;
    var TotalItemQuantity = 0;
    $(".product").each(function () {

        var row = $(this);
        var subtotal = parseFloat(row.find("#txtproductamount").val());
        var gst = parseFloat(row.find("#txtgstAmount").val());
        var totalquantity = parseFloat(row.find("#txtproductquantity").val());

        totalSubtotal += subtotal * totalquantity;
        totalGst += gst;
        TotalItemQuantity += totalquantity;
        totalAmount = totalSubtotal + totalGst;
    });
    $("#cart-subtotal").val(totalSubtotal.toFixed(2));
    $("#totalgst").val(totalGst.toFixed(2));
    $("#cart-total").val(totalAmount.toFixed(2));
    $("#TotalProductQuantity").text(TotalItemQuantity);
    $("#TotalProductPrice").html(totalSubtotal.toFixed(2));
    $("#TotalProductGST").html(totalGst.toFixed(2));
    $("#TotalProductAmount").html(totalAmount.toFixed(2));
}
function fn_POremoveItem(btn) {
    $(btn).closest("tr").remove();
    fn_updatePORowNumbers();
    fn_updatePOTotals();
}
function fn_OpenPOShippingModal() {
    $('#textmdAddress').val('');
    $('#textmdQty').val('');
    $('#mdShippingAdd').modal('show');
}
function fn_mdAddPOAddress() {
    var rowcount = $('#dvShippingAddress .row.ac-invoice-shippingadd').length + 1
    if ($('#textmdAddress').val() != null && $('#textmdAddress').val().trim() != "") {
        var html = `<div class="row ac-invoice-shippingadd">
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                </div>`
        $('#dvShippingAddress').append(html);
    } else {
        tostar.error('Please select address!');
        $('#textmdAddress').focus();
    }

}
function fn_removeShippingAdd(that) {
    $(that).closest('.ac-invoice-shippingadd').remove();
}
function fn_POtoggleShippingAddress() {
    var checkbox = document.getElementById("hideShippingAddress");
    var shippingFields = document.getElementById("shippingAddressFields");

    if (checkbox.checked) {
        shippingFields.style.display = "none";
    } else {
        shippingFields.style.display = "block";
    }
}
function fn_InsertPurchaseOrderDetails() {

    if ($("#CreatePOForm").valid()) {
        if ($('#addnewproductlink tr').length >= 1) {
            var ProductDetails = [];
            $(".product").each(function () {
                var orderRow = $(this);
                var productName = orderRow.find("#textProductName").text().trim();
                var productId = orderRow.find("#textProductId").val().trim();
                var objData = {
                    Product: productName,
                    ProductId: productId,
                    ProductType: orderRow.find("#textProductType").val(),
                    Quantity: orderRow.find("#txtproductquantity").val(),
                    Price: orderRow.find("#txtproductamount").val(),
                    Gstamount: orderRow.find("#txtgstAmount").val(),
                    Gstper: orderRow.find("#txtgstPercentage").val(),
                    Hsn: orderRow.find("#txtHSNcode").val(),
                    ProductTotal: orderRow.find("#txtproducttotalamount").val(),
                };
                ProductDetails.push(objData);
            });
            var PONumber = $("#textPoId").val();
            var PODetails = {
                ProjectId: $("#textProjectId").val(),
                OrderId: $("#textPoId").val(),
                VendorId: $("#textVendorNameHidden").val(),
                CompanyId: $("#textCompanyNameHidden").val(),
                TotalGst: $("#totalgst").val(),
                SubTotal: $("#cart-subtotal").val(),
                TotalAmount: $("#cart-total").val(),
                DeliveryDate: $("#UnitTypeId").val(),
                OrderDate: $("#textOrderDate").val(),
                OrderStatus: $("#UnitTypeId").val(),
                PaymentMethod: $("#txtPOpaymentmethod").val(),
                PaymentStatus: $("#txtPOpaymenttype").val(),
                DeliveryStatus: $("#textDeliveryStatus").val(),
                CreatedBy: $("#textCreatedById").val(),
                Address: $('#hideShippingAddress').is(':checked') ? $('#textCompanyBillingAddress').val() : $('#textShippingAddress').val(),
                ProductList: ProductDetails,
                DollarPrice: $('#PODollarAmount').val(),
            }

            var form_data = new FormData();
            form_data.append("PurchaseOrder", JSON.stringify(PODetails));
            $.ajax({
                url: '/PurchaseOrderMaster/InsertMultiplePurchaseOrderDetails',
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
                            window.location = '/PurchaseOrderMaster/PurchaseOrderDetails/?OrderId=' + PONumber;
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
        else {
            if ($('#addnewproductlink tr').length == 0) {
                $('#AddVendorModelButton').addClass('error-border');
                toastr.warning("Please select product!");
            }
        }
    }
    else {
        toastr.warning("Kindly fill all data fields");
    }
}
function createPO() {

    if ($("#drpProjectName").val() == "") {
        Swal.fire({
            title: "Kindly select project on dashboard.",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        });
    }
    else {
        window.location = '/PurchaseOrderMaster/CreatePurchaseOrder';
    }
}

$("#deliveredactive").click(function () {
    $("#status-delivered").show();
    $("#dvdeliveredstatus").hide();
});
$("#allordersactive").click(function () {
    $("#project-overview").show();
    $("#dvdeliveredstatus").hide();
});
$("#pickupactive").click(function () {
    $("#status-pickups").show();
    $("#dvdeliveredstatus").hide();
});
$("#cancelledactive").click(function () {
    $("#status-cancelled").show();
    $("#dvdeliveredstatus").hide();
});
$("#inprogressactive").click(function () {
    $("#status-inprogress").show();
    $("#dvdeliveredstatus").hide();
});
$("#pendingactive").click(function () {
    $("#status-pending").show();
    $("#dvdeliveredstatus").hide();
});
$("#returnsactive").click(function () {
    $("#status-returns").show();
    $("#dvdeliveredstatus").hide();
});


$(document).ready(function () {
    function formatWithCommas(value) {
        return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }

    function updateTotals() {
        const totalProductPrice = document.getElementById("TotalProductPrice").textContent;
        const totalProductGST = document.getElementById("TotalProductGST").textContent;
        const totalProductAmount = document.getElementById("TotalProductAmount").textContent;
        const cartSubtotal = document.getElementById("cart-subtotal").value;
        const totalGst = document.getElementById("totalgst").value;
        const cartTotal = document.getElementById("cart-total").value;

        document.getElementById("TotalProductPrice").textContent = formatWithCommas(totalProductPrice);
        document.getElementById("TotalProductGST").textContent = formatWithCommas(totalProductGST);
        document.getElementById("TotalProductAmount").textContent = formatWithCommas(totalProductAmount);
        document.getElementById("cart-subtotal").value = formatWithCommas(cartSubtotal);
        document.getElementById("totalgst").value = formatWithCommas(totalGst);
        document.getElementById("cart-total").value = formatWithCommas(cartTotal);
    }

    updateTotals();
});

let POdollarModal;

function checkPOCurrencySelection() {
    const currencySelect = document.getElementById('POcurrency-select');

    if (!POdollarModal) {
        POdollarModal = new bootstrap.Modal(document.getElementById('PODollarModal'));
    }

    if (currencySelect.value === "$") {
        POdollarModal.show();
    }
}

function ClosePODollarModel() {

    if (POdollarModal) {
        POdollarModal.hide();
    }
}


function SavePODollarAmount() {

    const POdollarModal = document.getElementById('txtPODollarAmount').value;

    if (POdollarModal == "") {
        toastr.warning("Emter DollarAmount!");
    }
    else {
        document.getElementById('PODollarAmount').value = POdollarModal;
        ClosePODollarModel();
    }
}