
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
        formData.append("OrderId", $("#orderId").val());
        formData.append("CompanyName", $("#companyname").val());
        formData.append("Product", $("#productname").val());
        formData.append("Quantity", $("#productquantity").val());
        formData.append("Amount", $("#amount").val());
        formData.append("Total", $("#totalamount").val());
        formData.append("OrderDate", $("#orderdate").val());
        formData.append("DeliveryDate", $("#deliverydate").val());
        formData.append("PaymentMethod", $("#payment").val());
        formData.append("DeliveryStatus", $("#deliveredstatus").val());
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
