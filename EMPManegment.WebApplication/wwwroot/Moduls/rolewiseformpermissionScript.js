GetUserRoleList();

function GetUserRoleList() {
   
    $.ajax({
        url: '/UserProfile/RolewisePermissionListAction',
        success: function (result) {
            $.each(result, function (i, data) {
             
                $('#txtUserRole').append('<Option value=' + data.id + '>' + data.role + '</Option>')
            });
        }
    });
}

function AllRolewiseFormUserTable() {

    $.get("/UserProfile/RolewisePermissionListAction")
        .done(function (result) {
       
            $("#UserRoletbody").html(result).show();
            $('#dveditRolePermissionForm').hide();
        })
        .fail(function (error) {
            console.error(error);
        });
}

function EditRoleWiseFormDetails() {
    RoleId = $('#txtUserRole').val();
    $.ajax({
        url: '/UserProfile/GetRolewiseFormListById?RoleId=' + RoleId,
        type: 'post',
        dataType: 'json',
        processData: false,
        contentType: false,
        complete: function (Result) {
            document.getElementById("backbtn").style.display = "block";
            document.getElementById("updatebtn").style.display = "block";
            $('#dveditRolePermissionForm').html(Result.responseText).show();
        },
        Error: function () {

            Swal.fire({
                title: "Can't get data!",
                icon: 'warning',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK',
            })
        }
    });
}

function UpdateRolewiseFormPermission() {
 
    var formPermissions = [];
    $(".forms").each(function () {

        var rolewiseformRow = $(this);
        var objData = {
            RoleId: $('#txtRoleId').val(),
            FormId: rolewiseformRow.find('#formId').val(),
            IsAddAllow: rolewiseformRow.find('#isAdd').prop('checked'),
            IsViewAllow: rolewiseformRow.find('#isView').prop('checked'),
            IsEditAllow: rolewiseformRow.find('#isEdit').prop('checked'),
            IsDeleteAllow: rolewiseformRow.find('#isDelete').prop('checked'),
        };

        formPermissions.push(objData);
    });

    var form_data = new FormData();
    form_data.append("RolewisePermissionDetails", JSON.stringify(formPermissions));

    $.ajax({
        url: '/UserProfile/UpdateMultipleRolewiseFormPermission',
        type: 'post',
        data: form_data,
        processData: false,
        contentType: false,
        dataType: 'json',
        success: function (Result) {
          
            if (Result.code == 200) {
                Swal.fire({
                    title: Result.message,
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK'
                }).then(function () {
                    window.location = '/UserProfile/RolewisePermission';
                });
            } else {
                Swal.fire({
                    title: Result.message,
                    icon: 'warning',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK'
                })
            }
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}

function backBtn() {
    window.location = '/UserProfile/RolewisePermission';
}