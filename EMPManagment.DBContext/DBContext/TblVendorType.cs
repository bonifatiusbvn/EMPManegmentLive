using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblVendorType
{
    public int Id { get; set; }

    public string VendorName { get; set; } = null!;

    public virtual ICollection<TblVendorMaster> TblVendorMasters { get; set; } = new List<TblVendorMaster>();
}
