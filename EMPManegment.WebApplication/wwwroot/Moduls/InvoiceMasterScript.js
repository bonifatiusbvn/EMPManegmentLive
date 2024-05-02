
$(document).ready(function () {

    GetAllVendorData()
    GetAllTransactionData()
    AllInvoiceList()

});
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

$(document).ready(function () {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();

    today = yyyy + '-' + mm + '-' + dd;
    $("#textInvoiceDate").val(today);
    $("#textInvoiceDate").prop("disabled", true);
});


function GetInvoiceDetailsByOrderId(OrderId) {
    $.ajax({
        url: '/Invoice/GetInvoiceDetailsByOrderId/?OrderId=' + OrderId,
        type: 'GET',
        success: function (result) {
            if (result.code == 400) {
                Swal.fire({
                    title: result.message,
                    icon: result.icone,
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                });
            } else {
                window.location = '/Invoice/InvoiceDetails/?OrderId=' + OrderId;
            }
        },
        error: function (xhr, status, error) {
            console.error('AJAX Error:', error);
            // Handle error here, for example, show an alert
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
                Swal.fire({
                    title: result.message,
                    icon: result.icone,
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                });
            } else {
                window.location = '/Invoice/DisplayInvoiceDetails?OrderId=' + OrderId;
            }
        },
        error: function (xhr, status, error) {
            console.error('AJAX Error:', error);
            // Handle error here, for example, show an alert
            alert('An error occurred while fetching data.');
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
                    ProductType: orderRow.find("#textProductType").val(),
                    Quantity: orderRow.find("#txtproductquantity").val(),
                    Price: orderRow.find("#txtproductamount").val(),
                    GSTamount: orderRow.find("#txtgstAmount").val(),
                    Gst: orderRow.find("#txtgst").val(),
                    ProductTotal: orderRow.find("#txtproducttotalamount").val(),
                };
                ProductDetails.push(objData);
            });
            var Invoicedetails = {
                ProjectId: $("#textProjectId").val(),
                InvoiceNo: $("#textInvoiceNo").val(),
                VandorId: $("#textVendorName").val(),
                CompanyName: $("#textCompanyName").val(),
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
                            window.location = '/Invoice/InvoiceListView';
                        });
                    }
                    else {
                        Swal.fire({
                            title: Result.message,
                            icon: 'warning',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'OK'
                        });
                    }
                },
                error: function (xhr, status, error) {
                    Swal.fire({
                        title: 'Error',
                        text: 'An error occurred while processing your request.',
                        icon: 'error',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    });
                }
            });
        } else {
            if ($('#addnewproductlink tr').length == 0) {
                $("#spnitembutton").text("Please Select Product!");
            } else {
                $("#spnitembutton").text("");
            }
        }
    }
    else {
        Swal.fire({
            title: "Kindly fill all data fields",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        })
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
        newItem.find("input").val(""); // Clear input values for the new item
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
                } else {
                    alert("Error generating invoice. Please try again.");
                }
            },
            error: function () {
                alert("An error occurred. Please try again.");
            }
        });
    });

});
function deleteInvoice(InvoiceId) {
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
                        window.location = '/Invoice/InvoiceListView';
                    })
                },
                error: function () {
                    Swal.fire({
                        title: "Can't Delete Invoice!",
                        icon: 'warning',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/Invoice/InvoiceListView';
                    })
                }
            })
        } else if (result.dismiss === Swal.DismissReason.cancel) {

            Swal.fire(
                'Cancelled',
                'Invoice Have No Changes.!!😊',
                'error'
            );
        }
    });
}

