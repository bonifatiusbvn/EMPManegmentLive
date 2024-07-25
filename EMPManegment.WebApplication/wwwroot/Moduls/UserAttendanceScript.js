
var datas = userPermissions

$(document).ready(function () {

    $("#usernamebox").show();
    $("#monthbox").show();
    function clearTextBox() {
        $('#drpAttusername').find('option').not(':first').remove();
        $('#ddlmyattendanceser').find('option').not(':first').remove();
        $('#txtdate').val('');
        $('#txtstartdatebox').val('');
        $('#txtenddatebox').val('');
        $('#txtstartdate').val('');
        $('#txtenddate').val('');
        $('#txtmonth').val('');
    }

    $('.dropdown-item').click(function (e) {
        e.preventDefault();

        var selectedValue = $(this).data('value');
        $('#ddlatendanceser').data('value', selectedValue).text($(this).text());
        $('#ddlmyattendanceser').data('value', selectedValue).text($(this).text());

        clearTextBox();
        $("#usernamebox").show();

        if (selectedValue === "ByUsername") {
            GetUsernameList();
            $("#usernamebox").show();
            $("#datesbox").hide();
            $("#startdatebox").hide();
            $("#enddatebox").hide();
        }
        else if (selectedValue === "ByDate") {
            $("#datesbox").show();
            $("#usernamebox").hide();
            $("#startdatebox").hide();
            $("#enddatebox").hide();
        }
        else if (selectedValue === "ByDateAndUser") {
            GetUsernameList();
            $("#usernamebox").show();
            $("#datesbox").show();
            $("#startdatebox").hide();
            $("#enddatebox").hide();
        }
        else if (selectedValue === "ByDatesAndUser") {
            GetUsernameList();
            $("#usernamebox").show();
            $("#datesbox").hide();
            $("#startdatebox").show();
            $("#enddatebox").show();
        }
        else if (selectedValue == "ByMonth") {
            $("#monthbox").show();
            $("#datebox").hide();
            $("#datebox1").hide();
        }
        else if (selectedValue == "BetweenDates") {
            $("#monthbox").hide();
            $("#datebox").show();
            $("#datebox1").show();
        }
    });

});



$(document).ready(function () {
    function data(datas) {
        var userPermission = datas;
        GetUserAttendance(userPermission);
    }

    function GetUserAttendance(userPermission) {
        var userPermissionArray = JSON.parse(userPermission);
        var canEdit = userPermissionArray.some(permission => permission.formName === "Users Attendance" && permission.edit);
        var columns = [
            { "data": "userName", "name": "UserName" },
            {
                "data": "date", "name": "Date",
                "render": function (data, type, full, meta) {
                    return getCommonDateformat(data);
                }
            },
            {
                "data": "intime", "name": "InTime",
                render: function (data) {
                    return new Date(data).toLocaleTimeString('en-US');
                }
            },
            {
                "data": "outTime", "name": "OutTime",
                render: function (data, type, full) {
                    var userDate = new Date(full.date).toLocaleDateString('en-US');
                    var todayDate = new Date().toLocaleDateString('en-US');
                    if (data != null) {
                        return new Date(data).toLocaleTimeString('en-US');
                    }
                    else if (data == null && userDate == todayDate) {
                        return "Pending...";
                    }
                    else {
                        return "Missing";
                    }
                }
            },
            {
                "data": "totalHours", "name": "TotalHours",
                render: function (data, type, full) {
                    var userDate = new Date(full.date).toLocaleDateString('en-US');
                    var todayDate = new Date().toLocaleDateString('en-US');
                    if (full.totalHours != null) {
                        return full.totalHours.substr(0, 8) + ' hr';
                    } else if (full.totalHours == null && userDate === todayDate) {
                        return "Pending...";
                    } else {
                        return "Missing";
                    }
                }
            },
        ];

        if (canEdit) {
            columns.push({
                "data": null,
                "orderable": false,
                "searchable": false,
                "render": function (data, type, full) {
                    return '<a onclick="EditUserAttendance(\'' + full.attendanceId + '\')" class="btn text-primary">' +
                        '<i class="fa-regular fa-pen-to-square"></i></a>';
                }
            });
        }

        $('#attendanceTableData').DataTable({
            processing: false,
            serverSide: true,
            filter: true,
            destroy: true,
            pageLength: 30,
            lengthMenu: [[10, 25, 30, 50, -1], [10, 25, 30, 50, "All"]],
            ajax: {
                type: "POST",
                url: '/UserProfile/GetUserAttendanceList',
                dataType: 'json'
            },
            columns: columns,
            scrollY: 400,
            scrollX: true,
            scrollCollapse: true,
            fixedHeader: {
                header: true,
                footer: true
            },
            autoWidth: false,
            columnDefs: [
                {
                    targets: '_all', width: 'auto'
                }
            ],
            order: [[1, 'asc']]
        });
    }


    data(datas);
});

