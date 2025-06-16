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
        formData.append("ProductName", $("#txtProductType").val());

        $.ajax({
            url: '/ProductMaster/AddProductType',
            type: 'Post',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (Result) {
                if (Result.code == 200) {
                    Swal.fire({
                        title: Result.message,
                        icon: "success",
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/ProductMaster/CreateProduct';
                    });
                }
                else {
                    toastr.error(Result.message);
                }
            }
        });
    } else {
        toastr.warning("Kindly fill the product type")
    }
}
$(document).ready(function () {
    $('#txtProductTypeList').select2({
        placeholder: 'Select Product Type',
        width: '100%',
        dropdownAutoWidth: true,
        allowClear: true,
        ajax: {
            url: '/ProductMaster/GetProduct',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                return {
                    results: data.map(item => ({
                        id: item.id,
                        text: item.productName
                    }))
                };
            }
        }
    });
    $('#txtProducts').select2({
        placeholder: 'Select Product Type',
        width: '100%',
        dropdownAutoWidth: true,
        allowClear: true,
        dropdownParent: $('#UpdateProductDetails'), 
        ajax: {
            url: '/ProductMaster/GetProduct',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                return {
                    results: data.map(item => ({
                        id: item.id,
                        text: item.productName
                    }))
                };
            }
        }
    });
})

function selectProductTypeId() {
    document.getElementById("txtProductTypeidHidden").value = document.getElementById("txtProducts").value;
}

$(document).ready(function () {

    $("#createproductform").validate({
        rules: {
            txtproductname: "required",
            txtproductdescription: "required",
            txtPerUnitPrice: "required",
            txtGST: "required",
            txtGstPerUnit: "required",
            txtshortdescription: {
                maxlength: 50
            }
        },
        messages: {
            txtproductname: "Please enter product name",
            txtproductdescription: "Please enter product description",
            txtPerUnitPrice: "Please enter price",
            txtGST: "Please enter Gst",
            txtGstPerUnit: "Please enter Gst %",
            txtshortdescription: {
                maxlength: "Short description cannot exceed 50 characters"
            }
        }
    })
    $("#saveproductdetails").on('click', function () {
        $("#createproductform").validate();
    });

    $("#addproduct").validate({
        rules: {
            txtProductType: "required"
        },
        messages: {
            txtProductType: "Enter Product Type"
        }
    })
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
        formData.append("ProductType", $("#txtProductTypeidHidden").val());
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
                        icon: "success",
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/ProductMaster/ProductList';
                    });
                }
                else {
                    toastr.error(Result.message);
                }

            }
        })
    }
    else {
        siteloaderhide()
        toastr.warning("Kindly fill all datafield");
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
            //$('#txtProducts').val(response.productTypeName);
            //$('#txtProductTypeidHidden').val(response.productType);
            $('#txtshortdescription').val(response.productShortDescription);
            $('#txtproductImage').val(response.productImage);

            setSelectedProductType(response.productType, response.productTypeName);
        },
        error: function () {
            toastr.error("Can't get Data");
        }
    });
}
function setSelectedProductType(productType, productTypeName) {
    if (!productType || productTypeName === "null") {
        return;
    }

    const $dropdown = $('#txtProducts');

    $dropdown.empty();

    const newOption = new Option(productTypeName, productType, true, true);
    $dropdown.append(newOption).trigger('change');
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
        formData.append("UpdatedBy", $("#txtUpdatedby").val());

        $.ajax({
            url: '/ProductMaster/UpdateProductDetails',
            type: 'Post',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (Result) {
                if (Result.code == 200) {
                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/ProductMaster/ProductList';
                    });
                }
                else {
                    toastr.error(Result.message);
                }
            }
        })
    }
    else {
        toastr.warning("Kindly fill all datafield");
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
                    if (Result.code == 200) {
                        Swal.fire({
                            title: Result.message,
                            icon: 'success',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'OK'
                        }).then(function () {
                            window.location = '/ProductMaster/ProductList';
                        })
                    } else {
                        toastr.error(Result.message);
                    }
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
            txtGST: "Please Enter GST",
            txtGstPerUnit: "Please Enter Gst %",
            txtProducts: "Please Select Product Type"
        }
    })
    $("#UpdateDetailsBtn").on('click', function () {
        $("#UpdateDetailsForm").validate();
    });
});


