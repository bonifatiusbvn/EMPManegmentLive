var formData = Array.isArray(window.userFormPermissions) ? window.userFormPermissions : [];

let UserGridOptions = {};

$(document).ready(function () {
    let canEdit = false;
    let canDelete = false;



    const userPermission = formData.find(p => p.formName === "Users");
    if (userPermission) {

        canEdit = !!userPermission.edit;
        canDelete = !!userPermission.delete;
    }

    UserGridOptions = {
        rowHeight: 50,
        columnDefs: [
            {
                headerName: "User Id",
                field: "userName",
                sortable: true,
                filter: true,
                cellRenderer: function (params) {
                    if (!params.data?.id) return '';
                    return `<div><a href="/UserProfile/UserInfo/?Id=${params.data.id}" class="fw-medium" style="color: #16989A !important;"><strong>${params.data.userName}</strong></a></div>`;
                }
            },
            {
                headerName: "Department Name",
                field: "departmentName",
                sortable: true,
                filter: true,
                cellRenderer: function (params) {
                    if (!params.data?.id) return '';
                    return `<div class="d-flex"><div class="flex-grow-1 tasks_name">${params.data.departmentName}</div></div>`;
                }
            },
            { headerName: "Role", field: "roleName", sortable: true, filter: true },
            {
                headerName: "User Name",
                field: "firstName",
                sortable: true,
                filter: true,
                cellRenderer: function (params) {
                    if (!params.data?.id) return '';

                    const colors = [
                        { bg: 'bg-primary-subtle', text: 'text-primary' },
                        { bg: 'bg-secondary-subtle', text: 'text-secondary' },
                        { bg: 'bg-success-subtle', text: 'text-success' },
                        { bg: 'bg-info-subtle', text: 'text-info' },
                        { bg: 'bg-warning-subtle', text: 'text-warning' },
                        { bg: 'bg-danger-subtle', text: 'text-danger' },
                        { bg: 'bg-dark-subtle', text: 'text-dark' }
                    ];

                    const initials = `${params.data.firstName?.[0] || ''}${params.data.lastName?.[0] || ''}`.toUpperCase();
                    const color = colors[Math.floor(Math.random() * colors.length)];

                    let profileHtml = '';

                    if (params.data.image?.trim()) {
                        profileHtml = `
            <img src="/${params.data.image}" 
                 style="height: 40px; width: 40px; border-radius: 50%;"
                 onerror="this.style.display='none'; this.nextElementSibling.style.display='inline-block';" />

            <div class="flex-shrink-0 avatar-xs me-2" style="display: none;">
                <div class="avatar-title ${color.bg} ${color.text} rounded-circle fs-13"
                     style="height: 40px; width: 40px;">${initials}</div>
            </div>`;
                    } else {
                        // No image provided, show initials directly
                        profileHtml = `
            <div class="flex-shrink-0 avatar-xs me-2">
                <div class="avatar-title ${color.bg} ${color.text} rounded-circle fs-13"
                     style="height: 40px; width: 40px;">${initials}</div>
            </div>`;
                    }

                    return `
        <div class="d-flex align-items-center">
            ${profileHtml}
            <div class="flex-grow-1 tasks_name ml-2" style="color: #16989A !important; margin-left: 10px;">
                ${params.data.firstName} ${params.data.lastName}
            </div>
        </div>`;
                }
            },
            {
                headerName: "Active",
                field: "isActive",
                sortable: true,
                filter: true,
                cellRenderer: function (params) {
                    if (!params.data?.id) return '';
                    return params.data.isActive
                        ? '<span class="badge bg-success text-uppercase">Active</span>'
                        : '<span class="badge bg-danger text-uppercase">Deactive</span>';
                }
            },
            { headerName: "Gender", field: "gender", sortable: true, filter: true },
            {
                headerName: "Date Of Birth",
                field: "dateOfBirth",
                sortable: true,
                filter: true,
                cellRenderer: function (params) {
                    if (!params.data?.id) return '';
                    return getCommonDateformat(params.data.dateOfBirth);
                }
            },
            { headerName: "Email", field: "email", sortable: true, filter: true },
            { headerName: "Phone No", field: "phoneNumber", sortable: true, filter: true },
            { headerName: "Address", field: "address", sortable: true, filter: true },
        ],
        defaultColDef: {
            sortable: true,
            filter: true,
            cellClass: 'ag-cell-default-style',
            width: 175
        },
        rowSelection: 'single',
        rowClassRules: {
            'selected-row': params => params.node.isSelected()
        },
        onGridReady: function (params) {
            UserGridOptions.api = params.api;
            UserGridOptions.columnApi = params.columnApi;
            params.api.sizeColumnsToFit();
        },
        rowModelType: 'infinite',
        cacheBlockSize: 10,
        datasource: {
            getRows: function (params) {
                const request = {
                    StartRow: params.startRow,
                    PageSize: UserGridOptions.cacheBlockSize || 10,
                    SearchType: "",
                    SortModel: params.sortModel || [],
                    SortColumn: params.sortModel?.[0]?.colId || "",
                    SortDirection: params.sortModel?.[0]?.sort || "",
                    filters: Object.entries(params.filterModel || {}).map(([key, value]) => ({
                        colId: key,
                        filterValue: value.filter
                    })),
                    SearchValue: $('#txtUserSearch').val() || ""
                };

                $.ajax({
                    url: '/UserProfile/GetUserList',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(request),
                    success: function (response) {
                        params.successCallback(response.rowsThisPage, response.totalRowCount);
                    },
                    error: function () {
                        params.failCallback();
                    }
                });
            }
        }
    };

    if (canEdit || canDelete) {
        UserGridOptions.columnDefs.push({
            headerName: "Actions",
            field: "actions",
            sortable: false,
            filter: false,
            cellRenderer: function (params) {
                if (!params.data?.id) return '';

                let buttons = '';
                if (canEdit) {
                    buttons += `<a title="Edit" href="/UserProfile/UserInfo/?Id=${params.data.id}" aria-label="Edit">
                        <i class="fa-solid fa-pen-to-square"></i></a>`;
                }


                return buttons;
            }
        });
    }

    const gridElement = document.querySelector('#UserTable');
    agGrid.createGrid(gridElement, UserGridOptions);

    $('#txtUserSearch').on('change keyup', function () {
        if (UserGridOptions.api) {
            UserGridOptions.api.onFilterChanged();
        }
    });
});




