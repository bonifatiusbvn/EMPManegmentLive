
$(document).ready(function () {
    GetVendorNameList()
    GetAllVendorData()
    GetAllTransactionData()
    AllInvoiceList()
    GetCompanyNameList()
    GetProductDetailsList()
    GetPaymentTypeList()
});
$(document).ready(function () {
    $("#CreateInvoiceForm").validate({
        rules: {
            textVendorName: "required",
            textCompanyName: "required",
            textPaymentMethod: "required",
            textDispatchThrough: "required",
        },
        messages: {
            textVendorName: "Select Vendor Name",
            textCompanyName: "Select Company Name",
            textPaymentMethod: "Select Payment Method",
            textDispatchThrough: "Please Enter DispatchThrough",
        }
    });
});
function clearItemErrorMessage() {
    $("#spnitembutton").text("");
}
$(document).on("click", "#addItemButton", function () {
    clearItemErrorMessage();
});
function GetVendorNameList() {

    $.ajax({
        url: '/ProductMaster/GetVendorsNameList',
        success: function (result) {
            $.each(result, function (i, data) {
                //$('#txtvendorname').append('<Option value=' + data.id + '>' + data.vendorCompany + '</Option>')
                //$('#txtvendorname1').append('<Option value=' + data.id + '>' + data.vendorCompany + '</Option>')
                $('#textVendorName').append('<Option value=' + data.id + '>' + data.vendorCompany + '</Option>')
            });
        }
    });
}
function selectvendorId() {
    //document.getElementById("txtvendorTypeid").value = document.getElementById("txtvendorname").value;
    //document.getElementById("txtvendorTypeid1").value = document.getElementById("txtvendorname1").value;
    document.getElementById("txtvendorTypeid").value = document.getElementById("txtvendorname").value;
}
$(document).ready(function () {
    $('#textVendorName').change(function () {
        getVendorDetail($(this).val());
    });
});

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
$(document).ready(function () {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();

    today = yyyy + '-' + mm + '-' + dd;
    $("#textInvoiceDate").val(today);
    $("#textInvoiceDate").prop("disabled", true);
});

function GetProductDetailsList() {
    var searchText = $('#mdProductSearch').val();

    $.get("/Invoice/GetAllProductList", { searchText: searchText })
        .done(function (result) {
            $("#mdlistofItem").html(result);
        })
        .fail(function (xhr, status, error) {
            console.error("Error:", error);
        });
}

function fn_OpenAddproductmodal() {

    $('#mdProductSearch').val('');
    $('#mdPoproductModal').modal('show');
}

