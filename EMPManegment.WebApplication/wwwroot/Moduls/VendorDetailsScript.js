$(document).ready(function () {
    GetAllVendorData();
    GetVendorType()
    fn_getState('dropState', 1);
});
function GetAllVendorData() {

    $('#VendorTableData').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "Post",
            url: '/Vendor/GetVendorList',
            dataType: 'json'
        },
        columns: [
            { "data": "vendorCompany", "name": "VendorCompany" },
            { "data": "vendorFirstName", "name": "VendorFirstName" },
            { "data": "vendorEmail", "name": "VendorEmail" },
            { "data": "vendorPhone", "name": "VendorPhone" },
            { "data": "vendorCompanyNumber", "name": "VendorCompanyNumber" },
            { "data": "vendorCompanyEmail", "name": "VendorCompanyEmail" },
            {
                "render": function (data, type, full) {
                    return '<div class="flex-shrink-0 ms-4"><ul class="list-inline tasks-list-menu mb-0"><li class="list-inline-item"><a onclick="VendorDetails(\'' + full.vid + '\')" style=" margin-left: -22px;"><i class="fa-solid fa-eye""></i></a><li class="list-inline-item"><a onclick="EditVendorDetails(\'' + full.vid + '\')"><i class="fas fa-edit"></i></a></li></ul></div>';
                }
            },
        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all",
        }]
    });
}

function VendorDetails(Id) {
    $.ajax({
        url: '/Vendor/GetVendorDetailsById?VendorId=' + Id,
        type: "get",
        contentType: 'application/json;charset=utf-8;',
        dataType: 'json',
        success: function (response) {
            $('#VendorDetailsModel').modal('show');
            $('#VendorFirstName').text(response.vendorFirstName);
            $('#VendorLastName').text(response.vendorLastName);
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
            alert('Data not found');
        }
    });
}

function EditVendorDetails(Id) {

    var editing = true;
    $("#editPersonalDetailsModel").prop("disabled", true);
    $("#editBankDetailsModel").prop("disabled", true);

    $.ajax({
        url: '/Vendor/GetVendorDetailsById?VendorId=' + Id,
        type: "Get",
        contentType: 'application/json;charset=utf-8;',
        dataType: 'json',
        success: function (response) {

            $('#UpdateVendorModel').modal('show');
            $('#vendorId').val(response.vid);
            $('#EditfirstnameInput').val(response.vendorFirstName);
            $('#EditlastnameInput').val(response.vendorLastName);
            $('#EditemailidInput').val(response.vendorEmail);
            $('#EditphonenumberInput').val(response.vendorPhone);
            $('#EditworknumberInput').val(response.vendorContectNo);
            $('#EditVendorTypeInput').val(response.vendorTypeName);
            $('#EditpincodeidInput').val(response.vendorPinCode);
            $('#EditAddressidInput').val(response.vendorAddress);
            $('#EditcompanynameInput').val(response.vendorCompany);
            $('#EditCompanyType').val(response.vendorCompanyType);
            $('#EditcompanyemailInput').val(response.vendorCompanyEmail);
            $('#EditcontactnumberInput').val(response.vendorCompanyNumber);
            $('#EditaccountnumberInput').val(response.vendorBankAccountNo);
            $('#EditbanknameInput').val(response.vendorBankName);
            $('#EditbranchInput').val(response.vendorBankBranch);
            $('#EditaccountnameInput').val(response.vendorAccountHolderName);
            $('#EditifscInput').val(response.vendorBankIfsc);
            $('#EditGSTNumberInput').val(response.vendorGstnumber);
            $("#textVendorType").val(response.vendorTypeId);

            fn_getcitiesbystateId('dropCity', response.vendorState);
            $('#dropState').val(response.vendorState);
            $("#textCountry").val(response.vendorCountry);
            CheckValidation();
            setTimeout(function () { $('#dropCity').val(response.vendorCity); }, 100)
            CheckValidation();
        },
        error: function () {
            alert('Data not found');
        }
    });
}

