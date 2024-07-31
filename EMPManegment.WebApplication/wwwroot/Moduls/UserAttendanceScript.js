
var datas = userPermissions
var selectedUserName = null;
var selectedDate = null;
var selectedStartDate = null;
var selectedEndDate = null;
var selectedMonth = null;
$(document).ready(function () {
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
            $("#searchUserAttendancebtn").show();
        }
        else if (selectedValue === "ByDate") {
            $("#datesbox").show();
            $("#usernamebox").hide();
            $("#startdatebox").hide();
            $("#enddatebox").hide();
            $("#searchUserAttendancebtn").show();
        }
        else if (selectedValue === "ByDateAndUser") {
            GetUsernameList();
            $("#usernamebox").show();
            $("#datesbox").show();
            $("#startdatebox").hide();
            $("#enddatebox").hide();
            $("#searchUserAttendancebtn").show();
        }
        else if (selectedValue === "ByDatesAndUser") {
            GetUsernameList();
            $("#usernamebox").show();
            $("#datesbox").hide();
            $("#startdatebox").show();
            $("#enddatebox").show();
            $("#searchUserAttendancebtn").show();
        }
        else if (selectedValue == "ByMonth") {
            $("#monthbox").show();
            $("#datebox").hide();
            $("#datebox1").hide();
            $("#myattendanceseachbtn").show();
        }
        else if (selectedValue == "BetweenDates") {
            $("#monthbox").hide();
            $("#datebox").show();
            $("#datebox1").show();
            $("#myattendanceseachbtn").show();
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
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            $.each(response, function (index, item) {
                $('#AttandanceId').val(item.attendanceId);
                $('#UserName').val(item.userName);
                $('#Date').val(getCommonDateformat(item.date));

                var intime = item.intime ? getCommonDatetime(item.date, item.intime) : '';
                $('#Intime').val(intime);

                var outTime = item.outTime ? getCommonDatetime(item.date, item.outTime) : '';
                $('#OutTime').val(outTime);
            });
        },
        error: function () {
            toastr.error("Can't get Data");
        }
    });
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
        selectedUserName = $("#drpAttusername").val();
        selectedDate = $('#txtdate').val();
        selectedStartDate = $("#txtstartdatebox").val();
        selectedEndDate = $("#txtenddatebox").val();
        var FilterData = {
            Date: selectedDate,
            UserId: selectedUserName,
            StartDate: selectedStartDate,
            EndDate: selectedEndDate
        };
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
        selectedMonth = $('#txtmonth').val();
        selectedStartDate = $("#txtstartdate").val();
        selectedEndDate = $("#txtenddate").val();
        var FilterData = {
            Cmonth: selectedMonth,
            StartDate: selectedStartDate,
            EndDate: selectedEndDate
        };
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
                return '<a onclick="editMyAttendance(\'' + full.attendanceId + '\')" class="btn text-primary">' +
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
                $('#srcDate').val(getCommonDateformat(item.date));
                function formatDateToLocal(date) {
                    var yyyy = date.getFullYear();
                    var mm = (date.getMonth() + 1).toString().padStart(2, '0');
                    var dd = date.getDate().toString().padStart(2, '0');
                    var hh = date.getHours().toString().padStart(2, '0');
                    var mi = date.getMinutes().toString().padStart(2, '0');
                    var ss = date.getMinutes().toString().padStart(2, '0');

                    return `${yyyy}-${mm}-${dd}T${hh}:${mi}:${ss}`;
                }
                function setDateAttributes(selector, date, time) {
                    var minDate = new Date(date);
                    var maxDate = new Date(minDate);
                    maxDate.setDate(minDate.getDate() + 7);

                    var formattedMinDate = formatDateToLocal(minDate);
                    var formattedMaxDate = formatDateToLocal(maxDate);

                    $(selector).attr('min', formattedMinDate);
                    $(selector).attr('max', formattedMaxDate);

                    if (time) {
                        var timeDate = new Date(time);
                        var formattedTime = formatDateToLocal(timeDate);
                        $(selector).val(formattedTime);
                    } else {
                        $(selector).val(formattedMinDate);
                    }
                }
                if (item.intime == null) {
                    setDateAttributes('#srcIntime', item.date, null);
                } else {
                    setDateAttributes('#srcIntime', item.date, item.intime);
                }

                if (item.outTime == null) {
                    setDateAttributes('#srcOutTime', item.date, null);
                } else {
                    setDateAttributes('#srcOutTime', item.date, item.outTime);
                }

            });
            $('#editTimeModelsearch').modal('show');
        },
        error: function () {
            toastr.error("Can't get Data");
        }
    })
}

