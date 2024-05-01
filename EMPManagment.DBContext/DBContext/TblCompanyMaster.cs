using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblCompanyMaster
{
    public Guid Id { get; set; }

    public string? CompnyName { get; set; }

    public string? Address { get; set; }

    public int? City { get; set; }

    public int? State { get; set; }

    public int? Country { get; set; }

    public string? Gst { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
