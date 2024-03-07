using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.OrderModels
{
    public class OrderView
    {
        public Guid Id { get; set; }

        public string? Type { get; set; }

        public Guid? VendorId { get; set; }

        public string? CompanyName { get; set; }

        public int ProductType { get; set; }
        public Guid? ProductId { get; set; }
        public decimal? TotalGst { get; set; }
        public decimal? GstPerUnit { get; set; }

        public string Quantity { get; set; } = null!;

        public decimal? AmountPerUnit { get; set; }
        public decimal? SubTotal { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public int? PaymentMethod { get; set; }

        public string? PaymentStatus { get; set; }

        public string? DeliveryStatus { get; set; }
        public string? OrderStatus { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? CreatedBy { get; set; }

        public string? OrderId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductShortDescription { get; set; }
        public Guid ProjectId { get; set; }
    }
}
