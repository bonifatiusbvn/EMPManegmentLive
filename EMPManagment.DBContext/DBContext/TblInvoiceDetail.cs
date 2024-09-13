using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblInvoiceDetail
{
    public int Id { get; set; }

    public Guid InvoiceRefId { get; set; }

    public Guid ProductId { get; set; }

    public string Product { get; set; } = null!;

    public string? Description { get; set; }

    public int? Hsn { get; set; }

    public int ProductType { get; set; }

    public decimal Quantity { get; set; }

    public decimal Price { get; set; }

    public decimal? DiscountPer { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal GstPer { get; set; }

    public decimal? GstAmount { get; set; }

    public decimal? Igst { get; set; }

    public decimal ProductTotal { get; set; }

    public bool? IsDeleted { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual TblInvoice InvoiceRef { get; set; } = null!;
}