function editMyAttendance(attandenceId) {
    $.ajax({
        url: '/UserProfile/EditOutTime?attendanceId=' + attandenceId,
        type: 'Get',
        dataType: 'json',
        processData: false,
        contentType: false,
        success: function (response) {
            $.each(response, function (index, item) {
                $('#txtmyAttandanceId').val(item.attendanceId);
                $('#txtmyUserName').val(item.userName);
                $('#txtmyDate').val(getCommonDateformat(item.date));
                function formatDateToLocal(date) {
                    var yyyy = date.getFullYear();
                    var mm = (date.getMonth() + 1).toString().padStart(2, '0');
                    var dd = date.getDate().toString().padStart(2, '0');
                    var hh = date.getHours().toString().padStart(2, '0');
                    var mi = date.getMinutes().toString().padStart(2, '0');
                    var ss = date.getMinutes().toString().padStart(2, '0');

                    return `${yyyy}-${mm}-${dd}T${hh}:${mi}:${ss}`;
                }
                function setDateAttributes(selector, date, time) {
                    var minDate = new Date(date);
                    var maxDate = new Date(minDate);
                    maxDate.setDate(minDate.getDate() + 7);

                    var formattedMinDate = formatDateToLocal(minDate);
                    var formattedMaxDate = formatDateToLocal(maxDate);

                    $(selector).attr('min', formattedMinDate);
                    $(selector).attr('max', formattedMaxDate);

                    if (time) {
                        var timeDate = new Date(time);
                        var formattedTime = formatDateToLocal(timeDate);
                        $(selector).val(formattedTime);
                    } else {
                        $(selector).val(formattedMinDate);
                    }
                }
                if (item.intime == null) {
                    setDateAttributes('#txtmyIntime', item.date, null);
                } else {
                    setDateAttributes('#txtmyIntime', item.date, item.intime);
                }

                if (item.outTime == null) {
                    setDateAttributes('#txtmyOutTime', item.date, null);
                } else {
                    setDateAttributes('#txtmyOutTime', item.date, item.outTime);
                }

            });
            $('#editMyAttendanceTime').modal('show');
        },
        error: function () {
            toastr.error("Can't get Data");
        }
    })
}

var UserPermissionData = userPermissions;
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
                        icon: "success",
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function ()
                    {
                        $('#editTimeModelsearch').modal('hide');

                        var FilterData = {
                            Date: selectedDate,
                            UserId: selectedUserName,
                            StartDate: selectedStartDate,
                            EndDate: selectedEndDate
                        };
                        GetUserSearchAttendanceList(FilterData, UserPermissionData);
                    });
                }
            },
        })
    }
}

function updateUserAttendance() {
    var date = $("#Date").val();
    var intime = $("#Intime").val();
    var outTime = $("#OutTime").val();

    var objData = {
        AttendanceId: $("#AttandanceId").val(),
        Intime: moment(intime).format('YYYY-MM-DD HH:mm:ss'),
        OutTime: outTime ? moment(outTime).format('YYYY-MM-DD HH:mm:ss') : null,
        UserName: $("#UserName").val(),
        Date: moment(date).format('YYYY-MM-DD'),
        UpdatedBy: $("#textUpdatedById").val(),
    };

    if (!objData.Intime) {
        $("#Intime").css('border-color', 'red');
        $("#Intime").focus();
    } else {
        $("#Intime").css('border-color', 'lightgray');
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
                } else {
                    toastr.warning(Result.message);
                }
            },
            error: function () {
                toastr.error("An error occurred while updating the attendance.");
            }
        });
    }
}

function updateMyAttendance() {
    var objData = {
        AttendanceId: $("#txtmyAttandanceId").val(),
        OutTime: $("#txtmyOutTime").val(),
        Intime: $("#txtmyIntime").val(),
        UserName: $("#txtmyUserName").val(),
        Date: $("#txtmyDate").val(),
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
                        icon: "success",
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        $('#editMyAttendanceTime').modal('hide');
                        var FilterData = {
                            Cmonth: selectedMonth,
                            StartDate: selectedStartDate,
                            EndDate: selectedEndDate
                        };
                        MySearchAttendanceList(UserPermissionData, FilterData);

                    });
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

