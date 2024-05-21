
$(document).ready(function () {

    GetDepartment();
    GetCountry();
    GetQuestion();
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


function Questiontext(sel) {
    $("#txtquestionid").val((sel.options[sel.selectedIndex].text));
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

function GetQuestion() {

    $.ajax({
        url: '/Authentication/GetQuestion',
        success: function (result) {

            $.each(result, function (i, data) {
                $('#ddlQuestion').append('<Option value=' + data.id + '>' + data.questions + '</Option>')

            });
        }
    });
}