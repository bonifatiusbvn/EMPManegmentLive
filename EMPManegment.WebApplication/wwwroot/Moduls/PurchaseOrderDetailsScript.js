$(document).ready(function () {

    fn_GetPOPaymentTypeList();
    fn_GetPOVendorNameList();
    fn_GetPOCompanyNameList();
    fn_GetPOProductDetailsList();
    fn_GetPOPaymentMethodList();
    fn_updatePOTotals();
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
    $("#CreatePOForm").validate({

        rules: {
            textVendorName: "required",
            textCompanyName: "required",
            txtpaymentmethod: "required",
            textDescription: "required",
            textDeliveryStatus: "required",
        },
        messages: {
            textVendorName: "Select Vendor Name",
            textCompanyName: "Select Company Name",
            txtpaymentmethod: "Select Payment Method",
            textDescription: "Please Enter Description",
            textDeliveryStatus: "Please Enter Delivery Status",
        }
    });
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
    $('#createorder').on('click', function () {
        $("#createOrderForm").validate();
    });

    $("#statusform").validate({
        rules: {
            idStatus: "required"
        },
        messages: {
            idStatus: "Please enter delivered status"
        }
    })
    $('#statussearch').on('click', function () {
        $("#statusform").validate();
    });

    $('#textVendorName').change(function () {
        fn_getPOVendorDetail($(this).val());
    });
    $('#textCompanyName').change(function () {
        fn_getPOCompanyDetail($(this).val());
    });
    function handleFocus(event, selector) {
        if (event.keyCode == 13 || event.keyCode == 9) {
            event.preventDefault();
            $(selector).focus();
        }
    }
    $(document).on('input', '.product-quantity', function () {
        var row = $(this).closest('.product');
        fn_updatePOProductAmount(row);
        fn_updatePOTotals();
    }).on('keydown', '.product-quantity', function (event) {
        var row = $(this).closest(".product");
        var productFocus = row.find('#txtproductamount');
        handleFocus(event, productFocus);
    });
    function debounce(func, delay) {
        let timer;
        return function (...args) {
            clearTimeout(timer);
            timer = setTimeout(() => func.apply(this, args), delay);
        };
    }

    $(document).on('input', '#txtPOdiscountpercentage', debounce(function () {
        var value = $(this).val();
        var productRow = $(this).closest(".product");
        if (value > 100) {
            toastr.warning("Discount cannot be greater than 100%");
            productRow.find("#txtPOdiscountpercentage").val(0);
            productRow.find("#txtPOdiscountamount").val(0);
        } else if (value <= 0 || value == "") {
            productRow.find("#txtPOdiscountamount").val(0);
            productRow.find("#txtPOdiscountpercentage").val(0);
            fn_updatePOProductAmount(productRow);
        } else {
            fn_UpdatePODiscountPercentage(productRow);
        }
    }, 300)).on('keydown', '#txtPOdiscountpercentage', function (event) {
        var productRow = $(this).closest(".product");
        var gstFocus = productRow.find('#txtgst');
        handleFocus(event, gstFocus);
    });

    $(document).on('input', '#txtPOdiscountamount', debounce(function () {
        var productRow = $(this).closest(".product");
        var discountAmount = parseFloat($(this).val());
        var productAmount = parseFloat($(productRow).find("#productamount").val());

        if (discountAmount > productAmount) {
            toastr.warning("Amount cannot be greater than Item price");
            productRow.find("#txtPOdiscountamount").val(0);
            productRow.find("#txtPOdiscountpercentage").val(0);
        } else if (discountAmount <= 0 || discountAmount == "") {
            productRow.find("#txtPOdiscountamount").val(0);
            productRow.find("#txtPOdiscountpercentage").val(0);
            fn_updatePOProductAmount(productRow);
        } else {
            fn_updatePODiscount(productRow);
        }
    }, 300)).on('keydown', '#txtPOdiscountamount', function (event) {
        var productRow = $(this).closest(".product");
        var discountPercentagefocus = productRow.find('#txtPOdiscountpercentage');
        handleFocus(event, discountPercentagefocus);
    });

    $(document).on('input', '#txtproductamount', function () {
        var productRow = $(this).closest(".product");
        var productAmount = parseFloat($(this).val());
        var discountAmountfocus = productRow.find('#txtPOdiscountamount');

        if (!isNaN(productAmount)) {
            productRow.find("#txtPOdiscountamount").val(0);
            productRow.find("#txtPOdiscountpercentage").val(0);
        }

        productRow.find("#productamount").val(productAmount.toFixed(2));
        fn_updatePOProductAmount(productRow);
        fn_updatePOTotals();
    }).on('keydown', '#txtproductamount', function (event) {
        var productRow = $(this).closest(".product");
        var discountAmountfocus = productRow.find('#txtPOdiscountamount');
        handleFocus(event, discountAmountfocus);
    });

    $(document).on('input', '#txtgstPercentage', function () {
        var row = $(this).closest('.product');
        fn_updatePOProductAmount(row);
        fn_updatePOTotals();
    }).on('keydown', '#txtgstPercentage', function (event) {
        if (event.key === 'Enter') {
            $(this).blur();
        }
    });
    $(document).on('input', '#cart-roundOff', debounce(function () {
        var roundoff = $('#cart-roundOff').val();
        if (isNaN(roundoff) || (roundoff < -0.99 || roundoff > 0.99)) {
            toastr.warning("Value must be between -0.99 and 0.99");
        }
        else {
            fn_updatePOTotals();
        }
    }, 300));
    $(document).on('focusout', '.product-quantity', function () {
        $(this).trigger('input');
    });
});
function preventPOEmptyValue(input) {

    if (input.value === "") {

        input.value = 1;
    }
}
function showPaymentDetails() {
    $("#PaymentDetails").modal("show")
}

