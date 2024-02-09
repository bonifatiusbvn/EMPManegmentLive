$(document).ready(function () {

    GetVendorNameList()
    GetProducts()
    document.getElementById("txtvendorname").click()
    document.getElementById("txtProductList").click()
    document.getElementById("txtProducts").click()
});

function GetVendorNameList() {
    $.ajax({
        url: '/ProductMaster/GetVendorsNameList',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#txtvendorname').append('<Option value=' + data.id + '>' + data.vendorCompany + '</Option>')
            });

            $.each(result, function (i, data) {
                $('#txtvendornamed').append('<Option value=' + data.id + '>' + data.vendorCompany + '</Option>')
            });

        }
    });
}
function selectvendorId() {
    document.getElementById("txtvendorTypeid").value = document.getElementById("txtvendorname").value;
}


function ClearTextBox() {
    $("#txtProductType").val('');
    $("#txtvendorname").val('');
}
function AddProductType() {

    if ($("#addproduct").valid()) {

        var formData = new FormData();
        formData.append("ProductId", $("#txtvendorTypeid").val());
        formData.append("ProductName", $("#txtProductType").val());

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
        });
    }
}


function GetProducts() {

    $.ajax({
        url: '/ProductMaster/GetProduct',
        success: function (result) {

            $.each(result, function (i, data) {
                $('#txtProducts').append('<Option value=' + data.id + '>' + data.productName + '</Option>')
            });
            $.each(result, function (i, data) {
                $('#txtProductList').append('<Option value=' + data.id + '>' + data.productName + '</Option>')
            });
        }
    });
}

function selectProductId() {
    document.getElementById("txtProductListId").value = document.getElementById("txtProductList").value;
}
function selectProductTypeId() {

    document.getElementById("txtProductTypeid").value = document.getElementById("txtProducts").value;
}


$("#txtProductList").change(function () {
    ProductDetailsByProductTypeId()
})
function ProductDetailsByProductTypeId() {
    var form_data = new FormData();
    form_data.append("ProductId", $('#txtProductList').val());
    $.ajax({
        url: '/ProductMaster/GetProductDetailsByProductId',
        type: 'Post',
        datatype: 'json',
        data: form_data,
        processData: false,
        contentType: false,
        complete: function (Result) {
            $("#table-product-list-all").hide();
            $("#dvproductdetails").html(Result.responseText);
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
            txtvendorname: "required",
            txtProducts: "required"
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
            txtvendorname: "Please Select Vendor Name",
            txtProducts: "Please Select Product Type"
        }
    })
    $("#saveproductdetails").on('click', function () {
        $("#createproductform").validate();
    });
});

function SaveProductDetails() {

    if ($('#createproductform').valid()) {

        var formData = new FormData();
        formData.append("ProductName", $("#txtproductname").val());
        formData.append("ProductType", $("#txtProductTypeid").val());
        formData.append("VendorId", $("#txtvendornamed").val());
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

$("#txtvendorname").change(function () {
    ProductDetails()
})
function ProductDetails() {
    var form_data = new FormData();
    form_data.append("VendorId", $('#txtvendorname').val());
    $.ajax({
        url: '/ProductMaster/GetProductDetailsByVendorId',
        type: 'Post',
        datatype: 'json',
        data: form_data,
        processData: false,
        contentType: false,
        complete: function (Result) {
            $("#table-product-list-all").hide();
            $("#dvproductdetails").html(Result.responseText);
        }
    });
}

function EditProductDetails(Id) {
    $.ajax({
        url: '/ProductMaster/EditProductDetails?ProductId=' + Id,
        type: "Get",
        contentType: 'application/json;charset=utf-8;',
        dataType: 'json',
        success: function (response) {
            $('#UpdateProductDetails').modal('show');
            $('#txtProductId').val(response.id);
            $('#txtProductName').val(response.productName);
            $('#txtProductDescription').val(response.productDescription);
            $('#txtPerUnitPrice').val(response.perUnitPrice);
            $('#txtGST').val(response.gst);
            $('#txtPerUnitWithGSTPrice').val(response.perUnitWithGstprice);
            $('#txtHSN').val(response.hsn);
            $('#txtstocks').val(response.productStocks);
            $('#txtProducts').val(response.productType);
            $('#txtshortdescription').val(response.productShortDescription);
            $('#txtvendornamed').val(response.vendorId);
            $('#txtproductImage').val(response.productImage);
        },
        error: function () {
            alert('Data not found');
        }
    });
}

function UpdateProductDetails() {
    if ($('#UpdateDetailsForm').valid()) {
        var formData = new FormData();
        formData.append("Id", $("#txtProductId").val());
        formData.append("ProductName", $("#txtProductName").val());
        formData.append("ProductType", $("#txtProducts").val());
        formData.append("VendorId", $("#txtvendornamed").val());
        formData.append("ProductDescription", $("#txtProductDescription").val());
        formData.append("ProductShortDescription", $("#txtshortdescription").val());
        formData.append("ProductStocks", $("#txtstocks").val());
        formData.append("PerUnitPrice", $("#txtPerUnitPrice").val());
        formData.append("Hsn", $("#txtHSN").val());
        formData.append("Gst", $("#txtGST").val());
        formData.append("PerUnitWithGstprice", $("#txtPerUnitWithGSTPrice").val());
        formData.append("ProductImage", $("#txtproductImage").val());

        $.ajax({
            url: '/ProductMaster/UpdateProductDetails',
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
                        window.location = '/ProductMaster/ProductList';
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

    $("#UpdateDetailsForm").validate({
        rules: {
            txtProductName: "required",
            txtProductDescription: "required",
            txtshortdescription: "required",
            txtstocks: "required",
            txtproductImage: "required",
            txtPerUnitPrice: "required",
            txtHSN: "required",
            txtGST: "required",
            txtPerUnitWithGSTPrice: "required",
            txtvendornamed: "required",
            txtProducts: "required"
        },
        messages: {
            txtProductName: "Please Enter Product Name",
            txtProductDescription: "Please Enter Product Description",
            txtshortdescription: "Please Enter Product Short Description",
            txtstocks: "Please Enter Product Stocks",
            txtPerUnitPrice: "Please Enter Per Unit Price of the Product",
            txtproductImage: "Please Enter Product Image",
            txtHSN: "Please Enter HSN",
            txtGST: "Please Enter GST",
            txtPerUnitWithGSTPrice: "Please Enter Per Unit With GST Price of the Product",
            txtvendornamed: "Please Select Vendor Name",
            txtProducts: "Please Select Product Type"
        }
    })
    $("#UpdateDetailsBtn").on('click', function () {
        $("#UpdateDetailsForm").validate();
    });
});

//$("#txtvendorname").change(function () {
//    SearchProductName()
//})
function SearchProductName() {
    var form_data = new FormData();
    form_data.append("ProductName", $('#txtsearch').val());
    $.ajax({
        url: '/ProductMaster/SearchProductName',
        type: 'Post',
        datatype: 'json',
        data: form_data,
        processData: false,
        contentType: false,
        complete: function (Result) {
            $("#table-product-list-all").hide();
            $("#dvproductdetails").html(Result.responseText);
        }
    });
} 