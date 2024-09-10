﻿
GetChateMembes();
GetAllNotifications();
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
            $('#ChatConversationBtn').removeClass('d-none');
        },

    });
}

function fn_SendMessage(ChatData) {
    event.preventDefault();
    var ConversationId = $('#txtChatConversationId').val();

    $.ajax({
        url: '/Home/SendMessage',
        type: 'POST',
        data: ChatData,
        dataType: 'json',
        success: function (result) {
            if (result.code == 200) {
                GetConversation(ConversationId);
                $('#txtChatMessage').val('');
            } else {
                toastr.warning(result.message);
            }
        },
    });
}
function fn_GetChatUserInformation(UserId) {
    $.ajax({
        url: '/Home/GetChatUserInformation?UserId=' + UserId,
        type: 'GET',
        datatype: 'html',
        success: function (result) {
            $('#userProfileCanvasExample').removeClass('d-none');
            $("#contactinfo").html(result);
        },
    });
}

function fn_ReadMessage(conversationId, secondUserId) {

    var chatData = {
        UserId: secondUserId,
        ConversationId: conversationId
    };

    $.ajax({
        url: '/Home/ReadMessage',
        type: 'POST',
        data: chatData,
        dataType: 'json',
        success: function (result) {
            if (result.code == 200) {

            } else {
                toastr.warning(result.message);
            }
        },
        error: function (xhr, status, error) {
            toastr.error("An error occurred while marking messages as read.");
        }
    });
}



function DisplayUserListForChat() {
    $.ajax({
        url: '/Home/AllUserListForChat',
        type: 'Post',
        dataType: 'json',
        processData: false,
        contentType: false,
        complete: function (Result) {
            $('#showAllUserforChat').html(Result.responseText);
        },
    })
}

var activenavlink = null;
function SearchChatUserName() {
    var searchUserName = $("#searchUserNameforchat").val();
    var activeTabLink = $('.userlistnav-link .nav-link.active');
    activenavlink = activeTabLink.attr('href').substring(1);

    if (activenavlink == "chats") {
        $.ajax({
            url: '/Home/GetChateMembes?searchUserName=' + searchUserName,
            type: 'Get',
            datatype: 'json',
            complete: function (Result) {
                $("#chatmebmer").html(Result.responseText);
            }
        });

    }
    else if (activenavlink == "contacts") {

        $.ajax({
            url: '/Home/AllUserListForChat?searchUserName=' + searchUserName,
            type: 'POST',
            dataType: 'json',
            processData: false,
            contentType: false,
            complete: function (Result) {

                $('#showAllUserforChat').html(Result.responseText);
            }
        });
    }
}

function clearUsernameSearchInput() {
    $("#searchUserNameforchat").val('');
    if (activenavlink == "chats") {
        GetChateMembes()
    }
    else {
        DisplayUserListForChat();
    }
}

function UserTaskNotification() {
    $.ajax({
        url: '/Home/GetUserTaskNotification',
        type: 'Get',
        dataType: 'json',
        processData: false,
        contentType: false,

        complete: function (Result) {

            $('#userTaskNotificationId').html(Result.responseText);
        },
    })
}

document.addEventListener('DOMContentLoaded', function () {
    UserTaskNotification();
});

function searchChatMessages() {
    var searchTerm = document.getElementById('searchChatMessage').value.toLowerCase();
    var chatGroups = document.querySelectorAll('#users-chat .chat-date-separator, #users-chat .chat-list');

    var groupVisibility = new Map();
    chatGroups.forEach(function (element) {
        if (element.classList.contains('chat-date-separator')) {
            groupVisibility.set(element, false);
        } else if (element.classList.contains('chat-list')) {
            var messageText = element.querySelector('.ctext-content').textContent.toLowerCase();

            if (messageText.includes(searchTerm)) {
                element.style.display = '';
                var groupSeparator = element.previousElementSibling;
                while (groupSeparator && !groupSeparator.classList.contains('chat-date-separator')) {
                    groupSeparator = groupSeparator.previousElementSibling;
                }
                if (groupSeparator) {
                    groupVisibility.set(groupSeparator, true);
                }
            } else {
                element.style.display = 'none';
            }
        }
    });
    
    groupVisibility.forEach((isVisible, separator) => {
        separator.style.display = isVisible ? '' : 'none';
    });
}

function GetUserForChat(selectedUserId) {
    var userId = $("#textchatsessionUserId").val();
    $.ajax({
        url: '/Home/CheckUserConversationId',
        type: 'GET',
        data: { userId: userId, selectedUserId: selectedUserId },
        datatype: 'html',
        success: function (result) {
            $("#userconversation").html(result);
            $('#ChatConversationBtn').removeClass('d-none');
            var conversationId = $('#txtChatConversationId').val();
            GetConversation(conversationId);
        }
    });
}

function GetNewMessageNotifications() {
    $.ajax({
        url: '/Home/GetUserNewMessagesNotification',
        type: 'GET',
        datatype: 'html',
        success: function (result) {
            $("#userNewMessageNotificationId").html(result);
        }
    });
}

function GetAllNotifications() {
    $.ajax({
        url: '/Home/GetUserAllNotifications',
        type: 'GET', 
        dataType: 'html',  
        complete: function (Result) {
            $('#userAllNotificationId').html(Result.responseText);    
            var totalcount = $('#totalNotificationCount').val();
            $('#CountAllNotification').text(totalcount);
            $('#CountAllNewNotification').text(totalcount);
            var UnreadMessages = $('#TotalUnreadMessages').val();
            $("#CountUnreadMessage").text(UnreadMessages); 
            var totalTasks = $('#totalTotalTasks').val();
            $("#CountTotalTask").text(totalTasks);
        },
    });
}

function openchatfromNotification(SelectedUserId) {
    var selectedUserId = SelectedUserId;
    var userId = $("#textchatsessionUserId").val();

    $.ajax({
        url: '/Home/CheckUserConversationId',
        type: 'GET',
        data: { userId: userId, selectedUserId: selectedUserId },
        datatype: 'html',
        success: function (result) {
            var parsedHtml = $('<div>').html(result);
            var conversationId = parsedHtml.find('#txtChatConversationId').val();
            localStorage.setItem('conversationId', conversationId);
            window.location.href = '/Home/ChatMessages';
        }
    });
}


$(document).ready(function () {
    var conversationId = localStorage.getItem('conversationId');
    if (conversationId) {
        GetConversation(conversationId);
        localStorage.removeItem('conversationId'); 
    }
});