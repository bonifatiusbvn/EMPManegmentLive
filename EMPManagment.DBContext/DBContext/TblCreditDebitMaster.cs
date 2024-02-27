using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblCreditDebitMaster
{
    public int Id { get; set; }

    public Guid? VendorId { get; set; }

    public string? Type { get; set; }

    public string? InvoiceNo { get; set; }

    public DateTime? Date { get; set; }

    public int? PaymentType { get; set; }

    public decimal? CreditDebitAmount { get; set; }

    public decimal? PendingAmount { get; set; }

    public decimal? TotalAmount { get; set; }

    public int? PaymentMethod { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual TblPaymentMethodType? PaymentMethodNavigation { get; set; }

    public virtual TblPaymentType? PaymentTypeNavigation { get; set; }

    public virtual TblVendorMaster? Vendor { get; set; }
}
