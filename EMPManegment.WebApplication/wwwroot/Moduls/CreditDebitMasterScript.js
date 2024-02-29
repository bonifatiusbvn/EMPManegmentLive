
$(document).ready(function () {
    GetPaymentMethodList();
    GetPaymentTypeList();
});

function GetPaymentMethodList() {

    $.ajax({
        url: '/OrderMaster/GetPaymentMethodList',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#txtpaymentmethod').append('<Option value=' + data.id + '>' + data.paymentMethod + '</Option>')
            });
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
        }
    });
}