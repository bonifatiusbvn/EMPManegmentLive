function DisplayAddExpenseModel() {
    clearText();
    $('#AddExpenseModel').modal('show');
}

$(document).ready(function () {
    GetExpenseTotalAmount();
    GetAllUserExpenseList();
    ApprovedExpenseList();
    UserExpensesDetails();
    GetExpenseTypeList();
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
        method: 'GET',
        success: function (result) {
            var expenseTypes = result.map(function (data) {
                return {
                    label: data.type,
                    value: data.id
                };
            });
            expenseTypes.sort(function (a, b) {
                if (a.label < b.label) {
                    return -1;
                }
                if (a.label > b.label) {
                    return 1;
                }
                return 0;
            });
            function setupAutocomplete(inputId, hiddenId) {
                $(inputId).autocomplete({
                    source: expenseTypes,
                    minLength: 0,
                    focus: function (event, ui) {
                        event.preventDefault();

                    },
                    select: function (event, ui) {
                        $(inputId).val(ui.item.label);
                        $(hiddenId).val(ui.item.value);
                        event.preventDefault();
                        return false;
                    }
                }).focus(function () {
                    $(this).autocomplete("search");
                });
            }

            setupAutocomplete("#txtexpensetype", "#txtexpensetypeHidden");
            setupAutocomplete("#Editexpensetype", "#EditexpensetypeHidden");
        },
        error: function (err) {
            toastr.error("Failed to fetch expense types: ", err);
        }
    });
}
$(document).ready(function () {
    function GetUsersList() {
        $.ajax({
            url: '/Task/GetUserName',
            method: 'GET',
            success: function (result) {
                var UserList = result.map(function (data) {
                    return {
                        label: data.firstName + ' ' + data.lastName + ' (' + data.userName + ')',
                        value: data.id
                    };
                });

                function setupAutocomplete(inputId, hiddenId) {
                    $(inputId).autocomplete({
                        source: UserList,
                        minLength: 0,
                        focus: function (event, ui) {
                            event.preventDefault();
                            $(inputId).val(ui.item.label);
                        },
                        select: function (event, ui) {
                            $(inputId).val(ui.item.label);
                            $(hiddenId).val(ui.item.value);
                            event.preventDefault();
                            return false;
                        }
                    }).focus(function () {
                        $(this).autocomplete("search");
                    });
                }

                setupAutocomplete("#txtExpenseUsername", "#txtExpenseUsernameHidden");
                setupAutocomplete("#EditExpenseUserName", "#EditExpenseUserNameHidden");
            },
            error: function (err) {
                toastr.error("Failed to fetch User List: ", err);
            }
        });
    }

    GetUsersList();
});

function SelectExpenseTypeId() {
    document.getElementById("txtexpensetypeid").value = document.getElementById("txtexpensetype").value;
    document.getElementById("Editexpensetypeid").value = document.getElementById("Editexpensetype").value;
}
function SelectPaymentTypeId() {
    document.getElementById("txtpaymenttypeid").value = document.getElementById("txtExpensepaymenttype").value;
    document.getElementById("Editpaymenttypeid").value = document.getElementById("EditExpensepaymenttype").value;
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

function AddMyExpenseDetails() {
    if ($('#userexpenseform').valid()) {
        var formData = new FormData();
        formData.append("ExpenseType", $("#txtexpensetypeHidden").val());
        formData.append("Description", $("#txtDescription").val());
        formData.append("BillNumber", $("#txtbillno").val());
        formData.append("Date", $("#txtdate").val());
        formData.append("Account", $("#txtaccount").val());
        formData.append("TotalAmount", $("#txttotalamount").val());
        formData.append("UserId", $("#txtExpenseUserId").val());
        formData.append("Image", $("#txtimage")[0].files[0]);
        $.ajax({
            url: '/ExpenseMaster/AddexpenseDetails',
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
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/ExpenseMaster/MyExpense';
                    });
                } else {
                    toastr.error(Result.message);
                }
            }
        })
    }
    else {
        toastr.warning("Kindly fill all datafield");
    }
}
function AddAllUserExpenseDetails() {
    if ($('#formexpensedetails').valid()) {
        var formData = new FormData();
        formData.append("ExpenseType", $("#txtexpensetypeHidden").val());
        formData.append("Description", $("#txtDescription").val());
        formData.append("BillNumber", $("#txtbillno").val());
        formData.append("Date", $("#txtdate").val());
        formData.append("Account", $("#txtaccount").val());
        formData.append("TotalAmount", $("#txttotalamount").val());
        formData.append("Image", $("#txtimage")[0].files[0]);
        formData.append("UserId", $("#txtExpenseUsernameHidden").val());
        $.ajax({
            url: '/ExpenseMaster/AddexpenseDetails',
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
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/ExpenseMaster/AllExpense';
                    });
                } else {
                    toastr.error(Result.message);
                }
            }
        })
    }

    else {
        toastr.warning("Kindly fill all datafield");
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
            $('#Editexpensetype').val(response.expenseTypeName);
            $('#EditexpensetypeHidden').val(response.expenseType);
            $('#Editid').val(response.id);
            $('#EditDescription').val(response.description);
            $('#Editbillno').val(response.billNumber);
            $('#Editdate').val(response.date);
            $('#Edittotalamount').val(response.totalAmount);
            $('#Editaccount').val(response.account);
            $('#txtExpensepaymenttype').val(response.paymentTypeName);
            $('#EditExpensepaymenttypeid').val(response.paymentType);
            $('#EditIsPaid').val(response.isPaid ? "True" : "False");
            $('#EditIsApproved').val(response.isApproved ? "True" : "False");
        },
        error: function () {
            toastr.error("Data not found");
        }
    });
}

