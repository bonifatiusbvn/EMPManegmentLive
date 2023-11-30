$(document).ready(function () {
    
    GetAllVendorData();
/*    $("#BuisnessDetailsModel").attr('disabled', 'disabled');*/
    $("#BankDetailsModel").attr('disabled', 'disabled');
   
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
    

    if ($("#VendorbankDetails").valid())
    {
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

        debugger
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

function nexttoBuisnessDetails() {
    debugger
    if ($('#VendorFormId').valid()) {
       
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
function nexttoBankDetails() {
    debugger
    if ($('#Vendorbusiness').valid()) {
       
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

$(document).ready(function () {
    debugger
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
   
});







