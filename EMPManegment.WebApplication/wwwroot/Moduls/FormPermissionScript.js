
var Formdata = window.userFormPermissions || 0;

let RolePermissionGridOptions = [];
let currentRoleId = null;

$(document).ready(function () {

    $('#ddlRoleWiseFormPermission').select2({
        placeholder: 'Select Role',
        width: '100%',
        dropdownAutoWidth: true,
        allowClear: true,
        ajax: {
            url: '/UserProfile/RolewisePermissionListAction',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                return {
                    results: data?.map(item => ({
                        id: item.roleId,
                        text: item.role
                    })) || []
                };
            }
        }
    }).on('select2:open', function () {
        document.querySelector('.select2-container--open .select2-dropdown').style.marginTop = '5px';
    }).on('change', function () {
        if ($(this).val()) {
            $('#updatebtn').show();
        } else {
            $('#updatebtn').hide();
        }
    });

    $('#ddlRoleWiseFormPermission').on('select2:select', function (e) {
        currentRoleId = e.params.data.id;
        RolePermissionGridOptions.api.onFilterChanged();
    });

    $('#ddlRoleWiseFormPermission').on('select2:unselect', function () {
        currentRoleId = null;
        if (RolePermissionGridOptions.api) {
            RolePermissionGridOptions.api.showNoRowsOverlay();
        }
    });

    const RolePermissionGridOptionsLocal = {
        rowHeight: 60,
        columnDefs: [
            {
                headerName: "Form Name",
                field: "formName",
                sortable: true,
                filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.formId) {
                        return '';
                    }
                    return `
                        <h6 class="pt-1" style="color:#16989A; font-weight:600;">${params.data.formName}</h6>
                        <input type="hidden" data-role-id="${params.data.roleId}" />
                        <input type="hidden" data-form-id="${params.data.formId}" />
                    `;
                }
            },
            ...['Add', 'View', 'Edit', 'Delete'].map(type => ({
                headerName: type,
                field: `is${type}Allow`,
                sortable: false,
                filter: false,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.formId) return '';

                    const key = type.toLowerCase();
                    const checkboxId = `${key}_${params.data.formId}`;
                    const checked = params.value ? 'checked' : '';

                    setTimeout(() => {
                        const checkbox = document.getElementById(checkboxId);
                        if (checkbox) {
                            checkbox.onchange = function () {
                                params.data[`is${type}Allow`] = checkbox.checked;
                                updateSelectAll(params.data.formId);
                            };
                        }
                    }, 0);

                    return `
                        <div class="custom-control custom-switch">
                            <input class="custom-control-input toggle-checkbox" type="checkbox" 
                                id="${checkboxId}" name="${key}" ${checked}>
                            <label class="custom-control-label" for="${checkboxId}"></label>
                        </div>
                    `;
                }
            })),
            {
                headerName: "Select All",
                field: "selectAll",
                sortable: false,
                filter: false,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.formId) return '';

                    const checkboxId = `checkboxAll_${params.data.formId}`;
                    const allChecked = params.data.isAddAllow && params.data.isViewAllow &&
                        params.data.isEditAllow && params.data.isDeleteAllow;

                    setTimeout(() => {
                        const checkbox = document.getElementById(checkboxId);
                        if (checkbox) {
                            checkbox.onchange = function () {
                                const checked = checkbox.checked;
                                ['isAddAllow', 'isViewAllow', 'isEditAllow', 'isDeleteAllow'].forEach(key => {
                                    params.data[key] = checked;
                                    const field = key.toLowerCase().replace('is', '');
                                    const subCheckbox = document.getElementById(`${field}_${params.data.formId}`);
                                    if (subCheckbox) subCheckbox.checked = checked;
                                });
                            };
                        }
                    }, 0);

                    return `
                        <div class="custom-control custom-switch">
                            <input class="custom-control-input form-check-input-all" type="checkbox" onclick="toggleCheckboxes('${params.data.formId}')" 
                                id="${checkboxId}" ${allChecked ? 'checked' : ''}>
                            <label class="custom-control-label" for="${checkboxId}"></label>
                        </div>
                    `;
                }
            }
        ],
        defaultColDef: {
            sortable: true,
            filter: true,
            resizable: true,
            cellClass: 'ag-cell-default-style',
            flex: 1
        },
        rowSelection: 'single',
        rowClassRules: {
            'selected-row': params => params.node.isSelected()
        },
        onGridReady: function (params) {
            RolePermissionGridOptionsLocal.api = params.api;
            RolePermissionGridOptionsLocal.columnApi = params.columnApi;
            RolePermissionGridOptionsLocal.api.showNoRowsOverlay();
            params.api.sizeColumnsToFit();
        },
        rowModelType: 'infinite',
        cacheBlockSize: 100,
        datasource: {
            getRows: function (params) {
                if (!currentRoleId) {
                    RolePermissionGridOptionsLocal.api.showNoRowsOverlay();
                    params.successCallback([], 0);
                    return;
                }

                const sortModel = params.sortModel?.[0] || {};
                const request = {
                    RoleId: currentRoleId,
                    StartRow: params.startRow,
                    PageSize: RolePermissionGridOptionsLocal.cacheBlockSize || 10,
                    SearchType: "",
                    SearchValue: "",
                    SortModel: Array.isArray(params.sortModel) ? params.sortModel : [],
                    SortColumn: sortModel.colId || "",
                    SortDirection: sortModel.sort || "",
                    filters: Object.entries(params.filterModel || {}).map(([key, value]) => ({
                        colId: key,
                        filterValue: value.filter
                    }))
                };

                $.ajax({
                    url: '/UserProfile/GetRolewiseFormListById',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(request),
                    success: function (response) {
                        if (response.data?.length > 0) {
                            const mappedData = response.data.map(item => ({
                                formName: item.formName,
                                formId: item.formId,
                                roleId: item.roleId,
                                isAddAllow: item.isAddAllow,
                                isViewAllow: item.isViewAllow,
                                isEditAllow: item.isEditAllow,
                                isDeleteAllow: item.isDeleteAllow
                            }));
                            params.successCallback(mappedData, response.recordsTotal);
                            RolePermissionGridOptionsLocal.api.hideOverlay();
                        } else {
                            RolePermissionGridOptionsLocal.api.showNoRowsOverlay();
                            params.successCallback([], 0);
                        }
                    },
                    error: function () {
                        RolePermissionGridOptionsLocal.api.showNoRowsOverlay();
                        params.failCallback();
                    }
                });
            }
        }
    };

    RolePermissionGridOptions = RolePermissionGridOptionsLocal;

    const gridElement = document.querySelector('#RolePermissionGrid');
    agGrid.createGrid(gridElement, RolePermissionGridOptions);
});

