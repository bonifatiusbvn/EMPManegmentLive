using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblProductTypeMaster
{
    public int Id { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<TblOrderMaster> TblOrderMasters { get; set; } = new List<TblOrderMaster>();
}
