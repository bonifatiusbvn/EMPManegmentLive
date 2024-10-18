using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblPurchaseOrderMaster
{
    public Guid Id { get; set; }

    public Guid? ProjectId { get; set; }

    public string? OrderId { get; set; }

    public Guid? VendorId { get; set; }

    public Guid? CompanyId { get; set; }

    public decimal? DollarPrice { get; set; }

    public decimal? TotalGst { get; set; }

    public decimal? SubTotal { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime OrderDate { get; set; }

    public string? OrderStatus { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public int? PaymentMethod { get; set; }

    public int? PaymentStatus { get; set; }

    public string? DeliveryStatus { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual TblPaymentMethodType? PaymentMethodNavigation { get; set; }

    public virtual TblPaymentType? PaymentStatusNavigation { get; set; }

    public virtual ICollection<TblPurchaseOrderDetail> TblPurchaseOrderDetails { get; set; } = new List<TblPurchaseOrderDetail>();
}
