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
                $('#Editexpensetype').append('<Option value=' + data.id + '>' + data.type + '</Option>')
            });
        }
    });
}
function SelectExpenseTypeId() {
    document.getElementById("txtexpensetypeid").value = document.getElementById("txtexpensetype").value;
    document.getElementById("Editexpensetypeid").value = document.getElementById("Editexpensetype").value;
}
function GetPaymentTypeList() {
    $.ajax({
        url: '/ExpenseMaster/GetPaymentTypeList',
        success: function (result) {
            debugger
            $.each(result, function (i, data) {
                $('#txtpaymenttype').append('<Option value=' + data.id + '>' + data.type + '</Option>')
                $('#Editpaymenttype').append('<Option value=' + data.id + '>' + data.type + '</Option>')
            });
        }
    });
}
function SelectPaymentTypeId() {
    document.getElementById("txtpaymenttypeid").value = document.getElementById("txtpaymenttype").value;
    document.getElementById("Editpaymenttypeid").value = document.getElementById("Editpaymenttype").value;
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
function EditExpenseDetails(Id) {
    debugger
    $.ajax({
        url: '/ExpenseMaster/EditExpenseDetails?ExpenseId=' + Id,
        type: 'Get',
        datatype: 'json',
        success: function (response) {
            debugger;
            $('#EditExpenseModel').modal('show');
            $('#Editexpenseid').val(response.id);
            $('#EditUserId').val(response.userId);
            $('#Editexpensetype').val(response.expenseType);
            $('#EditDescription').val(response.description);
            $('#Editbillno').val(response.billNumber);
            $('#Editdate').val(response.date);
            $('#Edittotalamount').val(response.totalAmount);
            $('#Editimage').val(response.image);

            // Populate Expense Type dropdown
            var expenseTypeDropdown = $('#Editexpensetype');
            expenseTypeDropdown.empty(); // Clear existing options

           debugger
            expenseTypeDropdown.append($('<option>').text('---Select Expense Type---').attr('disabled', 'true').attr('selected', 'true'));

            // Populate options based on response data
            $.each(response.expenseTypes, function (index, expenseType) {
                expenseTypeDropdown.append($('<option>').text(expenseType.name).attr('value', expenseType.id));
            });

            debugger
            expenseTypeDropdown.val(response.expenseType);

            // Populate Account dropdown
            var accountDropdown = $('#Editaccount');
            accountDropdown.empty();
            accountDropdown.append($('<option>').text('Select Account Type').attr('disabled', 'true').attr('selected', 'true'));
            accountDropdown.append($('<option>').text('Credit').attr('value', 'Credit'));
            accountDropdown.append($('<option>').text('Debit').attr('value', 'Debit'));

            // Set the selected value for the account dropdown
            accountDropdown.val(response.account);

           debugger
            var paymentTypeDropdown = $('#Editpaymenttype');
            paymentTypeDropdown.empty();
            paymentTypeDropdown.append($('<option>').text('---Select Payment Type---').attr('disabled', 'true').attr('selected', 'true'));

            // Set the selected value for the payment type dropdown
            paymentTypeDropdown.val(response.paymentType);

            // Populate Is Paid dropdown
            var isPaidDropdown = $('#Editispaid');
            isPaidDropdown.empty();
            isPaidDropdown.append($('<option>').text('Is Paid').attr('disabled', 'true').attr('selected', 'true'));
            isPaidDropdown.append($('<option>').text('True').attr('value', 'True'));
            isPaidDropdown.append($('<option>').text('False').attr('value', 'False'));

            // Set the selected value for the is paid dropdown
            isPaidDropdown.val(response.isPaid);

            // Populate Is Approved dropdown
            var isApprovedDropdown = $('#Editisapproved');
            isApprovedDropdown.empty();
            isApprovedDropdown.append($('<option>').text('Is Approved').attr('disabled', 'true').attr('selected', 'true'));
            isApprovedDropdown.append($('<option>').text('True').attr('value', 'True'));
            isApprovedDropdown.append($('<option>').text('False').attr('value', 'False'));

            // Set the selected value for the is approved dropdown
            isApprovedDropdown.val(response.isApproved);

            // ...
            // Continue setting other values as you did before
        },

        error: function () {
            alert("Data not found");
        }
    })
}
//function UpdateExpenseDetails() {
//    if ($('#frmtaskdetails').valid()) {
//        var formData = new FormData();
//        formData.append("ExpenseType", $("#editexpensetypeid").val());
//        formData.append("Description", $("#editDescription").val());
//        formData.append("BillNumber", $("#editbillno").val());
//        formData.append("Date", $("#editdate").val());
//        formData.append("TotalAmount", $("#edittotalamount").val());
//        formData.append("Image", $("#editimage").val());
//        formData.append("Account", $("#editaccount").val());
//        formData.append("PaymentType", $("#editpaymenttypeid").val());
//        formData.append("IsPaid", $("#editispaid").val());
//        formData.append("IsApproved", $("#editisapproved").val());
//        $.ajax({
//            url: '/ExpenseMaster/AddTaskDetails',
//            type: 'Post',
//            data: formData,
//            dataType: 'json',
//            contentType: false,
//            processData: false,
//            success: function (Result) {

//                if (Result.message != null) {
//                    Swal.fire({
//                        title: Result.message,
//                        icon: 'success',
//                        confirmButtonColor: '#3085d6',
//                        confirmButtonText: 'OK',
//                    }).then(function () {
//                        window.location = '/ExpenseMaster/AllTaskDetails';
//                    });
//                }
//            }
//        })
//    }
//    else {
//        Swal.fire({
//            title: "Kindly Fill All Datafield",
//            icon: 'warning',
//            confirmButtonColor: '#3085d6',
//            confirmButtonText: 'OK',
//        })
//    }
//}
//}