function DisplayAddExpenseModel() {
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
            var firstPaymentMethod = result[0];

            $('#txtpaymenttype').val(firstPaymentMethod.id);
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
                        title: "Can't Delete Expense!",
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
            { "data": "description", "name": "Description" },
            { "data": "billNumber", "name": "BillNumber" },
            {
                "data": "date",
                "name": "Date",
                "render": function (data, type, full, meta) {
                    return new Date(data).toLocaleDateString();
                }
            },
            { "data": "totalAmount", "name": "TotalAmount" },
            { "data": "account", "name": "Account" },
            {
                "data": null,
                "name": "Action",
                "render": function (data, type, full, meta) {
                    return '<ul class="list-inline hstack gap-2 mb-0">' +
                        '<li class="list-inline-item" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="View">' +
                        '<a class="text-primary d-inline-block" href="/Invoice/InvoiceDetails">' +
                        '<i class="ri-eye-fill fs-16"></i>' +
                        '</a>' +
                        '</li>' +
                        '<li class="list-inline-item edit" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="Edit">' +
                        '<a onclick="EditExpenseDetails(\'' + full.Id + '\')" data-bs-toggle="modal" class="text-primary d-inline-block edit-item-btn">' +
                        '<i class="ri-pencil-fill fs-16"></i>' +
                        '</a>' +
                        '</li>' +
                        '</ul>';
                }
            }
        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all",
        }]
    });
}

function GetParameterByName(name, url) {

    if (!url) url = window.location.href;
    console.log("URL:", url);
    if (!name) {
        console.error("Parameter name is not provided.");
        return null;
    }

    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)", "i");
    var results = regex.exec(url);
    console.log("Results:", results);
    if (!results) {
        console.error("Parameter not found in the URL.");
        return null;
    }
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

$(document).ready(function () {

    var userId = GetParameterByName('userId');
    if (userId) {
        GetAllUserExpenseList(userId);
    }

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
                    var imageSrc = full.image ? '<img src="/' + full.image + '" class="avatar-xxs rounded-circle image_src object-fit-cover">' : '';
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
            { "data": "totalAmount", "name": "TotalAmount" },
        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all",
        }]
    });
});


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
                "data": "billNumber", "name": "BillNumber",
                "className": "text-center"
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
                if (obj.account == "Dabit") {
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

$(document).ready(function () {

    var UserId = $("#txtuserid").val();

    if (UserId) {
        GetAllUserExpenseList(UserId);
        UserDebitExpenseList(UserId);
        UserCreditExpenseList(UserId);
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
    var Account = "Dabit";
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