$(document).ready(function () {
    GetVendorNameList()
    GetProducts()
    GetAllProductDetailsList();
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

    var formData = new FormData();
    formData.append("ProductName", $("#txtProductType").val());

    $.ajax({
        url: '/ProductMaster/AddProductType',
        type: 'Post',
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (Result) {
            if (Result.message == "Product successfully inserted") {
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

    $("#table-product-list-all").hide();
    $("#getallproductlist").hide();
    document.getElementById("txtProductListId").value = document.getElementById("txtProductList").value;
}
function selectProductTypeId() {
    document.getElementById("txtProductTypeid").value = document.getElementById("txtProducts").value;
}


$("#txtProductList").change(function () {
    ProductDetailsByProductTypeId()
})
function ProductDetailsByProductTypeId() {

    var productId = $('#txtProductList').val();
    $.ajax({
        url: '/ProductMaster/GetAllProductList?ProductId=' + productId,
        type: 'Get',
        datatype: 'json',
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
            txtPerUnitPrice: "required",
            txtGST: "required",
            txtGstPerUnit: "required",
        },
        messages: {
            txtproductname: "Please enter product name",
            txtproductdescription: "Please enter product description",
            txtPerUnitPrice: "Please enter price",
            txtGST: "Please enter Gst",
            txtGstPerUnit: "Please enter Gst %",
        }
    })
    $("#saveproductdetails").on('click', function () {
        $("#createproductform").validate();
    });
});

function WithGSTSelected() {
    var isWithGstCheckbox = document.getElementById('txtIsWithGst');
    var gstAmountInput = document.getElementById('txtGstAmount');
    var gstPercentageInput = document.getElementById('txtGstPerUnit');
    var priceInput = document.getElementById('txtPerUnitPrice');

    var price = parseFloat(priceInput.value);
    var gstPercentage = parseFloat(gstPercentageInput.value);

    if (isWithGstCheckbox.checked) {

        if (!isNaN(price) && !isNaN(gstPercentage)) {
            var totalAmount = 100 + gstPercentage;
            var baseAmount = price - (price * gstPercentage / totalAmount);
            var gstAmount = price - baseAmount;
            gstAmountInput.value = gstAmount.toFixed(2);
            priceInput.value = baseAmount.toFixed(2);
        } else {
            gstAmountInput.value = "";
        }
    } else {

        if (!isNaN(price) && !isNaN(gstPercentage)) {
            var Amount = (gstPercentage / 100) * price;
            gstAmountInput.value = Amount.toFixed(2);
        } else {
            gstAmountInput.value = "";
        }
    }
}

document.getElementById('txtGstPerUnit').addEventListener('input', function () {
    WithGSTSelected();
});
document.getElementById('txtPerUnitPrice').addEventListener('input', function () {
    WithGSTSelected();
});
document.getElementById('txtIsWithGst').addEventListener('change', function () {
    WithGSTSelected();
});
function SaveProductDetails() {
    siteloadershow()
    if ($('#createproductform').valid()) {

        var formData = new FormData();
        formData.append("ProductName", $("#txtproductname").val());
        formData.append("ProductType", $("#txtProductTypeid").val());
        formData.append("ProductDescription", $("#txtproductdescription").val());
        formData.append("ProductShortDescription", $("#txtshortdescription").val());
        formData.append("ProductImage", $("#txtproductimage")[0].files[0]);
        formData.append("PerUnitPrice", $("#txtPerUnitPrice").val());
        formData.append("IsWithGst", $("#txtIsWithGst").prop('checked'));
        formData.append("GstPercentage", $("#txtGstPerUnit").val());
        formData.append("GstAmount", $("#txtGstAmount").val());
        formData.append("Hsn", $("#txtHSN").val());
        formData.append("Gst", $("#txtGST").val());
        $.ajax({
            url: '/ProductMaster/AddProductDetails',
            type: 'Post',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (Result) {

                if (Result.code == 200) {
                    siteloaderhide()
                    Swal.fire({
                        title: Result.message,
                        icon: Result.icone,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/ProductMaster/CreateProduct';
                    });
                }
                else {

                    Swal.fire({
                        title: Result.message,
                        icon: Result.icone,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    })

                }
            }
        })
    }
    else {
        siteloaderhide()
        toastr.warning("Kindly fill all details");
    }
}

$("#txtvendorname").change(function () {
    ProductDetails()
})
function ProductDetails() {
    var GetVendorId = {
        VendorId: $('#txtvendorname').val(),
    }
    var form_data = new FormData();
    form_data.append("VendorId", JSON.stringify(GetVendorId));
    $.ajax({
        url: '/ProductMaster/DisplayProductDetailsByVendorId',
        type: 'Post',
        datatype: 'json',
        data: form_data,
        processData: false,
        contentType: false,
        complete: function (Result) {
            $("#getallproductlist").hide();
            $("#dvproductdetails").html(Result.responseText).show();
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
            $('#txtIsWithGst').prop('checked', response.isWithGst);
            $('#txtGstPerUnit').val(response.gstPercentage);
            $('#txtGstAmount').val(response.gstAmount);
            $('#txtHSN').val(response.hsn);
            $('#txtProducts').val(response.productType);
            $('#txtshortdescription').val(response.productShortDescription);
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
        formData.append("ProductDescription", $("#txtProductDescription").val());
        formData.append("ProductShortDescription", $("#txtshortdescription").val());
        formData.append("PerUnitPrice", $("#txtPerUnitPrice").val());
        formData.append("IsWithGst", $("#txtIsWithGst").prop('checked'));
        formData.append("GstPercentage", $("#txtGstPerUnit").val());
        formData.append("GstAmount", $("#txtGstAmount").val());
        formData.append("Hsn", $("#txtHSN").val());
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
            title: "Kindly fill all details",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        })
    }
}
function DeleteProductDetails(ProductId) {
    Swal.fire({
        title: "Are you sure want to delete this product?",
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
                url: '/ProductMaster/DeleteProductDetails?ProductId=' + ProductId,
                type: 'POST',
                dataType: 'json',
                success: function (Result) {

                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/ProductMaster/ProductList';
                    })
                },
                error: function () {
                    Swal.fire({
                        title: "Can't delete product!",
                        icon: 'warning',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/ProductMaster/ProductList';
                    })
                }
            })
        } else if (result.dismiss === Swal.DismissReason.cancel) {

            Swal.fire(
                'Cancelled',
                'Product have no changes.!!😊',
                'error'
            );
        }
    });
}

