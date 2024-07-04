using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblTaskDetail
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public Guid? ProjectId { get; set; }

    public int? TaskType { get; set; }

    public string? TaskTitle { get; set; }

    public string? TaskDetails { get; set; }

    public DateTime? TaskDate { get; set; }

    public DateTime? TaskEndDate { get; set; }

    public string? TaskStatus { get; set; }

    public string? Document { get; set; }

    public string? IsCompleted { get; set; }

    public Guid? CompletedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual TblProjectMaster? Project { get; set; }

    public virtual TblUser? User { get; set; }
}
