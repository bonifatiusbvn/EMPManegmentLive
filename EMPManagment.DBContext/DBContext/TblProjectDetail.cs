using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblProjectDetail
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }

    public Guid? UserId { get; set; }

    public string? ProjectType { get; set; }

    public string? ProjectTitle { get; set; }

    public string? UserRole { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? TotalMember { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public virtual TblProjectMaster? Project { get; set; }

    public virtual TblUser? User { get; set; }
}
