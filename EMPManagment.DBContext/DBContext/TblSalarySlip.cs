using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblSalarySlip
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public string? SalarySlip { get; set; }

    public DateTime? Month { get; set; }

    public virtual TblUser User { get; set; } = null!;
}