$(document).ready(function () {
    GetUserAttendanceInTime();
    UserBirsthDayWish();
    clearSelectedBox();
    GetUserRoleList();
});

$(document).ready(function () {
    $("#frmuserDetails").validate({
        rules: {
            firstnameInput: "required",
            lastnameInput: "required",
            birthdateInput: "required",
            genderInput: "required",
            emailInput: {
                required: true,
                email: true
            },
            phonenumberInput: {
                required: true,
                digits: true,
                minlength: 10,
                maxlength: 10
            },
            addressInput: "required",
        },
        messages: {
            firstnameInput: "Please Enter FirstName",
            lastnameInput: "Please Enter LastName",
            birthdateInput: "Please Enter DateOfBirth",
            genderInput: "Please Enter Gender",
            emailInput: {
                required: "Please Enter Email",
                email: "Please enter a valid email address"
            },
            phonenumberInput: {
                required: "Please Enter phone number",
                digits: "phone number must contain only digits",
                minlength: "phone number must be 10 digits long",
                maxlength: "phone number must be 10 digits long"
            },
            addressInput: "Please Enter Address",
        },
        errorPlacement: function (error, element) {

            if (element.hasClass("select2-hidden-accessible")) {
                error.insertAfter(element.next('.select2-container'));
            }
            else {
                error.insertAfter(element);
            }
        }
    })
    $('#btnUpdateDetails').on('click', function () {
        $("#frmuserDetails").validate();
    });
    $('.select2').on('change', function () {
        $(this).valid();
    });
});

