
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
    debugger
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
                icon: 'warning',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK'
            })

        },
        error: function () {
            toastr.error('There is some problem in your request.');
        }
    })
}

function EnterOutTime() {
    debugger
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
                icon: 'warning',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK'
            })

        },
        error: function () {
            toastr.error('There is some problem in your request.');
        }
    })
}

function ResetPassword()
{
    debugger
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
            debugger
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
