var Formdata = window.userFormPermissions || 0;

function DisplayAddExpenseModel() {
    clearText();
    $('#AddExpenseModel').modal('show');
}

$(document).ready(function () {
    GetExpenseTotalAmount();
    ApprovedExpenseList();
   // GetExpenseTypeList();
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

//function GetExpenseTypeList() {
//    $.ajax({
//        url: '/ExpenseMaster/GetExpenseTypeList',
//        method: 'GET',
//        success: function (result) {
//            var expenseTypes = result.map(function (data) {
//                return {
//                    label: data.type,
//                    value: data.id
//                };
//            });

//            // Sort alphabetically by label
//            expenseTypes.sort(function (a, b) {
//                return a.label.localeCompare(b.label);
//            });

//            function setupAutocomplete(inputId, hiddenId) {
//                $(inputId).autocomplete({
//                    source: expenseTypes,
//                    minLength: 0,
//                    focus: function (event, ui) {
//                        if (ui?.item) {
//                            $(inputId).val(ui.item.label);
//                        }
//                        event.preventDefault(); // Prevent value insertion on focus
//                    },
//                    select: function (event, ui) {
//                        if (ui?.item) {
//                            $(inputId).val(ui.item.label);
//                            $(hiddenId).val(ui.item.value);
//                        }
//                        event.preventDefault(); // Prevent default behavior
//                        return false;
//                    }
//                }).focus(function () {
//                    $(this).autocomplete("search", "");
//                });
//            }

//            // Apply to both input sets
//            setupAutocomplete("#txtexpensetype", "#txtexpensetypeHidden");
//            setupAutocomplete("#Editexpensetype", "#EditexpensetypeHidden");
//        },
//        error: function (xhr, status, error) {
//            console.error("Failed to fetch expense types:", error);
//            toastr.error("Failed to fetch expense types.");
//        }
//    });
//}


$(document).ready(function () {
    $('#txtexpensetype').select2({
        placeholder: 'Select Expense Type',
        width: '100%',
        dropdownParent: $('#AddExpenseModel'), 
        ajax: {
            url: '/ExpenseMaster/GetExpenseTypeList',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                return {
                    results: data.map(item => ({
                        id: item.id,
                        text: item.type
                    }))
                };
            }
        }
    });

    $(' #Editexpensetype').select2({
        placeholder: 'Select Expense Type',
        width: '100%',
        dropdownParent: $('#EditExpenseModel'),
        ajax: {
            url: '/ExpenseMaster/GetExpenseTypeList',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                return {
                    results: data.map(item => ({
                        id: item.id,
                        text: item.type
                    }))
                };
            }
        }
    });

    $('#txtExpensepaymenttype').select2({
        placeholder: 'Select Payment type',
        width: '100%',
        dropdownParent: $('#EditExpenseModel'),
        ajax: {
            url: '/ExpenseMaster/GetPaymentTypeList',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {debugger
                return {
                    results: data.map(item => ({
                        id: item.id,
                        text: item.type
                    }))
                };
            },
            error: function (xhr, status, error) {
                console.error("Error fetching vendor list:", error);
            }
        }
    });
    $('#txtExpenseUsername').select2({
        placeholder: 'Select Employee',
        width: '100%',
        dropdownParent: $('#AddExpenseModel'),
        ajax: {
            url: '/Task/GetUserName',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                debugger
                return {
                    results: data.map(item => ({
                        id: item.id,
                        text: item.firstName + ' ' + item.lastName + ' (' + item.userName + ')',
                    }))
                };
            },
            error: function (xhr, status, error) {
                console.error("Error fetching vendor list:", error);
            }
        }
    });
    $('#EditExpenseUserName').select2({
        placeholder: 'Select Employee',
        width: '100%',
        dropdownParent: $('#EditExpenseModel'),
        ajax: {
            url: '/Task/GetUserName',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                debugger
                return {
                    results: data.map(item => ({
                        id: item.id,
                        text: item.firstName + ' ' + item.lastName + ' (' + item.userName + ')',
                    }))
                };
            },
            error: function (xhr, status, error) {
                console.error("Error fetching vendor list:", error);
            }
        }
    });
});

//$(document).ready(function () {

//    function GetUsersList() {
//        $.ajax({
//            url: '/Task/GetUserName',
//            method: 'GET',
//            success: function (result) {
//                var UserList = result.map(function (data) {
//                    return {
//                        label: data.firstName + ' ' + data.lastName + ' (' + data.userName + ')',
//                        value: data.id
//                    };
//                });

//                function setupAutocomplete(inputId, hiddenId) {
//                    $(inputId).autocomplete({
//                        source: UserList,
//                        minLength: 0,
//                        focus: function (event, ui) {
//                            event.preventDefault();
//                            $(inputId).val(ui.item.label);
//                        },
//                        select: function (event, ui) {
//                            $(inputId).val(ui.item.label);
//                            $(hiddenId).val(ui.item.value);
//                            event.preventDefault();
//                            return false;
//                        }
//                    }).focus(function () {
//                        $(this).autocomplete("search");
//                    });
//                }

//                setupAutocomplete("#txtExpenseUsername", "#txtExpenseUsernameHidden");
//                setupAutocomplete("#EditExpenseUserName", "#EditExpenseUserNameHidden");
//            },
//            error: function (err) {
//                toastr.error("Failed to fetch User List: ", err);
//            }
//        });
//    }

