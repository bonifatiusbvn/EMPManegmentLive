
$(document).ready(function () {

    GetDepartment();
    GetCountry();
    GetVendorTypes();
    GetPaymentMethodList();
    GetPaymentTypeList();
    GetCompanyNameList();
    GetVendorNameList();
    fn_GetAllCities();
    $('#ddlCountry').change(function () {
        var Text = $("#ddlCountry Option:Selected").text();
        var StateId = $(this).val();
        $("#txtcountry").val(Text);
        $.ajax({
            url: '/Authentication/GetState?StateId=' + StateId,
            success: function (result) {

                $.each(result, function (i, data) {
                    $('#ddlState').append('<Option value=' + data.id + '>' + data.stateName + '</Option>')
                });
            }
        });
    });

    $('#ddlState').change(function () {

        var Text = $("#ddlState Option:Selected").text();
        var CityId = $(this).val();
        $("#txtstate").val(CityId);

        $.ajax({
            url: '/Authentication/GetCity?CityId=' + CityId,
            success: function (result) {
                $.each(result, function (i, data) {
                    $('#ddlCity').append('<Option value=' + data.id + '>' + data.cityName + '</Option>');

                });
            }
        });
    });
    $('#dropVendorState').change(function () {
        var txtVendorid = $(this).val();
        $("#txtVendorstate").val(txtVendorid);
    });
    $('#VendorCity').change(function () {
        var txtVendorcity = $(this).val();
        $("#txtVendorCity").val(txtVendorcity);
    });

    $('#dropCompanyState').change(function () {
        var txtCompanyid = $(this).val();
        $("#txtCompanystate").val(txtCompanyid);
    });
    $('#tCompanyCity').change(function () {
        var txtCompanycity = $(this).val();
        $("#txttCompanyCity").val(txttCompanycity);
    });
    $('#dropProjectState').change(function () {

        var txtProjectid = $(this).val();
        $("#txtProjectstate").val(txtProjectid);
    });
    $('#ProjectCity').change(function () {
        var txtProjectcity = $(this).val();
        $("#txtProjectCity").val(txtProjectcity);
    });
    $('#dropUserState').change(function () {
        var txtUserid = $(this).val();
        $("#txtUserstate").val(txtUserid);
    });
    $('#drpCuCity').change(function () {
        var txtUsercity = $(this).val();
        $("#txtUserCity").val(txtUsercity);
    });
});

function UserLogout() {
    Swal.fire({
        title: 'Logout confirmation',
        text: 'Are you sure you want to logout?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, logout'
    }).then((result) => {
        if (result.isConfirmed) {

            logout();
        }
    });
}

function logout() {
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
function GetCountry() {

    $.ajax({
        url: '/Authentication/GetCountrys',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#ddlCountry').append('<Option value=' + data.id + '>' + data.countryName + '</Option>')
            });
        }
    });
}

$(document).ready(function () {
    $('#drpCuCountry,#VendorCountry,#CompanyCountry,#projectCountry').select2({
        placeholder: 'Select Contry',
        width: '100%',
        dropdownAutoWidth: true,
        allowClear: true,
        ajax: {
            url: '/Authentication/GetCountrys',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                return {
                    results: data.map(item => ({
                        id: item.id,
                        text: item.countryName
                    }))
                };
            }
        }
    });

    $('#drpCuState,#VendorState,#CompanyState,#ProjectState').select2({
        placeholder: 'Select State',
        width: '100%',
        allowClear: true
    });

    $('#drpCuCity,#VendorCity,#CompanyCity,#ProjectCity').select2({
        placeholder: 'Select City',
        width: '100%',
        allowClear: true
    });

    $('#ddlVendorType').select2({
        placeholder: 'Select Vendor Type',
        width: '100%',
        dropdownAutoWidth: true,
        allowClear: true,
        ajax: {
            url: '/Vendor/GetVendorType',
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                return {
                    results: data.map(item => ({
                        id: item.id,
                        text: item.vendorType
                    }))
                };
            }
        }
    });
})
function fn_getUserState(drpUserstate, countryId, that) {
    var cid = countryId ?? $(that).val();

    let $stateDropdown = $('#' + drpUserstate);
    $stateDropdown.empty().append('<option value="">--Select State--</option>');

    $.ajax({
        url: '/Authentication/GetState?StateId=' + cid,
        success: function (result) {
            $.each(result, function (i, data) {
                $stateDropdown.append('<option value="' + data.id + '">' + data.stateName + '</option>');
            });

            $stateDropdown.trigger('change');
        }
    });
}

