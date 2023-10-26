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
                object += '<td>' + item.documentName + '</td>';
                object += '<td>' + (new Date(item.createdOn)).toLocaleDateString('en-US') + '</td>';
                object += '<td>' + item.createdBy + '</td>';
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

//$(function () {
//    $("#SubmitButton").click(function () {
//        var documents = $("#Documents");
//        if (documents.val() == "") {
//            //If the "Please Select" option is selected display error.
//            alert("Please select document!");
//            return false;
//        }
//        return true;
//    });
//});

//$("#formFile").validate({
//    onfocusout: function (e) {
//        this.element(e);
//    },
//    rules: {
//        resume: {
//            required: true,
//            extension: "pdf"
//        }
//    },
//    resume: {
//        required: "input type is required",
//        extension: "select valied input file format"
//    }
//});


//$(document).ready(function () {

//    $("#formFile").validate({
//        rules: {
//            DocumentName: {
//                required: true,
//                extension: "pdf"

//            }
//        },
//        messages: {
//            DocumentName: {
//                extension: "Please select only .pdf files"
//            }
//        }
//    });
//});