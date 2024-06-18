
$(document).ready(function () {
    $("#CreateInvoiceForm").validate({
        rules: {
            textVendorName: "required",
            textCompanyName: "required",
            txtpaymentmethod: "required",
            textDispatchThrough: "required",
        },
        messages: {
            textVendorName: "Select Vendor Name",
            textCompanyName: "Select Company Name",
            txtpaymentmethod: "Select Payment Method",
            textDispatchThrough: "Please Enter DispatchThrough",
        }
    });
});
function clearItemErrorMessage() {
    $("#spnitembutton").text("");
}
$(document).on("click", "#addItemButton", function () {
    clearItemErrorMessage();
});


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
function InsertInvoiceDetails() {

    if ($("#CreateInvoiceForm").valid()) {
        if ($('#addnewproductlink tr').length >= 1) {

            var ProductDetails = [];
            $(".product").each(function () {
                var orderRow = $(this);
                var productName = orderRow.find("#textProductName").text().trim();
                var productId = orderRow.find("#textProductId").val().trim();
                var objData = {
                    Product: productName,
                    ProductId: productId,
                    ProductType: orderRow.find("#txtPOProductType_" + orderRow.find("#textProductId").val()).val(),
                    Quantity: orderRow.find("#txtproductquantity").val(),
                    Hsn: orderRow.find("#txtHSNcode").val(),
                    Price: orderRow.find("#txtproductamount").val(),
                    GSTamount: orderRow.find("#txtgstAmount").val(),
                    Gst: orderRow.find("#txtgst").val(),
                    ProductTotal: orderRow.find("#txtproducttotalamount").val(),
                    DiscountAmount: orderRow.find("#txtdiscountamount").val(),
                    DiscountPer: orderRow.find("#txtdiscountpercentage").val(),
                };
                ProductDetails.push(objData);
            });
            var Invoicedetails = {
                ProjectId: $("#textProjectId").val(),
                InvoiceNo: $("#textInvoiceNo").val(),
                VandorId: $("#textVendorName").val(),
                CompanyId: $("#textCompanyName").val(),
                TotalGst: $("#totalgst").val(),
                Cgst: $("#textCGst").val(),
                Sgst: $("#textSGst").val(),
                Igst: $("#textIGst").val(),
                SubTotal: $("#cart-subtotal").val(),
                TotalAmount: $("#cart-total").val(),
                DispatchThrough: $("#textDispatchThrough").val(),
                BuyesOrderNo: $("#textBuysOrderNo").val(),
                BuyesOrderDate: $("#textBuysOrderDate").val(),
                InvoiceDate: $("#textInvoiceDate").val(),
                OrderStatus: $("#UnitTypeId").val(),
                PaymentMethod: $("#txtpaymentmethod").val(),
                PaymentStatus: $("#txtpaymenttype").val(),
                CreatedBy: $("#textCreatedById").val(),
                RoundOff: $('#cart-roundOff').val(),
                TotalDiscount: $('#cart-discount').val(),
                ShippingAddress: $('#hideShippingAddress').is(':checked') ? $('#textCompanyBillingAddress').val() : $('#textShippingAddress').val(),
                InvoiceDetails: ProductDetails,
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
function CheckProjectIdIsSelected() {
    var ProjectId = $('#textProjectId').val();
    if (ProjectId == "") {
        toastr.warning('Please Select Project!');
    } else {
        UpdateInvoiceDetails()
    }
}
function UpdateInvoiceDetails() {

    if ($("#CreateInvoiceForm").valid()) {
        if ($('#addnewproductlink tr').length >= 1) {

            var ProductDetails = [];
            $(".product").each(function () {
                var orderRow = $(this);
                var productName = orderRow.find("#textProductName").text().trim();
                var productId = orderRow.find("#textProductId").val().trim();
                var objData = {
                    Product: productName,
                    ProductId: productId,
                    ProductType: orderRow.find("#txtPOProductType_" + orderRow.find("#textProductId").val()).val(),
                    Quantity: orderRow.find("#txtproductquantity").val(),
                    Hsn: orderRow.find("#txtHSNcode").val(),
                    Price: orderRow.find("#txtproductamount").val(),
                    GSTamount: orderRow.find("#txtgstAmount").val(),
                    Gst: orderRow.find("#txtgst").val(),
                    ProductTotal: orderRow.find("#txtproducttotalamount").val(),
                    DiscountAmount: orderRow.find("#txtdiscountamount").val(),
                    DiscountPer: orderRow.find("#txtdiscountpercentage").val(),
                };
                ProductDetails.push(objData);
            });
            var Invoicedetails = {
                Id: $("#textInvoiceId").val(),
                ProjectId: $("#textProjectId").val(),
                InvoiceNo: $("#textInvoiceNo").val(),
                VandorId: $("#textVendorName").val(),
                CompanyId: $("#textCompanyName").val(),
                TotalGst: $("#totalgst").val(),
                Cgst: $("#textCGst").val(),
                Sgst: $("#textSGst").val(),
                Igst: $("#textIGst").val(),
                SubTotal: $("#cart-subtotal").val(),
                TotalAmount: $("#cart-total").val(),
                DispatchThrough: $("#textDispatchThrough").val(),
                BuyesOrderNo: $("#textBuysOrderNo").val(),
                BuyesOrderDate: $("#textBuysOrderDate").val(),
                InvoiceDate: $("#textInvoiceDate").val(),
                OrderStatus: $("#UnitTypeId").val(),
                PaymentMethod: $("#txtpaymentmethod").val(),
                PaymentStatus: $("#txtpaymenttype").val(),
                CreatedBy: $("#textCreatedById").val(),
                CreatedOn: $("#txtCreatedOn").val(),
                RoundOff: $('#cart-roundOff').val(),
                TotalDiscount: $('#cart-discount').val(),
                UpdatedBy: $("#textCreatedById").val(),
                ShippingAddress: $('#hideShippingAddress').is(':checked') ? $('#textCompanyBillingAddress').val() : $('#textShippingAddress').val(),
                InvoiceDetails: ProductDetails,
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


$(document).ready(function () {
    $("#generatePDF").click(function () {

        var pdf = new jsPDF();


        var htmlContent = "<h1 style='color: #3498db; text-align: center;'>PDF Generated with jQuery</h1>";
        htmlContent += "<p>This is a dynamically generated PDF content.</p>";


        pdf.fromHTML(htmlContent, 15, 15, {
            'width': 170,
            'elementHandlers': {
                '#ignorePDF': function (element, renderer) {
                    return true;
                }
            }
        });


        pdf.save("generated_pdf.pdf");
    });
});
$(document).ready(function () {
    var itemCounter = 0;

    $("#addItemBtn").click(function () {
        var newItem = $(".invoice-item:first").clone().removeAttr("style");
        newItem.find("input").val("");
        $("#invoiceItems").append(newItem);

        itemCounter++;
    });

    $("#SubmitInvoiceBtn").click(function (e) {

        e.preventDefault();

        var formData =
        {
            CompanyAddress: $("#idStatusCompany").val(),
            InvoiceNo: $("#InvoiceNo").val(),
            Date: $("#date-field").val(),
            PaymentStatus: $("#choices-payment-status").val(),
            TotalAmount: $("#totalamountInput").val(),
            BillingName: $("#billingName").val(),
            BillingAddress: $("#billingAddress").val(),
            BillingNumber: $("#billingPhoneno").val(),
            BillingTaxNumber: $("#billingTaxno").val(),
            ShippingName: $("#shippingName").val(),
            ShippingAddress: $("#shippingAddress").val(),
            ShippingNumber: $("#shippingPhoneno").val(),
            ShippingTaxNumber: $("#shippingTaxno").val(),
            ProductName: $("#productName-1").val(),
            ProductDetails: $("#productDetails-1").val(),
            HSN: $("#productHsn-1").val(),
            Price: $("#productRate-1").val(),
            Quantity: $("#product-qty-1").val(),
            PaymentMethod: $("#choices-payment-type").val(),
            CardHolderName: $("#cardholderName").val(),
            CardNumber: $("#cardNumber").val(),
        };


        $.ajax({
            url: '/Invoice/GenerateInvoice',
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response) {
                    generatePdf(response);
                }
                else {
                    toastr.error("Error generating invoice. please try again.");
                }
            },
            error: function () {
                toastr.error("An error occurred. please try again.");
            }
        });
    });
});

function deleteInvoice(InvoiceId) {
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

function generatePdf(data) {

    var pdf = new jsPDF();


    var htmlContent = '<div class="col-lg-12"><div class="d-flex"><div class="flex-grow-1"><div class="mt-sm-5 mt-4"><h6 class="text-muted text-uppercase fw-semibold">Address</h6><p class="text-muted mb-1" id="address-details">California, United States</p></div></div><div class="flex-shrink-0 mt-sm-0 mt-3"><h6><span class="text-muted fw-normal">Legal Registration No:</span><span id="legal-register-no">987654</span></h6><h6><span class="text-muted fw-normal">Email:</span><span id="email">velzon@themesbrand.com</span></h6><h6 class="mb-0"><span class="text-muted fw-normal">Contact No: </span><span id="contact-no"> +(01) 234 6789</span></h6></div></div></div ></div >';
    htmlContent += '<div class="col-lg-12"><div class="row g-3"><div class="col-lg-3 col-6"><p class="text-muted mb-2 text-uppercase fw-semibold">Invoice No</p><h5 class="fs-14 mb-0">#VL<span id="invoice-no">' + data.invoiceNo + '</span></h5></div><div class="col-lg-3 col-6"><p class="text-muted mb-2 text-uppercase fw-semibold">Date</p><h5 class="fs-14 mb-0"><span id="invoice-date">23 Nov, 2021</span> <small class="text-muted" id="invoice-time">' + data.date + '</small></h5></div><div class="col-lg-3 col-6"><p class="text-muted mb-2 text-uppercase fw-semibold">Payment Status</p><span class="badge bg-success-subtle text-success fs-11" id="payment-status">' + data.paymentStatus + '</span></div><div class="col-lg-3 col-6"><p class="text-muted mb-2 text-uppercase fw-semibold">Total Amount</p><h5 class="fs-14 mb-0">$<span id="total-amount">' + data.totalAmount + '</span></h5></div></div></div >';
    pdf.fromHTML(htmlContent, 15, 15, {
        'width': 170,
        'elementHandlers': {
            '#ignorePDF': function (element, renderer) {
                return true;
            }
        }
    });

    pdf.save("generated_pdf.pdf");
}


$('#idStatus').change(function () {
    if ($("#idStatus").val() == "Selse") {
        $("#companyaddress").show();
        $("#fullcompanyaddress").show();
        $("#txtvendorname1").show();
        $("#vendername").hide();
        $("#CompanyAddress1").hide();
        $("#fullcompanyaddress1").hide();
    }
    if ($("#idStatus").val() == "Purchase") {
        $("#vendername").show();
        $("#CompanyAddress1").show();
        $("#fullcompanyaddress1").show();
        $("#companyaddress").hide();
        $("#fullcompanyaddress").hide();
        $("#txtvendorname1").hide();
    }
});



function downloadPDF() {

    var htmlContent = document.getElementById('printableContent').innerHTML;
    var form_data = new FormData();
    form_data.append("DOWNLOADINVOICE", htmlContent);

    $.ajax({
        url: '/Invoice/DownloadPdf',
        type: 'POST',
        data: form_data,
        contentType: false,
        processData: false,
        success: function (data) {

            console.log('PDF generated successfully:', data);

            window.location.href = data;
        },
        error: function (xhr, status, error) {

            toastr.error(xhr.responseText);
        }
    });
}


var datas = userPermissions
$(document).ready(function () {
    function data(datas) {
        userPermission = datas;
        AllInvoiceList(userPermission);
    }
    function AllInvoiceList(userPermission) {
        var userPermissionArray = [];
        userPermissionArray = JSON.parse(userPermission);

        var canEdit = false;
        var canDelete = false;

        for (var i = 0; i < userPermissionArray.length; i++) {
            var permission = userPermissionArray[i];
            if (permission.formName == "InvoiceListView") {
                canEdit = permission.edit;
                canDelete = permission.delete;
                break;
            }
        }

        var columns = [
            {
                "data": "invoiceNo",
                "name": "InvoiceNo",
                "render": function (data, type, full) {
                    return '<h5 class="fs-15"><a href="/Invoice/InvoiceDetails?InvoiceId=' + full.id + '" class="fw-medium link-primary">' + full.invoiceNo + '</a></h5>';
                }
            },
            { "data": "vendorName", "name": "VendorName", "className": "text-center" },
            { "data": "projectName", "name": "ProjectName", "className": "text-center" },
            { "data": "totalAmount", "name": "TotalAmount", "className": "text-center" }
        ];

        if (canEdit || canDelete) {
            columns.push({
                "data": null,
                "orderable": false,
                "searchable": false,
                "render": function (data, type, full) {
                    var buttons = '<ul class="list-inline hstack gap-2 mb-0">';

                    if (canEdit) {
                        buttons += '<a href="/Invoice/CreateInvoice?Id=' + full.id + '" class="btn text-primary btndeletedoc">' +
                            '<i class="fa-regular fa-pen-to-square"></i></a>';
                    }

                    if (canDelete) {
                        buttons += '<a onclick="deleteInvoice(\'' + full.id + '\')" class="btn text-danger btndeletedoc">' +
                            '<i class="fas fa-trash"></i></a>';
                    }

                    buttons += '</ul>';
                    return buttons;
                }
            });
        }

        $('#invoiceTable').DataTable({
            processing: false,
            serverSide: true,
            filter: true,
            "bDestroy": true,
            ajax: {
                type: "POST",
                url: '/Invoice/GetInvoiceListView',
                dataType: 'json'
            },
            columns: columns,
            columnDefs: [{
                "defaultContent": "",
                "targets": "_all"
            }]
        });
    }
    data(datas);
});

function createInvoice() {
    if ($("#txtInvoice").val() == "") {
        Swal.fire({
            title: "Kindly select project on dashboard.",
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

    fn_GetInvoiceVendorNameList()
    $('#textVendorName').change(function () {
        fn_getInvoiceVendorDetail($(this).val());
    });
    fn_GetInvoiceCompanyNameList()
    $('#textCompanyName').change(function () {
        fn_getInvoiceCompanyDetail($(this).val());
    });
    fn_GetInvoiceProductDetailsList()
    fn_GetInvoicePaymentMethodList()
    fn_GetInvoicePaymentTypeList()
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

    $(document).on('input', '#txtdiscountpercentage', debounce(function () {debugger
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

function fn_GetInvoiceVendorNameList() {
    $.ajax({
        url: '/ProductMaster/GetVendorsNameList',
        success: function (result) {
            var selectedValue = $('#textVendorName').find('option:first').val();
            $.each(result, function (i, data) {
                if (data.id !== selectedValue) {
                    $('#textVendorName').append('<Option value=' + data.id + '>' + data.vendorCompany + '</Option>')
                }
            });
        }
    });
}
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
function fn_GetInvoiceCompanyNameList() {
    $.ajax({
        url: '/Company/GetCompanyNameList',
        success: function (result) {
            var selectedValue = $('#textCompanyName').find('option:first').val();
            $.each(result, function (i, data) {
                if (data.id !== selectedValue) {
                    $('#textCompanyName').append('<Option value=' + data.id + '>' + data.compnyName + '</Option>')
                }
            });
        }
    });
}
function fn_getInvoiceCompanyDetail(CompanyName) {
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
function fn_GetInvoicePaymentMethodList() {

    $.ajax({
        url: '/PurchaseOrderMaster/GetPaymentMethodList',
        success: function (result) {
            var selectedValue = $('#txtpaymentmethod').find('option:first').val();
            $.each(result, function (i, data) {
                if (data.id != selectedValue) {
                    $('#txtpaymentmethod').append('<Option value=' + data.id + '>' + data.paymentMethod + '</Option>')
                }

            });
        }
    });
}
function fn_GetInvoicePaymentTypeList() {
    $.ajax({
        url: '/ExpenseMaster/GetPaymentTypeList',
        success: function (result) {
            var selectedValue = $('#txtpaymenttype').find('option:first').val();
            $.each(result, function (i, data) {
                $('#textPaymentMethod').append('<Option value=' + data.id + '>' + data.type + '</Option>')
                if (data.id != selectedValue) {

                    $('#txtpaymenttype').append('<Option value=' + data.id + '>' + data.type + '</Option>')
                }
            });
        }
    });
}
function preventInvoiceEmptyValue(input) {

    if (input.value === "") {

        input.value = 1;
    }
}
function fn_OpenAddInvoiceproductmodal() {

    $('#mdProductSearch').val('');
    $('#mdInvoiceproductModal').modal('show');
}
function fn_GetInvoiceProductDetailsList() {
    var searchText = $('#mdProductSearch').val();

    $.get("/Invoice/GetInvoiceAllProductList", { searchText: searchText })
        .done(function (result) {
            $("#mdlistofInvoiceItem").html(result);
        })
        .fail(function (xhr, status, error) {
            console.error("Error:", error);
        });
}
function SearchInvoiceProductDetailsById(ProductId) {
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

    var totalGst = (AmtWithDisc * quantity * gst) / 100;

    var TotalAmountAfterDiscount = AmtWithDisc * quantity + totalGst;

    row.find("#txtgstAmount").val(totalGst.toFixed(2));
    row.find("#txtproducttotalamount").val(TotalAmountAfterDiscount.toFixed(2));
}
function fn_updateInvoiceDiscount(that) {debugger
    var row = $(that);
    var productPrice = parseFloat(row.find("#productamount").val());
    var quantity = parseFloat(row.find("#txtproductquantity").val());
    var discountprice = parseFloat(row.find("#txtdiscountamount").val());
    var discountPercentage = parseFloat(row.find("#txtdiscountpercentage").val());

    if (isNaN(discountprice)) {
        row.find("#txtdiscountamount").val(0);
        row.find("#txtdiscountpercentage").val(0);
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

    fn_updateInvoiceProductAmount(row);
    fn_updateInvoiceTotals();
}

function fn_UpdateInvoiceDiscountPercentage(that) {debugger
    var row = $(that);
    var productPrice = parseFloat(row.find("#productamount").val());
    var quantity = parseFloat(row.find("#txtproductquantity").val());
    var discountprice = parseFloat(row.find("#txtdiscountamount").val());
    var discountPercentage = parseFloat(row.find("#txtdiscountpercentage").val());

    if (isNaN(discountPercentage)) {
        row.find("#txtdiscountamount").val(0);
        row.find("#txtdiscountpercentage").val(0);
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
        totalAmount = totalSubtotal + totalGst;
        TotalItemQuantity += totalquantity;
        TotalDiscount += discountprice;
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