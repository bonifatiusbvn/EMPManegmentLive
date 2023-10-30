

function EditUserDetails(EmpId) {
   
    
    $.ajax({
       
        url: '/UserDetails/EditUserDetails?Id=' + EmpId,
        type: 'Get',
        contentType: 'application/json;charset=utf-8 ',
        datatype: 'json',
        success: function (response) {debugger
            $('.empmodal').modal('show');
            $('#Userid').val(response.id);
            $('#FirstName').val(response.firstName);
            $('#LastName').val(response.lastName);
            $('#Dob').val(response.dateOfBirth);
            $('#Gender').val(response.gender);
            $('#Email').val(response.email);

            $('#Contry').val(response.countryId);
            $('#ddlCountry').val(response.countryId);

            $('#state').val(response.stateId);
            $('#ddlState').val(response.stateId);

            $('#City').val(response.cityId);
            $('#ddlCity').val(response.cityId);

            $('#deptid').val(response.departmentId);
            $('#ddlDepartmenrnt').val(response.departmentId);

            $('#PhoneNo').val(response.phoneNumber);
            $('#Address').val(response.address);

       
        },
    })
}



function Update() {debugger
    var objData = {
        Id: $('#Userid').val(),
        FirstName: $('#FirstName').val(),
        LastName: $('#LastName').val(),
        DateOfBirth: $('#Dob').val(),
        Gender: $('#Gender').val(),
        Email: $('#Email').val(),
        CountryId: $('#Contry').val(),
        StateId: $('#state').val(),
        CityId: $('#City').val(),
        DepartmentId: $('#deptid').val(),
        PhoneNumber: $('#PhoneNo').val(),
        Address: $('#Address').val(),
    }
    $.ajax({
        url: '/UserDetails/UpdateUserDetails',
        type: 'post',
        data: objData,
        datatype: 'json',
        success: function (Result) {

            Swal.fire({
                title: Result.message,
                icon: 'success',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK'
            }).then(function () {
                window.location = '/UserDetails/UserEditList';
            });
        },
    })

}


//validation

var firstName, lastName, dateOfBirth, email , phoneNumber, address, pincode;
var isValid = true;

$('#btnUpdate').click(function () {debugger
    if (CheckValidation() == false) {
        return false;
    }
});
function CheckValidation() {debugger
    firstName = $('#FirstName').val();
    lastName = $('#LastName').val();
    dateOfBirth = $('#Dob').val();
    gender =$('#Gender').val();
    email = $('#Email').val();
    phoneNumber = $('#PhoneNo').val();
    address = $('#Address').val();
   
    //fname
    if (firstName == "") {debugger
        $('#txtFirstName').text('FirstName can not be blank.');
        $('#FirstName').css('border-color', 'red');
        $('#FirstName').focus();
        isValid = false;
    }
    else {debugger
        $('#txtFirstName').text('');
        $('#FirstName').css('border-color', 'green');
    }
    //lname
    if (lastName == "") {
        debugger
        $('#txtLastName').text('LastName can not be blank.');
        $('#LastName').css('border-color', 'red');
        $('#LastName').focus();
        isValid = false;
    }
    else {
        debugger
        $('#txtLastName').text('');
        $('#LastName').css('border-color', 'green');
    }
    //dob
    if (dateOfBirth == "") {
        debugger
        $('#txtDob').text('DateOfBirth can not be blank.');
        $('#Dob').css('border-color', 'red');
        $('#Dob').focus();
        isValid = false;
    }
    else {
        debugger
        $('#txtDob').text('');
        $('#Dob').css('border-color', 'green');
    }
   
    //email
    if (email == "") {
        debugger
        $('#txtEmail').text('EmailId can not be blank.');
        $('#Email').css('border-color', 'red');
        $('#Email').focus();
        isValid = false;
    }
    else {
        debugger
        $('#txtEmail').text('');
        $('#Email').css('border-color', 'green');
    }
    
    
    
    //phone
    if (phoneNumber == "") {
        debugger
        $('#txtphno').text('PhoneNumber can not be blank.');
        $('#PhoneNo').css('border-color', 'red');
        $('#PhoneNo').focus();
        isValid = false;
    }
    else {
        debugger
        $('#txtphno').text('');
        $('#PhoneNo').css('border-color', 'green');
    }
    //address
    if (address == "") {
        debugger
        $('#txtAddress').text('Address can not be blank.');
        $('#Address').css('border-color', 'red');
        $('#Address').focus();
        isValid = false;
    }
    else {
        debugger
        $('#txtAddress').text('');
        $('#Address').css('border-color', 'green');
    }
    return isValid;
}
//serchbar
$('#txtserch').keyup(function () {
    debugger
    var typevalue = $(this).val();
    $('tbody tr').each(function () {
        if ($(this).text().search(new RegExp(typevalue, "i")) < 0) {
            $(this).hide();
        }
        else {
            $(this).show();
        }
    })
});

  