function UpdateExpenseDetails() {
    if ($('#EditExpenseForm').valid()) {
        var formData = new FormData();
        formData.append("Id", $("#Editid").val());
        formData.append("ExpenseType", $("#EditexpensetypeHidden").val());
        formData.append("Description", $("#EditDescription").val());
        formData.append("BillNumber", $("#Editbillno").val());
        formData.append("Date", $("#Editdate").val());
        formData.append("TotalAmount", $("#Edittotalamount").val());
        formData.append("PaymentType", $("#EditExpensepaymenttypeid").val());
        formData.append("IsPaid", $("#EditIsPaid").val());
        formData.append("IsApproved", $("#EditIsApproved").val());
        formData.append("Account", $("#Editaccount").val());
        formData.append("UserId", $("#txtExpenseUserId").val());

        $.ajax({
            url: '/ExpenseMaster/UpdateExpenseDetails',
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
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/ExpenseMaster/MyExpense';
                    });
                } else {
                    toastr.error(Result.message);
                }
            }
        })
    }
    else {
        toastr.warning("Kindly fill all datafield");
    }
}

function EditAllUserExpenseDetails(Id) {
    resetForm();
    $.ajax({
        url: '/ExpenseMaster/EditExpenseDetails?ExpenseId=' + Id,
        type: "Get",
        contentType: 'application/json;charset=utf-8;',
        dataType: 'json',
        success: function (response) {
            $('#EditExpenseModel').modal('show');
            $('#Editexpensetype').val(response.expenseTypeName);
            $('#EditexpensetypeHidden').val(response.expenseType);
            $('#Editid').val(response.id);
            $('#EditDescription').val(response.description);
            $('#Editbillno').val(response.billNumber);
            $('#Editdate').val(response.date);
            $('#Edittotalamount').val(response.totalAmount);
            $('#Editaccount').val(response.account);
            $('#EditExpensepaymenttype').val(response.paymentType);
            $('#EditExpenseUserName').val(response.fullName);
            $('#EditExpenseUserNameHidden').val(response.userId);
            $('#EditIsPaid').val(response.isPaid ? "True" : "False");
            $('#EditIsApproved').val(response.isApproved ? "True" : "False");
        },
        error: function () {
            toastr.error("Data not found");
        }
    });
}
function UpdateExpenseListDetails() {
    if ($('#EditExpenseForm').valid()) {
        var formData = new FormData();
        formData.append("Id", $("#Editid").val());
        formData.append("ExpenseType", $("#EditexpensetypeHidden").val());
        formData.append("Description", $("#EditDescription").val());
        formData.append("BillNumber", $("#Editbillno").val());
        formData.append("Date", $("#Editdate").val());
        formData.append("TotalAmount", $("#Edittotalamount").val());
        formData.append("PaymentType", $("#EditExpensepaymenttype").val());
        formData.append("IsPaid", $("#EditIsPaid").val());
        formData.append("IsApproved", $("#EditIsApproved").val());
        formData.append("Account", $("#Editaccount").val());
        formData.append("UserId", $("#EditExpenseUserNameHidden").val());

        $.ajax({
            url: '/ExpenseMaster/UpdateExpenseDetails',
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
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/ExpenseMaster/AllExpense';
                    });
                }
                else {
                    toastr.error(Result.message);
                }
            }
        })
    }
    else {
        toastr.warning("Kindly fill all datafield");
    }
}

