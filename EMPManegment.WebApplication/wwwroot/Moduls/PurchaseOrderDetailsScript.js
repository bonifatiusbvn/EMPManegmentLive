$(document).on("click", ".plus", function () {

    updateProductQuantity($(this).closest(".product"), 1);
    return
});

$(document).on("click", ".minus", function () {

    updateProductQuantity($(this).closest(".product"), -1);
    return
});
function preventEmptyValue(input) {

    if (input.value === "") {

        input.value = 1;
    }
}
$(document).ready(function () {
    $(document).on('input', '.product-quantity', function () {
        var row = $(this).closest('.product');
        updateProductTotalAmount(row);
        updateTotals();
    });


    $(document).on('keydown', '.product-quantity', function (event) {
        if (event.key === 'Enter') {
            $(this).blur();
        }
    });


    $(document).on('input', '#txtproductamount', function () {
        var row = $(this).closest('.product');
        updateProductTotalAmount(row);
        updateTotals();
    });


    $(document).on('keydown', '#txtproductamount', function (event) {
        if (event.key === 'Enter') {
            $(this).blur();
        }
    });

    $(document).on('input', '#txtgst', function () {
        var row = $(this).closest('.product');
        updateProductTotalAmount(row);
        updateTotals();
    });

    $(document).on('keydown', '#txtgst', function (event) {
        if (event.key === 'Enter') {
            $(this).blur();
        }
    }); 

    $(document).on('focusout', '.product-quantity', function () {
        $(this).trigger('input');
    });
});

GetPaymentTypeList();
GetVendorNameList();
GetCompanyNameList();
GetProductDetailsList();
GetProducts();
GetPaymentMethodList();
function SearchData() {
    if ($('#statusform').valid()) {
        if ($("#idStatus").val() != "All") {
            $("#status-delivered").hide();
            $("#status-cancelled").hide();
            $("#status-pickups").hide();
            $("#status-returns").hide();
            $("#status-inprogress").hide();
            $("#status-pending").hide();
            $("#project-overview").hide();
            if ($("#idStatus").val() == "Pickups") {
                $("#deliveredactive").removeClass('active');
                $('#pendingactive').removeClass('active');
                $('#cancelledactive').removeClass('active');
                $('#inprogressactive').removeClass('active');
                $('#returnsactive').removeClass('active');
                $('#allordersactive').removeClass('active');
                $('#pickupactive').addClass('active');
                var DeliveryStatus = "Pickups";
            }
            else {
                if ($("#idStatus").val() == "Delivered") {
                    $('#allordersactive').removeClass('active');
                    $("#deliveredactive").addClass('active');
                    $('#pendingactive').removeClass('active');
                    $('#cancelledactive').removeClass('active');
                    $('#inprogressactive').removeClass('active');
                    $('#returnsactive').removeClass('active');
                    $('#pickupactive').removeClass('active');
                    var DeliveryStatus = "Delivered";
                }
                else {
                    if ($("#idStatus").val() == "Returns") {
                        $('#allordersactive').removeClass('active');
                        $("#deliveredactive").removeClass('active');
                        $('#pendingactive').removeClass('active');
                        $('#cancelledactive').removeClass('active');
                        $('#inprogressactive').removeClass('active');
                        $('#returnsactive').addClass('active');
                        $('#pickupactive').removeClass('active');
                        var DeliveryStatus = "Returns";
                    }
                    else {
                        if ($("#idStatus").val() == "Cancelled") {
                            $('#allordersactive').removeClass('active');
                            $("#deliveredactive").removeClass('active');
                            $('#pendingactive').removeClass('active');
                            $('#cancelledactive').addClass('active');
                            $('#inprogressactive').removeClass('active');
                            $('#returnsactive').removeClass('active');
                            $('#pickupactive').removeClass('active');
                            var DeliveryStatus = "Cancelled";
                        }
                        else {
                            if ($("#idStatus").val() == "Pending") {
                                $('#allordersactive').removeClass('active');
                                $("#deliveredactive").removeClass('active');
                                $('#pendingactive').addClass('active');
                                $('#cancelledactive').removeClass('active');
                                $('#inprogressactive').removeClass('active');
                                $('#returnsactive').removeClass('active');
                                $('#pickupactive').removeClass('active');
                                $('#pickupactive').removeClass('active');
                                var DeliveryStatus = "Pending";
                            }
                            else {
                                if ($("#idStatus").val() == "Inprogress") {
                                    $('#allordersactive').removeClass('active');
                                    $("#deliveredactive").removeClass('active');
                                    $('#pendingactive').removeClass('active');
                                    $('#cancelledactive').removeClass('active');
                                    $('#inprogressactive').addClass('active');
                                    $('#returnsactive').removeClass('active');
                                    $('#pickupactive').removeClass('active');
                                    var DeliveryStatus = "Inprogress";
                                }
                            }
                        }
                    }
                }
            }
            var formData = new FormData();
            formData.append("DeliveryStatus", DeliveryStatus);

            $.ajax({
                url: '/PurchaseOrderMaster/GetPurchaseOrderDetailsByStatus',
                type: 'Post',
                dataType: 'json',
                data: formData,
                processData: false,
                contentType: false,
                complete: function (Result) {
                    $("#dvdeliveredstatus").show();
                    $("#dvdeliveredstatus").html(Result.responseText);
                }
            });
        }
        else {
            window.location = '/OrderMaster/CreateOrder';
        }
    }
    else {
        Swal.fire({
            title: "Kindly Fill the Status",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        })
    }
}

