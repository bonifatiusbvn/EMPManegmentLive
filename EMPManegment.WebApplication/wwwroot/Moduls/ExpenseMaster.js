function DisplayAddExpenseModel() {
    clearText();
    $('#AddExpenseModel').modal('show');
}

$(document).ready(function () {
    GetExpenseTypeList();
    GetPaymentTypeList();
    GetExpenseTotalAmount();
    GetAllUserExpenseList();
    ApprovedExpenseList();
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
    $('#txtexpensetype').select2({
        theme: 'bootstrap4',
        width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
        placeholder: $(this).data('placeholder'),
        allowClear: Boolean($(this).data('allow-clear')),
        dropdownParent: $("#AddExpenseModel")
    });
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
    if (ExpenseTypeForm) {
        ExpenseTypeForm.resetForm();
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
            $('#txtexpensetype').empty().append('<option selected disabled value="">---Select Expense Type---</option>');
            $('#Editexpensetype').empty().append('<option selected disabled value="">---Select Expense Type---</option>');
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
            title: "Kindly fill all datafield",
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
            title: "Kindly fill all datafield",
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

            $('#Editexpensetype').select2({
                theme: 'bootstrap4',
                width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
                placeholder: $(this).data('placeholder'),
                allowClear: Boolean($(this).data('allow-clear')),
                dropdownParent: $("#EditExpenseModel")
            });

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
            title: "Kindly fill all details",
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
            title: "Kindly fill all details",
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
            EditDescription: "Please enter description",
            Editbillno: "Please enter bill no",
            Edittotalamount: "please enter correct total amount",
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
                        window.location = '/ExpenseMaster/ExpenseList';
                    })
                },
                error: function () {
                    Swal.fire({
                        title: "Can't delete expense!",
                        icon: 'warning',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/ExpenseMaster/ExpenseList';
                    })
                }
            })
        } else if (result.dismiss === Swal.DismissReason.cancel) {

            Swal.fire(
                'Cancelled',
                'Expense have no changes.!!😊',
                'error'
            );
        }
    });
}

function GetPayUserExpenseCreditList(userId) {
    var filterType = "credit";
    $('#UserPayExpenseTableCredit').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        searching: false,
        info: false,
        lengthChange: false,
        ajax: {
            type: "POST",
            url: '/ExpenseMaster/GetUserExpenseList?UserId=' + userId + '&filterType=' + filterType,
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
                    return getCommonDateformat(data);
                }
            },
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full, meta) {
                    var color = full.account.toLowerCase() === "debit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + '₹' + data + '</span>';
                }
            },
            { "data": "account", "name": "Account", "visible": false },
        ],
        scrollX: true,
        scrollCollapse: true,
        fixedHeader: {
            header: true,
            footer: true
        },
        autoWidth: false,
        columnDefs: [{
            targets: [0],
            orderable: false,
            width: "auto"
        }],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;


            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                        i : 0;
            };


            total = api
                .column(4)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);


            $(api.column(4).footer()).html(
                '<span style="color: black;">Total: ' + '₹' + total + '</span>'
            );
        }
    });
}
function GetPayUserExpenseDebitList(userId) {
    var filtertype = "debit";
    $('#UserPayExpenseTableDebit').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        searching: false,
        info: false,
        lengthChange: false,
        ajax: {
            type: "POST",
            url: '/ExpenseMaster/GetUserExpenseList?UserId=' + userId + '&filterType=' + filtertype,
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
                    return getCommonDateformat(data);
                }
            },
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full, meta) {
                    var color = full.account.toLowerCase() === "credit" ? "red" : "green";
                    return '<span style="color: ' + color + ';">' + '₹' + data + '</span>';
                }
            },
            { "data": "account", "name": "Account", "visible": false },
        ],
        scrollX: true,
        scrollCollapse: true,
        fixedHeader: {
            header: true,
            footer: true
        },
        autoWidth: false,
        columnDefs: [{
            targets: [0],
            orderable: false,
            width: "auto"
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
                '<span style="color: black;">Total: ' + '₹' + total + '</span>'
            );
        }
    });
}