var EditExpensesForm;
$(document).ready(function () {
    EditExpensesForm = $("#EditExpenseForm").validate({
        rules: {
            EditDescription: "required",
            Edittotalamount: "required",
            EditExpenseUserName: "required",
        },
        messages: {
            EditDescription: "Please enter description",
            Edittotalamount: "Please enter correct total amount",
            EditExpenseUserName: "Please enter UserName",
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
            txtExpenseUsername: "required",
        },
        messages: {
            txtexpensetype: "Please Select Expense Type",
            txtDescription: "Please Enter Description",
            txtdate: "Please Select the Date",
            txttotalamount: "Please Enter Correct Total Amount",
            txtExpenseUsername: "Please Enter UserName",
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
                    if (Result.code) {
                        Swal.fire({
                            title: Result.message,
                            icon: 'success',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'OK'
                        }).then(function () {
                            window.location = '/ExpenseMaster/AllExpense';
                        })
                    }
                    else {
                        toastr.error(Result.message);
                    }
                },
                error: function () {
                    Swal.fire({
                        title: "Can't delete expense!",
                        icon: 'warning',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/ExpenseMaster/AllExpense';
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

$(document).ready(function () {
    $(document).on('change', '#textselectedmonth', function () {
        var selectedMonth = $(this).val();
        var userId = $("#txtuserid").val();
        var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        var selectedYear = selectedMonth.split('-')[0];
        var month = parseInt(selectedMonth.split('-')[1], 10);
        var selectedText = monthNames[month - 1];
        GetPayUserExpenseCreditList(userId, selectedMonth);
        GetPayUserExpenseDebitList(userId, selectedMonth, selectedText, selectedYear);
    });
});

function getNextMonth() {
    const date = new Date();
    let month = date.getMonth();
    let year = date.getFullYear();

    if (month > 11) {
        month = 0;
        year += 1;
    }
    const formattedMonth = month < 9 ? '0' + (month + 1) : (month + 1);
    return `${year}-${formattedMonth}`;
}

function GetPayUserExpenseCreditList(userId, selectedMonth) {
    var filterType;
    var selectMonthlyExpense;
    var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    var today = new Date();
    var monthName = months[today.getMonth()];
    var monthValue = today.getMonth() + 1;

    if (selectedMonth != null) {
        selectMonthlyExpense = selectedMonth;
        filterType = "credit";
    } else {
        filterType = "thismonth and credit";
        selectMonthlyExpense = '';
        document.getElementById('textselectedmonth').value = getNextMonth();
    }

    $('#UserPayExpenseTableCredit').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        destroy: true,
        order: [[3, 'asc']],
        pageLength: 30,
        info: false,
        lengthChange: false,
        searching: false,
        pagingType: "simple",
        ajax: {
            type: "POST",
            url: '/ExpenseMaster/GetUserExpenseList?UserId=' + userId + '&filterType=' + filterType + '&selectMonthlyExpense=' + selectMonthlyExpense,
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
            width: "20%"
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
                '<span style="color: black;">Total: ' + '₹' + total.toFixed(2) + '</span>'
            );
        }
    });
}

function GetPayUserExpenseDebitList(userId, selectedMonth, selectedText, selectedYear) {
    var filterType;
    var selectMonthlyExpense;
    var SelectedMonthName;
    var PreviousMonthName;
    var Year;
    var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    var today = new Date();
    var monthName = months[today.getMonth()];
    var currentYear = today.getFullYear();

    function getPreviousMonth(currentMonthName) {
        var currentIndex = months.indexOf(currentMonthName);
        var previousIndex = (currentIndex - 1 + months.length) % months.length;
        return months[previousIndex];
    }

    if (selectedMonth != null) {
        selectMonthlyExpense = selectedMonth;
        SelectedMonthName = selectedText;
        Year = selectedYear;
        PreviousMonthName = getPreviousMonth(SelectedMonthName);
        filterType = "debit";
    } else {
        filterType = "thismonth and debit";
        selectMonthlyExpense = '';
        SelectedMonthName = monthName;
        Year = currentYear;
        PreviousMonthName = getPreviousMonth(SelectedMonthName);
    }

    $('#UserPayExpenseTableDebit').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        destroy: true,
        order: [[3, 'asc']],
        pageLength: 30,
        info: false,
        lengthChange: false,
        searching: false,
        pagingType: "simple",
        ajax: {
            type: "POST",
            url: '/ExpenseMaster/GetUserExpenseList?UserId=' + userId + '&filterType=' + filterType + '&selectMonthlyExpense=' + selectMonthlyExpense,
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
            }
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
            width: "20%"
        }],
        footerCallback: function (row, data, start, end, display) {
            var api = this.api();

            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\₹,]/g, '') * 1 :
                    typeof i === 'number' ?
                        i : 0;
            };

            var total = api
                .column(4)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            var $cumulativePendingFooterRow = $('#monthlyPendingExpenseFooter');
            var $cumulativePendingFooterCell = $cumulativePendingFooterRow.find('th').last();

            var monthlyDataArray = api.ajax.json().userPendingExpenseAmount;

            if (Array.isArray(monthlyDataArray)) {
                var isPreviousMonthFound = false;
                monthlyDataArray.forEach(function (monthlyData) {
                    if (monthlyData.monthName == PreviousMonthName && monthlyData.yearNumber == Year) {

                        isPreviousMonthFound = true;
                        $('#monthlyPendingExpenseFooter').show();

                        $cumulativePendingFooterCell.html(
                            '<span style="color: black;">Last Month Pending:  ₹' + monthlyData.cumulativePending.toFixed(2) + '</span>'
                        );
                    }
                });

                if (!isPreviousMonthFound) {
                    $('#monthlyPendingExpenseFooter').hide();
                }
            }

            var $totalFooterRow = $('#totaldebitExpenseFooter');
            var $totalFooterCell = $totalFooterRow.find('th').last();

            $totalFooterCell.html(
                '<span style="color: black;">Total: ₹' + total.toFixed(2) + '</span>'
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

    $('.nav-btn').click(function () {
        var targetTab = $(this).attr('href');
        if (targetTab === '#GetUserBetweenDatesExpenseList') {
            GetUserBetweenDateExpenseList();
        }
    });
});

