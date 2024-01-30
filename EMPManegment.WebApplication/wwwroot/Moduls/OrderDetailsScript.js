
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

function SaveCreateOrder()
{
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

function SerchProductDetailsById() {debugger
    var GetProductId = {
        Id: $('#searchproductname').val(),
    }
    var form_data = new FormData();
    form_data.append("PRODUCTID", JSON.stringify(GetProductId));debugger
    $.ajax({
        url: '/ProductMaster/DisplayProductDetailsById',
        type: 'Post',
        datatype: 'json',
        data: form_data,
        processData: false,
        contentType: false,
        complete: function (Result) {debugger
            $("#table-product-list-all").hide();
            $("#productdetailsPartial").html(Result.responseText);
        }
    });
}

$(document).ready(function () {debugger
    var rowCounter = 0;

    $("#addItemButton").click(function () {
        debugger
        var newRow = $("#templateRow").clone().removeAttr("style").removeAttr("id");
        debugger
        newRow.find("select").attr("name", "data[" + rowCounter + "].Option");
        debugger
        $("#DisplayProductTable tbody").append(newRow);
        debugger
        rowCounter++;
    });

    // Submit form using AJAX
});

//$(document).ready(function () {
//    var dataArray = [];

//    function displayData() {debugger
//        /*$("#searchproductname").empty();*/

//        for (var i = 0; i < dataArray.length; i++) {debugger
//            $("#searchproductname").append("<li>" + dataArray[i] + "</li>");
//        }
//    }

//    $('#addNewProductBtn').click(function () {debugger
//        var Data = { Id: $('#searchproductname').val(), }
//        debugger
//        dataArray.push(Data);
//        debugger
//        displayData();
//        debugger
//        SerchProductDetailsById();
//    })
//})

//$("body").on("click", "#addNewProductBtn", function () {debugger
//    //Reference the Name and Country TextBoxes.
//    var ProductId = $('#searchproductname').val();
//    debugger
//    //Get the reference of the Table's TBODY element.
//    var tBody = $("#DisplayProductTable > TBODY")[0];
//    debugger
//    //Add Row.
//    var row = tBody.insertRow(-1);
//    debugger
//    //Add Name cell.
//    var cell = $(row.insertCell(-1));
//    cell.html(ProductId.val());
//    debugger
//    //Add Button cell.
//    cell = $(row.insertCell(-1));
//    var btnRemove = $("<input />");
//    btnRemove.attr("type", "button");
//    btnRemove.attr("onclick", "Remove(this);");
//    btnRemove.val("Remove");
//    cell.append(btnRemove);
//    debugger
//    //Clear the TextBoxes.
//    ProductId.val("");
//});

//$("body").on("click", "#addItemButton", function () {
//    //Loop through the Table rows and build a JSON array.
//    var Products = new Array();
//    $("#DisplayProductTable TBODY TR").each(function () {
//        var row = $(this);
//        var product = {};
//        product.ProductName = row.find("TD").eq(0).html();
//        product.Id = row.find("TD").eq(1).html();
//        product.ProductShortDescription = row.find("TD").eq(2).html();
//        product.PerUnitPrice = row.find("TD").eq(3).html();
//        product.PerUnitWithGstprice = row.find("TD").eq(4).html();
//        Products.push(product);
//    });

//    //Send the JSON array to Controller using AJAX.
//    $.ajax({
//        type: "POST",
//        url: "/Home/InsertCustomers",
//        data: JSON.stringify(customers),
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (r) {
//            alert(r + " record(s) inserted.");
//        }
//    });
//});



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