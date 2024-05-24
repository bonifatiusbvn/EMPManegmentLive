
$("#departmentbox").hide();
$("#backbtn").hide();

$(document).ready(function () {
    GetUserAttendanceInTime();
    UserBirsthDayWish();
    GetAllUserData();
    clearSelectedBox();
    //GetUserRoleList();
    //GetDepartment();

    $("#activeInactiveForm").validate({
        rules: {
            ddlusername: "required",
            ddlDepartmenrnt: "required"
        },
        messages: {
            ddlusername: "Please Select UserName",
            ddlDepartmenrnt: "Please Select Department"
        }
    })
    $('#searchEmployeeform').on('click', function () {
        $("#activeInactiveForm").validate();
    });
    $("#frmuserDetails").validate({
        rules: {
            firstnameInput: "required",
            lastnameInput: "required",
            birthdateInput: "required",
            genderInput: "required",
            emailInput: "required",
            phonenumberInput: "required",
            addressInput: "required",
        },
        messages: {
            firstnameInput: "Please Enter FirstName",
            lastnameInput: "Please Enter LastName",
            birthdateInput: "Please Enter DateOfBirth",
            genderInput: "Please Enter Gender",
            emailInput: "Please Enter Email",
            phonenumberInput: "Please Enter PhoneNumber",
            addressInput: "Please Enter Address",
        }
    })
    $('#btnUpdateDetails').on('click', function () {
        $("#frmuserDetails").validate();
    });
});


function GetAllUserData() {
    $('#UserTableData').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "Post",
            url: '/UserProfile/GetUserList',
            dataType: 'json'
        },
        columns: [
            {
                "data": "userName", "name": "UserName",
                "render": function (data, type, full) {
                    return '<h5 class="fs-15"><a href="/UserProfile/DisplayUserDetails/?Id=' + full.id + '" class="fw-medium link-primary">' + full.userName + '</a></h5>';
                }
            },
            {
                "data": "departmentId", "name": "DepartmentName",
                "render": function (data, type, full) {
                    return '<div class="d-flex"><div class="flex-grow-1 tasks_name">' + full.departmentName + '</div>';
                }
            },
            { "data": "roleName", "name": "RoleName" },
            {
                "data": "firstName", "name": "FirstName",
                "render": function (data, type, full) {
                    return '<div class="d-flex">' +
                        '<div class="flex-grow-1 tasks_name">' +
                        '<img src="/' + full.image + '" style="height: 40px; width: 40px; border-radius: 50%;" ' +
                        'onmouseover="showIcons(event, this.parentElement)" onmouseout="hideIcons(event, this.parentElement)">' +
                        full['firstName'] + ' ' + full['lastName'] +
                        '</div>';
                }


            },
            {
                "data": "isActive", "name": "IsActive",
                "render": function (data, type, full) {
                    if (full.isActive == true) {
                        return '<span class="badge bg-success text-uppercase">Active</span>';
                    } else {
                        return '<span class="badge bg-danger text-uppercase">Deactive</span>';
                    }
                }
            },
            { "data": "gender", "name": "Gender" },
            {
                "data": "dateOfBirth", "name": "DateOfBirth", "type": "date",
                "render": function (data, type, full, meta) {
                    return getCommonDateformat(data);
                }
            },
            { "data": "email", "name": "Email" },
            { "data": "phoneNumber", "name": "PhoneNumber" },
            { "data": "address", "name": "Address" }
        ],
        scrollY: 400,
        scrollX: true,
        scrollCollapse: true,
        fixedHeader: {
            header: true,
            footer: true
        },
        autoWidth: false,
        columnDefs: [{
            defaultContent: "",
            targets: "_all",
            width: 'auto'
        }]
    });
}