$(document).ready(function () {
    $('#txtvendorname').change(function () {
        var Text = $("#txtvendorname Option:Selected").text();
        var ProductId = $(this).val();
        $("#txtvendornameid").val(Text);
        $('#productname').empty();
        $('#productname').append('<Option >--Select Product--</Option>');
        $.ajax({
            url: '/ProductMaster/GetProductById?ProductId=' + ProductId,
            success: function (result) {

                $.each(result, function (i, data) {
                    $('#productname').append('<Option value=' + data.id + '>' + data.productType + '</Option>')
                });
            }
        });
    });
});

function SaveCreatePurchaseOrder() {

    if ($('#createOrderForm').valid()) {

        var formData = new FormData();
        formData.append("Type", $("#OrderType").val());
        formData.append("OrderId", $("#POId").val());
        formData.append("VendorId", $("#txtvendorname").val());
        formData.append("CompanyName", $("#txtvendorname").val());
        formData.append("Product", $("#productname").val());
        formData.append("Quantity", $("#productquantity").val());
        formData.append("Amount", $("#amount").val());
        formData.append("Total", $("#totalamount").val());
        formData.append("OrderDate", $("#orderdate").val());
        formData.append("DeliveryDate", $("#deliverydate").val());
        formData.append("paymenttype", $("#payment").val());
        formData.append("DeliveryStatus", $("#deliveredstatus").val());
        formData.append("CreatedBy", $("#txtuserid").val());
        $.ajax({
            url: '/PurchaseOrderMaster/CreatePurchaseOrder',
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
                        window.location = '/PurchaseOrderMaster/CreatePurchaseOrder';
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

$(document).ready(function () {
    $("#createOrderForm").validate({
        rules: {
            orderId: "required",
            companyname: "required",
            productname: "required",
            productquantity: "required",
            amount: "required",
            totalamount: "required",
            orderdate: "required",
            deliverydate: "required",
            payment: "required",
            deliveredstatus: "required",
        },
        messages: {
            orderId: "Please Enter Order Id",
            companyname: "Please Enter Compaany Name",
            productname: "Please Select Product",
            productquantity: "Please Enter Product Quantity",
            amount: "Please Enter Amount",
            totalamount: "Please Enter Total Amount",
            orderdate: "Please Enter Order Date",
            deliverydate: "Please Enter Delivery Date",
            payment: "Please Enter Payment Method",
            deliveredstatus: "Please Enter Delivered Status",
        }
    })
    $('#createorder').on('click', function () {
        $("#createOrderForm").validate();
    });


    $("#statusform").validate({
        rules: {
            idStatus: "required"
        },
        messages: {
            idStatus: "Please Enter Delivered Status"
        }
    })
    $('#statussearch').on('click', function () {
        $("#statusform").validate();
    });
});

$("#deliveredactive").click(function () {
    $("#status-delivered").show();
    $("#dvdeliveredstatus").hide();
});
$("#allordersactive").click(function () {
    $("#project-overview").show();
    $("#dvdeliveredstatus").hide();
});
$("#pickupactive").click(function () {
    $("#status-pickups").show();
    $("#dvdeliveredstatus").hide();
});
$("#cancelledactive").click(function () {
    $("#status-cancelled").show();
    $("#dvdeliveredstatus").hide();
});
$("#inprogressactive").click(function () {
    $("#status-inprogress").show();
    $("#dvdeliveredstatus").hide();
});
$("#pendingactive").click(function () {
    $("#status-pending").show();
    $("#dvdeliveredstatus").hide();
});
$("#returnsactive").click(function () {
    $("#status-returns").show();
    $("#dvdeliveredstatus").hide();
});


$(document).ready(function () {
    $("#CreatePOForm").validate({

        rules: {
            textVendorName: "required",
            textCompanyName: "required",
            textPaymentMethod: "required",
            textDescription: "required",
            textDeliveryStatus: "required",
        },
        messages: {
            textVendorName: "Select Vendor Name",
            textCompanyName: "Select Company Name",
            textPaymentMethod: "Select Payment Method",
            textDescription: "Please Enter Description",
            textDeliveryStatus: "Please Enter Delivery Status",
        }
    });
});

function showPaymentDetails() {
    $("#PaymentDetails").modal("show")
}

function GetProductDetailsList() {
    var searchText = $('#mdProductSearch').val();

    $.get("/PurchaseOrderMaster/GetAllProductList", { searchText: searchText })
        .done(function (result) {
            $("#mdlistofItem").html(result);
        })
        .fail(function (xhr, status, error) {
            console.error("Error:", error);
        });
}

$(document).ready(function () {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();

    today = yyyy + '-' + mm + '-' + dd;
    $("#textOrderDate").val(today);
    $("#textOrderDate").prop("disabled", true);
});

function GetVendorNameList() {
    $.ajax({
        url: '/ProductMaster/GetVendorsNameList',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#textVendorName').append('<Option value=' + data.id + '>' + data.vendorCompany + '</Option>')
            });
        }
    });
}

$(document).ready(function () {
    $('#textVendorName').change(function () {
        getVendorDetail($(this).val());
    });
});


function getVendorDetail(VendorId) {
    $.ajax({
        url: '/Vendor/GetVendorDetailsById?vendorId=' + VendorId,
        type: 'GET',
        contentType: 'application/json;charset=utf-8',
        dataType: 'json',
        success: function (response) {       
            if (response) {
                $('#textVendorMobile').val(response.vendorPhone);
                $('#textVendorGSTNumber').val(response.vendorGstnumber);
                $('#textVendorAddress').val(response.vendorAddress);
            } else {
                console.log('Empty response received.');
            }
        },
    });
}

function selectvendorId() {
    document.getElementById("txtvendorTypeid").value = document.getElementById("txtvendorname").value;
}

function GetCompanyNameList() {
    $.ajax({
        url: '/Company/GetCompanyNameList',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#textCompanyName').append('<Option value=' + data.id + '>' + data.compnyName + '</Option>')
            });
        }
    });
}