$(document).ready(function () {

    $("#UpdateDetailsForm").validate({
        rules: {
            txtProductName: "required",
            txtProductDescription: "required",
            txtshortdescription: "required",
            txtproductImage: "required",
            txtPerUnitPrice: "required",
            txtGST: "required",
            txtGstPerUnit: "required",
            txtProducts: "required"
        },
        messages: {
            txtProductName: "Please Enter Product Name",
            txtProductDescription: "Please Enter Product Description",
            txtshortdescription: "Please Enter Product Short Description",
            txtPerUnitPrice: "Please Enter Per Unit Price of the Product",
            txtproductImage: "Please Enter Product Image",
            txtGST: "Please Enter GST",
            txtGstPerUnit: "Please Enter Gst %",
            txtProducts: "Please Select Product Type"
        }
    })
    $("#UpdateDetailsBtn").on('click', function () {
        $("#UpdateDetailsForm").validate();
    });
});


function SearchProductName() {
    var searchValue = $('#txtsearch').val();

    $.ajax({
        url: '/ProductMaster/GetAllProductList?ProductName='+searchValue,
        type: 'Get',
        datatype: 'json',
        complete: function (Result) {
            $("#dvproductdetails").html(Result.responseText);
        }
    });    
}
function sortProductTable() {
    var sortBy = $('#ddlSortBy').val();
    $.ajax({
        url: '/ProductMaster/GetAllProductList?sortBy=' + sortBy,
        type: 'GET',
        
        success: function (result) {
            $("#dvproductdetails").html(result);
        },
        error: function (xhr, status, error) {
            toastr.error(error);
        }
    });
}
function GetAllProductDetailsList(page) {
    $.get("/ProductMaster/GetAllProductList", { page: page})
        .done(function (result) {
            $("#dvproductdetails").html(result);
        })
        .fail(function (error) {
            console.error(error);
        });
}

$(document).ready(function () {
    GetAllProductDetailsList(1, "");
});

$(document).on("click", ".pagination a", function (e) {
    e.preventDefault();
    var page = $(this).data("page") || $(this).attr("href").split('page=')[1]; 
    GetAllProductDetailsList(page);
});


function clearProductImage() {
    var inputFile = document.getElementById('txtproductimage');
    var previewImage = document.getElementById('imgPreview');
    var clearButton = document.querySelector('.btn-danger');

    inputFile.value = '';
    previewImage.src = "/assets/images/no-preview.png";

    clearButton.style.display = 'none';
}

document.addEventListener('DOMContentLoaded', function () {
    var fileInput = document.getElementById('txtproductimage');
    var clearButton = document.querySelector('.btn-danger');
    var previewImage = document.getElementById('imgPreview');

    if (fileInput) {
        fileInput.addEventListener('change', function () {
            if (this.files && this.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    previewImage.src = e.target.result;
                };

                reader.readAsDataURL(this.files[0]);

                clearButton.style.display = 'block';
            } else {
                clearButton.style.display = 'none';
            }
        });
    }
});

function clearSearchInput() {
    var input = document.getElementById('txtsearch');
    input.value = '';
    input.focus();
    GetAllProductDetailsList(1); 
}
