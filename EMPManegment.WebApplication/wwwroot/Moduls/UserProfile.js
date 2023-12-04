var UserId = '';

//function Details() {
//    debugger

//    $.ajax({
//        url: '/UserDetails/UserProfile?UserId=' + UserId,
//        type: 'Get',
//        contentType: 'application/json;charset=utf-8;',
//        dataType: 'json',
//        success: function (Result) {
//            debugger
            
//        },
//        error: function () {
//            alert('Data not Found');
//        }
//    })
//}

$(document).ready(function () {
    GetDocumentList();
    GetDocumentType();
    GetProjectList();
});

function GetDocumentType() {

    $.ajax({
        url: '/UserDetails/GetDocumentType',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#Documents').append('<Option value=' + data.id + '>' + data.documentType + '</Option>')
            });
        }
    });
}


function GetDocumentList() {
    $.ajax({
        url: '/UserDetails/DisplayDocumentList',
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
                object += '<td><a href="/UserDetails/DownloadDocument/?documentName='+item.documentName+'"><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="currentColor" d="M13 12h3l-4 4l-4-4h3V8h2v4Zm2-8H5v16h14V8h-4V4ZM3 2.992C3 2.444 3.447 2 3.999 2H16l5 5v13.993A1 1 0 0 1 20.007 22H3.993A1 1 0 0 1 3 21.008V2.992Z"/></svg></a></td>';
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
        url: '/UserDetails/UploadDocument',
        type: 'Post',
        data: fromData,
        dataType: 'json',
        processData: false,
        contentType: false,
        success: function () {
            GetDocumentList();
        }
    });
}

function GetUserProjectList() {

    $.ajax({
        url: '/Project/GetUserProjectList',
        type: 'Post',
        dataType: 'json',
        processData: false,
        contentType: false,
        complete: function (Result) {

            $('#dvprojectdetail').html(Result.responseText);
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

function GetProjectList() {

    $.ajax({
        url: '/Project/GetProjectList',
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