function GetUserRoleList(itemId, selectedRoleId) {
    $.ajax({
        url: '/UserProfile/RolewisePermissionListAction',
        success: function (result) {
            var roleDropdown = $('#ddlUserRole_' + itemId);
            roleDropdown.empty();
            $.each(result, function (i, data) {
                var selected = data.roleId == selectedRoleId ? 'selected' : '';
                roleDropdown.append('<option value=' + data.roleId + ' ' + selected + '>' + data.role + '</option>');
            });
        }
    });
}
function GetDepartmentList(itemId, selectedDepartmentId) {
    $.ajax({
        url: '/Authentication/GetDepartment',
        success: function (result) {
            var departmentDropdown = $('#ddlDepartment_' + itemId);
            departmentDropdown.empty();
            $.each(result, function (i, data) {
                var selected = data.id == selectedDepartmentId ? 'selected' : '';
                departmentDropdown.append('<option value=' + data.id + ' ' + selected + '>' + data.departments + '</option>');
            });
        }
    });
}
function UserActiveDeactive(UserId, checkboxElement) {

    UpdatedBy = $("#txtUpdatedById").val();
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
                url: '/UserProfile/UserActiveDecative?UserId=' + UserId + '&UpdatedBy=' + UpdatedBy,
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
        UpdatedBy: $("#txtUpdatedById").val(),
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
                toastr.error(result.message);
            }
        },
        error: function (xhr, status, error) {
            toastr.error(
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

            if (Result.code == 200) {

                Swal.fire({
                    title: Result.message,
                    icon: Result.icone,
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK'
                })
                GetUserAttendanceInTime();
            }
            else {
                toastr.warning(Result.message);
            }
        },
    })
}
function EnterOutTime() {
    const isPending = $("#todayouttime").text() === "Pending";
    const InTimeStr = $('#todayintime').text();
    const currentTime = new Date();
    const InTimeParts = InTimeStr.match(/(\d+):(\d+) (\w+)/);
    const InTime = new Date(currentTime);
    InTime.setHours(parseInt(InTimeParts[1]));
    InTime.setMinutes(parseInt(InTimeParts[2]));
    const ampm = InTimeParts[3].toUpperCase();
    if (ampm === 'PM' && InTime.getHours() !== 12) {
        InTime.setHours(InTime.getHours() + 12);
    } else if (ampm === 'AM' && InTime.getHours() === 12) {
        InTime.setHours(0);
    }
    const TotalMinutes = (currentTime - InTime) / 60000;

    const userId = $("#txtuserid").val();
    const formData = new FormData();
    formData.append("UserId", userId);
    formData.append("OutTime", currentTime.toISOString());

    const ajaxCall = () => {
        $.ajax({
            url: '/Home/EnterUserOutTime',
            type: 'POST',
            data: formData,
            dataType: 'json',
            processData: false,
            contentType: false,
            success: function (Result) {
                if (Result.code === 200) {
                    Swal.fire({
                        text: Result.message,
                        icon: "success",
                        confirmButtonClass: "btn btn-primary w-xs mt-2",
                        buttonsStyling: false
                    });
                    GetUserAttendanceInTime();
                } else {
                    toastr.warning(Result.message);
                }
            }
        });
    };

    if (isPending) {
        if (TotalMinutes < 60) {

            toastr.warning("You can't enter out-time now!");
        } else {

            Swal.fire({
                title: "Are you sure you want to enter out-time?",
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
                    ajaxCall();
                } else if (result.dismiss === Swal.DismissReason.cancel) {
                    Swal.fire(
                        'Cancelled',
                        'No changes were made!',
                        'error'
                    );
                }
            });
        }
    } else {
        ajaxCall();
    }
}
function ResetPassword() {
    var form = document.getElementById('resetPasswordForm');
    if (form.checkValidity()) {
        var objData = {
            UserId: $('#ddlusername').val(),
            Password: $('#password-input').val(),
            ConfirmPassword: $('#confirm-password-input').val(),
        }
        $.ajax({
            url: '/UserProfile/ResetUserPassword',
            type: 'post',
            data: objData,
            datatype: 'json',
            success: function (Result) {
                if (Result.code == 200) {
                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        logoutAfterPasswordReset();
                    });
                }
                else {
                    toastr.error(Result.message);
                }
            },
        })
    } else {
        toastr.warning("Kindly Fill all Datafields.")
        form.reportValidity();
    }
}

