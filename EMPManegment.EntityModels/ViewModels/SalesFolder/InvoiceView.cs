using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.SalesFolder
{
    public class InvoiceView
    {
        public string? InvoiceNo { get; set; }
        public string? CompanyAddress { get; set; }
        public int? PostalCode { get; set; }
        public int? LegalRegistrationNo { get; set; }
        public string? EmailAddress { get; set; }
        public string? Website { get; set; }
        public string? PhoneNo { get; set; }
        public DateOnly? Date { get; set; }
        public string? PaymentStatus { get; set; }
        public string? BillingName { get; set; }
        public string? BillingAddress { get; set; }
        public string? BillingPhoneNo { get; set; }
        public string? BillingTaxNo { get; set; }
        public string? ShippingName { get; set; }
        public string? ShippingAddress { get; set; }
        public string? ShippingPhoneNo { get; set; }
        public string? ShippingTaxNo { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDetails { get; set; }
        public int? Rate { get; set; }
        public int? Quantity { get; set; }
        public int? Amount { get; set; }
        public int? Subtotal { get; set; }
    }
}