function GetUserBetweenDateExpenseList() {
    $.ajax({
        url: '/ExpenseMaster/DisplayExpenseList',
        type: 'GET',
        success: function (result) {
            $("#AllUserExpensePartial").html(result);
            SearchUserBetweenDateExpenseList();
        },
        error: function () {
            alert('Error loading expenses. Please try again.');
        }
    });
}

function SearchUserBetweenDateExpenseList() {
    var startDate = $('#approveExpenseStartDate').val();
    var endDate = $('#approveExpenseEndDate').val();

    if (startDate == "" && endDate == "") {
        toastr.warning("Select dates");
    } else if (startDate == "") {
        toastr.warning("Select Start date");
    } else if (endDate == "") {
        toastr.warning("Select End date");
    } else {
        DisplayUserBetweenDateExpenseList(startDate, endDate)
    }
}

function DisplayUserBetweenDateExpenseList(startDate, endDate) {
    var userId = GetParameterByName('userId');
    var filterType = 'daterange';
    $('#UserallExpenseTable').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        destroy: true,
        order: [[7, 'desc']],
        ajax: {
            url: '/ExpenseMaster/GetUserExpenseList?startDate=' + startDate + '&endDate=' + endDate + '&UserId=' + userId + '&filterType=' + filterType,
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
                "orderable": false,
                width: "05%"
            },
            { "data": "id", "name": "Id", "visible": false },
            { "data": "expenseTypeName", "name": "ExpenseTypeName" },
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
                "orderable": false,
                width: "05%"
            },
            { "data": "id", "name": "Id", "visible": false },
            { "data": "expenseTypeName", "name": "ExpenseTypeName" },
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
                "orderable": false,
                width: "05%"
            },
            { "data": "id", "name": "Id", "visible": false },
            { "data": "expenseTypeName", "name": "ExpenseTypeName" },
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
                "orderable": false,
                width: "05%"
            },
            { "data": "id", "name": "Id", "visible": false },
            { "data": "expenseTypeName", "name": "ExpenseTypeName" },
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

