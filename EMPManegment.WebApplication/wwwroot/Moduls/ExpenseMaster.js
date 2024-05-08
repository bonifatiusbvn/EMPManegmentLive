function DisplayAddExpenseModel() {
    clearText();
    $('#AddExpenseModel').modal('show');
}

$(document).ready(function () {
    GetExpenseTypeList();
    GetPaymentTypeList();
    GetUserExpenseList();
    DisplayExpenseList();
    GetExpenseTotalAmount();
    GetAllUserExpenseList();
    ApprovedExpenseList();
    /*    GetPayUserExpenseList();*/
    UserExpensesDetails();
});

function clearText() {
    resetForm();
    $("#txtexpensetype").val('');
    $("#txtDescription").val('');
    $("#txtbillno").val('');
    $("#txtdate").val('');
    $("#txttotalamount").val('');
    $("#txtimage").val('');
}

function resetForm() {
    if (EditExpensesForm) {
        EditExpensesForm.resetForm();
    }
    if (UserExpenseForm) {
        UserExpenseForm.resetForm();
    }
    if (FormExpenseDetails) {
        FormExpenseDetails.resetForm();
    }
}
function preventEmptyValue(input) {

    if (input.value === "") {

        input.value = 1;
    }
}
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
            var firstPaymentMethod = result[0];


        }
    });
}
function SelectPaymentTypeId() {
    document.getElementById("txtpaymenttypeid").value = document.getElementById("txtpaymenttype").value;
    document.getElementById("Editpaymenttypeid").value = document.getElementById("Editpaymenttype").value;
}
function GetParameterByName(name, url) {

    if (!url) url = window.location.href;

    if (!name) {
        return null;
    }

    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)", "i");
    var results = regex.exec(url);

    if (!results) {
        return null;
    }
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function AddExpenseDetails() {
    if ($('#formexpensedetails').valid()) {
        var formData = new FormData();
        formData.append("ExpenseType", $("#txtexpensetype").val());
        formData.append("Description", $("#txtDescription").val());
        formData.append("BillNumber", $("#txtbillno").val());
        formData.append("Date", $("#txtdate").val());
        formData.append("Account", $("#txtaccount").val());
        formData.append("TotalAmount", $("#txttotalamount").val());
        formData.append("Image", $("#txtimage")[0].files[0]);
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
function AddUserExpenseDetails() {
    if ($('#userexpenseform').valid()) {
        var formData = new FormData();
        formData.append("ExpenseType", $("#txtexpensetype").val());
        formData.append("Description", $("#txtDescription").val());
        formData.append("BillNumber", $("#txtbillno").val());
        formData.append("Date", $("#txtdate").val());
        formData.append("Account", $("#txtaccount").val());
        formData.append("TotalAmount", $("#txttotalamount").val());
        formData.append("Image", $("#txtimage")[0].files[0]);
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
                        window.location = '/ExpenseMaster/UserExpenseList';
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
    resetForm();
    $.ajax({
        url: '/ExpenseMaster/EditExpenseDetails?ExpenseId=' + Id,
        type: "Get",
        contentType: 'application/json;charset=utf-8;',
        dataType: 'json',
        success: function (response) {
            debugger

            $('#EditExpenseModel').modal('show');
            $('#Editexpensetype').val(response.expenseType);
            $('#Editid').val(response.id);
            $('#EditDescription').val(response.description);
            $('#Editbillno').val(response.billNumber);
            $('#Editdate').val(response.date);
            $('#Edittotalamount').val(response.totalAmount);
            $('#Editaccount').val(response.account);
            $('#Editpaymenttype').val(response.paymentType);
            $('#Editpaymenttypeid').val(response.paymentType);
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
                        window.location = '/ExpenseMaster/UserExpenseList';
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

function UpdateExpenseListDetails() {
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

var EditExpensesForm;
$(document).ready(function () {
    EditExpensesForm = $("#EditExpenseForm").validate({
        rules: {
            EditDescription: "required",
            Editbillno: "required",
            Edittotalamount: "required",
        },
        messages: {
            EditDescription: "Please enter Description",
            Editbillno: "Please enter bill no",
            Edittotalamount: "please enter Correct total amount",
        }
    })
    $("#updatedetailbtn").on('click', function () {
        $("#EditExpenseForm").validate();
    });
})

var UserExpenseForm;
$(document).ready(function () {
    UserExpenseForm = $("#userexpenseform").validate({
        rules: {
            txtexpensetype: "required",
            txtDescription: "required",
            txtdate: "required",
            txttotalamount: "required",
        },
        messages: {
            txtexpensetype: "Please Select Expense Type",
            txtDescription: "Please Enter Description",
            txtdate: "Please Select the Date",
            txttotalamount: "Please Enter Correct Total Amount",
        }
    })
});

var FormExpenseDetails;
$(document).ready(function () {
    FormExpenseDetails = $("#formexpensedetails").validate({
        rules: {
            txtexpensetype: "required",
            txtDescription: "required",
            txtdate: "required",
            txttotalamount: "required",
        },
        messages: {
            txtexpensetype: "Please Select Expense Type",
            txtDescription: "Please Enter Description",
            txtdate: "Please Select the Date",
            txttotalamount: "Please Enter Correct Total Amount",
        }
    })
});


function deleteExpense(Id) {
    Swal.fire({
        title: "Are you sure want to Delete This?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, Delete it!",
        cancelButtonText: "No, cancel!",
        confirmButtonClass: "btn btn-primary w-xs me-2 mt-2",
        cancelButtonClass: "btn btn-danger w-xs mt-2",
        buttonsStyling: false,
        showCloseButton: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/ExpenseMaster/DeleteExpense?Id=' + Id,
                type: 'POST',
                dataType: 'json',
                success: function (Result) {
                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/ExpenseMaster/UserExpenseList';
                    })
                },
                error: function () {
                    Swal.fire({
                        title: "Can't Delete Expense!",
                        icon: 'warning',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/ExpenseMaster/UserExpenseList';
                    })
                }
            })
        } else if (result.dismiss === Swal.DismissReason.cancel) {

            Swal.fire(
                'Cancelled',
                'Expense Have No Changes.!!😊',
                'error'
            );
        }
    });
}

function GetUserExpenseList() {
    $('#UserExpenseTable').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "Post",
            url: '/ExpenseMaster/UserExpenseListTable',
            dataType: 'json'
        },
        columns: [
            {
                "data": null,
                "render": function (data, type, full, meta) {
                    var account = full.account.toLowerCase();
                    if (account === "credit") {
                        return '<div class="avatar-xs"><div class="avatar-title bg-success-subtle text-success rounded-circle fs-16"><i class="ri-arrow-left-down-fill"></i></div></div>';
                    } else if (account === "debit") {
                        return '<div class="avatar-xs"><div class="avatar-title bg-danger-subtle text-danger rounded-circle fs-16"><i class="ri-arrow-right-up-fill"></i></div></div>';
                    } else {
                        return '';
                    }
                },
                "orderable": false
            },
            { "data": "description", "name": "Description" },
            { "data": "billNumber", "name": "BillNumber" },
            {
                "data": "date",
                "name": "Date",
                "render": function (data, type, full, meta) {
                    return new Date(data).toLocaleDateString();
                }
            },
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full, meta) {

                    var color = full.account.toLowerCase() === "credit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + data + '</span>';
                }
            },
            //{
            //    "data": "account",
            //    "name": "Account",
            //    "render": function (data, type, full, meta) {

            //        var color = data.toLowerCase() === "credit" ? "green" : "red";
            //        return '<span style="color: ' + color + ';">' + data + '</span>';
            //    },

            //},
            {
                "data": null,
                "name": "Action",
                "render": function (data, type, full, meta) {
                    return '<div class="d-flex justify-content-center">' +
                        '<ul class="list-inline hstack gap-2 mb-0">' +
                        '<li class="list-inline-item edit" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="Edit">' +
                        '<a onclick="EditExpenseDetails(\'' + full.id + '\')" data-bs-toggle="modal" class="text-primary d-inline-block edit-item-btn">' +
                        '<i class="ri-pencil-fill fs-16"></i>' +
                        '</a>' +
                        '</li>' +
                        '<li class="list-inline-item" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="Delete">' +
                        '<a class="btn text-danger btndeletedoc" href="/Invoice/InvoiceDetails">' +
                        '<i class="fas fa-trash"></i>' +
                        '</a>' +
                        '</li>' +
                        '</ul>' +
                        '</div>';
                },
                "orderable": false
            }
        ],
        columnDefs: [
            {
                "targets": [0, -1],
                "orderable": false
            }
        ]
    });
}
function GetPayUserExpenseCreditList(userId) {
    $('#UserPayExpenseTableCredit').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "POST",
            url: '/ExpenseMaster/GetPayCreditExpenseListTable?UserId=' + userId,
            dataType: 'json',
        },
        columns: [
            {
                "data": null,
                "render": function (data, type, full, meta) {
                    return '<div class="avatar-xs"><div class="avatar-title bg-danger-subtle text-danger rounded-circle fs-16"><i class="ri-arrow-right-up-fill"></i></div></div>';
                },
                "orderable": false
            },
            { "data": "id", "name": "Id", "visible": false },
            { "data": "description", "name": "Description" },
            {
                "data": "date",
                "name": "Date",
                "render": function (data, type, full, meta) {
                    return new Date(data).toLocaleDateString();
                }
            },
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full, meta) {
                    var color = full.account.toLowerCase() === "debit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + data + '</span>';
                }
            },
            { "data": "account", "name": "Account", "visible": false },
        ],
        columnDefs: [{
            "targets": [0],
            "orderable": false
        }],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;

            // Remove the formatting to get integer data for summation
            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                        i : 0;
            };

            // Total over all pages
            total = api
                .column(4)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Update footer
            $(api.column(4).footer()).html(
                '<span style="color: black;">Total: ' + total + '</span>'
            );
        }
    });
}