//    GetUsersList();
//});

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
        formData.append("ExpenseType", $("#txtexpensetype").val());
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
        formData.append("ExpenseType", $("#txtexpensetype").val());
        formData.append("Description", $("#txtDescription").val());
        formData.append("BillNumber", $("#txtbillno").val());
        formData.append("Date", $("#txtdate").val());
        formData.append("Account", $("#txtaccount").val());
        formData.append("TotalAmount", $("#txttotalamount").val());
        formData.append("Image", $("#txtimage")[0].files[0]);
        formData.append("UserId", $("#txtExpenseUsername").val());
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
            $('#Editid').val(response.id);
            $('#EditDescription').val(response.description);
            $('#Editbillno').val(response.billNumber);
            $('#Editdate').val(response.date);
            $('#Edittotalamount').val(response.totalAmount);
            $('#Editaccount').val(response.account);
            $('#EditIsPaid').val(response.isPaid ? "True" : "False");
            $('#EditIsApproved').val(response.isApproved ? "True" : "False");

            setSelectedExpenseType(response.expenseType, response.expenseTypeName);
            setSelectedPaymentType(response.paymentType, response.paymentTypeName);
        },
        error: function () {
            toastr.error("Data not found");
        }
    });
}
function setSelectedExpenseType(expenseTypeId, expenseTypeName) {
    if (!expenseTypeName || expenseTypeName === "null") {
        return;
    }

    const $dropdown = $('#Editexpensetype');

    $dropdown.empty();

    const newOption = new Option(expenseTypeName, expenseTypeId, true, true);
    $dropdown.append(newOption).trigger('change');
}
function setSelectedPaymentType(PaymentTypeId, PaymentTypeName) {
    if (!PaymentTypeName || PaymentTypeName === "null") {
        return;
    }

    const $dropdown = $('#txtExpensepaymenttype');

    $dropdown.empty();

    const newOption = new Option(PaymentTypeName, PaymentTypeId, true, true);
    $dropdown.append(newOption).trigger('change');
}
function setSelectedUser(UserId, UserName) {
    if (!UserName || UserName === "null") {
        return;
    }

    const $dropdown = $('#EditExpenseUserName');

    $dropdown.empty();

    const newOption = new Option(UserName, UserId, true, true);
    $dropdown.append(newOption).trigger('change');
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
        formData.append("PaymentType", $("#txtExpensepaymenttype").val());
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
            $('#Editid').val(response.id);
            $('#EditDescription').val(response.description);
            $('#Editbillno').val(response.billNumber);
            $('#Editdate').val(response.date);
            $('#Edittotalamount').val(response.totalAmount);
            $('#Editaccount').val(response.account);
            $('#EditExpensepaymenttype').val(response.paymentType);
            $('#EditIsPaid').val(response.isPaid ? "True" : "False");
            $('#EditIsApproved').val(response.isApproved ? "True" : "False");

            setSelectedExpenseType(response.expenseType, response.expenseTypeName);
            setSelectedUser(response.userId, response.fullName);
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
        formData.append("ExpenseType", $("#Editexpensetype").val());
        formData.append("Description", $("#EditDescription").val());
        formData.append("BillNumber", $("#Editbillno").val());
        formData.append("Date", $("#Editdate").val());
        formData.append("TotalAmount", $("#Edittotalamount").val());
        formData.append("PaymentType", $("#EditExpensepaymenttype").val());
        formData.append("IsPaid", $("#EditIsPaid").val());
        formData.append("IsApproved", $("#EditIsApproved").val());
        formData.append("Account", $("#Editaccount").val());
        formData.append("UserId", $("#EditExpenseUserName").val());

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
const now = new Date();
const currentMonth = now.toISOString().slice(0, 7);
$('#textselectedmonth').val(currentMonth);

let UserPayExpenseCreditOptions = [];
let UserPayExpenseCreditMonthFilter = currentMonth;

$(document).ready(function () {
    UserPayExpenseCreditOptions = {
        rowHeight: 50,
        columnDefs: [
            {
                headerName: "", field: "account", sortable: false, filter: false, width: 60,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';
                    return '<div class="avatar-xs"><div class="avatar-title bg-danger-subtle text-danger rounded-circle fs-16"><i class="ri-arrow-right-up-fill"></i></div></div>';
                }
            },
            { headerName: "Description", field: "description", sortable: true, filter: true },
            {
                headerName: "Date", field: "date", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';
                    return getCommonDateformat(params.data.date);
                }
            },
            {
                headerName: "Total amount", field: "totalamount", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';

                    function formatNumberWithCommas(number) {
                        return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                    }
                    var formattedAmount = '₹ ' + formatNumberWithCommas(parseFloat(params.data.totalAmount).toFixed(2));
                    var color = params.data.account && params.data.account.toLowerCase() === "credit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + formattedAmount + '</span>';
                }
            },
        ],
        defaultColDef: {
            sortable: true,
            filter: true,
            cellClass: 'ag-cell-default-style',
            width: 175,
        },
        rowSelection: 'single',
        rowClassRules: {
            'selected-row': params => params.node.isSelected()
        },
        onGridReady: function (params) {
            UserPayExpenseCreditOptions.api = params.api;
            UserPayExpenseCreditOptions.columnApi = params.columnApi;
            UserPayExpenseCreditOptions.api.sizeColumnsToFit();
        },
        rowModelType: 'infinite',
        cacheBlockSize: 20,
        pagination: true,
        paginationPageSize: 20,
        suppressPaginationPanel: true,
        datasource: getUserPayExpenseCreditDatasource()
    };

    function getUserPayExpenseCreditDatasource() {
        return {
            getRows: function (params) {
                const request = {
                    StartRow: params.startRow,
                    PageSize: params.endRow - params.startRow,
                    SearchType: "",
                    SearchValue: "",
                    SortModel: params.sortModel || [],
                    SortColumn: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].colId : "",
                    SortDirection: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].sort : "",
                    filters: Object.entries(params.filterModel || {}).map(([key, value]) => ({
                        colId: key,
                        filterValue: value.filter
                    })),
                    UserId: $('#txtuserid').val(),
                    Month: UserPayExpenseCreditMonthFilter,
                    FilterType: "Credit",
                };

                $.ajax({
                    url: '/ExpenseMaster/GetUserExpenseList',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(request),
                    success: function (response) {
                        params.successCallback(response.rowsThisPage, response.totalRowCount);
                    },
                    error: function () {
                        params.failCallback();
                    }
                });
            }
        };
    }

    const myGridElement = document.querySelector('#UserPayExpenseCreditTable');
    agGrid.createGrid(myGridElement, UserPayExpenseCreditOptions);

    $('#textselectedmonth').on('change', function () {
        UserPayExpenseCreditMonthFilter = $(this).val();
        UserPayExpenseCreditOptions.api.purgeInfiniteCache();
    });
});


let UserPayExpenseDebitOptions = [];
let UserPayExpenseDebitMonthFilter = currentMonth;

$(document).ready(function () {
    UserPayExpenseDebitOptions = {
        rowHeight: 50,
        columnDefs: [
            {
                headerName: "", field: "account", sortable: false, filter: false, width: 60,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';
                    return '<div class="avatar-xs"><div class="avatar-title bg-success-subtle text-success rounded-circle fs-16"><i class="ri-arrow-right-down-fill"></i></div></div>';
                }
            },
            { headerName: "Description", field: "description", sortable: true, filter: true },
            {
                headerName: "Date", field: "date", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';
                    return getCommonDateformat(params.data.date);
                }
            },
            {
                headerName: "Total amount", field: "totalamount", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';

                    function formatNumberWithCommas(number) {
                        return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                    }
                    var formattedAmount = '₹ ' + formatNumberWithCommas(parseFloat(params.data.totalAmount).toFixed(2));
                    var color = params.data.account && params.data.account.toLowerCase() === "credit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + formattedAmount + '</span>';
                }
            },
        ],
        defaultColDef: {
            sortable: true,
            filter: true,
            cellClass: 'ag-cell-default-style',
            width: 175,
        },
        rowSelection: 'single',
        rowClassRules: {
            'selected-row': params => params.node.isSelected()
        },
        onGridReady: function (params) {
            UserPayExpenseDebitOptions.api = params.api;
            UserPayExpenseDebitOptions.columnApi = params.columnApi;
            UserPayExpenseDebitOptions.api.sizeColumnsToFit();
        },
        rowModelType: 'infinite',
        cacheBlockSize: 20,
        pagination: true,
        paginationPageSize: 20,
        suppressPaginationPanel: true,
        datasource: getUserPayExpenseDebitDatasource()
    };

    function getUserPayExpenseDebitDatasource() {
        return {
            getRows: function (params) {
                const request = {
                    StartRow: params.startRow,
                    PageSize: params.endRow - params.startRow,
                    SearchType: "",
                    SearchValue: "",
                    SortModel: params.sortModel || [],
                    SortColumn: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].colId : "",
                    SortDirection: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].sort : "",
                    filters: Object.entries(params.filterModel || {}).map(([key, value]) => ({
                        colId: key,
                        filterValue: value.filter
                    })),
                    UserId: $('#txtuserid').val(),
                    Month: UserPayExpenseDebitMonthFilter,
                    FilterType: "Debit",
                };

                $.ajax({
                    url: '/ExpenseMaster/GetUserExpenseList',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(request),
                    success: function (response) {
                        params.successCallback(response.rowsThisPage, response.totalRowCount);
                    },
                    error: function () {
                        params.failCallback();
                    }
                });
            }
        };
    }

    const myGridElement = document.querySelector('#UserPayExpenseDebitTable');
    agGrid.createGrid(myGridElement, UserPayExpenseDebitOptions);

    $('#textselectedmonth').on('change', function () {
        UserPayExpenseDebitMonthFilter = $(this).val();
        UserPayExpenseDebitOptions.api.purgeInfiniteCache();
    });
});

let UserListExpenseGridOptions = [];

