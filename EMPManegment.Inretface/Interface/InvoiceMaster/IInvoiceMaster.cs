using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.EntityModels.ViewModels.Purchase_Request;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Interface.InvoiceMaster
{
    public interface IInvoiceMaster
    {
        Task<InvoicePayVendorModel> GetInvoiceListByVendorId(Guid Vid);
        Task<IEnumerable<CreditDebitView>> GetLastTransactionByVendorId(Guid Vid);
        Task<List<CreditDebitView>> GetAllTransaction();
        Task<jsonData> GetAllTransactionByVendorId(Guid Vid, DataTableRequstModel dataTable);
        Task<jsonData> GetInvoiceDetailsList(DataTableRequstModel dataTable);
        Task<IEnumerable<CreditDebitView>> GetCreditDebitListView();
        string CheckInvoiceNo(string porjectname);
        Task<UserResponceModel> InsertInvoiceDetails(InvoiceMasterModel InsertInvoice);
        Task<PurchaseOrderResponseModel> GetInvoiceDetailsByOrderId(string OrderId);
        Task<IEnumerable<CreditDebitView>> GetCreditDebitListByVendorId(Guid Vid);
        Task<UserResponceModel> InsertCreditDebitDetails(CreditDebitView CreditDebit);
        Task<PurchaseOrderResponseModel> DisplayInvoiceDetails(string OrderId);
        Task<UserResponceModel> IsDeletedInvoice(Guid InvoiceId);
        Task<UpdateInvoiceModel> EditInvoiceDetails(string InvoiceNo);
        Task<UserResponceModel> UpdateInvoiceDetails(InvoiceMasterModel UpdateInvoice);
        Task<PurchaseOrderResponseModel> ShowInvoiceDetailsByOrderId(string OrderId);
        Task<IEnumerable<InvoiceViewModel>> InvoiceActivity(Guid ProjectId);
        Task<InvoiceMasterModel> DisplayInvoiceDetailsById(Guid Id);
        Task<List<InvoiceDetailsViewModel>> GetProductDetailsById(Guid ProductId);
        Task<IEnumerable<InvoiceViewModel>> InvoicActivityByUserId(Guid UserId);
        Task<UserResponceModel> DeleteTransaction(int Id);
    }
}
