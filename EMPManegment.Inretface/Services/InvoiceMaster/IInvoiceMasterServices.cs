using EMPManegment.EntityModels.ViewModels.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Services.InvoiceMaster
{
    public interface IInvoiceMasterServices
    {
        Task<InvoiceViewModel> GetInvoiceDetailsById(Guid Id);
        Task<IEnumerable<InvoiceViewModel>> GetInvoiceNoList();
        Task<IEnumerable<InvoiceViewModel>> GetInvoiceDetailsList();
    }
}
