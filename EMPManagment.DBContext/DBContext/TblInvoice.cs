using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblInvoice
{
    public Guid Id { get; set; }

    public int InvoiceType { get; set; }

    public Guid VandorId { get; set; }

    public string? InvoiceNo { get; set; }

    public DateTime InvoiceDate { get; set; }

    public string BuyesOrderNo { get; set; } = null!;

    public DateTime? BuyesOrderDate { get; set; }

    public Guid ProductId { get; set; }

    public string DispatchThrough { get; set; } = null!;

    public string Destination { get; set; } = null!;

    public decimal? Cgst { get; set; }

    public decimal? Sgst { get; set; }

    public decimal? Igst { get; set; }

    public decimal? TotalGst { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }
}
