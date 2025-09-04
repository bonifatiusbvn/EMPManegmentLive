using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.Leave
{
    public class LeaveMasterModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public string? UserName { get; set; }

        public int Reason { get; set; }

        public string? ReasonName { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public double Days { get; set; }

        public string? Description { get; set; }

        public string Approvers { get; set; } = null!;

        public string? ApproverName { get; set; }

        public Guid? ApproveBy { get; set; }

        public string? ApprovedByName { get; set; }

        public DateTime? ApproveOn { get; set; }

        public string? Attachment { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public bool? IsApproved { get; set; }
    }

    public class LeaveReasonModel
    {
        public int Id { get; set; }

        public string LeaveReason { get; set; } = null!;
    }

    public class ApproveLeaveModel
    {
        public Guid Id { get; set; }

        public Guid? ApproveBy { get; set; }

        public DateTime? ApproveOn { get; set; }

        public bool? IsApproved { get; set; }
    }
}