function UserExpensesDetails() {
    $('#UserListTable').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        order: [[4, 'asc']],
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
                    var imageSrc;
                    if (full.image && full.image.trim() !== '') {
                        imageSrc = '<img src="/' + full.image + '" style="height: 40px; width: 40px; border-radius: 50%;" ' +
                            'onmouseover="showIcons(event, this.parentElement)" onmouseout="hideIcons(event, this.parentElement)">';
                    } else {
                        var initials = (full.firstName ? full.firstName[0] : '') + (full.lastName ? full.lastName[0] : '');
                        imageSrc = '<div class="flex-shrink-0 avatar-xs me-2">' +
                            '<div class="avatar-title bg-success-subtle text-success rounded-circle fs-13" style="height: 40px; width: 40px; border-radius: 50%;">' + initials.toUpperCase() + '</div></div>';
                    }
                    return '<div class="d-flex align-items-center">' +
                        '<div class="flex-shrink-0">' +
                        imageSrc +
                        '</div>' +
                        '<div class="flex-grow-1 ms-2 name">' +
                        '<h5 class="fs-15"><a href="/ExpenseMaster/ApprovedExpense?UserId=' + full.userId + '&UserName=' + full.fullName + '" class="fw-medium link-primary view-details" data-userid="' + full.userId + '">' + data + '</a></h5>' +
                        '</div>' +
                        '</div>';
                }
            },
            { "data": "userName", "name": "UserName" },
            {
                "data": "date",
                "name": "Date",
                "render": function (data, type, full, meta) {
                    return getCommonDateformat(data);
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
$(document).ready(function () {
    $('.nav-link').click(function () {
        var targetTab = $(this).attr('href');

        if (targetTab === 'AllExpenseList') {

            GetAllUserExpenseList();
        } else if (targetTab === 'UnapproveExpenseList') {
            GetAllUserUnapproveExpense();
        } else if (targetTab === 'ApproveExpenseList') {
            GetAllUserApproveExpense();
        }
    });
});
function GetAllUserExpenseList() {
    $.ajax({
        url: '/ExpenseMaster/DisplayExpenseList',
        type: 'GET',
        success: function (result) {
            $("#AllUserExpensePartial").html(result);
            DisplayAllUserExpenseList();
        },
        error: function () {
            alert('Error loading expenses. Please try again.');
        }
    });
}
function GetAllUserUnapproveExpense() {
    $.ajax({
        url: '/ExpenseMaster/DisplayExpenseList',
        type: 'GET',
        success: function (result) {
            $("#AllUserUnapproveExpensePartial").html(result);
            DisplayUnApprovedExpenseList();
        },
        error: function () {
            alert('Error loading expenses. Please try again.');
        }
    });
}
function GetAllUserApproveExpense() {

    $.ajax({
        url: '/ExpenseMaster/DisplayExpenseList',
        type: 'GET',
        success: function (result) {

            $("#AllUserExpensePartial").html(result);
            DisplayAllApprovedExpenseList();
        },
        error: function () {
            alert('Error loading expenses. Please try again.');
        }
    });
}

function DisplayAllUserExpenseList() {
    var userId = GetParameterByName('userId');
    $('#UserallExpenseTable').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        destroy: true,
        order: [[7, 'desc']],
        ajax: {
            url: '/ExpenseMaster/GetUserExpenseList',
            type: 'POST',
            data: function (d) {
                d.UserId = userId;
            }
        },
        columns: [
            {
                "data": null, "visible": false,
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
                    return getCommonDateformat(data);
                }
            },
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full, meta) {
                    var color = full.account && full.account.toLowerCase() === "credit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + '₹' + data + '</span>';
                }
            },
            { "data": "account", "name": "Account", "visible": false }
        ],
        scrollY: 400,
        scrollX: true,
        scrollCollapse: true,
        fixedHeader: {
            header: true,
            footer: true
        },
        autoWidth: false,
        columnDefs: [
            {
                defaultContent: "",
                targets: "_all",
                width: 'auto'
            }
        ]
    });
}

function DisplayUnApprovedExpenseList() {
    var UserId = GetParameterByName('userId');
    var unapprove = false;
    $('#UserallExpenseTable').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        destroy: true,
        order: [[7, 'desc']],
        ajax: {
            type: "POST",
            url: '/ExpenseMaster/GetUserExpenseList',
            data: function (d) {
                d.UserId = UserId;
                d.unapprove = unapprove;
            },
            dataType: 'json'
        },
        columns: [
            {
                "data": null,
                "render": function (data, type, full, meta) {
                    return '<div class="form-check"><input class="form-check-input" data-id="' + full.id + '" type="checkbox" name="chk_child"></div>';
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
                    return getCommonDateformat(data);
                }
            },
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full, meta) {
                    var color = full.account && full.account.toLowerCase() === "credit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + '₹' + data + '</span>';
                }
            },
            { "data": "account", "name": "Account", "visible": false },
        ],
        scrollY: 400,
        scrollX: true,
        scrollCollapse: true,
        fixedHeader: {
            header: true,
            footer: true
        },
        autoWidth: false,
        columnDefs: [
            {
                defaultContent: "",
                targets: "_all",
                width: 'auto'
            }
        ],
        drawCallback: function () {

            updateCheckedAllState();
        }
    });
}

