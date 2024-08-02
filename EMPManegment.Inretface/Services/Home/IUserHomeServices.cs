using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Services.Home
{
    public interface IUserHomeServices
    {
        Task SendMessageAsync(ChatMessagesView chatMessages);
        Task<List<TblChatMessage>> ReceiveMessagesAsync(Guid userId, Guid? conversationId);
        Task MarkMessagesAsReadAsync(Guid userId, Guid? conversationId);

        Task<IEnumerable<ChatMessagesView>> GetMyConversation(Guid userId);
        Task<IEnumerable<ChatMessagesView>> GetMyConversationList(Guid userId);
    }
}
