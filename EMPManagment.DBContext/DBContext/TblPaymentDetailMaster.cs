using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblPaymentDetailMaster
{
    public int Id { get; set; }

    public Guid? VendorId { get; set; }

    public Guid? ProductId { get; set; }

    public string? ProductName { get; set; }

    public decimal? Amount { get; set; }

    public string? Paid { get; set; }

    public decimal? Pending { get; set; }

    public virtual TblProductDetailsMaster? Product { get; set; }

    public virtual TblVendorMaster? Vendor { get; set; }
}
