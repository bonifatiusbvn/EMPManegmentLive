using EMPManegment.EntityModels.ViewModels.TaskModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.Chat
{
    public class ChatMessagesView
    {
        public int MessageId { get; set; }

        public Guid UserId { get; set; }

        public string? UserName { get; set; }

        public string? MessageText { get; set; }

        public DateTime? SentDateTime { get; set; }

        public bool? IsRead { get; set; }

        public bool? IsDeleted { get; set; }

        public Guid? ConversationId { get; set; }
        public string? UserImage { get; set; }
        public string? UserIdentity { get; set; }
    }

    public class NewChatMessageModel
    {
        public Guid MyUserId { get; set; }
        public Guid SelectedUserId { get; set; }
        public string? MyUserIdentity { get; set; }
    }

    public class AllNotificationModel
    {
        public List<ChatMessagesView>? Messages { get; set; }
        public List<TaskDetailsView>? Tasks { get; set; }
    }
}
