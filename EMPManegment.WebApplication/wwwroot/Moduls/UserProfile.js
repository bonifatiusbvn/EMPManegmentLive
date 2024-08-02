﻿var UserId = '';
var userPermissions = '';
$(document).ready(function () {
    GetDocumentList();
    GetDocumentType();
    GetProjectList();
    loadPartialView();
});

function GetDocumentType() {

    $.ajax({
        url: '/UserProfile/GetDocumentType',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#Documents').append('<Option value=' + data.id + '>' + data.documentType + '</Option>')
            });
        }
    });
}


function GetDocumentList() {
    $.ajax({
        url: '/UserProfile/DisplayDocumentList',
        type: 'Get',
        dataType: 'json',
        contentType: 'application/json;charset=utf-8;',
        success: function (result) {
            var object = '';
            $.each(result, function (index, item) {
                object += '<tr>';
                object += '<td>' + item.documentType + '</td>';
                let documentName = item.documentName;
                let extractedDocumentName = documentName.substring(documentName.lastIndexOf('_') + 1);
                object += '<td>' + extractedDocumentName + '</td>';
                object += '<td>' + getCommonDateformat(item.createdOn) + '</td>';
                object += '<td>' + item.createdBy + '</td>';
                object += '<td><a onclick="DownloadDocument(\'' + item.documentName + '\')"><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="currentColor" d="M13 12h3l-4 4l-4-4h3V8h2v4Zm2-8H5v16h14V8h-4V4ZM3 2.992C3 2.444 3.447 2 3.999 2H16l5 5v13.993A1 1 0 0 1 20.007 22H3.993A1 1 0 0 1 3 21.008V2.992Z"/></svg></a></td>';
                object += '</tr>';
            });
            $('#TableData').html(object);
        },
        error: function () {
            toastr.error("Can't get Data");
        }
    });
};


function UploadDocument() {
    var document = $("#formFile")[0].files[0];
    var documentType = $("#Documents").val();
    if (document != undefined && documentType != "--Document Type--") {
        var fromData = new FormData();
        fromData.append("documentType", $("#Documents").val());
        fromData.append("documentTypeId", $("#Documents").val());
        fromData.append("DocumentName", $("#formFile")[0].files[0]);
        fromData.append("CreatedBy", $("#CreatedBy").val());
        fromData.append("Id", $("#Id").val());
        fromData.append("UserId", $("#UserID").val());

        $.ajax({
            url: '/UserProfile/UploadDocument',
            type: 'Post',
            data: fromData,
            dataType: 'json',
            processData: false,
            contentType: false,
            success: function (Result) {
                if (Result.code == 200) {
                    Swal.fire({
                        title: Result.message,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK',
                    }).then(function () {
                        window.location = '/UserProfile/UserProfile';
                    });
                } else {
                    toastr.error(Result.message);
                }
            }
        });
    }
    else {
        toastr.warning('Please select document and documentType!');
    }
}

function GetUserProjectList(page) {

    var searchBy = $("#idinputSearch").val();
    var searchFor = $("#txtinputSearch").val();

    $.get("/Project/ShowUserProjectList", { searchby: searchBy, searchfor: searchFor, page: page })
        .done(function (result) {

            $("#dvshowprojectdetail").html(result);
        })

}

GetUserProjectList(1);
$(document).on("click", ".pagination a", function (e) {

    e.preventDefault();
    var page = $(this).text();
    GetUserProjectList(page);
});


$(document).on("click", "#btnbackButton", function (e) {

    e.preventDefault();
    var page = $(this).text();
    GetUserProjectList(page);
});

function btnserrchproject() {


    GetUserProjectList(1);
}

function GetProjectList() {
    $.ajax({
        url: '/Project/GetUserProjectList',
        type: 'Post',
        dataType: 'json',
        processData: false,
        contentType: false,
        complete: function (Result) {
            $('#dvuserprojectlist').html(Result.responseText);
        },
    })
}



$(document).ready(function () {
    $("#open").click(function () {
        $("#open").hide();
        $("#closed").show();
        $("#password").attr("type", "text");
    });
    $("#closed").click(function () {
        $("#closed").hide();
        $("#open").show();
        $("#password").attr("type", "password");
    });
});

function loadPartialView(page) {

    var searchBy = $("#inputSearch").val();
    var searchFor = $("#inputsearch").val();

    $.get("/Home/GetHomeProjectListPartial", { searchby: searchBy, searchfor: searchFor, page: page })
        .done(function (result) {

            $("#projectListContainer").html(result);
        })

}

loadPartialView(1);


$(document).on("click", ".pagination a", function (e) {
    e.preventDefault();
    var page = $(this).text();
    loadPartialView(page);
});


$(document).on("click", "#backButton", function (e) {
    e.preventDefault();
    var page = $(this).text();
    loadPartialView(page);
});

function serrchproject() {

    loadPartialView(1);
    GetUserProjectList(1);
}
function UserProjectActivity() {

    $.ajax({
        url: '/UserProfile/GetInvoiceActivityByUserId',
        type: 'Get',
        dataType: 'json',
        processData: false,
        contentType: false,
        complete: function (Result) {
            $('#UserProjectActivity').html(Result.responseText);
            UserTaskActivity();
        },
    })
}
function UserTaskActivity() {

    $.ajax({
        url: '/Task/ProjectActivityByUserId',
        type: 'Get',
        dataType: 'json',
        processData: false,
        contentType: false,
        complete: function (Result) {
            $('#UserTaskActivity').html(Result.responseText);
        },
    })
}
function UserProfilePhoto() {
    var formData = new FormData();
    formData.append("UserId", $("#UserID").val());
    var fileInput = document.getElementById("profile-img-file-input");
    if (fileInput.files.length > 0) {
        formData.append("Image", fileInput.files[0]);
    }
    $.ajax({
        url: '/UserProfile/UserProfilePhoto',
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
                    window.location = '/UserProfile/UserProfile';
                });
            } else {
                toastr.error(Result.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            toastr.error('An error occurred: ' + textStatus);
        }
    });
}
function showHidebtn() {
    var fileInput = document.getElementById('profile-img-file-input');
    if (fileInput.files && fileInput.files.length > 0) {
        document.getElementById('btnSave').style.display = 'block';
    } else {
        document.getElementById('btnSave').style.display = 'none';
    }
}

function DownloadDocument(documentName) {
    $.ajax({
        url: '/UserProfile/DownloadDocument?DocumentName=' + documentName,
        type: "GET",
        dataType: 'json',
        success: function (result) {
            siteloaderhide();

            if (result && result.memory) {
                try {
                    var binaryString = window.atob(result.memory);
                    var length = binaryString.length;
                    var bytes = new Uint8Array(length);

                    for (var i = 0; i < length; i++) {
                        bytes[i] = binaryString.charCodeAt(i);
                    }

                    var blob = new Blob([bytes], { type: result.contentType });

                    var link = document.createElement('a');
                    link.href = window.URL.createObjectURL(blob);
                    link.setAttribute('download', result.fileName);

                    document.body.appendChild(link);
                    link.click();

                    document.body.removeChild(link);
                } catch (e) {
                    toastr.error("Error decoding file: " + e.message);
                }
            } else {
                toastr.warning(result.Message || "No document found for selected");
            }
        },
        error: function () {
            siteloaderhide();
            toastr.error("Can't get data");
        }
    });
}
