using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.Invoice
{
    public class GenerateInvoiceModel
    {
        public Guid Id { get; set; }

        public string InvoiceType { get; set; } = null!;

        public Guid VandorId { get; set; }

        public string? InvoiceNo { get; set; }

        public Guid? ProjectId { get; set; }

        public string? OrderId { get; set; }

        public DateTime InvoiceDate { get; set; }

        public string? BuyesOrderNo { get; set; }

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
        public int? PaymentType { get; set; }

        public decimal? CreditDebitAmount { get; set; }
        public decimal? PendingAmount { get; set; }
    }
}