function fn_getUsercitiesbystateId(drpUsercity, stateid, that) {
    var sid = stateid ?? $(that).val();

    let $cityDropdown = $('#' + drpUsercity);
    $cityDropdown.empty().append('<option value="">--Select City--</option>');

    $.ajax({
        url: '/Authentication/GetCity?CityId=' + sid,
        success: function (result) {
            $.each(result, function (i, data) {
                $cityDropdown.append('<option value="' + data.id + '">' + data.cityName + '</option>');
            });

            $cityDropdown.trigger('change');
        }
    });
}
function fn_getVendorState(drpVendorstate, countryId, that, callback) {
    var cid = countryId ?? $(that).val();

    let $stateDropdown = $('#' + drpVendorstate);
    $stateDropdown.empty().append('<option value="">--Select State--</option>');

    $.ajax({
        url: '/Authentication/GetState?StateId=' + cid,
        success: function (result) {
            $.each(result, function (i, data) {
                $stateDropdown.append('<option value="' + data.id + '">' + data.stateName + '</option>');
            });

            if (typeof callback === 'function') callback();
        }
    });
}

function fn_getVendorcitiesbystateId(drpVendorcity, stateid, that, callback) {

    var sid = stateid ?? $(that).val();

    let $cityDropdown = $('#' + drpVendorcity);
    $cityDropdown.empty().append('<option value="">--Select City--</option>');

    $.ajax({
        url: '/Authentication/GetCity?CityId=' + sid,
        success: function (result) {
            $.each(result, function (i, data) {
                $cityDropdown.append('<option value="' + data.id + '">' + data.cityName + '</option>');
            });

            if (typeof callback === 'function') callback();
        }
    });
}
function fn_getCompanyState(drpCompanystate, countryId, that, callback) {
    var cid = countryId ?? $(that).val();

    let $stateDropdown = $('#' + drpCompanystate);
    $stateDropdown.empty().append('<option value="">--Select State--</option>');

    $.ajax({
        url: '/Authentication/GetState?StateId=' + cid,
        success: function (result) {
            $.each(result, function (i, data) {
                $stateDropdown.append('<option value="' + data.id + '">' + data.stateName + '</option>');
            });

            if (typeof callback === 'function') callback();
        }
    });
}

function fn_getCompanycitiesbystateId(drpCompanycity, stateid, that, callback) {
    var sid = stateid ?? $(that).val();

    let $cityDropdown = $('#' + drpCompanycity);
    $cityDropdown.empty().append('<option value="">--Select City--</option>');

    $.ajax({
        url: '/Authentication/GetCity?CityId=' + sid,
        success: function (result) {
            $.each(result, function (i, data) {
                $cityDropdown.append('<option value="' + data.id + '">' + data.cityName + '</option>');
            });

            if (typeof callback === 'function') callback();
        }
    });
}

function fn_getProjectState(drpProjectstate, countryId, that, callback) {
    var cid = countryId ?? $(that).val();

    let $stateDropdown = $('#' + drpProjectstate);
    $stateDropdown.empty().append('<option value="">--Select State--</option>');

    $.ajax({
        url: '/Authentication/GetState?StateId=' + cid,
        success: function (result) {
            $.each(result, function (i, data) {
                $stateDropdown.append('<option value="' + data.id + '">' + data.stateName + '</option>');
            });

            if (typeof callback === 'function') callback();
        }
    });
}

function fn_getProjectcitiesbystateId(drpProjectcity, stateid, that, callback) {

    var sid = stateid ?? $(that).val();

    let $cityDropdown = $('#' + drpProjectcity);
    $cityDropdown.empty().append('<option value="">--Select City--</option>');

    $.ajax({
        url: '/Authentication/GetCity?CityId=' + sid,
        success: function (result) {
            $.each(result, function (i, data) {
                $cityDropdown.append('<option value="' + data.id + '">' + data.cityName + '</option>');
            });

            if (typeof callback === 'function') callback();
        }
    });
}
function Citytext(sel) {
    $("#txtcity").val((sel.options[sel.selectedIndex].text));
}


function Departmenttext(sel) {

    $("#txtdeptid").val((sel.options[sel.selectedIndex].text));
}


function GetDepartment() {

    $.ajax({
        url: '/Authentication/GetDepartment',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#ddlDepartmenrnt').append('<Option value=' + data.id + '>' + data.departments + '</Option>')

            });

            $('#ddlDepartment').empty();
            $('#ddlDepartment').append('<option selected disabled value="">--Select Department--</option>');
            $.each(result, function (i, data) {
                $('#ddlDepartment').append('<option value=' + data.id + '>' + data.departments + '</option>');
            });
        }
    });
}



function fn_OpenAddproductmodal() {
    $('#mdProductSearch').val('');
    $('#mdPoproductModal').modal('show');
    fn_GetProductDetailsList(1);
}