function UserActiveDeactive(UserId, checkboxElement) {
    var isActive = checkboxElement.checked;
    var action = isActive ? 'activate' : 'deactivate';
    var confirmationMessage = isActive ? "Are you sure you want to activate this user?" : "Are you sure you want to deactivate this user?";

    Swal.fire({
        title: confirmationMessage,
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, proceed!",
        cancelButtonText: "No, cancel!",
        confirmButtonClass: "btn btn-primary w-xs me-2 mt-2",
        cancelButtonClass: "btn btn-danger w-xs mt-2",
        buttonsStyling: false,
        showCloseButton: true
    }).then((result) => {
        
        if (result.isConfirmed) {
            $.ajax({
                url: '/UserProfile/UserActiveDecative?UserName=' + UserId,
                type: 'POST',
                contentType: 'application/json;charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    Swal.fire({
                        title: "Success!",
                        text: result.message,
                        icon: "success",
                        confirmButtonClass: "btn btn-primary w-xs mt-2",
                        buttonsStyling: false
                    }).then(function () {
                        location.reload();
                    });
                },
                error: function (xhr, status, error) {
                    Swal.fire(
                        'Error',
                        'An error occurred while processing your request. Please try again later.',
                        'error'
                    );
                }
            });
        } else if (result.dismiss === Swal.DismissReason.cancel) {
            Swal.fire(
                'Cancelled',
                'No changes were made.',
                'error'
            );
            checkboxElement.checked = !isActive;
        }
    });
}
function UpdateUserRoleAndDept(userId) {
    var objData = {
        Id: $('#txtUserId_' + userId).val(),
        RoleId: $('#ddlUserRole_' + userId).val(),
        DepartmentId: $('#ddlDepartment_' + userId).val(),
        DateOfBirth: $('#txtDateOfBirth_' + userId).val(),
        Gender: $('#txtGender_' + userId).val(),
        FirstName: $('#txtFirstName_' + userId).val(),
        LastName: $('#txtLastName_' + userId).val(),
        Email: $('#txtEmail_' + userId).val(),
        PhoneNumber: $('#txtPhoneNumber_' + userId).val(),
        Address: $('#txtAddress_' + userId).val()
    };
    var form_data = new FormData();
    form_data.append("USERUPDATE", JSON.stringify(objData));

    $.ajax({
        url: '/UserProfile/UpdateUserRoleAndDepartment',
        type: 'post',
        data: form_data,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (result) {
            if (result.code == 200) {
                Swal.fire({
                    title: result.message,
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK'
                }).then(function () {
                    window.location = '/UserProfile/UserActiveDecative';
                });
            } else {
                Swal.fire({
                    title: result.message,
                    icon: 'warning',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK'
                });
            }
        },
        error: function (xhr, status, error) {
            Swal.fire(
                'Error',
                'An error occurred while updating user details. Please try again later.',
                'error'
            );
        }
    });
}

function EnterInTime() {

    var fromData = new FormData();
    fromData.append("UserId", $("#txtuserid").val());
    fromData.append("Date", $("#txttodayDate").val());
    fromData.append("CreatedBy", $("#txtuserid").val());
    $.ajax({
        url: '/Home/EnterUserInTime',
        type: 'Post',
        data: fromData,
        dataType: 'json',
        processData: false,
        contentType: false,
        success: function (Result) {
            Swal.fire({
                title: Result.message,
                icon: Result.icone,
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK'
            })
            GetUserAttendanceInTime();
        },

    })
}

