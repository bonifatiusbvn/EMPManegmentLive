using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblProjectMaster
{
    public Guid ProjectId { get; set; }

    public string? ProjectType { get; set; }

    public string? ProjectTitle { get; set; }

    public string? ProjectHead { get; set; }

    public string? ProjectDescription { get; set; }

    public string? ProjectLocation { get; set; }

    public string? ProjectPriority { get; set; }

    public string? ProjectStatus { get; set; }

    public DateTime ProjectStartDate { get; set; }

    public DateTime ProjectEndDate { get; set; }

    public DateTime ProjectDeadline { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public Guid UserId { get; set; }

    public virtual TblUser User { get; set; } = null!;
}
