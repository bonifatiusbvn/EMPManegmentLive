

$(document).ready(function () {
    $('#dropdownButton').click(function () {
        var dropdown = $('#customDropdown');
        if (dropdown.is(':visible')) {
            dropdown.hide();
        } else {
            dropdown.show();
            GetUserRoleList();
        }
    });

    $(document).on('click', '.dropdown-item-custom', function () {
        var selectedText = $(this).text();
        var selectedValue = $(this).data('value');
        $('#dropdownButton').text(selectedText).attr('data-selected-value', selectedValue);
        $('#customDropdown').hide();
        EditRoleWiseFormDetails(selectedValue);
    });

    $(document).click(function (event) {
        if (!$(event.target).closest('#dropdownButton').length && !$(event.target).closest('#customDropdown').length) {
            $('#customDropdown').hide();
        }
    });

    function GetUserRoleList() {
        $.ajax({
            url: '/UserProfile/RolewisePermissionListAction',
            success: function (result) {
                var dropdown = $('#customDropdown');
                dropdown.empty();
                $.each(result, function (i, data) {
                    dropdown.append('<div class="dropdown-item-custom" data-value="' + data.roleId + '">' + data.role + '</div>');
                });
            }
        });
    }

    function EditRoleWiseFormDetails(roleId) {
        var RoleId = roleId
        $.ajax({
            url: '/UserProfile/GetRolewiseFormListById?RoleId=' + RoleId,
            type: 'post',
            dataType: 'json',
            processData: false,
            contentType: false,
            complete: function (Result) {
                if (Result.responseText != "{\"code\":400}") {
                    document.getElementById("updatebtn").style.display = "block";
                    $('#dveditRolePermissionForm').html(Result.responseText).show();
                }
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
});




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
                })
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

function createRole() {
    if ($("#addUserRole").valid()) {
        var formData = new FormData();
        formData.append("Role", $("#textRoleName").val());
        formData.append("CreatedBy", $("#txtUserId").val());

        $.ajax({
            url: '/UserProfile/CreateUserRole',
            type: 'Post',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
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
                }
                else {
                    Swal.fire({
                        title: Result.message,
                        icon: 'warning',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    })
                }
            }
        });
    }
    else {
        Swal.fire({
            title: "Kindly fill role",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        })
    }
}

var UserRoleForm;

function validateAndCreateRole() {
    UserRoleForm = $("#addUserRole").validate({
        rules: {
            textRoleName: "required",
        },
        messages: {
            textRoleName: "Please enter role",
        }
    })
    var isValid = true;

    if (isValid) {
        createRole();
    }
}

function clearTextRoleName() {
    ResetUserRoleForm();
    document.getElementById("textRoleName").value = "";
    $('#createRoleModal').modal('show');
}

function ResetUserRoleForm() {
    if (UserRoleForm) {
        UserRoleForm.resetForm();
    }
}

