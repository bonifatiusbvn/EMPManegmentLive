var datas = userPermissions
$(document).ready(function () {
    fn_updatePRProductAmount();
    GetPurchaseRequestList();
    fn_updatePRTotals();
    CountCartTotalItems();
    $(document).on('input', '.product-quantity', function () {
        fn_updatePRProductAmount()
    });

    $(document).on('keydown', '.product-quantity', function (event) {
        if (event.key === 'Enter') {
            $(this).blur();
        }
    });

    $(document).on('keydown', '#txtproductamount', function (event) {
        if (event.key === 'Enter') {
            $(this).blur();
        }
    });

    $(document).on('focusout', '.product-quantity', function () {
        $(this).trigger('input');
    });
});

$(document).ready(function () {

    function data(datas) {
        userPermission = datas;
        GetPRData(userPermission);
    }

    function GetPRData(userPermission) {
        var userPermissionArray = JSON.parse(userPermission);

        var canEdit = false;
        var canDelete = false;

        for (var i = 0; i < userPermissionArray.length; i++) {
            var permission = userPermissionArray[i];
            if (permission.formName.trim() === "Purchase Request List") {
                canEdit = permission.edit;
                canDelete = permission.delete;
                break;
            }
        }

        var columns = [
            {
                "data": "prNo","name": "PrNo",
                "render": function (data, type, full) {
                    return '<h5 class="fs-15"><a href="/PurchaseRequest/PurchaseRequestDetails?prNo=' + full.prNo + '" class="fw-medium link-primary">' + full.prNo + '</a></h5>';
                }
            },
            {
                "data": null, "name": "FullName",
                "render": function (data, type, full) {
                    return full.fullName + ' ( ' + full.userName + ' )';
                },
            },
            { "data": "projectName", "name": "ProjectName" },
            { "data": "productName", "name": "ProductName" },
            { "data": "quantity", "name": "Quantity" },
            {
                "data": null,
                "render": function (data, type, full, meta) {
                    var isChecked = full.isApproved;
                    return '<div class="form-check">' +
                        '<input class="form-check-input" ' +
                        'data-id="' + full.prId + '" ' +
                        'data-approved="' + isChecked + '" ' +
                        'type="checkbox" name="chk_child"' + (isChecked ? ' checked' : '') + '>' +
                        '</div>';
                },
                "orderable": false
            }
        ];

        if (canEdit || canDelete) {
            columns.push({
                "data": null,
                "orderable": false,
                "searchable": false,
                "render": function (data, type, full) {
                    var buttons = '<ul class="list-inline hstack gap-2 mb-0">';

                    if (canEdit) {
                        buttons += '<a class="btn text-info" href="/PurchaseRequest/CreatePurchaseRequest?id=' + full.prNo + '">' +
                            '<i class="fa-regular fa-pen-to-square"></i></a>';
                    }

                    if (canDelete) {
                        buttons += '<a class="text-danger" onclick="DeletePurchaseRequest(\'' + full.prNo + '\')">' +
                            '<i class="fas fa-trash"></i></a>';
                    }

                    buttons += '</ul>';
                    return buttons;
                }
            });
        }

        $('#PRListTable').DataTable({
            processing: false,
            serverSide: true,
            filter: true,
            "bDestroy": true,
            ajax: {
                type: "POST",
                url: '/PurchaseRequest/GetPRList',
                dataType: 'json'
            },
            columns: columns,
            columnDefs: [{
                "defaultContent": "",
                "targets": "_all"
            }],
            drawCallback: function () {

                updateCheckedAllState();
            }
        });
    }
    data(datas);
});

function fn_SearchItemDetailsById(Id) {
    $.ajax({
        url: '/PurchaseRequest/DisplayProductDetailsListById?ProductId=' + Id,
        type: 'Post',
        datatype: 'json',
        processData: false,
        contentType: false,
        complete: function (Result) {
            fn_AddNewPRRow(Result.responseText);
        }
    });
}

