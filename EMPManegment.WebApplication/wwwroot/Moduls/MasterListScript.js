
$(document).ready(function () {

    GetDepartment();
    GetCountry();
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

});


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

function clearProductListSearchText() {
    $('#mdProductSearch').val('');
    fn_GetProductDetailsList();
}