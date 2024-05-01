using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblProjectDocument
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }

    public Guid UserId { get; set; }

    public DateTime Date { get; set; }

    public string DocumentName { get; set; } = null!;

    public DateTime? CreadetOn { get; set; }

    public Guid? CreatdBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual TblProjectMaster Project { get; set; } = null!;

    public virtual TblUser User { get; set; } = null!;
}