function EnterOutTime() {

    if ($("#todayouttime").text() == "Pending") {

        Swal.fire({
            title: "Are you sure want to enter out-time..?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes, enter it!",
            cancelButtonText: "No, cancel!",
            confirmButtonClass: "btn btn-primary w-xs me-2 mt-2",
            cancelButtonClass: "btn btn-danger w-xs mt-2",
            buttonsStyling: false,
            showCloseButton: true
        }).then((result) => {

            if (result.isConfirmed) {
                var formData = new FormData();
                formData.append("UserId", $("#txtuserid").val());

                $.ajax({
                    url: '/Home/EnterUserOutTime',
                    type: 'Post',
                    data: formData,
                    dataType: 'json',
                    processData: false,
                    contentType: false,
                    success: function (Result) {
                        Swal.fire({
                            title: "Deleted!",
                            text: Result.message,
                            icon: "success",
                            confirmButtonClass: "btn btn-primary w-xs mt-2",
                            buttonsStyling: false
                        });
                        GetUserAttendanceInTime();
                    }
                });
            } else if (result.dismiss === Swal.DismissReason.cancel) {

                Swal.fire(
                    'Cancelled',
                    'User have no changes.!!😊',
                    'error'
                );
            }
        });


    } else {

        var formData = new FormData();
        formData.append("UserId", $("#txtuserid").val());
        $.ajax({
            url: '/Home/EnterUserOutTime',
            type: 'Post',
            data: formData,
            dataType: 'json',
            processData: false,
            contentType: false,
            success: function (Result) {
                Swal.fire({
                    title: Result.message,
                    icon: Result.icone,
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK'
                });
                GetUserAttendanceInTime();
            }
        });

    }
}
function ResetPassword() {
    if ($("#passwordform").valid()) {
        var objData = {
            UserName: $('#txtUserName').val(),
            Password: $('#passwordinput').val(),
            ConfirmPassword: $('#confirmpasswordinput').val(),
        }

        $.ajax({
            url: '/UserProfile/ResetUserPassword',
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
                    window.location = '/UserProfile/DisplayUserList';
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


function GetUserAttendanceInTime() {

    $.ajax({
        url: '/Home/GetUserAttendanceInTime',
        type: 'Post',
        dataType: 'json',
        processData: false,
        contentType: false,
        success: function (Result) {

            var datetime = Result.data;
            if (datetime != null) {
                var userInTime = datetime.intime;
                var inTime = userInTime.substr(11, 2);
                let newformat = inTime >= 12 ? 'PM' : 'AM';
                inTime = inTime % 12;
                inTime = inTime ? inTime : 12;
                var minutes = userInTime.substr(13, 3);
                $("#todayintime").text(inTime + minutes + ' ' + newformat);
                var userOutTime = datetime.outTime;
                if (userOutTime != null) {
                    var outtime = userOutTime.substr(11, 2);
                    let newtimeformat = outtime >= 12 ? 'PM' : 'AM';
                    outtime = outtime % 12;
                    outtime = outtime ? outtime : 12;
                    var minutes = userOutTime.substr(13, 3);
                    $("#todayouttime").text(outtime + minutes + ' ' + newtimeformat);

                    var userTotalHour = datetime.totalHours;
                    if (userTotalHour != null) {
                        var TotalHour = userTotalHour.substr(0, 2);
                        let Hours = TotalHour > 01 ? 'hrs' : 'hr';
                        var minutes = userTotalHour.substr(2, 3);
                        $("#txttotalhours").text(TotalHour + minutes + ' ' + Hours);
                    }
                    else {
                        $("#txttotalhours").text("Pending");
                    }
                }
                else {
                    $("#todayouttime").text("Pending");
                    $("#txttotalhours").text("Pending");
                }
            }
            else {
                $("#todayintime").text("Pending");
                $("#todayouttime").text("Missing");
                $("#txttotalhours").text("Pending");
            }

        },

    })
}

function UserBirsthDayWish() {
    if (sessionStorage.getItem('birthdayWishSent')) {
        return;
    }

    $.ajax({
        url: '/Home/UserBirsthDayWish',
        type: 'Get',
        dataType: 'json',
        processData: false,
        contentType: false,
        success: function (Result) {


            if (Result.message != null) {
                Swal.fire(
                    {
                        html: '<div class="mt-3"><lord-icon src="https://cdn.lordicon.com/lupuorrc.json" trigger="loop" colors="primary:#0ab39c,secondary:#405189" style="width:120px;height:120px"></lord-icon><div class="mt-4 pt-2 fs-15"><h4>' + Result.message + '</h4></div></div>',
                        showCancelButton: !0,
                        showConfirmButton: !1,
                        cancelButtonClass: "btn btn-primary w-xs mb-1",
                        cancelButtonText: "Thank You",
                        buttonsStyling: !1,
                        showCloseButton: !0
                    }
                );
                sessionStorage.setItem('birthdayWishSent', true);
            }
        }
    });
}


function EditUserDetails(EmpId) {
    $.ajax({
        url: '/UserProfile/EditUserDetails?Id=' + EmpId,
        type: 'Get',
        contentType: 'application/json;charset=utf-8 ',
        datatype: 'json',
        success: function (response) {
            $('.empmodal').modal('show');
            $('#Userid').val(response.id);
            $('#FirstName').val(response.firstName);
            $('#LastName').val(response.lastName);
            $('#Dob').val(response.dateOfBirth);
            $('#Gender').val(response.gender);
            $('#Email').val(response.email);
            $('#deptid').val(response.departmentId);
            $('#ddlDepartmenrnt').val(response.departmentId);
            $('#PhoneNo').val(response.phoneNumber);
            $('#Address').val(response.address);
        },
    })
}


function UserLogout() {
    Swal.fire({
        title: 'Logout confirmation',
        text: 'Are you sure you want to logout?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, logout'
    }).then((result) => {
        if (result.isConfirmed) {

            logout();
        }
    });
}

function logout() {

    fetch('/Authentication/Logout', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
            'RequestVerificationToken': '@Token.Get(Request.HttpContext)'

        },
        body: ''
    })
        .then(response => {

            window.location.href = '/Authentication/Login';
        })
        .catch(error => {
            console.error('Error:', error);

        });
}

function UpdateUserDetails() {
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
        url: '/UserProfile/UpdateUserDetails',
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
                window.location = '/UserProfile/DisplayUserList';
            });
        },
    })

}


