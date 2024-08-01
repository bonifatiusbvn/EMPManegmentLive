using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.Invoice
{
    public class InvoiceMasterModel
    {
        public Guid Id { get; set; }

        public string? InvoiceType { get; set; }

        public Guid VandorId { get; set; }

        public string? InvoiceNo { get; set; }

        public Guid? ProjectId { get; set; }

        public string? OrderId { get; set; }

        public DateTime InvoiceDate { get; set; }

        public string? BuyesOrderNo { get; set; }

        public DateTime? BuyesOrderDate { get; set; }

        public string? DispatchThrough { get; set; }

        public string? DispatchDocNo { get; set; }

        public string? Destination { get; set; }

        public string? MotorVehicleNo { get; set; }

        public string? ShippingAddress { get; set; }

        public decimal? TotalDiscount { get; set; }

        public decimal? TotalGst { get; set; }
        public decimal? RoundOff { get; set; }

        public decimal? TotalAmount { get; set; }

        public int? PaymentMethod { get; set; }

        public int? PaymentStatus { get; set; }

        public string? Status { get; set; }

        public bool? IsDeleted { get; set; }
        public int? PaymentType { get; set; }

        public decimal? CreditDebitAmount { get; set; }

        public decimal? PendingAmount { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid CreatedBy { get; set; }
        public string? VendorName { get; set; }
        public string? VendorFullAddress { get; set; }
        public string? VendorGstnumber { get; set; }

        public string? VendorEmail {  get; set; }
        public string? ProjectName {  get; set; }
        public string? VendorCompanyName {  get; set; }
        public string? VendorBankName { get; set; }

        public string? VendorBankBranch { get; set; }

        public string? VendorAccountHolderName { get; set; }

        public string? VendorBankAccountNo { get; set; } 

        public string? VendorBankIfsc { get; set; }
        public string? PaymentMethodName { get; set; }

        public string? PaymentStatusName { get; set; }
        public string? VendorAddress { get; set; }
        public string? VendorCompanyNumber { get; set; }
        public Guid? CompanyId { get; set; }

        public string? CompnyName { get; set; }
        public string? CompanyGst { get; set; }
        public string? CompanyAddress { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? Date { get; set; }

        public List<InvoiceDetailsViewModel>? InvoiceDetails { get; set; }
    }
}
