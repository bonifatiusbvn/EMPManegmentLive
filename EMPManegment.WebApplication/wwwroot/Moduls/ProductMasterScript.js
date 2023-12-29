$(document).ready(function () {
    GetProducts();

});

function AddProductType() {
    if ($("#createproduct-form").valid()) {
        var formData = new FormData();
        formData.append("ProductType", $("#txtProductType").val());
        $.ajax({
            url: '/ProductMaster/AddProductType',
            type: 'Post',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (Result) {

                if (Result.message == "Product Successfully Inserted") {
                    Swal.fire({
                        title: Result.message,
                        icon: Result.icone,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/ProductMaster/CreateProduct';
                    });
                }
                else {
                    Swal.fire({
                        title: Result.message,
                        icon: Result.icone,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
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


$(document).ready(function(){
    $("#createproduct-form").validate({
        rules: {
            txtProductType: "required"
        },
        messages: {
            txtProductType: "Please Enter Product"
        }
    })
    $('#AddProductTypeButton').on('click', function () {
        $("#createproduct-form").validate();
    });
})

function GetProducts() {

    $.ajax({
        url: '/ProductMaster/GetProduct',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#txtProducts').append('<Option value=' + data.id + '>' + data.productType + '</Option>')
            });
        }
    });
}