function GetPayUserExpenseDebitList(userId) {
    $('#UserPayExpenseTableDebit').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "POST",
            url: '/ExpenseMaster/GetPayDebitExpenseListTable?UserId=' + userId,
            dataType: 'json',
        },
        columns: [
            {
                "data": null,
                "render": function (data, type, full, meta) {
                    return '<div class="avatar-xs"><div class="avatar-title bg-success-subtle text-success rounded-circle fs-16"><i class="ri-arrow-right-down-fill"></i></div></div>';
                },
                "orderable": false
            },
            { "data": "id", "name": "Id", "visible": false },
            { "data": "description", "name": "Description" },
            {
                "data": "date",
                "name": "Date",
                "render": function (data, type, full, meta) {
                    return new Date(data).toLocaleDateString();
                }
            },
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full, meta) {
                    var color = full.account.toLowerCase() === "credit" ? "red" : "green";
                    return '<span style="color: ' + color + ';">' + data + '</span>';
                }
            },
            { "data": "account", "name": "Account", "visible": false },
        ],
        columnDefs: [{
            "targets": [0],
            "orderable": false
        }],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;

            // Remove the formatting to get integer data for summation
            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                        i : 0;
            };

            // Total over all pages
            total = api
                .column(4)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Update footer
            $(api.column(4).footer()).html(
                '<span style="color: black;">Total: ' + total + '</span>'
            );
        }
    });
}






