using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblForm
{
    public int FormId { get; set; }

    public string? FormGroup { get; set; }

    public string FormName { get; set; } = null!;

    public string? Controller { get; set; }

    public int? OrderId { get; set; }

    public string? Action { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<TblRolewiseFormPermission> TblRolewiseFormPermissions { get; set; } = new List<TblRolewiseFormPermission>();
}