function fn_GetProductDetailsList(page) {
    var searchText = $('#mdProductSearch').val();

    $.get("/PurchaseRequest/GetAllProductDetailsList", { searchText: searchText, page: page })
        .done(function (result) {
            $("#mdlistofItem").html(result);
        })
        .fail(function (xhr, status, error) {
            console.error("Error:", error);
        });
}

$(document).on("click", ".pagination a", function (e) {
    e.preventDefault();
    var page = $(this).text();
    fn_GetProductDetailsList(page);
});

$(document).on("click", "#backButton", function (e) {
    e.preventDefault();
    var page = $(this).text();
    fn_GetProductDetailsList(page);
});


function fn_filterallProducts() {
    var searchText = $('#mdProductSearch').val();

    $.ajax({
        url: '/PurchaseRequest/GetAllProductDetailsList',
        type: 'GET',
        data: {
            searchText: searchText,
        },
        success: function (result) {
            $("#mdlistofItem").html(result);
        },
    });
}
function GetVendorTypes() {

    $.ajax({
        url: '/Vendor/GetVendorType',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#textVendorType').append('<Option value=' + data.id + '>' + data.vendorType + '</Option>')
            });
        }
    });
}

function GetPaymentMethodList() {

    $.ajax({
        url: '/PurchaseOrderMaster/GetPaymentMethodList',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#drpcreditdebitpaymentmethod').append('<Option value=' + data.id + '>' + data.paymentMethod + '</Option>')
            });
            var firstPaymentMethod = result[0];
            $('#drpcreditdebitpaymentmethod').val(firstPaymentMethod.id);
        }
    });
}
function GetPaymentTypeList() {
    $.ajax({
        url: '/ExpenseMaster/GetPaymentTypeList',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#drpcreditdebitpaymenttype').append('<Option value=' + data.id + '>' + data.type + '</Option>')
            });
            var firstPaymentType = result[0];
            $('#drpcreditdebitpaymenttype').val(firstPaymentType.id);
            $.each(result, function (i, data) {
               // $('#txtExpensepaymenttype').append('<Option value=' + data.id + '>' + data.type + '</Option>')
                $('#EditExpensepaymenttype').append('<Option value=' + data.id + '>' + data.type + '</Option>')
            });
        }
    });
}

function GetCompanyNameList() {
    $.ajax({
        url: '/ProductMaster/GetVendorsNameList',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#textTransactionCompanyName').append('<option value="' + data.id + '">' + data.vendorCompany + '</option>');
            });
        },
    });
}

function GetVendorNameList() {
    $.ajax({
        url: '/ProductMaster/GetVendorsNameList',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#txtvendorname').append('<Option value=' + data.id + '>' + data.vendorCompany + '</Option>')
            });
            $.each(result, function (i, data) {
                $('#txtvendornamed').append('<Option value=' + data.id + '>' + data.vendorCompany + '</Option>')
            });

        }
    });
}

function fn_GetAllCities() {
    $.ajax({
        url: '/Authentication/GetAllCities',
        method: 'GET',
        success: function (result) {
            var allCities = result.map(function (data) {
                return {
                    label: data.cityName,
                    value: data.id
                };
            });

            $("#AllCityDRP").autocomplete({
                source: allCities,
                minLength: 0,
                select: function (event, ui) {
                    event.preventDefault();
                    $("#AllCityDRP").val(ui.item.label);
                    $("#AllCityDRPHidden").val(ui.item.value);
                    fn_ChangeCityDrp(ui.item.label);
                }
            }).focus(function () {
                $(this).autocomplete("search");
            });

            var selectedCity = sessionStorage.getItem('SelectedCityName');
            if (selectedCity) {
                var defaultCity = allCities.find(city => city.label == selectedCity);
                if (defaultCity) {
                    $("#AllCityDRP").val(defaultCity.label);
                    $("#AllCityDRPHidden").val(defaultCity.value);
                    fn_ChangeCityDrp(defaultCity.label);
                }
            } else {
                var defaultCity = allCities.find(city => city.label === "Vadodara");
                if (defaultCity) {
                    $("#AllCityDRP").val(defaultCity.label);
                    $("#AllCityDRPHidden").val(defaultCity.value);
                    fn_ChangeCityDrp(defaultCity.label);
                }
            }
        },
        error: function (err) {
            console.error("Failed to fetch Cities: ", err);
        }
    });
}

function fn_ChangeCityDrp(selectedCity) {
    sessionStorage.setItem('SelectedCityName', selectedCity);
    /*showWeatherAPI(selectedCity);*/
}

function clearProductListSearchText() {
    $('#mdProductSearch').val('');
    fn_GetProductDetailsList();
}