function UpdateVendorDetails() {

    $("#editPersonalDetailsModel").prop("disabled", true);
    $("#editBankDetailsModel").prop("disabled", true);
    if ($("#editVendorbankDetails").valid()) {
        var fromData = {
            "Vid": $("#vendorId").val(),
            "VendorFirstName": $("#EditfirstnameInput").val(),
            "VendorLastName": $("#EditlastnameInput").val(),
            "VendorEmail": $("#EditemailidInput").val(),
            "VendorPhone": $("#EditphonenumberInput").val(),
            "VendorContectNo": $('#EditworknumberInput').val(),
            "VendorTypeId": $("#textVendorType").val(),
            "VendorCountry": $("#textCountry").val(),
            "VendorState": $("#dropState").val(),
            "VendorCity": $("#dropCity").val(),
            "VendorAddress": $("#EditAddressidInput").val(),
            "VendorPinCode": $("#EditpincodeidInput").val(),
            "VendorCompany": $("#EditcompanynameInput").val(),
            "VendorCompanyType": $("#EditCompanyType").val(),
            "VendorCompanyEmail": $("#EditcompanyemailInput").val(),
            "VendorCompanyNumber": $("#EditcontactnumberInput").val(),
            "VendorBankName": $("#EditbanknameInput").val(),
            "VendorBankBranch": $("#EditbranchInput").val(),
            "VendorAccountHolderName": $("#EditaccountnameInput").val(),
            "VendorBankAccountNo": $("#EditaccountnumberInput").val(),
            "VendorBankIfsc": $("#EditifscInput").val(),
            "VendorGstnumber": $("#EditGSTNumberInput").val()
        };
        $.ajax({
            url: '/Vendor/UpdateVendorDetails',
            type: 'post',
            data: fromData,
            datatype: 'json',
            success: function (Result) {

                Swal.fire({
                    title: Result.message,
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK'
                }).then(function () {
                    window.location = '/Vendor/DisplayVendorList';
                });
            },
        })
    }
    else {
        Swal.fire({
            title: "Kindly fill all datafield",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        })
    }
}

$('#AddVendorModelButton').click(function () {
    ClearTextBox();
});
function ClearTextBox() {
    $("#firstnameInput").val('');
    $("#lastnameInput").val('');
    $("#contactnumberInput").val('');
    $("#phonenumberInput").val('');
    $("#emailidInput").val('');
    $("#ddlVendorType").val('--Select Type--');
    $("#ddlCountry").val('--Select Country--');
    $("#ddlState").val('--Select State--');
    $("#ddlCity").val('--Select City--');
    $("#AddressidInput").val('');
    $("#pincodeidInput").val('');
    $("#companynameInput").val('');
    $("#CompanyType").val('');
    $("#companyemailInput").val('');
    $("#worknumberInput").val('');
    $("#banknameInput").val('');
    $("#branchInput").val('');
    $("#accountnameInput").val('');
    $("#accountnumberInput").val('');
    $("#ifscInput").val('');
    $("#GSTNumberInput").val('');
    $('#dropCity').val('');
    $('#dropState').val('');
    $('#textVendorType').val('');
}
function AddVendorDetails() {

    if ($("#VendorbankDetails").valid()) {
        var fromData = {
            "VendorFirstName": $("#firstnameInput").val(),
            "VendorLastName": $("#lastnameInput").val(),
            "VendorContectNo": $('#contactnumberInput').val(),
            "VendorPhone": $("#phonenumberInput").val(),
            "VendorEmail": $("#emailidInput").val(),
            "VendorTypeId": $("#ddlVendorType").val(),
            "VendorCountry": $("#ddlCountry").val(),
            "VendorState": $("#ddlState").val(),
            "VendorCity": $("#ddlCity").val(),
            "VendorAddress": $("#AddressidInput").val(),
            "VendorPinCode": $("#pincodeidInput").val(),
            "VendorCompany": $("#companynameInput").val(),
            "VendorCompanyType": $("#CompanyType").val(),
            "VendorCompanyEmail": $("#companyemailInput").val(),
            "VendorCompanyNumber": $("#worknumberInput").val(),
            "VendorBankName": $("#banknameInput").val(),
            "VendorBankBranch": $("#branchInput").val(),
            "VendorAccountHolderName": $("#accountnameInput").val(),
            "VendorBankAccountNo": $("#accountnumberInput").val(),
            "VendorBankIfsc": $("#ifscInput").val(),
            "VendorGstnumber": $("#GSTNumberInput").val()
        };


        var from_Data = new FormData();
        from_Data.append("ADDVENDOR", JSON.stringify(fromData));


        $.ajax({
            url: '/Vendor/AddVandorDetail',
            type: 'Post',
            data: from_Data,
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
                    window.location = '/Vendor/DisplayVendorList';
                });

            },
        })
    }
    else {
        Swal.fire({
            title: "Kindly fill all datafield",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        })
    }
}

