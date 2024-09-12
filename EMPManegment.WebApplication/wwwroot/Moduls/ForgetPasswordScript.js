﻿function ForgetPassword()
{
    if ($('#emailsentform').valid()) {
        var formData = new FormData();
        formData.append("Email", $("#txtemail").val());

        $.ajax({
            url: '/Authentication/ForgetPassword',
            type: 'Post',
            data: formData,
            dataType: 'json',
            processData: false,
            contentType: false,
            success: function (Result) {
            debugger
                if (Result.code == 200) {
                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/Authentication/Login';
                    });
                }
                else {
                    $("#invalidmessage").text(Result.message).show();
                }
            },
        })
    } else {
        $("#invalidmessage").hide();
            toastr.warning("Enter email id");     
    }   
}

$(document).ready(function () {
    $("#invalidmessage").hide();
    $("#emailsentform").validate({
        rules: {
            Email: "required",
        },
        messages: {
            Email: "Please enter registered email",
        }
    });
});