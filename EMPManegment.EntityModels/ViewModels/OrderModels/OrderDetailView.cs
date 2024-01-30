using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.OrderModels
{
    public class OrderDetailView
    {
        public Guid Id { get; set; }

        public string OrderId { get; set; } = null!;
        public string? Type { get; set; }

        public Guid? VendorId { get; set; }

        public string? CompanyName { get; set; }

        public int Product { get; set; }
        public string ProductName { get; set; }

        public string Quantity { get; set; } = null!;

        public decimal? Amount { get; set; }

        public decimal? Total { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public string? PaymentMethod { get; set; }

        public string? DeliveryStatus { get; set; }
        public string ProductDescription { get; set; } = null!;

        public string? ProductShortDescription { get; set; }

        public string? ProductImage { get; set; }

        public decimal ProductStocks { get; set; }

        public decimal PerUnitPrice { get; set; }

        public int? Hsn { get; set; }

        public decimal Gst { get; set; }

        public decimal PerUnitWithGstprice { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? CreatedBy { get; set; }
    }
}
