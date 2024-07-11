﻿$(document).ready(function () {
    $('#VendorDetailsModel').modal('hide');
});

var datas = userPermissions
$(document).ready(function () {
    function data(datas) {
        var userPermission = datas;
        GetAllVendorData(userPermission);
    }
    function GetAllVendorData(userPermission) {
        var userPermissionArray = JSON.parse(userPermission);
        var canEdit = userPermissionArray.some(permission => permission.formName === "Vendor List" && permission.edit);
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
                "data": "vendorCompany", "name": "VendorCompany",
                "render": function (data, type, full) {
                    var profileImageHtml;
                    if (full.vendorCompanyLogo && full.vendorCompanyLogo.trim() !== '') {
                        profileImageHtml = '<img src="/Content/Image/' + full.vendorCompanyLogo + '" style="height: 40px; width: 40px; border-radius: 50%;" ' +
                            'onmouseover="showIcons(event, this.parentElement)" onmouseout="hideIcons(event, this.parentElement)">';
                    } else {
                        var initials = (full.vendorCompany ? full.vendorCompany[0] : '');
                        var randomColor = colorClasses[Math.floor(Math.random() * colorClasses.length)];
                        profileImageHtml = '<div class="flex-shrink-0 avatar-xs me-2">' +
                            '<div class="avatar-title ' + randomColor.bgClass + ' ' + randomColor.textClass + ' rounded-circle" style="height: 40px; width: 40px; border-radius: 50%;">' + initials.toUpperCase() + '</div></div>';
                    }
                    return '<a href="/Vendor/CreateVendor?Vid=' + full.vid + '&viewMode=true" class="link-primary" style="display: flex; align-items: center;">' + profileImageHtml + '<span style="margin-left: 5px;">' + full.vendorCompany + '</span></a>';
                }
            },
            { "data": "vendorFirstName", "name": "VendorFirstName" },
            { "data": "vendorCompanyNumber", "name": "VendorCompanyNumber" },
            { "data": "vendorCompanyEmail", "name": "VendorCompanyEmail" },
        ];

        if (canEdit) {
            columns.push({
                "data": null,
                "orderable": false,
                "searchable": false,
                "render": function (data, type, full) {
                    return '<li class="list-inline-item"><a class="text-primary" href="EditVendorDetails?VId=' + full.vid + '"><i class="fa-regular fa-pen-to-square"></i></a></li>';
                }
            });
        }

        $('#VendorTableData').DataTable({
            processing: false,
            serverSide: true,
            filter: true,
            destroy: true,
            pageLength: 30,
            ajax: {
                type: "Post",
                url: '/Vendor/GetVendorList',
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

function VendorDetails(Id) {
    $.ajax({
        url: '/Vendor/GetVendorDetailsById?VendorId=' + Id,
        type: "get",
        contentType: 'application/json;charset=utf-8;',
        dataType: 'json',
        success: function (response) {
            $('#VendorDetailsModel').modal('show');
            $('#VendorFirstName').text(response.vendorFirstName + " " + response.vendorLastName);
            $('#VendorEmail').text(response.vendorCompanyEmail);
            $('#VendorPhone').text(response.vendorPhone);
            $('#VendorContectNo').text(response.vendorContectNo);
            $('#VendorType').text(response.vendorTypeName);
            $('#VendorCountry').text(response.vendorCountryName);
            $('#VendorState').text(response.vendorStateName);
            $('#VendorCity').text(response.vendorCityName);
            $('#VendorPinCode').text(response.vendorPinCode);
            $('#VendorAddress').text(response.vendorAddress);
            $('#VendorCompany').text(response.vendorCompany);
            $('#VendorCompanyType').text(response.vendorCompanyType);
            $('#VendorCompanyEmail').text(response.vendorEmail);
            $('#VendorCompanyNumber').text(response.vendorCompanyNumber);
            $('#VendorBankAccountNo').text(response.vendorBankAccountNo);
            $('#VendorBankName').text(response.vendorBankName);
            $('#VendorBankBranch').text(response.vendorBankBranch);
            $('#VendorAccountHolderName').text(response.vendorAccountHolderName);
            $('#VendorBankIfsc').text(response.vendorBankIfsc);
            $('#VendorGstnumber').text(response.vendorGstnumber);
        },
        error: function () {
            toastr.error("Can't get Data");
        }
    });
}

//function showCard(cardId) {

//    document.querySelectorAll('.card-body').forEach(function (card) {
//        card.style.display = 'none';
//    });
//    // Show the selected card-body
//    var selectedCard = document.getElementById(cardId);
//    if (selectedCard) {
//        selectedCard.style.display = 'block';

//    }
//}

function EditVendor() {
    document.querySelectorAll('#createVendorform input, #createVendorform select, #createVendorform textarea').forEach(function (element) {
        element.disabled = false;
    });
    document.querySelector('#companylogoInput').disabled = false;
    var deleteButton = document.querySelector('#deleteVendorImageButton');
    if (deleteButton) {
        deleteButton.style.display = 'block';
        deleteButton.disabled = false;
    }
    $("#editVendorbtn").hide();
    $("#updateVendorbtn").show();
}

function UpdateVendorDetails() {
    if ($("#createVendorform").valid()) {
        var formData = new FormData();
        formData.append("Vid", $("#vendorIdInput").val());
        formData.append("VendorFirstName", $("#firstnameInput").val());
        formData.append("VendorLastName", $("#lastnameInput").val());
        formData.append("VendorContectNo", $('#contactnumberInput').val());
        formData.append("VendorPhone", $("#phonenumberInput").val());
        formData.append("VendorEmail", $("#emailidInput").val());
        formData.append("VendorTypeId", $("#ddlVendorType").val());
        formData.append("VendorCountry", $("#VendorCountry").val());
        formData.append("VendorState", $("#VendorState").val());
        formData.append("VendorCity", $("#VendorCity").val());
        formData.append("VendorAddress", $("#AddressidInput").val());
        formData.append("VendorPinCode", $("#pincodeidInput").val());
        formData.append("VendorCompany", $("#companynameInput").val());
        formData.append("VendorCompanyType", $("#CompanyType").val());
        formData.append("VendorCompanyEmail", $("#companyemailInput").val());
        formData.append("VendorCompanyNumber", $("#worknumberInput").val());
        formData.append("VendorBankName", $("#banknameInput").val());
        formData.append("VendorBankBranch", $("#branchInput").val());
        formData.append("VendorAccountHolderName", $("#accountnameInput").val());
        formData.append("VendorBankAccountNo", $("#accountnumberInput").val());
        formData.append("VendorBankIfsc", $("#ifscInput").val());
        formData.append("VendorGstnumber", $("#GSTNumberInput").val());
        formData.append("UpdatedBy", $("#txtUpdatedby").val());
        var imageName = $("#currentVendorImageName").text().trim();
        var imageFile = $("#companylogoInput")[0].files[0];
        if (imageName && !imageFile) {
            formData.append("VendorImageName", imageName);
        } else if (imageFile) {
            formData.append("VendorCompanyLogo", imageFile);
        }
        $.ajax({
            url: '/Vendor/UpdateVendorDetails',
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
                    window.location = '/Vendor/VendorList';
                });
            },
        })
    }
    else {
        toastr.warning("Kindly fill all datafield");
    }
}
function AddVendorDetails() {
    if ($("#createVendorform").valid()) {
        var formData = new FormData();
        formData.append("Vid", $("#vendorIdInput").val());
        formData.append("VendorFirstName", $("#firstnameInput").val());
        formData.append("VendorLastName", $("#lastnameInput").val());
        formData.append("VendorContectNo", $('#contactnumberInput').val());
        formData.append("VendorPhone", $("#phonenumberInput").val());
        formData.append("VendorEmail", $("#emailidInput").val());
        formData.append("VendorTypeId", $("#ddlVendorType").val());
        formData.append("VendorCountry", $("#VendorCountry").val());
        formData.append("VendorState", $("#VendorState").val());
        formData.append("VendorCity", $("#VendorCity").val());
        formData.append("VendorAddress", $("#AddressidInput").val());
        formData.append("VendorPinCode", $("#pincodeidInput").val());
        formData.append("VendorCompany", $("#companynameInput").val());
        formData.append("VendorCompanyType", $("#CompanyType").val());
        formData.append("VendorCompanyEmail", $("#companyemailInput").val());
        formData.append("VendorCompanyNumber", $("#worknumberInput").val());
        formData.append("VendorBankName", $("#banknameInput").val());
        formData.append("VendorBankBranch", $("#branchInput").val());
        formData.append("VendorAccountHolderName", $("#accountnameInput").val());
        formData.append("VendorBankAccountNo", $("#accountnumberInput").val());
        formData.append("VendorBankIfsc", $("#ifscInput").val());
        formData.append("VendorGstnumber", $("#GSTNumberInput").val());
        formData.append("VendorCompanyLogo", $("#companylogoInput")[0].files[0]);


        $.ajax({
            url: '/Vendor/AddVandorDetail',
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
                        window.location = '/Vendor/VendorList';
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

    $("#createVendorform").validate({

        rules: {

            companynameInput: "required",
            contactnumberInput: {
                required: true,
                digits: true,
                minlength: 10,
                maxlength: 10
            },
            phonenumberInput: {
                required: true,
                digits: true,
                minlength: 10,
                maxlength: 10
            },
            emailidInput: {
                required: true,
                email: true
            },
            ddlVendorType: "required",
            VendorCountry: "required",
            VendorState: "required",
            VendorCity: "required",
            AddressidInput: "required",
            pincodeidInput: {
                required: true,
                digits: true,
                minlength: 6,
                maxlength: 6
            },
            firstnameInput: "required",
            lastnameInput: "required",
            CompanyType: "required",
            companyemailInput: {
                required: true,
                email: true
            },
            worknumberInput: {
                required: true,
                digits: true,
                minlength: 10,
                maxlength: 10
            },
            banknameInput: "required",
            branchInput: "required",
            accountnameInput: "required",
            accountnumberInput: "required",
            ifscInput: "required",
            GSTNumberInput: "required",
            banknameInput: "required",
            branchInput: "required",
            accountnameInput: "required",
            accountnumberInput: "required",
            ifscInput: "required",
            GSTNumberInput: "required",
        },
        messages: {
            companynameInput: "Please Enter Company",
            contactnumberInput: {
                required: "Please Enter Contact Number",
                digits: "Contact Number must contain only digits",
                minlength: "Contact Number must be 10 digits long",
                maxlength: "Contact Number must be 10 digits long"
            },
            phonenumberInput: {
                required: "Please Enter phone number",
                digits: "phone number must contain only digits",
                minlength: "phone number must be 10 digits long",
                maxlength: "phone number must be 10 digits long"
            },
            emailidInput: {
                required: "Please Enter Email",
                email: "Please enter a valid email address"
            },
            ddlVendorType: "Please Enter vendorTypeId",
            VendorCity: "Please Enter City",
            VendorState: "Please Enter State",
            VendorCountry: "Please Enter Country",
            AddressidInput: "Please Enter Address",
            pincodeidInput: {
                required: "Please Enter Pincode",
                digits: "Pin code must contain only digits",
                minlength: "Pin code must be 6 digits long",
                maxlength: "Pin code must be 6 digits long"
            },
            firstnameInput: "Please Enter First Name",
            lastnameInput: "Please Enter Last Name",
            CompanyType: "Please Enter CompanyType",
            companyemailInput: {
                required: "Please Enter Company Email",
                email: "Please enter a valid email address"
            },
            worknumberInput: {
                required: "Please Enter work number",
                digits: "Work number must contain only digits",
                minlength: "Work number must be 10 digits long",
                maxlength: "Work number must be 10 digits long"
            },
            banknameInput: "Please Enter BankName",
            branchInput: "Please Enter BankBranch",
            accountnameInput: "Please Enter AccountHolderName",
            accountnumberInput: "Please Enter BankAccountNo",
            ifscInput: "Please Enter BankIfsc",
            GSTNumberInput: "Please Enter Gstnumber",
            banknameInput: "Please Enter BankName",
            branchInput: "Please Enter BankBranch",
            accountnameInput: "Please Enter AccountHolderName",
            accountnumberInput: "Please Enter BankAccountNo",
            ifscInput: "Please Enter BankIfsc",
            GSTNumberInput: "Please Enter Gstnumber",
        }
    })
});

function editnexttoPersonalDetails() {

    if ($('#editVendorFormId').valid()) {
        $("#editPersonalDetailsModel").prop("disabled", false);
        document.getElementById("editPersonalDetailsModel").click()
    }
    else {
        Swal.fire({
            title: "Kindly fill remaining datafield",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        })
    }
}
function editnexttoBankDetails() {

    if ($('#editVendorPersonalDetails').valid()) {
        $("#editBankDetailsModel").prop("disabled", false);
        document.getElementById("editBankDetailsModel").click()
    }
    else {
        Swal.fire({
            title: "Kindly fill remaining datafield",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        })
    }
}

$(document).ready(function () {
    function toggleImagePreview(show) {
        if (show) {
            $('#VendorImageContainer').show();
        } else {
            $('#VendorImageContainer').hide();
        }
    }
    $('#deleteVendorImageButton').click(function () {
        $('#vendorImagePreview').attr('src', '');
        $('#companylogoInput').val('');
        $("#currentVendorImageName").text('');
        toggleImagePreview(false);
    });
    $('#companylogoInput').change(function () {
        var input = this;
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#vendorImagePreview').attr('src', e.target.result);
                toggleImagePreview(true);
            }
            reader.readAsDataURL(input.files[0]);
        } else {
            toggleImagePreview(false);
        }
    });

    if ($('#vendorImagePreview').attr('src') === '' || $('#vendorImagePreview').attr('src') === '#') {
        toggleImagePreview(false);
    } else {
        toggleImagePreview(true);
    }
});