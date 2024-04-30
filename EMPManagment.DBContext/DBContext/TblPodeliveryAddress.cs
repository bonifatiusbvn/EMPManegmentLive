using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblPodeliveryAddress
{
    public int Aid { get; set; }

    public Guid Poid { get; set; }

    public string Address { get; set; } = null!;

    public bool? IsDeleted { get; set; }
}
