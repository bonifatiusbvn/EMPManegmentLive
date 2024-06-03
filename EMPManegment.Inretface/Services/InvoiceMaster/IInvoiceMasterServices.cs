using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Services.InvoiceMaster
{
    public interface IInvoiceMasterServices
    {
        Task<InvoiceMasterModel> GetInvoiceDetailsById(Guid Id);
        Task<InvoicePayVendorModel> GetInvoiceListByVendorId(Guid Vid);
        Task<jsonData> GetAllTransactionByVendorId(Guid Vid, DataTableRequstModel dataTable);
        Task<IEnumerable<CreditDebitView>> GetLastTransactionByVendorId(Guid Vid);
        Task<List<CreditDebitView>> GetAllTransaction();
        Task<jsonData> GetInvoiceDetailsList(DataTableRequstModel dataTable);
        string CheckInvoiceNo(string porjectname);
        Task<UserResponceModel> InsertInvoiceDetails(InvoiceMasterModel InsertInvoice);
        Task<PurchaseOrderResponseModel> GetInvoiceDetailsByOrderId(string OrderId);
        Task<UserResponceModel> InsertCreditDebitDetails(CreditDebitView CreditDebit);
        Task<IEnumerable<CreditDebitView>> GetCreditDebitListByVendorId(Guid Vid);
        Task<PurchaseOrderResponseModel> DisplayInvoiceDetails(string OrderId);
        Task<UserResponceModel> IsDeletedInvoice(Guid InvoiceId);
        Task<UpdateInvoiceModel> EditInvoiceDetails(string InvoiceNo);
        Task<UserResponceModel> UpdateInvoiceDetails(UpdateInvoiceModel UpdateInvoice);
        Task<PurchaseOrderResponseModel> ShowInvoiceDetailsByOrderId(string OrderId);
        Task<IEnumerable<InvoiceViewModel>> InvoicActivity(Guid ProId);
    }
}
