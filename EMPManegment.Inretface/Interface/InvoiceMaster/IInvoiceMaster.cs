using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
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
        Task<InvoiceViewModel> GetInvoiceDetailsById(Guid Id);
        Task<IEnumerable<InvoiceViewModel>> GetInvoiceListByVendorId(Guid Vid);
        Task<IEnumerable<CreditDebitView>> GetLastTransactionByVendorId(Guid Vid);
        Task<IEnumerable<CreditDebitView>> GetAllTransactionByVendorId(Guid Vid);
        Task<jsonData> GetAllTransaction(DataTableRequstModel dataTable);
        Task<IEnumerable<InvoiceViewModel>> GetInvoiceDetailsList();
        string CheckInvoiceNo(string OrderId);
        Task<UserResponceModel> InsertInvoiceDetails(GenerateInvoiceModel InsertInvoice);
        Task<OrderResponseModel> GetInvoiceDetailsByOrderId(string OrderId);
        Task<IEnumerable<CreditDebitView>> GetCreditDebitListByVendorId(Guid Vid);
        Task<UserResponceModel> InsertCreditDebitDetails(CreditDebitView CreditDebit);
    }
}