function GetVendorType() {

    $.ajax({
        url: '/Vendor/GetVendorType',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#ddlVendorType').append('<Option value=' + data.id + '>' + data.vendorType + '</Option>')

            });
        }
    });
}


$('#CloseButton').click(function () {
    window.location = '/Vendor/AddVandorDetails';
});

function openvendormodel() {

    $("#PersonalDetailsModel").prop("disabled", true);
    $("#BankDetailsModel").prop("disabled", true);
    $("#addSeller").modal('show');

}


//-----------------Validation-----------------//

function nexttoPersonalDetails() {

    if ($('#VendorFormId').valid()) {
        $("#PersonalDetailsModel").prop("disabled", false);
        document.getElementById("PersonalDetailsModel").click()
    }
    else {
        Swal.fire({
            title: "Kindly fill all datafield",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        })
    }
}
function nexttoBankDetails() {

    if ($('#VendorPersonalDetails').valid()) {
        $("#BankDetailsModel").prop("disabled", false);
        document.getElementById("BankDetailsModel").click()
    }
    else {
        Swal.fire({
            title: "Kindly fill all datafield",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        })
    }
}

$(document).ready(function () {

    $("#VendorFormId").validate({

        rules: {

            companynameInput: "required",
            contactnumberInput: "required",
            phonenumberInput: "required",
            emailidInput: "required",
            ddlVendorType: "required",
            ddlCountry: "required",
            ddlState: "required",
            ddlCity: "required",
            AddressidInput: "required",
            pincodeidInput: {
                required: true,
                digits: true,
                minlength: 6,
                maxlength: 6
}
        },
        messages: {
            companynameInput: "Please Enter Company",
            contactnumberInput: "Please Enter ContectNo",
            phonenumberInput: "Please Enter Phone",
            emailidInput: "Please Enter Email",
            ddlVendorType: "Please Enter vendorTypeId",
            ddlCountry: "Please Enter Country",
            ddlState: "Please Enter State",
            ddlCity: "Please Enter City",
            AddressidInput: "Please Enter Address",
            pincodeidInput: {
                required: "Please Enter Pincode",
                digits: "Pin code must contain only digits",
                minlength: "Pin code must be 6 digits long",
                maxlength: "Pin code must be 6 digits long"
            },
        }
    })
    $("#VendorPersonalDetails").validate({
        rules: {
            firstnameInput: "required",
            lastnameInput: "required",
            CompanyType: "required",
            companyemailInput: "required",
            worknumberInput: "required",
            companylogoInput: "required",
            banknameInput: "required",
            branchInput: "required",
            accountnameInput: "required",
            accountnumberInput: "required",
            ifscInput: "required",
            GSTNumberInput: "required",
        },
        messages: {
            firstnameInput: "Please Enter FirstName",
            lastnameInput: "Please Enter LastName",
            CompanyType: "Please Enter CompanyType",
            companyemailInput: "Please Enter CompanyEmail",
            worknumberInput: "Please Enter CompanyNumber",
            companylogoInput: "Please Enter CompanyLogo",
            banknameInput: "Please Enter BankName",
            branchInput: "Please Enter BankBranch",
            accountnameInput: "Please Enter AccountHolderName",
            accountnumberInput: "Please Enter BankAccountNo",
            ifscInput: "Please Enter BankIfsc",
            GSTNumberInput: "Please Enter Gstnumber",
        }
    })
    $("#VendorbankDetails").validate({
        rules: {
            banknameInput: "required",
            branchInput: "required",
            accountnameInput: "required",
            accountnumberInput: "required",
            ifscInput: "required",
            GSTNumberInput: "required",
        },
        messages: {
            banknameInput: "Please Enter BankName",
            branchInput: "Please Enter BankBranch",
            accountnameInput: "Please Enter AccountHolderName",
            accountnumberInput: "Please Enter BankAccountNo",
            ifscInput: "Please Enter BankIfsc",
            GSTNumberInput: "Please Enter Gstnumber",
        }
    })

    $("#editVendorFormId").validate({

        rules: {

            EditcompanynameInput: "required",
            EditcontactnumberInput: "required",
            EditphonenumberInput: "required",
            EditemailidInput: "required",
            EditAddressidInput: "required",
            EditpincodeidInput: "required",
        },
        messages: {
            EditcompanynameInput: "Please Enter Company",
            EditcontactnumberInput: "Please Enter ContectNo",
            EditphonenumberInput: "Please Enter Phone",
            EditemailidInput: "Please Enter Email",
            EditddlVendorType: "Please Enter vendorTypeId",
            EditddlCountry: "Please Enter Country",
            EditddlState: "Please Enter State",
            EditddlCity: "Please Enter City",
            EditAddressidInput: "Please Enter Address",
            EditpincodeidInput: "Please Enter PinCode",
        }
    })
    $("#editVendorPersonalDetails").validate({
        rules: {
            EditfirstnameInput: "required",
            EditlastnameInput: "required",
            EditCompanyType: "required",
            EditcompanyemailInput: "required",
            EditworknumberInput: "required",
            EditbanknameInput: "required",
            EditbranchInput: "required",
            EditaccountnameInput: "required",
            EditaccountnumberInput: "required",
            EditifscInput: "required",
            EditGSTNumberInput: "required",
        },
        messages: {
            EditfirstnameInput: "Please Enter FirstName",
            EditlastnameInput: "Please Enter LastName",
            EditCompanyType: "Please Enter CompanyType",
            EditcompanyemailInput: "Please Enter CompanyEmail",
            EditworknumberInput: "Please Enter CompanyNumber",
            EditcompanylogoInput: "Please Enter CompanyLogo",
            EditbanknameInput: "Please Enter BankName",
            EditbranchInput: "Please Enter BankBranch",
            EditaccountnameInput: "Please Enter AccountHolderName",
            EditaccountnumberInput: "Please Enter BankAccountNo",
            EditifscInput: "Please Enter BankIfsc",
            EditGSTNumberInput: "Please Enter Gstnumber",
        }
    })
    $("#editVendorbankDetails").validate({
        rules: {
            EditbanknameInput: "required",
            EditbranchInput: "required",
            EditaccountnameInput: "required",
            EditaccountnumberInput: "required",
            EditifscInput: "required",
            EditGSTNumberInput: "required",
        },
        messages: {
            EditbanknameInput: "Please Enter BankName",
            EditbranchInput: "Please Enter BankBranch",
            EditaccountnameInput: "Please Enter AccountHolderName",
            EditaccountnumberInput: "Please Enter BankAccountNo",
            EditifscInput: "Please Enter BankIfsc",
            EditGSTNumberInput: "Please Enter Gstnumber",
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

    GetVendorCountry();
    GetVendorTypes();
    $('#dropState').change(function () {

        var Text = $("#dropState Option:Selected").text();
        var txtstateid = $(this).val();
        $("#txtstate").val(txtstateid);
    });

    $('#dropCity').change(function () {

        var Text = $("#dropCity Option:Selected").text();
        var txtcityid = $(this).val();
        $("#txtcity").val(txtcityid);
    });

});

function fn_getState(drpstate, countryId, that) {

    var cid = countryId;
    if (cid == undefined || cid == null) {
        var cid = $(that).val();
    }

    $('#' + drpstate).empty();
    $('#' + drpstate).append('<Option >--Select State--</Option>');
    $.ajax({
        url: '/Authentication/GetState?StateId=' + cid,
        success: function (result) {

            $.each(result, function (i, data) {

                $('#' + drpstate).append('<Option value=' + data.id + '>' + data.stateName + '</Option>')
            });
        }
    });
}

function fn_getcitiesbystateId(drpcity, stateid, that) {

    var sid = stateid;
    if (sid == undefined || sid == null) {
        var sid = $(that).val();
    }
    $('#' + drpcity).empty();
    $('#' + drpcity).append('<Option >--Select City--</Option>');
    $.ajax({
        url: '/Authentication/GetCity?CityId=' + sid,
        success: function (result) {

            $.each(result, function (i, data) {

                $('#' + drpcity).append('<Option value=' + data.id + '>' + data.cityName + '</Option>');

            });
        }
    });
}

function GetVendorCountry() {

    $.ajax({
        url: '/Authentication/GetCountrys',
        success: function (result) {
            $.each(result, function (i, data) {

                $('#textCountry').append('<Option value=' + data.id + ' Selected>' + data.countryName + '</Option>')

            });
        }
    });
}

function GetVendorTypes() {

    $.ajax({
        url: '/Vendor/GetVendorType',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#textVendorType').append('<Option value=' + data.id + '>' + data.vendorType + '</Option>')

            });
        }
    });
}