$(document).ready(function () {
    function formatNumberWithCommas(number) {
        return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }

    UserListExpenseGridOptions = {
        rowHeight: 50,
        columnDefs: [
            {
                headerName: "Employee Name", field: "firstName", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.userId) return '';

                    var imageSrc;
                    if (params.data.image && params.data.image.trim() !== '') {
                        imageSrc = '<img src="/' + params.data.image + '" style="height: 40px; width: 40px; border-radius: 50%;" ' +
                            'onmouseover="showIcons(event, this.parentElement)" onmouseout="hideIcons(event, this.parentElement)">';
                    } else {
                        var initials = (params.data.firstName ? params.data.firstName[0] : '') + (params.data.lastName ? params.data.lastName[0] : '');
                        imageSrc = '<div class="flex-shrink-0 avatar-xs me-2">' +
                            '<div class="avatar-title bg-success-subtle text-success rounded-circle fs-13" style="height: 40px; width: 40px; border-radius: 50%;">' + initials.toUpperCase() + '</div></div>';
                    }

                    return '<a href="/ExpenseMaster/ApprovedExpense?UserId=' + params.data.userId + '&UserName=' + params.data.firstName + ' ' + params.data.lastName + '" class="link-primary" style="display: flex; align-items: center;" data-userid="' + params.data.userId + '">' + imageSrc + '<span style="margin-left: 10px;color: #16989A !important;">' + params.data.firstName + ' ' + params.data.lastName + '</span></a>';
                }
            },
            { headerName: "Employee Id", field: "userName", sortable: true, filter: true },
            {
                headerName: "Date", field: "date", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.userId) return '';
                    return getCommonDateformat(params.data.date);
                }
            },
            {
                headerName: "Unapprove Pending Amount", field: "unapprovedPendingAmount", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.userId) return '';

                    return '₹' + formatNumberWithCommas(params.data.unapprovedPendingAmount);
                }
            },
            {
                headerName: "Total Pending Amount", field: "totalPendingAmount", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.userId) return '';
                    return '₹' + formatNumberWithCommas(params.data.totalPendingAmount);
                }
            },
            {
                headerName: "Total Amount", field: "totalAmount", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.userId) return '';

                    return '₹' + formatNumberWithCommas(params.data.totalAmount);
                }
            },
        ],
        defaultColDef: {
            sortable: true,
            filter: true,
            cellClass: 'ag-cell-default-style',
            width: 175,
        },
        rowSelection: 'single',
        rowClassRules: {
            'selected-row': params => params.node.isSelected()
        },
        onGridReady: function (params) {
            UserListExpenseGridOptions.api = params.api;
            UserListExpenseGridOptions.columnApi = params.columnApi;
            UserListExpenseGridOptions.api.sizeColumnsToFit();
            createEnhancedPagination(params.api);
        },
        rowModelType: 'infinite',
        cacheBlockSize: 20,
        pagination: true,
        paginationPageSize: 20,
        suppressPaginationPanel: true,
        datasource: getUserListExpenseDatasource()
    };

    function getUserListExpenseDatasource() {
        return {
            getRows: function (params) {
                const request = {
                    StartRow: params.startRow,
                    PageSize: params.endRow - params.startRow,
                    SearchType: "",
                    SearchValue: "",
                    SortModel: params.sortModel || [],
                    SortColumn: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].colId : "",
                    SortDirection: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].sort : "",
                    filters: Object.entries(params.filterModel || {}).map(([key, value]) => ({
                        colId: key,
                        filterValue: value.filter
                    })),
                    searchValue: $('#txtUserListExpenseSearch').val(),
                };

                $.ajax({
                    url: '/ExpenseMaster/GetUserListTable',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(request),
                    success: function (response) {
                        params.successCallback(response.rowsThisPage, response.totalRowCount);
                        const rangeDisplay = document.querySelector('#UserListExpenseTable .range-display');
                        if (rangeDisplay) updateEnhancedPagination(UserListExpenseGridOptions.api, rangeDisplay);
                    },
                    error: function () {
                        params.failCallback();
                    }
                });
            }
        };
    }

    function createEnhancedPagination(gridApi) {
        const paginationContainer = document.createElement('div');
        paginationContainer.className = 'enhanced-pagination-container';

        const pageSizeContainer = document.createElement('div');
        pageSizeContainer.className = 'page-size-container';
        pageSizeContainer.innerHTML = `
            <span>Page Size: </span>
            <select class="page-size-selector">
                <option value="10">10</option>
                <option value="20" selected>20</option>
                <option value="50">50</option>
                <option value="100">100</option>
            </select>
        `;

        const rangeDisplay = document.createElement('div');
        rangeDisplay.className = 'range-display';

        const navContainer = document.createElement('div');
        navContainer.className = 'navigation-container';

        const prevButton = document.createElement('button');
        prevButton.className = 'pagination-button';
        prevButton.innerHTML = '<i class="ri-arrow-left-s-line"></i> Previous';
        prevButton.addEventListener('click', () => {
            gridApi.paginationGoToPreviousPage();
            updateEnhancedPagination(gridApi, rangeDisplay);
        });

        const nextButton = document.createElement('button');
        nextButton.className = 'pagination-button';
        nextButton.innerHTML = 'Next <i class="ri-arrow-right-s-line"></i>';
        nextButton.addEventListener('click', () => {
            gridApi.paginationGoToNextPage();
            updateEnhancedPagination(gridApi, rangeDisplay);
        });

        const pageButtonsContainer = document.createElement('div');
        pageButtonsContainer.className = 'page-buttons';

        navContainer.appendChild(prevButton);
        navContainer.appendChild(pageButtonsContainer);
        navContainer.appendChild(nextButton);

        const pageInfo = document.createElement('div');
        pageInfo.className = 'page-info';

        paginationContainer.appendChild(pageSizeContainer);
        paginationContainer.appendChild(rangeDisplay);
        paginationContainer.appendChild(navContainer);
        paginationContainer.appendChild(pageInfo);

        const eGui = document.querySelector('#UserListExpenseTable');
        const paginationEl = document.createElement('div');
        paginationEl.className = 'ag-paging-panel enhanced';
        paginationEl.appendChild(paginationContainer);
        eGui.appendChild(paginationEl);

        const pageSizeSelector = pageSizeContainer.querySelector('.page-size-selector');
        pageSizeSelector.addEventListener('change', function () {
            const newPageSize = Number(this.value);

            // Destroy and recreate grid with new block size
            const gridDiv = document.querySelector('#UserListExpenseTable');

            UserListExpenseGridOptions = {
                ...UserListExpenseGridOptions,
                cacheBlockSize: newPageSize,
                paginationPageSize: newPageSize,
                datasource: getUserListExpenseDatasource(),
            };

            // Clear old grid and re-init
            gridDiv.innerHTML = '';
            agGrid.createGrid(gridDiv, UserListExpenseGridOptions);
        });

        updateEnhancedPagination(gridApi, rangeDisplay);
    }

    function updateEnhancedPagination(gridApi, rangeDisplay) {
        const currentPage = gridApi.paginationGetCurrentPage() + 1;
        const totalPages = gridApi.paginationGetTotalPages();
        const totalRows = gridApi.paginationGetRowCount();
        const pageSize = UserListExpenseGridOptions.paginationPageSize;

        const startRow = totalRows > 0 ? ((currentPage - 1) * pageSize + 1) : 0;
        const endRow = totalRows > 0 ? Math.min(currentPage * pageSize, totalRows) : 0;

        rangeDisplay.textContent = totalRows > 0 ? `${startRow} to ${endRow} of ${totalRows}` : '0 to 0 of 0';

        const pageInfo = document.querySelector('.page-info');
        if (pageInfo) pageInfo.textContent = `Page ${currentPage} of ${totalPages || 1}`;

        const pageButtonsContainer = document.querySelector('.page-buttons');
        if (!pageButtonsContainer) return;

        pageButtonsContainer.innerHTML = '';

        const startPage = Math.max(1, currentPage - 1);
        const endPage = Math.min(totalPages, currentPage + 1);

        for (let i = startPage; i <= endPage; i++) {
            const pageButton = document.createElement('button');
            pageButton.className = `pagination-button ${i === currentPage ? 'active' : ''}`;
            pageButton.textContent = i;
            pageButton.addEventListener('click', () => {
                gridApi.paginationGoToPage(i - 1);
                updateEnhancedPagination(gridApi, rangeDisplay);
            });
            pageButtonsContainer.appendChild(pageButton);
        }

        const prevButton = document.querySelector('.navigation-container .pagination-button:first-child');
        const nextButton = document.querySelector('.navigation-container .pagination-button:last-child');
        if (prevButton) prevButton.disabled = currentPage === 1;
        if (nextButton) nextButton.disabled = currentPage === totalPages || totalPages === 0;

        const pageSizeSelector = document.querySelector('.page-size-selector');
        if (pageSizeSelector) pageSizeSelector.value = pageSize;
    }

    const myGridElement = document.querySelector('#UserListExpenseTable');
    agGrid.createGrid(myGridElement, UserListExpenseGridOptions);

    $('#btnUserListExpensesearch').on('click', function () {
        UserListExpenseGridOptions.api.onFilterChanged();
    });
    $('#txtUserListExpenseSearch').on('keypress', function (e) {
        if (e.which === 13) {
            $('#btnUserListExpensesearch').click();
        }
    });
    $('#txtUserListExpenseSearch').on('input', function () {
        var searchText = $(this).val().trim();

        if (searchText === '') {
            if (UserListExpenseGridOptions.api) {
                UserListExpenseGridOptions.api.refreshInfiniteCache();
            }
        }
    });

});