function logoutAfterPasswordReset() {
    sessionStorage.removeItem('SelectedProjectId');
    sessionStorage.removeItem('SelectedUserProjectId');
    sessionStorage.removeItem('SelectedCityName');
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
            toastr.error('Error:', error);

        });
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
            if (Result.code == 200) {
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
        error: function () {
            toastr.error("Can't get Data");
        }
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
function GetActiveDeactiveList(page) {
    DepartmentId = $("#UADDepartmentListHidden").val();
    Id = $("#UADUserListHidden").val();

    $.get("/UserProfile/UserActiveDecativeList", { DepartmentId: DepartmentId, Id: Id, page: page })
        .done(function (result) {

            $("#activedeactivepartial").html(result);
        })
        .fail(function (error) {
            toastr.error(error);
        });
}

GetActiveDeactiveList(1);
var searchValue;
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
function clearsearchtextbox() {
    $("#UADDepartmentListHidden").val('');
    $("#UADDepartmentList").val('');
    $("#UADUserList").val('');
    $("#UADUserListHidden").val('');
    $("#ddlUACSearch").val('');
}

function GetAllUserList() {
    $.ajax({
        url: '/Task/GetUserName',
        method: 'GET',
        success: function (result) {
            var unitTypes = result.map(function (data) {
                return {
                    label: data.firstName + ' ' + data.lastName + ' (' + data.userName + ')',
                    value: data.id
                };
            });


            $("#UADUserList").autocomplete({
                source: unitTypes,
                minLength: 0,
                select: function (event, ui) {

                    event.preventDefault();
                    $("#UADUserList").val(ui.item.label);
                    $("#UADUserListHidden").val(ui.item.value);

                }
            }).focus(function () {
                $(this).autocomplete("search");
            });
        },
        error: function (err) {
            console.error("Failed to fetch user list: ", err);
        }
    });
}


function GetAllDepartmentList() {
    $.ajax({
        url: '/Authentication/GetDepartment',
        method: 'GET',
        success: function (result) {
            var unitTypes = result.map(function (data) {
                return {
                    label: data.departments,
                    value: data.id
                };
            });


            $("#UADDepartmentList").autocomplete({
                source: unitTypes,
                minLength: 0,
                select: function (event, ui) {

                    event.preventDefault();
                    $("#UADDepartmentList").val(ui.item.label);
                    $("#UADDepartmentListHidden").val(ui.item.value);

                }
            }).focus(function () {
                $(this).autocomplete("search");
            });
        },
        error: function (err) {
            console.error("Failed to fetch department list: ", err);
        }
    });
}
$(document).ready(function () {
    $('#drpCuDepartment').select2({
        placeholder: 'Select Department',
        width: '100%',
        dropdownAutoWidth: true,
        allowClear: true,
        minimumResultsForSearch: Infinity,
        ajax: {
            url: '/Authentication/GetDepartment',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                return {
                    results: data.map(item => ({
                        id: item.id,
                        text: item.departments
                    }))
                };
            }
        }
    }).on('select2:open', function () {
        document.querySelector('.select2-container--open .select2-dropdown').style.marginTop = '5px';
    });

    $('#drpCuGender').select2({
        placeholder: 'Select Gender',
        width: '100%',
        minimumResultsForSearch: Infinity,
    }).on('select2:open', function () {
        document.querySelector('.select2-container--open .select2-dropdown').style.marginTop = '5px';
    });
});

function UADBackbtn() {
    clearsearchtextbox();
    $("#backBtn").hide();
    $("#usernamebox").hide();
    $("#departmentbox").hide();
    $("#UADSearchbtn").hide();
    $("#ddlUACSearch").show();
    $("#ddlUACSearch").text("Search By");
    GetActiveDeactiveList(1);
}
$('.dropdown-item').click(function () {
    var selectedValue = $(this).attr('data-value');
    $('#ddlUACSearch').data('value', selectedValue);

    if (selectedValue === "UserName") {
        clearsearchtextbox();
        GetAllUserList();
        $("#usernamebox").show();
        $("#departmentbox").hide();
        $("#UADSearchbtn").show();
    }
    if (selectedValue === "Department") {
        clearsearchtextbox();
        GetAllDepartmentList();
        $("#usernamebox").hide();
        $("#departmentbox").show();
        $("#UADSearchbtn").show();
    }
    $('.btn-group #ddlUACSearch').text($(this).text());
});

