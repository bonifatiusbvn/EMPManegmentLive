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
    
    var objData = {
        VendorName: $("#VendorName").val(),
        VendorEmail: $("#VendorEmail").val(),
        VendorPhone: $("#VendorPhone").val(),
        VendorAddress: $("#VendorAddress").val(),
        VendorBankAccountNo: $("#VendorBankAccountNo").val(),
        VendorBankName: $("#VendorBankName").val(),
        VendorBankIfsc: $("#VendorBankIfsc").val(),
        VendorGstnumber: $('#VendorGstnumber').val(),
        CreatedOn: $('#CreatedOn').val(),
        CreatedBy: $('#CreatedBy').val(),
    }

    $.ajax({
        url: '/Vendor/AddVandorDetails',
        type: 'Post',
        data: objData,
        dataType: 'json',
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
        error: function () {
            toastr.error('There is some problem in your request.');
        }
    })
}


//-------------------Validation--------------------//

/* Validate VendorName*/

$("#name").hide();
let vendornameError = true;
$("#VendorName").keyup(function () {
    validateVendorname();
});

function validateVendorname() {
    let nameValue = $("#VendorName").val();
    if (nameValue.length == "") {
        $("#name").show();
        vendornameError = false;
        return false;
    }
    else {
        $("#name").hide();
    }
}
/* Validate Email */

$("#email").hide();
let emailError = true;
$("#VendorEmail").keyup(function () {
    validateEmail();
});
function validateEmail() {
    let emailValue = $("#VendorEmail").val();
    if (emailValue.length == "") {
        $("#email").show();
        emailError = false;
        return false;
    } 
     else {
        $("#email").hide();
    }
}

//const email = document.getElementById("email");
//email.addEventListener("blur", () => {
//    let regex =
//        /^([_\-\.0-9a-zA-Z]+)@([_\-\.0-9a-zA-Z]+)\.([a-zA-Z]){2,7}$/;
//    let s = email.value;
//    if (regex.test(s)) {
//        email.classList.remove("is-invalid");
//        emailError = true;
//    } else {
//        email.classList.add("is-invalid");
//        emailError = false;
//    }
//});

/* Validate VendorAddress */
$("#address").hide();
let addressError = true;
$("#VendorAddress").keyup(function () {
    validateAddress();
});
function validateAddress() {
    let addressValue = $("#VendorAddress").val();
    if (addressValue.length == "") {
        $("#address").show();
        addressError = false;
        return false;
    }
    else {
        $("#address").hide();
    }
}

/* Validate PhoneNo */
$("#phone").hide();
let phoneError = true;
$("#VendorPhone").keyup(function () {
    validatephone();
});
function validatephone() {
    let phoneValue = $("#VendorPhone").val();
    if (phoneValue.length == "") {
        $("#phone").show();
        phoneError = false;
        return false;
    }
    else {
        $("#phone").hide();
    }
}

/* Validate VendorBankAccountNo */

$("#accountNo").hide();
let bankaccountError = true;
$("#VendorBankAccountNo").keyup(function () {
    validateBankAccountNo();
});
function validateBankAccountNo() {
    let bankaccountValue = $("#VendorBankAccountNo").val();
    if (bankaccountValue.length == "") {
        $("#accountNo").show();
        bankaccountError = false;
        return false;
    }
    else {
        $("#accountNo").hide();
    }
}

/* Validate VendorBankName */

$("#bankName").hide();
let BankNameError = true;
$("#VendorBankName").keyup(function () {
    validateBankName();
});
function validateBankName() {
    let BankNameValue = $("#VendorBankName").val();
    if (BankNameValue.length == "") {
        $("#bankName").show();
        BankNameError = false;
        return false;
    }
    else {
        $("#bankName").hide();
    }
}

/* Validate VendorBankIfsc */

$("#bankIFSC").hide();
let BankIfscError = true;
$("#VendorBankIfsc").keyup(function () {
    validateBankIfsc();
});
function validateBankIfsc() {
    let BankIfscValue = $("#VendorBankIfsc").val();
    if (BankIfscValue.length == "") {
        $("#bankIFSC").show();
        BankIfscError = false;
        return false;
    }
    else {
        $("#bankIFSC").hide();
    }
}

/* Validate VendorGstnumber */

$("#GSTnumber").hide();
let GstnumberError = true;
$("#VendorGstnumber").keyup(function () {
    validateGstnumber();
});
function validateGstnumber() {
    let GstnumberValue = $("#VendorGstnumber").val();
    if (GstnumberValue.length == "") {
        $("#GSTnumber").show();
        GstnumberError = false;
        return false;
    }
    else {
        $("#GSTnumber").hide();
    }
}

