using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblPaymentType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public virtual ICollection<TblCreditDebitMaster> TblCreditDebitMasters { get; set; } = new List<TblCreditDebitMaster>();

    public virtual ICollection<TblExpenseMaster> TblExpenseMasters { get; set; } = new List<TblExpenseMaster>();

    public virtual ICollection<TblInvoice> TblInvoices { get; set; } = new List<TblInvoice>();

    public virtual ICollection<TblPurchaseOrderMaster> TblPurchaseOrderMasters { get; set; } = new List<TblPurchaseOrderMaster>();
}
