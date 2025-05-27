
var Formdata = window.userFormPermissions || 0;

let RolePermissionGridOptions = [];
let currentRoleId = null;

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
    });

    $('#usercustomDropdown').on('select2:select', function (e) {
        var selectedValue = e.params.data.id;
        EditUserFormDetails(selectedValue);
    });

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


    RolePermissionGridOptions = {
        rowHeight: 60,
        columnDefs: [
            {
                headerName: "Form Name",
                field: "formName",
                sortable: true,
                filter: true,
                cellRenderer: function (params) {
                    debugger
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
                    if (!params.data || !params.data.formId) {
                        return '';
                    }
                    const key = type.toLowerCase();
                    return `
                        <div class="custom-control custom-switch">
                            <input class="custom-control-input toggle-checkbox" type="checkbox" 
                                id="${key}_${params.data.formId}" name="${key}" 
                                ${params.value ? 'checked' : ''} 
                                onchange="updateSelectAll('${params.data.formId}')">
                            <label class="custom-control-label" for="${key}_${params.data.formId}"></label>
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
                    if (!params.data || !params.data.formId) {
                        return '';
                    }
                    const allChecked = params.data.isAddAllow && params.data.isViewAllow &&
                        params.data.isEditAllow && params.data.isDeleteAllow;
                    return `
                        <div class="custom-control custom-switch">
                            <input class="custom-control-input form-check-input-all" type="checkbox" 
                                id="checkboxAll_${params.data.formId}" 
                                onclick="toggleCheckboxes('${params.data.formId}')" 
                                ${allChecked ? 'checked' : ''}>
                            <label class="custom-control-label" for="checkboxAll_${params.data.formId}"></label>
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
            RolePermissionGridOptions.api = params.api;
            RolePermissionGridOptions.columnApi = params.columnApi;
            RolePermissionGridOptions.api.showNoRowsOverlay();
            params.api.sizeColumnsToFit();
        },
        rowModelType: 'infinite',
        cacheBlockSize: 100,
        datasource: {
            getRows: function (params) {
                if (!currentRoleId) {
                    RolePermissionGridOptions.api.showNoRowsOverlay();
                    params.successCallback([], 0);
                    return;
                }

                const sortModel = params.sortModel?.[0] || {};
                const request = {
                    RoleId: currentRoleId,
                    StartRow: params.startRow,
                    PageSize: RolePermissionGridOptions.cacheBlockSize || 10,
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
                        debugger
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
                            RolePermissionGridOptions.api.hideOverlay();
                        } else {
                            RolePermissionGridOptions.api.showNoRowsOverlay();
                            params.successCallback([], 0);
                        }
                    },
                    error: function () {
                        RolePermissionGridOptions.api.showNoRowsOverlay();
                        params.failCallback();
                    }
                });
            }
        }
    };

    const gridElement = document.querySelector('#RolePermissionGrid');
    agGrid.createGrid(gridElement, RolePermissionGridOptions);
});


function toggleCheckboxes(formId) {
    const isChecked = document.getElementById(`checkboxAll_${formId}`).checked;
    ['add', 'view', 'edit', 'delete'].forEach(action => {
        const checkbox = document.getElementById(`${action}_${formId}`);
        if (checkbox) checkbox.checked = isChecked;


        const rowNode = RolePermissionGridOptions.api.getRowNode(formId);
        if (rowNode) {
            rowNode.setDataValue(`is${action.charAt(0).toUpperCase() + action.slice(1)}Allow`, isChecked);
        }
    });
}

function updateSelectAll(formId) {
    const allChecked = ['add', 'view', 'edit', 'delete'].every(action => {
        const checkbox = document.getElementById(`${action}_${formId}`);
        return checkbox && checkbox.checked;
    });
    const selectAllCheckbox = document.getElementById(`checkboxAll_${formId}`);
    if (selectAllCheckbox) selectAllCheckbox.checked = allChecked;
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
            RoleId: rolewiseformRow.find('#txtRoleId').val(),
            CreatedBy: $("#txtUserId").val(),
            FormId: rolewiseformRow.find('#formId').val(),
            IsAddAllow: rolewiseformRow.find('#isAdd_' + rolewiseformRow.data('product-id')).prop('checked'),
            IsViewAllow: rolewiseformRow.find('#isView_' + rolewiseformRow.data('product-id')).prop('checked'),
            IsEditAllow: rolewiseformRow.find('#isEdit_' + rolewiseformRow.data('product-id')).prop('checked'),
            IsDeleteAllow: rolewiseformRow.find('#isDelete_' + rolewiseformRow.data('product-id')).prop('checked'),
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

    // Loop through each row with class 'forms'
    $(".forms").each(function () {
        var $row = $(this);
        var formId = $row.data('product-id'); // safer to cache this

        var objData = {
            UserId: $row.find(`#textUserId_${formId}`).val(),
            CreatedBy: $("#textuserId").val(),
            FormId: $row.find(`#textFormId_${formId}`).val(),
            IsAddAllow: $(`#txtIsAdd_${formId}`).prop('checked'),
            IsViewAllow: $(`#txtIsView_${formId}`).prop('checked'),
            IsEditAllow: $(`#txtIsEdit_${formId}`).prop('checked'),
            IsDeleteAllow: $(`#txtIsDelete_${formId}`).prop('checked')
        };

        formPermissions.push(objData);
    });

    var form_data = new FormData();
    form_data.append("UserPermissionDetails", JSON.stringify(formPermissions));

    $.ajax({
        url: '/UserProfile/UpdateUserPermission',
        type: 'POST',
        data: form_data,
        processData: false,
        contentType: false,
        dataType: 'json',
        success: function (result) {
            if (result.code === 200) {
                Swal.fire({
                    title: result.message,
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK'
                });
            } else {
                toastr.error(result.message || 'An error occurred while updating permissions.');
            }
        },
        error: function (xhr, status, error) {
            toastr.error(error || 'Unexpected error occurred.');
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

function toggleCheckboxes(formId) {
    var isChecked = document.getElementById("checkboxAll_" + formId).checked;
    document.getElementById("isAdd_" + formId).checked = isChecked;
    document.getElementById("isView_" + formId).checked = isChecked;
    document.getElementById("isEdit_" + formId).checked = isChecked;
    document.getElementById("isDelete_" + formId).checked = isChecked;


}
function updateSelectAll(formId) {
    const isAdd = document.getElementById(`isAdd_${formId}`);
    const isView = document.getElementById(`isView_${formId}`);
    const isEdit = document.getElementById(`isEdit_${formId}`);
    const isDelete = document.getElementById(`isDelete_${formId}`);
    const checkboxAll = document.getElementById(`checkboxAll_${formId}`);

    const allChecked = isAdd.checked && isView.checked && isEdit.checked && isDelete.checked;

    checkboxAll.checked = allChecked;
}

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
