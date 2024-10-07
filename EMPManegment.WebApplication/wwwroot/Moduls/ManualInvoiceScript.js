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
    $(document).on('input', '#textProductName', function () {
        var productRow = $(this).closest(".product");
    }).on('keydown', '#textProductName', function (event) {
        var productRow = $(this).closest(".product");
        var productFocus = productRow.find('#txtHSNcode');
        handleFocus(event, productFocus);
    });

    $(document).on('input', '#txtHSNcode', function () {
        var productRow = $(this).closest(".product");
    }).on('keydown', '#txtHSNcode', function (event) {
        var productRow = $(this).closest(".product");
        var productFocus = productRow.find('#txtproductquantity');
        handleFocus(event, productFocus);
    });

    $(document).on('input', '#txtproductquantity', function () {
        var productRow = $(this).closest(".product");
        updateProductTotalAmount(productRow);
        updateTotals();
    }).on('keydown', '#txtproductquantity', function (event) {
        var productRow = $(this).closest(".product");
        var productFocus = productRow.find('[id ^= "txtPOProductType_"]');
        handleFocus(event, productFocus);
    });

    $(document).on('input', '[id^="txtPOProductType_"]', function () {
        var productRow = $(this).closest(".product");
    }).on('keydown', '[id^="txtPOProductType_"]', function (event) {
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

    $(document).on('input', '#txtigst', function () {
        var productRow = $(this).closest(".product");
        var gstvalue = $('#txtigst').val();
        if (gstvalue > 100) {
            toastr.warning("IGST% cannot be greater than 100%");
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
        var productAmount = parseFloat($(productRow).find("#txtproductamount").val());

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
    $('#addnewproductlink tr').each(function (index) {
        $(this).find('.product-id').text(index + 1);
        $(this).find('select').attr('id', 'txtPOProductType_' + (index + 1));
    });

    let rowCount = $('#addnewproductlink tr').length;
    ProductTypeDropdown(rowCount);
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
    var igst = parseFloat(row.find("#txtigst").val());
    var gstAmount = parseFloat(row.find("#txtgstAmount").val());
    var discount = parseFloat(row.find("#txtdiscountamount").val());

    var totalAmount = (productPrice * quantity) - discount;
    var igstcount = (totalAmount * igst / 100);
    var gstAmount = (totalAmount * gst / 100) + igstcount;
    var productTotalAmount = totalAmount + gstAmount;

    if (!isNaN(totalAmount)) {
        row.find("#txtproducttotalamount").val(productTotalAmount.toFixed(2));
    }
    else {
        row.find("#txtproducttotalamount").val(0);
    }

    if (!isNaN(gstAmount)) {
        row.find("#txtgstAmount").val(gstAmount.toFixed(2));
    }
    updateTotals();
}
function updateDiscount(that) {
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
updateTotals();
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
        TotalItemQuantity += totalquantity;
        TotalDiscount += discountprice;
        totalAmount = (totalSubtotal + totalGst) - TotalDiscount;
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
function ProductTypeDropdown(rowId) {
    if ($('#txtPOProductType_' + rowId + ' option').length > 1) {
        return;
    }

    $.ajax({
        url: '/ProductMaster/GetProduct',
        success: function (result) {
            var $dropdown = $('#txtPOProductType_' + rowId);
            var selectedValue = $dropdown.val();

            $dropdown.empty();
            $dropdown.append('<option value="">--Select Type--</option>');

            $.each(result, function (i, data) {
                $dropdown.append('<option value="' + data.id + '">' + data.productName + '</option>');
            });
            $dropdown.val(selectedValue);
            if ($dropdown.val() === '') {
                $dropdown.val($("#txtproducttype_" + rowId).val());
            }
        }
    });
}

$(document).ready(function () {
    $("#CreateManualInvoiceForm").validate({
        rules: {
            textVendorName: "required",
            textCompanyName: "required",
            textInvoiceNo: "required",
            textInvoiceDate: "required",
        },
        messages: {
            textVendorName: "Enter Vendor Name",
            textCompanyName: "Enter Company Name",
            textInvoiceNo: "Please Enter InvoiceNo",
            textInvoiceDate: "Please Select Date",
        }
    });
});
function InsertManualInvoiceDetails() {
    if ($("#CreateManualInvoiceForm").valid()) {
        if ($('#addnewproductlink tr').length >= 1) {
            var ProductDetails = [];
            var isValidProduct = true;

            $(".product").each(function () {
                var orderRow = $(this);

                var descriptions = [];
                orderRow.find(".txtManualInvoiceProductDes").each(function () { 
                    var descriptionVal = $(this).val().trim();
                    if (descriptionVal) {
                        descriptions.push(descriptionVal); 
                    }
                });
                
                var objData = {
                    row: orderRow.find("#rowno").text(),
                    Product: orderRow.find("#textProductName").val(),
                    Description: descriptions.join("<br>"),
                    Quantity: orderRow.find("#txtproductquantity").val(),
                    Price: orderRow.find("#txtproductamount").val(),
                    Hsn: orderRow.find("#txtHSNcode").val(),
                    Gst: orderRow.find("#txtgst").val(),
                    IGst: orderRow.find("#txtigst").val(),
                    GstAmount: orderRow.find("#txtgstAmount").val(),
                    Discount: orderRow.find("#txtdiscountamount").val(),
                    DiscountPercent: orderRow.find("#txtdiscountpercentage").val(),
                    ProductTotal: orderRow.find("#txtproducttotalamount").val(),
                    ProductType: orderRow.find('[id^="txtPOProductType_"]').val(),
                };

                if ($("#ddlinvoicetype").val() == 3) {
                    if (!objData.Product.trim() || objData.ProductType == 0) {
                        isValidProduct = false;
                        if (!objData.Product.trim()) {
                            orderRow.find("#textProductName").css("border", "2px solid red");
                        }
                        if (objData.ProductType == 0) {
                            orderRow.find('[id^="txtPOProductType_"]').css("border", "2px solid red");
                        }
                    } else {
                        ProductDetails.push(objData);
                    }
                } else {
                    if (!objData.Product.trim() || objData.Price == 0 || objData.ProductType == 0) {
                        isValidProduct = false;
                        if (!objData.Product.trim()) {
                            orderRow.find("#textProductName").css("border", "2px solid red");
                        }
                        if (objData.Price == 0) {
                            orderRow.find("#txtproductamount").css("border", "2px solid red");
                        }
                        if (objData.ProductType == 0) {
                            orderRow.find('[id^="txtPOProductType_"]').css("border", "2px solid red");
                        }
                    } else {
                        ProductDetails.push(objData);
                    }
                }
            });

            var dateInput = $("#textBuysOrderDate").val();
            if (dateInput === "") {
                dateInput = null;
            }
            if (isValidProduct) {
                var Invoicedetails = {
                    ProjectId: $("#textProjectId").val(),
                    InvoiceNo: $("#textInvoiceNo").val(),
                    InvoiceType: $("#ddlinvoicetype").val(),
                    VendorName: $("#textVendorName").val(),
                    VendorPhoneNo: $("#textVendorMobile").val(),
                    VendorGstNo: $("#textVendorGSTNumber").val(),
                    VendorAddress: $("#textVendorAddress").val(),
                    CompanyName: $("#textCompanyName").val(),
                    CompanyAddress: $("#textCompanyBillingAddress").val(),
                    CompanyGstNo: $("#textCompanyGstNo").val(),
                    TotalGst: $("#totalgst").val(),
                    DispatchDocNo: $("#textDispatchDocNo").val(),
                    Destination: $("#textDestination").val(),
                    MotorVehicleNo: $("#textMotorVehicleNo").val(),
                    SubTotal: $("#cart-subtotal").val(),
                    TotalAmount: $("#cart-total").val(),
                    DispatchThrough: $("#textDispatchThrough").val(),
                    BuyesOrderNo: $("#textBuysOrderNo").val(),
                    BuyesOrderDate: dateInput,
                    InvoiceDate: $("#textInvoiceDate").val(),
                    OrderStatus: $("#UnitTypeId").val(),
                    PaymentMethod: $("#txtpaymentmethod").val(),
                    PaymentStatus: $("#txtpaymenttype").val(),
                    CreatedBy: $("#textCreatedById").val(),
                    RoundOff: $("#cart-roundOff").val(),
                    ShippingAddress: $('#hideShippingAddress').is(':checked') ? $('#textVendorAddress').val() : $('#textShippingAddress').val(),
                    ManualInvoiceDetails: ProductDetails,
                    DollarPrice: $('#DollarAmount').val(),
                };
                var form_data = new FormData();
                form_data.append("ManualInvoiceDetails", JSON.stringify(Invoicedetails));

                $.ajax({
                    url: '/ManualInvoice/InsertManualInvoice',
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
                                window.location = '/ManualInvoice/ManualInvoiceDetails?InvoiceId=' + Result.data;
                            });
                        } else {
                            toastr.error(Result.message);
                        }
                    },
                    error: function (xhr, status, error) {
                        toastr.error("An error occurred while processing your request.");
                    }
                });
            }
            else {
                toastr.warning("Kindly fill all data fields");
            }
        } else {
            toastr.warning("Please select a product!");
            $('#ManualInvoiceAddproduct').addClass('error-border');
        }
    } else {
        toastr.warning("Kindly fill all data fields");
    }
}
var datas = userPermissions
$(document).ready(function () {
    function data(datas) {
        userPermission = datas;
        AllManualInvoiceList(userPermission);
    }
    function AllManualInvoiceList(userPermission) {
        var userPermissionArray = [];
        userPermissionArray = JSON.parse(userPermission);

        var canEdit = false;
        var canDelete = false;

        for (var i = 0; i < userPermissionArray.length; i++) {
            var permission = userPermissionArray[i];
            if (permission.formName == "Manual Invoices") {
                canEdit = permission.edit;
                canDelete = permission.delete;
                break;
            }
        }
        var columns = [
            {
                "data": "invoiceNo",
                "name": "InvoiceNo",
                "render": function (data, type, full) {
                    return '<div class="d-flex justify-content-center"><h5 class="fs-15"><a href="/ManualInvoice/ManualInvoiceDetails?InvoiceId=' + full.id + '" class="fw-medium link-primary text-center">' + full.invoiceNo + '</a></h5></div>';
                }
            },
            { "data": "vendorName", "name": "VendorName", "className": "text-center" },
            { "data": "companyName", "name": "CompanyName", "className": "text-center" },
            { "data": "totalAmount", "name": "TotalAmount", "className": "text-center" }
        ];

        if (canEdit || canDelete) {
            columns.push({
                "data": null,
                "orderable": false,
                "searchable": false,
                "render": function (data, type, full) {
                    var buttons = '<div class="d-flex justify-content-center">';
                    buttons += '<ul class="list-inline mb-0">';

                    if (canEdit) {
                        buttons += '<a href="/ManualInvoice/CreateInvoiceManual?InvoiceId=' + full.id + '" class="btn text-info list-inline-item">' +
                            '<i class="fa-regular fa-pen-to-square"></i></a>';
                    }

                    if (canDelete) {
                        buttons += '<a onclick="deleteManualInvoice(\'' + full.id + '\')" class="btn text-danger btndeletedoc list-inline-item">' +
                            '<i class="fas fa-trash"></i></a>';
                    }

                    buttons += '</ul></div>';
                    return buttons;
                }
            });
        }

        $('#manualInvoiceTable').DataTable({
            processing: false,
            serverSide: true,
            filter: true,
            "bDestroy": true,
            ajax: {
                type: "POST",
                url: '/ManualInvoice/GetManualInvoiceList',
                dataType: 'json'
            },
            columns: columns,
            columnDefs: [{
                "defaultContent": "",
                "targets": "_all"
            }]
        });
    }
    data(datas);
});
function deleteManualInvoice(InvoiceId) {
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
                url: '/ManualInvoice/DeleteManualInvoice?InvoiceId=' + InvoiceId,
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
                            window.location = '/ManualInvoice/ManualInvoices';
                        })
                    } else {
                        toastr.error(Result.message);
                    }
                },
                error: function () {
                    toastr.error("Can't delete Invoice!");
                }
            })
        } else if (result.dismiss === Swal.DismissReason.cancel) {

            Swal.fire(
                'Cancelled',
                'Manual Invoice have no changes.!!😊',
                'error'
            );
        }
    });
}
function UpdateManualInvoiceDetails() {

    if ($("#CreateManualInvoiceForm").valid()) {
        if ($('#addnewproductlink tr').length >= 1) {

            var ProductDetails = [];
            var isValidProduct = true;
            $(".product").each(function () {
                var orderRow = $(this);
                var descriptions = [];
                orderRow.find(".txtManualInvoiceProductDes").each(function () {
                    var descriptionVal = $(this).val().trim();
                    if (descriptionVal) {
                        descriptions.push(descriptionVal);
                    }
                });

                var objData = {
                    RefId: orderRow.find("#textRefId").val(),
                    row: orderRow.find("#rowno").text(),
                    Product: orderRow.find("#textProductName").val(),
                    Description: descriptions.join("<br>"),
                    Quantity: orderRow.find("#txtproductquantity").val(),
                    Price: orderRow.find("#txtproductamount").val(),
                    Hsn: orderRow.find("#txtHSNcode").val(),
                    Gst: orderRow.find("#txtgst").val(),
                    IGst: orderRow.find("#txtigst").val(),
                    GstAmount: orderRow.find("#txtgstAmount").val(),
                    Discount: orderRow.find("#txtdiscountamount").val(),
                    DiscountPercent: orderRow.find("#txtdiscountpercentage").val(),
                    ProductTotal: orderRow.find("#txtproducttotalamount").val(),
                    ProductType: orderRow.find('[id^="txtPOProductType_"]').val(),
                };
                orderRow.find("#textProductName").on('input', function () {
                    $(this).css("border", "1px solid #ced4da");
                });
                orderRow.find("#txtproductamount").on('input', function () {
                    $(this).css("border", "1px solid #ced4da");
                });
                orderRow.find('[id^="txtPOProductType_"]').on('input', function () {
                    $(this).css("border", "1px solid #ced4da");
                });

                if ($("#ddlinvoicetype").val() == 3) {

                    if (!objData.Product.trim() || objData.ProductType == 0) {
                        isValidProduct = false;
                        if (!objData.Product.trim()) {
                            orderRow.find("#textProductName").css("border", "2px solid red");
                        }
                        if (objData.ProductType == 0) {
                            orderRow.find('[id^="txtPOProductType_"]').css("border", "2px solid red");
                        }
                    } else {
                        ProductDetails.push(objData);
                    }
                }
                else {
                    if (!objData.Product.trim() || objData.Price == 0 || objData.ProductType == 0) {
                        isValidProduct = false;
                        if (!objData.Product.trim()) {
                            orderRow.find("#textProductName").css("border", "2px solid red");
                        }
                        if (objData.Price == 0) {
                            orderRow.find("#txtproductamount").css("border", "2px solid red");
                        }
                        if (objData.ProductType == 0) {
                            orderRow.find('[id^="txtPOProductType_"]').css("border", "2px solid red");
                        }
                    } else {
                        ProductDetails.push(objData);
                    }
                }
            });
            var dateInput = $("#textBuysOrderDate1").val();
            if (dateInput === "0001-01-01") {
                dateInput = null;
            }
            if (isValidProduct) {

                var formatedDate = $("#textModelCreatedOn").val(),
                    Createdon = moment.utc(formatedDate, "DD-MM-YYYY HH:mm:ss").local().toDate();

                var Invoicedetails = {
                    Id: $("#textMIid").val(),
                    ProjectId: $("#textProjectId").val(),
                    InvoiceNo: $("#textInvoiceNo").val(),
                    InvoiceType: $("#ddlinvoicetype").val(),
                    VendorName: $("#textVendorName").val(),
                    VendorPhoneNo: $("#textVendorMobile").val(),
                    VendorGstNo: $("#textVendorGSTNumber").val(),
                    VendorAddress: $("#textVendorAddress").val(),
                    CompanyName: $("#textCompanyName").val(),
                    CompanyAddress: $("#textCompanyBillingAddress").val(),
                    CompanyGstNo: $("#textCompanyGstNo").val(),
                    TotalGst: $("#totalgst").val(),
                    DispatchDocNo: $("#textDispatchDocNo").val(),
                    Destination: $("#textDestination").val(),
                    MotorVehicleNo: $("#textMotorVehicleNo").val(),
                    SubTotal: $("#cart-subtotal").val(),
                    TotalAmount: $("#cart-total").val(),
                    DispatchThrough: $("#textDispatchThrough").val(),
                    BuyesOrderNo: $("#textBuysOrderNo").val(),
                    BuyesOrderDate: dateInput,
                    InvoiceDate: $("#textInvoiceDate").val(),
                    OrderStatus: $("#UnitTypeId").val(),
                    PaymentMethod: $("#txtpaymentmethod").val(),
                    PaymentStatus: $("#txtpaymenttype").val(),
                    UpdatedBy: $("#textCreatedById").val(),
                    CreatedBy: $("#textCreatedBy").val(),
                    RoundOff: $("#cart-roundOff").val(),
                    CreatedOn: Createdon,
                    ShippingAddress: $('#hideShippingAddress').is(':checked') ? $('#textVendorAddress').val() : $('#textShippingAddress').val(),
                    ManualInvoiceDetails: ProductDetails,
                    DollarPrice: $('#DollarAmount').val(),
                };
                var form_data = new FormData();
                form_data.append("UpdateManualInvoice", JSON.stringify(Invoicedetails));

                $.ajax({
                    url: '/ManualInvoice/UpdateManualInvoice',
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
                                window.location = '/ManualInvoice/ManualInvoiceDetails?InvoiceId=' + Invoicedetails.Id;
                            });
                        } else {
                            toastr.error(Result.message);
                        }
                    },
                    error: function (xhr, status, error) {
                        toastr.error("An error occurred while processing your request.");
                    }
                });
            }
            else {
                toastr.warning("Kindly fill all data fields");
            }
        } else {
            toastr.warning("Please select a product!");
            $('#ManualInvoiceAddproduct').addClass('error-border');
        }
    } else {
        toastr.warning("Kindly fill all data fields");
    }
}
function AddProductButton() {
    $.ajax({
        url: '/ManualInvoice/DisplayProducts',
        type: 'Post',
        datatype: 'json',
        processData: false,
        contentType: false,
        complete: function (Result) {
            if (Result.statusText === "success") {

                AddNewRow(Result.responseText);
            }
            else if (Result.statusText === "OK") {
                AddNewRow(Result.responseText);
            }
            else {
                toastr.error("Error in display product");
            }
        }
    });
}
function AddNewRow(Result) {
    let rowCount = $('#addnewproductlink tr').length;
    rowCount++;
    $('#addnewproductlink').append(Result);
    updateRowNumbers();
}