let UserExpenseApproveGridOptions = [];
let UserExpenseApprovestartDate = null;
let UserExpenseApproveendDate = null;
let UserExpenseApproveselectedTab = $(".Returns.active").attr("id");
let UserExpenseApproveselectedMonthFilter = "";

$(document).ready(function () {
    function getCheckboxColumnDef() {
        return {
            headerName: "",
            field: "select",
            width: 50,
            suppressMenu: true,
            suppressSorting: false,
            cellRenderer: function (params) {
                if (!params.data || !params.data.id) return '';

                const checkboxId = `ApproveCheckbox_${params.data.id}`;
                return `
                <div class="custom-control custom-checkbox">
                    <input class="custom-control-input custom-control-input-teal" 
                           data-id="${params.data.id}" 
                           type="checkbox" 
                           name="chk_child" 
                           id="${checkboxId}">
                    <label class="custom-control-label" for="${checkboxId}"></label>
                </div>
            `;
            }
        };
    }

    function getColumnDefs() {
        const baseColumns = [
            {
                headerName: "", field: "account", sortable: false, filter: false, width: 60,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';
                    if (params.data.account === "Credit") {
                        return '<div class="avatar-xs"><div class="avatar-title bg-success-subtle text-success rounded-circle fs-16"><i class="ri-arrow-left-down-fill"></i></div></div>';
                    } else if (params.data.account === "Debit") {
                        return '<div class="avatar-xs"><div class="avatar-title bg-danger-subtle text-danger rounded-circle fs-16"><i class="ri-arrow-right-up-fill"></i></div></div>';
                    } else {
                        return '';
                    }
                }
            },
            { headerName: "Expense Type", field: "expenseTypeName", sortable: true, filter: true },
            { headerName: "Bill No.", field: "billNumber", sortable: true, filter: true },
            { headerName: "Description", field: "description", sortable: true, filter: true },
            {
                headerName: "Date", field: "date", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';
                    return getCommonDateformat(params.data.date);
                }
            },
            {
                headerName: "Total amount", field: "totalamount", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';

                    function formatNumberWithCommas(number) {
                        return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                    }
                    var formattedAmount = '₹ ' + formatNumberWithCommas(parseFloat(params.data.totalAmount).toFixed(2));
                    var color = params.data.account && params.data.account.toLowerCase() === "credit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + formattedAmount + '</span>';
                }
            },
        ];

        if (UserExpenseApproveselectedTab === "Unapprove") {
            baseColumns.unshift(getCheckboxColumnDef());
        }

        return baseColumns;
    }

    function getUserExpenseApproveDatasource() {
        return {
            getRows: function (params) {
                const request = {
                    StartRow: params.startRow,
                    PageSize: params.endRow - params.startRow,
                    SearchType: "",
                    SearchValue: "",
                    SortModel: params.sortModel || [],
                    SortColumn: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].colId : "",
                    SortDirection: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].sort : "",
                    filters: Object.entries(params.filterModel || {}).map(([key, value]) => ({
                        colId: key,
                        filterValue: value.filter
                    })),
                    UserId: GetParameterByName('userId'),
                    FilterType: UserExpenseApproveselectedTab,
                    UnapproveFilter: UserExpenseApproveselectedTab === "Unapprove",
                    Approvefilter: UserExpenseApproveselectedTab === "Approve",
                    AccountFilter: UserExpenseApproveselectedTab === "Credit" ? "Credit" : "",
                    Month: UserExpenseApproveselectedMonthFilter === "lastMonthRadio" ? "Last" : UserExpenseApproveselectedMonthFilter === "currentMonthRadio" ? "Current" : "",
                    StartDate: UserExpenseApprovestartDate,
                    EndDate: UserExpenseApproveendDate,
                };

                $.ajax({
                    url: '/ExpenseMaster/GetUserExpenseList',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(request),
                    success: function (response) {
                        params.successCallback(response.rowsThisPage, response.totalRowCount);
                        const rangeDisplay = document.querySelector('#UserExpenseApproveTable .range-display');
                        if (rangeDisplay) updateUserExpenseApproveEnhancedPagination(UserExpenseApproveGridOptions.api, rangeDisplay);
                    },
                    error: function () {
                        params.failCallback();
                    }
                });
            }
        };
    }

    function initGrid() {
        UserExpenseApproveGridOptions = {
            rowHeight: 50,
            columnDefs: getColumnDefs(),
            defaultColDef: {
                sortable: true,
                filter: true,
                cellClass: 'ag-cell-default-style',
                width: 175,
            },
            rowSelection: 'single',
            rowClassRules: {
                'selected-row': params => params.node.isSelected()
            },
            onGridReady: function (params) {
                UserExpenseApproveGridOptions.api = params.api;
                UserExpenseApproveGridOptions.columnApi = params.columnApi;
                UserExpenseApproveGridOptions.api.sizeColumnsToFit();
                createUserExpenseApproveEnhancedPagination(params.api);
            },
            rowModelType: 'infinite',
            cacheBlockSize: 20,
            pagination: true,
            paginationPageSize: 20,
            suppressPaginationPanel: true,
            datasource: getUserExpenseApproveDatasource()
        };

        const gridDiv = document.querySelector('#UserExpenseApproveTable');
        gridDiv.innerHTML = ''; // Clear old grid
        agGrid.createGrid(gridDiv, UserExpenseApproveGridOptions);
    }

    function createUserExpenseApproveEnhancedPagination(gridApi) {
        const paginationContainer = document.createElement('div');
        paginationContainer.className = 'enhanced-pagination-container';

        const pageSizeContainer = document.createElement('div');
        pageSizeContainer.className = 'page-size-container';
        pageSizeContainer.innerHTML = `
            <span>Page Size: </span>
            <select class="page-size-selector">
                <option value="10">10</option>
                <option value="20" selected>20</option>
                <option value="50">50</option>
                <option value="100">100</option>
            </select>
        `;

        const rangeDisplay = document.createElement('div');
        rangeDisplay.className = 'range-display';

        const navContainer = document.createElement('div');
        navContainer.className = 'navigation-container';

        const prevButton = document.createElement('button');
        prevButton.className = 'pagination-button';
        prevButton.innerHTML = '<i class="ri-arrow-left-s-line"></i> Previous';
        prevButton.addEventListener('click', () => {
            gridApi.paginationGoToPreviousPage();
            updateUserExpenseApproveEnhancedPagination(gridApi, rangeDisplay);
        });

        const nextButton = document.createElement('button');
        nextButton.className = 'pagination-button';
        nextButton.innerHTML = 'Next <i class="ri-arrow-right-s-line"></i>';
        nextButton.addEventListener('click', () => {
            gridApi.paginationGoToNextPage();
            updateUserExpenseApproveEnhancedPagination(gridApi, rangeDisplay);
        });

        const pageButtonsContainer = document.createElement('div');
        pageButtonsContainer.className = 'page-buttons';

        navContainer.appendChild(prevButton);
        navContainer.appendChild(pageButtonsContainer);
        navContainer.appendChild(nextButton);

        const pageInfo = document.createElement('div');
        pageInfo.className = 'page-info';

        paginationContainer.appendChild(pageSizeContainer);
        paginationContainer.appendChild(rangeDisplay);
        paginationContainer.appendChild(navContainer);
        paginationContainer.appendChild(pageInfo);

        const eGui = document.querySelector('#UserExpenseApproveTable');
        const paginationEl = document.createElement('div');
        paginationEl.className = 'ag-paging-panel enhanced';
        paginationEl.appendChild(paginationContainer);
        eGui.appendChild(paginationEl);

        const pageSizeSelector = pageSizeContainer.querySelector('.page-size-selector');
        pageSizeSelector.addEventListener('change', function () {
            const newPageSize = Number(this.value);

            UserExpenseApproveGridOptions.cacheBlockSize = newPageSize;
            UserExpenseApproveGridOptions.paginationPageSize = newPageSize;
            UserExpenseApproveGridOptions.datasource = getUserExpenseApproveDatasource();

            initGrid(); // Re-init with new page size
        });

        updateUserExpenseApproveEnhancedPagination(gridApi, rangeDisplay);
    }

    function updateUserExpenseApproveEnhancedPagination(gridApi, rangeDisplay) {
        const currentPage = gridApi.paginationGetCurrentPage() + 1;
        const totalPages = gridApi.paginationGetTotalPages();
        const totalRows = gridApi.paginationGetRowCount();
        const pageSize = UserExpenseApproveGridOptions.paginationPageSize;

        const startRow = totalRows > 0 ? ((currentPage - 1) * pageSize + 1) : 0;
        const endRow = totalRows > 0 ? Math.min(currentPage * pageSize, totalRows) : 0;

        rangeDisplay.textContent = totalRows > 0 ? `${startRow} to ${endRow} of ${totalRows}` : '0 to 0 of 0';

        const pageInfo = document.querySelector('.page-info');
        if (pageInfo) pageInfo.textContent = `Page ${currentPage} of ${totalPages || 1}`;

        const pageButtonsContainer = document.querySelector('.page-buttons');
        if (!pageButtonsContainer) return;

        pageButtonsContainer.innerHTML = '';

        const startPage = Math.max(1, currentPage - 1);
        const endPage = Math.min(totalPages, currentPage + 1);

        for (let i = startPage; i <= endPage; i++) {
            const pageButton = document.createElement('button');
            pageButton.className = `pagination-button ${i === currentPage ? 'active' : ''}`;
            pageButton.textContent = i;
            pageButton.addEventListener('click', () => {
                gridApi.paginationGoToPage(i - 1);
                updateUserExpenseApproveEnhancedPagination(gridApi, rangeDisplay);
            });
            pageButtonsContainer.appendChild(pageButton);
        }

        const prevButton = document.querySelector('.navigation-container .pagination-button:first-child');
        const nextButton = document.querySelector('.navigation-container .pagination-button:last-child');
        if (prevButton) prevButton.disabled = currentPage === 1;
        if (nextButton) nextButton.disabled = currentPage === totalPages || totalPages === 0;

        const pageSizeSelector = document.querySelector('.page-size-selector');
        if (pageSizeSelector) pageSizeSelector.value = pageSize;
    }

    initGrid();

    $('.nav-link').on('click', function () {
        UserExpenseApproveselectedTab = this.id;

        $('.nav-radio').prop('checked', false);
        UserExpenseApproveselectedMonthFilter = "";
        $('#dateFilterContainer').hide();
        $('#txtstartdatebox').val('');
        $('#txtenddatebox').val('');
        UserExpenseApprovestartDate = null;
        UserExpenseApproveendDate = null;

        initGrid(); // re-init grid with updated tab logic (checkbox)
    });

    $('.nav-radio').on('change', function () {
        UserExpenseApproveselectedMonthFilter = this.id;
        const isBetweenSelected = $('#approveExpensebetweenMonth').is(':checked');

        if (isBetweenSelected) {
            $('#dateFilterContainer').show();
        } else {
            $('#dateFilterContainer').hide();
            $('#txtstartdatebox').val('');
            $('#txtenddatebox').val('');
            UserExpenseApprovestartDate = null;
            UserExpenseApproveendDate = null;
        }
        UserExpenseApproveselectedTab = '';
        UserExpenseApproveGridOptions.api.purgeInfiniteCache();
    });

    $('#applyFilters').click(() => {
        UserExpenseApprovestartDate = $('#txtstartdatebox').val() || null;
        UserExpenseApproveendDate = $('#txtenddatebox').val() || null;
        if (UserExpenseApproveGridOptions.api) {
            UserExpenseApproveGridOptions.api.onFilterChanged();
        }
    });

    $('#txtstartdatebox').datepicker({
        format: 'yyyy-mm-dd',
        autoclose: true
    });

    $('#txtenddatebox').datepicker({
        format: 'yyyy-mm-dd',
        autoclose: true
    });
});

