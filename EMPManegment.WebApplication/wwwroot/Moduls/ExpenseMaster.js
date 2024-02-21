function DisplayAddExpenseModel() {
    $('#AddExpenseModel').modal('show');
}

$(document).ready(function () {
    GetExpenseTypeList();
    GetPaymentTypeList();
});
function GetExpenseTypeList() {

    $.ajax({
        url: '/ExpenseMaster/GetExpenseTypeList',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#txtexpensetype').append('<Option value=' + data.id + '>' + data.type + '</Option>')
            });
        }
    });
}
function SelectExpenseTypeId() {
    document.getElementById("txtexpensetypeid").value = document.getElementById("txtexpensetype").value;
}
function GetPaymentTypeList() {
    $.ajax({
        url: '/ExpenseMaster/GetPaymentTypeList',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#txtpaymenttype').append('<Option value=' + data.id + '>' + data.productName + '</Option>')
            });
        }
    });
}
function SelectPaymentTypeId() {
    document.getElementById("txtpaymenttypeid").value = document.getElementById("txtpaymenttype").value;
}