$(document).ready(function () {
    GetAllUserExpenseDetails();

    $('.nav-link').click(function () {
        var targetTab = $(this).attr('href');
        if (targetTab === '#AllExpenseDetails') {
            GetAllUserExpenseDetails();
        } else if (targetTab === '#AllUnapprovedExpenseDetails') {
            GetAllUserUnapproveExpenseDetails();
        } else if (targetTab === '#AllTodayExpenseDetails') {
            GetAllUserTodayExpenseDetails();
        }
    });
});

function GetAllUserExpenseDetails() {
    $.ajax({
        url: '/ExpenseMaster/DisplayAllUserExpenseDetails',
        type: 'GET',
        success: function (result) {
            $("#AllExpenseDetailsPartial").html(result);
            DisplayAllExpenseList('#AllExpenseDetailsPartial table');
        },
        error: function () {
            alert('Error loading expenses. Please try again.');
        }
    });
}

function GetAllUserUnapproveExpenseDetails() {
    $.ajax({
        url: '/ExpenseMaster/DisplayAllUserExpenseDetails',
        type: 'GET',
        success: function (result) {
            $("#AllUnapprovedExpenseDetailsPartial").html(result);
            DisplayAllUnApproveExpenseDetails('#AllUnapprovedExpenseDetailsPartial table');
        },
        error: function () {
            alert('Error loading expenses. Please try again.');
        }
    });
}

function GetAllUserTodayExpenseDetails() {
    $.ajax({
        url: '/ExpenseMaster/DisplayAllUserExpenseDetails',
        type: 'GET',
        success: function (result) {
            $("#AllTodayExpenseDetailsPartial").html(result);
            DisplayAllTodayExpenseDetails('#AllTodayExpenseDetailsPartial table');
        },
        error: function () {
            alert('Error loading expenses. Please try again.');
        }
    });
}

function DisplayAllExpenseList(tableId) {
    $(tableId).DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        destroy: true,
        order: [[3, 'asc']],
        pageLength: 10,
        ajax: {
            type: "POST",
            url: '/ExpenseMaster/GetExpenseDetailsList',
            dataType: 'json'
        },
        columns: [
            { data: null, visible: false, orderable: false },
            { data: "userName", name: "UserName", className: "text-center" },
            { data: "description", name: "Description", className: "text-center" },
            {
                data: "billNumber", name: "BillNumber",
                render: function (data, type, full) {
                    return full.image
                        ? `<div class="d-flex">
                            <div class="flex-grow-1 tasks_name">${full.billNumber}</div>
                            <div class="flex-shrink-0 ms-4 task-icons">
                                <ul class="list-inline tasks-list-menu mb-0">
                                    <a onclick="downloadBill('${full.image}')">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
                                            <path fill="currentColor" d="M13 12h3l-4 4l-4-4h3V8h2v4Zm2-8H5v16h14V8h-4V4ZM3 2.992C3 2.444 3.447 2 3.999 2H16l5 5v13.993A1 1 0 0 1 20.007 22H3.993A1 1 0 0 1 3 21.008V2.992Z" />
                                        </svg>
                                    </a>
                                </ul>
                            </div>
                        </div>`
                        : `<div class="d-flex">
                            <div class="flex-grow-1 tasks_name">${full.billNumber}</div>
                            <div class="flex-shrink-0 ms-4 task-icons"></div>
                        </div>`;
                }
            },
            {
                data: "date", name: "Date", className: "text-center",
                render: function (data) {
                    return getCommonDateformat(data);
                }
            },
            {
                data: "totalAmount", name: "TotalAmount", className: "text-center",
                render: function (data, type, full) {
                    var color = full.account && full.account.toLowerCase() === "credit" ? "green" : "red";
                    return `<span style="color: ${color};">₹${data}</span>`;
                }
            },
            { data: "account", name: "Account", className: "text-center" },
            {
                data: null, orderable: false, searchable: false,
                render: function (data, type, full) {
                    return `<a class="btn text-primary" onclick="EditAllUserExpenseDetails('${full.id}')"><i class="fa-regular fa-pen-to-square"></i></a><a class="btn text-danger" onclick="deleteExpense('${full.id}')"><i class="fas fa-trash"></i></a>`;
                }
            }
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
    });
}