$(document).ready(function () {

    var userId = GetParameterByName('userId');
    if (userId) {
        GetAllUserExpenseList(userId);
    }
});
function UserExpensesDetails() {
    debugger
    $('#UserListTable').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "Post",
            url: '/ExpenseMaster/GetUserListTable',
            dataType: 'json'
        },
        columns: [
            { "data": "userId", "name": "UserID", "visible": false },
            {
                "data": "fullName",
                "name": "FullName",
                "render": function (data, type, full, meta) {
                    var imageSrc = full.image ? '<img src="/' + full.image + '" class="direct-chat-img">' : '';
                    return '<div class="d-flex align-items-center">' +
                        '<div class="flex-shrink-0">' +
                        imageSrc +
                        '</div>' +
                        '<div class="flex-grow-1 ms-2 name">' +
                        '<h5 class="fs-15"><a href="/ExpenseMaster/ApprovedExpense?UserId=' + full.userId + '" class="fw-medium link-primary view-details" data-userid="' + full.userId + '">' + data + '</a></h5>' +
                        '</div>' +
                        '</div>';
                }
            },
            { "data": "userName", "name": "UserName" },
            {
                "data": "date",
                "name": "Date",
                "render": function (data, type, full, meta) {
                    return new Date(data).toLocaleDateString();
                }
            },
            { "data": "unapprovedPendingAmount", "name": "UnapprovedPendingAmount" },
            { "data": "totalPendingAmount", "name": "TotalPendingAmount" },
            { "data": "totalAmount", "name": "TotalAmount" },
        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all",
        }]
    });
}


