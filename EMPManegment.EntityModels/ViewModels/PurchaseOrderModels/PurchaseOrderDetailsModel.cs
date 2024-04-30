using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.PurchaseOrderModels
{
    public class PurchaseOrderDetailsModel
    {
        public int Id { get; set; }

        public Guid PorefId { get; set; }

        public Guid ProductId { get; set; }

        public string Product { get; set; } = null!;

        public int ProductType { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal? Discount { get; set; }

        public decimal Gst { get; set; }

        public decimal ProductTotal { get; set; }

        public bool? IsDeleted { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string? ProductTypeName { get; set; }
    }
}