function toggleShippingAddress() {
    var checkbox = document.getElementById("hideShippingAddress");
    var shippingAddressFields = document.getElementById("shippingAddressFields");
    if (checkbox.checked) {
        shippingAddressFields.style.display = "none";
    } else {
        shippingAddressFields.style.display = "block";
    }
}

function fn_AddManualInvoiceProductDescription(element) {

    var $row = $(element).closest('tr');
    var itemId = $row.find('#ProductDesBtn').data('item-id');

    var $container = $row.find(`#ProductDescriptionContainer[data-item-id='${itemId}']`);
    var $errorMessage = $row.find('#error-message');

    var lastInput = $container.find('input').last();

    if (lastInput.length > 0 && lastInput.val().trim() === '') {
        $errorMessage.show();
        return;
    } else {
        $errorMessage.hide();
    }

    var newDescriptionRow = `
       <div class="row align-items-center" data-item-id="${itemId}" style="margin-top: 10px;">
           <div class="col-sm-10" style="margin-right:-7px;">
               <input type="text" class="txtManualInvoiceProductDes form-control" placeholder="Description" />
           </div>
           <div class="col-sm-1">
               <div class="text-danger" style="cursor: pointer; font-size:20px;" onclick="removeDescriptionRow(this)">x</div>
           </div>
       </div>
    `;

    $container.append(newDescriptionRow);
}


function removeDescriptionRow(element) {
    var $row = $(element).closest('tr');
    $(element).closest('.row').remove();
    $row.find('#error-message').hide();
}

let dollarModal;

function checkCurrencySelection() {
    const currencySelect = document.getElementById('currency-select');

    if (!dollarModal) {
        dollarModal = new bootstrap.Modal(document.getElementById('DollarModal'));
    }

    if (currencySelect.value === "$") {
        dollarModal.show();
    }
}

function CloseDollarModel() {

    if (dollarModal) {
        dollarModal.hide();
    }
}


function SaveDollarAmount() {

    const dollarAmount = document.getElementById('txtDollarAmount').value;
    
    if (dollarAmount == "") {
        toastr.warning("Emter DollarAmount!");
    }
    else {
        document.getElementById('DollarAmount').value = dollarAmount;
        CloseDollarModel();
    }
}