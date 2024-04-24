using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblRolewiseFormPermission
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public int FormId { get; set; }

    public bool IsAddAllow { get; set; }

    public bool IsViewAllow { get; set; }

    public bool IsEditAllow { get; set; }

    public bool IsDeleteAllow { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual TblForm Form { get; set; } = null!;
}