function GetUserSearchData() {
    var selectedValue = $('#ddlUACSearch').data('value');
    var isValid = true;
    var errorMessage = "Kindly fill all required fields";
    if (typeof selectedValue === "undefined") {
        isValid = false;
        errorMessage = "Please select a search criteria";
    } else if (selectedValue === "UserName" && $("#UADUserListHidden").val() === "") {
        isValid = false;
        errorMessage = "Please select a Username";
    } else if (selectedValue === "Department" && $("#UADDepartmentListHidden").val() === "") {
        isValid = false;
        errorMessage = "Please select a Department";
    }
    if (isValid) {

        $("#backBtn").show();
        GetActiveDeactiveList(1);
    }
    else {
        $("#backBtn").hide();
        toastr.warning(errorMessage);
    }
}
function EdituserDetails() {

    document.getElementById("firstnameInput").removeAttribute("readonly");
    document.getElementById("lastnameInput").removeAttribute("readonly");
    document.getElementById("phonenumberInput").removeAttribute("readonly");
    document.getElementById("emailInput").removeAttribute("readonly");
    document.getElementById("birthdateInput").removeAttribute("readonly");
    document.getElementById("addressInput").removeAttribute("readonly");
    document.getElementById("genderInput").removeAttribute("disabled");

    //document.querySelectorAll('#frmuserDetails input, #frmuserDetails select, #frmuserDetails textarea').forEach(function (element) {
    //    element.disabled = false;
    //});

    populateGenderOptions();
    $("#btnEdit").hide();
    $("#btnUpdateDetails").show();
}

