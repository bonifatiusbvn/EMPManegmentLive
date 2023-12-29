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
$(document).ready(function () {
    $('#txtproductimage').change(function () {
        const [file] = txtproductimage.files;
        if (file) {
            imgPreview.src = URL.createObjectURL(file);
        }
    });

    $("#createproductform").validate({
        rules: {
            txtproductname: "required",
            txtproductdescription: "required",
            txtshortdescription: "required",
            txtproductimage: "required",
            txtstocks: "required",
            txtPerUnitPrice: "required",
            txtHSN: "required",
            txtGST: "required",
            txtPerUnitWithGSTPrice: "required",
        },
        messages: {
            txtproductname: "Please Enter Product Name",
            txtproductdescription: "Please Enter Product Description",
            txtshortdescription: "Please Enter Product Short Description",
            txtproductimage: "Please Enter Product Image",
            txtstocks: "Please Enter Product Stocks",
            txtPerUnitPrice: "Please Enter Per Unit Price of the Product",
            txtHSN: "Please Enter HSN",
            txtGST: "Please Enter GST",
            txtPerUnitWithGSTPrice: "Please Enter Per Unit With GST Price of the Product",
        }
    })
    $('#saveproductdetails').on('click', function () {
        $("#createproductform").validate();
    });
});

function SaveProductDetails()
{ 
    if ($('#createproductform').valid()) {
        var formData = new FormData();
        formData.append("ProductName", $("#txtproductname").val());
        formData.append("ProductDescription", $("#txtproductdescription").val());
        formData.append("ProductShortDescription", $("#txtshortdescription").val());
        formData.append("ProductImage", $("#txtproductimage")[0].files[0]);
        formData.append("ProductStocks", $("#txtstocks").val());
        formData.append("PerUnitPrice", $("#txtPerUnitPrice").val());
        formData.append("Hsn", $("#txtHSN").val());
        formData.append("Gst", $("#txtGST").val());
        formData.append("PerUnitWithGstprice", $("#txtPerUnitWithGSTPrice").val());
        $.ajax({
            url: '/ProductMaster/AddProductDetails',
            type: 'Post',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (Result) {
                debugger
                if (Result.message != null) {
                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/ProductMaster/CreateProduct';
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