function GetAllUserExpenseList(userId) {
    $('#UserallExpenseTable').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "POST",
            url: '/ExpenseMaster/GetAllUserExpenseListTable?UserId=' + userId,
            dataType: 'json',
        },
        columns: [
            {
                "data": null,
                "render": function (data, type, full, meta) {
                    var account = full.account.toLowerCase();
                    if (account === "credit") {
                        return '<div class="avatar-xs"><div class="avatar-title bg-success-subtle text-success rounded-circle fs-16"><i class="ri-arrow-left-down-fill"></i></div></div>';
                    } else if (account === "debit") {
                        return '<div class="avatar-xs"><div class="avatar-title bg-danger-subtle text-danger rounded-circle fs-16"><i class="ri-arrow-right-up-fill"></i></div></div>';
                    } else {
                        return '';
                    }
                },
                "orderable": false
            },
            { "data": "id", "name": "Id", "visible": false },
            { "data": "expenseTypeName", "name": "ExpenseTypeName" },
            { "data": "paymentTypeName", "name": "PaymentTypeName" },
            { "data": "billNumber", "name": "BillNumber" },
            { "data": "description", "name": "Description" },
            {
                "data": "date",
                "name": "Date",
                "render": function (data, type, full, meta) {
                    return new Date(data).toLocaleDateString();
                }
            },
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full, meta) {
                    var color = full.account && full.account.toLowerCase() === "credit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + data + '</span>';
                }
            },
            { "data": "account", "name": "Account", "visible": false },
        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all",
        }]
    });
}

function GetUserUnApprovedExpenseList(UserId) {
    $('#UserallUnApprovedExpenseTable').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "POST",
            url: '/ExpenseMaster/GetUserUnApprovedExpenseList?UserId=' + UserId,
            dataType: 'json',
        },
        columns: [
            {
                "data": null,
                "render": function (data, type, full, meta) {
                    return '<div class="form-check"><input class="form-check-input" data-id="' + full.id + '" type="checkbox" name="chk_child" value="option1"></div>';
                },
                "orderable": false
            },
            {
                "data": null,
                "render": function (data, type, full, meta) {
                    var account = full.account.toLowerCase();
                    if (account === "credit") {
                        return '<div class="avatar-xs"><div class="avatar-title bg-success-subtle text-success rounded-circle fs-16"><i class="ri-arrow-left-down-fill"></i></div></div>';
                    } else if (account === "debit") {
                        return '<div class="avatar-xs"><div class="avatar-title bg-danger-subtle text-danger rounded-circle fs-16"><i class="ri-arrow-right-up-fill"></i></div></div>';
                    } else {
                        return '';
                    }
                },
                "orderable": false
            },
            { "data": "id", "name": "Id", "visible": false },
            { "data": "expenseTypeName", "name": "ExpenseTypeName" },
            { "data": "paymentTypeName", "name": "PaymentTypeName" },
            { "data": "billNumber", "name": "BillNumber" },
            { "data": "description", "name": "Description" },
            {
                "data": "date",
                "name": "Date",
                "render": function (data, type, full, meta) {
                    return new Date(data).toLocaleDateString();
                }
            },
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full, meta) {
                    var color = full.account && full.account.toLowerCase() === "credit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + data + '</span>';
                }
            },
            { "data": "account", "name": "Account", "visible": false },
        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all",
        }]
    });
}
function GetUserApprovedExpenseList(UserId) {
    $('#GetUserApprovedExpenseList').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "POST",
            url: '/ExpenseMaster/GetUserApprovedExpenseList?UserId=' + UserId,
            dataType: 'json',
        },
        columns: [
            {
                "data": null,
                "render": function (data, type, full, meta) {
                    var account = full.account.toLowerCase();
                    if (account === "credit") {
                        return '<div class="avatar-xs"><div class="avatar-title bg-success-subtle text-success rounded-circle fs-16"><i class="ri-arrow-left-down-fill"></i></div></div>';
                    } else if (account === "debit") {
                        return '<div class="avatar-xs"><div class="avatar-title bg-danger-subtle text-danger rounded-circle fs-16"><i class="ri-arrow-right-up-fill"></i></div></div>';
                    } else {
                        return '';
                    }
                },
                "orderable": false
            },
            { "data": "id", "name": "Id", "visible": false },
            { "data": "expenseTypeName", "name": "ExpenseTypeName" },
            { "data": "paymentTypeName", "name": "PaymentTypeName" },
            { "data": "billNumber", "name": "BillNumber" },
            { "data": "description", "name": "Description" },
            {
                "data": "date",
                "name": "Date",
                "render": function (data, type, full, meta) {
                    return new Date(data).toLocaleDateString();
                }
            },
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full, meta) {
                    var color = full.account && full.account.toLowerCase() === "credit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + data + '</span>';
                }
            },
            { "data": "account", "name": "Account", "visible": false },
        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all",
        }]
    });
}