document.addEventListener('change', function (e) {
    if (e.target && e.target.name === 'chk_child') {
        toggleApproveButton();
    }
});

function toggleApproveButton() {

    const isAnyChecked = document.querySelectorAll('input[name="chk_child"]:checked').length > 0;

    const btn = document.getElementById('remove-actions');
    btn.style.display = isAnyChecked ? 'inline-block' : 'none';
}

document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('remove-actions').style.display = 'none';
});


let AllExpenseGridOptions = [];
let AllExpenseTodayDate = null;
let AllExpenseselectedTab = $(".Returns.active").attr("id");

$(document).ready(function () {
    function getAllExpenseCheckboxColumnDef() {
        return {
            headerName: "",
            field: "select",
            width: 50,
            suppressMenu: true,
            suppressSorting: false,
            cellRenderer: function (params) {
                if (!params.data || !params.data.id) return '';

                const checkboxId = `ApproveCheckbox_${params.data.id}`;
                return `
                    <div class="custom-control custom-checkbox">
                        <input class="custom-control-input custom-control-input-teal" 
                               data-id="${params.data.id}" 
                               type="checkbox" 
                               name="check_Box"
                               id="${checkboxId}">
                        <label class="custom-control-label" for="${checkboxId}"></label>
                    </div>
                `;
            }
        };
    }

    function getColumnDefs() {
        const baseColumns = [
            {
                headerName: "Employee Name", field: "firstName", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';
                    return params.data.firstName + ' ' + params.data.lastName;
                }
            },
            { headerName: "Description", field: "description", sortable: true, filter: true },
            {
                headerName: "Bill No.", field: "billNumber", sortable: false, filter: false, width: 60,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';
                    return params.data.image
                        ? `<div class="d-flex">
                                <div class="flex-grow-1 tasks_name">${params.data.billNumber}</div>
                                <div class="flex-shrink-0 ms-4 task-icons">
                                    <ul class="list-inline tasks-list-menu mb-0">
                                        <a onclick="downloadBill('${params.data.image}')">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
                                                <path fill="currentColor" d="M13 12h3l-4 4l-4-4h3V8h2v4Zm2-8H5v16h14V8h-4V4ZM3 2.992C3 2.444 3.447 2 3.999 2H16l5 5v13.993A1 1 0 0 1 20.007 22H3.993A1 1 0 0 1 3 21.008V2.992Z" />
                                            </svg>
                                        </a>
                                    </ul>
                                </div>
                            </div>`
                        : `<div class="d-flex">
                                <div class="flex-grow-1 tasks_name">${params.data.billNumber}</div>
                                <div class="flex-shrink-0 ms-4 task-icons"></div>
                            </div>`;
                }
            },
            {
                headerName: "Date", field: "date", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';
                    return getCommonDateformat(params.data.date);
                }
            },
            {
                headerName: "Total Amount", field: "totalAmount", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';

                    function formatNumberWithCommas(number) {
                        return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                    }

                    var formattedAmount = '₹ ' + formatNumberWithCommas(parseFloat(params.data.totalAmount).toFixed(2));
                    var color = params.data.account && params.data.account.toLowerCase() === "credit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + formattedAmount + '</span>';
                }
            },
            { headerName: "Account", field: "account", sortable: true, filter: true }
        ];

        if (AllExpenseselectedTab === "Unapprove") {
            baseColumns.unshift(getAllExpenseCheckboxColumnDef());
        }

        const userFormPermissionArray = Formdata;
        let canEdit = false;
        let canDelete = false;

        for (let i = 0; i < userFormPermissionArray.length; i++) {
            if (userFormPermissionArray[i].formName === "Expenses") {
                canEdit = userFormPermissionArray[i].edit;
                canDelete = userFormPermissionArray[i].delete;
                break;
            }
        }

        if (canEdit || canDelete) {
            baseColumns.push({
                headerName: "Action",
                field: "actions",
                sortable: false,
                filter: false,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';
                    let buttons = '';
                    if (canEdit) {
                        buttons += `<a onclick="EditAllUserExpenseDetails('${params.data.id}')"><i class="fa-regular fa-pen-to-square"></i></a>`;
                    }
                    if (canDelete) {
                        buttons += `<a onclick="deleteExpense('${params.data.id}')"><i class="fas fa-trash"></i></a>`;
                    }
                    return buttons;
                }
            });
        }

        return baseColumns;
    }

    function getAllExpenseDatasource() {
        return {
            getRows: function (params) {
                const request = {
                    StartRow: params.startRow,
                    PageSize: params.endRow - params.startRow,
                    SearchType: "",
                    SearchValue: "",
                    SortModel: params.sortModel || [],
                    SortColumn: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].colId : "",
                    SortDirection: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].sort : "",
                    filters: Object.entries(params.filterModel || {}).map(([key, value]) => ({
                        colId: key,
                        filterValue: value.filter
                    })),
                    UnapproveFilter: AllExpenseselectedTab === "Unapprove",
                    TodayDate: AllExpenseTodayDate,
                };

                $.ajax({
                    url: '/ExpenseMaster/GetExpenseDetailsList',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(request),
                    success: function (response) {
                        params.successCallback(response.rowsThisPage, response.totalRowCount);
                        const rangeDisplay = document.querySelector('#AllExpenseTable .range-display');
                        if (rangeDisplay) updateAllExpenseEnhancedPagination(AllExpenseGridOptions.api, rangeDisplay);
                    },
                    error: function () {
                        params.failCallback();
                    }
                });
            }
        };
    }

    function initGrid() {
        AllExpenseGridOptions = {
            rowHeight: 50,
            columnDefs: getColumnDefs(),
            defaultColDef: {
                sortable: true,
                filter: true,
                cellClass: 'ag-cell-default-style',
                width: 175,
            },
            rowSelection: 'single',
            rowClassRules: {
                'selected-row': params => params.node.isSelected()
            },
            onGridReady: function (params) {
                AllExpenseGridOptions.api = params.api;
                AllExpenseGridOptions.columnApi = params.columnApi;
                AllExpenseGridOptions.api.sizeColumnsToFit();
                createAllExpenseEnhancedPagination(params.api);
            },
            rowModelType: 'infinite',
            cacheBlockSize: 20,
            pagination: true,
            paginationPageSize: 20,
            suppressPaginationPanel: true,
            datasource: getAllExpenseDatasource()
        };

        const gridDiv = document.querySelector('#AllExpenseTable');
        if (gridDiv) {
            gridDiv.innerHTML = '';
            agGrid.createGrid(gridDiv, AllExpenseGridOptions);
        }
    }

    function createAllExpenseEnhancedPagination(gridApi) {
        const paginationContainer = document.createElement('div');
        paginationContainer.className = 'enhanced-pagination-container';

        const pageSizeContainer = document.createElement('div');
        pageSizeContainer.className = 'page-size-container';
        pageSizeContainer.innerHTML = `
            <span>Page Size: </span>
            <select class="page-size-selector">
                <option value="10">10</option>
                <option value="20" selected>20</option>
                <option value="50">50</option>
                <option value="100">100</option>
            </select>
        `;

        const rangeDisplay = document.createElement('div');
        rangeDisplay.className = 'range-display';

        const navContainer = document.createElement('div');
        navContainer.className = 'navigation-container';

        const prevButton = document.createElement('button');
        prevButton.className = 'pagination-button';
        prevButton.innerHTML = '<i class="ri-arrow-left-s-line"></i> Previous';
        prevButton.addEventListener('click', () => {
            gridApi.paginationGoToPreviousPage();
            updateAllExpenseEnhancedPagination(gridApi, rangeDisplay);
        });

        const nextButton = document.createElement('button');
        nextButton.className = 'pagination-button';
        nextButton.innerHTML = 'Next <i class="ri-arrow-right-s-line"></i>';
        nextButton.addEventListener('click', () => {
            gridApi.paginationGoToNextPage();
            updateAllExpenseEnhancedPagination(gridApi, rangeDisplay);
        });

        const pageButtonsContainer = document.createElement('div');
        pageButtonsContainer.className = 'page-buttons';

        navContainer.appendChild(prevButton);
        navContainer.appendChild(pageButtonsContainer);
        navContainer.appendChild(nextButton);

        const pageInfo = document.createElement('div');
        pageInfo.className = 'page-info';

        paginationContainer.appendChild(pageSizeContainer);
        paginationContainer.appendChild(rangeDisplay);
        paginationContainer.appendChild(navContainer);
        paginationContainer.appendChild(pageInfo);

        const eGui = document.querySelector('#AllExpenseTable');
        const paginationEl = document.createElement('div');
        paginationEl.className = 'ag-paging-panel enhanced';
        paginationEl.appendChild(paginationContainer);
        eGui.appendChild(paginationEl);

        const pageSizeSelector = pageSizeContainer.querySelector('.page-size-selector');
        pageSizeSelector.addEventListener('change', function () {
            const newPageSize = Number(this.value);

            AllExpenseGridOptions.cacheBlockSize = newPageSize;
            AllExpenseGridOptions.paginationPageSize = newPageSize;
            AllExpenseGridOptions.datasource = getAllExpenseDatasource();

            initGrid();
        });

        updateAllExpenseEnhancedPagination(gridApi, rangeDisplay);
    }

    function updateAllExpenseEnhancedPagination(gridApi, rangeDisplay) {
        const currentPage = gridApi.paginationGetCurrentPage() + 1;
        const totalPages = gridApi.paginationGetTotalPages();
        const totalRows = gridApi.paginationGetRowCount();
        const pageSize = AllExpenseGridOptions.paginationPageSize;

        const startRow = totalRows > 0 ? ((currentPage - 1) * pageSize + 1) : 0;
        const endRow = totalRows > 0 ? Math.min(currentPage * pageSize, totalRows) : 0;

        rangeDisplay.textContent = totalRows > 0 ? `${startRow} to ${endRow} of ${totalRows}` : '0 to 0 of 0';

        const pageInfo = document.querySelector('.page-info');
        if (pageInfo) pageInfo.textContent = `Page ${currentPage} of ${totalPages || 1}`;

        const pageButtonsContainer = document.querySelector('.page-buttons');
        if (!pageButtonsContainer) return;

        pageButtonsContainer.innerHTML = '';

        const startPage = Math.max(1, currentPage - 1);
        const endPage = Math.min(totalPages, currentPage + 1);

        for (let i = startPage; i <= endPage; i++) {
            const pageButton = document.createElement('button');
            pageButton.className = `pagination-button ${i === currentPage ? 'active' : ''}`;
            pageButton.textContent = i;
            pageButton.addEventListener('click', () => {
                gridApi.paginationGoToPage(i - 1);
                updateAllExpenseEnhancedPagination(gridApi, rangeDisplay);
            });
            pageButtonsContainer.appendChild(pageButton);
        }

        const prevButton = document.querySelector('.navigation-container .pagination-button:first-child');
        const nextButton = document.querySelector('.navigation-container .pagination-button:last-child');
        if (prevButton) prevButton.disabled = currentPage === 1;
        if (nextButton) nextButton.disabled = currentPage === totalPages || totalPages === 0;

        const pageSizeSelector = document.querySelector('.page-size-selector');
        if (pageSizeSelector) pageSizeSelector.value = pageSize;
    }

    $('.nav-link').on('click', function () {
        AllExpenseselectedTab = $(this).attr('id');

        if (AllExpenseselectedTab === "TodayExpense") {
            const today = new Date();
            AllExpenseTodayDate = today.toISOString().split('T')[0];
        } else {
            AllExpenseTodayDate = null;
        }

        initGrid();
    });

    initGrid();
});
document.addEventListener('change', function (e) {
    if (e.target && e.target.name === 'check_Box') {
        toggleApproveAllExpensesButton();
    }
});

