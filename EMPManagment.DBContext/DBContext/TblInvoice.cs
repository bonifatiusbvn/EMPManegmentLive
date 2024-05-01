﻿using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblInvoice
{
    public Guid Id { get; set; }

    public string? InvoiceType { get; set; }

    public Guid VandorId { get; set; }

    public string? InvoiceNo { get; set; }

    public Guid? ProjectId { get; set; }

    public string? OrderId { get; set; }

    public DateTime InvoiceDate { get; set; }

    public string? BuyesOrderNo { get; set; }

    public DateTime? BuyesOrderDate { get; set; }

    public string DispatchThrough { get; set; } = null!;

    public decimal? Cgst { get; set; }

    public decimal? Sgst { get; set; }

    public decimal? Igst { get; set; }

    public decimal? TotalGst { get; set; }

    public decimal? TotalAmount { get; set; }

    public int? PaymentMethod { get; set; }

    public int? Status { get; set; }

    public string? ShippingAddress { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual TblPaymentMethodType? PaymentMethodNavigation { get; set; }

    public virtual TblProjectMaster? Project { get; set; }

    public virtual TblPaymentType? StatusNavigation { get; set; }

    public virtual ICollection<TblInvoiceDetail> TblInvoiceDetails { get; set; } = new List<TblInvoiceDetail>();
}
