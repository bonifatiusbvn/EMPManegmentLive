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
    debugger

    $.ajax({
        url: '/ProductMaster/DisplayProductDetils?ProductId=' + Id,
        type: 'Post',
        datatype: 'json',
        complete: function (Result) {
            debugger
            $("#displayProductDetail").append(Result.responseText);
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

function CreatePurchaseRequestDetails() {

    if ($("#UpdatePurchaseRequestDetailsForm").valid()) {

        var objData = {
            UserName: $('#txtusername').val(),
            ProjectName: $('#txtprojectId').val(),
            ProductName: $('#txtproductName').val(),
            Quantity: $('#txtquantity').val(),
        }
        $.ajax({
            url: '/PurchaseRequest/AddPurchaseRequestDetails',
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
                    window.location = '/PurchaseRequest/UpdateOrderModel';
                });
            },

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
    function GetPurchaseRequestDetailsById(PrId) {

        $.ajax({
            url: '/PurchaseRequest/GetPurchaseRequestDetailsById?PrId=' + PrId,
            type: 'GET',
            contentType: 'application/json;charset=utf-8',
            dataType: 'json',
            success: function (response) {

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