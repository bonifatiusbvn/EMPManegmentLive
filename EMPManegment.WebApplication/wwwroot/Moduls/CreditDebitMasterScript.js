
$(document).ready(function () {
    GetPaymentMethodList();
    GetPaymentTypeList();
    GetCreditDebitTotalAmount();
});

function GetPaymentMethodList() {

    $.ajax({
        url: '/PurchaseOrderMaster/GetPaymentMethodList',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#txtpaymentmethod').append('<Option value=' + data.id + '>' + data.paymentMethod + '</Option>')
            });
            var firstPaymentMethod = result[0];

            $('#txtpaymentmethod').val(firstPaymentMethod.id);
        }
    });
}
function GetPaymentTypeList() {
    $.ajax({
        url: '/ExpenseMaster/GetPaymentTypeList',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#paymenttype').append('<Option value=' + data.id + '>' + data.type + '</Option>')
            });
            var firstPaymentType = result[0];

            $('#paymenttype').val(firstPaymentType.id);
        }
    });
}

function InsertCreditDebitDetails() {

    var value = $('#txtcreditdebitamount').val();
    if (value.trim() === '') {

        $('#warningMessage').text('Please Enter value for credit amount!!');
        return;
    }
    else {
        var objData = {
            VendorId: document.getElementById("txtvendorid").innerText,
            Type: document.getElementById("txtinvoicetype").innerText,
            InvoiceNo: document.getElementById("txtinvoiceno").innerText,
            PaymentType: $("#paymenttype").val(),
            PaymentMethod: $("#txtpaymentmethod").val(),
            CreditDebitAmount: $("#txtcreditdebitamount").val(),
            PendingAmount: $("#txtpendingamount").val(),
            TotalAmount: $("#txttotalamount").val(),
            CreatedBy: $("#txtuserid").val(),
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
                Swal.fire({
                    title: result.message,
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                }).then(function () {
                    window.location = '/Invoice/CreditDebitListView';
                });
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
}

function GetCreditDebitTotalAmount() {
    var Vid = {
        VendorId: document.getElementById("txtvendorid").innerText,
    }

    var form_data = new FormData();
    form_data.append("CREDITDEBITDETAILSBYID", JSON.stringify(Vid));
    $.ajax({
        url: '/Invoice/GetCreditDebitDetailsByVendorId',
        type: 'Post',
        data: form_data,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (result) {
            var total = 0;
            result.forEach(function (obj) {
                if (obj.creditDebitAmount) {
                    total += obj.creditDebitAmount;
                }
            });
            $("#txttotalcreditamount").text('₹' + total);

            var totalAmount = parseFloat($('#txttotalamount').val());
            var totalpendingAmount = totalAmount - total;

            $("#txttotalpendingamount").text('₹' + totalpendingAmount);
            $("#pendingamount").text('₹' + totalpendingAmount);

            $('#txtcreditdebitamount').on('input', function () {
                var enteredAmount = parseFloat($(this).val());

                if (!isNaN(enteredAmount)) {
                    var pendingAmount = totalpendingAmount - enteredAmount;

                    if (enteredAmount > totalpendingAmount) {
                        $('#warningMessage').text('Entered amount cannot exceed pending amount.');
                    } else {
                        $('#txtpendingamount').val(pendingAmount.toFixed(2));
                    }
                } else {
                    $('#warningMessage').text('');
                    $('#txtpendingamount').val('');
                }
            });
        },
    });
};