var count = 0;
function fn_AddNewPRRow(Result) {
    var newProductRow = $(Result);
    var newProductId = newProductRow.find('.card-body').data('product-id');
    var isDuplicate = false;

    $('#displayPurchaseRequest .products .card-body').each(function () {
        var existingProductRow = $(this);
        var existingProductId = existingProductRow.data('product-id');
        if (existingProductId === newProductId) {
            isDuplicate = true;
            return false;
        }
    });

    if (!isDuplicate) {
        count++;
        $("#displayPurchaseRequest").append(Result);
        fn_updatePRProductAmount();
        fn_updatePRTotals();
        showHidePRCreatebtn();
        CountCartTotalItems();
    } else {
        toastr.warning("Product already added!");
    }
}
function fn_updatePRProductAmount() {

    $(".products").each(function () {
        var row = $(this);
        var subtotal = parseFloat(row.find("#dspperunitprice").text().replace('₹', ''));
        var totalquantity = parseFloat(row.find("#txtproductquantity").val());
        var gstAmount = parseFloat(row.find("#txtProductGstAmount").text().replace('₹', ''));
        var totalGstamount = gstAmount * totalquantity;
        var Amount = subtotal * totalquantity;
        var TotalAmount = totalGstamount + Amount;

         var formattedTotalAmount = TotalAmount.toLocaleString('en-IN', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
        row.find("#dsptotalAmount").text('₹' + formattedTotalAmount);

    });
    fn_updatePRTotals();
}
function fn_updatePRTotals() {

    var cartSubtotal = 0;
    var cartGst = 0;
    var cartTotalamount = 0;

    $(".products").each(function () {
        var row = $(this);

        var subtotal = parseFloat(row.find("#dspperunitprice").text().replace('₹', ''));
        var gst = parseFloat(row.find("#txtProductGstAmount").text().replace('₹', ''));
        var quantity = parseFloat(row.find("#txtproductquantity").val());
        var totalGstamount = gst * quantity;
        var Amount = subtotal * quantity;

        cartSubtotal += Amount;
        cartGst += totalGstamount;
        cartTotalamount = cartSubtotal + cartGst;
    });

    $("#cart-subtotal").html('₹' + cartSubtotal.toFixed(2));
    $("#cart-Gst").html('₹' + cartGst.toFixed(2));
    $("#cart-total").html('₹' + cartTotalamount.toFixed(2));
}
function CountCartTotalItems() {
    var totalItems = $('.products').length;
    $("#cartTotalItemCount").html('Your Select ' + '(' + totalItems + ' ' + 'items)');
};
function removeProduct(btn) {
    $(btn).closest(".products").remove();
    fn_updatePRProductAmount();
    fn_updatePRTotals();
    CountCartTotalItems();
    showHidePRCreatebtn();
}
function showHidePRCreatebtn() {
    var totalAmount = $("#dsptotalAmount").text();
    if (totalAmount != "") {
        $("#btnpurchaserequest").show();
    } else {
        $("#btnpurchaserequest").hide();
    }
}
function preventEmptyValue(input) {

    if (input.value === "") {

        input.value = 1;
    }
}
function ApproveUnapprovePR() {
    Swal.fire({
        title: "Are you sure you want to approve this purchase request?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, enter it!",
        cancelButtonText: "No, cancel!",
        confirmButtonClass: "btn btn-primary w-xs me-2 mt-2",
        cancelButtonClass: "btn btn-danger w-xs mt-2",
        buttonsStyling: false,
        showCloseButton: true
    }).then((result) => {
        if (result.isConfirmed) {
            let selectedIds = [];
            $("input[name=chk_child]").each(function () {
                selectedIds.push({
                    PrId: $(this).attr("data-id"),
                    IsApproved: $(this).is(":checked")
                });
            });


            var PRDetails = {
                PRList: selectedIds
            };

            var form_data = new FormData();
            form_data.append("PRIsApproved", JSON.stringify(PRDetails));

            $.ajax({
                url: '/PurchaseRequest/ApproveUnapprovePR',
                type: 'POST',
                processData: false,
                contentType: false,
                data: form_data,
                success: function (Result) {
                    if (Result.code == 200) {
                        Swal.fire({
                            title: Result.message,
                            icon: "success",
                            confirmButtonClass: "btn btn-primary w-xs mt-2",
                            buttonsStyling: false
                        }).then(function () {
                            window.location = '/PurchaseRequest/PurchaseRequests';
                        });
                    } else {
                        toastr.error(Result.message);
                    }
                }
            });
        } else if (result.dismiss === Swal.DismissReason.cancel) {
            Swal.fire(
                'Cancelled',
                'User has no changes.😊',
                'error'
            ).then(function () {
                window.location = '/PurchaseRequest/PurchaseRequests';
            });
        }
    });
}

function GetPurchaseRequestList() {
    $.ajax({
        url: '/PurchaseRequest/GetPurchaseRequestList',
        type: 'Post',
        dataType: 'json',
        processData: false,
        contentType: false,
        complete: function (result) {
            $('#addNewlink').html(result.responseText);
        },
    })
}
function CreatePurchaseRequest() {
    var TotalAmount = $("#dsptotalAmount").text();
    if (TotalAmount != "") {
        var purchaseRequests = [];
        $(".products").each(function () {
            var orderRow = $(this);
            var objData = {
                UserId: orderRow.find("#txtuserId").val(),
                ProjectId: orderRow.find("#txtprojectId").val(),
                ProductId: orderRow.find("#txtproductId").val(),
                ProductName: orderRow.find("#txtProductName").val(),
                ProductTypeId: orderRow.find("#txtproducttype").val(),
                Quantity: orderRow.find("#txtproductquantity").val(),
                CreatedBy: $('#txtuserId').val(),
                PrNo: $('#prNo').val(),
                PrDate: $('#txtPrDate').val(),
            };
            purchaseRequests.push(objData);
        });

        var data = {
            PRList: purchaseRequests,
        }

        var form_data = new FormData();
        form_data.append("InsertPRDetails", JSON.stringify(data));

        $.ajax({
            url: '/PurchaseRequest/CreateMutiplePurchaseRequest',
            type: 'POST',
            data: form_data,
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
                        window.location = '/PurchaseRequest/PurchaseRequests';
                    });
                }
                else {
                    toastr.error(Result.message);
                }
            },
            error: function () {
                toastr.error("An error occurred while creating the purchase request.");
            }
        });
    } else {
        toastr.warning("Kindly add the products");
    }
}
function UpdatePurchaseRequestDetails() {

    var TotalAmount = $("#dsptotalAmount").text();
    if (TotalAmount != "") {
        var purchaseRequests = [];
        $(".products").each(function () {
            var orderRow = $(this);
            var objData = {
                UserId: orderRow.find("#txtuserId").val(),
                ProjectId: $("#textModelProjectIdPR").val(),
                ProductId: orderRow.find("#txtproductId").val(),
                ProductName: orderRow.find("#txtProductName").val(),
                ProductTypeId: orderRow.find("#txtproducttype").val(),
                Quantity: orderRow.find("#txtproductquantity").val(),
                UpdatedBy: $('#txtuserId').val(),
                PrNo: $('#prNo').val(),
                PrDate: $('#txtUpdatePrDate').val(),
            };
            purchaseRequests.push(objData);
        });

        var data = {
            PRList: purchaseRequests,
            PrNo: $('#prNo').val(),
        }

        var form_data = new FormData();
        form_data.append("UpdatePRDetails", JSON.stringify(data));

        $.ajax({
            url: '/PurchaseRequest/UpdatePurchaseRequestDetails',
            type: 'Post',
            data: form_data,
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
                        window.location = '/PurchaseRequest/PurchaseRequests';
                    });
                }
                else {
                    toastr.error(Result.message);
                }
            }
        })
    }
    else {
        toastr.warning("Please Add Product!");
    }

}
function DeletePurchaseRequest(PrNo) {

    Swal.fire({
        title: "Are you sure want to delete this?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, delete it!",
        cancelButtonText: "No, cancel!",
        confirmButtonClass: "btn btn-primary w-xs me-2 mt-2",
        cancelButtonClass: "btn btn-danger w-xs mt-2",
        buttonsStyling: false,
        showCloseButton: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/PurchaseRequest/DeletePurchaseRequest?PrNo=' + PrNo,
                type: 'POST',
                dataType: 'json',
                success: function (Result) {
                    if (Result.code == 200) {
                        Swal.fire({
                            title: Result.message,
                            icon: 'success',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'OK'
                        }).then(function () {
                            GetPRData();
                        })
                    }
                    else {
                        toastr.error(Result.message);
                    }
                },
                error: function () {
                    Swal.fire({
                        title: "Can't delete order!",
                        icon: 'warning',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        GetPRData();
                    })
                }
            })
        } else if (result.dismiss === Swal.DismissReason.cancel) {

            Swal.fire(
                'Cancelled',
                'Order have no changes.!!😊',
                'error'
            );
        }
    });
}
function createPR() {
    if ($("#txtProjectId").val() == "") {
        Swal.fire({
            title: "Kindly select project on dashboard.",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        });
    }
    else {
        window.location = '/PurchaseRequest/CreatePurchaseRequest';
    }
}