function DisplayAllApprovedExpenseList() {

    var UserId = GetParameterByName('userId');
    var approve = true;
    $('#UserallExpenseTable').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        order: [[7, 'desc']],
        ajax: {
            type: "POST",
            url: '/ExpenseMaster/GetUserExpenseList?UserId=' + UserId + '&approve=' + approve,
            dataType: 'json',
        },
        columns: [
            {
                "data": null, "visible": false,
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
                    return getCommonDateformat(data);
                }
            },
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full, meta) {
                    var color = full.account && full.account.toLowerCase() === "credit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + '₹' + data + '</span>';
                }
            },
            { "data": "account", "name": "Account", "visible": false },
        ],
        scrollY: 400,
        scrollX: true,
        scrollCollapse: true,
        fixedHeader: {
            header: true,
            footer: true
        },
        autoWidth: false,
        columnDefs: [
            {
                defaultContent: "",
                targets: "_all",
                width: 'auto'
            }
        ]
    });
}


var datas = userPermissions
$(document).ready(function () {
    function data(datas) {
        var userPermission = datas;
        DisplayAllExpenseList(userPermission);
    }
    function DisplayAllExpenseList(userPermission) {
        var userPermissionArray = [];
        userPermissionArray = JSON.parse(userPermission);

        var canEdit = false;
        var canDelete = false;

        for (var i = 0; i < userPermissionArray.length; i++) {
            var permission = userPermissionArray[i];
            if (permission.formName == "All Expenses List") {
                canEdit = permission.edit;
                canDelete = permission.delete;
                break;
            }
        }
        var columns = [
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
                    if (full.image != null) {
                        return '<div class="d-flex">' +
                            '<div class="flex-grow-1 tasks_name">' + full['billNumber'] + '</div>' +
                            '<div class="flex-shrink-0 ms-4 task-icons">' +
                            '<ul class="list-inline tasks-list-menu mb-0">' +

                            '<li class="list-inline-item"><a href="/ExpenseMaster/DownloadBill/?BillName=' + full.image + '"><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="currentColor" d="M13 12h3l-4 4l-4-4h3V8h2v4Zm2-8H5v16h14V8h-4V4ZM3 2.992C3 2.444 3.447 2 3.999 2H16l5 5v13.993A1 1 0 0 1 20.007 22H3.993A1 1 0 0 1 3 21.008V2.992Z" /></svg></a></li>'; +
                                '</ul>' +
                                '</div>' +
                                '</div>';
                    } else {
                        return '<div class="d-flex">' +
                            '<div class="flex-grow-1 tasks_name">' + full['billNumber'] + '</div>' +
                            '<div class="flex-shrink-0 ms-4 task-icons">' +
                            '<ul class="list-inline tasks-list-menu mb-0">' +
                            '</div>' +
                            '</div>';
                    }

                }
            },
            {
                "data": "date",
                "name": "Date",
                "className": "text-center",

                "render": function (data, type, full, meta) {
                    return getCommonDateformat(data);
                }
            },
            {
                "data": "totalAmount", "name": "TotalAmount",
                "className": "text-center",
                "render": function (data, type, full, meta) {
                    var color = full.account && full.account.toLowerCase() === "credit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + '₹' + data + '</span>';
                }
            },
            {
                "data": "account", "name": "Account",
                "className": "text-center"
            }
        ];

        if (canEdit || canDelete) {
            columns.push({
                "data": null,
                "orderable": false,
                "searchable": false,
                "render": function (data, type, full) {

                    var buttons = '';

                    if (canEdit) {
                        buttons += '<a class="btn text-primary" onclick="EditExpenseDetails(\'' + full.id + '\')">' +
                            '<i class="fa-regular fa-pen-to-square"></i></a>';
                    }

                    if (canDelete) {
                        buttons += '<a class="btn text-danger" onclick="deleteExpense(\'' + full.id + '\')">' +
                            '<i class="fas fa-trash"></i></a>';
                    }
                    return buttons;
                }

            });
        }

        $('#ExpenseTable').DataTable({
            processing: false,
            serverSide: true,
            filter: true,
            "bDestroy": true,
            order: [[3, 'desc']],
            ajax: {
                type: "POST",
                url: '/ExpenseMaster/GetExpenseDetailsList',
                dataType: 'json',
            },
            columns: columns,
            scrollY: 400,
            scrollX: true,
            scrollCollapse: true,
            fixedHeader: {
                header: true,
                footer: true
            },
            autoWidth: false,
            columnDefs: [{
                "defaultContent": "",
                "targets": "_all",
            }]
        });
    }
    data(datas);

});

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

            var total = 0;
            result.forEach(function (obj) {
                if (obj.totalAmount) {
                    total += obj.totalAmount;
                }
            });
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
    var userName = $('#txtgetUserName').val();
    Swal.fire({
        title: "Are you sure want to approve this?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, approve it!",
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
                                window.location = '/ExpenseMaster/ApprovedExpense?UserId=' + userId + '&UserName=' + userName;
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
                'You have no changes.!!😊',
                'error'
            );
        }
    });
}

