
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
    today = getCommonDateformat(today);
    $("#textInvoiceDate").val(today);
    $("#textInvoiceDate").prop("disabled", true);
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
                toastr.error(Result.message);
            } else {
                window.location = '/Invoice/InvoiceDetails?OrderId=' + OrderId;
            }
        },
        error: function (xhr, status, error) {
            toastr.error('AJAX Error:', error);
            // Handle error here, for example, show an alert
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
                        toastr.error(Result.message);
                    }
                },
                error: function (xhr, status, error) {
                    toastr.error("An error occurred while processing your request.");  
                }
            });
        } else {
            if ($('#addnewproductlink tr').length == 0) {
                $("#spnitembutton").text("Please select product!");
            } else {
                $("#spnitembutton").text("");
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
                    if (Result.code == 200) {
                        Swal.fire({
                            title: Result.message,
                            icon: 'success',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'OK'
                        }).then(function () {
                            window.location = '/Invoice/InvoiceListView';
                        })
                    }
                    else {
                        toastr.error(Result.message);
                    }
                },
                error: function () {
                    Swal.fire({
                        title: "Can't delete invoice!",
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

            toastr.error(xhr.responseText);
        }
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
            toastr.error("Can't get Data");
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
                if (Result.code == 200) {
                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/Invoice/InvoiceListView';
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
                        buttons += '<a onclick="EditInvoiceDetails(\'' + full.invoiceNo + '\')" class="btn text-primary btndeletedoc">' +
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
