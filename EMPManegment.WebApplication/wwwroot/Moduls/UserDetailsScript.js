


$(document).ready(function () {
    GetUserAttendanceInTime();
    UserBirsthDayWish();
    GetAllUserData();
});
function GetAllUserData() {
    $('#UserTableData').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        "bDestroy": true,
        ajax: {
            type: "Post",
            url: '/UserDetails/GetUserList',
            dataType: 'json'
        },
        columns: [
            { "data": "departmentName", "name": "DepartmentName" },
            { "data": "userName", "name": "UserName" },
            {
                "data": "firstName",
                "render": function (data, type, full) {
                    return '<div class="d-flex align-items-center fw-medium"><img src="/' + full.image + '" style="height: 40px; width: 40px; border-radius: 50%;">' + full['firstName'] + ' ' + full['lastName'] + '</div >';
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
            { "data": "countryName", "name": "CountryName" },
            { "data": "stateName", "name": "StateName"},
            { "data": "cityName", "name": "CityName" },
            { "data": "address", "name": "Address" },
           
        ],
        columnDefs: [{
            "defaultContent": "",
            "targets": "_all",
        }]
    });
}

function ActiveDeactive(UserName) {
    
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't to Active Or Deactive this User!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, Active/Deactive it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            
            $.ajax({
                url: '/UserDetails/UserActiveDecative?UserName=' + UserName,
                type: 'Post',
                contentType: 'application/json;charset=utf-8;',
                dataType: 'json',
            
                success: function (Result) {
                    
                    swalWithBootstrapButtons.fire(
                        'Done!',
                         Result.message,
                        'success'
                    ).then(function () {
                        window.location = '/UserDetails/UserActiveDecative';
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
        url: '/UserDetails/ResetUserPassword',
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
                window.location = '/UserDetails/ResetUserPassword';
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

