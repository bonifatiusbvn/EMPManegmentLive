using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.Inretface.Interface.InvoiceMaster;
using EMPManegment.Inretface.Interface.ProductMaster;
using EMPManegment.Inretface.Services.InvoiceMaster;
using System;
using System.Collections.Generic;
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

        public string CheckInvoiceNo(string OrderId)
        {
            return InvoiceMaster.CheckInvoiceNo(OrderId);
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

        public async Task<IEnumerable<InvoiceViewModel>> GetInvoiceNoList()
        {

            return await InvoiceMaster.GetInvoiceNoList();
        }

        public async Task<UserResponceModel> InsertInvoiceDetails(GenerateInvoiceModel InsertInvoice)
        {
            return await InvoiceMaster.InsertInvoiceDetails(InsertInvoice);
        }
    }
}
