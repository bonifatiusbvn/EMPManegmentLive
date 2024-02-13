
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
                url: '/OrderMaster/GetOrderDetailsByStatus',
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

function SaveCreateOrder() {
    if ($('#createOrderForm').valid()) {

        var formData = new FormData();
        formData.append("Type", $("#OrderType").val());
        formData.append("OrderId", $("#orderId").val());
        formData.append("VendorId", $("#txtvendorname").val());
        formData.append("CompanyName", $("#txtvendorname").val());
        formData.append("Product", $("#productname").val());
        formData.append("Quantity", $("#productquantity").val());
        formData.append("Amount", $("#amount").val());
        formData.append("Total", $("#totalamount").val());
        formData.append("OrderDate", $("#orderdate").val());
        formData.append("DeliveryDate", $("#deliverydate").val());
        formData.append("PaymentMethod", $("#payment").val());
        formData.append("DeliveryStatus", $("#deliveredstatus").val());
        formData.append("CreatedBy", $("#ddlusername").val());
        $.ajax({
            url: '/OrderMaster/CreateOrder',
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
                        window.location = '/OrderMaster/CreateOrder';
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


function InsertMultipleOrder() {
    debugger

    var orderDetails = [];
    for (var i = 0; i < 2; i++) {debugger
        var objData = {
            Type: $("#OrderType").val(),
            OrderId: $("#orderId").val(),
            PaymentStatus: $("#txtPaymentStatus").val(),
            PaymentMethod: $("#paymentMethod").val(),
            OrderDate: $("#orderdate").val(),
            DeliveryDate: $("#deliverydate").val(),
            VendorId: $("#txtvendorname").val(),
            CompanyName: $("#txtvendornameid").val(),
            ProductType: $("#productname").val(),
            Quantity: $("#txtproductquantity").val(),
            Amount: $("#txtproductamount").val(),
            Total: $("#txtproducttotalamount").val(),
        };
        debugger
        orderDetails.push(objData);
    }
    var form_data = new FormData();
    form_data.append("ORDERDETAILS", JSON.stringify(orderDetails));
    $.ajax({
        url: '/OrderMaster/InsertMultipleOrders',
        type: 'Post',
        data: form_data,
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
                    window.location = '/OrderMaster/CreateOrderView';
                });
            }
        }
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




$(document).ready(function () {
    GetVendorNameList()
    document.getElementById("txtvendorname").click()
    document.getElementById("productname").click()
    document.getElementById("searchproductname").click()
});
function GetVendorNameList() {
    $.ajax({
        url: '/ProductMaster/GetVendorsNameList',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#txtvendorname').append('<Option value=' + data.id + '>' + data.vendorCompany + '</Option>')
            });
        }
    });
}
function selectvendorId() {
    document.getElementById("txtvendorTypeid").value = document.getElementById("txtvendorname").value;
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
                    $('#productname').append('<Option value=' + data.id + '>' + data.productName + '</Option>')
                });
            }
        });
    });
    $('#productname').change(function () {

        var Text = $("#productname Option:Selected").text();
        var ProductTypeId = $(this).val();
        var VendorTypeId = $("#txtvendorname").val();
        $("#txtProductnameid").val(Text);
        $('#searchproductname').empty();
        $('#searchproductname').append('<Option >--Select ProductName--</Option>');
        $.ajax({
            url: '/ProductMaster/SerchProductByVendor?ProductId=' + ProductTypeId + '&VendorId=' + VendorTypeId,
            type: 'Post',
            success: function (result) {
                $.each(result, function (i, data) {
                    $('#searchproductname').append('<Option value=' + data.id + '>' + data.productName + '</Option>');

                });
            }
        });
    });
});
function searchProductTypeId() {
    document.getElementById("searchproductnameid").value = document.getElementById("searchproductname").value;
}

function SerchProductDetailsById() {

    var GetProductId = {
        Id: $('#searchproductname').val(),
    }
    var form_data = new FormData();
    form_data.append("PRODUCTID", JSON.stringify(GetProductId));


    $.ajax({
        url: '/ProductMaster/DisplayProductDetailsById',
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
                var vendorname = $('#txtvendorname').val();
                var productname = $('#productname').val();
                var serchproductname = $('#searchproductname').val();
                if (vendorname === '' || vendorname === null) {
                    $('#vendorvalidationMessage').text('Please select Vendor!!');
                }
                if (productname === "--Select Product--" || productname === null) {
                    $('#productvalidationMessage').text('Please select Product!!');
                }
                if (serchproductname === "--Select ProductName--" || serchproductname === null) {
                    $('#searchvalidationMessage').text('Please select Product!!');
                }
                else {
                    $('#vendorvalidationMessage').text('');
                    $('#productvalidationMessage').text('');
                    $('#searchvalidationMessage').text('');
                }
            }
        }
    });
}


