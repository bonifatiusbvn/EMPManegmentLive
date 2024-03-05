using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblProjectMaster
{
    public Guid ProjectId { get; set; }

    public string? ProjectType { get; set; }

    public string? ProjectTitle { get; set; }

    public string? ProjectName { get; set; }

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

    public virtual ICollection<TblInvoice> TblInvoices { get; set; } = new List<TblInvoice>();

    public virtual ICollection<TblProjectDocument> TblProjectDocuments { get; set; } = new List<TblProjectDocument>();

    public virtual ICollection<TblProjectMembe> TblProjectMembes { get; set; } = new List<TblProjectMembe>();

    public virtual ICollection<TblTaskDetail> TblTaskDetails { get; set; } = new List<TblTaskDetail>();
}
