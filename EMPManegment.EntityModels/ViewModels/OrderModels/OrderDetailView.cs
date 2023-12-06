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

        public string CompanyName { get; set; } = null!;

        public string Product { get; set; } = null!;

        public string Quantity { get; set; } = null!;

        public decimal? Amount { get; set; }

        public decimal? Total { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public string? PaymentMethod { get; set; }

        public string? DeliveryStatus { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? CreatedBy { get; set; }
    }
}