function toggleCheckboxes(formId) {

    const selectAllCheckbox = document.getElementById(`checkboxAll_${formId}`);
    const isChecked = selectAllCheckbox.checked;

    const checkboxes = [
        document.getElementById(`add_${formId}`),
        document.getElementById(`view_${formId}`),
        document.getElementById(`edit_${formId}`),
        document.getElementById(`delete_${formId}`)
    ];

    checkboxes.forEach(checkbox => {
        if (checkbox) {
            checkbox.checked = isChecked;

            const event = new Event('change');
            checkbox.dispatchEvent(event);
        }
    });
}

function updateSelectAll(formId) {

    const checkboxes = [
        document.getElementById(`add_${formId}`),
        document.getElementById(`view_${formId}`),
        document.getElementById(`edit_${formId}`),
        document.getElementById(`delete_${formId}`)
    ];

    const selectAllCheckbox = document.getElementById(`checkboxAll_${formId}`);

    const allChecked = checkboxes.every(checkbox => checkbox && checkbox.checked);

    if (selectAllCheckbox) {
        selectAllCheckbox.checked = allChecked;
    }
}

$('#drpAttusername').change(function () {
    var Text = $("#drpAttusername Option:Selected").text();
    $("#textUserIdfrm").val(Text);
});

function UpdateRolewiseFormPermission() {
    if (!RolePermissionGridOptions.api) return;

    const formPermissions = [];
    RolePermissionGridOptions.api.forEachNode(node => {
        const data = node.data;
        if (!data || !data.formId) return;

        formPermissions.push({
            RoleId: data.roleId,
            CreatedBy: $("#txtUserId").val(),
            FormId: data.formId,
            IsAddAllow: data.isAddAllow,
            IsViewAllow: data.isViewAllow,
            IsEditAllow: data.isEditAllow,
            IsDeleteAllow: data.isDeleteAllow
        });
    });

    if (formPermissions.length === 0) {
        toastr.warning("No permission data found to update.");
        return;
    }

    const form_data = new FormData();
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
                });
            } else {
                toastr.error(Result.message);
            }
        },
        error: function (xhr, status, error) {
            toastr.error(error);
        }
    });
}

let UserPermissionGridOptions = [];
let currentUserId = null;

