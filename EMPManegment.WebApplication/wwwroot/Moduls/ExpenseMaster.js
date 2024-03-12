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
    console.log("URL:", url); // Log the URL to verify its format
    if (!name) {
        console.error("Parameter name is not provided.");
        return null;
    }
    // Make the parameter name case-insensitive in the regular expression
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)", "i");
    var results = regex.exec(url);
    console.log("Results:", results); // Log the results to see if the parameter is found
    if (!results) {
        console.error("Parameter not found in the URL.");
        return null;
    }
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}



//var isExpenseLoaded = false; // Flag to track if the expense is already loaded

//function LoadApprovedExpense(userId) {
//    debugger
//    // Check if the expense is already loaded
//    if (isExpenseLoaded) {
//        debugger
//        console.log("Expense already loaded.");
//        return;
//    }

//    debugger
//    GetAllUserExpenseList(userId)
//        .then(() => {
//            // Set the flag to true to indicate that the expense is loaded
//            isExpenseLoaded = true;

//            // Append the userId parameter to the URL and navigate
//            window.location.href = '/ExpenseMaster/ApprovedExpense?UserId=' + userId;
//        })
//        .catch(error => {
//            console.error("Error loading expense:", error);
//        });
//}


//function LoadApprovedExpense(userId) {debugger
//    GetAllUserExpenseList(userId)
//}

$(document).ready(function () {

    var userId = GetParameterByName('userId'); // Pass 'userId' as the parameter name
    if (userId) {
        GetAllUserExpenseList(userId); // Pass userId to the function
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
            {
                "data": null,
                "render": function (data, type, full, meta) {
                    return '<div class="form-check"><input class="form-check-input" type="checkbox" name="chk_child" value="option1"></div>';
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



$(document).ready(function () {
    function anyCheckboxChecked() {
        return $("input[name='chk_child']:checked").length > 0;
    }

    $('#UserallExpenseTable').on('change', 'input[name="chk_child"]', function () {
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
            $("#txtTotalAmount").text('₹' + total);

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
            $("#txttotaldebitedamount").text('₹' + Dabitamount);
        },
    });
}


function ApproveExpense(expenseIds) {
    debugger
    $.ajax({
        url: '/ExpenseMaster/ApproveExpense',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(expenseIds),
        success: function (response) {

            console.log(response);
        },
        error: function (xhr, textStatus, errorThrown) {

            console.error(xhr.responseText);
        }
    });
}