function formatDateToLocal(date) {
    var year = date.getFullYear();
    var month = (date.getMonth() + 1).toString().padStart(2, '0');
    var day = date.getDate().toString().padStart(2, '0');
    var hours = date.getHours().toString().padStart(2, '0');
    var minutes = date.getMinutes().toString().padStart(2, '0');
    return `${year}-${month}-${day}T${hours}:${minutes}`;
}

function EditUserAttendance(attandenceId) {
    $('#EditTimeModel').modal('show');
    $.ajax({
        url: '/UserProfile/EditOutTime?attendanceId=' + attandenceId,
        type: 'Get',
        dataType: 'json',
        processData: false,
        contentType: false,
        success: function (response) {
            $.each(response, function (index, item) {
                $('#AttandanceId').val(item.attendanceId);
                $('#UserName').val(item.userName);
                $('#Date').val((new Date(item.date)).toLocaleDateString('en-GB'));
                $('#Intime').val((new Date(item.intime)).toLocaleTimeString('en-GB'));
                if (item.outTime == null) {

                    var MinDate = new Date(item.date);
                    var MaxDate = new Date(MinDate);
                    MaxDate.setDate(MinDate.getDate() + 7);

                    var formattedMinDate = formatDateToLocal(MinDate);
                    var formattedMaxDate = formatDateToLocal(MaxDate);

                    $('#OutTime').attr('min', formattedMinDate);
                    $('#OutTime').attr('max', formattedMaxDate);
                    $('#OutTime').val(formattedMinDate);
                }
                else {
                    var MinDate = new Date(item.date);
                    var MaxDate = new Date(MinDate);
                    MaxDate.setDate(MinDate.getDate() + 7);

                    var formattedMinDate = formatDateToLocal(MinDate);
                    var formattedMaxDate = formatDateToLocal(MaxDate);

                    $('#OutTime').attr('min', formattedMinDate);
                    $('#OutTime').attr('max', formattedMaxDate);
                    $('#OutTime').val(item.outTime);
                }
            });
        },
        error: function () {
            toastr.error("Can't get Data");
        }
    })
}

function UpdateUserAttendance() {
    var objData = {
        AttendanceId: $("#AttandanceId").val(),
        OutTime: $("#OutTime").val(),
        Intime: $("#Intime").val(),
        UserName: $("#UserName").val(),
        Date: $("#Date").val(),
    }


    if (objData.OutTime == "") {
        $("#OutTime").css('border-color', 'red');
        $("#OutTime").focus();
    }

    else {
        $("#OutTime").css('border-color', 'lightgray');
        $.ajax({
            url: '/UserProfile/UpdateOutTime',
            type: 'Post',
            data: objData,
            dataType: 'json',
            success: function (Result) {

                var ricon = "warning";

                if (Result.icone == ricon) {

                    Swal.fire({
                        title: Result.message,
                        icon: Result.icone,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/UserProfile/UsersAttendance';
                    });
                }
            },
        })
    }
}

function formatDate(date) {
    var day = String(date.getDate()).padStart(2, '0');
    var month = String(date.getMonth() + 1).padStart(2, '0');
    var year = date.getFullYear();

    return day + '-' + month + '-' + year;
}

var datas = userPermissions;

function GetSearchAttendanceList() {
    var UserPermissionData = datas;
    $.ajax({
        url: '/UserProfile/GetSearchAttendanceList',
        type: 'GET',
        success: function (result) {
            
            $("#attendancedt").hide();
            $("#dvattendancelist").html(result);
            fn_SearchUserAttendanceList(UserPermissionData);
        },
        error: function () {
            alert('Error loading attendance list. Please try again.');
        }
    });
}

