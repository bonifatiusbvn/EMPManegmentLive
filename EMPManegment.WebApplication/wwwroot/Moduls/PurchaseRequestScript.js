﻿$(document).ready(function () {
    GetAllItemDetailsList();
    updateTotals();
    GetPurchaseRequestList();
    GetPRData();
    showHidebtn();
});

function GetAllItemDetailsList(page) {
    var searchText = $('#mdProductSearch').val();

    $.get("/PurchaseRequest/GetAllProductDetailsList", { searchText: searchText, page: page })
        .done(function (result) {
            $("#mdlistofItem").html(result);
        })
}

GetAllItemDetailsList(1);


$(document).on("click", ".pagination a", function (e) {
    e.preventDefault();
    var page = $(this).text();
    GetAllItemDetailsList(page);
});

$(document).on("click", "#backButton", function (e) {
    e.preventDefault();
    var page = $(this).text();
    GetAllItemDetailsList(page);
});


function clearsearchtext() {
    $('#mdProductSearch').val('');
    GetAllItemDetailsList();
}

function filterallItemTable() {
    var searchText = $('#mdProductSearch').val();

    $.ajax({
        url: '/PurchaseRequest/GetAllProductDetailsList',
        type: 'GET',
        data: {
            searchText: searchText,
        },
        success: function (result) {
            $("#mdlistofItem").html(result);
        },
    });
}

function SerchItemDetailsById(Id) {
    $.ajax({
        url: '/ProductMaster/DisplayProductDetilsListById?ProductId=' + Id,
        type: 'Post',
        datatype: 'json',
        processData: false,
        contentType: false,
        complete: function (Result) {
            AddNewRow(Result.responseText);
        }
    });
}

var count = 0;
function AddNewRow(Result) {
    var newProductRow = $(Result);
    var productId = newProductRow.data('product-id');
    var newProductId = newProductRow.attr('data-product-id');
    var isDuplicate = false;

    $('#displayPurchaseRequest .products').each(function () {
        var existingProductRow = $(this);
        var existingProductId = existingProductRow.attr('data-product-id');
        if (existingProductId === newProductId) {
            isDuplicate = true;
            return false;
        }
    });

    if (!isDuplicate) {
        count++;
        $("#displayPurchaseRequest").append(Result);
        updateRowNumbers();
    } else {
        Swal.fire({
            title: "Product already added!",
            text: "The selected product is already added.",
            icon: "warning",
            confirmButtonColor: "#3085d6",
            confirmButtonText: "OK"
        });
    }
    updateTotals();
    showHidebtn();
}

function updateRowNumbers() {

    $(".product-id").each(function (index) {
        $(this).text(index + 1);
    });
}

function preventEmptyValue(input) {

    if (input.value === "") {

        input.value = 1;
    }
}

$(document).ready(function () {
    $(document).on('input', '.product-quantity', function () {
        updateTotals()
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

function updateTotals() {
    $(".products").each(function () {
        var row = $(this);
        var subtotal = parseFloat(row.find("#dspperunitprice").text().replace('₹', ''));
        var totalquantity = parseFloat(row.find("#txtproductquantity").val());
        var totalAmount = subtotal * totalquantity;
        row.find("#dsptotalAmount").text(totalAmount.toFixed(2));
    });
}

function removeProduct(btn) {
    var cardBody = btn.closest('.card-body');
    if (cardBody) {
        cardBody.remove();
    }
    updateTotals();
    showHidebtn();
}
function showHidebtn() {
    var totalAmount = $("#dsptotalAmount").text();
    if (totalAmount != "") {
        $("#btnpurchaserequest").show();
    } else {
        $("#btnpurchaserequest").hide();
    }
}

function GetPRData() {
    
    $('#PRListTable').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "Post",
            url: '/PurchaseRequest/GetPRList',
            dataType: 'json'
        },
        columns: [
            {
                "data": "prNo", "name": "PRNo",
                "render": function (data, type, full) {

                    return '<h5 class="fs-15"><a href="/PurchaseRequest/PurchaseRequestDetails?prNo=' + full.prNo + '" class="fw-medium link-primary">' + full.prNo; '</a></h5>';
                }
            },
            {
                "render": function (data, type, full) {
                    return full.fullName + ' ( ' + full.userName + ')';
                },

            },
            { "data": "projectName", "name": "ProjectName" },
            { "data": "productName", "name": "ProductName" },
            { "data": "quantity", "name": "Quantity" },
            {
                "data": "isApproved", "name": "IsApproved",
                "render": function (data, type, full) {
                    return '<input type="checkbox" ' + (full.isApproved ? 'checked' : '') + ' onclick="ApproveUnapprovePR(this,\'' + full.prNo + '\')">';
                }
            },
            {
                "render": function (data, type, full) {
                    var canEdit = userPermissions.some(function (permission) {
                        return permission.FormName == "Purchase Request List" && permission.Edit == true;
                    });
                    var canDelete = userPermissions.some(function (permission) {
                        return permission.FormName == "Purchase Request List" && permission.Delete == true;
                    });
                    var buttons = '<ul class="list-inline hstack gap-2 mb-0">';

                    if (canEdit) {
                        buttons += '<li class="btn list-inline-item edit" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="Edit">' +
                            '<a onclick="EditPurchaseRequestDetails(\'' + full.prId + '\')">' +
                            '<i class="ri-pencil-fill fs-16"></i></a></li>';
                    }

                    if (canDelete) {
                        buttons += '<li class="btn text-danger list-inline-item delete" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="Delete" style="margin-left:12px;">' +
                            '<a onclick="DeletePurchaseRequest(\'' + full.prNo + '\')">' +
                            '<i class="fas fa-trash"></i></a></li>';
                    }

                    buttons += '</ul>';
                    return buttons;
                   /* return ('<li class="btn list-inline-item edit" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="Edit"><a onclick="EditPurchaseRequestDetails(\'' + full.prId + '\')"><i class="ri-pencil-fill fs-16"></i></a></li><li class="btn text-danger list-inline-item delete" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="Delete" style="margin-left:12px;"><a onclick="DeletePurchaseRequest(\'' + full.prNo + '\')"><i class="fas fa-trash"></i></a></li>');*/
                }
            },
        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all",
        }]
    });
}

