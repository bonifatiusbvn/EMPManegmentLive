using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.ManualInvoice;
using EMPManegment.EntityModels.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Services.ManualInvoiceServices
{
    public interface IManualInvoiceServices
    {
        Task<UserResponceModel> InsertManualInvoice(ManualInvoiceMasterModel InvoiceDetails);
        Task<jsonData> GetManualInvoiceList(DataTableRequstModel dataTable);
        Task<ManualInvoiceMasterModel> GetManualInvoiceDetails(Guid InvoiceId);
        Task<UserResponceModel> DeleteManualInvoice(Guid InvoiceId);
        Task<UserResponceModel> UpdateManualInvoice(ManualInvoiceMasterModel UpdateInvoice);
    }
}
