var datas = userPermissions
$(document).ready(function () {
    function data(datas) {
        var userPermission = datas;
        GetAllCompanyData(userPermission);
    }
    function GetAllCompanyData(userPermission) {
        var userPermissionArray = JSON.parse(userPermission);
        var canEdit = userPermissionArray.some(permission => permission.formName === "Company List" && permission.edit);
        var canDelete = userPermissionArray.some(permission => permission.formName === "Company List" && permission.delete);
        var colorClasses = [
            { bgClass: 'bg-primary-subtle', textClass: 'text-primary' },
            { bgClass: 'bg-secondary-subtle', textClass: 'text-secondary' },
            { bgClass: 'bg-success-subtle', textClass: 'text-success' },
            { bgClass: 'bg-info-subtle', textClass: 'text-info' },
            { bgClass: 'bg-warning-subtle', textClass: 'text-warning' },
            { bgClass: 'bg-danger-subtle', textClass: 'text-danger' },
            { bgClass: 'bg-dark-subtle', textClass: 'text-dark' }
        ];

        var columns = [
            {
                "data": "compnyName", "name": "CompanyName",
                "render": function (data, type, full) {
                    var profileImageHtml;
                    if (full.companyLogo && full.companyLogo.trim() !== '') {
                        profileImageHtml = '<img src="/Content/Image/' + full.companyLogo + '" style="height: 40px; width: 40px; border-radius: 50%;" ' +
                            'onmouseover="showIcons(event, this.parentElement)" onmouseout="hideIcons(event, this.parentElement)">';
                    } else {
                        var initials = (full.compnyName ? full.compnyName[0] : '');
                        var randomColor = colorClasses[Math.floor(Math.random() * colorClasses.length)];
                        profileImageHtml = '<div class="flex-shrink-0 avatar-xs me-2">' +
                            '<div class="avatar-title ' + randomColor.bgClass + ' ' + randomColor.textClass + ' rounded-circle" style="height: 40px; width: 40px; border-radius: 50%;">' + initials.toUpperCase() + '</div></div>';
                    }
                    return '<a href="/Company/CreateCompany?CompanyId=' + full.id + '&viewMode=true" class="link-primary" style="display: flex; align-items: center;">' + profileImageHtml + '<span style="margin-left: 5px;">' + full.compnyName + '</span></a>';
                }
            },
            { "data": "contactNumber", "name": "ContactNumber" },
            { "data": "email", "name": "Email" },
            { "data": "address", "name": "Address" },
        ];

        if (canEdit || canDelete) {
            columns.push({
                "data": null,
                "render": function (data, type, full) {
                    var buttons = '';

                    if (canEdit) {
                        buttons +=
                           '<li class="list-inline-item"><a class="text-info" href="EditCompanyDetails?CompanyId=' + full.id + '"><i class="fa-regular fa-pen-to-square"></i></a></li>';
                    }

                    if (canDelete) {
                        buttons += '<a class="btn text-danger btndeletedoc" onclick="deleteCompany(\'' + full.id + '\')">' +
                            '<i class="fas fa-trash"></i></a>';
                    }

                    return buttons;
                }
            });
        }

        $('#CompanyTableData').DataTable({
            processing: false,
            serverSide: true,
            filter: true,
            destroy: true,
            ajax: {
                type: "Post",
                url: '/Company/GetDatatableCompanyList',
                dataType: 'json'
            },
            columns: columns,
            scrollY: 400,
            scrollX: true,
            scrollCollapse: true,
            fixedHeader: {
                header: true,
                footer: true
            },
            autoWidth: false,
            columnDefs: [
                {
                    targets: '_all', width: 'auto'
                }
            ]
        });
    }
    data(datas);
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
        formData.append("CompanyLogo", $("#companylogo")[0].files[0]);

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

$(document).ready(function () {
    var CompanyId = $('#txtcompanyId').val();
    if (CompanyId == "00000000-0000-0000-0000-000000000000") {
        function toggleImagePreview(show) {
            if (show) {
                $('#companyImagePreview').show();
                $('#DisplayCompanyInitials').hide();
                $("#CompanyImageContainer").show();
            } else {
                $('#companyImagePreview').hide();
                $('#DisplayCompanyInitials').show();
                $("#CompanyImageContainer").hide();
            }
        }
    }
    else {
        function toggleImagePreview(show) {
            if (show) {
                $('#companyImagePreview').show();
                $('#DisplayCompanyInitials').hide();
                $("#CompanyImageContainer").show();
            } else {
                $('#companyImagePreview').hide();
                $('#DisplayCompanyInitials').show();
            }
        }
    }


    $('#deleteCompanyImageButton').click(function () {
        $('#companyImagePreview').attr('src', '#').hide();
        $('#companylogo').val('');
        $("#currentCompanyImageName").text('');
        $("#CompanyImageContainer").hide();
        toggleImagePreview(false);
    });

    $('#companylogo').change(function () {
        var input = this;
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#companyImagePreview').attr('src', e.target.result).show();
                toggleImagePreview(true);
            }
            reader.readAsDataURL(input.files[0]);
        } else {
            toggleImagePreview(false);
        }
    });

    if ($('#companyImagePreview').attr('src') === '' || $('#companyImagePreview').attr('src') === '#') {
        toggleImagePreview(false);
    } else {
        toggleImagePreview(true);
    }
});

function EditCompany() {
    document.querySelectorAll('#createCompanyform input, #createCompanyform select, #createCompanyform textarea').forEach(function (element) {
        element.disabled = false;
    });
    document.querySelector('#companylogo').disabled = false;
    var deleteButton = document.querySelector('#deleteCompanyImageButton');
    if (deleteButton) {
        deleteButton.style.display = 'block';
        deleteButton.disabled = false;
    }
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
        formData.append("CompanyLogo", $("#companylogo")[0].files[0]);
        formData.append("UpdatedBy", $("#txtCompanyUpdatedby").val());
        formData.append("Gst", $("#txtcompanygst").val());
        var imageName = $("#currentCompanyImageName").text().trim();
        var imageFile = $("#companylogo")[0].files[0];
        if (imageName && !imageFile) {
            formData.append("CompanyImageName", imageName);
        } else if (imageFile) {
            formData.append("CompanyLogo", imageFile);
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