$(document).ready(function () {
    $('#textCompanyName').change(function () {
        getCompanyDetail($(this).val());
    });
});


function getCompanyDetail(CompanyId) {
    $.ajax({
        url: '/Company/GetCompanyDetailsById',
        type: 'GET',
        contentType: 'application/json;charset=utf-8',
        dataType: 'json',
        data: { CompanyId: CompanyId },
        success: function (response) {
            if (response) {
                $('#textCompanyGstNo').val(response.gst);
                $('#textCompanyBillingAddress').val(response.fullAddress);
            } else {
                console.log('Empty response received.');
            }
        },
    });
}

function GetProducts() {
    $.ajax({
        url: '/ProductMaster/GetProduct',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#txtProducts').append('<Option value=' + data.id + '>' + data.productName + '</Option>')
            });
        }
    });
}
function GetPaymentTypeList() {
    $.ajax({
        url: '/ExpenseMaster/GetPaymentTypeList',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#textPaymentMethod').append('<Option value=' + data.id + '>' + data.type + '</Option>')
            });
        }
    });
}
function selectProductTypeId() {
    document.getElementById("txtProductTypeid").value = document.getElementById("txtProducts").value;
}
function GetPaymentMethodList() {

    $.ajax({
        url: '/PurchaseOrderMaster/GetPaymentMethodList',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#txtpaymentmethod').append('<Option value=' + data.id + '>' + data.paymentMethod + '</Option>')
            });
        }
    });
}

function ProductTypeDropdown(productId) {
    $.ajax({
        url: '/ProductMaster/GetProduct',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#txtPOProductType_' + productId).append('<option value=' + data.id + '>' + data.productName + '</option>');
            });
        }
    });
}


$(document).ready(function () {
    $('#txtProducts').change(function () {


        var Text = $("#txtProducts Option:Selected").text();
        var ProductTypeId = $(this).val();
        var VendorTypeId = $("#txtvendorname").val();
        var Productid = $("#txtProductid").val(ProductTypeId);

        $("#txtProductTypeid").val(Text);
        $('#searchproductname').empty();
        $('#searchproductname').append('<Option >--Select ProductName--</Option>');
        $.ajax({
            url: '/ProductMaster/SerchProductByVendor?ProductId=' + ProductTypeId + '&VendorId=' + VendorTypeId,
            type: 'Post',
            success: function (result) {
                $.each(result, function (i, data) {
                    $('#searchproductname').append('<Option value=' + data.id + '>' + data.productName + '</Option>');
                    $('#txtvendorname').prop('disabled', true);
                    $('#txtProducts').prop('disabled', true);
                });
            }
        });
    });
});

function searchProductTypeId() {
    document.getElementById("searchproductnameid").value = document.getElementById("searchproductname").value;
}

function SearchProductDetailsById(ProductId) {
    var GetProductId = {
        Id: ProductId,
    }
    var form_data = new FormData();
    form_data.append("ProductId", JSON.stringify(GetProductId));


    $.ajax({
        url: '/ProductMaster/DisplayProductDetailsListById',
        type: 'Post',
        datatype: 'json',
        data: form_data,
        processData: false,
        contentType: false,
        complete: function (Result) {
        
            if (Result.statusText === "success") {
                AddNewRow(Result.responseText);
            }
            else {
                var GetProductId = $('#searchProductname').val();
                if (GetProductId === "Select ProductName" || GetProductId === null) {
                    $('#searchvalidationMessage').text('Please select ProductName!!');
                }
                else {
                    $('#searchvalidationMessage').text('');
                }
            }
        }
    });
}

