using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.OrderModels
{
    public class PurchaseOrderDetailView
    {
        public Guid Id { get; set; }

        public Guid? ProjectId { get; set; }

        public string? OrderId { get; set; }

        public Guid? VendorId { get; set; }

        public Guid? CompanyId { get; set; }

        public decimal? TotalGst { get; set; }

        public decimal? SubTotal { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime OrderDate { get; set; }

        public string? OrderStatus { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public int? PaymentMethod { get; set; }

        public int? PaymentStatus { get; set; }

        public string? DeliveryStatus { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string? CompanyName { get; set; }

        public string? VendorEmail { get; set; }

        public string? VendorContact { get; set; }
        public string? VendorAddress { get; set; }
        public string? InvoiceNo { get; set; }
        public string? PaymentMethodName { get; set; }
    }
    public class PurchaseOrderResponseModel
    {
        public string? Message { get; set; }

        public int Code { get; set; }

        public List<PurchaseOrderDetailView> Data { get; set; }

    }
}
