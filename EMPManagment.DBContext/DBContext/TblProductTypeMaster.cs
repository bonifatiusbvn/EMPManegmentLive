using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblProductTypeMaster
{
    public int Id { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<TblPurchaseOrderMaster> TblPurchaseOrderMasters { get; set; } = new List<TblPurchaseOrderMaster>();

    public virtual ICollection<TblPurchaseRequest> TblPurchaseRequests { get; set; } = new List<TblPurchaseRequest>();
}
