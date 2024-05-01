GetAllItemDetailsList();
updateTotals();
GetPurchaseRequestList();
function GetAllItemDetailsList() {
    var searchText = $('#mdProductSearch').val();

    $.get("/PurchaseRequest/GetAllProductDetailsList", { searchText: searchText })
        .done(function (result) {
            $("#mdlistofItem").html(result);
        })
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
        url: '/ProductMaster/DisplayProductDetils?ProductId=' + Id,
        type: 'Post',
        datatype: 'json',
        complete: function (Result) {
            $("#displayPurchaseRequest").append(Result.responseText);
        }
    });
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

    $(".product").each(function () {
        var row = $(this);
        var subtotal = parseFloat(row.find("#dspperunitprice").text().replace('₹', ''));
        var totalquantity = parseFloat(row.find("#txtproductquantity").val());
        var totalAmount = subtotal * totalquantity;

        row.find("#dsptotalAmount").text(totalAmount.toFixed(2));
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
    /*if ($("#UpdatePurchaseRequestDetailsForm").valid()) {*/

        var objData = {
            UserId: $('#txtuserId').val(),
            ProjectId: $('#txtprojectId').val(),
            ProductId: $('#txtproductId').val(),
            ProductName: $('#txtProductName').text(),
            ProductTypeId: $('#txtproducttype').val(),
            Quantity: $('#txtproductquantity').val(),
            CreatedBy: $('#txtuserId').val(),
        }
        $.ajax({
            url: '/PurchaseRequest/CreatePurchaseRequest',
            type: 'post',
            data: objData,
            datatype: 'json',
            success: function (Result) {

                Swal.fire({
                    title: Result.message,
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK'
                }).then(function () {
                    window.location = '/PurchaseRequest/PurchaseRequestList';
                });
            },

        })
    //}
    //else {
    //    Swal.fire({
    //        title: "Kindly Fill All Datafield",
    //        icon: 'warning',
    //        confirmButtonColor: '#3085d6',
    //        confirmButtonText: 'OK',
    //    })
    //}
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
    debugger
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
            success: function (Result) {debugger
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
function DeletePurchaseRequestDetails(PrId) {debugger

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
                url: '/PurchaseRequest/DeletePurchaseRequestDetails?PrId=' + PrId,
                type: 'POST',
                dataType: 'json',
                success: function (Result) {debugger

                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/PurchaseRequest/PurchaseRequestList';
                    })
                },
                error: function () {
                    Swal.fire({
                        title: "Can't Delete Order!",
                        icon: 'warning',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/PurchaseRequest/PurchaseRequestList';
                    })
                }
            })
        } else if (result.dismiss === Swal.DismissReason.cancel) {

            Swal.fire(
                'Cancelled',
                'Order Have No Changes.!!😊',
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