function ApprovedExpenseList() {
    var UserId = $("#txtuserid").val();
    var approve = true;
    $('#GetUserApprovedExpenseList').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "POST",
            url: '/ExpenseMaster/GetUserExpenseList?UserId=' + UserId + '&approve=' + approve,
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
                    return getCommonDateformat(data);
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
        GetPayUserExpenseCreditList(UserId)
        GetPayUserExpenseDebitList(UserId)
    }
});
function UserDebitExpenseList(UserId) {
    var Account = "Debit";
    var filterType = 'debit';
    $('#UserallUnApprovedExpenseTable').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        pageLength: 30,
        ajax: {
            type: "POST",
            url: '/ExpenseMaster/GetUserExpenseList?UserId=' + UserId + '&account=' + Account + '&filterType=' + filterType,
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
                    return getCommonDateformat(data);
                }
            },
            { "data": "totalAmount", "name": "TotalAmount" },
            { "data": "account", "name": "Account" },
        ],
        scrollY: 400,
        scrollX: true,
        scrollCollapse: true,
        fixedHeader: {
            header: true,
            footer: true
        },
        autoWidth: false,
        columnDefs: [{
            defaultContent: "",
            targets: "_all",
            width: 'auto'
        }]
    });
}
function UserCreditExpenseList(UserId) {
    var Account = "Credit";
    var filterType = 'credit';
    $('#GetUserApprovedExpenseList').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "POST",
            url: '/ExpenseMaster/GetUserExpenseList?UserId=' + UserId + '&account=' + Account + '&filterType=' + filterType,
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
                    return getCommonDateformat(data);
                }
            },
            { "data": "totalAmount", "name": "TotalAmount" },
            { "data": "account", "name": "Account" },
        ],
        scrollY: 400,
        scrollX: true,
        scrollCollapse: true,
        fixedHeader: {
            header: true,
            footer: true
        },
        autoWidth: false,
        columnDefs: [{
            defaultContent: "",
            targets: "_all",
            width: 'auto'
        }]
    });
}

