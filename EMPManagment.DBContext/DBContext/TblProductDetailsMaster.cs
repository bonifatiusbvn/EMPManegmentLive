using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblProductDetailsMaster
{
    public Guid Id { get; set; }

    public int? ProductType { get; set; }

    public string ProductName { get; set; } = null!;

    public string ProductDescription { get; set; } = null!;

    public string? ProductShortDescription { get; set; }

    public string? ProductImage { get; set; }

    public decimal PerUnitPrice { get; set; }

    public bool? IsWithGst { get; set; }

    public decimal? GstAmount { get; set; }

    public decimal? GstPercentage { get; set; }

    public int? Hsn { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<TblPurchaseOrderDetail> TblPurchaseOrderDetails { get; set; } = new List<TblPurchaseOrderDetail>();

    public virtual ICollection<TblPurchaseRequest> TblPurchaseRequests { get; set; } = new List<TblPurchaseRequest>();
}
