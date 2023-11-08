using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblTaskDetail
{
    public Guid Id { get; set; }

    public int? TaskType { get; set; }

    public string? TaskTitle { get; set; }

    public string? TaskDetails { get; set; }

    public DateTime? TaskDate { get; set; }

    public DateTime? TaskEndDate { get; set; }

    public Guid? UserId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public string? TaskStatus { get; set; }

    public virtual TblTaskMaster? TaskTypeNavigation { get; set; }

    public virtual TblUser? User { get; set; }
}