function fn_OpenAddPOproductmodal() {

    $('#mdPOProductSearch').val('');
    $('#mdPoproductModal').modal('show');
}

function fn_GetPOProductDetailsList(page) {
    var searchText = $('#mdPOProductSearch').val();

    $.get("/PurchaseOrderMaster/GetAllPOProductList", { searchText: searchText, page: page })
        .done(function (result) {
            $("#mdlistofPOItem").html(result);
        })
        .fail(function (xhr, status, error) {
            console.error("Error:", error);
        });
}

fn_GetPOProductDetailsList(1);

$(document).on("click", ".pagination a", function (e) {
    e.preventDefault();
    var page = $(this).text();
    fn_GetPOProductDetailsList(page);
});

$(document).on("click", "#backButton", function (e) {
    e.preventDefault();
    var page = $(this).text();
    fn_GetPOProductDetailsList(page);
});

function fn_clearPOSearchText() {
    $('#mdPOProductSearch').val('');
    fn_GetPOProductDetailsList();
}

function fn_filterallPOProducts() {
    var searchText = $('#mdPOProductSearch').val();

    $.ajax({
        url: '/PurchaseOrderMaster/GetAllPOProductList',
        type: 'GET',
        data: {
            searchText: searchText,
        },
        success: function (result) {
            $("#mdlistofPOItem").html(result);
        },
    });
}


