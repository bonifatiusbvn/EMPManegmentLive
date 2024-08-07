using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels.Chat;
using EMPManegment.EntityModels.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Services.Home
{
    public interface IUserHomeServices
    {
        Task<UserResponceModel> SendMessageAsync(ChatMessagesView chatMessages);
        Task<List<TblChatMessage>> ReceiveMessagesAsync(Guid userId, Guid? conversationId);
        Task<UserResponceModel> MarkMessagesAsReadAsync(ChatMessagesView ChatMessage);
        Task<IEnumerable<ChatMessagesView>> GetMyConversation(Guid userId);
        Task<IEnumerable<ChatMessagesView>> GetMyConversationList(Guid userId);
    }
}
