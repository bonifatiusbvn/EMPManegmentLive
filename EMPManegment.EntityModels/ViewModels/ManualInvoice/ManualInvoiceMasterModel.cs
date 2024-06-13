using EMPManegment.EntityModels.ViewModels.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.ManualInvoice
{
    public class ManualInvoiceMasterModel
    {
        public Guid Id { get; set; }

        public string? InvoiceNo { get; set; }

        public string? VandorName { get; set; }

        public Guid? ProjectId { get; set; }

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

        public int? PaymentStatus { get; set; }

        public string? Status { get; set; }

        public string? ShippingAddress { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public List<ManualInvoiceDetailsModel>? ManualInvoiceDetails { get; set; }
    }
}
