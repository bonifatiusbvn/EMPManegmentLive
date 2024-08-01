using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.Invoice
{
    public class InvoiceDetailsViewModel
    {
        public int? Id { get; set; }

        public Guid? InvoiceRefId { get; set; }

        public Guid ProductId { get; set; }

        public string? Product { get; set; } 

        public int ProductType { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal? DiscountPer { get; set; }

        public decimal? DiscountAmount { get; set; }

        public decimal GstPer { get; set; }

        public decimal? GstAmount { get; set; }
        public decimal? Igst { get; set; }

        public decimal ProductTotal { get; set; }

        public bool? IsDeleted { get; set; }

        public decimal? PerUnitPrice { get; set; }

        public int? Hsn { get; set; }

        public string? ProductTypeName { get; set; }
        public int? RowNumber { get; set; }
        public string? ProductDescription { get; set; }
        public Guid CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