function toggleApproveAllExpensesButton() {

    const isAnyChecked = document.querySelectorAll('input[name="check_Box"]:checked').length > 0;

    const btn = document.getElementById('Approve-actions');
    btn.style.display = isAnyChecked ? 'inline-block' : 'none';
}

document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('Approve-actions').style.display = 'none';
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

            var formattedCreditAmount = creditamount.toLocaleString('en-IN', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
            var formattedDebitAmount = debitamount.toLocaleString('en-IN', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
            var pendingAmount = debitamount - creditamount;
            var formattedPendingAmount = pendingAmount.toLocaleString('en-IN', { minimumFractionDigits: 2, maximumFractionDigits: 2 });

            $("#txttotalcreditamount").text('₹' + formattedCreditAmount);
            $("#txtTotalAmount").text('₹' + formattedDebitAmount);
            $("#pendingamount").text('₹' + formattedPendingAmount);
            $("#txttotaldebitedamount").text('₹' + formattedPendingAmount);
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
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full) {
                    function formatNumberWithCommas(number) {
                        return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                    }
                    var formattedAmount = formatNumberWithCommas(parseFloat(data).toFixed(2));
                    return formattedAmount;
                }
            },
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
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full) {
                    function formatNumberWithCommas(number) {
                        return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                    }
                    var formattedAmount = formatNumberWithCommas(parseFloat(data).toFixed(2));
                    return formattedAmount;
                }
            },
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
            {
                "data": "totalAmount",
                "name": "TotalAmount",
                "render": function (data, type, full) {
                    function formatNumberWithCommas(number) {
                        return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                    }
                    var formattedAmount = formatNumberWithCommas(parseFloat(data).toFixed(2));
                    return formattedAmount;
                }
            },
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