var datas = userPermissions
$(document).ready(function () {
    GetMyExpenseList();
    $('.nav-link').click(function () {
        var targetTab = $(this).attr('href');
        if (targetTab === '#GetMyAllExpenseList') {
            GetMyExpenseList();
        } else if (targetTab === '#GetAllUnApprovedExpenseList') {
            GetMyUnapproveExpenseList(datas);
        } else if (targetTab === '#GetAllApprovedExpenseList') {
            GetMyApproveExpenseList();
        } else if (targetTab === '#GetAllCreditExpenseList') {
            GetMyCreditExpenseList();
        }
    });

    $('.nav-radio').click(function () {
        var targetTab = $(this).attr('href');
        if (targetTab === '#GetUserLastMonthExpenseList') {
            GetMyLastMonthExpenseList();
        } else if (targetTab === '#GetUserCurrentMonthExpenseList') {
            GetMyCurrentMonthExpenseList();
        }
    });

    $('.nav-btn').click(function () {
        var targetTab = $(this).attr('href');
        if (targetTab === '#GetBetweendatesExpenseList') {
            GetMyBetweenDateExpenseList();
        }
    });
    function GetMyUnapproveExpenseList(datas) {
        $.ajax({
            url: '/ExpenseMaster/DisplayUserExpenseDetails',
            type: 'GET',
            success: function (result) {

                $("#UserExpenseListPartial").html(result);
                GetAllUserUnapproveExpenseList(datas);
            },
            error: function () {
                alert('Error loading expenses. Please try again.');
            }
        });
    }
    function GetAllUserUnapproveExpenseList(datas) {
        var UserId = $("#txtuserid").val();
        var IsApprove = false;

        var userPermissionArray = JSON.parse(datas);
        var canEdit = false;
        var canDelete = false;

        for (var i = 0; i < userPermissionArray.length; i++) {
            var permission = userPermissionArray[i];
            if (permission.formName == "Expenses") {
                canEdit = permission.edit;
                canDelete = permission.delete;
                break;
            }
        }
        var columns = [
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
                    return getCommonDateformat(data);
                }
            },
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full, meta) {
                    var color = full.account && full.account.toLowerCase() === "credit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + '₹' + data + '</span>';
                }
            },
        ];
        if (canEdit || canDelete) {
            columns.push({
                "data": null,
                "render": function (data, type, full) {
                    var buttons = '';

                    if (canEdit) {
                        buttons +=
                            '<a class="btn text-primary" onclick="EditExpenseDetails(\'' + full.id + '\')">' +
                            '<i class="fa-regular fa-pen-to-square"></i></a>';
                    }

                    if (canDelete) {
                        buttons += '<a class="btn text-danger btndeletedoc" onclick="deleteExpense(\'' + full.id + '\')">' +
                            '<i class="fas fa-trash"></i></a>';
                    }

                    return buttons;
                }
            });
        }
        debugger
        $('#GetMyUnapproveExpense').DataTable({
            processing: false,
            serverSide: true,
            filter: true,
            "bDestroy": true,
            order: [[3, 'asc']],
            pageLength: 30,
            ajax: {
                type: "Post",
                url: '/ExpenseMaster/GetUserExpenseList?unapprove=' + IsApprove + "&UserId=" + UserId,
                dataType: 'json'
            },
            columns: columns,
            scrollY: 400,
            scrollX: true,
            scrollCollapse: true,
            fixedHeader: {
                header: true,
                footer: true
            },
            autoWidth: false,
            columnDefs: [{
                targets: [0],
                orderable: false,
                width: "auto"
            }],
            footerCallback: function (row, data, start, end, display) {
                var api = this.api();

                var intVal = function (i) {
                    return typeof i === 'string' ?
                        i.replace(/[\$,]/g, '') * 1 :
                        typeof i === 'number' ?
                            i : 0;
                };

                var total = api
                    .column(4)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);

                $(api.column(5).footer()).html(
                    '<span style="color: black;">Total: ' + '₹' + total + '</span>'
                );
            }
        });
    }
});
function GetMyExpenseList() {
    $.ajax({
        url: '/ExpenseMaster/DisplayUserExpenseList',
        type: 'GET',
        success: function (result) {
            $("#UserExpenseListPartial").html(result);
            GetUserAllExpenseDetails();
        },
        error: function () {
            alert('Error loading expenses. Please try again.');
        }
    });
}

