using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblPageMaster
{
    public int Id { get; set; }

    public string? PageName { get; set; }

    public Guid? UserId { get; set; }

    public virtual TblUser? User { get; set; }
}
