using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblInvoiceTypeMaster
{
    public int Id { get; set; }

    public string? InvoiceType { get; set; }

    public virtual ICollection<TblManualInvoice> TblManualInvoices { get; set; } = new List<TblManualInvoice>();
}
