
function DeliveryStatus(DeliveryStatus) {

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
            
                    $("#dvdeliveredstatus").html(Result.responseText);;
                } 
     });       
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
});