function populateGenderOptions() {

    var genderSelect = document.getElementById("genderInput");
    var selectedGender = genderSelect.value;

    if (genderSelect.options.length === 2) {
        genderSelect.innerHTML = `
            <option value="Male">Male</option>
            <option value="Female">Female</option>
            <option value="Other">Other</option>
        `;
        genderSelect.value = selectedGender;
    }
}
function updateuserDetails() {
    if ($('#frmuserDetails').valid()) {
        var UserId = $('#UseridInput').val()
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
            RoleId: $('#textUserRole').val(),

        }
        $.ajax({
            url: '/UserProfile/UpdateUserDetails',
            type: 'post',
            data: objData,
            datatype: 'json',
            success: function (Result) {
                if (Result.code == 200) {
                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/UserProfile/UserInfo/?Id=' + UserId;
                    });
                }
                else {
                    toastr.error(Result.message);
                }
            },
        })
    } else {
        toastr.warning("Kindly fill all datafield");
    }
}
$(document).ready(function () {
    $("#CreateUserForm").validate({

        rules: {
            txtCuFirstname: "required",
            textCuPhoneNumber: {
                required: true,
                digits: true,
                minlength: 10,
                maxlength: 10
            },
            txtCuEmail: {
                required: true,
                email: true
            },
            txtCuLastname: "required",
            drpCuDepartment: "required",
            textCuAddress: "required",
            txtCuDOB: "required",
            drpCuGender: "required",
            drpCuCountry: "required",
            drpCuState: "required",
            drpCuCity: "required",
            passwordinput: "required",
            confirmpasswordinput: "required",
        },
        messages: {
            txtCuFirstname: "Please Enter First Name",
            textCuPhoneNumber: {
                required: "Please Enter phone number",
                digits: "phone number must contain only digits",
                minlength: "phone number must be 10 digits long",
                maxlength: "phone number must be 10 digits long"
            },
            txtCuEmail: {
                required: "Please Enter Email",
                email: "Please enter a valid email address"
            },
            txtCuLastname: "Please Enter Last Name",
            drpCuDepartment: "Please Select Department",
            textCuAddress: "Please Enter Address",
            txtCuDOB: "Please Enter Date of Birth",
            drpCuGender: "Please Select Gender",
            drpCuCountry: "Please Select Country",
            drpCuState: "Please Select State",
            drpCuCity: "Please Select City",
            passwordinput: "Please Enter Password",
            confirmpasswordinput: "Please Enter Confirm Password",
        },
        errorPlacement: function (error, element) {

            if (element.hasClass("select2-hidden-accessible")) {
                error.insertAfter(element.next('.select2-container'));
            }
            else {
                error.insertAfter(element);
            }
        }
    })
    $('.select2').on('change', function () {
        $(this).valid();
    });
});
function CreateUser() {
    var form = document.getElementById('CreateUserForm');
    if ($("#CreateUserForm").valid()) {
        if (form.checkValidity()) {
            var formData = new FormData();
            formData.append("UserName", $('#EmpId').val());
            formData.append("FirstName", $('#txtCuFirstname').val());
            formData.append("LastName", $('#txtCuLastname').val());
            formData.append("DepartmentId", $('#drpCuDepartment').val());
            formData.append("Email", $('#txtCuEmail').val());
            formData.append("Address", $('#textCuAddress').val());
            formData.append("PhoneNumber", $('#textCuPhoneNumber').val());
            formData.append("DateOfBirth", $('#txtCuDOB').val());
            formData.append("Gender", $('#drpCuGender').val());
            formData.append("CountryId", $('#drpCuCountry').val());
            formData.append("StateId", $('#drpCuState').val());
            formData.append("CityId", $('#drpCuCity').val());
            formData.append("Password", $('#passwordinput').val());
            formData.append("Image", $('#filecuImage')[0].files[0]);
            $.ajax({
                url: '/UserProfile/CreateUser',
                type: 'post',
                data: formData,
                processData: false,
                contentType: false,
                datatype: 'json',
                success: function (Result) {
                    if (Result.code == 200) {
                        Swal.fire({
                            title: Result.message,
                            icon: 'success',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'OK'
                        }).then(function () {
                            window.location = '/UserProfile/UserList';
                        });
                    }
                    else {
                        toastr.error(Result.message);
                    }
                },
            })
        }
        else {
            form.reportValidity();
        }
    }
    else {
        toastr.warning("Kindly Fill all Datafields.")
    }
}
function EditExperienceDate() {

    document.getElementById("experiencedate").removeAttribute("readonly");
    $("#btnEditExeperience").hide();
    $("#btnUpdateExeperience").show();

}
function updateExperienceDate() {
    if ($('#frmExperience').valid()) {
        var UserId = $('#UseridInput').val()
        var objData = {
            Id: UserId,
            LastDate: $('#experiencedate').val(),
        }
        $.ajax({
            url: '/UserProfile/UpdateUserExeperience',
            type: 'post',
            data: objData,
            datatype: 'json',
            success: function (Result) {
                if (Result.code == 200) {
                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/UserProfile/UserInfo/?Id=' + UserId;
                    });
                }
                else {
                    toastr.error(Result.message);
                }
            },
        })
    } else {
        toastr.warning("Kindly fill all datafield");
    }
}
function fn_UpdateUserPassword() {
    var form = document.getElementById('UserInfoPasswordForm');
    if (form.checkValidity()) {
        var objData = {
            UserId: $('#UseridInput').val(),
            Password: $('#UserInfopassword').val(),
            ConfirmPassword: $('#UserInfoconfirmpassword').val(),
        }
        $.ajax({
            url: '/UserProfile/ResetUserPassword',
            type: 'post',
            data: objData,
            datatype: 'json',
            success: function (Result) {
                if (Result.code == 200) {
                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/UserProfile/UserInfo/?Id=' + UserId;
                    });
                }
                else {
                    toastr.error(Result.message);
                }
            },
        })
    } else {
        form.reportValidity();
    }
}
AllRolewiseFormUserTable();
function AllRolewiseFormUserTable() {

    $.get("/UserProfile/GetRolewiseFormPermissionList")
        .done(function (result) {

            $("#UserRoletbody").html(result);
        })
        .fail(function (error) {
            siteloaderhide();

        });
}
function ClearUserTextBox() {
    var offcanvasElement = document.getElementById("createFormGroup");
    var offcanvas = new bootstrap.Offcanvas(offcanvasElement);
    offcanvas.show();
}