function ApproveUnapprovePR(checkbox,PrNo) {
    
    var isChecked = $(checkbox).is(':checked');
    var confirmationMessage = isChecked ? "Are you sure you want to approve this purchase request?" : "Are you sure you want to unapprove this purchase request?";

    Swal.fire({
        title: confirmationMessage,
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
            var formData = new FormData();
            formData.append("PrNo", PrNo);

            $.ajax({
                url: '/PurchaseRequest/ApproveUnapprovePR?PrNo=' + PrNo,
                type: 'Post',
                contentType: 'application/json;charset=utf-8;',
                dataType: 'json',
                success: function (Result) {
                    if (Result.code == 200) {
                        Swal.fire({
                            title: isChecked ? "Approve!" : "Unapprove!",
                            text: Result.message,
                            icon: "success",
                            confirmButtonClass: "btn btn-primary w-xs mt-2",
                            buttonsStyling: false
                        }).then(function () {
                            GetPRData();
                        });
                    } else {
                        GetPRData();
                    }

                }
            });
        } else if (result.dismiss === Swal.DismissReason.cancel) {

            Swal.fire(
                'Cancelled',
                'User have no changes.!!😊',
                'error'
            ).then(function () {
                GetPRData();
            });;
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
        Error: function () {
            Swal.fire({
                title: "Can't get data!",
                icon: 'warning',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK',
            })
        }
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
                        window.location = '/PurchaseRequest/PurchaseRequestList';
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
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
                Swal.fire({
                    title: "Error",
                    text: "An error occurred while creating the purchase request.",
                    icon: "error",
                    confirmButtonColor: "#3085d6",
                    confirmButtonText: "OK"
                });
            }
        });
    } else {
        Swal.fire({
            title: "Kindly add products!",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        })
    }
}




function EditPurchaseRequestDetails(PrId) {
    $.ajax({
        url: '/PurchaseRequest/EditPurchaseRequestDetails?PrId=' + PrId,
        type: "Get",
        contentType: 'application/json;charset=utf-8;',
        dataType: 'json',
        success: function (response) {
            $('#UpdateOrderModel').modal('show');
            $('#txtPrId').val(response.prId);
            $('#txtusername').val(response.userId);
            $('#txtprojectId').val(response.projectId);
            $('#txtProductId').val(response.productId);
            $('#txtproductName').val(response.productName);
            $('#txtProductTypeId').val(response.productTypeId);
            $('#txtquantity').val(response.quantity);
            $('#txtIsApproved').val(response.isApproved);
        },
        error: function () {
            alert('Data not found');
        }
    });
}

function UpdatePurchaseRequestDetails() {
    //if ($('#UpdatePurchaseRequestDetailsForm').valid())
    //{
    var objData = {
        UserName: $('#txtusername').val(),
        ProjectName: $('#txtprojectId').val(),
        ProductName: $('#txtproductName').val(),
        Quantity: $('#txtquantity').val(),
    }

    $.ajax({
        url: '/PurchaseRequest/UpdatePurchaseRequestDetails',
        type: 'Post',
        data: objData,
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
                    window.location = '/PurchaseRequest/PurchaseRequestList';
                });
            }
        }
    })
    //}
    //else {
    //    Swal.fire({
    //        title: "Kindly Fill All Details",
    //        icon: 'warning',
    //        confirmButtonColor: '#3085d6',
    //        confirmButtonText: 'OK',
    //    })
    //}
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
                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        GetPRData();
                    })
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
//function validateAndCreatePurchaseRequest() {
//    PRForm = $("#UpdatePurchaseRequestDetailsForm").validate({
//        rules: {
//            txtusername: "required",
//            txtprojectId: "required",
//            txtProductId: "required",
//            txtproductName: "required",
//            txtProductTypeId: "required",
//        },
//        messages: {
//            txtusername: "Please Enter UserNAme",
//            txtprojectId: "Please Enter ProjectId",
//            txtProductId: "Please Enter ProductId",
//            txtproductName: "Please Enter ProductName",
//            txtProductTypeId: "Please Enter ProductTypeId",

//        }
//    })
//    var isValid = true;

//    if (isValid) {
//        if ($("#txtPrId").val() == '') {
//            CreatePurchaseRequest();
//        }
//        else {
//            UpdatePurchaseRequestDetails()
//        }
//    }
//}


