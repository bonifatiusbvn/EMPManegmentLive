function ForgetPassword()
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
                    $("#invalidmessage").show();
                    $("#invalidmessage").text(Result.message);
                }
            },
        })
    } else {
        $("#invalidmessage").hide();
        Swal.fire({
            title: "Enter Email Id",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        })
    }   
}

$(document).ready(function () {
    $("#invalidmessage").hide();
    $("#emailsentform").validate({
        rules: {
            Email: "required",
        },
        messages: {
            Email: "Please Enter Registered Email",
        }
    });
});