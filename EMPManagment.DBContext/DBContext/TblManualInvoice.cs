using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblManualInvoice
{
    public Guid Id { get; set; }

    public string? InvoiceNo { get; set; }

    public int? InvoiceType { get; set; }

    public string? VendorName { get; set; }

    public string? VendorPhoneNo { get; set; }

    public string? VendorAddress { get; set; }

    public string? VendorGstNo { get; set; }

    public string? CompanyName { get; set; }

    public string? CompanyAddress { get; set; }

    public string? CompanyGstNo { get; set; }

    public Guid? ProjectId { get; set; }

    public DateTime InvoiceDate { get; set; }

    public string? BuyesOrderNo { get; set; }

    public DateTime? BuyesOrderDate { get; set; }

    public string DispatchThrough { get; set; } = null!;

    public string? DispatchDocNo { get; set; }

    public string? Destination { get; set; }

    public string? MotorVehicleNo { get; set; }

    public decimal? TotalGst { get; set; }

    public decimal? RoundOff { get; set; }

    public decimal? TotalAmount { get; set; }

    public int? PaymentMethod { get; set; }

    public int? PaymentStatus { get; set; }

    public string? Status { get; set; }

    public string? ShippingAddress { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual TblInvoiceTypeMaster? InvoiceTypeNavigation { get; set; }
}
