$("#usernamebox").show();
$("#departmentbox").hide();
$("#backbtn").hide();

$(document).ready(function () {
    GetUserAttendanceInTime();
    UserBirsthDayWish();
    GetAllUserData();
    clearSelectedBox()
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
});

function clearSelectedBox() {
  
    $("#ddlusername").find("option").remove().end().append(
        '<option selected disabled value = "">--Select Username--</option>');

    $("#ddlDepartmenrnt").find("option").remove().end().append(
        '<option selected disabled value = "">--Select Department--</option>');
}

$('#searchEmployee').change(function () {
   
    if ($("#searchEmployee").val() == "ByUsername") {
        clearSelectedBox();
        GetUsername();
        $("#empnamebox").show();
        $("#departmentbox").hide();
    }
    if ($("#searchEmployee").val() == "ByDepartment") {
        clearSelectedBox();
        GetDepartment();
        $("#empnamebox").hide();
        $("#departmentbox").show();        
    } 
});

function GetAllUserData() {
    $('#UserTableData').DataTable({
        processing: true,
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
                "data": "departmentName",
                "render": function (data, type, full) {
                    return '<div class="d-flex"><div class="flex-grow-1 tasks_name">' + full.departmentName + '</div><div class="flex-shrink-0 ms-4"><ul class="list-inline tasks-list-menu mb-0"><li class="list-inline-item"><a onclick="UserProfileDetails(\'' + full.id + '\')"><i class="ri-eye-fill align-bottom me-2 text-muted"></i></a></li><li class="list-inline-item"><a onclick="EditUserDetails(\'' + full.id + '\')"><i class="ri-pencil-fill align-bottom me-2 text-muted"></i></a></li></ul></div></div>';
                }
            },
            /*{ "data": "departmentName", "name": "DepartmentName" },*/
            { "data": "userName", "name": "UserName" },
            {
                "data": "firstName",
                "render": function (data, type, full) {
                    return '<div class="d-flex align-items-center fw-medium"><img src="/' + full.image + '" style="height: 40px; width: 40px; border-radius: 50%;">' + full['firstName'] + ' ' + full['lastName'] + '</div >';
                }
            },
            {
                "data": "isActive", "name": "IsActive",
                "render": function (data, type, full) {

                    if (full.isActive == true) {
                        return '<a class="badge bg-success text-uppercase">' + full.isActive + '</a>';
                    }
                    else {
                        return '<a class="badge bg-danger text-uppercase">' + full.isActive + '</a>';
                    }
                }
            },
            { "data": "gender", "name": "Gender" },
            {
                "data": "dateOfBirth", "name": "DateOfBirth", "type": "date",
                "render": function (data) {
                    var date = new Date(data);
                    var month = date.getMonth() + 1;
                    var day = date.getDate();
                    return ("0" + day).slice(-2) + "/"
                        + (month.length > 1 ? month : month) + "/"
                        + date.getFullYear();
                }
            },
            { "data": "email", "name": "Email" },
            { "data": "phoneNumber", "name": "PhoneNumber" },
            { "data": "cityName", "name": "CityName" },
            { "data": "address", "name": "Address" },
           
        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all",
        }]
    });
}

function ActiveUser(UserName) {
    
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })



    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't to Active this User!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, Active it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            
            $.ajax({
                url: '/UserProfile/UserActiveDecative?UserName=' + UserName,
                type: 'Post',
                contentType: 'application/json;charset=utf-8;',
                dataType: 'json',
            
                success: function (Result) {
                    
                    swalWithBootstrapButtons.fire(
                        'Done!',
                         Result.message,
                        'success'
                    ).then(function () {
                        window.location = '/UserProfile/UserActiveDecative';
                    }); 

                },
                error: function () {
                    toastr.error('There is some problem in your request.');
                }
            })
            
        } else if (
            /* Read more about handling dismissals below */
            result.dismiss === Swal.DismissReason.cancel
        ) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'User Have No Changes.!! :)',
                'error'
            )
        }
    })
  
}
function DeactiveUser(UserName) {

    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })



    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't to DeActive this User!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, Deactive it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                url: '/UserProfile/UserActiveDecative?UserName=' + UserName,
                type: 'Post',
                contentType: 'application/json;charset=utf-8;',
                dataType: 'json',

                success: function (Result) {

                    swalWithBootstrapButtons.fire(
                        'Done!',
                        Result.message,
                        'success'
                    ).then(function () {
                        window.location = '/UserProfile/UserActiveDecative';
                    });

                },
                error: function () {
                    toastr.error('There is some problem in your request.');
                }
            })

        } else if (
            /* Read more about handling dismissals below */
            result.dismiss === Swal.DismissReason.cancel
        ) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'User Have No Changes.!! :)',
                'error'
            )
        }
    })

}

function EnterInTime() {
    
    var fromData = new FormData();
    fromData.append("UserId", $("#txtuserid").val());
    fromData.append("Date", $("#txttodayDate").val());
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
    
    var fromData = new FormData();
    fromData.append("UserId", $("#txtuserid").val());
    $.ajax({
        url: '/Home/EnterUserOutTime',
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

function ResetPassword()
{
    var fromData = new FormData();
    fromData.append("UserName", $("#txtUserName").val());
    fromData.append("Password", $("#password-input").val());

    $.ajax({
        url: '/UserProfile/ResetUserPassword',
        type: 'Post',
        data: fromData,
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
                window.location = '/UserProfile/ResetUserPassword';
            }); 

        },
        error: function () {
            alert('There is some problem in your request.');
        }
    })
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
                        var TotalHour = userTotalHour.substr(0,2);
                        let Hours = TotalHour > 01 ? 'hrs' : 'hr';
                        var minutes = userTotalHour.substr(2,3);
                        $("#txttotalhours").text(TotalHour + minutes + ' ' + Hours);
                    }
                    else
                    {
                        $("#txttotalhours").text("Pending");
                    }
                }
                else
                {
                    $("#todayouttime").text("Pending");
                    $("#txttotalhours").text("Pending");
                }
            }
            else {
                $("#todayintime").text("Pending");
                $("#todayouttime").text("Pending");
                $("#txttotalhours").text("Pending");
            }

        },
       
    })
}

function UserBirsthDayWish() {
    
    $.ajax({
        url: '/Home/UserBirsthDayWish',
        type: 'Get',
        dataType: 'json',
        processData: false,
        contentType: false,
        success: function (Result) {
            var Userdata = Result.data;
            if (Userdata != null) {
                Swal.fire(
                    {
                        html: '<div class="mt-3"><lord-icon src="https://cdn.lordicon.com/lupuorrc.json" trigger="loop" colors="primary:#0ab39c,secondary:#405189" style="width:120px;height:120px"></lord-icon><div class="mt-4 pt-2 fs-15"><h4>' + Result.message + '</h4></div></div>',
                        showCancelButton: !0,
                        showConfirmButton: !1,
                        cancelButtonClass: "btn btn-primary w-xs mb-1",
                        cancelButtonText: "Thank You",
                        buttonsStyling: !1,
                        showCloseButton: !0
                    })
            }
            else {

            }
        },

    })
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
                clearSelectedBox()
            }
        });
    } else {
        Swal.fire({
            title: "Kindly Fill the Status",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        })
    }
};