$(document).ready(function () {

    var UserId = GetParameterByName('userId');

    if (UserId) {
        GetUserUnApprovedExpenseList(UserId);
        GetUserApprovedExpenseList(UserId);

    }

    $('.nav-link').click(function () {
        var targetTab = $(this).attr('href');

        if (targetTab === '#allExpense') {

            GetAllUserExpenseList(userId);
        } else if (targetTab === '#allUnApprovedExpense') {
            GetUserUnApprovedExpenseList(UserId);
        } else if (targetTab === '#allApprovedExpense') {
            GetUserApprovedExpenseList(UserId);
        }
    });
});


$(document).ready(function () {
    function anyCheckboxChecked() {
        return $("input[name='chk_child']:checked").length > 0;
    }

    $('#UserallUnApprovedExpenseTable').on('change', 'input[name="chk_child"]', function () {
        if (anyCheckboxChecked()) {
            $('#remove-actions').show();
        } else {
            $('#remove-actions').hide();
        }
        var allChecked = $('input[name="chk_child"]:checked').length === $('input[name="chk_child"]').length;
        $('#checkedAll').prop('checked', allChecked);
    });

    $('#checkedAll').on('change', function () {
        $('input[name="chk_child"]').prop('checked', $(this).prop('checked'));
        if ($(this).prop('checked')) {
            $('#remove-actions').show();
        } else {
            if (!anyCheckboxChecked()) {
                $('#remove-actions').hide();
            }
        }
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
                "data": "userName", "name": "UserName",
                "className": "text-center"
            },
            {
                "data": "description", "name": "Description",
                "className": "text-center"
            },
            {
                "render": function (data, type, full) {
                    return '<div class="d-flex">' +
                        '<div class="flex-grow-1 tasks_name">' + full['billNumber'] + '</div>' +
                        '<div class="flex-shrink-0 ms-4 task-icons">' +
                        '<ul class="list-inline tasks-list-menu mb-0">' +
                        '<li class="list-inline-item"><a href="/ExpenseMaster/DownloadBill/?BillName=' + full.image + '"><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="currentColor" d="M13 12h3l-4 4l-4-4h3V8h2v4Zm2-8H5v16h14V8h-4V4ZM3 2.992C3 2.444 3.447 2 3.999 2H16l5 5v13.993A1 1 0 0 1 20.007 22H3.993A1 1 0 0 1 3 21.008V2.992Z" /></svg></a></li>'; +
                            '</ul>' +
                            '</div>' +
                            '</div>';
                }
            },
            {
                "data": "date",
                "name": "Date",
                "className": "text-center",

                "render": function (data, type, full, meta) {
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
                "data": "totalAmount", "name": "TotalAmount",
                "className": "text-center"
            },
            {
                "data": "account", "name": "Account",
                "className": "text-center"
            },
            {
                "data": "Action",
                "name": "Action",
                "className": "text-center",
                "orderable": false,
                "render": function (data, type, full) {
                    return ('<li class="btn list-inline-item edit" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="Edit"><a onclick="EditExpenseDetails(\'' + full.id + '\')"><i class="ri-pencil-fill fs-16"></i></a></li><li class="btn text-danger list-inline-item delete" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="Delete" style="margin-left:12px;"><a onclick="deleteExpense(\'' + full.id + '\')"><i class="ri-delete-bin-5-fill fs-16"></i></a></li>');
                }
            },
        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all",
        }]
    });
}