function fn_SearchUserAttendanceList(UserPermissionData) {
    var selectedValue = $('#ddlatendanceser').data('value');
    var isValid = true;
    var errorMessage = "Kindly fill all required fields";

    if (typeof selectedValue === "undefined") {
        isValid = false;
        errorMessage = "Please select a search criteria";
    } else if (selectedValue === "ByUsername" && $("#drpAttusername").val() === "") {
        isValid = false;
        errorMessage = "Please select a Username";
    } else if (selectedValue === "ByDate" && $("#txtdate").val() === "") {
        isValid = false;
        errorMessage = "Please select a Date";
    } else if (selectedValue === "ByDateAndUser" && ($("#drpAttusername").val() === "" || $("#txtdate").val() === "")) {
        isValid = false;
        errorMessage = "Please select both Username and Date";
    } else if (selectedValue === "ByDatesAndUser" && ($("#drpAttusername").val() === "" || $("#txtstartdatebox").val() === "" || $("#txtenddatebox").val() === "")) {
        isValid = false;
        errorMessage = "Please select Username, Start Date, and End Date";
    }

    if (isValid) {
        var FilterData = {
            Date: $('#txtdate').val(),
            UserId: $("#drpAttusername").val(),
            StartDate: $("#txtstartdatebox").val(),
            EndDate: $("#txtenddatebox").val()
        }
        GetUserSearchAttendanceList(FilterData, UserPermissionData);
    } else {
        $("#backbtn").hide();
        toastr.warning(errorMessage);
    }
}

function GetUserSearchAttendanceList(FilterData, UserPermissionData) {
    var userPermissionArray = JSON.parse(UserPermissionData);
    var canEdit = userPermissionArray.some(permission => permission.formName === "Users Attendance" && permission.edit);
    var columns = [
        { "data": "userName", "name": "UserName" },
        {
            "data": "date", "name": "Date",
            "render": function (data) {
                return getCommonDateformat(data);
            }
        },
        {
            "data": "intime", "name": "InTime",
            "render": function (data) {
                return new Date(data).toLocaleTimeString('en-US');
            }
        },
        {
            "data": "outTime", "name": "OutTime",
            "render": function (data, type, full) {
                var userDate = new Date(full.date).toLocaleDateString('en-US');
                var todayDate = new Date().toLocaleDateString('en-US');
                if (data) {
                    return new Date(data).toLocaleTimeString('en-US');
                } else if (userDate === todayDate) {
                    return "Pending...";
                } else {
                    return "Missing";
                }
            }
        },
        {
            "data": "totalHours", "name": "TotalHours",
            "render": function (data, type, full) {
                var userDate = new Date(full.date).toLocaleDateString('en-US');
                var todayDate = new Date().toLocaleDateString('en-US');
                if (full.totalHours) {
                    return full.totalHours.substr(0, 8) + ' hr';
                } else if (userDate === todayDate) {
                    return "Pending...";
                } else {
                    return "Missing";
                }
            }
        },
    ];

    if (canEdit) {
        columns.push({
            "data": null,
            "orderable": false,
            "searchable": false,
            "render": function (data, type, full) {
                return '<a onclick="editUserAttendanceSrc(\'' + full.attendanceId + '\')" class="btn text-primary">' +
                    '<i class="fa-regular fa-pen-to-square"></i></a>';
            }
        });
    }

    $('#FilterAttendanceTable').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        destroy: true,
        pageLength: 30,
        lengthMenu: [[10, 25, 30, 50, -1], [10, 25, 30, 50, "All"]],
        ajax: {
            type: "POST",
            url: '/UserProfile/GetUserSearchAttendanceList',
            dataType: 'json',
            data: FilterData,
        },
        columns: columns,
        scrollY: 400,
        scrollX: true,
        scrollCollapse: true,
        fixedHeader: {
            header: true,
            footer: true
        },
        autoWidth: false,
        columnDefs: [
            {
                targets: '_all', width: 'auto'
            }
        ],
        order: [[1, 'asc']]
    });
}
function GetMySearchAttendanceList() {
    var UserPermissionData = datas;
    $.ajax({
        url: '/UserProfile/GetSearchAttendanceList',
        type: 'GET',
        success: function (result) {

            $("#attendancedt").hide();
            $("#GetMyAttendanceList").html(result);
            fn_SearchMyAttendanceList(UserPermissionData);
        },
        error: function () {
            alert('Error loading attendance list. Please try again.');
        }
    });
}
function fn_SearchMyAttendanceList(UserPermissionData) {
    if ($('#txtmonth').val() == "" && $("#txtstartdate").val() == "" && $("#txtenddate").val() == "") {
        toastr.warning("Select the Month or UserName");
    } else {
        var FilterData = {
            Cmonth: $('#txtmonth').val(),
            StartDate: $("#txtstartdate").val(),
            EndDate: $("#txtenddate").val()
        }
        MySearchAttendanceList(UserPermissionData, FilterData);
    }
}