$(document).ready(function () {

    $('#usercustomDropdown').select2({
        placeholder: 'Select User',
        width: '100%',
        dropdownAutoWidth: true,
        allowClear: true,
        ajax: {
            url: '/Task/GetUserName',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                return {
                    results: data.map(item => ({
                        id: item.id,
                        text: item.firstName + ' ' + item.lastName
                    }))
                };
            }
        }
    }).on('select2:open', function () {
        document.querySelector('.select2-container--open .select2-dropdown').style.marginTop = '5px';
    }).on('change', function () {
        if ($(this).val()) {
            $('#userupdatebtn').show();
        } else {
            $('#userupdatebtn').hide();
        }
    });

    $('#usercustomDropdown').on('select2:select', function (e) {
        currentUserId = e.params.data.id;
        UserPermissionGridOptions.api.onFilterChanged();
    });

    $('#usercustomDropdown').on('select2:unselect', function () {
        currentUserId = null;
        if (UserPermissionGridOptions.api) {
            UserPermissionGridOptions.api.showNoRowsOverlay();
        }
    });

    const UserPermissionGridOptionsLocal = {
        rowHeight: 60,
        columnDefs: [
            {
                headerName: "Form Name",
                field: "formName",
                sortable: true,
                filter: true,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.formId) {
                        return '';
                    }
                    return `
                        <h6 class="pt-1" style="color:#16989A; font-weight:600;">${params.data.formName}</h6>
                        <input type="hidden" data-role-id="${params.data.roleId}" />
                        <input type="hidden" data-form-id="${params.data.formId}" />
                    `;
                }
            },
            ...['Add', 'View', 'Edit', 'Delete'].map(type => ({
                headerName: type,
                field: `is${type}Allow`,
                sortable: false,
                filter: false,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.formId) return '';

                    const key = type.toLowerCase();
                    const checkboxId = `${key}_${params.data.formId}`;
                    const checked = params.value ? 'checked' : '';

                    setTimeout(() => {
                        const checkbox = document.getElementById(checkboxId);
                        if (checkbox) {
                            checkbox.onchange = function () {
                                params.data[`is${type}Allow`] = checkbox.checked;
                                updateUserFormSelectAll(params.data.formId);
                            };
                        }
                    }, 0);

                    return `
                        <div class="custom-control custom-switch">
                            <input class="custom-control-input toggle-checkbox" type="checkbox" 
                                id="${checkboxId}" name="${key}" ${checked}>
                            <label class="custom-control-label" for="${checkboxId}"></label>
                        </div>
                    `;
                }
            })),
            {
                headerName: "Select All",
                field: "selectAll",
                sortable: false,
                filter: false,
                cellRenderer: function (params) {
                    if (!params.data || !params.data.formId) return '';

                    const checkboxId = `checkboxAll_${params.data.formId}`;
                    const allChecked = params.data.isAddAllow && params.data.isViewAllow &&
                        params.data.isEditAllow && params.data.isDeleteAllow;

                    setTimeout(() => {
                        const checkbox = document.getElementById(checkboxId);
                        if (checkbox) {
                            checkbox.onchange = function () {
                                const checked = checkbox.checked;
                                ['isAddAllow', 'isViewAllow', 'isEditAllow', 'isDeleteAllow'].forEach(key => {
                                    params.data[key] = checked;
                                    const field = key.toLowerCase().replace('is', '');
                                    const subCheckbox = document.getElementById(`${field}_${params.data.formId}`);
                                    if (subCheckbox) subCheckbox.checked = checked;
                                });
                            };
                        }
                    }, 0);

                    return `
                        <div class="custom-control custom-switch">
                            <input class="custom-control-input form-check-input-all" type="checkbox" onclick="toggleUserFormCheckboxes('${params.data.formId}')" 
                                id="${checkboxId}" ${allChecked ? 'checked' : ''}>
                            <label class="custom-control-label" for="${checkboxId}"></label>
                        </div>
                    `;
                }
            }
        ],
        defaultColDef: {
            sortable: true,
            filter: true,
            resizable: true,
            cellClass: 'ag-cell-default-style',
            flex: 1
        },
        rowSelection: 'single',
        rowClassRules: {
            'selected-row': params => params.node.isSelected()
        },
        onGridReady: function (params) {
            UserPermissionGridOptionsLocal.api = params.api;
            UserPermissionGridOptionsLocal.columnApi = params.columnApi;
            UserPermissionGridOptionsLocal.api.showNoRowsOverlay();
            params.api.sizeColumnsToFit();
        },
        rowModelType: 'infinite',
        cacheBlockSize: 100,
        datasource: {
            getRows: function (params) {
                if (!currentUserId) {
                    UserPermissionGridOptionsLocal.api.showNoRowsOverlay();
                    params.successCallback([], 0);
                    return;
                }

                const sortModel = params.sortModel?.[0] || {};
                const request = {
                    UserId: currentUserId,
                    StartRow: params.startRow,
                    PageSize: UserPermissionGridOptionsLocal.cacheBlockSize || 10,
                    SearchType: "",
                    SearchValue: "",
                    SortModel: Array.isArray(params.sortModel) ? params.sortModel : [],
                    SortColumn: sortModel.colId || "",
                    SortDirection: sortModel.sort || "",
                    filters: Object.entries(params.filterModel || {}).map(([key, value]) => ({
                        colId: key,
                        filterValue: value.filter
                    }))
                };

                $.ajax({
                    url: '/UserProfile/GetUserFormListById',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(request),
                    success: function (response) {
                        if (response.data?.length > 0) {
                            const mappedData = response.data.map(item => ({
                                formName: item.formName,
                                formId: item.formId,
                                userId: item.userId,
                                isAddAllow: item.isAddAllow,
                                isViewAllow: item.isViewAllow,
                                isEditAllow: item.isEditAllow,
                                isDeleteAllow: item.isDeleteAllow
                            }));
                            params.successCallback(mappedData, response.recordsTotal);
                            UserPermissionGridOptionsLocal.api.hideOverlay();
                        } else {
                            UserPermissionGridOptionsLocal.api.showNoRowsOverlay();
                            params.successCallback([], 0);
                        }
                    },
                    error: function () {
                        UserPermissionGridOptionsLocal.api.showNoRowsOverlay();
                        params.failCallback();
                    }
                });
            }
        }
    };

    UserPermissionGridOptions = UserPermissionGridOptionsLocal;

    const gridElement = document.querySelector('#UserPermissionTable');
    agGrid.createGrid(gridElement, UserPermissionGridOptions);
});
function toggleUserFormCheckboxes(formId) {

    const selectAllCheckbox = document.getElementById(`checkboxAll_${formId}`);
    const isChecked = selectAllCheckbox.checked;

    const checkboxes = [
        document.getElementById(`add_${formId}`),
        document.getElementById(`view_${formId}`),
        document.getElementById(`edit_${formId}`),
        document.getElementById(`delete_${formId}`)
    ];

    checkboxes.forEach(checkbox => {
        if (checkbox) {
            checkbox.checked = isChecked;

            const event = new Event('change');
            checkbox.dispatchEvent(event);
        }
    });
}

