
var Formdata = window.userFormPermissions || 0;
function GetInvoiceDetailsByOrderId(OrderId) {
    $.ajax({
        url: '/Invoice/GetInvoiceDetailsByOrderId/?OrderId=' + OrderId,
        type: 'GET',
        success: function (result) {
            if (result.code == 400) {
                toastr.error(result.message);
            } else {
                window.location = '/Invoice/InvoiceDetails/?OrderId=' + OrderId;
            }
        },
        error: function (xhr, status, error) {
            console.error('AJAX Error:', error);
            alert('An error occurred while fetching data.');
        }
    });
}
function ShowInvoiceDetailsByOrderId(OrderId) {

    $.ajax({
        url: '/Invoice/ShowInvoiceDetailsByOrderId/?OrderId=' + OrderId,
        type: 'GET',
        success: function (result) {
            if (result.code == 400) {
                toastr.error(Result.message);
            } else {
                window.location = '/Invoice/InvoiceDetails?OrderId=' + OrderId;
            }
        },
        error: function (xhr, status, error) {
            toastr.error('AJAX Error:', error);
            toastr.error('An error occurred while fetching data.');
        }
    });
}
function fn_InsertInvoiceDetails() {

    if ($("#CreateInvoiceForm").valid()) {
        if ($('#addnewproductlink tr').length >= 1) {

            var ProductDetails = [];
            $(".product").each(function () {
                var orderRow = $(this);
                var descriptions = [];
                orderRow.find(".txtInvoiceProductDes").each(function () {
                    var descriptionVal = $(this).val().trim();
                    if (descriptionVal) {
                        descriptions.push(descriptionVal);
                    }
                });

                var productName = orderRow.find("#textProductName").text().trim();
                var productId = orderRow.find("#textProductId").val().trim();
                var objData = {
                    Product: productName,
                    Description: descriptions.join("<br>"),
                    ProductId: productId,
                    ProductType: orderRow.find("#txtPOProductType_" + orderRow.find("#textProductId").val()).val(),
                    Quantity: orderRow.find("#txtproductquantity").val(),
                    Hsn: orderRow.find("#txtHSNcode").val(),
                    Price: orderRow.find("#txtproductamount").val(),
                    GstAmount: orderRow.find("#txtgstAmount").val(),
                    GstPer: orderRow.find("#txtgst").val(),
                    IGst: orderRow.find("#txtigst").val(),
                    ProductTotal: orderRow.find("#txtproducttotalamount").val(),
                    DiscountAmount: orderRow.find("#txtdiscountamount").val(),
                    DiscountPer: orderRow.find("#txtdiscountpercentage").val(),
                };
                ProductDetails.push(objData);
            });
            var dateInput = $("#textBuysOrderDate").val();
            if (dateInput === "") {
                dateInput = null;
            }
            var Invoicedetails = {
                ProjectId: $("#textProjectId").val(),
                InvoiceNo: $("#textInvoiceNo").val(),
                VandorId: $("#ddlVendorName").val(),
                CompanyId: $("#ddlinvompanyName").val(),
                TotalGst: $("#totalgst").val(),
                Cgst: $("#textCGst").val(),
                Sgst: $("#textSGst").val(),
                Igst: $("#textIGst").val(),
                SubTotal: $("#cart-subtotal").val(),
                TotalAmount: $("#cart-total").val(),
                DispatchThrough: $("#textDispatchThrough").val(),
                DispatchDocNo: $("#textDispatchDocNo").val(),
                Destination: $("#textDestination").val(),
                MotorVehicleNo: $("#textMotorVehicleNo").val(),
                BuyesOrderNo: $("#textBuysOrderNo").val(),
                BuyesOrderDate: dateInput,
                InvoiceDate: $("#textInvoiceDate").val(),
                OrderStatus: $("#UnitTypeId").val(),
                PaymentMethod: $("#ddlInvoicepaymentmethod").val(),
                PaymentStatus: $("#ddlInvoicepaymenttype").val(),
                CreatedBy: $("#textCreatedById").val(),
                RoundOff: $('#cart-roundOff').val(),
                TotalDiscount: $('#cart-discount').val(),
                ShippingAddress: $('#hideShippingAddress').is(':checked') ? $('#textVendorAddress').val() : $('#textShippingAddress').val(),
                InvoiceDetails: ProductDetails,
                DollarPrice: $("#InvoiceDollarAmount").val(),
            }
            var form_data = new FormData();
            form_data.append("INVOICEDETAILS", JSON.stringify(Invoicedetails));

            $.ajax({
                url: '/Invoice/InsertInvoiceDetails',
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
                            window.location = '/Invoice/Invoices';
                        });
                    }
                    else {
                        toastr.error(Result.message);
                    }
                },
                error: function (xhr, status, error) {
                    toastr.error("An error occurred while processing your request.");
                }
            });
        } else {
            if ($('#addnewproductlink tr').length == 0) {
                $('#AddVendorModelButton').addClass('error-border');
                toastr.warning("Please select product!");
            }
        }
    }
    else {
        toastr.warning("Kindly fill all datafield");
    }
}
function fn_UpdateInvoiceDetails() {

    if ($("#CreateInvoiceForm").valid()) {
        if ($('#addnewproductlink tr').length >= 1) {

            var ProductDetails = [];
            $(".product").each(function () {
                var orderRow = $(this);
                var descriptions = [];
                orderRow.find(".txtInvoiceProductDes").each(function () {
                    var descriptionVal = $(this).val().trim();
                    if (descriptionVal) {
                        descriptions.push(descriptionVal);
                    }
                });

                var productName = orderRow.find("#textProductName").text().trim();
                var productId = orderRow.find("#textProductId").val().trim();
                var objData = {
                    Product: productName,
                    Description: descriptions.join("<br>"),
                    ProductId: productId,
                    ProductType: orderRow.find("#txtPOProductType_" + orderRow.find("#textProductId").val()).val(),
                    Quantity: orderRow.find("#txtproductquantity").val(),
                    Hsn: orderRow.find("#txtHSNcode").val(),
                    Price: orderRow.find("#txtproductamount").val(),
                    GstAmount: orderRow.find("#txtgstAmount").val(),
                    GstPer: orderRow.find("#txtgst").val(),
                    IGst: orderRow.find("#txtigst").val(),
                    ProductTotal: orderRow.find("#txtproducttotalamount").val(),
                    DiscountAmount: orderRow.find("#txtdiscountamount").val(),
                    DiscountPer: orderRow.find("#txtdiscountpercentage").val(),
                };
                ProductDetails.push(objData);
            });
            var dateInput = $("#textBuysOrderDate1").val();
            if (dateInput === "0001-01-01") {
                dateInput = null;
            }
            var Invoicedetails = {
                Id: $("#textInvoiceId").val(),
                ProjectId: $("#textinvoiceProjectId").val(),
                InvoiceNo: $("#textInvoiceNo").val(),
                VandorId: $("#textVendorNameHidden").val(),
                CompanyId: $("#textCompanyNameHidden").val(),
                TotalGst: $("#totalgst").val(),
                SubTotal: $("#cart-subtotal").val(),
                TotalAmount: $("#cart-total").val(),
                DispatchThrough: $("#textDispatchThrough").val(),
                DispatchDocNo: $("#textDispatchDocNo").val(),
                Destination: $("#textDestination").val(),
                MotorVehicleNo: $("#textMotorVehicleNo").val(),
                BuyesOrderNo: $("#textBuysOrderNo").val(),
                BuyesOrderDate: dateInput,
                InvoiceDate: $("#textInvoiceDate").val(),
                OrderStatus: $("#UnitTypeId").val(),
                PaymentMethod: $("#txtInvoicepaymentmethod").val(),
                PaymentStatus: $("#txtInvoicepaymenttype").val(),
                CreatedBy: $("#textCreatedById").val(),
                CreatedOn: $("#txtCreatedOn").val(),
                RoundOff: $('#cart-roundOff').val(),
                TotalDiscount: $('#cart-discount').val(),
                UpdatedBy: $("#textCreatedById").val(),
                ShippingAddress: $('#hideShippingAddress').is(':checked') ? $('#textVendorAddress').val() : $('#textShippingAddress').val(),
                InvoiceDetails: ProductDetails,
                DollarPrice: $("#InvoiceDollarAmount").val(),
            }

            var form_data = new FormData();
            form_data.append("UPDATEINVOICEDETAILS", JSON.stringify(Invoicedetails));

            $.ajax({
                url: '/Invoice/UpdateInvoiceDetails',
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
                            window.location = '/Invoice/Invoices';
                        });
                    }
                    else {
                        toastr.error(Result.message);
                    }
                },
                error: function (xhr, status, error) {
                    toastr.error("An error occurred while processing your request.");
                }
            });
        } else {
            if ($('#addnewproductlink tr').length == 0) {
                $('#AddVendorModelButton').addClass('error-border');
                toastr.warning("Please select product!");
            }
        }
    }
    else {
        toastr.warning("Kindly fill all datafield");
    }
}

