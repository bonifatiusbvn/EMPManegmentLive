using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.Inretface.Interface.InvoiceMaster;
using EMPManegment.Inretface.Interface.ProductMaster;
using EMPManegment.Inretface.Services.InvoiceMaster;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.InvoiceMaster
{
    public class InvoiceMasterServices : IInvoiceMasterServices
    {
        private readonly IInvoiceMaster InvoiceMaster;
        public InvoiceMasterServices(IInvoiceMaster invoiceMaster)
        {
            InvoiceMaster = invoiceMaster;
        }

        public string CheckInvoiceNo(string porjectname)
        {
            return InvoiceMaster.CheckInvoiceNo(porjectname);
        }

        public Task<OrderResponseModel> DisplayInvoiceDetails(string OrderId)
        {
            return InvoiceMaster.DisplayInvoiceDetails(OrderId);
        }

        public Task<UpdateInvoiceModel> EditInvoiceDetails(string InvoiceNo)
        {
            return InvoiceMaster.EditInvoiceDetails(InvoiceNo);
        }

        public Task<jsonData> GetAllTransaction(DataTableRequstModel dataTable)
        {
            return InvoiceMaster.GetAllTransaction(dataTable);
        }

        public async Task<IEnumerable<CreditDebitView>> GetAllTransactionByVendorId(Guid Vid)
        {
            return await InvoiceMaster.GetAllTransactionByVendorId(Vid);
        }

        public async Task<IEnumerable<CreditDebitView>> GetCreditDebitListByVendorId(Guid Vid)
        {
            return await InvoiceMaster.GetCreditDebitListByVendorId(Vid);
        }

        public async Task<InvoiceViewModel> GetInvoiceDetailsById(Guid Id)
        {
            return await InvoiceMaster.GetInvoiceDetailsById(Id);
        }

        public async Task<OrderResponseModel> GetInvoiceDetailsByOrderId(string OrderId)
        {
            return await InvoiceMaster.GetInvoiceDetailsByOrderId(OrderId);
        }

        public async Task<IEnumerable<InvoiceViewModel>> GetInvoiceDetailsList()
        {
            return await InvoiceMaster.GetInvoiceDetailsList();
        }

        public async Task<IEnumerable<InvoiceViewModel>> GetInvoiceListByVendorId(Guid Vid)
        {
            return await InvoiceMaster.GetInvoiceListByVendorId(Vid);
        }

        public async Task<IEnumerable<CreditDebitView>> GetLastTransactionByVendorId(Guid Vid)
        {
            return await InvoiceMaster.GetLastTransactionByVendorId(Vid);
        }

        public async Task<UserResponceModel> InsertCreditDebitDetails(CreditDebitView CreditDebit)
        {
            return await InvoiceMaster.InsertCreditDebitDetails(CreditDebit);
        }

        public async Task<UserResponceModel> InsertInvoiceDetails(GenerateInvoiceModel InsertInvoice)
        {
            return await InvoiceMaster.InsertInvoiceDetails(InsertInvoice);
        }

        public async Task<UserResponceModel> IsDeletedInvoice(string InvoiceNo)
        {
            return await InvoiceMaster.IsDeletedInvoice(InvoiceNo);
        }

        public async Task<UserResponceModel> UpdateInvoiceDetails(UpdateInvoiceModel UpdateInvoice)
        {
            return await InvoiceMaster.UpdateInvoiceDetails(UpdateInvoice);
        }

        public async Task<OrderResponseModel> ShowInvoiceDetailsByOrderId(string OrderId)
        {
            return await InvoiceMaster.ShowInvoiceDetailsByOrderId(OrderId);
        }
    }
}
