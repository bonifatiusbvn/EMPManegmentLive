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

        public string OrderId { get; set; }
        public string? Type { get; set; }

        public Guid? VendorId { get; set; }

        public string? CompanyName { get; set; }

        public int Product { get; set; }
        public string ProductName { get; set; }

        public string? Quantity { get; set; }

        public decimal? AmountPerUnit { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal TotalAmount { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public int? PaymentMethod { get; set; }
        public string? PaymentMethodName { get; set; }

        public string? DeliveryStatus { get; set; }
        public string? OrderStatus { get; set; }
        public string ProductDescription { get; set; }

        public string? ProductShortDescription { get; set; }

        public string? ProductImage { get; set; }

        public decimal? ProductStocks { get; set; }

        public decimal? PerUnitPrice { get; set; }

        public int? Hsn { get; set; }

        public decimal? GstPerUnit { get; set; }
        public decimal? TotalGst { get; set; }

        public decimal? PerUnitWithGstprice { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? CreatedBy { get; set; }
        public Guid? ProductId { get; set; }
        public int? PaymentStatus { get; set; }
        public string? VendorEmail { get; set; }

        public string? VendorContact { get; set; }
        public string? VendorAddress { get; set; }
        public string? InvoiceNo { get; set; }
    }
    public class PurchaseOrderResponseModel
    {
        public string? Message { get; set; }

        public int Code { get; set; }

        public List<PurchaseOrderDetailView> Data { get; set; }

    }
    public class UpdatePurchaseOrderView
    {
        public Guid Id { get; set; }
        public string OrderId { get; set; }
        public string? CompanyName { get; set; }
        public string ProductName { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public int? PaymentMethod { get; set; }
        public string? DeliveryStatus { get; set; }
        public string? OrderStatus { get; set; }
    }
}
