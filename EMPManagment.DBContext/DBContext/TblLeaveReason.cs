using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblLeaveReason
{
    public int Id { get; set; }

    public string LeaveReason { get; set; } = null!;
}
