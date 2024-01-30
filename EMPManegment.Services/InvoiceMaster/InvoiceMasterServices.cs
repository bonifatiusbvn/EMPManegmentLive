using EMPManegment.EntityModels.ViewModels.Invoice;
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
    public class InvoiceMasterServices:IInvoiceMasterServices
    {
        private readonly IInvoiceMaster InvoiceMaster;
        public InvoiceMasterServices(IInvoiceMaster invoiceMaster)
        {
            InvoiceMaster = invoiceMaster;
        }

        public async Task<InvoiceViewModel> GetInvoiceDetailsById(Guid Id)
        {
            return await InvoiceMaster.GetInvoiceDetailsById(Id);
        }

        public async Task<IEnumerable<InvoiceViewModel>> GetInvoiceDetailsList()
        {
            return await InvoiceMaster.GetInvoiceDetailsList();
        }

        public async Task<IEnumerable<InvoiceViewModel>> GetInvoiceNoList()
        {

            return await InvoiceMaster.GetInvoiceNoList();
        }
    }
}
