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
    }
}