function selectProductTypeId() {
    document.getElementById("txtProductnameid").value = document.getElementById("productname").value;
}
$("#productname").change(function () {
    ProductDetailsByProductTypeId()
})
function ProductDetailsByProductTypeId() {
    var form_data = new FormData();
    form_data.append("ProductId", $('#productname').val());
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
}), flatpickr("#date-field", {
    enableTime: !0,
    dateFormat: "d M, Y, h:i K"
}), isData();
var count = 1;

function AddNewRow(Result) {
    count++;
    var e = document.createElement("tr");
    e.id = count;
    e.className = "product";
    e.innerHTML = document.getElementById("newRowfrm").innerHTML + Result;
    document.getElementById("addNewlink").appendChild(e);

    // Initialize Choices plugin for new elements
    Array.from(e.querySelectorAll("[data-trigger]")).forEach(function (element) {
        new Choices(element, {
            placeholderValue: "This is a placeholder set in the config",
            searchPlaceholderValue: "This is a search placeholder"
        });
    });

    bindEventListeners();

    // Update product total amount for the new row
    updateProductTotalAmount(e);

    // Update totals after adding the new row
    updateTotals();
}

function bindEventListeners() {
    // Event listener for removing a product
    document.querySelectorAll(".product-removal a").forEach(function (e) {
        e.addEventListener("click", function (event) {
            removeItem(event.target.closest("tr"));
            updateTotals();
        });
    });

    // Event listener for increasing product quantity
    document.querySelectorAll(".plus").forEach(function (btn) {
        btn.addEventListener("click", function (event) {
            updateProductQuantity(event.target.closest("tr"), 1);
            updateTotals();
        });
    });

    // Event listener for decreasing product quantity
    document.querySelectorAll(".minus").forEach(function (btn) {
        btn.addEventListener("click", function (event) {
            updateProductQuantity(event.target.closest("tr"), -1);
            updateTotals();
        });
    });
}

function updateProductQuantity(row, increment) {
    var quantityInput = row.querySelector(".product-quantity");
    var currentQuantity = parseInt(quantityInput.value);
    var newQuantity = currentQuantity + increment;
    if (newQuantity >= 0) {
        quantityInput.value = newQuantity;
        updateProductTotalAmount(row);
    }
}

function updateProductTotalAmount(row) {
    var productPrice = parseFloat(row.querySelector("#txtproductamount").value);
    var quantity = parseInt(row.querySelector(".product-quantity").value);
    var gst = parseFloat(row.querySelector("#txtgst").value);
    var totalGst = (productPrice * quantity * gst) / 100;
    var totalAmount = productPrice * quantity + totalGst;

    row.querySelector("#txtproductamountwithGST").value = totalGst.toFixed(2);
    row.querySelector("#txtproducttotalamount").value = totalAmount.toFixed(2);
}

function updateTotals() {
    var totalSubtotal = 0;
    var totalGst = 0;
    var totalAmount = 0;

    document.querySelectorAll(".product").forEach(function (row) {
        var subtotal = parseFloat(row.querySelector("#txtproducttotalamount").value);
        var gst = parseFloat(row.querySelector("#txtproductamountwithGST").value);

        totalSubtotal += subtotal;
        totalGst += gst;
        totalAmount += subtotal + gst;
    });

    $("#cart-subtotal").val(totalSubtotal.toFixed(2));
    $("#totalgst").val(totalGst.toFixed(2));
    $("#cart-total").val(totalAmount.toFixed(2));
}





remove();
var taxRate = .125,
    shippingRate = 65,
    discountRate = .15,
    gst = 18;




function remove() {
    Array.from(document.querySelectorAll(".product-removal a")).forEach(function (e) {
        e.addEventListener("click", function (e) {
            removeItem(e), resetRow()
        })
    })
}

function resetRow() {
    Array.from(document.getElementById("addNewlink").querySelectorAll("tr")).forEach(function (e, t) {
        t += 1;
        e.querySelector(".product-id").innerHTML = t
    })
}

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

function removeItem(e) {
    e.target.closest("tr").remove(), recalculateCart()
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
var cleaveBlocks = new Cleave("#cardNumber", {
    blocks: [4, 4, 4, 4],
    uppercase: !0
}),
    genericExamples = document.querySelectorAll('[data-plugin="cleave-phone"]');
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