function EditRoleWiseFormDetails(RoleId) {
    siteloadershow();

    $.ajax({
        url: '/UserProfile/GetUserRolewiseFormListById?RoleId=' + RoleId,
        type: 'post',
        dataType: 'json',
        processData: false,
        contentType: false,
        complete: function (Result) {
            siteloaderhide();
            $('#dveditRolePermissionForm').html(Result.responseText);
            $('#rolePermissionTable').show();
            if (Result.responseText.trim() !== "") {
                $('#userupdatebtn').show();
            } else {
                $('#userupdatebtn').hide();
            }
        },
        Error: function () {
            siteloaderhide();
            toastr.error("Can't get data!");
        }
    });
}

function UpdateRolewiseFormPermission() {
    var formPermissions = [];
    $(".forms").each(function () {

        var rolewiseformRow = $(this);
        var objData = {
            RoleId: rolewiseformRow.find('#txtUserRoleId').val(),
            CreatedBy: $("#txtuserId").val(),
            FormId: rolewiseformRow.find('#txtuserformId').val(),
            IsAddAllow: rolewiseformRow.find('#isUserAdd_' + rolewiseformRow.data('product-id')).prop('checked'),
            IsViewAllow: rolewiseformRow.find('#isUserView_' + rolewiseformRow.data('product-id')).prop('checked'),
            IsEditAllow: rolewiseformRow.find('#isUserEdit_' + rolewiseformRow.data('product-id')).prop('checked'),
            IsDeleteAllow: rolewiseformRow.find('#isUserDelete_' + rolewiseformRow.data('product-id')).prop('checked'),
        };
        formPermissions.push(objData);
    });

    var form_data = new FormData();
    form_data.append("RolewisePermissionDetails", JSON.stringify(formPermissions));

    $.ajax({
        url: '/UserProfile/UpdateUserMultipleRolewiseFormPermission',
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
function toggleUserCheckboxes(formId) {
    var isChecked = document.getElementById("usercheckboxAll_" + formId).checked;
    document.getElementById("isUserAdd_" + formId).checked = isChecked;
    document.getElementById("isUserView_" + formId).checked = isChecked;
    document.getElementById("isUserEdit_" + formId).checked = isChecked;
    document.getElementById("isUserDelete_" + formId).checked = isChecked;
}
function updateUserSelectAll(formId) {
    const isUserAdd = document.getElementById(`isUserAdd_${formId}`);
    const isUserView = document.getElementById(`isUserView_${formId}`);
    const isUserEdit = document.getElementById(`isUserEdit_${formId}`);
    const isUserDelete = document.getElementById(`isUserDelete_${formId}`);
    const usercheckboxAll = document.getElementById(`usercheckboxAll_${formId}`);

    const allChecked = isUserAdd.checked && isUserView.checked && isUserEdit.checked && isUserDelete.checked;

    usercheckboxAll.checked = allChecked;
}
function userRoleAllCheckboxes(masterCheckbox) {
    var checkboxes = document.querySelectorAll('.form-check-input-all, .alluser-checkbox');
    checkboxes.forEach(function (checkbox) {
        checkbox.checked = masterCheckbox.checked;
    });
}
function RoleActiveDecative(roleId) {

    var isChecked = $('#flexSwitchCheckChecked_' + roleId).is(':checked');
    var confirmationMessage = isChecked ? "Are you sure want to active this role?" : "Are you sure want to deactive this role?";

    Swal.fire({
        title: confirmationMessage,
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
            formData.append("RoleId", roleId);

            $.ajax({
                url: '/UserProfile/RoleActiveDecative?RoleId=' + roleId,
                type: 'Post',
                contentType: 'application/json;charset=utf-8;',
                dataType: 'json',
                success: function (Result) {
                    siteloaderhide();
                    if (Result.code == 200) {
                        siteloaderhide();
                        Swal.fire({
                            title: isChecked ? "Active!" : "Deactive!",
                            text: Result.message,
                            icon: "success",
                            confirmButtonClass: "btn btn-primary w-xs mt-2",
                            buttonsStyling: false
                        }).then(function () {
                            window.location = '/UserProfile/UserRolePermission';
                        });
                    } else {
                        siteloaderhide();
                        toastr.error(Result.message);
                    }

                }
            });
        } else if (result.dismiss === Swal.DismissReason.cancel) {

            Swal.fire(
                'Cancelled',
                'Role have no changes.!!😊',
                'error'
            ).then(function () {
                window.location = '/UserProfile/UserRolePermission';
            });;
        }
    });
}