function GetExpenseTotalAmount() {

    var userId = {
        UserId: $("#txtuserid").val(),
    }
    var form_data = new FormData();
    form_data.append("USERID", JSON.stringify(userId));
    $.ajax({
        url: '/ExpenseMaster/GetExpenseDetailsByUserId',
        type: 'Post',
        data: form_data,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (result) {
            debugger
            var total = 0;
            result.forEach(function (obj) {
                if (obj.totalAmount) {
                    total += obj.totalAmount;
                }
            });
            debugger
            var creditamount = 0;
            result.forEach(function (obj) {
                if (obj.account == "Credit") {
                    creditamount += obj.totalAmount;
                }
            });
            $("#txttotalcreditamount").text('₹' + creditamount);
            var Dabitamount = 0;
            result.forEach(function (obj) {
                if (obj.account == "Debit") {
                    Dabitamount += obj.totalAmount;
                }
            });
            $("#txtTotalAmount").text('₹' + Dabitamount);
            var PendingAmount = Dabitamount - creditamount;
            $("#pendingamount").text('₹' + PendingAmount);
            $("#txttotaldebitedamount").text('₹' + PendingAmount);
            $('#txtcreditamount').on('input', function () {
                var enteredAmount = parseFloat($(this).val());

                if (!isNaN(enteredAmount)) {
                    var pendingAmount = PendingAmount - enteredAmount;

                    if (enteredAmount > PendingAmount) {
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
}

function ApproveExpense() {
    var userId = $("#txtgetUserId").val();
    Swal.fire({
        title: "Are you sure want to Approve This?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, Approve it!",
        cancelButtonText: "No, cancel!",
        confirmButtonClass: "btn btn-primary w-xs me-2 mt-2",
        cancelButtonClass: "btn btn-danger w-xs mt-2",
        buttonsStyling: false,
        showCloseButton: true
    }).then((result) => {
        if (result.isConfirmed) {
            let val = [];
            $("input[name=chk_child]:checked").each(function () {
                val.push($(this).attr("data-id"));
            });
            if (val.length > 0) {
                var form_data = new FormData();
                form_data.append("EXPENSEID", val);
                $.ajax({
                    url: '/ExpenseMaster/ApproveExpense',
                    type: 'POST',
                    contentType: 'application/json',
                    data: form_data,
                    processData: false,
                    contentType: false,
                    success: function (Result) {
                        if (Result.message != null) {
                            Swal.fire({
                                title: Result.message,
                                icon: 'success',
                                confirmButtonColor: '#3085d6',
                                confirmButtonText: 'OK',
                            }).then(function () {
                                window.location = '/ExpenseMaster/ApprovedExpense?UserId=' + userId;
                            });
                        }
                    },
                    error: function (xhr, textStatus, errorThrown) {

                        console.error(xhr.responseText);
                    }
                });
            }
        } else if (result.dismiss === Swal.DismissReason.cancel) {

            Swal.fire(
                'Cancelled',
                'You Have No Changes.!!😊',
                'error'
            );
        }
    });
}

function ApprovedExpenseList() {
    var UserId = $("#txtuserid").val();
    $('#GetUserApprovedExpenseList').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "POST",
            url: '/ExpenseMaster/GetUserApprovedExpenseList?UserId=' + UserId,
            dataType: 'json',
        },
        columns: [

            { "data": "id", "name": "Id", "visible": false },
            { "data": "expenseTypeName", "name": "ExpenseTypeName" },
            { "data": "paymentTypeName", "name": "PaymentTypeName" },
            { "data": "billNumber", "name": "BillNumber" },
            { "data": "description", "name": "Description" },
            {
                "data": "date",
                "name": "Date",
                "render": function (data, type, full, meta) {
                    return new Date(data).toLocaleDateString();
                }
            },
            { "data": "totalAmount", "name": "TotalAmount" },
            { "data": "account", "name": "Account" },
        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all",
        }]
    });
}
function GetPayExpense() {
    if ($("#GetPayForm").valid()) {
        var formData = new FormData();
        formData.append("ExpenseType", $("#txtexpensetype").val());
        formData.append("Account", $("#txtAccount").val());
        formData.append("UserId", $("#txtuserid").val());
        formData.append("ApprovedBy", $("#txtuseraproveid").val());
        formData.append("ApprovedByName", $("#txtuseraprovename").val());
        formData.append("TotalAmount", $("#txtcreditamount").val());
        formData.append("PaymentType", $("#txtpaymenttype").val());
        formData.append("CreatedBy", $("#txtuseraproveid").val());
        $.ajax({
            url: '/ExpenseMaster/GetPayExpense',
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
                        window.location = '/ExpenseMaster/GetPayExpense';
                    });
                }
            }
        })
    }
    else {
        Swal.fire({
            title: "Kindly fill all data fields",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        })
    }
}

