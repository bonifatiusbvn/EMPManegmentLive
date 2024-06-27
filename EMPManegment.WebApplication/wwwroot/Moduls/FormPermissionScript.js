
$(document).ready(function () {
    GetFormList();
    $('#dropdownButton').click(function () {
        var dropdown = $('#customDropdown');
        if (dropdown.is(':visible')) {
            dropdown.hide();
        } else {
            dropdown.show();
            GetUserRoleList();
        }
    });
    $('#userdropdownButton').click(function () {
        var dropdown = $('#usercustomDropdown');
        if (dropdown.is(':visible')) {
            dropdown.hide();
        } else {
            dropdown.show();
            GetUsernameForm();
        }
    });
    $(document).on('click', '.Role-dropdown-item-custom', function () {
        var selectedText = $(this).text();
        var selectedValue = $(this).data('value');
        $('#dropdownButton').text(selectedText).attr('data-selected-value', selectedValue);
        $('#customDropdown').hide();
        EditRoleWiseFormDetails(selectedValue);
    });
    $(document).on('click', '.User-dropdown-item-custom', function () {
        var selectedText = $(this).text();
        var selectedValue = $(this).data('value');
        $('#userdropdownButton').text(selectedText).attr('data-selected-value', selectedValue);
        $('#usercustomDropdown').hide();
        EditUserFormDetails(selectedValue);
    });
    $(document).click(function (event) {
        if (!$(event.target).closest('#dropdownButton').length && !$(event.target).closest('#customDropdown').length) {
            $('#customDropdown').hide();
        }
    });
    $(document).click(function (event) {
        if (!$(event.target).closest('#userdropdownButton').length && !$(event.target).closest('#usercustomDropdown').length) {
            $('#usercustomDropdown').hide();
        }
    });
    function GetUserRoleList() {
        $.ajax({
            url: '/UserProfile/RolewisePermissionListAction',
            success: function (result) {
                var dropdown = $('#customDropdown');
                dropdown.empty();
                $.each(result, function (i, data) {
                    dropdown.append('<div class="Role-dropdown-item-custom dropdown-item" data-value="' + data.roleId + '">' + data.role + '</div>');
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
                } else {
                    toastr.error(Result.message);
                }
            },
        });
    }

});
function EditUserFormDetails(userId) {
    var UserId = userId
    $.ajax({
        url: '/UserProfile/GetUserFormListById?UserId=' + UserId,
        type: 'post',
        dataType: 'json',
        processData: false,
        contentType: false,
        complete: function (Result) {
            if (Result.responseText == "") {
                toastr.warning("No data found");
                $('#dveditUserForm').html(Result.responseText).show();
                $('#userupdatebtn').hide();
            } else {
                document.getElementById("userupdatebtn").style.display = "block";
                $('#dveditUserForm').html(Result.responseText).show();
            }
        }
    });
}
$('#drpAttusername').change(function () {
    var Text = $("#drpAttusername Option:Selected").text();
    $("#textUserIdfrm").val(Text);
});

function UpdateRolewiseFormPermission() {

    var formPermissions = [];
    $(".forms").each(function () {

        var rolewiseformRow = $(this);
        var objData = {
            RoleId: $('#txtRoleId').val(),
            CreatedBy: $("#txtUserId").val(),
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
        url: '/UserProfile/UpdatePermission',
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
                toastr.error(Result.message);
            }
        },
        error: function (xhr, status, error) {
            toastr.error(error);
        }
    });
}
function UpdateUserFormPermission() {
    var formPermissions = [];
    $(".forms").each(function () {

        var userformRow = $(this);
        var objData = {
            UserId: $('#textUserId').val(),
            FormId: userformRow.find('#textFormId').val(),
            IsAddAllow: userformRow.find('#txtIsAdd').prop('checked'),
            IsViewAllow: userformRow.find('#txtIsView').prop('checked'),
            IsEditAllow: userformRow.find('#txtIsEdit').prop('checked'),
            IsDeleteAllow: userformRow.find('#txtIsDelete').prop('checked'),
        };
        formPermissions.push(objData);
    });
    var form_data = new FormData();
    form_data.append("UserPermissionDetails", JSON.stringify(formPermissions));

    $.ajax({
        url: '/UserProfile/UpdateUserPermission',
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
                toastr.error(Result.message);
            }
        },
        error: function (xhr, status, error) {
            toastr.error(error);
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
                    toastr.error(Result.message);
                }
            }
        });
    }
    else {
        toastr.warning("Kindly fill role");
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
function clearTextUserName() {
    ResetUserForm();
    document.getElementById("drpAttusername").value = "";
    $('#createUserModal').modal('show');
}
function ResetUserRoleForm() {
    if (UserRoleForm) {
        UserRoleForm.resetForm();
    }
}
function ResetUserForm() {
    if (UserForm) {
        UserForm.resetForm();
    }
}

function GetFormList() {
    $.ajax({
        url: '/UserProfile/GetFormNameList',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#drpFormList').append('<Option value=' + data.formId + '>' + data.formName + '</Option>')
            });
        }
    });
}
function SaveFormDetails() {
    siteloadershow();
    var formData = new FormData();
    formData.append("FormId", $("#drpFormList").val());
    $.ajax({
        url: '/UserProfile/CreateRolewisePermissionForm',
        type: 'Post',
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (Result) {
            if (Result.code == 200) {
                siteloaderhide();
                Swal.fire({
                    title: Result.message,
                    icon: "success",
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                }).then(function () {
                    window.location = '/UserProfile/FormCreation';
                });
            }
            else {
                siteloaderhide();
                toastr.warning(Result.message);
            }

        }
    });

}
function CreateUserForm() {
    if ($("#addUserForm").valid()) {
        var UserId = $("#drpAttusername").val();

        $.ajax({
            url: '/UserProfile/CreateUserForm?UserId=' + UserId,
            type: 'Post',
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
                        window.location = '/UserProFile/UserFormPermission';
                    });
                }
                else {
                    toastr.warning(Result.message);
                }
            }
        });
    }
    else {
        toastr.warning("Kindly fill user");
    }
}
var UserForm;
function validateAndCreateUser() {
    UserForm = $("#addUserForm").validate({
        rules: {
            drpAttusername: "required",
        },
        messages: {
            drpAttusername: "Please enter user",
        }
    })
    var isValid = true;

    if (isValid) {
        CreateUserForm();
    }
}
