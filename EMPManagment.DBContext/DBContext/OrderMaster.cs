using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class OrderMaster
{
    public Guid Id { get; set; }

    public Guid? ProjectId { get; set; }

    public string? OrderId { get; set; }

    public string? Type { get; set; }

    public Guid? VendorId { get; set; }

    public string? CompanyName { get; set; }

    public string? ProductName { get; set; }

    public string? ProductShortDescription { get; set; }

    public Guid? ProductId { get; set; }

    public int ProductType { get; set; }

    public string Quantity { get; set; } = null!;

    public decimal? Amount { get; set; }

    public decimal? Total { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public string? PaymentMethod { get; set; }

    public string? PaymentStatus { get; set; }

    public string? DeliveryStatus { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public virtual TblProductDetailsMaster? Product { get; set; }

    public virtual TblProductTypeMaster ProductTypeNavigation { get; set; } = null!;

    public virtual TblProjectMaster? Project { get; set; }
}