function DisplayAllUnApproveExpenseDetails(tableId) {
    $(tableId).DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        destroy: true,
        order: [[3, 'asc']],
        ajax: {
            type: "POST",
            url: '/ExpenseMaster/GetExpenseDetailsList?unapprove=false',
            dataType: 'json'
        },
        columns: [
            {
                data: null,
                render: function (data, type, full, meta) {
                    return '<div class="form-check"><input class="form-check-input" data-id="' + full.id + '" type="checkbox" name="check_Box"></div>';
                },
                orderable: false
            },
            { data: "userName", name: "UserName", className: "text-center" },
            { data: "description", name: "Description", className: "text-center" },
            {
                data: "billNumber", name: "BillNumber",
                render: function (data, type, full) {
                    return full.image
                        ? `<div class="d-flex">
                            <div class="flex-grow-1 tasks_name">${full.billNumber}</div>
                            <div class="flex-shrink-0 ms-4 task-icons">
                                <ul class="list-inline tasks-list-menu mb-0">
                                    <a onclick="downloadBill('${full.image}')">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
                                            <path fill="currentColor" d="M13 12h3l-4 4l-4-4h3V8h2v4Zm2-8H5v16h14V8h-4V4ZM3 2.992C3 2.444 3.447 2 3.999 2H16l5 5v13.993A1 1 0 0 1 20.007 22H3.993A1 1 0 0 1 3 21.008V2.992Z" />
                                        </svg>
                                    </a>
                                </ul>
                            </div>
                        </div>`
                        : `<div class="d-flex">
                            <div class="flex-grow-1 tasks_name">${full.billNumber}</div>
                            <div class="flex-shrink-0 ms-4 task-icons"></div>
                        </div>`;
                }
            },
            {
                data: "date", name: "Date", className: "text-center",
                render: function (data) {
                    return getCommonDateformat(data);
                }
            },
            {
                data: "totalAmount", name: "TotalAmount", className: "text-center",
                render: function (data, type, full) {
                    var color = full.account && full.account.toLowerCase() === "credit" ? "green" : "red";
                    return `<span style="color: ${color};">₹${data}</span>`;
                }
            },
            { data: "account", name: "Account", className: "text-center" },
            {
                data: null, orderable: false, searchable: false,
                render: function (data, type, full) {
                    return `<a class="btn text-primary" onclick="EditAllUserExpenseDetails('${full.id}')"><i class="fa-regular fa-pen-to-square"></i></a><a class="btn text-danger" onclick="deleteExpense('${full.id}')"><i class="fas fa-trash"></i></a>`;
                }
            }
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
        drawCallback: function (settings) {

        }
    });
}

