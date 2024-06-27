$(document).ready(function () {
    GetAllVendorData();
    GetPaymentMethodList();
    GetPaymentTypeList();
    AllTransactionData();
    getVendorTransactionList();
});

function GetAllVendorData() {
    var colorClasses = [
        { bgClass: 'bg-primary-subtle', textClass: 'text-primary' },
        { bgClass: 'bg-secondary-subtle', textClass: 'text-secondary' },
        { bgClass: 'bg-success-subtle', textClass: 'text-success' },
        { bgClass: 'bg-info-subtle', textClass: 'text-info' },
        { bgClass: 'bg-warning-subtle', textClass: 'text-warning' },
        { bgClass: 'bg-danger-subtle', textClass: 'text-danger' },
        { bgClass: 'bg-dark-subtle', textClass: 'text-dark' }
    ];

    $('#VendorTableData').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        destroy: true, // Use 'destroy' instead of 'bDestroy'
        ajax: {
            type: "POST",
            url: '/Invoice/GetVendorList',
            dataType: 'json'
        },
        columns: [
            {
                data: "vendorCompany",
                name: "VendorCompany",
                render: function (data, type, full) {
                    var profileImageHtml;
                    if (full.vendorCompanyLogo && full.vendorCompanyLogo.trim() !== '') {
                        profileImageHtml = '<img src="/Content/Image/' + full.vendorCompanyLogo +
                            '" style="height: 40px; width: 40px; border-radius: 50%;" ' +
                            'onmouseover="showIcons(event, this.parentElement)" ' +
                            'onmouseout="hideIcons(event, this.parentElement)">';
                    } else {
                        var initials = (full.vendorCompany ? full.vendorCompany[0] : '');
                        var randomColor = colorClasses[Math.floor(Math.random() * colorClasses.length)];
                        profileImageHtml = '<div class="flex-shrink-0 avatar-xs me-2">' +
                            '<div class="avatar-title ' + randomColor.bgClass + ' ' + randomColor.textClass +
                            ' rounded-circle" style="height: 40px; width: 40px; border-radius: 50%;">' +
                            initials.toUpperCase() + '</div></div>';
                    }
                    return '<a href="#" onclick="GetCreditDebitTotalAmount(\'' + full.vid + '\')" class="link-primary" style="display: flex; align-items: center;">' + profileImageHtml + '<span style="margin-left: 5px;">' + full.vendorCompany + '</span></a>';
                }
            },
            {
                name: "VendorFullName",
                render: function (data, type, full) {
                    return full.vendorFirstName + ' ' + full.vendorLastName;
                }
            },
            { data: "vendorEmail", name: "VendorEmail" },
            { data: "vendorPhone", name: "VendorPhone" }
        ],
        columnDefs: [{
            defaultContent: "",
            targets: "_all"
        }]
    });
}

function GetPaymentMethodList() {

    $.ajax({
        url: '/PurchaseOrderMaster/GetPaymentMethodList',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#txtpaymentmethod').append('<Option value=' + data.id + '>' + data.paymentMethod + '</Option>')
            });
            var firstPaymentMethod = result[0];

            $('#txtpaymentmethod').val(firstPaymentMethod.id);
        }
    });
}
function GetPaymentTypeList() {
    $.ajax({
        url: '/ExpenseMaster/GetPaymentTypeList',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#paymenttype').append('<Option value=' + data.id + '>' + data.type + '</Option>')
            });
            var firstPaymentType = result[0];

            $('#paymenttype').val(firstPaymentType.id);
        }
    });
}

function InsertCreditDebitDetails() {

    var value = $('#txtcreditdebitamount').val();
    if (value.trim() === '') {

        $('#warningMessage').text('Please enter value!!');
        toastr.warning("Kindly fill all datafield");
    }
    else {
        var VendorId = document.getElementById("txtvendorid").textContent;

        var objData = {
            VendorId: document.getElementById("txtvendorid").textContent,
            InvoiceNo: document.getElementById("txtinvoiceno").textContent,
            PaymentType: $("#paymenttype").val(),
            PaymentMethod: $("#txtpaymentmethod").val(),
            CreditDebitAmount: $("#txtcreditdebitamount").val(),
            PendingAmount: $("#txtpendingamount").val(),
            TotalAmount: $("#txttotalamount").val(),
            CreatedBy: $("#txtuserid").val(),
        };
        var form_data = new FormData();
        form_data.append("CREDITDEBITDETAILS", JSON.stringify(objData));
        $.ajax({
            url: '/Invoice/InsertCreditDebitDetails',
            type: 'POST',
            data: form_data,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.code == 200) {
                    Swal.fire({
                        title: result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/Invoice/PayVendors?Vid=' + VendorId;
                    });
                }
                else {
                    toastr.error(Result.message);
                }
            },
            error: function () {
                toastr.error('An error occurred while processing your request.');
            }
        });
    }
}