$(document).ready(function () {
    $("#GetPayForm").validate({
        rules: {
            txtcreditamount: "required",
            txtpaymenttype: "required",
        },
        messages: {
            txtcreditamount: "Please Enter Amount",
            txtpaymenttype: "Please Select Payment Type",
        }
    })
});
$(document).ready(function () {

    var UserId = $("#txtuserid").val();

    if (UserId) {
        GetAllUserExpenseList(UserId);
        UserDebitExpenseList(UserId);
        UserCreditExpenseList(UserId);
        GetPayUserExpenseCreditList(UserId)
        GetPayUserExpenseDebitList(UserId)
    }

    $('.nav-link').click(function () {
        var targetTab = $(this).attr('href');

        var UserId = $("#txtuserid").val();

        if (targetTab === '#GetallExpense') {
            GetAllUserExpenseList(UserId);
        } else if (targetTab === '#GetallUnApprovedExpense') {
            UserDebitExpenseList(UserId);
        } else if (targetTab === '#GetallApprovedExpense') {
            UserCreditExpenseList(UserId);
        }
    });
});
function UserDebitExpenseList(UserId) {
    var Account = "Debit";
    $('#UserallUnApprovedExpenseTable').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "POST",
            url: '/ExpenseMaster/GetCreditDebitExpenseListTable?UserId=' + UserId + '&Account=' + Account,
            dataType: 'json',
        },
        columns: [
            { "data": "id", "name": "Id", "visible": false },
            { "data": "expenseTypeName", "name": "ExpenseTypeName" },
            { "data": "paymentTypeName", "name": "PaymentTypeName" },
            { "data": "billNumber", "name": "BillNumber" },
            { "data": "description", "name": "Description" },
            {
                "data": "date",
                "name": "Date",
                "render": function (data, type, full, meta) {
                    return new Date(data).toLocaleDateString();
                }
            },
            { "data": "totalAmount", "name": "TotalAmount" },
            { "data": "account", "name": "Account" },
        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all",
        }]
    });
}
function UserCreditExpenseList(UserId) {
    var Account = "Credit";
    $('#GetUserApprovedExpenseList').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "POST",
            url: '/ExpenseMaster/GetCreditDebitExpenseListTable?UserId=' + UserId + '&Account=' + Account,
            dataType: 'json',
        },
        columns: [
            { "data": "id", "name": "Id", "visible": false },
            { "data": "expenseTypeName", "name": "ExpenseTypeName" },
            { "data": "paymentTypeName", "name": "PaymentTypeName" },
            { "data": "billNumber", "name": "BillNumber" },
            { "data": "description", "name": "Description" },
            {
                "data": "date",
                "name": "Date",
                "render": function (data, type, full, meta) {
                    return new Date(data).toLocaleDateString();
                }
            },
            { "data": "totalAmount", "name": "TotalAmount" },
            { "data": "account", "name": "Account" },
        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all",
        }]
    });
}

