using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblTaskMaster
{
    public int Id { get; set; }

    public string? TaskType { get; set; }

    public virtual ICollection<TblTaskDetail> TblTaskDetails { get; set; } = new List<TblTaskDetail>();
}