//function GetVendorDetails(Vid) {
//    GetCreditDebitTotalAmount(Vid);
//    window.location = '/Invoice/PayVendors?Vid=' + Vid;
//}
//$(document).ready(function () {
//    var Vid = $("#textvendorId").val();
///*    GetCreditDebitTotalAmount(Vid);*/
//})
function GetCreditDebitTotalAmount(Vid) {

    $.ajax({
        url: '/Invoice/GetCreditDebitDetailsByVendorId?VendorId=' + Vid,
        type: 'POST',
        dataType: 'json',
        success: function (result) {
            if (result.length == 0) {
                toastr.warning('There is no data for selected vendor.')
                return
            }
            else {
                window.location = '/Invoice/PayVendors?Vid=' + Vid;
                var total = 0;
                result.forEach(function (obj) {
                    if (obj.creditDebitAmount) {
                        total += obj.creditDebitAmount;
                    }
                });

                $("#txttotalcreditamount").text('₹' + total);
                var totalAmount = parseFloat($('#txttotalamount').val());
                var totalpendingAmount = totalAmount - total;

                $("#txttotalpendingamount").text('₹' + totalpendingAmount);
                $("#pendingamount").text('₹' + totalpendingAmount);

                $('#txtcreditdebitamount').off('input').on('input', function () {
                    var enteredAmount = parseFloat($(this).val());

                    if (!isNaN(enteredAmount)) {
                        var pendingAmount = totalpendingAmount - enteredAmount;

                        if (enteredAmount > totalpendingAmount) {
                            $('#warningMessage').text('Entered amount cannot exceed pending amount.');
                        } else {
                            $('#warningMessage').text('');
                            $('#txtpendingamount').val(pendingAmount.toFixed(2));
                        }
                    } else {
                        $('#warningMessage').text('');
                        $('#txtpendingamount').val('');
                    }
                });
            }

        },
        error: function (xhr, status, error) {
            toastr.error("Error in AJAX request:", status, error);
        }
    });
}



function getLastTransaction(Vid) {

    $.ajax({
        url: '/Invoice/GetLastTransactionByVendorId',
        type: 'GET',
        dataType: 'html',
        data: { Vid: Vid },
        success: function (response) {
            $("#lasttenTransaction").html(response);
            $("#zoomInModal").modal('show');
        },
    });
}



function getVendorTransactionList() {
    Vid = $("#inputVendorId").val();
    $('#vendorAllTransactionTable').DataTable({
        processing: false,
        serverSide: true,
        searching: true,
        destroy: true,
        ajax: {
            type: "POST",
            url: '/Invoice/GetVendorTransactionList?Vid=' + Vid,
            dataType: 'json',
        },
        autoWidth: false,
        columns: [
            {
                "render": function (data, type, row) {
                    return '<div class="avatar-xs" > <div class="avatar-title bg-danger-subtle text-danger rounded-circle fs-16"><i class="ri-arrow-right-up-fill"></i></div></div>';
                }
            },
            { "data": "vendorName", "name": "VendorName" },
            {
                "data": "date", "name": "Date",
                "render": function (data, type, row) {
                    return getCommonDateformat(data);
                }
            },
            { "data": "paymentMethodName", "name": "PaymentMethodName" },
            { "data": "paymentTypeName", "name": "PaymentTypeName" },
            { "data": "creditDebitAmount", "name": "CreditDebitAmount" },
            { "data": "pendingAmount", "name": "PendingAmount" },
            {
                "render": function (data, type, row) {
                    return '<span class="badge bg-primary-subtle text-primary fs-11"><i class="ri-time-line align-bottom"></i> Processing</span>';
                }
            },
        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all",
        }]
    });
}

$(document).ready(function () {
    $('#searchcreditdebitlist').on('keyup', function () {
        var value = $(this).val().toLowerCase();
        var hasVisibleItems = false;

        $('#AllVendorCreditDebitList .transaction-item').filter(function () {
            var isVisible = $(this).text().toLowerCase().indexOf(value) > -1;
            $(this).toggle(isVisible);
            if (isVisible) {
                hasVisibleItems = true;
            }
        });

        if (hasVisibleItems) {
            $('.noresult').hide();
        } else {
            $('.noresult').show();
        }
    });
});


function GetCompanyNameList() {
    $.ajax({
        url: '/ProductMaster/GetVendorsNameList',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#textCompanyName').append('<option value="' + data.id + '">' + data.vendorCompany + '</option>');
            });
        },
    });
}
function AllTransactionData() {
    $.ajax({
        url: '/Invoice/AllVendorTransaction',
        type: 'GET',
        dataType: 'html',
        success: function (response) {
            $("#AllTransactionPartial").html(response);
        },
        error: function () {
            toastr.error("Can't get Data");
        }
    });
}

$(document).ready(function () {
    GetCompanyNameList();

    $('#textCompanyName').on('change', SortCompanyName);
});

function SortCompanyName() {
    var CompanyId = $('#textCompanyName').val();
    if (CompanyId == "AllCompany") {
        $.ajax({
            url: '/Invoice/AllVendorTransaction',
            type: 'GET',
            dataType: 'html',
            success: function (response) {
                $("#AllTransactionPartial").html(response);
            },
        });
    } else {
        $.ajax({
            url: '/Invoice/AllVendorTransaction?VendorId=' + CompanyId,
            type: 'GET',
            dataType: 'html',
            success: function (response) {

                if (response == "\r\n") {
                    toastr.warning("There is no data for selected company!");
                    $("#AllTransactionPartial").html(response);
                } else {
                    $("#AllTransactionPartial").html(response);
                }
            },
        });
    }
}

function SearchDatesInVendorCreditDebitList() {
    var StartDate = $('#vendorstartdate').val();
    var EndDate = $('#vendorenddate').val();
    var CompanyId = $('#textCompanyName').val();
    if (StartDate == "" && EndDate == "") {
        toastr.warning("Select dates");
    } else if (StartDate == "") {
        toastr.warning("Select Start date");
    } else if (EndDate == "") {
        toastr.warning("Select End date");
    } else {
        $.ajax({
            url: '/Invoice/AllVendorTransaction?Startdate=' + StartDate + '&Enddate=' + EndDate + '&VendorId=' + CompanyId,
            type: 'GET',
            dataType: 'html',
            success: function (response) {
                if (response == "\r\n") {
                    toastr.warning("There is no data for selected date!");
                    $("#AllTransactionPartial").html(response);
                } else {
                    $("#AllTransactionPartial").html(response);
                }
            },
        });
    }
}