let MyExpenseGridOptions = [];
let startDate = null;
let endDate = null;
let selectedTab = $(".Returns.active").attr("id");
let selectedMonthFilter = "";

$(document).ready(function () {
    MyExpenseGridOptions = {
        rowHeight: 50,
        columnDefs: [
            {
                headerName: "", field: "account", sortable: false, filter: false, width: 60,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';
                    if (params.data.account === "Credit") {
                        return '<div class="avatar-xs"><div class="avatar-title bg-success-subtle text-success rounded-circle fs-16"><i class="ri-arrow-left-down-fill"></i></div></div>';
                    } else if (params.data.account === "Debit") {
                        return '<div class="avatar-xs"><div class="avatar-title bg-danger-subtle text-danger rounded-circle fs-16"><i class="ri-arrow-right-up-fill"></i></div></div>';
                    } else {
                        return '';
                    }
                }
            },
            { headerName: "Description", field: "description", sortable: true, filter: true },
            { headerName: "Bill No.", field: "billNumber", sortable: true, filter: true },
            {
                headerName: "Date", field: "date", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';
                    return getCommonDateformat(params.data.date);
                }
            },
            {
                headerName: "Total amount", field: "totalamount", sortable: true, filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';

                    function formatNumberWithCommas(number) {
                        return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                    }
                    var formattedAmount = '₹ ' + formatNumberWithCommas(parseFloat(params.data.totalAmount).toFixed(2));
                    var color = params.data.account && params.data.account.toLowerCase() === "credit" ? "green" : "red";
                    return '<span style="color: ' + color + ';">' + formattedAmount + '</span>';
                }
            },
        ],
        defaultColDef: {
            sortable: true,
            filter: true,
            cellClass: 'ag-cell-default-style',
            width: 175,
        },
        rowSelection: 'single',
        rowClassRules: {
            'selected-row': params => params.node.isSelected()
        },
        onGridReady: function (params) {
            MyExpenseGridOptions.api = params.api;
            MyExpenseGridOptions.columnApi = params.columnApi;
            MyExpenseGridOptions.api.sizeColumnsToFit();
            createMyExpenseEnhancedPagination(params.api);
        },
        rowModelType: 'infinite',
        cacheBlockSize: 20,
        pagination: true,
        paginationPageSize: 20,
        suppressPaginationPanel: true,
        datasource: getMyExpenseDatasource()
    };

    function getMyExpenseDatasource() {
        return {
            getRows: function (params) {

                const currentDate = new Date();
                const currentMonthValue = currentDate.getFullYear() + '-' + String(currentDate.getMonth() + 1).padStart(2, '0');

                let selectMonthlyExpense = "";

                if (selectedMonthFilter === "currentMonthRadio") {
                    selectMonthlyExpense = currentMonthValue;
                } else if (selectedMonthFilter === "lastMonthRadio") {
                    const lastMonthDate = new Date(currentDate.getFullYear(), currentDate.getMonth() - 1, 1);
                    selectMonthlyExpense = lastMonthDate.getFullYear() + '-' + String(lastMonthDate.getMonth() + 1).padStart(2, '0');
                }

                const request = {
                    StartRow: params.startRow,
                    PageSize: params.endRow - params.startRow,
                    SearchType: "",
                    SearchValue: "",
                    SortModel: params.sortModel || [],
                    SortColumn: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].colId : "",
                    SortDirection: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].sort : "",
                    filters: Object.entries(params.filterModel || {}).map(([key, value]) => ({
                        colId: key,
                        filterValue: value.filter
                    })),
                    UserId: $('#txtuserid').val(),
                    FilterType: selectedTab,
                    UnapproveFilter: selectedTab === "Unapprove" ? true : false,
                    Approvefilter: selectedTab === "Approve" ? true : false,
                    AccountFilter: selectedTab === "Credit" ? "Credit" : "",
                    Month: selectMonthlyExpense,
                    StartDate: startDate,
                    EndDate: endDate,
                };

                $.ajax({
                    url: '/ExpenseMaster/GetUserExpenseList',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(request),
                    success: function (response) {
                        params.successCallback(response.rowsThisPage, response.totalRowCount);
                        const rangeDisplay = document.querySelector('#MyExpenseTable .range-display');
                        if (rangeDisplay) updateMyExpenseEnhancedPagination(MyExpenseGridOptions.api, rangeDisplay);
                    },
                    error: function () {
                        params.failCallback();
                    }
                });
            }
        };
    }

    function createMyExpenseEnhancedPagination(gridApi) {
        const paginationContainer = document.createElement('div');
        paginationContainer.className = 'enhanced-pagination-container';

        const pageSizeContainer = document.createElement('div');
        pageSizeContainer.className = 'page-size-container';
        pageSizeContainer.innerHTML = `
            <span>Page Size: </span>
            <select class="page-size-selector">
                <option value="10">10</option>
                <option value="20" selected>20</option>
                <option value="50">50</option>
                <option value="100">100</option>
            </select>
        `;

        const rangeDisplay = document.createElement('div');
        rangeDisplay.className = 'range-display';

        const navContainer = document.createElement('div');
        navContainer.className = 'navigation-container';

        const prevButton = document.createElement('button');
        prevButton.className = 'pagination-button';
        prevButton.innerHTML = '<i class="ri-arrow-left-s-line"></i> Previous';
        prevButton.addEventListener('click', () => {
            gridApi.paginationGoToPreviousPage();
            updateMyExpenseEnhancedPagination(gridApi, rangeDisplay);
        });

        const nextButton = document.createElement('button');
        nextButton.className = 'pagination-button';
        nextButton.innerHTML = 'Next <i class="ri-arrow-right-s-line"></i>';
        nextButton.addEventListener('click', () => {
            gridApi.paginationGoToNextPage();
            updateMyExpenseEnhancedPagination(gridApi, rangeDisplay);
        });

        const pageButtonsContainer = document.createElement('div');
        pageButtonsContainer.className = 'page-buttons';

        navContainer.appendChild(prevButton);
        navContainer.appendChild(pageButtonsContainer);
        navContainer.appendChild(nextButton);

        const pageInfo = document.createElement('div');
        pageInfo.className = 'page-info';

        paginationContainer.appendChild(pageSizeContainer);
        paginationContainer.appendChild(rangeDisplay);
        paginationContainer.appendChild(navContainer);
        paginationContainer.appendChild(pageInfo);

        const eGui = document.querySelector('#MyExpenseTable');
        const paginationEl = document.createElement('div');
        paginationEl.className = 'ag-paging-panel enhanced';
        paginationEl.appendChild(paginationContainer);
        eGui.appendChild(paginationEl);

        const pageSizeSelector = pageSizeContainer.querySelector('.page-size-selector');
        pageSizeSelector.addEventListener('change', function () {
            const newPageSize = Number(this.value);

            // Destroy and recreate grid with new block size
            const gridDiv = document.querySelector('#MyExpenseTable');

            MyExpenseGridOptions = {
                ...MyExpenseGridOptions,
                cacheBlockSize: newPageSize,
                paginationPageSize: newPageSize,
                datasource: getMyExpenseDatasource(),
            };

            // Clear old grid and re-init
            gridDiv.innerHTML = '';
            agGrid.createGrid(gridDiv, MyExpenseGridOptions);
        });

        updateMyExpenseEnhancedPagination(gridApi, rangeDisplay);
    }

    function updateMyExpenseEnhancedPagination(gridApi, rangeDisplay) {
        const currentPage = gridApi.paginationGetCurrentPage() + 1;
        const totalPages = gridApi.paginationGetTotalPages();
        const totalRows = gridApi.paginationGetRowCount();
        const pageSize = MyExpenseGridOptions.paginationPageSize;

        const startRow = totalRows > 0 ? ((currentPage - 1) * pageSize + 1) : 0;
        const endRow = totalRows > 0 ? Math.min(currentPage * pageSize, totalRows) : 0;

        rangeDisplay.textContent = totalRows > 0 ? `${startRow} to ${endRow} of ${totalRows}` : '0 to 0 of 0';

        const pageInfo = document.querySelector('.page-info');
        if (pageInfo) pageInfo.textContent = `Page ${currentPage} of ${totalPages || 1}`;

        const pageButtonsContainer = document.querySelector('.page-buttons');
        if (!pageButtonsContainer) return;

        pageButtonsContainer.innerHTML = '';

        const startPage = Math.max(1, currentPage - 1);
        const endPage = Math.min(totalPages, currentPage + 1);

        for (let i = startPage; i <= endPage; i++) {
            const pageButton = document.createElement('button');
            pageButton.className = `pagination-button ${i === currentPage ? 'active' : ''}`;
            pageButton.textContent = i;
            pageButton.addEventListener('click', () => {
                gridApi.paginationGoToPage(i - 1);
                updateMyExpenseEnhancedPagination(gridApi, rangeDisplay);
            });
            pageButtonsContainer.appendChild(pageButton);
        }

        const prevButton = document.querySelector('.navigation-container .pagination-button:first-child');
        const nextButton = document.querySelector('.navigation-container .pagination-button:last-child');
        if (prevButton) prevButton.disabled = currentPage === 1;
        if (nextButton) nextButton.disabled = currentPage === totalPages || totalPages === 0;

        const pageSizeSelector = document.querySelector('.page-size-selector');
        if (pageSizeSelector) pageSizeSelector.value = pageSize;
    }

    const userFormPermissionArray = Formdata;
    let canEdit = false;
    let canDelete = false;
    for (let i = 0; i < userFormPermissionArray.length; i++) {
        if (userFormPermissionArray[i].formName === "Expenses") {
            canEdit = userFormPermissionArray[i].edit;
            canDelete = userFormPermissionArray[i].delete;
            break;
        }
    }

    if (canEdit || canDelete) {
        MyExpenseGridOptions.columnDefs.push({
            headerName: "Action",
            field: "actions",
            sortable: false,
            filter: false,
            cellRenderer: function (params) {
                if (!params.data || !params.data.id) return '';
                let buttons = '';
                if (canEdit) {
                    buttons += `
                         <a onclick="EditExpenseDetails('${params.data.id}')"><i class="fa-regular fa-pen-to-square"></i></a>`;
                }

                if (canDelete) {
                    buttons += `
                    <a onclick="deleteExpense('${params.data.id}')"><i class="fas fa-trash" style="margin-left:10px;"></i></a>`;
                }
                return buttons;
            }
        });
    }

    const myGridElement = document.querySelector('#MyExpenseTable');
    agGrid.createGrid(myGridElement, MyExpenseGridOptions);

    // Tab click handling
    $('.nav-link').on('click', function () {
        selectedTab = this.id;
        MyExpenseGridOptions.api.purgeInfiniteCache();

        $('.nav-radio').prop('checked', false);
        selectedMonthFilter = "";
        $('#dateFilterContainer').hide();
        $('#txtstartdatebox').val('');
        $('#txtenddatebox').val('');
        startDate = null;
        endDate = null;
    });

    // Radio (month) filter handling
    $('.nav-radio').on('change', function () {
        selectedMonthFilter = this.id;
        const isBetweenSelected = $('#betweenMonthRadio').is(':checked');

        if (isBetweenSelected) {
            $('#dateFilterContainer').show();
        } else {
            $('#dateFilterContainer').hide();
            $('#txtstartdatebox').val('');
            $('#txtenddatebox').val('');
            startDate = null;
            endDate = null;
        }

        selectedTab = ''; // Clear tab when date filter is used
        MyExpenseGridOptions.api.purgeInfiniteCache();
    });

    $('#applyFilters').click(() => {
        startDate = $('#txtstartdatebox').val() || null;
        endDate = $('#txtenddatebox').val() || null;
        if (MyExpenseGridOptions.api) {
            MyExpenseGridOptions.api.onFilterChanged();
        }
    });
    $('#txtstartdatebox').datepicker({
        format: 'yyyy-mm-dd',
        autoclose: true
    });
    $('#txtenddatebox').datepicker({
        format: 'yyyy-mm-dd',
        autoclose: true
    });
});


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