function GetUserAllExpenseDetails() {
    var UserId = $("#txtuserid").val();
    $('#DisplayUserAllExpenseList').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        order: [[3, 'asc']],
        pageLength: 30,
        ajax: {
            type: "Post",
            url: '/ExpenseMaster/GetUserExpenseList?UserId=' + UserId,
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
                    return getCommonDateformat(data);
                }
            },
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full, meta) {

                    var color = full.account.toLowerCase() === "credit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + '₹' + data + '</span>';
                }
            },
        ],
        scrollY: 400,
        scrollX: true,
        scrollCollapse: true,
        fixedHeader: {
            header: true,
            footer: true
        },
        autoWidth: false,
        columnDefs: [{
            targets: [0],
            orderable: false,
            width: "auto"
        }],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;

            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                        i : 0;
            };

            total = api
                .column(4)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            $(api.column(4).footer()).html(
                '<span style="color: black;">Total: ' + '₹' + total + '</span>'
            );
        }
    });
}
function GetMyApproveExpenseList() {
    $.ajax({
        url: '/ExpenseMaster/DisplayUserExpenseList',
        type: 'GET',
        success: function (result) {

            $("#UserExpenseListPartial").html(result);
            GetAllUserApproveExpenseList();
        },
        error: function () {
            alert('Error loading expenses. Please try again.');
        }
    });
}
function GetAllUserApproveExpenseList() {
    var UserId = $("#txtuserid").val();
    var IsApprove = true;
    $('#DisplayUserAllExpenseList').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        order: [[3, 'asc']],
        pageLength: 30,
        ajax: {
            type: "Post",
            url: '/ExpenseMaster/GetUserExpenseList?approve=' + IsApprove + "&UserId=" + UserId,
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
                    return getCommonDateformat(data);
                }
            },
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full, meta) {
                    var color = full.account && full.account.toLowerCase() === "credit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + '₹' + data + '</span>';
                }
            },
        ],
        scrollY: 400,
        scrollX: true,
        scrollCollapse: true,
        fixedHeader: {
            header: true,
            footer: true
        },
        autoWidth: false,
        columnDefs: [{
            targets: [0],
            orderable: false,
            width: "auto"
        }],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;

            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                        i : 0;
            };

            total = api
                .column(4)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            $(api.column(4).footer()).html(
                '<span style="color: black;">Total: ' + '₹' + total + '</span>'
            );
        }
    });
}
function GetMyCreditExpenseList() {
    $.ajax({
        url: '/ExpenseMaster/DisplayUserExpenseList',
        type: 'GET',
        success: function (result) {

            $("#UserExpenseListPartial").html(result);
            GetAllUserCreditExpenseList();
        },
        error: function () {
            alert('Error loading expenses. Please try again.');
        }
    });
}
function GetAllUserCreditExpenseList() {
    var UserId = $("#txtuserid").val();
    var Account = "Credit";
    var filterType = 'credit';

    $('#DisplayUserAllExpenseList').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        order: [[3, 'asc']],
        pageLength: 30,
        ajax: {
            type: "Post",
            url: '/ExpenseMaster/GetUserExpenseList?Credit=' + Account + "&UserId=" + UserId + '&filterType=' + filterType,
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
                    return getCommonDateformat(data);
                }
            },
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full, meta) {

                    var color = full.account.toLowerCase() === "credit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + '₹' + data + '</span>';
                }
            },
        ],
        scrollY: 400,
        scrollX: true,
        scrollCollapse: true,
        fixedHeader: {
            header: true,
            footer: true
        },
        autoWidth: false,
        columnDefs: [{
            targets: [0],
            orderable: false,
            width: "auto"
        }],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;

            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                        i : 0;
            };

            total = api
                .column(4)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            $(api.column(4).footer()).html(
                '<span style="color: black;">Total: ' + '₹' + total + '</span>'
            );
        }
    });
}
function GetMyLastMonthExpenseList() {
    $.ajax({
        url: '/ExpenseMaster/DisplayUserExpenseList',
        type: 'GET',
        success: function (result) {

            $("#UserExpenseListPartial").html(result);
            GetUserLastMonthExpenseList();
        },
        error: function () {
            alert('Error loading expenses. Please try again.');
        }
    });
}
function GetUserLastMonthExpenseList() {
    var filterType = "lastmonth";
    var UserId = $("#txtuserid").val();
    $('#DisplayUserAllExpenseList').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        order: [[3, 'asc']],
        searching: false,
        info: false,
        lengthChange: false,
        ajax: {
            type: "Post",
            url: '/ExpenseMaster/GetUserExpenseList?UserId=' + UserId + '&filterType=' + filterType,
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
                    return getCommonDateformat(data);
                }
            },
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full, meta) {
                    var color = full.account && full.account.toLowerCase() === "credit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + '₹' + data + '</span>';
                }
            },
        ],
        scrollY: 400,
        scrollX: true,
        scrollCollapse: true,
        fixedHeader: {
            header: true,
            footer: true
        },
        autoWidth: false,
        columnDefs: [{
            targets: [0],
            orderable: false,
            width: "auto"
        }],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;

            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                        i : 0;
            };

            total = api
                .column(4)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            $(api.column(4).footer()).html(
                '<span style="color: black;">Total: ' + '₹' + total + '</span>'
            );
        }
    });
}
function GetMyCurrentMonthExpenseList() {
    $.ajax({
        url: '/ExpenseMaster/DisplayUserExpenseList',
        type: 'GET',
        success: function (result) {

            $("#UserExpenseListPartial").html(result);
            GetUserCurrentMonthExpenseList();
        },
        error: function () {
            alert('Error loading expenses. Please try again.');
        }
    });
}
function GetUserCurrentMonthExpenseList() {
    var filterType = 'thismonth';
    var UserId = $("#txtuserid").val();
    $('#DisplayUserAllExpenseList').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        order: [[3, 'asc']],
        searching: false,
        info: false,
        lengthChange: false,
        ajax: {
            type: "Post",
            url: '/ExpenseMaster/GetUserExpenseList?UserId=' + UserId + '&filterType=' + filterType,
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
                    return getCommonDateformat(data);
                }
            },
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full, meta) {
                    var color = full.account.toLowerCase() === "debit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + '₹' + data + '</span>';
                }
            },
        ],
        scrollY: 400,
        scrollX: true,
        scrollCollapse: true,
        fixedHeader: {
            header: true,
            footer: true
        },
        autoWidth: false,
        columnDefs: [{
            targets: [0],
            orderable: false,
            width: "auto"
        }],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;

            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                        i : 0;
            };

            total = api
                .column(4)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            $(api.column(4).footer()).html(
                '<span style="color: black;">Total: ' + '₹' + total + '</span>'
            );
        }
    });
}
function GetMyBetweenDateExpenseList() {
    $.ajax({
        url: '/ExpenseMaster/DisplayUserExpenseList',
        type: 'GET',
        success: function (result) {

            $("#UserExpenseListPartial").html(result);
            SearchBetweenDateExpense();
        },
        error: function () {
            alert('Error loading expenses. Please try again.');
        }
    });
}
function SearchBetweenDateExpense() {
    var StartDate = $('#startDate').val();
    var EndDate = $('#endDate').val();
    var UserId = $('#txtuserid').val();

    if (StartDate == "" && EndDate == "") {
        toastr.error("Select dates");
    } else if (StartDate == "") {
        toastr.error("Select Satrtdate");
    } else if (EndDate == "") {
        toastr.error("Select Enddate");
    } else {
        GetUserBetweenMonthsExpenseList(StartDate, EndDate, UserId)
    }

}
function GetUserBetweenMonthsExpenseList(StartDate, EndDate, UserId) {
    var filterType = 'daterange';
    $('#DisplayUserAllExpenseList').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        order: [[3, 'asc']],
        searching: false,
        info: false,
        lengthChange: false,
        ajax: {
            type: "Post",
            url: '/ExpenseMaster/GetUserExpenseList?startDate=' + StartDate + '&endDate=' + EndDate + '&UserId=' + UserId + '&filterType=' + filterType,
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
                    return getCommonDateformat(data);
                }
            },
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full, meta) {
                    var color = full.account && full.account.toLowerCase() === "credit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + '₹' + data + '</span>';
                }
            },
        ],
        scrollY: 400,
        scrollX: true,
        scrollCollapse: true,
        fixedHeader: {
            header: true,
            footer: true
        },
        autoWidth: false,
        columnDefs: [{
            targets: [0],
            orderable: false,
            width: "auto"
        }],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;

            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                        i : 0;
            };

            total = api
                .column(4)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            $(api.column(4).footer()).html(
                '<span style="color: black;">Total: ' + '₹' + total + '</span>'
            );
        }
    });
}

var ExpenseTypeForm;
$(document).ready(function () {
    ExpenseTypeForm = $("#addExpenseType").validate({
        rules: {
            textExpenseType: "required",
        },
        messages: {
            textExpenseType: "Please Enter Expense Type",
        }
    })
});

function DisplayExpenseTypeModal() {
    clearExpenseTypeText();
    $('#ExpenseTypeModal').modal('show');
}

function clearExpenseTypeText() {
    resetForm();
    $("#textExpenseType").val('');
}
function AddExpenseType() {
    if ($("#addExpenseType").valid()) {

        var formData = new FormData();
        formData.append("Type", $("#textExpenseType").val());

        $.ajax({
            url: '/ExpenseMaster/AddExpenseType',
            type: 'Post',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (Result) {
                if (Result.code == 200) {
                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        $('#ExpenseTypeModal').modal('hide');
                        GetExpenseTypeList();
                    });
                }
                else {
                    Swal.fire({
                        title: Result.message,
                        icon: 'warning',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    });
                }
            }
        });
    }
    else {
        Swal.fire({
            title: "Kindly fill expense type",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        })
    }
}

