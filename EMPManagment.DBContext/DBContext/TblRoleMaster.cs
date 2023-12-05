using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblRoleMaster
{
    public int Id { get; set; }

    public string? Role { get; set; }

    public virtual ICollection<TblUser> TblUsers { get; set; } = new List<TblUser>();
}
