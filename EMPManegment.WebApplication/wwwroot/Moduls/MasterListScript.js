
$(document).ready(function () {

    GetDepartment();
    GetCountry();
    GetQuestion();
    GetVendorTypes();
    GetPaymentMethodList();
    GetPaymentTypeList();
    GetCompanyNameList();
    GetUsernameList();
    GetVendorNameList();
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


function GetCountry() {

    $.ajax({
        url: '/Authentication/GetCountrys',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#ddlCountry').append('<Option value=' + data.id + '>' + data.countryName + '</Option>')
            });
            $.each(result, function (i, data) {
                    $('#VendorCountry').append('<option value="' + data.id + '">' + data.countryName + '</option>');
            });
            $.each(result, function (i, data) {
                $('#projectCountry').append('<option value=' + data.id + '>' + data.countryName + '</option>')
            });

            var $countryDropdown = $("#drpCuCountry");
            $countryDropdown.empty();
            $countryDropdown.append('<option selected value="">--Select Country--</option>');
            $.each(result, function (i, data) {
                $countryDropdown.append('<option value="' + data.id + '">' + data.countryName + '</option>');
            });
        }
    });
}
function fn_getVendorState(drpVendorstate, countryId, that) {
    var cid = countryId;
    if (cid == undefined || cid == null) {
        var cid = $(that).val();
    }


    $('#' + drpVendorstate).empty();
    $('#' + drpVendorstate).append('<Option >--Select State--</Option>');
    $.ajax({
        url: '/Authentication/GetState?StateId=' + cid,
        success: function (result) {

            $.each(result, function (i, data) {
                $('#' + drpVendorstate).append('<Option value=' + data.id + '>' + data.stateName + '</Option>')
            });
        }
    });
}

function fn_getVendorcitiesbystateId(drpVendorcity, stateid, that) {

    var sid = stateid;
    if (sid == undefined || sid == null) {
        var sid = $(that).val();
    }


    $('#' + drpVendorcity).empty();
    $('#' + drpVendorcity).append('<Option >--Select City--</Option>');
    $.ajax({
        url: '/Authentication/GetCity?CityId=' + sid,
        success: function (result) {

            $.each(result, function (i, data) {
                $('#' + drpVendorcity).append('<Option value=' + data.id + '>' + data.cityName + '</Option>');

            });
        }
    });
}
function fn_getProjectState(drpProjectstate, countryId, that) {
    var cid = countryId;
    if (cid == undefined || cid == null) {
        var cid = $(that).val();
    }


    $('#' + drpProjectstate).empty();
    $('#' + drpProjectstate).append('<Option >--Select State--</Option>');
    $.ajax({
        url: '/Authentication/GetState?StateId=' + cid,
        success: function (result) {

            $.each(result, function (i, data) {
                $('#' + drpProjectstate).append('<Option value=' + data.id + '>' + data.stateName + '</Option>')
            });
        }
    });
}

function fn_getProjectcitiesbystateId(drpProjectcity, stateid, that) {

    var sid = stateid;
    if (sid == undefined || sid == null) {
        var sid = $(that).val();
    }


    $('#' + drpProjectcity).empty();
    $('#' + drpProjectcity).append('<Option >--Select City--</Option>');
    $.ajax({
        url: '/Authentication/GetCity?CityId=' + sid,
        success: function (result) {

            $.each(result, function (i, data) {
                $('#' + drpProjectcity).append('<Option value=' + data.id + '>' + data.cityName + '</Option>');

            });
        }
    });
}
function fn_getUserState(drpUserstate, countryId, that) {
    var cid = countryId;
    if (cid == undefined || cid == null) {
        var cid = $(that).val();
    }

    $('#' + drpUserstate).empty();
    $('#' + drpUserstate).append('<Option >--Select State--</Option>');
    $.ajax({
        url: '/Authentication/GetState?StateId=' + cid,
        success: function (result) {

            $.each(result, function (i, data) {
                $('#' + drpUserstate).append('<Option value=' + data.id + '>' + data.stateName + '</Option>')
            });
        }
    });
}

function fn_getUsercitiesbystateId(drpUsercity, stateid, that) {

    var sid = stateid;
    if (sid == undefined || sid == null) {
        var sid = $(that).val();
    }

    $('#' + drpUsercity).empty();
    $('#' + drpUsercity).append('<Option >--Select City--</Option>');
    $.ajax({
        url: '/Authentication/GetCity?CityId=' + sid,
        success: function (result) {

            $.each(result, function (i, data) {
                $('#' + drpUsercity).append('<Option value=' + data.id + '>' + data.cityName + '</Option>');

            });
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

            var $departmentDropdown = $("#drpCuDepartment");
            $departmentDropdown.empty();
            $departmentDropdown.append('<option selected value="">--Select Department--</option>');
            $.each(result, function (i, data) {
                $departmentDropdown.append('<Option value=' + data.id + '>' + data.departments + '</Option>')
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
            $.each(result, function (i, data) {
                $('#ddlVendorType').append('<Option value=' + data.id + '>' + data.vendorType + '</Option>')
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
                $('#txtExpensepaymenttype').append('<Option value=' + data.id + '>' + data.type + '</Option>')
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

function GetUsernameList() {
    $.ajax({
        url: '/Task/GetUserName',
        success: function (result) {
            var dropdown = $('#usercustomDropdown');
            dropdown.empty();
            $.each(result, function (i, data) {
                dropdown.append('<option class="User-dropdown-item-custom" data-value=' + data.id + '>' + data.firstName + ' ' + data.lastName + '</option>');
            });
            $.each(result, function (i, data) {
                $('#drpAttusername').append('<option value=' + data.id + '>' + data.firstName + ' ' + data.lastName + ' (' + data.userName + ')</option>');
            });
            $('#ddlusername').empty();
            $('#ddlusername').append('<option selected disabled value="">--Select Username--</option>');
            $.each(result, function (i, data) {
                $('#ddlusername').append('<Option value=' + data.id + '>' + data.firstName + " " + data.lastName + " " + "(" + data.userName + ")" + '</Option>')
            });
        }
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