$(document).ready(function () {
    GetAllVendorData();
});
function GetAllVendorData() {
    $('#VendorTableData').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "Post",
            url: '/Vendor/GetVendorList',
            dataType: 'json'
        },
        columns: [
            { "data": "vendorName", "name": "VendorName", "autowidth": true },
            { "data": "vendorEmail", "name": "VendorEmail", "autowidth": true },
            { "data": "vendorPhone", "name": "VendorPhone", "autowidth": true },
            { "data": "vendorBankAccountNo", "name": "VendorBankAccountNo", "autowidth": true },
            { "data": "vendorBankName", "name": "VendorBankName", "autowidth": true },
            { "data": "vendorBankIfsc", "name": "VendorBankIfsc", "autowidth": true },
            { "data": "vendorGstnumber", "name": "VendorGstnumber", "autowidth": true },
            { "data": "vendorAddress", "name": "VendorAddress", "autowidth": true },
        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all"
        }]
    });
}

function AddVendorDetails() {
    
    if ($('#VendorFormId').valid()) {
        var fromData = new FormData();

        fromData.append("VendorFirstName", $("#firstnameInput").val());
        fromData.append("VendorLastName", $("#lastnameInput").val());
        fromData.append("VendorContectNo", $('#contactnumberInput').val());
        fromData.append("VendorPhone", $("#phonenumberInput").val());
        fromData.append("VendorEmail", $("#emailidInput").val());
        fromData.append("VendorTypeId", $("#ddlVendorType").val());
        fromData.append("VendorCountry", $("#ddlCountry").val());
        fromData.append("VendorState", $("#ddlState").val());
        fromData.append("VendorCity", $("#ddlCity").val());
        fromData.append("VendorAddress", $("#AddressidInput").val());
        fromData.append("VendorPinCode", $("#pincodeidInput").val());
        fromData.append("VendorCompany", $("#companynameInput").val());
        fromData.append("VendorCompanyType", $("#CompanyType").val());
        fromData.append("VendorCompanyEmail", $("#companyemailInput").val());
        fromData.append("VendorCompanyNumber", $("#worknumberInput").val());
        fromData.append("VendorCompanyLogo", $("#companylogoInput")[0].files[0]);
        fromData.append("VendorBankName", $("#banknameInput").val());
        fromData.append("VendorBankBranch", $("#branchInput").val());
        fromData.append("VendorAccountHolderName", $("#accountnameInput").val());
        fromData.append("VendorBankAccountNo", $("#accountnumberInput").val());
        fromData.append("VendorBankIfsc", $("#ifscInput").val());
        fromData.append("VendorGstnumber", $("#GSTNumberInput").val());



        var from_Data = new FormData();

        from_Data.append("ADDVENDOR", JSON.stringify(fromData));
        $.ajax({
            url: '/Vendor/AddVandorDetails',
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
                    window.location = '/Vendor/AddVandorDetails';
                });

            },
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
$(document).ready(function () {
    GetVendorType();
});

$('#CloseButton').click(function () {
    window.location = '/Vendor/AddVandorDetails';
});

//-----------------Validation-----------------//

$(document).ready(function () {
    $("#VendorFormId").validate({
        rules: {
            firstnameInput: "required",
            lastnameInput: "required",
            contactnumberInput: "required",
            phonenumberInput: "required",
            emailidInput: "required",
            ddlVendorType: "required",
            ddlCountry: "required",
            ddlState: "required",
            ddlCity: "required",
            AddressidInput: "required",
            pincodeidInput: "required",
        },
        messages: {
            firstnameInput: "Please Enter FirstName",
            lastnameInput: "Please Enter LastName",
            contactnumberInput: "Please Enter ContectNo",
            phonenumberInput: "Please Enter Phone",
            emailidInput: "Please Enter Email",
            ddlVendorType: "Please Enter vendorTypeId",
            ddlCountry: "Please Enter Country",
            ddlState: "Please Enter State",
            ddlCity: "Please Enter City",
            AddressidInput: "Please Enter Address",
            pincodeidInput: "Please Enter PinCode",
        }
    })
    $("#Vendorbusiness").validate({
        rules: {
            companynameInput: "required",
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
            companynameInput: "Please Enter Company",
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
    //$('#SubmitVendorDetails').on('click', function () {
    //    FirstName = $('#firstnameInput').val();
    //    if (FirstName == "") {
    //        $('#firstnameInput').attr("aria-invalid", "true");
    //        $("label[for='firstnameInput']").addClass('failed');
    //    }
    //    lastname = $('#lastnameInput').val();
    //    if (lastname == "") {
    //        $('#lastnameInput').attr("aria-invalid", "true");
    //        $("label[for='lastnameInput']").addClass('failed');
    //    }
    //    contactnumber = $('#contactnumberInput').val();
    //    if (contactnumber == "") {
    //        $('#contactnumberInput').attr("aria-invalid", "true");
    //        $("label[for='contactnumberInput']").addClass('failed');
    //    }
    //    phonenumber = $('#phonenumberInput').val();
    //    if (phonenumber == "") {
    //        $('#phonenumberInput').attr("aria-invalid", "true");
    //        $("label[for='phonenumberInput']").addClass('failed');
    //    }
    //    emailid = $('#emailidInput').val();
    //    if (emailid == "") {
    //        $('#emailidInput').attr("aria-invalid", "true");
    //        $("label[for='emailidInput']").addClass('failed');
    //    }
    //    VendorType = $('#ddlVendorType').val();
    //    if (VendorType == "") {
    //        $('#ddlVendorType').attr("aria-invalid", "true");
    //        $("label[for='ddlVendorType']").addClass('failed');
    //    }
    //    Country = $('#ddlCountry').val();
    //    if (Country == "") {
    //        $('#ddlCountry').attr("aria-invalid", "true");
    //        $("label[for='ddlCountry']").addClass('failed');
    //    }
    //    State = $('#ddlState').val();
    //    if (State == "") {
    //        $('#ddlState').attr("aria-invalid", "true");
    //        $("label[for='ddlState']").addClass('failed');
    //    }
    //    City = $('#ddlCity').val();
    //    if (City == "") {
    //        $('#ddlCity').attr("aria-invalid", "true");
    //        $("label[for='ddlCity']").addClass('failed');
    //    }
    //    Address = $('#AddressidInput').val();
    //    if (Address == "") {
    //        $('#AddressidInput').attr("aria-invalid", "true");
    //        $("label[for='AddressidInput']").addClass('failed');
    //    }
    //    pincode = $('#pincodeidInput').val();
    //    if (pincode == "") {
    //        $('#pincodeidInput').attr("aria-invalid", "true");
    //        $("label[for='pincodeidInput']").addClass('failed');
    //    }
    //    companyname = $('#companynameInput').val();
    //    if (companyname == "") {
    //        $('#companynameInput').attr("aria-invalid", "true");
    //        $("label[for='companynameInput']").addClass('failed');
    //    }
    //    Company = $('#CompanyType').val();
    //    if (Company == "") {
    //        $('#CompanyType').attr("aria-invalid", "true");
    //        $("label[for='CompanyType']").addClass('failed');
    //    }
    //    companyemail = $('#companyemailInput').val();
    //    if (companyemail == "") {
    //        $('#companyemailInput').attr("aria-invalid", "true");
    //        $("label[for='companyemailInput']").addClass('failed');
    //    }
    //    worknumber = $('#worknumberInput').val();
    //    if (worknumber == "") {
    //        $('#worknumberInput').attr("aria-invalid", "true");
    //        $("label[for='worknumberInput']").addClass('failed');
    //    }
    //    companylogo = $('#companylogoInput').val();
    //    if (companylogo == "") {
    //        $('#companylogoInput').attr("aria-invalid", "true");
    //        $("label[for='companylogoInput']").addClass('failed');
    //    }
    //});
});




//function CheckValidation() {
//    var isValid = true;
//    vendorFirstName = $("#firstnameInput").val();
//    vendorLastName = $("#lastnameInput").val();
//    vendorContectNo = $("#contactnumberInput").val();
//    vendorPhone = $("#phonenumberInput").val();
//    vendorEmail = $("#emailidInput").val();
//    vendorTypeI = $("#ddlVendorType").val();
//    vendorCountry = $("#ddlCountry").val();
//    vendorState = $("#ddlState").val();
//    vendorCity = $('#ddlCity').val();
//    vendorAddress = $('#AddressidInput').val();
//    vendorPinCode = $('#pincodeidInput').val();
//    vendorCompany = $('#companynameInput').val();
//    vendorCompanyType = $('#CompanyType').val();
//    vendorCompanyEmail = $('#companyemailInput').val();
//    vendorCompanyNumber = $('#worknumberInput').val();
//    vendorCompanyLogo = $('#companylogoInput').val();
//    vendorBankName = $('#banknameInput').val();
//    vendorBankBranch = $('#branchInput').val();
//    vendorAccountHolderName = $('#accountnameInput').val();
//    vendorBankAccountNo = $('#accountnumberInput').val();
//    vendorBankIfsc = $('#ifscInput').val();
//    vendorGstnumber = $('#GSTNumberInput').val();

//    //vendorFirstName
//    if (vendorFirstName == "") {
//        $('#Validatefirstname').text('Please Enter FirstName');
//        $('#firstnameInput').css('border-color', 'red');
//        $('#firstnameInput').focus();
//        isValid = false;
//    }
//    else {
//        $('#Validatefirstname').text('');
//        $('#firstnameInput').css('border-color', 'lightgray');

//    }

//    //vendorLastName
//    if (vendorLastName == "") {
//        $('#Validatelastname').text('Please Enter LastName');
//        $('#lastnameInput').css('border-color', 'red');
//        $('#lastnameInput').focus();
//        isValid = false;
//    }

//    else {
//        $('#Validatelastname').text('');
//        $('#lastnameInput').css('border-color', 'lightgray');

//    }

//    //VendorContectNo
//    if (vendorContectNo == "") {
//        $('#Validatecontactnumber').text('Please Enter ContectNo');
//        $('#contactnumberInput').css('border-color', 'red');
//        $('#contactnumberInput').focus();
//        isValid = false;
//    }

//    else {
//        $('#Validatecontactnumber').text('');
//        $('#contactnumberInput').css('border-color', 'lightgray');
//    }

//    //VendorPhone
//    if (vendorPhone == "") {
//        $('#Validatephonenumber').text('Please Enter phonenumber');
//        $('#phonenumberInput').css('border-color', 'red');
//        $('#phonenumberInput').focus();
//        isValid = false;
//    }

//    else {
//        $('#Validatephonenumber').text('');
//        $('#phonenumberInput').css('border-color', 'lightgray');
//    }

//    //VendorEmail
//    if (vendorEmail == "") {
//        $('#Validateemail').text('Please Enter Email');
//        $('#emailidInput').css('border-color', 'red');
//        $('#emailidInput').focus();
//        isValid = false;
//    }

//    else {
//        $('#Validateemail').text('');
//        $('#emailidInput').css('border-color', 'lightgray');
//    }
//    //VendorTypeId
//    if (vendorTypeId == "") {
//        $('#ValidateVendorType').text('Please Enter VendorType');
//        $('#ddlVendorType').css('border-color', 'red');
//        $('#ddlVendorType').focus();
//        isValid = false;
//    }

//    else {
//        $('#ValidateVendorType').text('');
//        $('#ddlVendorType').css('border-color', 'lightgray');
//    }
//    //VendorCountry
//    if (vendorCountry == "") {
//        $('#Validatecountry').text('Please Enter country');
//        $('#ddlCountry').css('border-color', 'red');
//        $('#ddlCountry').focus();
//        isValid = false;
//    }

//    else {
//        $('#Validatecountry').text('');
//        $('#ddlCountry').css('border-color', 'lightgray');
//    }
//    //VendorState
//    if (vendorState == "") {
//        $('#Validatestate').text('Please Enter State');
//        $('#ddlState').css('border-color', 'red');
//        $('#ddlState').focus();
//        isValid = false;
//    }

//    else {
//        $('#Validatestate').text('');
//        $('#ddlState').css('border-color', 'lightgray');
//    }
//    //VendorCity
//    if (vendorCity == "") {
//        $('#Validatecity').text('Please Enter City');
//        $('#ddlCity').css('border-color', 'red');
//        $('#ddlCity').focus();
//        isValid = false;
//    }

//    else {
//        $('#Validatecity').text('');
//        $('#ddlCity').css('border-color', 'lightgray');
//    }
//    //VendorAddress
//    if (vendorAddress == "") {
//        $('#Validateaddress').text('Please Enter Address');
//        $('#AddressidInput').css('border-color', 'red');
//        $('#AddressidInput').focus();
//        isValid = false;
//    }

//    else {
//        $('#Validateaddress').text('');
//        $('#AddressidInput').css('border-color', 'lightgray');
//    }
//    //VendorPinCode
//    if (vendorPinCode == "") {
//        $('#Validatezipcode').text('Please Enter pincode');
//        $('#pincodeidInput').css('border-color', 'red');
//        $('#pincodeidInput').focus();
//        isValid = false;
//    }

//    else {
//        $('#Validatezipcode').text('');
//        $('#pincodeidInput').css('border-color', 'lightgray');
//    }
//    //VendorCompany
//    if (vendorCompany == "") {
//        $('#Validatecompanyname').text('Please Enter Company');
//        $('#companynameInput').css('border-color', 'red');
//        $('#companynameInput').focus();
//        isValid = false;
//    }

//    else {
//        $('#Validatecompanyname').text('');
//        $('#companynameInput').css('border-color', 'lightgray');
//    }
//    //VendorCompanyType
//    if (vendorCompanyType == "") {
//        $('#Validatecompanytype').text('Please Enter CompanyType');
//        $('#CompanyType').css('border-color', 'red');
//        $('#CompanyType').focus();
//        isValid = false;
//    }

//    else {
//        $('#Validatecompanytype').text('');
//        $('#CompanyType').css('border-color', 'lightgray');
//    }
//    //VendorCompanyEmail
//    if (vendorCompanyEmail == "") {
//        $('#Validatecompanyemail').text('Please Enter CompanyEmail');
//        $('#companyemailInput').css('border-color', 'red');
//        $('#companyemailInput').focus();
//        isValid = false;
//    }

//    else {
//        $('#Validatecompanyemail').text('');
//        $('#companyemailInput').css('border-color', 'lightgray');
//    }
//    //VendorCompanyNumber
//    if (vendorCompanyNumber == "") {
//        $('#Validateworknumber').text('Please Enter CompanyNumber');
//        $('#worknumberInput').css('border-color', 'red');
//        $('#worknumberInput').focus();
//        isValid = false;
//    }

//    else {
//        $('#Validateworknumber').text('');
//        $('#worknumberInput').css('border-color', 'lightgray');
//    }
//    //VendorCompanyLogo
//    if (vendorCompanyLogo == "") {
//        $('#Validatecompanylogo').text('Please Enter CompanyLogo');
//        $('#companylogoInput').css('border-color', 'red');
//        $('#companylogoInput').focus();
//        isValid = false;
//    }

//    else {
//        $('#Validatecompanylogo').text('');
//        $('#companylogoInput').css('border-color', 'lightgray');
//    }
//    //VendorBankName
//    if (vendorBankName == "") {
//        $('#Validatebankname').text('Please Enter BankName');
//        $('#banknameInput').css('border-color', 'red');
//        $('#banknameInput').focus();
//        isValid = false;
//    }

//    else {
//        $('#Validatebankname').text('');
//        $('#banknameInput').css('border-color', 'lightgray');
//    }
//    //VendorBankBranch
//    if (vendorBankBranch == "") {
//        $('#Validatebranch').text('Please Enter BankBranch');
//        $('#branchInput').css('border-color', 'red');
//        $('#branchInput').focus();
//        isValid = false;
//    }

//    else {
//        $('#Validatebranch').text('');
//        $('#branchInput').css('border-color', 'lightgray');
//    }
//    //VendorAccountHolderName
//    if (vendorAccountHolderName == "") {
//        $('#Validateaccountname').text('Please Enter AccountHolderName');
//        $('#accountnameInput').css('border-color', 'red');
//        $('#accountnameInput').focus();
//        isValid = false;
//    }

//    else {
//        $('#Validateaccountname').text('');
//        $('#accountnameInput').css('border-color', 'lightgray');
//    }
//    //VendorBankAccountNo
//    if (vendorBankAccountNo == "") {
//        $('#Validateaccountnumber').text('Please Enter BankAccountNo');
//        $('#accountnumberInput').css('border-color', 'red');
//        $('#accountnumberInput').focus();
//        isValid = false;
//    }

//    else {
//        $('#Validateaccountnumber').text('');
//        $('#accountnumberInput').css('border-color', 'lightgray');
//    }
//    //VendorBankIfsc
//    if (vendorBankIfsc == "") {
//        $('#Validateifsc').text('Please Enter BankIfsc');
//        $('#ifscInput').css('border-color', 'red');
//        $('#ifscInput').focus();
//        isValid = false;
//    }

//    else {
//        $('#Validateifsc').text('');
//        $('#ifscInput').css('border-color', 'lightgray');
//    }
//    //VendorGstnumber
//    if (vendorGstnumber == "") {
//        $('#ValidateGST').text('Please Enter Gstnumber');
//        $('#GSTNumberInput').css('border-color', 'red');
//        $('#GSTNumberInput').focus();
//        isValid = false;
//    }

//    else {
//        $('#ValidateGST').text('');
//        $('#GSTNumberInput').css('border-color', 'lightgray');
//    }
//}


