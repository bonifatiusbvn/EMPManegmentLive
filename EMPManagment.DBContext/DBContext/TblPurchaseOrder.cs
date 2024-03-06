using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblPurchaseOrder
{
    public Guid Id { get; set; }

    public Guid? VendorId { get; set; }

    public string? Opid { get; set; }

    public string? CompanyName { get; set; }

    public string? ProductName { get; set; }

    public string? ProductShortDescription { get; set; }

    public Guid? ProductId { get; set; }

    public int? ProductType { get; set; }

    public string? Quantity { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public virtual TblProductDetailsMaster? Product { get; set; }

    public virtual TblVendorMaster? Vendor { get; set; }
}