/* Validate CreatedOn */

$("#createdOn").hide();
let CreatedOnError = true;
$("#CreatedOn").keyup(function () {
    validateCreatedOn();
});
function validateCreatedOn() {
    let CreatedOnValue = $("#CreatedOn").val();
    if (CreatedOnValue.length == "") {
        $("#createdOn").show();
        CreatedOnError = false;
        return false;
    }
    else {
        $("#createdOn").hide();
    }
}

/* Validate CreatedBy */

$("#createdBy").hide();
let CreatedByError = true;
$("#CreatedBy").keyup(function () {
    validateCreatedBy();
});
function validateCreatedBy() {
    let CreatedByValue = $("#CreatedBy").val();
    if (CreatedByValue.length == "") {
        $("#createdBy").show();
        CreatedByError = false;
        return false;
    }
    else {
        $("#createdBy").hide();
    }
}

/* Validate Submit Button */
$("#SubmitButton").click(function () {
    validateVendorname();
    validateEmail();
    validateAddress();
    validatephone();
    validateBankAccountNo();
    validateBankName();
    validateBankIfsc();
    validateGstnumber();
    validateCreatedOn();
    validateCreatedBy();
    if (
        vendornameError == true &&
        emailError == true &&
        addressError == true &&
        phoneError == true &&
        bankaccountError == true &&
        BankNameError == true &&
        BankIfscError == true &&
        GstnumberError == true &&
        CreatedOnError == true &&
        CreatedByError==true
    ) {
        return true;
    } else {
        return false;
    }
});


//----------------Validation-------------------//


//$(document).ready(function () {
//    $("#SubmitButton").on("click", function () {

//        var VendorName = $('#VendorName').val();
//        var VendorEmail = $('#VendorEmail').val();
//        var VendorPhone = $('#VendorPhone').val();
//        var VendorAddress = $('#VendorAddress').val();
//        var VendorBankAccountNo = $('#VendorBankAccountNo').val();
//        var VendorBankName = $('#VendorBankName').val();
//        var VendorBankIfsc = $('#VendorBankIfsc').val();
//        var VendorGstnumber = $('#VendorGstnumber').val();
//        var CreatedOn = $('#CreatedOn').val();
//        var CreatedBy = $('#CreatedBy').val();
//        // Hiding error messages
//        $('.errorMsg').hide();

//        if (checkVendorname(VendorName) == false) {
//            $('#name').show();
//            return false;
//        } else if (checkEmail(VendorEmail) == false) {
//            $('#email').show();
//            return false;
//        } else if (checkMobileNumber(VendorPhone) == false) {
//            $('#phone').show();
//            return false;
//        } else if (checkAddress(VendorAddress) == false) {
//            $('#address').show();
//            return false;
//        } else if (checkAccountNo(VendorBankAccountNo) == false) {
//            $('#accountNo').show();
//            return false;
//        } else if (checkBankName(VendorBankName) == false) {
//            $('#bankName').show();
//            return false;
//        } else if (checkIFSC(VendorBankIfsc) == false) {
//            $('#bankIFSC').show();
//            return false;
//        } else if (checkGSTnumber(VendorGstnumber) == false) {
//            $('#GSTnumber').show();
//            return false;
//        } else if (checkCreatedOn(CreatedOn) == false) {
//            $('#createdOn').show();
//            return false;
//        } else if (checkCreatedBy(CreatedBy) == false) {
//            $('#createdBy').show();
//            return false;
//        }

//    });
//});

////function used to check valid email
//function checkEmail(email) {
//    //regular expression for email
//    var pattern = new RegExp(/^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i);
//    if (pattern.test(email)) {
//        return true;
//    } else {
//        return false;
//    }
//}

//function checkGSTnumber(url) {
//    //regular expression for GSTnumber
//    var pattern = new RegExp("^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$");

//    if (pattern.test(url)) {
//        return true;
//    } else {
//        return false;
//    }
//}
////function used to validate mobile number
//function checkMobileNumber(mobile) {
//    //regular expression for mobile number
//    var pattern = /^[0-9]{10}$/;
//    if (pattern.test(mobile)) {
//        return true;
//    } else {
//        return false;
//    }
//}
//function checkVendorname() {

//}
//function checkAddress() {

//}
//function checkAccountNo() {

//}
//function checkBankName() {

//}
//function checkIFSC() {

//}
//function checkCreatedOn() {

//}
//function checkCreatedBy() {

//}