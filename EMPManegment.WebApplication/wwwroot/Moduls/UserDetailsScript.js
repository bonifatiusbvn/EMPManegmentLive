GetUserAttendanceOutTime();
GetUserAttendanceInTime();
UserBirsthDayWish();
GetUserTotalHour();

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
            GetUserAttendanceOutTime();
            GetUserTotalHour();
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
                var InTime = datetime.substr(0, 2);
                let newformat = InTime >= 12 ? 'PM' : 'AM';
                InTime = InTime % 12;
                InTime = InTime ? InTime : 12;
                var minutes = datetime.substr(2, 5);
                $("#todayintime").text(InTime + minutes + ' ' + newformat);
            }
            else {
                $("#todayintime").text("undefined");
            }

        },
       
    })
}

function GetUserAttendanceOutTime() {
    
    $.ajax({
        url: '/Home/GetUserAttendanceOutTime',
        type: 'Post',
        dataType: 'json',
        processData: false,
        contentType: false,
        success: function (Result) {
            
            var datetime = Result.data;
            if (datetime != null) {
                var Outtime = datetime.substr(11, 2);
                let newformat = Outtime >= 12 ? 'PM' : 'AM';
                Outtime = Outtime % 12;
                Outtime = Outtime ? Outtime : 12;
                var minutes = datetime.substr(13, 3);
                $("#todayouttime").text(Outtime + minutes + ' ' + newformat);
            }
            else {
                $("#todayouttime").text("Pending");
            }
        },

    })
}

function GetUserTotalHour()
{
    $.ajax({
        url: '/Home/GetUserTotalHour',
        type: 'Post',
        dataType: 'json',
        processData: false,
        contentType: false,
        success: function (Result) {
            var datetime = Result.data;
            if (datetime != null) {
                var TotalHour = datetime.substr(0, 5);
                $("#txttotalhours").text(TotalHour);
            }
            else {
                $("#txttotalhours").text("OutTime Pending" + " ");
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
        },

    })
}