//function SearchProductName() {
//    var searchValue = $('#txtsearch').val();

//    $.ajax({
//        url: '/ProductMaster/GetAllProductList?ProductName=' + searchValue,
//        type: 'Get',
//        datatype: 'json',
//        complete: function (Result) {
//            $("#dvproductdetails").html(Result.responseText);
//        }
//    });
//}

//function selectProductId() {
//    var ProductId = document.getElementById("txtProductTypeHidden").value;
//    $('#ddlSortBy').val("");
//    GetAllProductDetailsList(1, null, ProductId);
//}

//function sortProductTable() {
//    var sortBy = $('#ddlSortBy').val();
//    document.getElementById("txtProductList").value = "";
//    GetAllProductDetailsList(1, sortBy, null);
//}
//function GetAllProductDetailsList(page, sortBy, ProductId) {
//    $.get("/ProductMaster/GetAllProductList", { page: page, sortBy: sortBy, ProductId: ProductId })
//        .done(function (result) {
//            $("#dvproductdetails").html(result);
//        })
//        .fail(function (error) {
//            console.error(error);
//        });
//}

//$(document).ready(function () {
//    GetAllProductDetailsList(1, null, null);
//});
//$(document).on("click", ".pagination a", function (e) {
//    e.preventDefault();
//    var page = $(this).data("page") || $(this).attr("href").split('page=')[1];
//    var sortBy = $('#ddlSortBy').val() || null;
//    var ProductId = document.getElementById("txtProductList").value || null;
//    GetAllProductDetailsList(page, sortBy, ProductId);
//});
//$(document).on("click", "#backButton", function (e) {
//    e.preventDefault();
//    var page = $(this).text();
//    var sortBy = $('#ddlSortBy').val() || null;
//    var ProductId = document.getElementById("txtProductList").value || null;
//    GetAllProductDetailsList(page, sortBy, ProductId);
//});


//function clearSearchInput() {
//    var input = document.getElementById('txtsearch');
//    input.value = '';
//    input.focus();
//    GetAllProductDetailsList(1);
//}

var Formdata = window.userFormPermissions || 0;