function updateUserFormSelectAll(formId) {

    const checkboxes = [
        document.getElementById(`add_${formId}`),
        document.getElementById(`view_${formId}`),
        document.getElementById(`edit_${formId}`),
        document.getElementById(`delete_${formId}`)
    ];

    const selectAllCheckbox = document.getElementById(`checkboxAll_${formId}`);

    const allChecked = checkboxes.every(checkbox => checkbox && checkbox.checked);

    if (selectAllCheckbox) {
        selectAllCheckbox.checked = allChecked;
    }
}

function UpdateUserFormPermission() {
    if (!UserPermissionGridOptions.api) return;

    const UserFormPermissions = [];
    UserPermissionGridOptions.api.forEachNode(node => {
        const data = node.data;
        if (!data || !data.formId) return;

        UserFormPermissions.push({
            UserId: data.userId,
            CreatedBy: $("#txtUserId").val(),
            FormId: data.formId,
            IsAddAllow: data.isAddAllow,
            IsViewAllow: data.isViewAllow,
            IsEditAllow: data.isEditAllow,
            IsDeleteAllow: data.isDeleteAllow
        });
    });

    if (UserFormPermissions.length === 0) {
        toastr.warning("No permission data found to update.");
        return;
    }

    const form_data = new FormData();
    form_data.append("UserPermissionDetails", JSON.stringify(UserFormPermissions));

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
                });
            } else {
                toastr.error(Result.message);
            }
        },
        error: function (xhr, status, error) {
            toastr.error(error);
        }
    });
}

//function UpdateUserFormPermission() {
//    var formPermissions = [];

//    $(".forms").each(function () {
//        var $row = $(this);
//        var formId = $row.data('product-id'); // safer to cache this

