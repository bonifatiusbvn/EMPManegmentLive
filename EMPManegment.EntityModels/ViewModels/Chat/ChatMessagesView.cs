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

        public string UserName { get; set; } = null!;

        public string MessageText { get; set; } = null!;

        public DateTime SentDateTime { get; set; }

        public bool? IsRead { get; set; }

        public bool? IsDeleted { get; set; }

        public Guid? ConversationId { get; set; }
        public string UserImage { get; set; }
    }
}