function SearchProductDetailsById(ProductId) {
    var GetProductId = {
        Id: ProductId,
    }
    var form_data = new FormData();
    form_data.append("ProductId", JSON.stringify(GetProductId));


    $.ajax({
        url: '/Invoice/DisplayProductDetailsListById',
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

function GetInvoiceDetailsByOrderId(OrderId) {
    $.ajax({
        url: '/Invoice/GetInvoiceDetailsByOrderId/?OrderId=' + OrderId,
        type: 'GET',
        success: function (result) {
            if (result.code == 400) {
                Swal.fire({
                    title: result.message,
                    icon: result.icone,
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                });
            } else {
                window.location = '/Invoice/InvoiceDetails/?OrderId=' + OrderId;
            }
        },
        error: function (xhr, status, error) {
            console.error('AJAX Error:', error);
            // Handle error here, for example, show an alert
            alert('An error occurred while fetching data.');
        }
    });
}

function ShowInvoiceDetailsByOrderId(OrderId) {

    $.ajax({
        url: '/Invoice/ShowInvoiceDetailsByOrderId/?OrderId=' + OrderId,
        type: 'GET',
        success: function (result) {
            if (result.code == 400) {
                Swal.fire({
                    title: result.message,
                    icon: result.icone,
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                });
            } else {
                window.location = '/Invoice/DisplayInvoiceDetails?OrderId=' + OrderId;
            }
        },
        error: function (xhr, status, error) {
            console.error('AJAX Error:', error);
            // Handle error here, for example, show an alert
            alert('An error occurred while fetching data.');
        }
    });
}
function InsertInvoiceDetails() {debugger
    if ($("#CreateInvoiceForm").valid()) {
        if ($('#addnewproductlink tr').length >= 1) {

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
        var Invoicedetails = {
            ProjectId: $("#textProjectId").val(),
            InvoiceNo: $("#textInvoiceNo").val(),
            VandorId: $("#textVendorName").val(),
            CompanyName: $("#textCompanyName").val(),
            TotalGst: $("#totalgst").val(),
            Cgst: $("#textCGst").val(),
            Sgst: $("#textSGst").val(),
            Igst: $("#textIGst").val(),
            SubTotal: $("#cart-subtotal").val(),
            TotalAmount: $("#cart-total").val(),
            DispatchThrough: $("#textDispatchThrough").val(),
            BuyesOrderNo: $("#textBuysOrderNo").val(),
            BuyesOrderDate: $("#textBuysOrderDate").val(),
            InvoiceDate: $("#textInvoiceDate").val(),
            OrderStatus: $("#UnitTypeId").val(),
            PaymentMethod: $("#textPaymentMethod").val(),
            CreatedBy: $("#textCreatedById").val(),
            ShippingAddress: $('#hideShippingAddress').is(':checked') ? $('#textCompanyBillingAddress').val() : $('#textShippingAddress').val(),
            InvoiceDetails: ProductDetails,
        }
        var form_data = new FormData();
        form_data.append("INVOICEDETAILS", JSON.stringify(Invoicedetails));
        debugger
        $.ajax({
            url: '/Invoice/InsertInvoiceDetails',
            type: 'POST',
            data: form_data,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (Result) {debugger
                if (Result.code == 200) {
                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/Invoice/InvoiceListView';
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
        } else {
            if ($('#addnewproductlink tr').length == 0) {
                $("#spnitembutton").text("Please Select Product!");
            } else {
                $("#spnitembutton").text("");
            }
        }
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



$(document).ready(function () {
    $("#generatePDF").click(function () {

        var pdf = new jsPDF();


        var htmlContent = "<h1 style='color: #3498db; text-align: center;'>PDF Generated with jQuery</h1>";
        htmlContent += "<p>This is a dynamically generated PDF content.</p>";


        pdf.fromHTML(htmlContent, 15, 15, {
            'width': 170,
            'elementHandlers': {
                '#ignorePDF': function (element, renderer) {
                    return true;
                }
            }
        });


        pdf.save("generated_pdf.pdf");
    });
});
$(document).ready(function () {
    var itemCounter = 0;

    $("#addItemBtn").click(function () {
        var newItem = $(".invoice-item:first").clone().removeAttr("style");
        newItem.find("input").val(""); // Clear input values for the new item
        $("#invoiceItems").append(newItem);

        itemCounter++;
    });

    $("#SubmitInvoiceBtn").click(function (e) {

        e.preventDefault();

        var formData =
        {
            CompanyAddress: $("#idStatusCompany").val(),
            InvoiceNo: $("#InvoiceNo").val(),
            Date: $("#date-field").val(),
            PaymentStatus: $("#choices-payment-status").val(),
            TotalAmount: $("#totalamountInput").val(),
            BillingName: $("#billingName").val(),
            BillingAddress: $("#billingAddress").val(),
            BillingNumber: $("#billingPhoneno").val(),
            BillingTaxNumber: $("#billingTaxno").val(),
            ShippingName: $("#shippingName").val(),
            ShippingAddress: $("#shippingAddress").val(),
            ShippingNumber: $("#shippingPhoneno").val(),
            ShippingTaxNumber: $("#shippingTaxno").val(),
            ProductName: $("#productName-1").val(),
            ProductDetails: $("#productDetails-1").val(),
            HSN: $("#productHsn-1").val(),
            Price: $("#productRate-1").val(),
            Quantity: $("#product-qty-1").val(),
            PaymentMethod: $("#choices-payment-type").val(),
            CardHolderName: $("#cardholderName").val(),
            CardNumber: $("#cardNumber").val(),
        };


        $.ajax({
            url: '/Invoice/GenerateInvoice',
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response) {
                    generatePdf(response);
                } else {
                    alert("Error generating invoice. Please try again.");
                }
            },
            error: function () {
                alert("An error occurred. Please try again.");
            }
        });
    });

});
function deleteInvoice(InvoiceId) {
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
                url: '/Invoice/IsDeletedInvoice?InvoiceId=' + InvoiceId,
                type: 'POST',
                dataType: 'json',
                success: function (Result) {
                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/Invoice/InvoiceListView';
                    })
                },
                error: function () {
                    Swal.fire({
                        title: "Can't Delete Invoice!",
                        icon: 'warning',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/Invoice/InvoiceListView';
                    })
                }
            })
        } else if (result.dismiss === Swal.DismissReason.cancel) {

            Swal.fire(
                'Cancelled',
                'Invoice Have No Changes.!!😊',
                'error'
            );
        }
    });
}

