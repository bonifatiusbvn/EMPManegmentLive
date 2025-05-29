var Formdata = window.userFormPermissions || 0;

let CompanyGridOptions = [];
$(document).ready(function () {

    CompanyGridOptions = {
        rowHeight: 50,
        columnDefs: [
            {
                headerName: "Company Name",
                field: "compnyName",
                sortable: true,
                filter: true,
                cellRenderer: function (params) {

                    if (!params.data || !params.data.id) {
                        return '';
                    }
                    var profileImageHtml;
                    if (params.data.companyLogo && params.data.companyLogo.trim() !== '') {
                        profileImageHtml = '<img src="/Content/Image/' + params.data.companyLogo + '" style="height: 40px; width: 40px; border-radius: 50%;" ' +
                            'onmouseover="showIcons(event, this.parentElement)" onmouseout="hideIcons(event, this.parentElement)">';
                    } else {
                        var initials = (params.data.compnyName ? params.data.compnyName[0] : '');
                        var randomColor = colorClasses[Math.floor(Math.random() * colorClasses.length)];
                        profileImageHtml = '<div class="flex-shrink-0 avatar-xs me-2">' +
                            '<div class="avatar-title ' + randomColor.bgClass + ' ' + randomColor.textClass + ' rounded-circle" style="height: 40px; width: 40px; border-radius: 50%;">' + initials.toUpperCase() + '</div></div>';
                    }
                    return '<a href="/Company/CreateCompany?CompanyId=' + params.data.id + '&viewMode=true" class="link-primary" style="display: flex; align-items: center;">' + profileImageHtml + '<span style="margin-left: 10px;color: #16989A !important;">' + params.data.compnyName + '</span></a>';
                }
            },
            { headerName: "Contact No", field: "contactNumber", sortable: true, filter: true },
            { headerName: "Email", field: "email", sortable: true, filter: true },
            { headerName: "Address", field: "address", sortable: true, filter: true },
        ],
        defaultColDef: {
            sortable: true,
            filter: true,
            cellClass: 'ag-cell-default-style',
            width: 175,
        },

        rowSelection: 'single',
        rowClassRules: {
            'selected-row': params => params.node.isSelected()
        },
        onGridReady: function (params) {
            CompanyGridOptions.api = params.api;
            CompanyGridOptions.columnApi = params.columnApi;
            CompanyGridOptions.api.sizeColumnsToFit();
        },

        rowModelType: 'infinite',
        cacheBlockSize: 10,
        datasource: {
            getRows: function (params) {

                const request = {
                    StartRow: params.startRow,
                    PageSize: CompanyGridOptions.cacheBlockSize || 10,
                    SearchType: "",
                    SearchValue: "",
                    SortModel: params.sortModel || [],
                    SortColumn: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].colId : "",
                    SortDirection: (params.sortModel && params.sortModel.length > 0) ? params.sortModel[0].sort : "",
                    filters: Object.entries(params.filterModel || {}).map(([key, value]) => ({
                        colId: key,
                        filterValue: value.filter
                    })),
                    searchValue: $('#txtCompanySearch').val(),
                };

                $.ajax({
                    url: '/Company/GetCompanyList',
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
        if (permission.formName === "Company List") {
            canEdit = permission.edit;
            canDelete = permission.delete;
            break;
        }
    }

    if (canEdit || canDelete) {
        CompanyGridOptions.columnDefs.push({
            headerName: "Actions",
            field: "actions",
            sortable: false,
            filter: false,
            cellRenderer: function (params) {

                if (!params.data || !params.data.id) {
                    return '';
                }

                let buttons = '';
                if (canEdit) {
                    buttons += `
                         <li class="list-inline-item"><a href="EditCompanyDetails?CompanyId=${params.data.id}"><i class="fa-regular fa-pen-to-square"></i></a></li>`;
                }

                if (canDelete) {
                    buttons += `
                    <a class="btn text-danger" onclick="deleteCompany('${params.data.id}')"><i class="fas fa-trash"></i></a>`;
                }
                return buttons;
            }
        });
    }

    const myGridElement = document.querySelector('#CompanyTable');
    agGrid.createGrid(myGridElement, CompanyGridOptions);

    $('#txtCompanySearch').on('change keyup', function () {
        CompanyGridOptions.api.onFilterChanged();
    });
});



function AddCompanyDetails() {
    if ($("#createCompanyform").valid()) {
        var formData = new FormData();
        formData.append("Id", $("#txtcompanyId").val());
        formData.append("CompnyName", $("#txtcompanyname").val());
        formData.append("Email", $("#txtcompanyemail").val());
        formData.append("ContactNumber", $('#txtcompanycontactnumber').val());
        formData.append("Country", $("#CompanyCountry").val());
        formData.append("State", $("#CompanyState").val());
        formData.append("City", $("#CompanyCity").val());
        formData.append("PinCode", $("#txtPincode").val());
        formData.append("Address", $("#txtCompanyAddress").val());
        formData.append("Gst", $("#txtcompanygst").val());
        const imgElement = document.getElementById('companylogo');
        const altText = imgElement.alt;
        var file = $("#companylogo").attr("src");
        if (file && file !== "assets/images/new-document.png") {
            var blob = dataURLToBlob(file);
            formData.append("CompanyLogo", blob, altText);
        }

        $.ajax({
            url: '/Company/AddCompany',
            type: 'Post',
            data: formData,
            dataType: 'json',
            processData: false,
            contentType: false,
            success: function (Result) {
                if (Result.code == 200) {
                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/Company/CompanyList';
                    });
                } else {
                    toastr.error(Result.message);
                }

            },
        })
    }
    else {
        toastr.warning("Kindly fill all datafield");
    }
}

function dataURLToBlob(dataURL) {
    var byteString = atob(dataURL.split(',')[1]);
    var mimeString = dataURL.split(',')[0].split(':')[1].split(';')[0];
    var ab = new ArrayBuffer(byteString.length);
    var ia = new Uint8Array(ab);
    for (var i = 0; i < byteString.length; i++) {
        ia[i] = byteString.charCodeAt(i);
    }
    return new Blob([ab], { type: mimeString });
}

function EditCompany() {
    document.querySelectorAll('#createCompanyform input, #createCompanyform select, #createCompanyform textarea').forEach(function (element) {
        element.disabled = false;
    });

    $("#editCompanybtn").hide();
    $("#updateCompanybtn").show();
}

function UpdateCompanyDetails() {
    if ($("#createCompanyform").valid()) {
        var formData = new FormData();
        formData.append("Id", $("#txtcompanyId").val());
        formData.append("CompnyName", $("#txtcompanyname").val());
        formData.append("Email", $("#txtcompanyemail").val());
        formData.append("ContactNumber", $('#txtcompanycontactnumber').val());
        formData.append("Country", $("#CompanyCountry").val());
        formData.append("State", $("#CompanyState").val());
        formData.append("City", $("#CompanyCity").val());
        formData.append("PinCode", $("#txtPincode").val());
        formData.append("Address", $("#txtCompanyAddress").val());
        formData.append("UpdatedBy", $("#txtCompanyUpdatedby").val());
        formData.append("Gst", $("#txtcompanygst").val());
        var imageName = $("#currentCompanyImageName").text().trim();
        const imageFile = document.getElementById('companylogo');
        if (imageName && (imageFile == null)) {
            formData.append("CompanyImageName", imageName);
        }
        else {
            if (imageFile != null) {
                const altImgText = imageFile.alt;
                var file = $("#companylogo").attr("src");
                if (file && file !== "assets/images/new-document.png") {
                    var blob = dataURLToBlob(file);
                    formData.append("CompanyLogo", blob, altImgText);
                }
            }
        }
        $.ajax({
            url: '/Company/UpdateCompanyDetails',
            type: 'POST',
            data: formData,
            dataType: 'json',
            processData: false,
            contentType: false,
            success: function (Result) {

                Swal.fire({
                    title: Result.message,
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK'
                }).then(function () {
                    window.location = '/Company/CompanyList';
                });
            },
        })
    }
    else {
        toastr.warning("Kindly fill all datafield");
    }
}

$(document).ready(function () {

    $("#createCompanyform").validate({
        rules: {

            txtcompanyname: "required",
            txtcompanycontactnumber: {
                required: true,
                digits: true,
                minlength: 10,
                maxlength: 10
            },
            txtcompanyemail: {
                required: true,
                email: true
            },
            CompanyCountry: "required",
            CompanyState: "required",
            CompanyCity: "required",
            txtPincode: "required",
            txtCompanyAddress: "required",
        },
        messages: {
            txtcompanyname: "Please Enter Company Name",
            txtcompanycontactnumber: {
                required: "Please Enter Contact Number",
                digits: "Contact Number must contain only digits",
                minlength: "Contact Number must be 10 digits long",
                maxlength: "Contact Number must be 10 digits long"
            },
            txtcompanyemail: {
                required: "Please Enter Email",
                email: "Please enter a valid email address"
            },
            CompanyCity: "Please Enter City",
            CompanyState: "Please Enter State",
            CompanyCountry: "Please Enter Country",
            txtPincode: "Please Enter Pincode",
            txtCompanyAddress: "Please Enter Address",
        }
    })
});

function deleteCompany(Id) {
    Swal.fire({
        title: "Are you sure want to Delete This?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, Delete it!",
        cancelButtonText: "No, cancel!",
        confirmButtonClass: "btn btn-primary w-xs me-2 mt-2",
        cancelButtonClass: "btn btn-danger w-xs mt-2",
        buttonsStyling: false,
        showCloseButton: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Company/DeleteCompanyDetails?CompanyId=' + Id,
                type: 'POST',
                dataType: 'json',
                success: function (Result) {
                    if (Result.code) {
                        Swal.fire({
                            title: Result.message,
                            icon: 'success',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'OK'
                        }).then(function () {
                            window.location = '/Company/CompanyList';
                        })
                    }
                    else {
                        toastr.error(Result.message);
                    }
                },
                error: function () {
                    Swal.fire({
                        title: "Can't delete Company!",
                        icon: 'warning',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/Company/CompanyList';
                    })
                }
            })
        } else if (result.dismiss === Swal.DismissReason.cancel) {

            Swal.fire(
                'Cancelled',
                'Company have no changes.!!😊',
                'error'
            );
        }
    });
}

