using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblPaymentMethodType
{
    public int Id { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public virtual ICollection<TblCreditDebitMaster> TblCreditDebitMasters { get; set; } = new List<TblCreditDebitMaster>();

    public virtual ICollection<TblInvoice> TblInvoices { get; set; } = new List<TblInvoice>();

    public virtual ICollection<TblPurchaseOrderMaster> TblPurchaseOrderMasters { get; set; } = new List<TblPurchaseOrderMaster>();
}
