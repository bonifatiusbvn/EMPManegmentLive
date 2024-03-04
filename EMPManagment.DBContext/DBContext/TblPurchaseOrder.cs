using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblPurchaseOrder
{
    public Guid Id { get; set; }

    public Guid? VendorId { get; set; }

    public string? Opid { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? Status { get; set; }

    public virtual TblVendorMaster? Vendor { get; set; }
}
