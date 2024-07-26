using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblManualInvoiceDetail
{
    public int Id { get; set; }

    public Guid RefId { get; set; }

    public string Product { get; set; } = null!;

    public int? Hsn { get; set; }

    public int? ProductType { get; set; }

    public decimal Quantity { get; set; }

    public decimal Price { get; set; }

    public decimal? DiscountPercent { get; set; }

    public decimal? Discount { get; set; }

    public decimal? GstAmount { get; set; }

    public decimal Gst { get; set; }

    public decimal? Igst { get; set; }

    public decimal ProductTotal { get; set; }

    public bool? IsDeleted { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