function MySearchAttendanceList(UserPermissionData, FilterData) {
    var userPermissionArray = JSON.parse(UserPermissionData);
    var canEdit = userPermissionArray.some(permission => permission.formName === "Users Attendance" && permission.edit);
    var columns = [
        { "data": "userName", "name": "UserName" },
        {
            "data": "date", "name": "Date",
            "render": function (data) {
                return getCommonDateformat(data);
            }
        },
        {
            "data": "intime", "name": "InTime",
            "render": function (data) {
                return new Date(data).toLocaleTimeString('en-US');
            }
        },
        {
            "data": "outTime", "name": "OutTime",
            "render": function (data, type, full) {
                var userDate = new Date(full.date).toLocaleDateString('en-US');
                var todayDate = new Date().toLocaleDateString('en-US');
                if (data) {
                    return new Date(data).toLocaleTimeString('en-US');
                } else if (userDate === todayDate) {
                    return "Pending...";
                } else {
                    return "Missing";
                }
            }
        },
        {
            "data": "totalHours", "name": "TotalHours",
            "render": function (data, type, full) {
                var userDate = new Date(full.date).toLocaleDateString('en-US');
                var todayDate = new Date().toLocaleDateString('en-US');
                if (full.totalHours) {
                    return full.totalHours.substr(0, 8) + ' hr';
                } else if (userDate === todayDate) {
                    return "Pending...";
                } else {
                    return "Missing";
                }
            }
        },
    ];

    if (canEdit) {
        columns.push({
            "data": null,
            "orderable": false,
            "searchable": false,
            "render": function (data, type, full) {
                return '<a onclick="editUserAttendanceSrc(\'' + full.attendanceId + '\')" class="btn text-primary">' +
                    '<i class="fa-regular fa-pen-to-square"></i></a>';
            }
        });
    }

    $('#FilterAttendanceTable').DataTable({
        processing: false,
        serverSide: true,
        filter: true,
        destroy: true,
        pageLength: 30,
        lengthMenu: [[10, 25, 30, 50, -1], [10, 25, 30, 50, "All"]],
        ajax: {
            type: "POST",
            url: '/UserProfile/GetAttendanceList',
            dataType: 'json',
            data: FilterData,
        },
        columns: columns,
        scrollY: 400,
        scrollX: true,
        scrollCollapse: true,
        fixedHeader: {
            header: true,
            footer: true
        },
        autoWidth: false,
        columnDefs: [
            {
                targets: '_all', width: 'auto'
            }
        ],
        order: [[1, 'asc']]
    });
}
$('#backbtn').on('click', function (event) {
    window.location = '/UserProfile/UsersAttendance';
});

function editUserAttendanceSrc(attandenceId) {
    $.ajax({
        url: '/UserProfile/EditOutTime?attendanceId=' + attandenceId,
        type: 'Get',
        dataType: 'json',
        processData: false,
        contentType: false,
        success: function (response) {
            $.each(response, function (index, item) {
                $('#srcAttandanceId').val(item.attendanceId);
                $('#srcUserName').val(item.userName);
                $('#srcDate').val((new Date(item.date)).toLocaleDateString('en-GB'));
                $('#srcIntime').val((new Date(item.intime)).toLocaleTimeString('en-GB'));
                if (item.outTime == null) {
                    $('#srcOutTime').val(item.date);
                }
                else {
                    $('#srcOutTime').val(item.outTime);
                }
            });
            $('#editTimeModelsearch').modal('show');
        },
        error: function () {
            toastr.error("Can't get Data");
        }
    })
}