//validation

var firstName, lastName, dateOfBirth, email, phoneNumber, address, pincode;
var isValid = true;

$('#btnUpdate').click(function () {
    if (CheckValidation() == false) {
        return false;
    }
});
function CheckValidation() {
    firstName = $('#FirstName').val();
    lastName = $('#LastName').val();
    dateOfBirth = $('#Dob').val();
    gender = $('#Gender').val();
    email = $('#Email').val();
    phoneNumber = $('#PhoneNo').val();
    address = $('#Address').val();

    //fname
    if (firstName == "") {
        $('#txtFirstName').text('FirstName can not be blank.');
        $('#FirstName').css('border-color', 'red');
        $('#FirstName').focus();
        isValid = false;
    }
    else {
        $('#txtFirstName').text('');
        $('#FirstName').css('border-color', 'green');
    }
    //lname
    if (lastName == "") {

        $('#txtLastName').text('LastName can not be blank.');
        $('#LastName').css('border-color', 'red');
        $('#LastName').focus();
        isValid = false;
    }
    else {

        $('#txtLastName').text('');
        $('#LastName').css('border-color', 'green');
    }
    //dob
    if (dateOfBirth == "") {

        $('#txtDob').text('DateOfBirth can not be blank.');
        $('#Dob').css('border-color', 'red');
        $('#Dob').focus();
        isValid = false;
    }
    else {

        $('#txtDob').text('');
        $('#Dob').css('border-color', 'green');
    }

    //email
    if (email == "") {

        $('#txtEmail').text('EmailId can not be blank.');
        $('#Email').css('border-color', 'red');
        $('#Email').focus();
        isValid = false;
    }
    else {

        $('#txtEmail').text('');
        $('#Email').css('border-color', 'green');
    }



    //phone
    if (phoneNumber == "") {

        $('#txtphno').text('PhoneNumber can not be blank.');
        $('#PhoneNo').css('border-color', 'red');
        $('#PhoneNo').focus();
        isValid = false;
    }
    else {

        $('#txtphno').text('');
        $('#PhoneNo').css('border-color', 'green');
    }
    //address
    if (address == "") {

        $('#txtAddress').text('Address can not be blank.');
        $('#Address').css('border-color', 'red');
        $('#Address').focus();
        isValid = false;
    }
    else {

        $('#txtAddress').text('');
        $('#Address').css('border-color', 'green');
    }
    return isValid;
}
//serchbar
$('#txtserch').keyup(function () {

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


function clearSelectedBox() {
    $("#ddlusername").find("option").remove().end().append(
        '<option selected disabled value="">--Select Username--</option>');

    $("#ddlDepartmenrnt").find("option").remove().end().append(
        '<option selected disabled value="">--Select Department--</option>');
}

function SearchEmployeeList() {
    if ($("#searchEmployee").val() == "ByUsername") {
        clearSelectedBox();
        GetUsername();
        $("#empnamebox").show();
    } else if ($("#searchEmployee").val() == "ByDepartment") {
        clearSelectedBox();
        GetDepartment();
        $("#empnamebox").show();
    }
}

function GetUsername() {
    $.ajax({
        url: '/Task/GetUserName',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#ddlusername').append('<option value=' + data.id + '>' + data.firstName + ' ' + data.lastName + ' (' + data.userName + ')</option>');
            });
        }
    });
}

