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

function AddExpenseDetails() {
    if ($('#frmtaskdetails').valid()) {
        var formData = new FormData();
        formData.append("ExpenseType", $("#txtexpensetypeid").val());
        formData.append("Description", $("#txtDescription").val());
        formData.append("BillNumber", $("#txtbillno").val());
        formData.append("Date", $("#txtdate").val());
        formData.append("TotalAmount", $("#txttotalamount").val());
        formData.append("Image", $("#txtimage").val());
        formData.append("Account", $("#txtaccount").val());
        formData.append("PaymentType", $("#txtpaymenttypeid").val());
        formData.append("IsPaid", $("#txtispaid").val());
        formData.append("IsApproved", $("#txtisapproved").val());
        $.ajax({
            url: '/Task/AddTaskDetails',
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
                        window.location = '/Task/AllTaskDetails';
                    });
                }
            }
        })
    }
    else {
        Swal.fire({
            title: "Kindly Fill All Datafield",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        })
    }
}