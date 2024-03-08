function DisplayAddExpenseModel() {
    $('#AddExpenseModel').modal('show');
}

$(document).ready(function () {
    GetExpenseTypeList();
    GetPaymentTypeList();
    DisplayExpenseList();
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
function populateDropdown(elementId, options) {
    var dropdown = $('#' + elementId);
    dropdown.empty();
    $.each(options, function (index, option) {
        dropdown.append($('<option></option>').attr('value', option).text(option));
    });
}

function EditExpenseDetails(Id) {

    $.ajax({
        url: '/ExpenseMaster/EditExpenseDetails?ExpenseId=' + Id,
        type: "Get",
        contentType: 'application/json;charset=utf-8;',
        dataType: 'json',
        success: function (response) {

            $('#EditExpenseModel').modal('show');
            $('#Editexpensetype').val(response.expenseType);
            $('#Editid').val(response.id);
            $('#EditDescription').val(response.description);
            $('#Editbillno').val(response.billNumber);
            $('#Editdate').val(response.date);
            $('#Edittotalamount').val(response.totalAmount);
            $('#Editaccount').val(response.account);
            $('#Editpaymenttype').val(response.paymentType);
            $('#EditIsPaid').val(response.isPaid ? "True" : "False");
            $('#EditIsApproved').val(response.isApproved ? "True" : "False");

        },
        error: function () {
            alert("Data not found");
        }
    });
}
function UpdateExpenseDetails() {
    if ($('#EditExpenseForm').valid()) {
        var formData = new FormData();
                formData.append("Id", $("#Editid").val());
                formData.append("ExpenseType", $("#Editexpensetype").val());
                formData.append("Description", $("#EditDescription").val());
                formData.append("BillNumber", $("#Editbillno").val());
                formData.append("Date", $("#Editdate").val());
                formData.append("TotalAmount", $("#Edittotalamount").val());
                formData.append("PaymentType", $("#Editpaymenttype").val());
                formData.append("IsPaid", $("#EditIsPaid").val());
                formData.append("IsApproved", $("#EditIsApproved").val());
                formData.append("Account", $("#Editaccount").val());

        $.ajax({
            url: '/ExpenseMaster/UpdateExpenseDetails',
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
            title: "Kindly Fill All Details",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        })
    }
}

$(document).ready(function () {
    $("#EditExpenseForm").validate({
        rules: {
            EditDescription: "required",
            Editbillno: "required",
            Edittotalamount: "required",
        },
        messages: {
            EditDescription: "Please enter Description",
            Editbillno: "Please enter bill no",
            Edittotalamount: "please enter total amount",
        }
    })
    $("#updatedetailbtn").on('click', function () {
        $("#EditExpenseForm").validate();
    });
});

function DisplayExpenseList() {
    $('#ExpenseTable').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "POST",
            url: '/ExpenseMaster/GetExpenseDetailsList',
            dataType: 'json',
        },
        columns: [
            {
                "data": "description", "name": "Description",
            },
            {
                "data": "billNumber", "name": "BillNumber"
            },
            {
                "data": "date", "name": "Date",
                render: function (data, type, row) {
                    var dateObj = new Date(data);
                    var day = dateObj.getDate();
                    var month = dateObj.getMonth() + 1;
                    var year = dateObj.getFullYear();
                    if (day < 10) {
                        day = '0' + day;
                    }
                    if (month < 10) {
                        month = '0' + month;
                    }
                    return day + '-' + month + '-' + year;
                }
            },
            {
                "data": "totalAmount", "name": "TotalAmount"
            },
            {
                "data": "account", "name": "Account"
            },
           
            {
                "data": "Action", "name": "Action",
                render: function (data, type, full) {
                    return ('<ul class="list-inline hstack gap-2 mb-0"><li class="list-inline-item" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="View"><a class="text-primary d-inline-block"><i class="ri-eye-fill fs-16"></i></a></li ><li class="list-inline-item edit" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="Edit"><a onclick="EditExpenseDetails(\'' + full.id + '\')" data-bs-toggle="modal" class="text-primary d-inline-block edit-item-btn"><i class="ri-pencil-fill fs-16"></i></a></li></ul >');
                }
            },
        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all",
        }]
    });
}