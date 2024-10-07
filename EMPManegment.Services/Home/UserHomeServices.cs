using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels.Chat;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.Home;
using EMPManegment.Inretface.Services.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.Home
{
    public class UserHomeServices : IUserHomeServices
    {
        public UserHomeServices(IUserHome userHome)
        {
            UserHome = userHome;
        }

        public IUserHome UserHome { get; }

        public async Task<IEnumerable<ChatMessagesView>> GetMyConversation(Guid userId)
        {
            return await UserHome.GetMyConversation(userId);
        }

        public async Task<IEnumerable<ChatMessagesView>> GetMyConversationList(Guid userId)
        {
            return await UserHome.GetMyConversationList(userId);
        }

        public Task<UserResponceModel> MarkMessagesAsReadAsync(ChatMessagesView ChatMessage)
        {
            return UserHome.MarkMessagesAsReadAsync(ChatMessage);
        }

        public async Task<List<TblChatMessage>> ReceiveMessagesAsync(Guid userId, Guid? conversationId)
        {
            return await UserHome.ReceiveMessagesAsync(userId, conversationId);
        }

        public Task<UserResponceModel> SendMessageAsync(ChatMessagesView chatMessages)
        {
            return UserHome.SendMessageAsync(chatMessages);
        }
        public Task<IEnumerable<ChatMessagesView>> CheckUserConversationId(NewChatMessageModel newChatMessage)
        {
            return UserHome.CheckUserConversationId(newChatMessage);
        }
        public Task<IEnumerable<ChatMessagesView>> GetUsersNewMessageList(Guid userId)
        {
            return UserHome.GetUsersNewMessageList(userId);
        }
        public Task<AllNotificationModel> GetUsersAllNotificationList(Guid userId)
        {
            return UserHome.GetUsersAllNotificationList(userId);
        }
        public Task<UserResponceModel> DeleteChatMessage(int MessageId)
        {
            return UserHome.DeleteChatMessage(MessageId);
        }
        public Task<UserResponceModel> RemoveAllNotifications(AllNotificationModel AllNotification)
        {
            return UserHome.RemoveAllNotifications(AllNotification);
        }
    }
}
