
$(document).ready(function () {
    GetInvoiceNoList()
    //$("#idStatusCompany").hide();
    //$("#idCompany").hide();
    //$("#Companyname").hide();
    //$("#CompanCCP").hide();
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

$(document).ready(function () {
    $("#generatePDF").click(function () {
        // Create a new jsPDF instance
        var pdf = new jsPDF();

        // Generate HTML content dynamically using jQuery
        var htmlContent = "<h1 style='color: #3498db; text-align: center;'>PDF Generated with jQuery</h1>";
        htmlContent += "<p>This is a dynamically generated PDF content.</p>";

        // Add the HTML content to the PDF with styles
        pdf.fromHTML(htmlContent, 15, 15, {
            'width': 170,
            'elementHandlers': {
                '#ignorePDF': function (element, renderer) {
                    return true;
                }
            }
        });

        // Save or download the PDF
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


//function generatePdf(data) {debugger
//    var doc = new jsPDF();
//    doc.text("Invoice Details", 20, 10);
//    var yPos = 20;
//    debugger
//    yPos += 10; debugger
//    doc.text("InvoiceNo: " + data.invoiceNo, 20, yPos);
//    yPos += 10;
//    doc.text("CompanyAddress: " + data.companyAddress, 20, yPos);
//    yPos += 10; debugger
//    doc.text("Date: " + data.date, 20, yPos);
//    yPos += 10;
//    doc.text("BillingName: " + data.billingName, 20, yPos);
//    yPos += 10;
//    doc.text("BillingAddress: " + data.billingAddress, 20, yPos);
//    yPos += 10;
//    doc.text("BillingNumber: " + data.billingNumber, 20, yPos);
//    yPos += 10;
//    doc.text("BillingTaxNumber: " + data.billingTaxNumber, 20, yPos);
//    yPos += 10;
//    doc.text("ShippingName: " + data.shippingName, 20, yPos);
//    yPos += 10;
//    doc.text("ShippingAddress: " + data.shippingAddress, 20, yPos);
//    yPos += 10;
//    doc.text("ShippingTaxNumber: " + data.shippingTaxNumber, 20, yPos);
//    yPos += 10;
//    doc.text("ProductName: " + data.productName, 20, yPos);
//    yPos += 10;
//    doc.text("ProductDetails: " + data.productDetails, 20, yPos);
//    yPos += 10;
//    doc.text("HSN: " + data.hsn, 20, yPos);
//    yPos += 10;
//    doc.text("Price: " + data.price, 20, yPos);
//    yPos += 10;
//    doc.text("Quantity: " + data.quantity, 20, yPos);
//    yPos += 10;
//    doc.text("TotalAmount: " + data.totalAmount, 20, yPos);
//    yPos += 10;
//    doc.text("PaymentMethod: " + data.paymentMethod, 20, yPos);
//    yPos += 10;
//    doc.text("CardHolderName: " + data.cardHolderName, 20, yPos);
//    yPos += 10;
//    doc.text("PaymentMethod: " + data.cardNumber, 20, yPos);
//    debugger
//    doc.save("invoice.pdf");
//}


//var formData = $(this).serializeArray();


//for (var i = 0; i < data.length; i++) {
//    debugger
//    yPos += 10; debugger
//    doc.text("InvoiceNo: " + data[i].invoiceNo, 20, yPos);
//    yPos += 10;
//    doc.text("CompanyAddress: " + data[i].companyAddress, 20, yPos);
//    yPos += 10; debugger
//    doc.text("Date: " + data[i].date, 20, yPos);
//    yPos += 10;
//    doc.text("PaymentStatus: $" + data[i].paymentStatus, 20, yPos);
//    yPos += 10;
//    doc.text("BillingName: " + data[i].billingName, 20, yPos);
//    yPos += 10;
//    doc.text("BillingAddress: " + data[i].billingAddress, 20, yPos);
//    yPos += 10;
//    doc.text("BillingNumber: " + data[i].billingNumber, 20, yPos);
//    yPos += 10;
//    doc.text("BillingTaxNumber: " + data[i].billingTaxNumber, 20, yPos);
//    yPos += 10;
//    doc.text("ShippingName: " + data[i].shippingName, 20, yPos);
//    yPos += 10;
//    doc.text("ShippingAddress: " + data[i].shippingAddress, 20, yPos);
//    yPos += 10;
//    doc.text("ShippingTaxNumber: " + data[i].shippingTaxNumber, 20, yPos);
//    yPos += 10;
//    doc.text("ProductName: " + data[i].productName, 20, yPos);
//    yPos += 10;
//    doc.text("ProductDetails: " + data[i].productDetails, 20, yPos);
//    yPos += 10;
//    doc.text("HSN: $" + data[i].hsn, 20, yPos);
//    yPos += 10;
//    doc.text("Price: $" + data[i].price, 20, yPos);
//    yPos += 10;
//    doc.text("Quantity: $" + data[i].quantity, 20, yPos);
//    yPos += 10;
//    doc.text("TotalAmount: $" + data[i].totalAmount, 20, yPos);
//    yPos += 10;
//    doc.text("PaymentMethod: $" + data[i].paymentMethod, 20, yPos);
//    yPos += 10;
//    doc.text("CardHolderName: " + data[i].cardHolderName, 20, yPos);
//    yPos += 10;
//    doc.text("PaymentMethod: " + data[i].cardNumber, 20, yPos);
//}

//function DownloadInvoice() {

//    //var Invoice = {
//    //    CompanyAddress : $("#companyAddress").val(),
//    //    PostalCode: $("#companyaddpostalcode").val(),
//    //    LegalRegistrationNo: $("#registrationNumber").val(),
//    //    EmailAddress : $("#companyEmail").val(),
//    //    Website: $("#companyWebsite").val(),
//    //    PhoneNo: $("#companyContactno").val(),
//    //    InvoiceNo: $("#invoicenoInput").val(),
//    //    Date: $("#date-field").val(),
//    //    PaymentStatus: $("#choices-payment-status").val(),
//    //    BillingName: $("#billingName").val(),
//    //    BillingAddress: $("#billingAddress").val(),
//    //    BillingPhoneNo: $("#billingPhoneno").val(),
//    //    BillingTaxNo: $("#billingTaxno").val(),
//    //    ShippingName: $("#shippingName").val(),
//    //    ShippingAddress: $("#shippingAddress").val(),
//    //    ShippingPhoneNo: $("#shippingPhoneno").val(),
//    //    ShippingTaxNo: $("#shippingTaxno").val(),
//    //    ProductName: $("#productName-1").val(),
//    //    ProductDetails: $("#productDetails-1").val(),
//    //    Quantity: $("#product-qty-1").val(),
//    //    Amount: $("#productPrice-1").val(),
//    //    Subtotal: $("#cart-subtotal").val()
//    //}
//    //var form_data = new FormData();
//    //form_data.append("ViewInvoice", JSON.stringify(Invoice));
//    var formData = new FormData();
//    formData.append("CompanyAddress", $("#companyAddress").val());
//    formData.append("PostalCode", $("#companyaddpostalcode").val());
//    formData.append("LegalRegistrationNo", $("#registrationNumber").val());
//    formData.append("EmailAddress", $("#companyEmail").val());
//    formData.append("Website", $("#companyWebsite").val());
//    formData.append("PhoneNo", $("#companyContactno").val());
//    formData.append("InvoiceNo", $("#invoicenoInput").val());
//    formData.append("Date", $("#date-field").val());
//    formData.append("PaymentStatus", $("#choices-payment-status").val());
//    formData.append("BillingName", $("#billingName").val());
//    formData.append("BillingAddress", $("#billingAddress").val());
//    formData.append("BillingPhoneNo", $("#billingPhoneno").val());
//    formData.append("BillingTaxNo", $("#billingTaxno").val());
//    formData.append("ShippingName", $("#shippingName").val());
//    formData.append("ShippingAddress", $("#shippingAddress").val());
//    formData.append("ShippingPhoneNo", $("#shippingPhoneno").val());
//    formData.append("ShippingTaxNo", $("#shippingTaxno").val());
//    formData.append("ProductName", $("#productName-1").val());
//    formData.append("ProductDetails", $("#productDetails-1").val());
//    formData.append("Rate", $("#productRate-1").val());
//    formData.append("Quantity", $("#product-qty-1").val());
//    formData.append("Amount", $("#productPrice-1").val());
//    formData.append("Subtotal", $("#cart-subtotal").val());
//    $.ajax({
//        url: '/Sales/GenerateInvoice',
//        type: 'Post',
//        data: formData,
//        dataType: 'json',
//        contentType: false,
//        processData: false,
//        success: function (Result) {

//            var Response = Result.data;
//            const url = window.URL.createObjectURL(new Blob([Result]));
//            const link = document.createElement('a');
//            link.href = url;
//            link.setAttribute('download', Result);
//            document.body.appendChild(link);
//            link.click();
//            Swal.fire({
//                title: 'Successfully Download',
//                icon: 'success',
//                confirmButtonColor: '#3085d6',
//                confirmButtonText: 'OK',
//            }).then(function () {
//                window.location = '/Sales/CreateInvoice';
//            });
//        }
//    })
//}
function cleartextBox() {

}

$('#idStatus').change(function () {
    if ($("#idStatus").val() == "Selse") {
        $("#idStatusCompany").show();
        $("#idStatusvender").hide();
        $("#idvender").show();
        $("#Companyname").show();
        cleartextBox();
    }
    if ($("#idStatus").val() == "Purchase") {

        $("#idStatusvender").show();
        $("#idvender").hide();
        $("#idCompany").show();
        $("#CompanCCP").show();
        $("#idStatusCompany").hide();
        $("#Companyname").hide();
        cleartextBox();
    }
});




//function DownloadInvoice() {
//
//    axios({
/*//        url: 'https://source.unsplash.com/random/500x500',*/
//        method: 'GET',
//        responseType: 'blob'
//    })
//        .then((response) => {
//            const url = window.URL
//                .createObjectURL(new Blob([response.data]));
//            const link = document.createElement('a');
//            link.href = url;
//            link.setAttribute('download', 'image.jpg');
//            document.body.appendChild(link);
//            link.click();
//        })
//}









