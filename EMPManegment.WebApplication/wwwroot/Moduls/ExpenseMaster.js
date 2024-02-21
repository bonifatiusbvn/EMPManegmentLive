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
function GetPaymentTypeList() {
    $.ajax({
        url: '/ExpenseMaster/GetPaymentTypeList',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#txtpaymenttype').append('<Option value=' + data.id + '>' + data.type + '</Option>')
            });
        }
    });
}


function AddExpenseDetails() {
    if ($('#formexpensedetails').valid()) {
        var formData = new FormData();
        formData.append("ExpenseType", $("#txtexpensetype").val());
        formData.append("Description", $("#txtDescription").val());
        formData.append("BillNumber", $("#txtbillno").val());
        formData.append("Date", $("#txtdate").val());
        formData.append("TotalAmount", $("#txttotalamount").val());
        formData.append("Image", $("#txtimage")[0].files[0]);
        formData.append("Account", $("#txtaccount").val());
        formData.append("PaymentType", $("#txtpaymenttype").val());
        formData.append("IsPaid", $("#txtispaid").val());
        formData.append("IsApproved", $("#txtisapproved").val());
        $.ajax({
            url: '/ExpenseMaster/AddexpenseDetails',
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
                        window.location = '/ExpenseMaster/ExpenseList';
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
$(document).ready(function () {
    $("#formexpensedetails").validate({
        rules: {
            txtexpensetype: "required",
            txtDescription: "required",
            txtbillno: "required",
            txtdate: "required",
            txttotalamount: "required",
            txtimage: "required",
            txtaccount: "required",
            txtpaymenttype: "required",
            txtispaid: "required",
            txtisapproved: "required",
        },
        messages: {
            txtexpensetype: "please enter Expensetype",
            txtDescription: "please enter description",
            txtbillno: "please enter bill no",
            txtdate: "please enter date",
            txttotalamount: "please enter amount",
            txtimage: "please enter image",
            txtaccount: "plese enter account",
            txtpaymenttype: "plese enter paymenttype",
            txtispaid: "plese select value",
            txtisapproved: "plese select value",
        }
    })
});