//        var objData = {
//            UserId: $row.find(`#textUserId_${formId}`).val(),
//            CreatedBy: $("#textuserId").val(),
//            FormId: $row.find(`#textFormId_${formId}`).val(),
//            IsAddAllow: $(`#txtIsAdd_${formId}`).prop('checked'),
//            IsViewAllow: $(`#txtIsView_${formId}`).prop('checked'),
//            IsEditAllow: $(`#txtIsEdit_${formId}`).prop('checked'),
//            IsDeleteAllow: $(`#txtIsDelete_${formId}`).prop('checked')
//        };

//        formPermissions.push(objData);
//    });

//    var form_data = new FormData();
//    form_data.append("UserPermissionDetails", JSON.stringify(formPermissions));

//    $.ajax({
//        url: '/UserProfile/UpdateUserPermission',
//        type: 'POST',
//        data: form_data,
//        processData: false,
//        contentType: false,
//        dataType: 'json',
//        success: function (result) {
//            if (result.code === 200) {
//                Swal.fire({
//                    title: result.message,
//                    icon: 'success',
//                    confirmButtonColor: '#3085d6',
//                    confirmButtonText: 'OK'
//                });
//            } else {
//                toastr.error(result.message || 'An error occurred while updating permissions.');
//            }
//        },
//        error: function (xhr, status, error) {
//            toastr.error(error || 'Unexpected error occurred.');
//        }
//    });
//}

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
                        window.location = '/UserProfile/FormPermission';
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

$(document).ready(function () {
    $('#drpFormList').select2({
        placeholder: 'Select Form',
        width: '100%',
        dropdownAutoWidth: true,
        allowClear: true,
        ajax: {
            url: '/UserProfile/GetFormNameList',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                return {
                    results: data.map(item => ({
                        id: item.formId,
                        text: item.formName
                    }))
                };
            }
        }
    });
})
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

//function toggleCheckboxes(formId) {
//    var isChecked = document.getElementById("checkboxAll_" + formId).checked;
//    document.getElementById("isAdd_" + formId).checked = isChecked;
//    document.getElementById("isView_" + formId).checked = isChecked;
//    document.getElementById("isEdit_" + formId).checked = isChecked;
//    document.getElementById("isDelete_" + formId).checked = isChecked;


//}
//function updateSelectAll(formId) {
//    const isAdd = document.getElementById(`isAdd_${formId}`);
//    const isView = document.getElementById(`isView_${formId}`);
//    const isEdit = document.getElementById(`isEdit_${formId}`);
//    const isDelete = document.getElementById(`isDelete_${formId}`);
//    const checkboxAll = document.getElementById(`checkboxAll_${formId}`);

//    const allChecked = isAdd.checked && isView.checked && isEdit.checked && isDelete.checked;

//    checkboxAll.checked = allChecked;
//}

function toggleAllCheckboxes(masterCheckbox) {
    var checkboxes = document.querySelectorAll('.form-check-input-all, .toggle-checkbox');
    checkboxes.forEach(function (checkbox) {
        checkbox.checked = masterCheckbox.checked;
    });
}
// When master checkbox (per row) is clicked
function userCheckboxes(formId) {
    var isChecked = document.getElementById("userCheckboxAll_" + formId).checked;

    document.getElementById("txtIsAdd_" + formId).checked = isChecked;
    document.getElementById("txtIsView_" + formId).checked = isChecked;
    document.getElementById("txtIsEdit_" + formId).checked = isChecked;
    document.getElementById("txtIsDelete_" + formId).checked = isChecked;
}

// When any individual permission changes (Add, View, Edit, Delete)
function userUpdateSelectAll(formId) {
    const txtIsAdd = document.getElementById(`txtIsAdd_${formId}`);
    const txtIsView = document.getElementById(`txtIsView_${formId}`);
    const txtIsEdit = document.getElementById(`txtIsEdit_${formId}`);
    const txtIsDelete = document.getElementById(`txtIsDelete_${formId}`);
    const userCheckboxAll = document.getElementById(`userCheckboxAll_${formId}`);

    userCheckboxAll.checked = txtIsAdd.checked && txtIsView.checked && txtIsEdit.checked && txtIsDelete.checked;
}
function userAllCheckboxes(masterCheckbox) {
    const isChecked = masterCheckbox.checked;
    const allRowCheckboxes = document.querySelectorAll('.user-checkbox');

    allRowCheckboxes.forEach(function (checkbox) {
        checkbox.checked = isChecked;
        const formId = checkbox.id.split("_")[1];
        userCheckboxes(formId); // Trigger row-level toggle
    });
}
