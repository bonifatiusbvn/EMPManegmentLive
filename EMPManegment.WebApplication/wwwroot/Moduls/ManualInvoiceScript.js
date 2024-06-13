$(document).ready(function () {
    function handleFocus(event, selector) {
        if (event.keyCode == 13 || event.keyCode == 9) {
            event.preventDefault();
            $(selector).focus();
        }
    }

    function showErrorMessage(selector, message) {
        $(selector).text(message).show();
    }

    let rowCount = 0;
    $('#AddVendorModelButton').on('click', function () {
        rowCount++;
        const newRow = `
            <tr id="templateRow_${rowCount}" class="product" data-product-id="@item.ItemId">
                <td scope="row" class="product-id">${rowCount}</td>
                <td class="text-start">
                    <input id="textProductName" class="product-quantity form-control" />
                </td>
                <td class="text-start">
                    <input type="text" class="form-control" id="txtHSNcode" style="width: 75px;" />
                </td>
                <td class="text-start">
                    <div class="">
                         <input type="text" class="product-quantity form-control" id="txtproductquantity" value="1" oninput="preventEmptyValue(this)" style="width:80px;">
                    </div>
                </td>
                <td class="text-start">
                    <div class="">
                    <select class="form-control" id="txtPOProductType_${rowCount}" style="width: 151px;">
                 <option value=""></option>
             </select>
                    </div>
                </td>
                <td class="text-start">
                    <input type="text" class="form-control" id="txtproductamount" value="0" oninput="preventEmptyValue(this)"/>
                </td>
                <td class="text-start">
                    <div class="row">
                        <div class="col-sm-6">
                            <input type="text" class="form-control" id="txtdiscountamount" value="0" oninput="preventEmptyValue(this)"/>
                        </div>
                        <div class="col-sm-6">
                            <input type="text" class="form-control" id="txtdiscountpercentage" value="0" oninput="preventEmptyValue(this)"/>
                        </div>
                    </div>
                </td>
                <td class="text-start">
                    <div class="row">
                        <div class="col-sm-6">
                            <input type="text" class="product-Gstper form-control" id="txtgst" value="0" oninput="preventEmptyValue(this)"/>
                        </div>
                        <div class="col-sm-6">
                            <input type="number" class="product-Gstamount form-control bg-light" id="txtgstAmount" value="0" readonly/>
                        </div>
                    </div>
                </td>
                <td class="text-start">
                    <div>
                        <input type="text" class="product-producttotalamount form-control bg-light" id="txtproducttotalamount" readonly/>
                    </div>
                </td>
                <td class="product-removal">
                    <a class="btn text-danger remove-btn" onclick="removeItem(this)"><i class="fas fa-trash"></i></a>
                </td>
            </tr>`;
        $('#addnewproductlink').append(newRow);
        updateRowNumbers();
    });

    $(document).on('input', '#txtproductquantity', function () {
        var productRow = $(this).closest(".product");
        updateProductTotalAmount(productRow);
        updateTotals();
    }).on('keydown', '#txtproductquantity', function (event) {
        var productRow = $(this).closest(".product");
        var productFocus = productRow.find('#txtproductamount');
        handleFocus(event, productFocus);
    });

    $(document).on('input', '#txtgst', function () {
        var productRow = $(this).closest(".product");
        var gstvalue = $('#txtgst').val();
        if (gstvalue > 100) {
            toastr.warning("GST% cannot be greater than 100%");
            $(this).val(100);
        }
        updateProductTotalAmount(productRow);
        updateTotals();
    })

    function debounce(func, delay) {
        let timer;
        return function (...args) {
            clearTimeout(timer);
            timer = setTimeout(() => func.apply(this, args), delay);
        };
    }

    $(document).on('input', '#txtdiscountpercentage', debounce(function () {
        var value = $(this).val();
        var productRow = $(this).closest(".product");
        if (value > 100) {
            toastr.warning("Discount cannot be greater than 100%");
            productRow.find("#txtdiscountpercentage").val(0);
            productRow.find("#txtdiscountamount").val(0);
        } else if (value <= 0 || value == "") {
            productRow.find("#txtdiscountamount").val(0);
            productRow.find("#txtdiscountpercentage").val(0);
            updateProductTotalAmount(productRow);
        } else {
            UpdateDiscountPercentage(productRow);
        }
    }, 300)).on('keydown', '#txtdiscountpercentage', function (event) {
        var productRow = $(this).closest(".product");
        var gstFocus = productRow.find('#txtgst');
        handleFocus(event, gstFocus);
    });

    $(document).on('input', '#txtdiscountamount', debounce(function () {
        var productRow = $(this).closest(".product");
        var discountAmount = parseFloat($(this).val());
        var productAmount = parseFloat($(productRow).find("#productamount").val());

        if (discountAmount > productAmount) {
            toastr.warning("Amount cannot be greater than Item price");
            productRow.find("#txtdiscountamount").val(0);
            productRow.find("#txtdiscountpercentage").val(0);
        } else if (discountAmount <= 0 || discountAmount == "") {
            productRow.find("#txtdiscountamount").val(0);
            productRow.find("#txtdiscountpercentage").val(0);
            updateProductTotalAmount(productRow);
        } else {
            updateDiscount(productRow);
        }
    }, 300)).on('keydown', '#txtdiscountamount', function (event) {
        var productRow = $(this).closest(".product");
        var discountPercentagefocus = productRow.find('#txtdiscountpercentage');
        handleFocus(event, discountPercentagefocus);
    });

    $(document).on('input', '#txtproductamount', function () {
        var productRow = $(this).closest(".product");
        var productAmount = parseFloat($(this).val());
        var discountAmountfocus = productRow.find('#txtdiscountamount');

        if (!isNaN(productAmount)) {
            productRow.find("#txtdiscountamount").val(0);
            productRow.find("#txtdiscountpercentage").val(0);
        }

        productRow.find("#productamount").val(productAmount.toFixed(2));
        updateProductTotalAmount(productRow);
        updateTotals();

    }).on('keydown', '#txtproductamount', function (event) {
        var productRow = $(this).closest(".product");
        var discountAmountfocus = productRow.find('#txtdiscountamount');
        handleFocus(event, discountAmountfocus);
    });


    $(document).on('input', '#cart-roundOff', debounce(function () {
        var roundoff = $('#cart-roundOff').val();
        if (isNaN(roundoff) || (roundoff < -0.99 || roundoff > 0.99)) {
            toastr.warning("Value must be between -0.99 and 0.99");
        }
        else {
            updateTotals();
        }
    }, 300));
});

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