$(document).ready(function () {
    const previewContainer = $('#CompanyImage-preview');


    if (previewContainer.children().length > 0) {
        previewContainer.show();
    } else {
        previewContainer.hide();
    }

    $('#fileUpload').on('change', function (event) {
        const files = event.target.files;

        previewContainer.empty();

        if (files.length === 0) {

            previewContainer.hide();
            return;
        }

        previewContainer.show();

        Array.from(files).forEach((file) => {
            const reader = new FileReader();

            reader.onload = function (e) {
                const imageSrc = e.target.result;
                const fileName = file.name;
                const fileSize = (file.size / 1024).toFixed(2) + ' KB';

                const previewTemplate = `
                    <li class="mt-2">
                        <div class="border rounded">
                            <div class="d-flex p-2">
                                <div class="flex-shrink-0 me-3">
                                    <div class="avatar-sm bg-light rounded">
                                        <img src="${imageSrc}" alt="${fileName}" class="img-fluid rounded d-block" id="companylogo"/>
                                    </div>
                                </div>
                                <div class="flex-grow-1">
                                    <div class="pt-1">
                                        <h5 class="fs-14 mb-1">${fileName}</h5>
                                        <p class="fs-13 text-muted mb-0">${fileSize}</p>
                                    </div>
                                </div>
                                <div class="flex-shrink-0 ms-3">
                                    <button class="btn btn-sm btn-primary remove-preview">Delete</button>
                                </div>
                            </div>
                        </div>
                    </li>`;

                previewContainer.append(previewTemplate);
            };

            reader.readAsDataURL(file);
        });
    });

    previewContainer.on('click', '.remove-preview', function () {
        $(this).closest('li').remove();
        $("#currentCompanyImageName").text('');

        if (previewContainer.children().length === 0) {
            previewContainer.hide();
        }
    });
});                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            