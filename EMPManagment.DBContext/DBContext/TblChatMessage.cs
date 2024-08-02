using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblChatMessage
{
    public int MessageId { get; set; }

    public Guid UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string MessageText { get; set; } = null!;

    public DateTime SentDateTime { get; set; }

    public bool? IsRead { get; set; }

    public bool? IsDeleted { get; set; }

    public Guid? ConversationId { get; set; }

    public virtual TblUser User { get; set; } = null!;
}
