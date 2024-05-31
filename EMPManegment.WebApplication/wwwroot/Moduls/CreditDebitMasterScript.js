
$(document).ready(function () {
    GetAllVendorData();
    GetAllTransactionData();
    GetPaymentMethodList();
    GetPaymentTypeList();
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
                    return '<a href="#" onclick="GetVendorDetails(\'' + full.vid + '\')" class="link-primary" style="display: flex; align-items: center;">' + profileImageHtml + '<span style="margin-left: 5px;">' + full.vendorCompany + '</span></a>';   
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

function GetVendorDetails(Vid) {
    debugger
    GetCreditDebitTotalAmount(Vid);
    window.location = '/Invoice/PayVendors?Vid=' + Vid;
}

function GetAllTransactionData() {
    $('#transactionTable').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "Post",
            url: '/Invoice/GetAllTransactiondata',
            dataType: 'json'
        },
        autoWidth: false,
        columns: [
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
            { "data": "vendorAddress", "name": "VendorAddress" },
        ],
        columnDefs: [
            {
                targets: 4,
                render: function (data, type, row) {
                    var amountClass = parseFloat(data) < 0 ? 'text-success' : 'text-success';
                    return '<span class="' + amountClass + '">' + data + '</span>';
                }
            },
            {
                targets: 5,
                render: function (data, type, row) {
                    var amountClass = parseFloat(data) < 0 ? 'text-danger' : 'text-danger';
                    return '<span class="' + amountClass + '">' + data + '</span>';
                }
            }
        ],
        createdRow: function (row, data, dataIndex) {
            $(row).addClass('text-muted');
            var htmlContent = '<td class="id" style="display:none;"><a href="javascript:void(0);" class="fw-medium link-primary">#VZ001</a></td>';
            htmlContent += '<td><div class="avatar-xs"><div class="avatar-title bg-danger-subtle text-danger rounded-circle fs-16"><i class="ri-arrow-right-up-fill"></i></div></div></td>';
            htmlContent += '<td class="date">' + data.vendorName + '<small class="text-muted"></small></td>';
            htmlContent += '<td class="form_name">' + getCommonDateformat(data.date) + '</td>';
            htmlContent += '<td class="to_name">' + data.paymentMethodName + '</td>';
            htmlContent += '<td class="to_name">' + data.paymentTypeName + '</td>';
            htmlContent += '<td class="to_name text-success">' + data.creditDebitAmount + '</td>';
            htmlContent += '<td class="to_name text-danger">' + data.pendingAmount + '</td>';
            htmlContent += '<td class="status"><span class="badge bg-primary-subtle text-primary fs-11"><i class="ri-time-line align-bottom"></i> Processing</span></td>';

            $(row).html(htmlContent);
        }
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
    debugger
    var value = $('#txtcreditdebitamount').val();
    if (value.trim() === '') {

        $('#warningMessage').text('Please enter value for credit amount!!');
        return;
    }
    else {
        debugger
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
                Swal.fire({
                    title: result.message,
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                }).then(function () {
                    window.location = '/Invoice/VendorTransactions';
                });
            },
            error: function (xhr, status, error) {
                Swal.fire({
                    title: 'Error',
                    text: 'An error occurred while processing your request.',
                    icon: 'error',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                });
            }
        });
    }
}

function GetCreditDebitTotalAmount(Vid) {
    var form_data = new FormData();
    form_data.append("CREDITDEBITDETAILSBYID", JSON.stringify(Vid));
    $.ajax({
        url: '/Invoice/GetCreditDebitDetailsByVendorId',
        type: 'Post',
        data: form_data,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (result) {
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

            $('#txtcreditdebitamount').on('input', function () {
                var enteredAmount = parseFloat($(this).val());

                if (!isNaN(enteredAmount)) {
                    var pendingAmount = totalpendingAmount - enteredAmount;

                    if (enteredAmount > totalpendingAmount) {
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
};


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