function GetDepartment() {
    $.ajax({
        url: '/Authentication/GetDepartment',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#ddlDepartmenrnt').append('<option value=' + data.id + '>' + data.departments + '</option>');
            });
        }
    });
}

$(document).ready(function () {
    $("#searchEmployee").change(SearchEmployeeList);
    $("#empnamebox").hide();
});


function GetSearchEmpList() {

    if ($('#activeInactiveForm').valid()) {
        var form_data = new FormData();
        form_data.append("DepartmentId", $('#ddlDepartmenrnt').val());
        form_data.append("Id", $("#ddlusername").val());
        $.ajax({
            url: '/UserProfile/GetSearchEmpList',
            type: 'Post',
            datatype: 'json',
            data: form_data,
            processData: false,
            contentType: false,
            complete: function (Result) {

                $("#allemplist").hide();
                $("#activedeactivepagination").hide();
                $("#backbtn").show();
                if (Result.responseText != '{\"code\":400}') {
                    $("#errorMessage").hide();
                    $("#dvseachemplist").show();
                    $("#dvseachemplist").html(Result.responseText);
                } else {
                    var message = "No Data Found On Selected Username Or Department!!";
                    $("#errorMessage").show();
                    $("#errorMessage").text(message);
                    $("#dvseachemplist").hide();
                }
                /*clearSelectedBox()*/
            }
        });
    } else {
        Swal.fire({
            title: "Kindly fill the status",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        })
    }
};



function GetActiveDeactiveList(page) {

    var searchBy = $("#searchEmployee").val();
    var searchFor = $('#txtsearchfor').val();
    $.get("/UserProfile/UserActiveDecativeList", { searchby: searchBy, searchfor: searchFor, page: page })
        .done(function (result) {

            $("#activedeactivepartial").html(result);
        })
        .fail(function (error) {
            console.error(error);
        });
}

GetActiveDeactiveList(1);
$(document).on("click", ".pagination a", function (e) {
    e.preventDefault();
    var page = $(this).text();
    GetActiveDeactiveList(page);
});


$(document).on("click", "#backbtn", function (e) {
    e.preventDefault();
    var page = $(this).text();
    GetActiveDeactiveList(page);
});

function BtnSearchDetails() {


    GetActiveDeactiveList(1);
}


function EdituserDetails() {

    document.getElementById("firstnameInput").removeAttribute("readonly");
    document.getElementById("lastnameInput").removeAttribute("readonly");
    document.getElementById("phonenumberInput").removeAttribute("readonly");
    document.getElementById("emailInput").removeAttribute("readonly");
    document.getElementById("birthdateInput").removeAttribute("readonly");
    document.getElementById("addressInput").removeAttribute("readonly");
    document.getElementById("genderInput").removeAttribute("readonly");

    //var inputs = document.querySelectorAll('input[readonly], textarea[readonly]');
    //for (var i = 0; i < inputs.length; i++) {
    //  inputs[i].removeAttribute('readonly');
    //};        
}

function updateuserDetails() {
    if ($('#frmuserDetails').valid()) {
        var UserId = $('#idInput').val()
        var objData = {
            Id: UserId,
            FirstName: $('#firstnameInput').val(),
            LastName: $('#lastnameInput').val(),
            DepartmentId: $('#departmentIdInput').val(),
            Email: $('#emailInput').val(),
            Address: $('#addressInput').val(),
            PhoneNumber: $('#phonenumberInput').val(),
            DateOfBirth: $('#birthdateInput').val(),
            Gender: $('#genderInput').val(),

        }
        $.ajax({
            url: '/UserProfile/UpdateUserDetails',
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
                    window.location = '/UserProfile/DisplayUserDetails/?Id=' + UserId;
                });
            },
        })
    } else {
        Swal.fire({
            title: 'Fill empty the details',
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK'
        })
    }
}
var PasswordForm;
function validateAndPassword() {

    PasswordForm= $("#passwordform").validate({
        rules: {
            passwordinput: "required",
            confirmpasswordinput: "required",
        },
        messages: {
            passwordinput: "Please Enter Password",
            confirmpasswordinput: "Please Enter ConfirmPassword",
        },
    })
    var isValid = true;
   
}