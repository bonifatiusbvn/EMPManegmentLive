var UserId = '';

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
                object += '<td>' + item.documentName.substring(37) + '</td>';
                object += '<td>' + (new Date(item.createdOn)).toLocaleDateString('en-US') + '</td>';
                object += '<td>' + item.createdBy + '</td>';
                object += '<td><a href="/UserProfile/DownloadDocument/?documentName=' + item.documentName + '"><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="currentColor" d="M13 12h3l-4 4l-4-4h3V8h2v4Zm2-8H5v16h14V8h-4V4ZM3 2.992C3 2.444 3.447 2 3.999 2H16l5 5v13.993A1 1 0 0 1 20.007 22H3.993A1 1 0 0 1 3 21.008V2.992Z"/></svg></a></td>';
                object += '</tr>';
            });
            $('#TableData').html(object);
        },
        error: function () {
            alert("data can't get");
        }
    });
};


function UploadDocument() {
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
        success: function () {
            Swal.fire({
                title: 'Document successfully uploaded.',
                icon: 'success',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK',
            }).then(function () {
                GetDocumentList();
            });
        }
    });
}

function GetUserProjectList(page) {

    var searchBy = $("#idinputSearch").val();
    var searchFor = $("#txtinputSearch").val();

    $.get("/Project/ShowUserProjectList", { searchby: searchBy, searchfor: searchFor, page: page })
        .done(function (result) {

            $("#dvshowprojectdetail").html(result);
        })
        .fail(function (error) {
            console.error(error);
        });
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
        Error: function () {
            Swal.fire({
                title: "Can't get data!",
                icon: 'warning',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK',
            })
        }
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
        .fail(function (error) {
            console.error(error);
        });
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