function generatePdf(data) {

    var pdf = new jsPDF();


    var htmlContent = '<div class="col-lg-12"><div class="d-flex"><div class="flex-grow-1"><div class="mt-sm-5 mt-4"><h6 class="text-muted text-uppercase fw-semibold">Address</h6><p class="text-muted mb-1" id="address-details">California, United States</p></div></div><div class="flex-shrink-0 mt-sm-0 mt-3"><h6><span class="text-muted fw-normal">Legal Registration No:</span><span id="legal-register-no">987654</span></h6><h6><span class="text-muted fw-normal">Email:</span><span id="email">velzon@themesbrand.com</span></h6><h6 class="mb-0"><span class="text-muted fw-normal">Contact No: </span><span id="contact-no"> +(01) 234 6789</span></h6></div></div></div ></div >';
    htmlContent += '<div class="col-lg-12"><div class="row g-3"><div class="col-lg-3 col-6"><p class="text-muted mb-2 text-uppercase fw-semibold">Invoice No</p><h5 class="fs-14 mb-0">#VL<span id="invoice-no">' + data.invoiceNo + '</span></h5></div><div class="col-lg-3 col-6"><p class="text-muted mb-2 text-uppercase fw-semibold">Date</p><h5 class="fs-14 mb-0"><span id="invoice-date">23 Nov, 2021</span> <small class="text-muted" id="invoice-time">' + data.date + '</small></h5></div><div class="col-lg-3 col-6"><p class="text-muted mb-2 text-uppercase fw-semibold">Payment Status</p><span class="badge bg-success-subtle text-success fs-11" id="payment-status">' + data.paymentStatus + '</span></div><div class="col-lg-3 col-6"><p class="text-muted mb-2 text-uppercase fw-semibold">Total Amount</p><h5 class="fs-14 mb-0">$<span id="total-amount">' + data.totalAmount + '</span></h5></div></div></div >';
    // Add the HTML content to the PDF with styles
    pdf.fromHTML(htmlContent, 15, 15, {
        'width': 170,
        'elementHandlers': {
            '#ignorePDF': function (element, renderer) {
                return true;
            }
        }
    });

    pdf.save("generated_pdf.pdf");
}


$('#idStatus').change(function () {
    if ($("#idStatus").val() == "Selse") {
        $("#companyaddress").show();
        $("#fullcompanyaddress").show();
        $("#txtvendorname1").show();
        $("#vendername").hide();
        $("#CompanyAddress1").hide();
        $("#fullcompanyaddress1").hide();
    }
    if ($("#idStatus").val() == "Purchase") {
        $("#vendername").show();
        $("#CompanyAddress1").show();
        $("#fullcompanyaddress1").show();
        $("#companyaddress").hide();
        $("#fullcompanyaddress").hide();
        $("#txtvendorname1").hide();
    }
});



function downloadPDF() {

    var htmlContent = document.getElementById('printableContent').innerHTML;
    var form_data = new FormData();
    form_data.append("DOWNLOADINVOICE", htmlContent);

    $.ajax({
        url: '/Invoice/DownloadPdf',
        type: 'POST',
        data: form_data,
        contentType: false,
        processData: false,
        success: function (data) {

            console.log('PDF generated successfully:', data);

            window.location.href = data;
        },
        error: function (xhr, status, error) {

            console.error(xhr.responseText);
        }
    });
}

