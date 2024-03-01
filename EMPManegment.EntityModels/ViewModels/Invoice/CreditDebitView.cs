using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.Invoice
{
    public class CreditDebitView
    {
        public int Id { get; set; }

        public Guid? VendorId { get; set; }

        public string? VendorName { get; set; }
        public int PaymentMethod { get; set; }
        public string PaymentMethodName { get; set; }

        public string? Type { get; set; }

        public string? InvoiceNo { get; set; }

        public DateTime? Date { get; set; }

        public int? PaymentType { get; set; }
        public string PaymentTypeName { get; set; }

        public decimal? CreditDebitAmount { get; set; }

        public decimal? PendingAmount { get; set; }

        public decimal? TotalAmount { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }


    }
}
