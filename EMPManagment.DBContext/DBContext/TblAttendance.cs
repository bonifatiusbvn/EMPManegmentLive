using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblAttendance
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public DateTime Date { get; set; }

    public DateTime Intime { get; set; }

    public DateTime? OutTime { get; set; }

    public TimeSpan? TotalHours { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual TblUser User { get; set; } = null!;
}