$(document).ready(function () {
    GetAllUserUnapproveExpenseList()

    $('.nav-link').click(function () {
        var targetTab = $(this).attr('href');
        if (targetTab === '#GetAllUnApprovedExpenseList') {

            GetAllUserUnapproveExpenseList();
        } else if (targetTab === '#GetAllApprovedExpenseList') {

            GetAllUserApproveExpenseList();
        } else if (targetTab === '#GetAllCreditExpenseList') {

            GetAllUserCreditExpenseList();
        }
    });
});
function GetAllUserUnapproveExpenseList() {
    var IsApprove = false;
    $('#GetUserUnapprovedExpenseList').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "Post",
            url: '/ExpenseMaster/GetAllUserUnapproveExpenseList?Unapprove=' + IsApprove,
            dataType: 'json'
        },
        columns: [
            {
                "data": null,
                "render": function (data, type, full, meta) {
                    var account = full.account.toLowerCase();
                    return '<div class="avatar-xs"><div class="avatar-title bg-danger-subtle text-danger rounded-circle fs-16"><i class="ri-arrow-right-up-fill"></i></div></div>';
                },
                "orderable": false
            },
            { "data": "description", "name": "Description" },
            { "data": "billNumber", "name": "BillNumber" },
            {
                "data": "date",
                "name": "Date",
                "render": function (data, type, full, meta) {
                    return new Date(data).toLocaleDateString();
                }
            },
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full, meta) {

                    var color = full.account.toLowerCase() === "credit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + data + '</span>';
                }
            },
            {
                "data": null,
                "name": "Action",
                "render": function (data, type, full, meta) {
                    return '<div class="d-flex justify-content-center">' +
                        '<ul class="list-inline hstack gap-2 mb-0">' +
                        '<li class="list-inline-item edit" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="Edit">' +
                        '<a onclick="EditExpenseDetails(\'' + full.id + '\')" data-bs-toggle="modal" class="text-primary d-inline-block edit-item-btn">' +
                        '<i class="ri-pencil-fill fs-16"></i>' +
                        '</a>' +
                        '</li>' +
                        '<li class="list-inline-item" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="Delete">' +
                        '<a class="btn text-danger btndeletedoc" onclick="deleteExpense(\'' + full.id + '\')">' +
                        '<i class="fas fa-trash"></i>' +
                        '</a>' +
                        '</li>' +
                        '</ul>' +
                        '</div>';
                },
                "orderable": false
            }
        ],
        columnDefs: [
            {
                "targets": [0, -1],
                "orderable": false
            }
        ]
    });
}
function GetAllUserApproveExpenseList() {
    var IsApprove = true;
    $('#GetAllUserApprovedExpenseList').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "Post",
            url: '/ExpenseMaster/GetAllApproveExpenseList?Approve=' + IsApprove,
            dataType: 'json'
        },
        columns: [
            {
                "data": null,
                "render": function (data, type, full, meta) {
                    var account = full.account.toLowerCase();
                    if (account === "credit") {
                        return '<div class="avatar-xs"><div class="avatar-title bg-success-subtle text-success rounded-circle fs-16"><i class="ri-arrow-left-down-fill"></i></div></div>';
                    } else if (account === "debit") {
                        return '<div class="avatar-xs"><div class="avatar-title bg-danger-subtle text-danger rounded-circle fs-16"><i class="ri-arrow-right-up-fill"></i></div></div>';
                    } else {
                        return '';
                    }
                },
                "orderable": false
            },
            { "data": "description", "name": "Description" },
            { "data": "billNumber", "name": "BillNumber" },
            {
                "data": "date",
                "name": "Date",
                "render": function (data, type, full, meta) {
                    return new Date(data).toLocaleDateString();
                }
            },
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full, meta) {
                    var color = full.account && full.account.toLowerCase() === "credit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + data + '</span>';
                }
            },
        ],
        columnDefs: [
            {
                "targets": [0, -1],
                "orderable": false
            }
        ]
    });
}

function GetAllUserCreditExpenseList() {
    var Account = "Credit";
    $('#UserallCreditExpenseTable').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "Post",
            url: '/ExpenseMaster/GetAllUserCreditExpenseList?Credit=' + Account,
            dataType: 'json'
        },
        columns: [
            {
                "data": null,
                "render": function (data, type, full, meta) {
                    var account = full.account.toLowerCase();
                    if (account === "credit") {
                        return '<div class="avatar-xs"><div class="avatar-title bg-success-subtle text-success rounded-circle fs-16"><i class="ri-arrow-left-down-fill"></i></div></div>';
                    } else if (account === "debit") {
                        return '<div class="avatar-xs"><div class="avatar-title bg-danger-subtle text-danger rounded-circle fs-16"><i class="ri-arrow-right-up-fill"></i></div></div>';
                    } else {
                        return '';
                    }
                },
                "orderable": false
            },
            { "data": "description", "name": "Description" },
            { "data": "billNumber", "name": "BillNumber" },
            {
                "data": "date",
                "name": "Date",
                "render": function (data, type, full, meta) {
                    return new Date(data).toLocaleDateString();
                }
            },
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full, meta) {

                    var color = full.account.toLowerCase() === "credit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + data + '</span>';
                }
            },
        ],
        columnDefs: [
            {
                "targets": [0, -1],
                "orderable": false
            }
        ]
    });
}