function updateProductTotalAmount(that) {

    var row = $(that);
    var productPrice = parseFloat(row.find("#txtproductamount").val());
    var quantity = parseFloat(row.find("#txtproductquantity").val());
    var gst = parseFloat(row.find("#txtgst").val());
    var gstAmount = parseFloat(row.find("#txtgstAmount").val());
    var discount = parseFloat(row.find("#txtdiscountamount").val());

    var totalAmount = (productPrice * quantity) - discount;
    var gstAmount = (totalAmount * gst / 100);
    var productTotalAmount = totalAmount + gstAmount;

    if (!isNaN(gstAmount)) {
        row.find("#txtgstAmount").val(gstAmount.toFixed(2));
    }
    row.find("#txtproducttotalamount").val(productTotalAmount.toFixed(2));
}

function updateDiscount(that) {
    debugger
    var row = $(that);
    var productPrice = parseFloat(row.find("#txtproductamount").val());
    var quantity = parseFloat(row.find("#txtproductquantity").val());
    var discountprice = parseFloat(row.find("#txtdiscountamount").val());
    var discountPercentage = parseFloat(row.find("#txtdiscountpercentage").val());

    if (isNaN(discountprice)) {
        row.find("#txtdiscountamount").val(0);
        row.find("#txtdiscountpercentage").val(0);
        updateProductTotalAmount(row);
        updateTotals();
        return;
    }

    if (discountPercentage == 0 && discountprice > 0) {
        var discountperbyamount = discountprice / productPrice * 100;
        row.find("#txtdiscountpercentage").val(discountperbyamount.toFixed(2));
    } else if (discountprice > 0 && discountPercentage > 0) {
        var discountperbyamount = discountprice / productPrice * 100;
        row.find("#txtdiscountpercentage").val(discountperbyamount.toFixed(2));
    }

    updateProductTotalAmount(row);
    updateTotals();
}

function UpdateDiscountPercentage(that) {
    var row = $(that);
    var productPrice = parseFloat(row.find("#txtproductamount").val());
    var quantity = parseFloat(row.find("#txtproductquantity").val());
    var discountprice = parseFloat(row.find("#txtdiscountamount").val());
    var discountPercentage = parseFloat(row.find("#txtdiscountpercentage").val());

    if (isNaN(discountPercentage)) {
        row.find("#txtdiscountamount").val(0);
        row.find("#txtdiscountpercentage").val(0);
        updateProductTotalAmount(row);
        updateTotals();
        return;
    }

    if (discountprice == 0 && discountPercentage > 0) {
        discountprice = productPrice * discountPercentage / 100;
        row.find("#txtdiscountamount").val(discountprice.toFixed(2));
    } else if (discountprice > 0 && discountPercentage > 0) {
        discountprice = productPrice * discountPercentage / 100;
        row.find("#txtdiscountamount").val(discountprice.toFixed(2));
    }
    updateProductTotalAmount(row);
    updateTotals();
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
        var discountprice = parseFloat(row.find("#txtdiscountamount").val());

        totalSubtotal += subtotal * totalquantity;
        totalGst += gst;
        totalAmount = totalSubtotal + totalGst;
        TotalItemQuantity += totalquantity;
        TotalDiscount += discountprice;
    });

    $("#cart-subtotal").val(totalSubtotal.toFixed(2));
    $("#totalgst").val(totalGst.toFixed(2));
    $("#cart-discount").val(TotalDiscount.toFixed(2));
    $("#TotalDiscountPrice").html(TotalDiscount.toFixed(2));
    $("#TotalProductQuantity").text(TotalItemQuantity);
    $("#TotalProductPrice").html(totalSubtotal.toFixed(2));
    $("#TotalProductGST").html(totalGst.toFixed(2));
    $("#TotalProductAmount").html(totalAmount.toFixed(2));

    if (roundoffvalue != 0) {

        var roundtotal = parseFloat(totalAmount) + parseFloat(roundoffvalue);
        $("#cart-total").val(roundtotal.toFixed(2))
    } else {
        $("#cart-total").val(totalAmount.toFixed(2));
    }
}

function removeItem(element) {
    $(element).closest('tr').remove();
    updateRowNumbers();
    updateTotals();
}
function preventEmptyValue(input) {

    if (input.value === "") {
        input.value = 0;
    }
}