function DisplayAllTodayExpenseDetails(tableId) {
    var todayDate = new Date().toISOString().split('T')[0];
    $(tableId).DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        destroy: true,


        ajax: {
            type: "POST",
            url: '/ExpenseMaster/GetExpenseDetailsList?TodayDate=' + todayDate,
            dataType: 'json'
        },
        columns: [
            { data: null, visible: false, orderable: false },
            { data: "userName", name: "UserName", className: "text-center" },
            { data: "description", name: "Description", className: "text-center" },
            {
                data: "billNumber", name: "BillNumber",
                render: function (data, type, full) {
                    return full.image
                        ? `<div class="d-flex">
                            <div class="flex-grow-1 tasks_name">${full.billNumber}</div>
                            <div class="flex-shrink-0 ms-4 task-icons">
                                <ul class="list-inline tasks-list-menu mb-0">
                                    <a onclick="downloadBill('${full.image}')">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
                                            <path fill="currentColor" d="M13 12h3l-4 4l-4-4h3V8h2v4Zm2-8H5v16h14V8h-4V4ZM3 2.992C3 2.444 3.447 2 3.999 2H16l5 5v13.993A1 1 0 0 1 20.007 22H3.993A1 1 0 0 1 3 21.008V2.992Z" />
                                        </svg>
                                    </a>
                                </ul>
                            </div>
                        </div>`
                        : `<div class="d-flex">
                            <div class="flex-grow-1 tasks_name">${full.billNumber}</div>
                            <div class="flex-shrink-0 ms-4 task-icons"></div>
                        </div>`;
                }
            },
            {
                data: "date", name: "Date", className: "text-center",
                render: function (data) {
                    return getCommonDateformat(data);
                }
            },
            {
                data: "totalAmount", name: "TotalAmount", className: "text-center",
                render: function (data, type, full) {
                    var color = full.account && full.account.toLowerCase() === "credit" ? "green" : "red";
                    return `<span style="color: ${color};">₹${data}</span>`;
                }
            },
            { data: "account", name: "Account", className: "text-center" },
            { data: null, visible: false, orderable: false },
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
                .column(5)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            $(api.column(5).footer()).html(
                '<span style="color: black;">Total: ' + '₹' + total.toFixed(2) + '</span>'
            );
        }
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
            var total = 0, creditamount = 0, debitamount = 0;

            result.forEach(function (obj) {
                if (obj.totalAmount) {
                    total += obj.totalAmount;
                }
                if (obj.account === "Credit") {
                    creditamount += obj.totalAmount || 0;
                }
                if (obj.account === "Debit") {
                    debitamount += obj.totalAmount || 0;
                }
            });

            $("#txttotalcreditamount").text('₹' + creditamount.toFixed(2));
            $("#txtTotalAmount").text('₹' + debitamount.toFixed(2));

            var pendingAmount = debitamount - creditamount;
            $("#pendingamount").text('₹' + pendingAmount.toFixed(2));
            $("#txttotaldebitedamount").text('₹' + pendingAmount.toFixed(2));

            $('#txtcreditamount').on('input', function () {
                var enteredAmount = parseFloat($(this).val());
                if (!isNaN(enteredAmount)) {
                    var newPendingAmount = pendingAmount - enteredAmount;
                    if (enteredAmount > pendingAmount) {
                        $('#warningMessage').text('Entered amount cannot exceed pending amount.');
                    } else {
                        $('#warningMessage').text('');
                        $('#txtpendingamount').val(newPendingAmount.toFixed(2));
                    }
                } else {
                    $('#warningMessage').text('');
                    $('#txtpendingamount').val('');
                }
            });
        },
        error: function (error) {
            console.log("Error:", error);
        }
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
function AllUserApproveExpense() {
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
            $("input[name=check_Box]:checked").each(function () {
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
                                window.location = '/ExpenseMaster/AllExpense';
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
        formData.append("Date", $("#txtpaydate").val());
        formData.append("UserId", $("#txtuserid").val());
        formData.append("ApprovedBy", $("#txtuseraproveid").val());
        formData.append("ApprovedByName", $("#txtuseraprovename").val());
        formData.append("TotalAmount", $("#txtcreditamount").val());
        formData.append("PaymentType", $("#txtExpensepaymenttype").val());
        formData.append("CreatedBy", $("#txtuseraproveid").val());
        formData.append("PaymentDetails", $("#txtpaymentDetails").val());
        $.ajax({
            url: '/ExpenseMaster/PayExpense',
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
                        window.location = '/ExpenseMaster/PayExpense';
                    });
                }
            }
        })
    }
    else {
        toastr.warning("Kindly fill all datafield");
    }
}

$(document).ready(function () {
    $("#GetPayForm").validate({
        rules: {
            txtcreditamount: "required",
            txtExpensepaymenttype: "required",
        },
        messages: {
            txtcreditamount: "Please Enter Amount",
            txtExpensepaymenttype: "Please Select Payment Type",
        }
    })
});