function deletePurchaseOrderDetails(OrderId) {

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
                url: '/PurchaseOrderMaster/DeletePurchaseOrderDetails?OrderId=' + OrderId,
                type: 'POST',
                dataType: 'json',
                success: function (Result) {

                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/PurchaseOrderMaster/CreatePurchaseOrder';
                    })
                },
                error: function () {
                    Swal.fire({
                        title: "Can't Delete Order!",
                        icon: 'warning',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/PurchaseOrderMaster/CreatePurchaseOrder';
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
function EditPurchaseOrderDetails(Id) {
    $.ajax({
        url: '/PurchaseOrderMaster/EditPurchaseOrderDetails?Id=' + Id,
        type: "Get",
        contentType: 'application/json;charset=utf-8;',
        dataType: 'json',
        success: function (response) {
            $('#UpdateOrderModel').modal('show');
            $('#POId').val(response.id);
            $('#txtproduct').val(response.product);
            $('#txtorderno').html(response.orderId);
            $('#txtorderdate').val(response.orderDate);
            $('#txtcompanyname').val(response.companyName);
            $('#txtorderdetails').val(response.productName);
            $('#txtamount').val(response.totalAmount);
            $('#txtpaymentmethod').val(response.paymentMethod);
            $('#txtdeliverystatus').val(response.deliveryStatus);
            $('#txtorderstatus').val(response.orderStatus);
        },
        error: function () {
            alert('Data not found');
        }
    });
}

function UpdatePurchaseOrderDetails() {
    if ($('#UpdateOrderDetailsForm').valid()) {
        var formData = new FormData();
        formData.append("Id", $("#POId").val());
        formData.append("Product", $("#txtproduct").val());
        formData.append("orderId", $("#txtorderno").val());
        formData.append("orderDate", $("#txtorderdate").val());
        formData.append("companyName", $("#txtcompanyname").val());
        formData.append("productName", $("#txtorderdetails").val());
        formData.append("totalAmount", $("#txtamount").val());
        formData.append("paymentMethod", $("#txtpaymentmethod").val());
        formData.append("deliveryStatus", $("#txtdeliverystatus").val());
        formData.append("orderStatus", $("#txtorderstatus").val());

        $.ajax({
            url: '/PurchaseOrderMaster/UpdatePurchaseOrderDetails',
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
                        window.location = '/PurchaseOrderMaster/CreatePurchaseOrder';
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

    $("#UpdateOrderDetailsForm").validate({
        rules: {
            txtorderdate: "required",
            txtcompanyname: "required",
            txtorderdetails: "required",
            txtamount: "required",
            txtpaymentmethod: "required",
            txtdeliverystatus: "required",
            txtorderstatus: "required"
        },
        messages: {
            txtorderdate: "Please emter order date",
            txtcompanyname: "Please enter company name",
            txtorderdetails: "Please enter order details",
            txtamount: "Please enter order Amount",
            txtpaymentmethod: "Please Enter Payment method",
            txtdeliverystatus: "Please Enter Delivery status",
            txtorderstatus: "Plese enter orderstatus"
        }
    })
    $("#updatedetailbtn").on('click', function () {
        $("#UpdateOrderDetailsForm").validate();
    });
});

function selectProductTypeId() {
    document.getElementById("txtProductTypeid").value = document.getElementById("txtProducts").value;
}
$("#txtProducts").change(function () {
    ProductDetailsByProductTypeId()
})
function ProductDetailsByProductTypeId() {
    var form_data = new FormData();
    form_data.append("ProductId", $('#txtProducts').val());
    $.ajax({
        url: '/ProductMaster/GetProductById',
        type: 'Post',
        datatype: 'json',
        data: form_data,
        processData: false,
        contentType: false,
        complete: function (Result) {
            $("#table-product-list-all").hide();
            $("#ProductdetailsPartial").html(Result.responseText);
        }
    });
}

var paymentSign = "$";

function otherPayment() {
    var e = document.getElementById("choices-payment-currency").value;
    paymentSign = e, Array.from(document.getElementsByClassName("product-line-price")).forEach(function (e) {
        isUpdate = e.value.slice(1), e.value = paymentSign + isUpdate
    }), recalculateCart()
}
Array.from(document.getElementsByClassName("product-line-price")).forEach(function (e) {
    e.value = paymentSign + "0.00"
});
//var isPaymentEl = document.getElementById("choices-payment-currency"),
//    choices = new Choices(isPaymentEl, {
//        searchEnabled: !1
//    });

function isData() {
    var e = document.getElementsByClassName("plus"),
        t = document.getElementsByClassName("minus");
    e && Array.from(e).forEach(function (n) {
        n.onclick = function (e) {
            var t;
            parseInt(n.previousElementSibling.value) < 10 && (e.target.previousElementSibling.value++, e = n.parentElement.parentElement.previousElementSibling.querySelector(".product-price").value, t = n.parentElement.parentElement.nextElementSibling.querySelector(".product-line-price"), updateQuantity(n.parentElement.querySelector(".product-quantity").value, e, t))
        }
    }), t && Array.from(t).forEach(function (n) {
        n.onclick = function (e) {
            var t;
            1 < parseInt(n.nextElementSibling.value) && (e.target.nextElementSibling.value--, e = n.parentElement.parentElement.previousElementSibling.querySelector(".product-price").value, t = n.parentElement.parentElement.nextElementSibling.querySelector(".product-line-price"), updateQuantity(n.parentElement.querySelector(".product-quantity").value, e, t))
        }
    })
}

document.querySelector("#profile-img-file-input").addEventListener("change", function () {
    var e = document.querySelector(".user-profile-image"),
        t = document.querySelector(".profile-img-file-input").files[0],
        n = new FileReader;
    n.addEventListener("load", function () {
        e.src = n.result
    }, !1), t && n.readAsDataURL(t)
}), isData();


var count = 0;
function AddNewRow(Result) {
    
    var newProductRow = $(Result);
    var productId = newProductRow.data('product-id');
    ProductTypeDropdown(productId);
    var newProductId = newProductRow.attr('data-product-id');
    var isDuplicate = false;

    $('#addnewproductlink .product').each(function () {
        var existingProductRow = $(this);
        var existingProductId = existingProductRow.attr('data-product-id');
        if (existingProductId === newProductId) {
            isDuplicate = true;
            return false;
        }
    });

    if (!isDuplicate) {
        count++;
        $("#addnewproductlink").append(Result);
        updateProductTotalAmount();
        updateTotals();
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
}



function updateRowNumbers() {
    $(".product-id").each(function (index) {
        $(this).text(index + 1);
    });
}
function bindEventListeners() {
    document.querySelectorAll(".product-removal a").forEach(function (e) {
        e.addEventListener("click", function (event) {
            removeItem(event.target.closest("tr"));
            updateTotals();
        });
    });


    document.querySelectorAll(".plus").forEach(function (btn) {
        btn.addEventListener("click", function (event) {
            updateProductQuantity(event.target.closest("tr"), 1);
            updateTotals();
        });
    });


    document.querySelectorAll(".minus").forEach(function (btn) {
        btn.addEventListener("click", function (event) {
            updateProductQuantity(event.target.closest("tr"), -1);
            updateTotals();
        });
    });

    
}

function updateProductTotalAmount() {
    debugger
    $(".product").each(function () {
        var row = $(this);
        var productPrice = parseFloat(row.find("#txtproductamount").val());
        var quantity = parseInt(row.find("#txtproductquantity").val());
        var gst = parseFloat(row.find("#txtgst").val());
        var totalGst = (productPrice * quantity * gst) / 100;
        var totalAmount = productPrice * quantity + totalGst;

        row.find("#txtgstAmount").val(totalGst.toFixed(2));
        row.find("#txtproducttotalamount").val(totalAmount.toFixed(2));
    });
}



function updateProductQuantity(row, increment) {
    var quantityInput = parseInt(row.find(".product-quantity").val());
    var newQuantity = quantityInput + increment;
    if (newQuantity >= 0) {
        row.find(".product-quantity").val(newQuantity);
        updateProductTotalAmount(row);
        updateTotals();
    }
}


function updateTotals() {
    debugger
    var totalSubtotal = 0;
    var totalGst = 0;
    var totalAmount = 0;
    var TotalItemQuantity = 0;

    $(".product").each(function () {
        debugger
        var row = $(this);
        var subtotal = parseFloat(row.find("#txtproductamount").val());
        var gst = parseFloat(row.find("#txtgstAmount").val());
        var totalquantity = parseFloat(row.find("#txtproductquantity").val());

        totalSubtotal += subtotal * totalquantity;
        totalGst += gst;
        totalAmount = totalSubtotal + totalGst;
        TotalItemQuantity += totalquantity;
    });

    $("#cart-subtotal").val(totalSubtotal.toFixed(2));
    debugger
    $("#totalgst").val(totalGst.toFixed(2));
    $("#cart-total").val(totalAmount.toFixed(2));
    $("#TotalProductQuantity").text(TotalItemQuantity);
    $("#TotalProductPrice").html(totalSubtotal);
    $("#TotalProductGST").html(totalGst.toFixed(2));
    $("#TotalProductAmount").html(totalAmount.toFixed(2));
}
function removeItem(btn) {
    $(btn).closest("tr").remove();
    updateRowNumbers();
    updateTotals();
}


var taxRate = .125,
    shippingRate = 65,
    discountRate = .15,
    gst = 18;

function recalculateCart() {
    var t = 0,
        e = (Array.from(document.getElementsByClassName("product")).forEach(function (e) {
            Array.from(e.getElementsByClassName("product-line-price")).forEach(function (e) {
                e.value && (t += parseFloat(e.value.slice(1)))
            })
        }), t * taxRate),
        n = t * discountRate,
        o = 0 < t ? shippingRate : 0,
        a = t + e + o - n,
        b = t * 18 / 100;
    p = t
    document.getElementById("cart-subtotal").value = t.toFixed(2), document.getElementById("cart-tax").value = paymentSign + e.toFixed(2), document.getElementById("totalgst").value = b.toFixed(2), document.getElementById("cart-shipping").value = paymentSign + o.toFixed(2), document.getElementById("cart-total").value = paymentSign + a.toFixed(2), document.getElementById("cart-discount").value = paymentSign + n.toFixed(2), document.getElementById("totalamountInput").value = paymentSign + a.toFixed(2), document.getElementById("amountTotalPay").value = paymentSign + a.toFixed(2)
}

function amountKeyup() {
    Array.from(document.getElementsByClassName("product-price")).forEach(function (n) {
        n.addEventListener("keyup", function (e) {
            var t = n.parentElement.nextElementSibling.nextElementSibling.querySelector(".product-line-price");
            updateQuantity(e.target.value, n.parentElement.nextElementSibling.querySelector(".product-quantity").value, t)
        })
    })
}

function updateQuantity(e, t, n) {
    e = (e = e * t).toFixed(2);
    n.value = paymentSign + e, recalculateCart()
}


amountKeyup();
var genericExamples = document.querySelectorAll("[data-trigger]");

function billingFunction() {
    document.getElementById("same").checked ? (document.getElementById("shippingName").value = document.getElementById("billingName").value, document.getElementById("shippingAddress").value = document.getElementById("billingAddress").value, document.getElementById("shippingPhoneno").value = document.getElementById("billingPhoneno").value, document.getElementById("shippingTaxno").value = document.getElementById("billingTaxno").value) : (document.getElementById("shippingName").value = "", document.getElementById("shippingAddress").value = "", document.getElementById("shippingPhoneno").value = "", document.getElementById("shippingTaxno").value = "")
}
Array.from(genericExamples).forEach(function (e) {
    new Choices(e, {
        placeholderValue: "This is a placeholder set in the config",
        searchPlaceholderValue: "This is a search placeholder"
    })
});

Array.from(genericExamples).forEach(function (e) {
    new Cleave(e, {
        delimiters: ["(", ")", "-"],
        blocks: [0, 3, 3, 4]
    })
});
let viewobj;
var value, invoices_list = localStorage.getItem("invoices-list"),
    options = localStorage.getItem("option"),
    invoice_no = localStorage.getItem("invoice_no"),
    invoices = JSON.parse(invoices_list);
if (null === localStorage.getItem("invoice_no") && null === localStorage.getItem("option") ? (viewobj = "", value = "#VL" + Math.floor(11111111 + 99999999 * Math.random()), document.getElementById("invoicenoInput").value = value) : viewobj = invoices.find(e => e.invoice_no === invoice_no), "" != viewobj && "edit-invoice" == options) {
    document.getElementById("registrationNumber").value = viewobj.company_details.legal_registration_no, document.getElementById("companyEmail").value = viewobj.company_details.email, document.getElementById("companyWebsite").value = viewobj.company_details.website, new Cleave("#compnayContactno", {
        prefix: viewobj.company_details.contact_no,
        delimiters: ["(", ")", "-"],
        blocks: [0, 3, 3, 4]
    }), document.getElementById("companyAddress").value = viewobj.company_details.address, document.getElementById("companyaddpostalcode").value = viewobj.company_details.zip_code;
    for (var preview = document.querySelectorAll(".user-profile-image"), paroducts_list = ("" !== viewobj.img && (preview.src = viewobj.img), document.getElementById("invoicenoInput").value = "#VAL" + viewobj.invoice_no, document.getElementById("invoicenoInput").setAttribute("readonly", !0), document.getElementById("date-field").value = viewobj.date, document.getElementById("choices-payment-status").value = viewobj.status, document.getElementById("totalamountInput").value = "$" + viewobj.order_summary.total_amount, document.getElementById("billingName").value = viewobj.billing_address.full_name, document.getElementById("billingAddress").value = viewobj.billing_address.address, new Cleave("#billingPhoneno", {
        prefix: viewobj.company_details.contact_no,
        delimiters: ["(", ")", "-"],
        blocks: [0, 3, 3, 4]
    }), document.getElementById("billingTaxno").value = viewobj.billing_address.tax, document.getElementById("shippingName").value = viewobj.shipping_address.full_name, document.getElementById("shippingAddress").value = viewobj.shipping_address.address, new Cleave("#shippingPhoneno", {
        prefix: viewobj.company_details.contact_no,
        delimiters: ["(", ")", "-"],
        blocks: [0, 3, 3, 4]
    }), document.getElementById("shippingTaxno").value = viewobj.billing_address.tax, viewobj.prducts), counter = 1; counter++, 1 < paroducts_list.length && document.getElementById("add-item").click(), paroducts_list.length - 1 >= counter;);
    var counter_1 = 1,
        cleave = (setTimeout(() => {
            Array.from(paroducts_list).forEach(function (e) {
                document.getElementById("productName-" + counter_1).value = e.product_name, document.getElementById("productDetails-" + counter_1).value = e.product_details, document.getElementById("productRate-" + counter_1).value = e.rates, document.getElementById("product-qty-" + counter_1).value = e.quantity, document.getElementById("productPrice-" + counter_1).value = "$" + e.rates * e.quantity, counter_1++
            })
        }, 300), document.getElementById("cart-subtotal").value = viewobj.order_summary.sub_total, document.getElementById("cart-tax").value = viewobj.order_summary.estimated_tex, document.getElementById("cart-discount").value = "$" + viewobj.order_summary.discount, document.getElementById("cart-shipping").value = "$" + viewobj.order_summary.shipping_charge, document.getElementById("cart-total").value = "$" + viewobj.order_summary.total_amount, document.getElementById("choices-payment-type").value = viewobj.payment_details.payment_method, document.getElementById("cardholderName").value = viewobj.payment_details.card_holder_name, new Cleave("#cardNumber", {
            prefix: viewobj.payment_details.card_number,
            delimiter: " ",
            blocks: [4, 4, 4, 4],
            uppercase: !0
        }));
    document.getElementById("amountTotalPay").value = "$" + viewobj.order_summary.total_amount, document.getElementById("exampleFormControlTextarea1").value = viewobj.notes
}
document.addEventListener("DOMContentLoaded", function () {
    var T = document.getElementById("invoice_form");
    document.getElementsByClassName("needs-validation");
    T.addEventListener("submit", function (e) {
        e.preventDefault();
        var t = document.getElementById("invoicenoInput").value.slice(4),
            e = document.getElementById("companyEmail").value,
            n = document.getElementById("date-field").value,
            o = document.getElementById("totalamountInput").value.slice(1),
            a = document.getElementById("choices-payment-status").value,
            l = document.getElementById("billingName").value,
            i = document.getElementById("billingAddress").value,
            c = document.getElementById("billingPhoneno").value.replace(/[^0-9]/g, ""),
            d = document.getElementById("billingTaxno").value,
            r = document.getElementById("shippingName").value,
            u = document.getElementById("shippingAddress").value,
            m = document.getElementById("shippingPhoneno").value.replace(/[^0-9]/g, ""),
            s = document.getElementById("shippingTaxno").value,
            p = document.getElementById("choices-payment-type").value,
            v = document.getElementById("cardholderName").value,
            g = document.getElementById("cardNumber").value.replace(/[^0-9]/g, ""),
            y = document.getElementById("amountTotalPay").value.slice(1),
            E = document.getElementById("registrationNumber").value.replace(/[^0-9]/g, ""),
            b = document.getElementById("companyEmail").value,
            I = document.getElementById("companyWebsite").value,
            h = document.getElementById("compnayContactno").value.replace(/[^0-9]/g, ""),
            _ = document.getElementById("companyAddress").value,
            B = document.getElementById("companyaddpostalcode").value,
            f = document.getElementById("cart-subtotal").value.slice(1),
            x = document.getElementById("cart-tax").value.slice(1),
            w = document.getElementById("cart-discount").value.slice(1),
            S = document.getElementById("cart-shipping").value.slice(1),
            j = document.getElementById("cart-total").value.slice(1),
            q = document.getElementById("exampleFormControlTextarea1").value,
            A = document.getElementsByClassName("product"),
            N = 1,
            C = [];
        Array.from(A).forEach(e => {
            var t = e.querySelector("#txtproductName-" + N).value,
                n = e.querySelector("#txtproductDescription-" + N).value,
                o = parseInt(e.querySelector("#txtproductamount-" + N).value),
                o = parseInt(e.querySelector("#txtdiscountamount-" + N).value),
                p = parseInt(e.querySelector("#txtgst-" + N).value),
                q = parseInt(e.querySelector("#txtproductamountwithGST-" + N).value),
                a = parseInt(e.querySelector("#product-qty-" + N).value),
                e = e.querySelector("#productPrice-" + N).value.split("$"),
                t = {
                    productName: t,
                    productShortDescription: n,
                    perUnitPrice: o,
                    gst: p,
                    perUnitWithGstprice: q,
                    quantity: a,
                    totalAmount: parseInt(e[1])
                };
            C.push(t), N++
        }), !1 === T.checkValidity() ? T.classList.add("was-validated") : ("edit-invoice" == options && invoice_no == t ? (objIndex = invoices.findIndex(e => e.invoice_no == t), invoices[objIndex].invoice_no = t, invoices[objIndex].customer = l, invoices[objIndex].img = "", invoices[objIndex].email = e, invoices[objIndex].date = n, invoices[objIndex].invoice_amount = o, invoices[objIndex].status = a, invoices[objIndex].billing_address = {
            full_name: l,
            address: i,
            phone: c,
            tax: d
        }, invoices[objIndex].shipping_address = {
            full_name: r,
            address: u,
            phone: m,
            tax: s
        }, invoices[objIndex].payment_details = {
            payment_method: p,
            card_holder_name: v,
            card_number: g,
            total_amount: y
        }, invoices[objIndex].company_details = {
            legal_registration_no: E,
            email: b,
            website: I,
            contact_no: h,
            address: _,
            zip_code: B
        }, invoices[objIndex].order_summary = {
            sub_total: f,
            estimated_tex: x,
            discount: w,
            shipping_charge: S,
            total_amount: j
        }, invoices[objIndex].prducts = C, invoices[objIndex].notes = q, localStorage.removeItem("invoices-list"), localStorage.removeItem("option"), localStorage.removeItem("invoice_no"), localStorage.setItem("invoices-list", JSON.stringify(invoices))) : localStorage.setItem("new_data_object", JSON.stringify({
            invoice_no: t,
            customer: l,
            img: "",
            email: e,
            date: n,
            invoice_amount: o,
            status: a,
            billing_address: {
                full_name: l,
                address: i,
                phone: c,
                tax: d
            },
            shipping_address: {
                full_name: r,
                address: u,
                phone: m,
                tax: s
            },
            payment_details: {
                payment_method: p,
                card_holder_name: v,
                card_number: g,
                total_amount: y
            },
            company_details: {
                legal_registration_no: E,
                email: b,
                website: I,
                contact_no: h,
                address: _,
                zip_code: B
            },
            order_summary: {
                sub_total: f,
                estimated_tex: x,
                discount: w,
                shipping_charge: S,
                total_amount: j
            },
            prducts: C,
            notes: q
        })), window.location.href = "apps-invoices-list.html")
    })
});


function fn_OpenShippingModal() {
    $('#textmdAddress').val('');
    $('#textmdQty').val('');
    $('#mdShippingAdd').modal('show');
}

function fn_mdAddAddress() {
    var rowcount = $('#dvShippingAddress .row.ac-invoice-shippingadd').length + 1
    if ($('#textmdAddress').val() != null && $('#textmdAddress').val().trim() != "") {
        var html = `<div class="row ac-invoice-shippingadd">
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                </div>`
        $('#dvShippingAddress').append(html);
    } else {
        tostar.error('Please select address!');
        $('#textmdAddress').focus();
    }

}
function fn_removeShippingAdd(that) {
    $(that).closest('.ac-invoice-shippingadd').remove();
}

function fn_OpenAddproductmodal() {
    
    $('#mdProductSearch').val('');
    $('#mdPoproductModal').modal('show');
}

function toggleShippingAddress() {
    var checkbox = document.getElementById("hideShippingAddress");
    var shippingFields = document.getElementById("shippingAddressFields");

    if (checkbox.checked) {
        shippingFields.style.display = "none";
    } else {
        shippingFields.style.display = "block";
    }
}

function InsertMultiplePurchaseOrderDetails() {
    if ($("#CreatePOForm").valid()) {

        var ProductDetails = [];
        $(".product").each(function () {
            var orderRow = $(this);
            var productName = orderRow.find("#textProductName").text().trim();
            var productId = orderRow.find("#textProductId").val().trim();
            var objData = {
                Product: productName,
                ProductId: productId,
                ProductType: orderRow.find("#textProductType").val(),
                Quantity: orderRow.find("#txtproductquantity").val(),
                Price: orderRow.find("#txtproductamount").val(),
                GSTamount: orderRow.find("#txtgstAmount").val(),
                Gst: orderRow.find("#txtgst").val(),
                ProductTotal: orderRow.find("#txtproducttotalamount").val(),
            };
            ProductDetails.push(objData);
        });

        var PONumber = $("#textPoId").val()
        var PODetails = {
            ProjectId: $("#textProjectId").val(),
            OrderId: $("#textPoId").val(),
            Type: $("#textProductType").val(),
            VendorId: $("#textVendorName").val(),
            CompanyName: $("#textCompanyName").val(),
            ProductShortDescription: $("#textDeliveryStatus").val(),
            ProductType: $("#textProductType").val(),
            Quantity: $("#txtproductquantity").val(),
            GstPerUnit: $("#txtgstAmount").val(),
            TotalGst: $("#totalgst").val(),
            AmountPerUnit: $("#txtproductamount").val(),
            SubTotal: $("#cart-subtotal").val(),
            TotalAmount: $("#cart-total").val(),
            DeliveryDate: $("#UnitTypeId").val(),
            OrderDate: $("#textOrderDate").val(),
            OrderStatus: $("#UnitTypeId").val(),
            PaymentMethod: $("#textPaymentMethod").val(),
            PaymentStatus: $("input[name='paymentStatus']:checked").val(),
            DeliveryStatus: $("#textDeliveryStatus").val(),
            CreatedBy: $("#textCreatedById").val(),
            Address: $('#hideShippingAddress').is(':checked') ? $('#textCompanyBillingAddress').val() : $('#textShippingAddress').val(),
            ProductList: ProductDetails,
        }

        var form_data = new FormData();
        form_data.append("PurchaseOrder", JSON.stringify(PODetails));
        $.ajax({
            url: '/PurchaseOrderMaster/InsertMultiplePurchaseOrderDetails',
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
                        window.location = '/PurchaseOrderMaster/GetPurchaseOrderDetailsById/?OrderId=' + PONumber;
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
    else {
        Swal.fire({
            title: "Kindly fill all data fields",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        })
    }
}