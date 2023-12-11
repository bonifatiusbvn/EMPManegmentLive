function DownloadInvoice() {
    
    //var Invoice = {
    //    CompanyAddress : $("#companyAddress").val(),
    //    PostalCode: $("#companyaddpostalcode").val(),
    //    LegalRegistrationNo: $("#registrationNumber").val(),
    //    EmailAddress : $("#companyEmail").val(),
    //    Website: $("#companyWebsite").val(),
    //    PhoneNo: $("#companyContactno").val(),
    //    InvoiceNo: $("#invoicenoInput").val(),
    //    Date: $("#date-field").val(),
    //    PaymentStatus: $("#choices-payment-status").val(),
    //    BillingName: $("#billingName").val(),
    //    BillingAddress: $("#billingAddress").val(),
    //    BillingPhoneNo: $("#billingPhoneno").val(),
    //    BillingTaxNo: $("#billingTaxno").val(),
    //    ShippingName: $("#shippingName").val(),
    //    ShippingAddress: $("#shippingAddress").val(),
    //    ShippingPhoneNo: $("#shippingPhoneno").val(),
    //    ShippingTaxNo: $("#shippingTaxno").val(),
    //    ProductName: $("#productName-1").val(),
    //    ProductDetails: $("#productDetails-1").val(),
    //    Quantity: $("#product-qty-1").val(),
    //    Amount: $("#productPrice-1").val(),
    //    Subtotal: $("#cart-subtotal").val()
    //}
    //var form_data = new FormData();
    //form_data.append("ViewInvoice", JSON.stringify(Invoice));
    var formData = new FormData();
    formData.append("CompanyAddress", $("#companyAddress").val());
    formData.append("PostalCode", $("#companyaddpostalcode").val());
    formData.append("LegalRegistrationNo", $("#registrationNumber").val());
    formData.append("EmailAddress", $("#companyEmail").val());
    formData.append("Website", $("#companyWebsite").val());
    formData.append("PhoneNo", $("#companyContactno").val());
    formData.append("InvoiceNo", $("#invoicenoInput").val());
    formData.append("Date", $("#date-field").val());
    formData.append("PaymentStatus", $("#choices-payment-status").val());
    formData.append("BillingName", $("#billingName").val());
    formData.append("BillingAddress", $("#billingAddress").val());
    formData.append("BillingPhoneNo", $("#billingPhoneno").val());
    formData.append("BillingTaxNo", $("#billingTaxno").val());
    formData.append("ShippingName", $("#shippingName").val());
    formData.append("ShippingAddress", $("#shippingAddress").val());
    formData.append("ShippingPhoneNo", $("#shippingPhoneno").val());
    formData.append("ShippingTaxNo", $("#shippingTaxno").val());
    formData.append("ProductName", $("#productName-1").val());
    formData.append("ProductDetails", $("#productDetails-1").val());
    formData.append("Rate", $("#productRate-1").val());
    formData.append("Quantity", $("#product-qty-1").val());
    formData.append("Amount", $("#productPrice-1").val());
    formData.append("Subtotal", $("#cart-subtotal").val());
    $.ajax({
        url: '/Sales/GenerateInvoice',
        type: 'Post',
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (Result) {
            debugger
            var Response = Result.data;
            const url = window.URL.createObjectURL(new Blob([Result]));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', Result);
            document.body.appendChild(link);
            link.click();
            Swal.fire({
                title: 'Successfully Download',
                icon: 'success',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK',
            }).then(function () {
                window.location = '/Sales/CreateInvoice';
            });
        }
    })
}


//function DownloadInvoice() {
//    debugger
//    axios({
//        url: 'https://source.unsplash.com/random/500x500',
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