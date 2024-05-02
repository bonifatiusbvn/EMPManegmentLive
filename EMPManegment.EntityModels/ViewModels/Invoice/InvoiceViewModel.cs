using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.Invoice
{
    public class InvoiceViewModel
    {
        public Guid Id { get; set; }

        public string InvoiceType { get; set; } = null!;

        public Guid VandorId { get; set; }

        public string? InvoiceNo { get; set; }

        public Guid? ProjectId { get; set; }

        public string? OrderId { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public string? BuyesOrderNo { get; set; }

        public DateTime? BuyesOrderDate { get; set; }

        public Guid ProductId { get; set; }
        public string? ProjectName { get; set; }

        public string? DispatchThrough { get; set; } = null!;

        public string Destination { get; set; } = null!;

        public decimal? Cgst { get; set; }

        public decimal? Sgst { get; set; }

        public decimal? Igst { get; set; }

        public decimal? TotalGst { get; set; }

        public decimal? TotalAmount { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }
        public decimal? PerUnitPrice { get; set; }
        public DateTime Date { get; set; }
        public string VendorName { get; set; }
        public string? PaymentStatus { get; set; }
        public string? BillingName { get; set; }
        public dynamic? BillingAddress { get; set; }
        public dynamic? BillingNumber { get; set; }
        public dynamic? BillingTaxNumber { get; set; }
        public string? ShippingName { get; set; }
        public dynamic? ShippingAddress { get; set; }
        public dynamic? ShippingNumber { get; set; }
        public dynamic? ShippingTaxNumber { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDetails { get; set; }
        public int? HSN { get; set; }
        public decimal? Price { get; set; }
        public string? Quantity { get; set; }
        public decimal? TotalAmountWithQuantity { get; set; }
        public string? PaymentMethod { get; set; }
        public string? CardHolderName { get; set; }
        public decimal? CardNumber { get; set; }
        public string? CompanyAddress { get; set; }
        public string? VendorCompanyEmail { get; set; }
        public string? UserImage { get; set; }
        public string? UserName { get; set; }
        public string? Status { get; set; }
    }
}
