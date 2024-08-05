
GetChateMembes();
function GetChateMembes() {
    $.ajax({
        url: '/Home/GetChateMembes',
        type: 'Get',
        datatype: 'json',
        complete: function (Result) {
            $("#chatmebmer").html(Result.responseText);
        }
    });
}

function GetConversation(conversationId) {
    $.ajax({
        url: '/Home/GetChatConversation',
        type: 'GET',
        data: { Id: conversationId },
        datatype: 'html',
        success: function (result) {
            $("#userconversation").html(result);
        },

    });
}

function fn_SendMessage() {
    event.preventDefault();
    var ConversationId = $('#txtChatConversationId').val();
    var ChatData = {
        UserId: $('#txtChatUserId').val(),
        UserName: $('#txtChatUserName').val(),
        MessageText: $('#txtChatMessage').val(),
        ConversationId: $('#txtChatConversationId').val(),
    }

    $.ajax({
        url: '/Home/SendMessage',
        type: 'POST',
        data: ChatData,
        dataType: 'json',
        success: function (result) {
            if (result.code == 200) {
                GetConversation(ConversationId);
            } else {
                toastr.warning(result.message);
            }
        },
    });
}


//function fn_ReadMessage(conversationId, secondUserId, secondMessageId) {
//    debugger


//    var ChatData = {
//        UserId: secondUserId,
//        MessageId: secondMessageId,
//        ConversationId: conversationId,
//    }

//    $.ajax({
//        url: '/Home/SendMessage',
//        type: 'POST',
//        data: ChatData,
//        dataType: 'json',
//        success: function (result) {
//            if (result.code == 200) {
//                GetConversation(ConversationId);
//            } else {
//                toastr.warning(result.message);
//            }
//        },
//    });
//}



