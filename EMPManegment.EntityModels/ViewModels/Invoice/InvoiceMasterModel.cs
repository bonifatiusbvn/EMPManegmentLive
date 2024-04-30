using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.Invoice
{
    public class InvoiceMasterModel
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

        public string? ShippingAddress { get; set; }

        public decimal? Cgst { get; set; }

        public decimal? Sgst { get; set; }

        public decimal? Igst { get; set; }

        public decimal? TotalGst { get; set; }

        public decimal? TotalAmount { get; set; }

        public int? PaymentMethod { get; set; }

        public string? Status { get; set; }

        public bool? IsDeleted { get; set; }
        public int? PaymentType { get; set; }

        public decimal? CreditDebitAmount { get; set; }

        public decimal? PendingAmount { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid CreatedBy { get; set; }

        public List<InvoiceDetailsViewModel>? InvoiceDetails { get; set; }
    }
}
