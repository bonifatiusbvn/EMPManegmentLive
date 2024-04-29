﻿using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblPurchaseOrderMaster
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

    public decimal? GstPerUnit { get; set; }

    public decimal? TotalGst { get; set; }

    public decimal? AmountPerUnit { get; set; }

    public decimal? SubTotal { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime OrderDate { get; set; }

    public string? OrderStatus { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public int? PaymentMethod { get; set; }

    public string? PaymentStatus { get; set; }

    public string? DeliveryStatus { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public virtual TblPaymentMethodType? PaymentMethodNavigation { get; set; }

    public virtual TblProductDetailsMaster? Product { get; set; }

    public virtual TblProductTypeMaster ProductTypeNavigation { get; set; } = null!;

    public virtual ICollection<TblPurchaseOrderDetail> TblPurchaseOrderDetails { get; set; } = new List<TblPurchaseOrderDetail>();
}