function fn_GetPOVendorNameList() {
    $.ajax({
        url: '/ProductMaster/GetVendorsNameList',
        success: function (result) {
            var selectedValue = $('#textVendorName').find('option:first').val();
            $.each(result, function (i, data) {
                if (data.id !== selectedValue) {
                    $('#textVendorName').append('<Option value=' + data.id + '>' + data.vendorCompany + '</Option>')
                }
            });
        }
    });
}
function fn_getPOVendorDetail(VendorId) {
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
var companyMap = {};
function fn_GetPOCompanyNameList() {
    $.ajax({
        url: '/Company/GetCompanyNameList',
        success: function (result) {
            var selectedValue = $('#textCompanyName').find('option:first').val();
            $.each(result, function (i, data) {
                if (data.id !== selectedValue) {
                    $('#textCompanyName').append('<Option value=' + data.id + '>' + data.compnyName + '</Option>')
                }
                companyMap[data.compnyName] = data.id;
            });
        }
    });
}
function fn_getPOCompanyDetail(CompanyName) {
    var CompanyId = CompanyName;
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
                toastr.error('Empty response received.');
            }
        },
    });
}
function fn_GetPOProductsList() {
    $.ajax({
        url: '/ProductMaster/GetProduct',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#txtProducts').append('<Option value=' + data.id + '>' + data.productName + '</Option>')
            });
        }
    });
}
function fn_GetPOPaymentTypeList() {
    $.ajax({
        url: '/ExpenseMaster/GetPaymentTypeList',
        success: function (result) {
            var selectedValue = $('#txtpaymenttype').find('option:first').val();
            $.each(result, function (i, data) {
                $('#textPaymentMethod').append('<Option value=' + data.id + '>' + data.type + '</Option>')
                if (data.id != selectedValue) {
                    
                    $('#txtpaymenttype').append('<Option value=' + data.id + '>' + data.type + '</Option>')
                }  
            });
        }
    });
}
function fn_GetPOPaymentMethodList() {

    $.ajax({
        url: '/PurchaseOrderMaster/GetPaymentMethodList',
        success: function (result) {
            var selectedValue = $('#txtpaymentmethod').find('option:first').val();
            $.each(result, function (i, data) {
                if (data.id != selectedValue) {
                    $('#txtpaymentmethod').append('<Option value=' + data.id + '>' + data.paymentMethod + '</Option>')
                }
               
            });
        }
    });
}
function fn_POProductTypeDropdown(productId) {

    if ($('#txtPOProductType_' + productId + ' option').length > 1) {
        return
    }
    $.ajax({
        url: '/ProductMaster/GetProduct',
        success: function (result) {
            $('#txtPOProductType_' + productId).empty();

            $.each(result, function (i, data) {
                $('#txtPOProductType_' + productId).append('<option value="' + data.id + '">' + data.productName + '</option>');

            });

            $('#txtPOProductType_' + productId).val($("#txtunittype_" + productId).val())


        }
    });

}
function fn_SearchPOProductDetailsById(ProductId) {
    var GetProductId = {
        ProductId: ProductId,
    }
    var form_data = new FormData();
    form_data.append("ProductId", JSON.stringify(GetProductId));

    $.ajax({
        url: '/PurchaseOrderMaster/DisplayPOProductDetailsListById',
        type: 'Post',
        datatype: 'json',
        data: form_data,
        processData: false,
        contentType: false,
        complete: function (Result) {

            if (Result.statusText === "success") {
                fn_AddNewPODetailsRow(Result.responseText);
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
function deletePurchaseOrderDetails(Id) {
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
                url: '/PurchaseOrderMaster/DeletePurchaseOrderDetails?Id=' + Id,
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
                            window.location = '/PurchaseOrderMaster/PurchaseOrders';
                        })
                    } else {
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
                        window.location = '/PurchaseOrderMaster/PurchaseOrders';
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
function fn_UpdatePurchaseOrderDetails() {
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
                Gstamount: orderRow.find("#txtgstAmount").val(),
                Gstper: orderRow.find("#txtgstPercentage").val(),
                Hsn: orderRow.find("#txtHSNcode").val(),
                ProductTotal: orderRow.find("#txtproducttotalamount").val(),
                DiscountAmt: orderRow.find("#txtPOdiscountamount").val(),
                DiscountPer: orderRow.find("#txtPOdiscountpercentage").val(),
            };
            ProductDetails.push(objData);
        });
        var PONumber = $("#textPoId").val();
        var PODetails = {
            Id: $("#txtID").val(),
            ProjectId: $("#txtPOProjectId").val(),
            OrderId: $("#textPoId").val(),
            VendorId: $("#textVendorName").val(),
            CompanyId: $("#textCompanyName").val(),
            TotalGst: $("#totalgst").val(),
            SubTotal: $("#cart-subtotal").val(),
            TotalAmount: $("#cart-total").val(),
            DeliveryDate: $("#UnitTypeId").val(),
            OrderDate: $("#textOrderDate").val(),
            OrderStatus: $("#UnitTypeId").val(),
            PaymentMethod: $("#txtpaymentmethod").val(),
            PaymentStatus: $("#txtpaymenttype").val(),
            DeliveryStatus: $("#textDeliveryStatus").val(),
            RoundOff: $('#cart-roundOff').val(),
            TotalDiscount: $('#cart-discount').val(),
            CreatedBy: $("#textCreatedByVal").val(),
            CreatedOn: $("#textCreatedOnId").val(),
            UpdatedBy: $("#textCreatedById").val(),
            Address: $('#hideShippingAddress').is(':checked') ? $('#textCompanyBillingAddress').val() : $('#textShippingAddress').val(),
            ProductList: ProductDetails,
        }

        var form_data = new FormData();
        form_data.append("PurchaseOrder", JSON.stringify(PODetails));
        $.ajax({
            url: '/PurchaseOrderMaster/UpdatePurchaseOrderDetails',
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
                        window.location = '/PurchaseOrderMaster/PurchaseOrderDetails/?OrderId=' + PONumber;
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
    else {
        toastr.warning("Kindly fill all data fields");
    }
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
var count = 0;
function fn_AddNewPODetailsRow(Result) {

    var newProductRow = $(Result);
    var productId = newProductRow.data('product-id');
    fn_POProductTypeDropdown(productId);
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
        fn_updatePOTotals();
        fn_updatePORowNumbers();
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
function fn_updatePORowNumbers() {
    $(".product-id").each(function (index) {
        $(this).text(index + 1);
    });
}
function fn_updatePOProductAmount(that) {
    var row = $(that);
    var hiddenproductPrice = parseFloat(row.find("#productamount").val());
    var quantity = parseFloat(row.find("#txtproductquantity").val());
    var discountprice = parseFloat(row.find("#txtPOdiscountamount").val());
    var AmtWithDisc = hiddenproductPrice - discountprice;

    var gst = parseFloat(row.find("#txtgstPercentage").val());

    var totalGst = (AmtWithDisc * quantity * gst) / 100;

    var TotalAmountAfterDiscount = AmtWithDisc * quantity + totalGst;

    row.find("#txtgstAmount").val(totalGst.toFixed(2));
    row.find("#txtproducttotalamount").val(TotalAmountAfterDiscount.toFixed(2));
}
function fn_updatePODiscount(that) {
    var row = $(that);
    var productPrice = parseFloat(row.find("#productamount").val());
    var discountprice = parseFloat(row.find("#txtPOdiscountamount").val());
    var discountPercentage = parseFloat(row.find("#txtPOdiscountpercentage").val());

    if (isNaN(discountprice)) {
        row.find("#txtPOdiscountamount").val(0);
        row.find("#txtPOdiscountpercentage").val(0);
        fn_updatePOProductAmount(row);
        fn_updatePOTotals();
        return;
    }

    if (discountPercentage == 0 && discountprice > 0) {
        var discountperbyamount = discountprice / productPrice * 100;
        row.find("#txtPOdiscountpercentage").val(discountperbyamount.toFixed(2));
    } else if (discountprice > 0 && discountPercentage > 0) {
        var discountperbyamount = discountprice / productPrice * 100;
        row.find("#txtPOdiscountpercentage").val(discountperbyamount.toFixed(2));
    }

    fn_updatePOProductAmount(row);
    fn_updatePOTotals();
}
function fn_UpdatePODiscountPercentage(that) {
    var row = $(that);
    var productPrice = parseFloat(row.find("#productamount").val());
    var discountprice = parseFloat(row.find("#txtPOdiscountamount").val());
    var discountPercentage = parseFloat(row.find("#txtPOdiscountpercentage").val());

    if (isNaN(discountPercentage)) {
        row.find("#txtPOdiscountamount").val(0);
        row.find("#txtPOdiscountpercentage").val(0);
        fn_updatePOProductAmount(row);
        fn_updatePOTotals();
        return;
    }

    if (discountprice == 0 && discountPercentage > 0) {
        discountprice = productPrice * discountPercentage / 100;
        row.find("#txtPOdiscountamount").val(discountprice.toFixed(2));
    } else if (discountprice > 0 && discountPercentage > 0) {
        discountprice = productPrice * discountPercentage / 100;
        row.find("#txtPOdiscountamount").val(discountprice.toFixed(2));
    }
    fn_updatePOProductAmount(row);
    fn_updatePOTotals();
}
function fn_updatePOTotals() {
 
    var totalSubtotal = 0;
    var totalGst = 0;
    var totalAmount = 0;
    var TotalItemQuantity = 0;
    var TotalDiscount = 0;
    var roundoffvalue = $('#cart-roundOff').val();
    $(".product").each(function () {
     
        var row = $(this);
        var subtotal = parseFloat(row.find("#txtproductamount").val());
        var gst = parseFloat(row.find("#txtgstAmount").val());
        var totalquantity = parseFloat(row.find("#txtproductquantity").val());
        var discountprice = parseFloat(row.find("#txtPOdiscountamount").val());

        totalSubtotal += subtotal * totalquantity;
        totalGst += gst;
        TotalItemQuantity += totalquantity;
        TotalDiscount += discountprice;
        totalAmount = totalSubtotal + totalGst - TotalDiscount;
    });
    $("#cart-subtotal").val(totalSubtotal.toFixed(2));
    $("#totalgst").val(totalGst.toFixed(2));
    $("#cart-total").val(totalAmount.toFixed(2));
    $("#TotalProductQuantity").text(TotalItemQuantity);
    $("#TotalProductPrice").html(totalSubtotal.toFixed(2));
    $("#TotalProductGST").html(totalGst.toFixed(2));
    $("#TotalProductAmount").html(totalAmount.toFixed(2));
    $("#TotalDiscountPrice").html(TotalDiscount.toFixed(2));
    $("#cart-discount").val(TotalDiscount.toFixed(2));
    if (roundoffvalue != 0) {

        var roundtotal = parseFloat(totalAmount) + parseFloat(roundoffvalue);
        $("#cart-total").val(roundtotal.toFixed(2))
    } else {
        $("#cart-total").val(totalAmount.toFixed(2));
    }
}
function fn_POremoveItem(btn) {
    $(btn).closest("tr").remove();
    fn_updatePORowNumbers();
    fn_updatePOTotals();
}
function fn_OpenPOShippingModal() {
    $('#textmdAddress').val('');
    $('#textmdQty').val('');
    $('#mdShippingAdd').modal('show');
}
function fn_mdAddPOAddress() {
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
function fn_POtoggleShippingAddress() {
    var checkbox = document.getElementById("hideShippingAddress");
    var shippingFields = document.getElementById("shippingAddressFields");

    if (checkbox.checked) {
        shippingFields.style.display = "none";
    } else {
        shippingFields.style.display = "block";
    }
}
function fn_InsertPurchaseOrderDetails() {
   
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
                Gstamount: orderRow.find("#txtgstAmount").val(),
                Gstper: orderRow.find("#txtgstPercentage").val(),
                Hsn: orderRow.find("#txtHSNcode").val(),
                ProductTotal: orderRow.find("#txtproducttotalamount").val(),
                DiscountAmt: orderRow.find("#txtPOdiscountamount").val(),
                DiscountPer: orderRow.find("#txtPOdiscountpercentage").val(),
            };
            ProductDetails.push(objData);
        });
        var PONumber = $("#textPoId").val();
        var PODetails = {
            ProjectId: $("#textProjectId").val(),
            OrderId: $("#textPoId").val(),
            VendorId: $("#textVendorName").val(),
            CompanyId: $("#textCompanyName").val(),
            TotalGst: $("#totalgst").val(),
            SubTotal: $("#cart-subtotal").val(),
            TotalAmount: $("#cart-total").val(),
            DeliveryDate: $("#UnitTypeId").val(),
            OrderDate: $("#textOrderDate").val(),
            OrderStatus: $("#UnitTypeId").val(),
            PaymentMethod: $("#txtpaymentmethod").val(),
            PaymentStatus: $("#txtpaymenttype").val(),
            DeliveryStatus: $("#textDeliveryStatus").val(),
            RoundOff: $('#cart-roundOff').val(),
            TotalDiscount: $('#cart-discount').val(),
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
                        window.location = '/PurchaseOrderMaster/PurchaseOrderDetails/?OrderId=' + PONumber;
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
    else {
        toastr.warning("Kindly fill all data fields");
    }
}
function createPO() {
    if ($("#txtPOId").val() == "") {
        Swal.fire({
            title: "Kindly select project on dashboard.",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        });
    }
    else {
        window.location = '/PurchaseOrderMaster/CreatePurchaseOrder';
    }
}

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