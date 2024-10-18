using EMPManegment.EntityModels.ViewModels.ProductMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.PurchaseOrderModels
{
    public class PurchaseOrderMasterView
    {
        public Guid Id { get; set; }

        public Guid? ProjectId { get; set; }

        public string? OrderId { get; set; }

        public Guid? VendorId { get; set; }

        public Guid? CompanyId { get; set; }
        public string? CompanyName { get; set; }

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

        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public Guid Poid { get; set; }

        public string Address { get; set; } = null!;

        public string? CompanyFullAddress { get; set; }

        public string? CompanyGstNumber { get; set; }

        public string? VendorFullAddress { get; set; }

        public string? VendorCompanyEmail { get; set; }

        public string? VendorAccountHolderName { get; set; }

        public string? VendorBankAccountNo { get; set; }

        public string? PaymentMethodName { get; set; }
        public string? PaymentStatusName { get; set; }
        public string? CompanyGst { get; set; }
        public string? VendorCompanyName { get; set; }
        public string? VendorCompanyNumber { get; set; }
        public string? VendorGstnumber { get; set; }
        public string? VendorAddress { get; set; }
        public List<PurchaseOrderDetailsModel>? ProductList { get; set; }
        public decimal? DollarPrice { get; set; }
    }
}