function UpdateUserAttendanceSrc() {
    var objData = {
        AttendanceId: $("#srcAttandanceId").val(),
        OutTime: $("#srcOutTime").val(),
        Intime: $("#srcIntime").val(),
        UserName: $("#srcUserName").val(),
        Date: $("#srcDate").val(),
    }


    if (objData.OutTime == "") {
        $("#OutTime").css('border-color', 'red');
        $("#OutTime").focus();
    }

    else {
        $("#OutTime").css('border-color', 'lightgray');
        $.ajax({
            url: '/UserProfile/UpdateOutTime',
            type: 'Post',
            data: objData,
            dataType: 'json',
            success: function (Result) {

                var ricon = "warning";

                if (Result.icone == ricon) {

                    Swal.fire({
                        title: Result.message,
                        icon: Result.icone,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    })
                }

                else {

                    Swal.fire({
                        title: Result.message,
                        icon: Result.icone,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/UserProfile/UsersAttendance';
                    });
                }
            },
        })
    }
}
function updateUserAttendance() {
    var objData = {
        AttendanceId: $("#AttandanceId").val(),
        OutTime: $("#OutTime").val(),
        Intime: $("#Intime").val(),
        UserName: $("#UserName").val(),
        Date: $("#Date").val(),
        UpdatedBy: $("#textUpdatedById").val(),
    }


    if (objData.OutTime == null) {
        $("#OutTime").css('border-color', 'red');
        $("#OutTime").focus();
    }

    else {
        $("#OutTime").css('border-color', 'lightgray');
        $.ajax({
            url: '/UserProfile/UpdateOutTime',
            type: 'Post',
            data: objData,
            dataType: 'json',
            success: function (Result) {
                if (Result.code == 200) {
                    Swal.fire({
                        title: Result.message,
                        icon: "success",
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        window.location = '/UserProfile/UsersAttendance';
                    });
                }
                else {
                    toastr.error(Result.message);
                }
            },

        })
    }
}

function ExportToExcel() {
    siteloadershow();
    $.ajax({
        url: '/UserProfile/ExportToExcel',
        type: 'GET',
        success: function (data, status, xhr) {
            siteloaderhide();
            var filename = "";
            var disposition = xhr.getResponseHeader('Content-Disposition');
            if (disposition && disposition.indexOf('attachment') !== -1) {
                var matches = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/.exec(disposition);
                if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, '');
            }

            var type = xhr.getResponseHeader('Content-Type');
            var blob = new Blob([data], { type: type });

            if (typeof window.navigator.msSaveBlob !== 'undefined') {
                window.navigator.msSaveBlob(blob, filename);
            } else {
                var URL = window.URL || window.webkitURL;
                var downloadUrl = URL.createObjectURL(blob);

                if (filename) {
                    var a = document.createElement("a");
                    if (typeof a.download === 'undefined') {
                        window.location = downloadUrl;
                    } else {
                        a.href = downloadUrl;
                        a.download = filename;
                        document.body.appendChild(a);
                        a.click();
                    }
                } else {
                    window.location = downloadUrl;
                }

                setTimeout(function () { URL.revokeObjectURL(downloadUrl); }, 100);
            }
        },
        error: function (xhr, status, error) {
            siteloaderhide();
            toastr.warning("No data for selected month or date");
        },
        xhrFields: {
            responseType: 'blob'
        }
    });
}

function ExportToPDF() {
    siteloadershow();
    $.ajax({
        url: '/UserProfile/ExportToPdf',
        type: 'POST',
        success: function (data, status, xhr) {
            siteloaderhide();
            var filename = "";
            var disposition = xhr.getResponseHeader('Content-Disposition');
            if (disposition && disposition.indexOf('attachment') !== -1) {
                var matches = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/.exec(disposition);
                if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, '');
            }

            var type = xhr.getResponseHeader('Content-Type');
            var blob = new Blob([data], { type: type });

            if (typeof window.navigator.msSaveBlob !== 'undefined') {
                window.navigator.msSaveBlob(blob, filename);
            } else {
                var URL = window.URL || window.webkitURL;
                var downloadUrl = URL.createObjectURL(blob);

                if (filename) {
                    var a = document.createElement("a");
                    if (typeof a.download === 'undefined') {
                        window.location = downloadUrl;
                    } else {
                        a.href = downloadUrl;
                        a.download = filename;
                        document.body.appendChild(a);
                        a.click();
                    }
                } else {
                    window.location = downloadUrl;
                }

                setTimeout(function () { URL.revokeObjectURL(downloadUrl); }, 100);
            }
        },
        error: function (xhr, status, error) {
            siteloaderhide();
            toastr.warning("No data for selected month or date");
        },
        xhrFields: {
            responseType: 'blob'
        }
    });
}