$(document).ready(function () {
    var UserId = $("#txtuserid").val();
    if (UserId) {
        GetPayUserExpenseCreditList(UserId, null)
        GetPayUserExpenseDebitList(UserId, null)
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
                "orderable": false,
                width: "05%"
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
        $('#GetMyUnapproveExpense').DataTable({
            processing: false,
            serverSide: true,
            filter: true,
            "bDestroy": true,
            order: [[3, 'asc']],
            pageLength: 10,
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
                    '<span style="color: black;">Total: ' + '₹' + total.toFixed(2) + '</span>'
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
        destroy: true,
        order: [[3, 'asc']],
        pageLength: 10,
        ajax: {
            type: "POST",
            url: '/ExpenseMaster/GetUserExpenseList?UserId=' + UserId,
            dataType: 'json'
        },
        columns: [
            {
                data: null,

                render: function (data, type, full, meta) {
                    var account = full.account.toLowerCase();
                    if (account === "credit") {
                        return '<div class="avatar-xs"><div class="avatar-title bg-success-subtle text-success rounded-circle fs-16"><i class="ri-arrow-left-down-fill"></i></div></div>';
                    } else if (account === "debit") {
                        return '<div class="avatar-xs"><div class="avatar-title bg-danger-subtle text-danger rounded-circle fs-16"><i class="ri-arrow-right-up-fill"></i></div></div>';
                    } else {
                        return '';
                    }
                },
                orderable: false,
                width: "05%"
            },
            { data: "description", name: "Description", width: "20%" },
            { data: "billNumber", name: "BillNumber", width: "20%" },
            {
                data: "date",
                name: "Date",
                render: function (data, type, full, meta) {
                    return getCommonDateformat(data);
                },
                width: "20%"
            },
            {
                data: "totalAmount",
                name: "TotalAmount",
                render: function (data, type, full, meta) {
                    var color = full.account.toLowerCase() === "credit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + '₹' + data + '</span>';
                },
                width: "20%"
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
        footerCallback: function (row, data, start, end, display) {
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
                '<span style="color: black;">Total: ' + '₹' + total.toFixed(2) + '</span>'
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
        pageLength: 10,
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

                "orderable": false,
                width: "05%"
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
                '<span style="color: black;">Total: ' + '₹' + total.toFixed(2) + '</span>'
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
        pageLength: 10,
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
                "orderable": false,
                width: "05%"
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
                '<span style="color: black;">Total: ' + '₹' + total.toFixed(2) + '</span>'
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
        pageLength: 10,
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
                "orderable": false,
                width: "05%"
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
                '<span style="color: black;">Total: ' + '₹' + total.toFixed(2) + '</span>'
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
        pageLength: 10,
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
                "orderable": false,
                width: "05%"
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
                '<span style="color: black;">Total: ' + '₹' + total.toFixed(2) + '</span>'
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
        toastr.warning("Select dates");
    } else if (StartDate == "") {
        toastr.warning("Select Start date");
    } else if (EndDate == "") {
        toastr.warning("Select End date");
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
        pageLength: 10,
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
                "orderable": false,
                width: "05%"
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
                '<span style="color: black;">Total: ' + '₹' + total.toFixed(2) + '</span>'
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
                    toastr.success(Result.message);
                    $('#ExpenseTypeModal').modal('hide');
                    GetExpenseTypeList();
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
        toastr.warning("Kindly fill expense type");
    }
}
function downloadBill(billName) {
    $.ajax({
        url: '/ExpenseMaster/DownloadBill?BillName=' + billName,
        type: "GET",
        dataType: 'json',
        success: function (result) {
            siteloaderhide();

            if (result && result.memory) {
                try {
                    var binaryString = window.atob(result.memory);
                    var length = binaryString.length;
                    var bytes = new Uint8Array(length);

                    for (var i = 0; i < length; i++) {
                        bytes[i] = binaryString.charCodeAt(i);
                    }

                    var blob = new Blob([bytes], { type: result.contentType });

                    var link = document.createElement('a');
                    link.href = window.URL.createObjectURL(blob);
                    link.setAttribute('download', result.fileName);

                    document.body.appendChild(link);
                    link.click();

                    document.body.removeChild(link);
                } catch (e) {
                    toastr.error("Error decoding file: " + e.message);
                }
            } else {
                toastr.warning(result.Message || "No document found for selected");
            }
        },
        error: function () {
            siteloaderhide();
            toastr.error("Can't get data");
        }
    });
}