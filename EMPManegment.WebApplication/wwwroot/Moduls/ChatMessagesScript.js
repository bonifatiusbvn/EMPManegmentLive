
GetChateMembes();
function GetChateMembes() {
    debugger
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
    debugger
    $.ajax({
        url: '/Home/GetChatConversation',
        type: 'GET',
        data: { Id: conversationId },
        datatype: 'html',
        success: function (result) {
            $("#userconversation").html(result);
        },
        error: function (xhr, status, error) {
            console.error("Error fetching conversation: ", error);
        }
    });
}


