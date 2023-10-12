function AddVendorDetails() {
    debugger
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
    })
}





//-------------------Validation--------------------//

/* Validate VendorName*/

$("#fname").hide();
let usernameError = true;
$("#FirstName").keyup(function () {
    validateFirstname();
});

function validateFirstname() {
    let usernameValue = $("#FirstName").val();
    if (usernameValue.length == "") {
        $("#fname").show();
        usernameError = false;
        return false;
    } else if (usernameValue.length < 3 || usernameValue.length > 10) {
        $("#fname").show();
        $("#fname").html("*");
        usernameError = false;
        return false;
    } else {
        $("#fname").hide();
    }
}
//Last Name

$("#lname").hide();
let lastnameError = true;
$("#LastName").keyup(function () {
    validateLastname();
});
function validateLastname() {
    let lastnameValue = $("#LastName").val();
    if (lastnameValue.length == "") {
        $("#lname").show();
        lastnameError = false;
        return false;
    } else if (lastnameValue.length < 3 || lastnameValue.length > 10) {
        $("#lname").show();
        $("#lname").html("*");
        lastnameError = false;
        return false;
    } else {
        $("#lname").hide();
    }
}

//contect no
$("#contect").hide();
let companynumberError = true;
$("#ContectNo").keyup(function () {
    validateCompanyNumber();
});
function validateCompanyNumber() {
    let CnumberValue = $("#ContectNo").val();
    if (CnumberValue.length == "") {
        $("#contect").show();
        companynumberError = false;
        return false;
    } else if (CnumberValue.length == 11) {
        $("#contect").show();
        $("#contect").html("**length of number is 10");
        companynumberError = false;
        return false;
    } else {
        $("#contect").hide();
    }
}

// email
$("#email").hide();
let personalemailError = true;
$("#Email").keyup(function () {
    validatePersonalEmail();
});
function validatePersonalEmail() {
    let pemailValue = $("#Email").val();
    if (pemailValue.length == "") {
        $("#email").show();
        personalemailError = false;
        return false;
    } else if (pemailValue.length < 3 || pemailValue.length > 20) {
        $("#email").show();
        $("#email").html("*");
        personalemailError = false;
        return false;
    } else {
        $("#email").hide();
    }
}

//address

$("#address").hide();
let addressError = true;
$("#Address").keyup(function () {
    validateAddress();
});
function validateAddress() {
    let usernameValue = $("#Address").val();
    if (usernameValue.length == "") {
        $("#address").show();
        usernameError = false;
        return false;
    } else if (usernameValue.length < 3 || usernameValue.length > 10) {
        $("#address").show();
        $("#address").html("*");
        usernameError = false;
        return false;
    } else {
        $("#address").hide();
    }
}

//City

$("#city").hide();
let cityError = true;
$("#City").keyup(function () {
    validatecity();
});
function validatecity() {
    let cityValue = $("#City").val();
    if (cityValue.length == "") {
        $("#city").show();
        cityError = false;
        return false;
    } else if (cityValue.length < 3 || cityValue.length > 10) {
        $("#city").show();
        $("#city").html("*");
        cityError = false;
        return false;
    } else {
        $("#city").hide();
    }
}

//Gender

$("#gender").hide();
let genderError = true;
$("#Gender").keyup(function () {
    validateGender();
});
function validateGender() {
    let genderValue = $("#Gender").val();
    if (genderValue.length == "") {
        $("#gender").show();
        genderError = false;
        return false;
    }
}

//Pincode

$("#pincode").hide();
let pincodeError = true;
$("#Pincode").keyup(function () {
    validatePincode();
});
function validatePincode() {
    let pincodeValue = $("#Pincode").val();
    if (pincodeValue.length == "") {
        $("#pincode").show();
        pincodeError = false;
        return false;
    }
}

////Dob

$("#dob").hide();
let dobError = true;
$("#Dob").keyup(function () {
    validateDob();
});
function validateDob() {
    let dobValue = $("#Dob").val();
    if (dobValue.length == "") {
        $("#dob").show();
        dobError = false;
        return false;
    }
}

// Submit button
$("#AddEmployee").click(function () {
    validateFirstname();
    validateLastname();
    validateCompanyNumber();
    validatePersonalEmail();
    validateAddress();
    validatecity();
    validateGender();
    validatePincode();
    validateDob();
    if (
        usernameError == true &&
        lastnameError == true &&
        companynumberError == true &&
        personalemailError == true &&
        addressError == true &&
        cityError == true &&
        genderError == true &&
        pincodeError == true &&
        dobError == true
    ) {
        return true;
    } else {
        return false;
    }
});