function generatePdf(data) {

    var pdf = new jsPDF();


    var htmlContent = '<div class="col-lg-12"><div class="d-flex"><div class="flex-grow-1"><div class="mt-sm-5 mt-4"><h6 class="text-muted text-uppercase fw-semibold">Address</h6><p class="text-muted mb-1" id="address-details">California, United States</p></div></div><div class="flex-shrink-0 mt-sm-0 mt-3"><h6><span class="text-muted fw-normal">Legal Registration No:</span><span id="legal-register-no">987654</span></h6><h6><span class="text-muted fw-normal">Email:</span><span id="email">velzon@themesbrand.com</span></h6><h6 class="mb-0"><span class="text-muted fw-normal">Contact No: </span><span id="contact-no"> +(01) 234 6789</span></h6></div></div></div ></div >';
    htmlContent += '<div class="col-lg-12"><div class="row g-3"><div class="col-lg-3 col-6"><p class="text-muted mb-2 text-uppercase fw-semibold">Invoice No</p><h5 class="fs-14 mb-0">#VL<span id="invoice-no">' + data.invoiceNo + '</span></h5></div><div class="col-lg-3 col-6"><p class="text-muted mb-2 text-uppercase fw-semibold">Date</p><h5 class="fs-14 mb-0"><span id="invoice-date">23 Nov, 2021</span> <small class="text-muted" id="invoice-time">' + data.date + '</small></h5></div><div class="col-lg-3 col-6"><p class="text-muted mb-2 text-uppercase fw-semibold">Payment Status</p><span class="badge bg-success-subtle text-success fs-11" id="payment-status">' + data.paymentStatus + '</span></div><div class="col-lg-3 col-6"><p class="text-muted mb-2 text-uppercase fw-semibold">Total Amount</p><h5 class="fs-14 mb-0">$<span id="total-amount">' + data.totalAmount + '</span></h5></div></div></div >';
    // Add the HTML content to the PDF with styles
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

            console.error(xhr.responseText);
        }
    });
}

function GetAllVendorData() {

    $('#VendorTableData').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "Post",
            url: '/Invoice/GetVendorList',
            dataType: 'json'
        },
        columns: [
            {
                "render": function (data, type, full) {

                    return '<h5 class="fs-15"><a href="/Invoice/VendorInvoiceListView/?Vid=' + full.vid + '" class="fw-medium link-primary">' + full.vendorCompany; '</a></h5>';
                }
            },
            {
                "data": null,
                "name": "VendorFullName",
                "render": function (data, type, full, meta) {
                    return full.vendorFirstName + ' ' + full.vendorLastName;
                }
            },
            { "data": "vendorEmail", "name": "VendorEmail" },
            { "data": "vendorPhone", "name": "VendorPhone" },
            { "data": "vendorAddress", "name": "VendorAddress" },
        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all",
        }]
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
function EditInvoiceDetails(InvoiceNo) {

    $.ajax({
        url: '/Invoice/EditInvoiceDetails?InvoiceNo=' + InvoiceNo,
        type: "Get",
        contentType: 'application/json;charset=utf-8;',
        dataType: 'json',
        success: function (response) {

            $('#UpdateInvoiceModel').modal('show');
            $('#txtinvoiceno').html(response.invoiceNo);
            $('#txtinvoicedate').val(response.invoiceDate);
            $('#txtid').val(response.id);
            $('#txtcompanyname').val(response.companyName);
            $('#txtamount').val(response.totalAmount);
            $('#txtpaymentmethod').val(response.paymentMethod);
            $('#txtstatus').val(response.status);
        },
        error: function () {
            alert('Data not found');
        }
    });
}

function UpdateInvoiceDetails() {
    if ($('#UpdateInvoiceDetailsForm').valid()) {
        var formData = new FormData();
        formData.append("InvoiceNo", $("#txtinvoiceno").val());
        formData.append("InvoiceDate", $("#txtinvoicedate").val());
        formData.append("Id", $("#txtid").val());
        formData.append("companyName", $("#txtcompanyname").val());
        formData.append("TotalAmount", $("#txtamount").val());
        formData.append("PaymentMethod", $("#txtpaymentmethod").val());
        formData.append("Status", $("#txtstatus").val());

        $.ajax({
            url: '/Invoice/UpdateInvoiceDetails',
            type: 'Post',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (Result) {
                if (Result.message != null) {
                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/Invoice/InvoiceListView';
                    });
                }
            }
        })
    }
    else {
        Swal.fire({
            title: "Kindly Fill All Details",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        })
    }
}

