
$(document).ready(function () {
    GetInvoiceNoList()
    GetVendorName()
    GetAllVendorData()
});
function GetInvoiceNoList() {
    $.ajax({
        url: '/Invoice/GetInvoiceNoList',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#txtInvoiceNo').append('<Option value=' + data.id + '>' + data.invoiceNo + '</Option>')
            });
        }
    });
}
function GetVendorName() {

    $.ajax({
        url: '/ProductMaster/GetVendorsNameList',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#txtvendorname').append('<Option value=' + data.id + '>' + data.vendorCompany + '</Option>')
                $('#txtvendorname1').append('<Option value=' + data.id + '>' + data.vendorCompany + '</Option>')
            });
        }
    });
}
function selectvendorId() {
    document.getElementById("txtvendorTypeid").value = document.getElementById("txtvendorname").value;
    document.getElementById("txtvendorTypeid1").value = document.getElementById("txtvendorname1").value;
}

$(document).ready(function () {
    $('#txtvendorname').change(function () {
        var VendorTypeId = $("#txtvendorname").val();
        $.ajax({
            url: '/Vendor/GetVendorDetailsById?VendorId=' + VendorTypeId,
            type: 'Post',
            success: function (result) {
                $('#vendorcompanyaddress').empty();
                $('#vendorcompanyaddress').append('<div><label>Vendor Company Address</label></div><div><div class="form-control"><h6 class="mb-0"><span class="text-muted fw-normal">' + result.vendorCompany + '</span></h6><h6 class="mb-0"><span class="text-muted fw-normal">Address : </span><span id="contact-no">' + result.vendorAddress + '</span></h6><h6 class="mb-0"><span class="text-muted fw-normal">Email: </span><span>' + result.vendorCompanyEmail + '</span></h6><h6 class="mb-0"><span class="text-muted fw-normal">Contact No: </span><span id="contact-no"> ' + result.vendorCompanyNumber + '</span></h6></div></div>');
            }
        });
    });

    $('#txtvendorname1').change(function () {
        var VendorTypeId = $("#txtvendorname1").val();
        $.ajax({
            url: '/Vendor/GetVendorDetailsById?VendorId=' + VendorTypeId,
            type: 'Post',
            success: function (result) {
                $('#vendorcompanyaddress1').empty();
                $('#vendorcompanyaddress1').append('<div><label>Vendor Company Address</label></div><div><div class="form-control"><h6 class="mb-0"><span class="text-muted fw-normal">' + result.vendorCompany + '</span></h6><h6 class="mb-0"><span class="text-muted fw-normal">Address : </span><span id="contact-no">' + result.vendorAddress + '</span></h6><h6 class="mb-0"><span class="text-muted fw-normal">Email: </span><span>' + result.vendorCompanyEmail + '</span></h6><h6 class="mb-0"><span class="text-muted fw-normal">Contact No: </span><span id="contact-no"> ' + result.vendorCompanyNumber + '</span></h6></div></div>');
            }
        });
    });
});

function GetInvoiceDetailsByOrderId(OrderId) {debugger
    $.ajax({
        url: '/Invoice/GetInvoiceDetailsByOrderId/?OrderId=' + OrderId,
        type: 'GET',
        success: function (result) {debugger
            if (result.code == 400) {
                Swal.fire({
                    title: result.message,
                    icon: result.icone,
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                });
            } else {debugger
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

function InsertInvoiceDetails() {debugger
function InsertInvoiceDetails() {
    debugger

    var objData = {
        InvoiceNo: $("#txtinvoiceid").val(),
        CreatedBy: $("#txtuserid").val(),
        ProjectId: $("#txtprojectid").val(),
        BuyesOrderDate: document.getElementById("txtdate").innerHTML,
        OrderId: document.getElementById("txtorderid").innerHTML,
        InvoiceType: document.getElementById("txtinvoicetype").innerHTML,
        VandorId: document.getElementById("txtvendorid").innerText,
        DispatchThrough: document.getElementById("txtshippingcompany").innerText,
        Destination: document.getElementById("txtshippingaddress").innerText,
        TotalAmount: document.getElementById("txttotalamount").innerText,
        TotalGst: document.getElementById("txttotalgst").innerText,
    };
    var form_data = new FormData();
    form_data.append("INVOICEDETAILS", JSON.stringify(objData));
    debugger
    $.ajax({
        url: '/Invoice/InsertInvoiceDetails',
        type: 'POST',
        data: form_data,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (result) {
            debugger
            if (result.message == "Invoice Generated successfully!") {
                Swal.fire({
                    title: result.message,
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                }).then(function () {
                    window.location = '/OrderMaster/CreateOrder';
                });
            }
            else {
                debugger
                Swal.fire({
                    title: result.message,
                    icon: result.icone,
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                })
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
});


$('#idStatus').change(function () {
    if ($("#idStatus").val() == "Selse") {
        $("#idStatusCompany").show();
        $("#Companyname").show();
        $("#idCompany").hide();
        $("#CompanCCP").hide();
        $("#vender").hide();
        $("#txtvendorname1").show();
        $("#vendorcompanyaddress1").show();
        $("#vendorcompanyaddress").hide();
        cleartextBox();
    }
    if ($("#idStatus").val() == "Purchase") {
        $("#idCompany").show();
        $("#CompanCCP").show();
        $("#idStatusCompany").hide();
        $("#Companyname").hide();
        $("#txtvendorname1").hide();
        $("#vender").show();
        $("#vendorcompanyaddress").show();
        $("#vendorcompanyaddress1").hide();
        cleartextBox();
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
            debugger
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
                    return '<h5 class="fs-15"><a href="/Invoice/VendorInvoiceListView/?Id=' + full.id + '" class="fw-medium link-primary">' + full.vendorCompany; '</a></h5>';
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