function fn_deleteInvoice(InvoiceId) {
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
                url: '/Invoice/IsDeletedInvoice?InvoiceId=' + InvoiceId,
                type: 'POST',
                dataType: 'json',
                success: function (Result) {
                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/Invoice/Invoices';
                    })
                },
                error: function () {
                    Swal.fire({
                        title: "Can't delete invoice!",
                        icon: 'warning',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/Invoice/Invoices';
                    })
                }
            })
        } else if (result.dismiss === Swal.DismissReason.cancel) {

            Swal.fire(
                'Cancelled',
                'Invoice have no changes.!!😊',
                'error'
            );
        }
    });
}

let invoiceTableGrid = [];
let startDate = null;
let endDate = null;

$(document).ready(function () {

    const userPermissionArray = Formdata || [];
    let canEdit = false;
    let canDelete = false;

    for (let i = 0; i < userPermissionArray.length; i++) {
        const permission = userPermissionArray[i];
        if (permission.formName === "Invoice List") {
            canEdit = permission.edit;
            canDelete = permission.delete;
            break;
        }
    }

    invoiceTableGrid = {
        rowHeight: 50,
        columnDefs: [
            {
                headerName: "Invoice No",
                field: "invoiceNo",
                sortable: true,
                filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';
                    return `<a href="/Invoice/InvoiceDetails?InvoiceId=${params.data.id}"><span style="color: #16989A !important;">` + params.data.invoiceNo + `</span></a>`;
                }
            },
            {
                headerName: "Invoice Date", field: "invoiceDate", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';
                    return getCommonDateformat(params.data.date);
                }
            },

            { headerName: "Vendor Name", field: "vendorName", sortable: true, filter: true },
            { headerName: "Project Name", field: "projectName", sortable: true, filter: true },
            { headerName: "Total Amount", field: "totalAmount", sortable: true, filter: true },
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
            invoiceTableGrid.api = params.api;
            invoiceTableGrid.columnApi = params.columnApi;
            invoiceTableGrid.api.sizeColumnsToFit();
            createEnhancedPagination(params.api);
        },
        rowModelType: 'infinite',
        cacheBlockSize: 20,
        pagination: true,
        paginationPageSize: 20,
        suppressPaginationPanel: true,
        datasource: getInvoiceDatasource()
    };
    function getInvoiceDatasource() {
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
                    searchValue: $('#txtInvoiceSearch').val(),
                    ComapnyFilter: $('#txtInvoiceCompanyName').val(),
                    VendorFilter: $('#txtInvoiceVendorName').val(),
                    StartDate: startDate,
                    EndDate: endDate,
                };

                $.ajax({
                    url: '/Invoice/GetInvoiceListView',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(request),
                    success: function (response) {
                        params.successCallback(response.rowsThisPage, response.totalRowCount);
                        const rangeDisplay = document.querySelector('#invoiceTable .range-display');
                        if (rangeDisplay) updateEnhancedPagination(invoiceTableGrid.api, rangeDisplay);
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

        const eGui = document.querySelector('#invoiceTable');
        const paginationEl = document.createElement('div');
        paginationEl.className = 'ag-paging-panel enhanced';
        paginationEl.appendChild(paginationContainer);
        eGui.appendChild(paginationEl);

        const pageSizeSelector = pageSizeContainer.querySelector('.page-size-selector');
        pageSizeSelector.addEventListener('change', function () {
            const newPageSize = Number(this.value);


            const gridDiv = document.querySelector('#invoiceTable');

            invoiceTableGrid = {
                ...invoiceTableGrid,
                cacheBlockSize: newPageSize,
                paginationPageSize: newPageSize,
                datasource: getInvoiceDatasource(),
            };

            gridDiv.innerHTML = '';
            agGrid.createGrid(gridDiv, invoiceTableGrid);
        });

        updateEnhancedPagination(gridApi, rangeDisplay);
    }

    function updateEnhancedPagination(gridApi, rangeDisplay) {
        const currentPage = gridApi.paginationGetCurrentPage() + 1;
        const totalPages = gridApi.paginationGetTotalPages();
        const totalRows = gridApi.paginationGetRowCount();
        const pageSize = invoiceTableGrid.paginationPageSize;

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

    if (canEdit || canDelete) {
        invoiceTableGrid.columnDefs.push({
            headerName: "Actions",
            field: "actions",
            sortable: false,
            filter: false,
            cellRenderer: function (params) {
                if (!params.data || !params.data.id) return '';

                let buttons = '';
                if (canEdit) {
                    buttons += `<a href="/Invoice/CreateInvoice?Id=${params.data.id}" class="btn text-info editbtn">
                        <i class="fa-regular fa-pen-to-square"></i></a>`;
                }
                if (canDelete) {
                    buttons += `<a onclick="fn_deleteInvoice('${params.data.id}')" class="btn text-danger">
                        <i class="fas fa-trash"></i></a>`;
                }
                return buttons;
            }
        });
    }

    const myGridElement = document.querySelector('#invoiceTable');
    agGrid.createGrid(myGridElement, invoiceTableGrid);


    $('#txtInvoiceSearch').on('change keyup', function () {
        invoiceTableGrid.api.onFilterChanged();
    });
    $('#txtInvoiceCompanyName').change(() => {
        const companyText = $("#txtInvoiceCompanyName option:selected").text();
        $("#txtCompanyName").val(companyText === 'All Company' ? '' : companyText);
        if (invoiceTableGrid.api) {
            invoiceTableGrid.api.onFilterChanged();
        }
    });

    $('#txtInvoiceVendorName').change(() => {
        const vendorText = $("#txtInvoiceVendorName option:selected").text();
        $("#txtVendorName").val(vendorText === 'All Vendor' ? '' : vendorText);
        if (invoiceTableGrid.api) {
            invoiceTableGrid.api.onFilterChanged();
        }
    });
    $('#toggleDateFilter').click(e => {
        e.stopPropagation();
        $('#dateFilterContainer').toggle();
    });

    $('#applyFilters').click(() => {
        startDate = $('#txtstartdatebox').val() || null;
        endDate = $('#txtenddatebox').val() || null;
        if (invoiceTableGrid.api) {
            invoiceTableGrid.api.onFilterChanged();
        }
    });
});
function ResetInvoiceFilters() {
    window.location = '/Invoice/Invoices';
}
function createInvoice() {
    if ($("#txtInvoice").val() == "") {
        Swal.fire({
            title: "Kindly select project!",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        });
    }
    else {
        window.location = '/Invoice/CreateInvoice';
    }
}

$(document).ready(function () {

    $("#CreateInvoiceForm").validate({
        rules: {
            textVendorName: "required",
            textCompanyName: "required",
            txtInvoicepaymentmethod: "required",
            textDispatchThrough: "required",
        },
        highlight: function (element) {
            if (element.id === "txtInvoicepaymentmethod" || element.id === "textDispatchThrough") {
                $(element).addClass('is-invalid');
            }
        },
        unhighlight: function (element) {
            if (element.id === "txtInvoicepaymentmethod" || element.id === "textDispatchThrough") {
                $(element).removeClass('is-invalid');
            }
        },
        errorPlacement: function (error, element) {
            if (element.attr("id") === "textVendorName" || element.attr("id") === "textCompanyName") {
                error.insertAfter(element);
            }
        },
        messages: {
            textVendorName: "Select Vendor Name",
            textCompanyName: "Select Company Name",
            txtInvoicepaymentmethod: "",
            textDispatchThrough: "",
        }
    });


    $('#ddlinvompanyName,#txtInvoiceCompanyName').select2({
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
                        text: item.compnyName
                    }))
                };
            },
            error: function (xhr, status, error) {
                console.error("Error fetching company list:", error);
            }
        }
    });

    $('#ddlinvompanyName').on('select2:select', function (e) {
        var companyId = e.params.data.id;
        fn_getInvoiceCompanyDetail(companyId);
    });

    $('#ddlVendorName,#txtInvoiceVendorName').select2({
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

    $('#ddlVendorName').on('select2:select', function (e) {
        var vendorId = e.params.data.id;
        fn_getInvoiceVendorDetail(vendorId);
    });


    $('#ddlInvoicepaymentmethod').select2({
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



    fn_updateInvoiceTotals()
    function handleFocus(event, selector) {
        if (event.keyCode == 13 || event.keyCode == 9) {
            event.preventDefault();
            $(selector).focus();
        }
    }
    $(document).on('input', '.product-quantity', function () {
        var row = $(this).closest('.product');
        fn_updateInvoiceProductAmount(row);
        fn_updateInvoiceTotals();
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

    $(document).on('input', '#txtdiscountpercentage', debounce(function () {

        var value = $(this).val();
        var productRow = $(this).closest(".product");
        if (value > 100) {
            toastr.warning("Discount cannot be greater than 100%");
            productRow.find("#txtdiscountpercentage").val(0);
            productRow.find("#txtdiscountamount").val(0);
        } else if (value <= 0 || value == "") {
            productRow.find("#txtdiscountamount").val(0);
            productRow.find("#txtdiscountpercentage").val(0);
            fn_updateInvoiceProductAmount(productRow);
        } else {
            fn_UpdateInvoiceDiscountPercentage(productRow);
        }
    }, 300)).on('keydown', '#txtdiscountpercentage', function (event) {
        var productRow = $(this).closest(".product");
        var gstFocus = productRow.find('#txtgst');
        handleFocus(event, gstFocus);
    });

    $(document).on('input', '#txtdiscountamount', debounce(function () {
        var productRow = $(this).closest(".product");
        var discountAmount = parseFloat($(this).val());
        var productAmount = parseFloat($(productRow).find("#productamount").val());

        if (discountAmount > productAmount) {
            toastr.warning("Amount cannot be greater than Item price");
            productRow.find("#txtdiscountamount").val(0);
            productRow.find("#txtdiscountpercentage").val(0);
        } else if (discountAmount <= 0 || discountAmount == "") {
            productRow.find("#txtdiscountamount").val(0);
            productRow.find("#txtdiscountpercentage").val(0);
            fn_updateInvoiceProductAmount(productRow);
        } else {
            fn_updateInvoiceDiscount(productRow);
        }
    }, 300)).on('keydown', '#txtdiscountamount', function (event) {
        var productRow = $(this).closest(".product");
        var discountPercentagefocus = productRow.find('#txtdiscountpercentage');
        handleFocus(event, discountPercentagefocus);
    });

    $(document).on('input', '#txtproductamount', function () {
        var productRow = $(this).closest(".product");
        var productAmount = parseFloat($(this).val());
        var discountAmountfocus = productRow.find('#txtdiscountamount');

        if (!isNaN(productAmount)) {
            productRow.find("#txtdiscountamount").val(0);
            productRow.find("#txtdiscountpercentage").val(0);
        }

        productRow.find("#productamount").val(productAmount.toFixed(2));
        fn_updateInvoiceProductAmount(productRow);
        fn_updateInvoiceTotals();
    }).on('keydown', '#txtproductamount', function (event) {
        var productRow = $(this).closest(".product");
        var discountAmountfocus = productRow.find('#txtdiscountamount');
        handleFocus(event, discountAmountfocus);
    });

    $(document).on('input', '#txtgst', function () {
        var row = $(this).closest('.product');
        fn_updateInvoiceProductAmount(row);
        fn_updateInvoiceTotals();
    }).on('keydown', '#txtgst', function (event) {
        if (event.key === 'Enter') {
            $(this).blur();
        }
    });
    $(document).on('input', '#txtigst', function () {
        var row = $(this).closest('.product');
        var gstvalue = $('#txtigst').val();
        if (gstvalue > 100) {
            toastr.warning("IGST% cannot be greater than 100%");
            $(this).val(100);
        }
        fn_updateInvoiceProductAmount(row);
        fn_updateInvoiceTotals();
    })
    $(document).on('focusout', '.product-quantity', function () {
        $(this).trigger('input');
    });
    $(document).on('input', '#cart-roundOff', debounce(function () {
        var roundoff = $('#cart-roundOff').val();
        if (isNaN(roundoff) || (roundoff < -0.99 || roundoff > 0.99)) {
            toastr.warning("Value must be between -0.99 and 0.99");
        }
        else {
            fn_updateInvoiceTotals();
        }
    }, 300));
});






function fn_getInvoiceVendorDetail(VendorId) {
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

function fn_getInvoiceCompanyDetail(CompanyId) {
    var CompanyId = CompanyId;
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

function preventInvoiceEmptyValue(input) {

    if (input.value === "") {

        input.value = 1;
    }
}

function fn_SearchItemDetailsById(ProductId) {
    var GetProductId = {
        ProductId: ProductId,
    }
    var form_data = new FormData();
    form_data.append("ProductId", JSON.stringify(GetProductId));

    $.ajax({
        url: '/Invoice/DisplayInvoiceProductDetailsListById',
        type: 'Post',
        datatype: 'json',
        data: form_data,
        processData: false,
        contentType: false,
        complete: function (Result) {

            if (Result.statusText === "success") {
                fn_InvoiceNewRow(Result.responseText);
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
var count = 0;
function fn_InvoiceNewRow(Result) {

    var newProductRow = $(Result);
    var productId = newProductRow.data('product-id');
    fn_InvoiceProductType(productId);
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
        fn_updateInvoiceTotals();
        fn_updateInvoiceRowNumbers();
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
function fn_updateInvoiceRowNumbers() {
    $(".product-id").each(function (index) {
        $(this).text(index + 1);
    });
}
function fn_InvoiceProductType(productId) {

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
function fn_updateInvoiceProductAmount(that) {
    var row = $(that);
    var productPrice = parseFloat(row.find("#txtproductamount").val());
    var hiddenproductPrice = parseFloat(row.find("#productamount").val());
    var quantity = parseFloat(row.find("#txtproductquantity").val());
    var discountprice = parseFloat(row.find("#txtdiscountamount").val());
    var AmtWithDisc = hiddenproductPrice - discountprice;

    var gst = parseFloat(row.find("#txtgst").val());
    var igst = parseFloat(row.find("#txtigst").val());

    var igstcount = (AmtWithDisc * igst / 100);
    var totalGst = (AmtWithDisc * quantity * gst) / 100 + igstcount;

    var TotalAmountAfterDiscount = AmtWithDisc * quantity + totalGst;

    row.find("#txtgstAmount").val(totalGst.toFixed(2));
    row.find("#txtproducttotalamount").val(TotalAmountAfterDiscount.toFixed(2));
}
function fn_updateInvoiceDiscount(that) {
    var row = $(that);
    var productPrice = parseFloat(row.find("#productamount").val());
    var quantity = parseFloat(row.find("#txtproductquantity").val());
    var discountprice = parseFloat(row.find("#txtdiscountamount").val());
    var discountPercentage = parseFloat(row.find("#txtdiscountpercentage").val());

    if (isNaN(discountprice)) {
        row.find("#txtdiscountamount").val(0);
        row.find("#txtdiscountpercentage").val(0);
        row.find("#txtproductamount").val(productPrice.toFixed(2));
        fn_updateInvoiceProductAmount(row);
        fn_updateInvoiceTotals();
        return;
    }

    if (discountPercentage == 0 && discountprice > 0) {
        var discountperbyamount = discountprice / productPrice * 100;
        row.find("#txtdiscountpercentage").val(discountperbyamount.toFixed(2));
    } else if (discountprice > 0 && discountPercentage > 0) {
        var discountperbyamount = discountprice / productPrice * 100;
        row.find("#txtdiscountpercentage").val(discountperbyamount.toFixed(2));
    }
    var AmountAfterDisc = productPrice - discountprice;
    row.find("#txtproductamount").val(AmountAfterDisc.toFixed(2));
    fn_updateInvoiceProductAmount(row);
    fn_updateInvoiceTotals();
}
function fn_UpdateInvoiceDiscountPercentage(that) {
    var row = $(that);
    var productPrice = parseFloat(row.find("#productamount").val());
    var quantity = parseFloat(row.find("#txtproductquantity").val());
    var discountprice = parseFloat(row.find("#txtdiscountamount").val());
    var discountPercentage = parseFloat(row.find("#txtdiscountpercentage").val());

    if (isNaN(discountPercentage)) {
        row.find("#txtdiscountamount").val(0);
        row.find("#txtdiscountpercentage").val(0);
        row.find("#txtproductamount").val(productPrice.toFixed(2));
        fn_updateInvoiceProductAmount(row);
        fn_updateInvoiceTotals();
        return;
    }

    if (discountprice == 0 && discountPercentage > 0) {
        discountprice = productPrice * discountPercentage / 100;
        row.find("#txtdiscountamount").val(discountprice.toFixed(2));
    } else if (discountprice > 0 && discountPercentage > 0) {
        discountprice = productPrice * discountPercentage / 100;
        row.find("#txtdiscountamount").val(discountprice.toFixed(2));
    }
    var AmountAfterDisc = productPrice - discountprice;
    row.find("#txtproductamount").val(AmountAfterDisc.toFixed(2));
    fn_updateInvoiceProductAmount(row);
    fn_updateInvoiceTotals();
}
function fn_updateInvoiceTotals() {
    var totalSubtotal = 0;
    var totalGst = 0;
    var totalAmount = 0;
    var TotalItemQuantity = 0;
    var TotalDiscount = 0;
    var roundoffvalue = $('#cart-roundOff').val();
    $(".product").each(function () {
        var row = $(this);
        var subtotal = parseFloat(row.find("#txtproductamount").val());
        var gst = parseFloat(row.find("#txtgstAmount").val());
        var totalquantity = parseFloat(row.find("#txtproductquantity").val());
        var discountprice = parseFloat(row.find("#txtdiscountamount").val());

        totalSubtotal += subtotal * totalquantity;
        totalGst += gst;
        totalAmount = totalSubtotal + totalGst - discountprice;
        TotalItemQuantity += totalquantity;
        TotalDiscount += discountprice * totalquantity;
    });
    $("#cart-subtotal").val(totalSubtotal.toFixed(2));
    $("#totalgst").val(totalGst.toFixed(2));
    $("#cart-total").val(totalAmount.toFixed(2));
    $("#TotalProductQuantity").text(TotalItemQuantity);
    $("#TotalProductPrice").html(totalSubtotal.toFixed(2));
    $("#TotalProductGST").html(totalGst.toFixed(2));
    $("#TotalProductAmount").html(totalAmount.toFixed(2));
    $("#TotalDiscountPrice").html(TotalDiscount.toFixed(2));
    $("#cart-discount").val(TotalDiscount.toFixed(2));
    if (roundoffvalue != 0) {

        var roundtotal = parseFloat(totalAmount) + parseFloat(roundoffvalue);
        $("#cart-total").val(roundtotal.toFixed(2))
    } else {
        $("#cart-total").val(totalAmount.toFixed(2));
    }
}
function fn_removeInvoiceItemRow(btn) {
    $(btn).closest("tr").remove();
    fn_updateInvoiceRowNumbers();
    fn_updateInvoiceTotals();
}
function tn_toggleInvoiceShippingAddress() {
    var checkbox = document.getElementById("hideShippingAddress");
    var shippingFields = document.getElementById("shippingAddressFields");

    if (checkbox.checked) {
        shippingFields.style.display = "none";
    } else {
        shippingFields.style.display = "block";
    }
}
function fn_PrintInvoicePage() {
    var printContents = document.getElementById('displayInvoiceDetail').innerHTML;
    var originalContents = document.body.innerHTML;
    document.body.innerHTML = printContents;
    document.body.innerHTML = originalContents;
    window.print();
}


$(document).ready(function () {
    function formatWithCommas(value) {
        return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }

    function updateInvoiceTotals() {
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

    updateInvoiceTotals();
});

function fn_AddInvoiceProductDescription(element) {

    var $row = $(element).closest('tr');
    var itemId = $row.find('#InvoiceProductDesBtn').data('item-id');

    var $container = $row.find(`#ProductDescriptionContainer[data-item-id='${itemId}']`);
    var $errorMessage = $row.find('#error-message');

    var lastInput = $container.find('input').last();

    if (lastInput.length > 0 && lastInput.val().trim() === '') {
        $errorMessage.show();
        return;
    } else {
        $errorMessage.hide();
    }

    var newDescriptionRow = `
       <div class="row align-items-center" data-item-id="${itemId}" style="margin-top: 10px;">
           <div class="col-sm-10" style="margin-right:-7px;">
               <input type="text" class="txtInvoiceProductDes form-control" placeholder="Description" />
           </div>
           <div class="col-sm-1">
               <div class="text-danger" style="cursor: pointer; font-size:20px;" onclick="removeInvoiceDescriptionRow(this)">x</div>
           </div>
       </div>
    `;

    $container.append(newDescriptionRow);
}


function removeInvoiceDescriptionRow(element) {
    var $row = $(element).closest('tr');
    $(element).closest('.row').remove();
    $row.find('#error-message').hide();
}

let InvoicedollarModal;

function checkInvoiceCurrencySelection() {
    const currencySelect = document.getElementById('Invoicecurrency-select');

    if (!InvoicedollarModal) {
        InvoicedollarModal = new bootstrap.Modal(document.getElementById('InvoiceDollarModal'));
    }

    if (currencySelect.value === "$") {
        InvoicedollarModal.show();
    }
}

function CloseInvoiceDollarModel() {

    if (InvoicedollarModal) {
        InvoicedollarModal.hide();
    }
}


function SaveInvoiceDollarAmount() {

    const InvoicedollarModal = document.getElementById('txtInvoiceDollarAmount').value;

    if (InvoicedollarModal == "") {
        toastr.warning("Emter DollarAmount!");
    }
    else {
        document.getElementById('InvoiceDollarAmount').value = InvoicedollarModal;
        CloseInvoiceDollarModel();
    }
}