
//function Details() {
//    debugger
//    $.ajax({
//        url: '/UserDetails/UserProfile?UserName=',
//        type: 'Get',
//        contentType: 'application/json;charset=utf-8;',
//        dataType: 'json',
//        success: function (response) {
//            debugger
//            $('#UserName').text(response.userName);
//            $('#FirstName').text(response.firstName);
//            $('#LastName').text(response.lastName);
//            $('#PhoneNumber').text(response.phoneNumber);
//            $('#Email').text(response.email);
//            $('#Address').text(response.address);
//        },
//        error: function () {
//            alert('Data not Found');
//        }
//    })
//}
alert('Ok');
$(document).ready(function () {
    GetDocument();
});

function GetDocument() {debugger

    $.ajax({
        url: '/UserDetails/GetDocumentList',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#Documents').append('<Option value=' + data.id + '>' + data.documentType + '</Option>')
            });
        }
    });
}