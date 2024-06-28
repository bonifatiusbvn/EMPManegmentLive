using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblPurchaseOrderDetail
{
    public int Id { get; set; }

    public Guid PorefId { get; set; }

    public Guid ProductId { get; set; }

    public int ProductType { get; set; }

    public decimal Quantity { get; set; }

    public int? Hsn { get; set; }

    public decimal Price { get; set; }

    public decimal? Gstper { get; set; }

    public decimal Gstamount { get; set; }

    public decimal ProductTotal { get; set; }

    public bool? IsDeleted { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual TblPurchaseOrderMaster Poref { get; set; } = null!;

    public virtual TblProductDetailsMaster Product { get; set; } = null!;

    public virtual TblProductTypeMaster ProductTypeNavigation { get; set; } = null!;
}
