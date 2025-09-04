using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblLeaveMaster
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public int Reason { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public double Days { get; set; }

    public string? Description { get; set; }

    public string Approvers { get; set; } = null!;

    public Guid? ApproveBy { get; set; }

    public DateTime? ApproveOn { get; set; }

    public string? Attachment { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool? IsApproved { get; set; }
}