$(document).ready(function () {

    $("#UpdateInvoiceDetailsForm").validate({
        rules: {
            txtinvoicedate: "required",
            txtamount: "required",
            txtpaymentmethod: "required",
            txtstatus: "required",
            txtcompanyname: "required",
        },
        messages: {
            txtinvoicedate: "Please emter invoice date",
            txtamount: "Please enter invoice Amount",
            txtpaymentmethod: "Please Enter Payment method",
            txtstatus: "Plese enter status",
            txtcompanyname: "Plese enter Company name",
        }
    })
    $("#updatedetailbtn").on('click', function () {
        $("#UpdateInvoiceDetailsForm").validate();
    });
});

function GetAllTransactionData() {
    $('#transactionTable').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "Post",
            url: '/Invoice/GetAllTransactiondata',
            dataType: 'json'
        },
        autoWidth: false,
        columns: [
            { "data": "vendorName", "name": "VendorName" },
            {
                "data": "date", "name": "Date",
                "render": function (data, type, row) {
                    var date = new Date(data);
                    return date.toLocaleDateString();
                }
            },
            { "data": "paymentMethodName", "name": "PaymentMethodName" },
            { "data": "paymentTypeName", "name": "PaymentTypeName" },
            { "data": "creditDebitAmount", "name": "CreditDebitAmount" },
            { "data": "pendingAmount", "name": "PendingAmount" },
            { "data": "vendorAddress", "name": "VendorAddress" },
        ],
        columnDefs: [
            {
                targets: 4,
                render: function (data, type, row) {
                    var amountClass = parseFloat(data) < 0 ? 'text-success' : 'text-success';
                    return '<span class="' + amountClass + '">' + data + '</span>';
                }
            },
            {
                targets: 5,
                render: function (data, type, row) {
                    var amountClass = parseFloat(data) < 0 ? 'text-danger' : 'text-danger';
                    return '<span class="' + amountClass + '">' + data + '</span>';
                }
            }
        ],
        createdRow: function (row, data, dataIndex) {
            $(row).addClass('text-muted');
            var htmlContent = '<td class="id" style="display:none;"><a href="javascript:void(0);" class="fw-medium link-primary">#VZ001</a></td>';
            htmlContent += '<td><div class="avatar-xs"><div class="avatar-title bg-danger-subtle text-danger rounded-circle fs-16"><i class="ri-arrow-right-up-fill"></i></div></div></td>';
            htmlContent += '<td class="date">' + data.vendorName + '<small class="text-muted"></small></td>';
            htmlContent += '<td class="form_name">' + data.date + '</td>';
            htmlContent += '<td class="to_name">' + data.paymentMethodName + '</td>';
            htmlContent += '<td class="to_name">' + data.paymentTypeName + '</td>';
            htmlContent += '<td class="to_name text-success">' + data.creditDebitAmount + '</td>';
            htmlContent += '<td class="to_name text-danger">' + data.pendingAmount + '</td>';
            htmlContent += '<td class="status"><span class="badge bg-primary-subtle text-primary fs-11"><i class="ri-time-line align-bottom"></i> Processing</span></td>';

            $(row).html(htmlContent);
        }
    });
}

function AllInvoiceList() {
    $('#invoiceTable').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "POST",
            url: '/Invoice/GetInvoiceListView',
            dataType: 'json',
        },
        columns: [
            {
                "render": function (data, type, full) {
                    return '<h5 class="fs-15"><a href="/Invoice/DisplayInvoiceDetails?OrderId=' + full.orderId + '" class="fw-medium link-primary">' + full.invoiceNo; '</a></h5>';
                }
            },
            {
                "data": "vendorName", "name": "VendorName",
                "className": "text-center"
            },
            {
                "data": "projectName", "name": "ProjectName",
                "className": "text-center"
            },
            {
                "data": "orderId", "name": "OrderId",
                "className": "text-center"
            },
            {
                "data": "totalAmount", "name": "TotalAmount",
                "className": "text-center"
            },
            {
                "data": "Action", "name": "Action",
                render: function (data, type, full) {
                    return ('<li class="btn list-inline-item edit" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="Edit"><a onclick="EditInvoiceDetails(\'' + full.invoiceNo + '\')"><i class="ri-pencil-fill fs-16"></i></a></li><li class="btn text-danger list-inline-item delete" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="Delete" style="margin-left:12px;"><a onclick="deleteInvoice(\'' + full.id + '\')"><i class="fas fa-trash"></i></a></li>');
                }
            },
        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all",
        }]
    });
}