function GetAllVendorData() {

    $('#VendorTableData').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "Post",
            url: '/Invoice/GetVendorList',
            dataType: 'json'
        },
        columns: [
            {
                "render": function (data, type, full) {
                    return '<h5 class="fs-15"><a href="/Invoice/VendorInvoiceListView/?Vid=' + full.id + '" class="fw-medium link-primary">' + full.vendorCompany; '</a></h5>';
                }
            },
            {
                "data": null,
                "name": "VendorFullName",
                "render": function (data, type, full, meta) {
                    return full.vendorFirstName + ' ' + full.vendorLastName;
                }
            },
            { "data": "vendorEmail", "name": "VendorEmail" },
            { "data": "vendorPhone", "name": "VendorPhone" },
            { "data": "vendorAddress", "name": "VendorAddress" },
        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all",
        }]
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
$(document).ready(function () {
    GetPaymentMethodList();
});
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
function EditInvoiceDetails(InvoiceNo) {

    $.ajax({
        url: '/Invoice/EditInvoiceDetails?InvoiceNo=' + InvoiceNo,
        type: "Get",
        contentType: 'application/json;charset=utf-8;',
        dataType: 'json',
        success: function (response) {

            $('#UpdateInvoiceModel').modal('show');
            $('#txtinvoiceno').html(response.invoiceNo);
            $('#txtinvoicedate').val(response.invoiceDate);
            $('#txtid').val(response.id);
            $('#txtcompanyname').val(response.companyName);
            $('#txtamount').val(response.totalAmount);
            $('#txtpaymentmethod').val(response.paymentMethod);
            $('#txtstatus').val(response.status);
        },
        error: function () {
            alert('Data not found');
        }
    });
}

function UpdateInvoiceDetails() {
    if ($('#UpdateInvoiceDetailsForm').valid()) {
        var formData = new FormData();
        formData.append("InvoiceNo", $("#txtinvoiceno").val());
        formData.append("InvoiceDate", $("#txtinvoicedate").val());
        formData.append("Id", $("#txtid").val());
        formData.append("companyName", $("#txtcompanyname").val());
        formData.append("TotalAmount", $("#txtamount").val());
        formData.append("PaymentMethod", $("#txtpaymentmethod").val());
        formData.append("Status", $("#txtstatus").val());

        $.ajax({
            url: '/Invoice/UpdateInvoiceDetails',
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
                        window.location = '/Invoice/InvoiceListView';
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

    $("#UpdateInvoiceDetailsForm").validate({
        rules: {
            txtinvoicedate: "required",
            txtamount: "required",
            txtpaymentmethod: "required",
            txtstatus: "required",
            txtcompanyname: "required",
        },
        messages: {
            txtinvoicedate: "Please emter invoice date",
            txtamount: "Please enter invoice Amount",
            txtpaymentmethod: "Please Enter Payment method",
            txtstatus: "Plese enter status",
            txtcompanyname: "Plese enter Company name",
        }
    })
    $("#updatedetailbtn").on('click', function () {
        $("#UpdateInvoiceDetailsForm").validate();
    });
});

function GetAllTransactionData() {
    $('#transactionTable').DataTable({
        processing: true,
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
                    var date = new Date(data);
                    return date.toLocaleDateString();
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
            htmlContent += '<td class="form_name">' + data.date + '</td>';
            htmlContent += '<td class="to_name">' + data.paymentMethodName + '</td>';
            htmlContent += '<td class="to_name">' + data.paymentTypeName + '</td>';
            htmlContent += '<td class="to_name text-success">' + data.creditDebitAmount + '</td>';
            htmlContent += '<td class="to_name text-danger">' + data.pendingAmount + '</td>';
            htmlContent += '<td class="status"><span class="badge bg-primary-subtle text-primary fs-11"><i class="ri-time-line align-bottom"></i> Processing</span></td>';

            $(row).html(htmlContent);
        }
    });
}

function AllInvoiceList() {
    $('#invoiceTable').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "POST",
            url: '/Invoice/GetInvoiceListView',
            dataType: 'json',
        },
        columns: [
            {
                "render": function (data, type, full) {
                    return '<h5 class="fs-15"><a href="/Invoice/DisplayInvoiceDetails?OrderId=' + full.orderId + '" class="fw-medium link-primary">' + full.invoiceNo; '</a></h5>';
                }
            },
            {
                "data": "vendorName", "name": "VendorName",
                "className": "text-center"
            },
            {
                "data": "projectName", "name": "ProjectName",
                "className": "text-center"
            },
            {
                "data": "orderId", "name": "OrderId",
                "className": "text-center"
            },
            {
                "data": "totalAmount", "name": "TotalAmount",
                "className": "text-center"
            },
            {
                "data": "Action", "name": "Action",
                render: function (data, type, full) {
                    return ('<li class="btn list-inline-item edit" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="Edit"><a onclick="EditInvoiceDetails(\'' + full.invoiceNo + '\')"><i class="ri-pencil-fill fs-16"></i></a></li><li class="btn text-danger list-inline-item delete" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="Delete" style="margin-left:12px;"><a onclick="deleteInvoice(\'' + full.id + '\')"><i class="fas fa-trash"></i></a></li>');
                }
            },
        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all",
        }]
    });
}