let ProductGridOptions = [];
$(document).ready(function () {
    ProductGridOptions = {
        rowHeight: 130,
        headerHeight: 45,
        suppressCellFocus: true,
        suppressRowClickSelection: true,
        animateRows: true,
        rowStyle: { 'border-bottom': '1px solid rgba(22, 152, 154, 0.05)' },
        columnDefs: [
            {
                headerName: "PRODUCT DETAILS",
                field: "productName",
                sortable: true,
                filter: true,
                flex: 3,
                minWidth: 350,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.id) return '';

                    return `
                        <div class="product-card-modern d-flex align-items-center">
                            <div class="flex-shrink-0 me-3 position-relative">
                                <div class="product-image-container">
                                    <img src="/${params.data.productImage}" alt="${params.data.productName}" 
                                         class="img-fluid h-100 w-100" />
                                    <span class="product-badge">NEW</span>
                                </div>
                            </div>
                            <div class="flex-grow-1 product-details" style="text-align: start;">
                                <h5 class="fs-15 mb-2">
                                    <a href="/ProductMaster/ProductDetails/?ProductId=${params.data.id}" 
                                       class="text-decoration-none" style="color: #16989A; font-weight:600;">
                                        ${params.data.productName}
                                    </a>
                                </h5>
                                <span class="product-category">${params.data.productTypeName}</span>
                                <p class="product-description mb-0">
                                    ${params.data.productShortDescription}
                                </p>
                            </div>
                        </div>
                    `;
                }
            },
            {
                headerName: "PRICE",
                field: "perUnitPrice",
                sortable: true,
                filter: true,
                flex: 1,
                minWidth: 120,
                cellRenderer: function (params) {
                    return params.value ? `<div class="product-price">$${params.value}</div>` : '';
                }
            },
        ],
        defaultColDef: {
            sortable: true,
            filter: true,
            resizable: true,
            cellClass: 'ag-cell-default-style',
        },
        rowSelection: 'single',
        rowClassRules: {
            'selected-row': params => params.node.isSelected()
        },
        onGridReady: function (params) {
            ProductGridOptions.api = params.api;
            ProductGridOptions.columnApi = params.columnApi;
            params.api.sizeColumnsToFit();
        },
        rowModelType: 'infinite',
        cacheBlockSize: 10,
        datasource: {
            getRows: function (params) {
                const request = {
                    StartRow: params.startRow,
                    PageSize: ProductGridOptions.cacheBlockSize || 10,
                    SearchType: "",
                    SearchValue: "",
                    SortModel: params.sortModel || [],
                    SortColumn: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].colId : "",
                    SortDirection: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].sort : "",
                    filters: Object.entries(params.filterModel || {}).map(([key, value]) => ({
                        colId: key,
                        filterValue: value.filter
                    })),
                    searchValue: $('#txtProductSearch').val(),
                    ProductTypeFilter: $('#txtProductTypeList').val(),
                };

                $.ajax({
                    url: '/ProductMaster/GetAllProductList',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(request),
                    success: function (response) {
                        params.successCallback(response.rowsThisPage, response.totalRowCount);
                    },
                    error: function () {
                        params.failCallback();
                    }
                });
            }
        }
    };

    const userPermissionArray = Formdata;
    let canEdit = false;
    let canDelete = false;

    for (let i = 0; i < userPermissionArray.length; i++) {
        const permission = userPermissionArray[i];
        if (permission.formName === "Product List") {
            canEdit = permission.edit;
            canDelete = permission.delete;
            break;
        }
    }

    if (canEdit || canDelete) {
        ProductGridOptions.columnDefs.push({
            headerName: "ACTIONS",
            field: "actions",
            sortable: false,
            filter: false,
            flex: 1,
            minWidth: 120,
            cellRenderer: function (params) {
                if (!params.data || !params.data.id) return '';

                let buttons = '<div class="action-buttons-modern">';
                if (canEdit) {
                    buttons += `
                        <div onclick="EditProductDetails('${params.data.id}')" title="Edit" style="color: #16989A;">
                            <i class="fa-regular fa-pen-to-square fs-16"></i>
                        </div>`;
                }
                if (canDelete) {
                    buttons += `
                        <div onclick="DeleteProductDetails('${params.data.id}')" title="Delete" style="color: #16989A;margin-left:5px;">
                            <i class="fas fa-trash fs-16"></i>
                        </div>`;
                }
                buttons += '</div>';
                return buttons;
            }
        });
    }

    const myGridElement = document.querySelector('#ProductTable');
    agGrid.createGrid(myGridElement, ProductGridOptions);

    $('#txtProductSearch').on('change keyup', function () {
        ProductGridOptions.api.onFilterChanged();
    });

    $('#txtProductTypeList').change(() => {
        const productText = $("#txtProductTypeList option:selected").text();
        $("#txtProductTypeName").val(productText === 'All Product' ? '' : productText);
        if (ProductGridOptions.api) {
            ProductGridOptions.api.onFilterChanged();
        }
    });
});

$(document).ready(function () {
    function toggleProductImagePreview(show) {
        if (show) {
            $('#ProductImageContainer').show();
        } else {
            $('#ProductImageContainer').hide();
        }
    }
    $('#deleteProductImageButton').click(function () {
        $('#productImagePreview').attr('src', '');
        $('#txtproductimage').val('');
        toggleProductImagePreview(false);
    });
    $('#txtproductimage').change(function () {
        var input = this;
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#productImagePreview').attr('src', e.target.result);
                toggleProductImagePreview(true);
            }
            reader.readAsDataURL(input.files[0]);
        } else {
            toggleProductImagePreview(false);
        }
    });

    if ($('#productImagePreview').attr('src') === '' || $('#productImagePreview').attr('src') === '#') {
        toggleProductImagePreview(false);
    } else {
        toggleProductImagePreview(true);
    }
});