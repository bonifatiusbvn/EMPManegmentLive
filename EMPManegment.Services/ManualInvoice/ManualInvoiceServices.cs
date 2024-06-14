using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.ManualInvoice;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.ManualInvoice;
using EMPManegment.Inretface.Services.ManualInvoiceServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.ManualInvoice
{
    public class ManualInvoiceServices:IManualInvoiceServices
    {
        public ManualInvoiceServices(IManualInvoice manualInvoice)
        {
            ManualInvoice = manualInvoice;
        }

        private readonly IManualInvoice ManualInvoice;

        public async Task<UserResponceModel> InsertManualInvoice(ManualInvoiceMasterModel InvoiceDetails)
        {
            return await ManualInvoice.InsertManualInvoice(InvoiceDetails);
        }
        public async Task<jsonData> GetManualInvoiceList(DataTableRequstModel dataTable)
        {
            return await ManualInvoice.GetManualInvoiceList(dataTable);
        }
        public async Task<ManualInvoiceMasterModel> GetManualInvoiceDetails(Guid InvoiceId)
        {
            return await ManualInvoice.GetManualInvoiceDetails(InvoiceId);
        }
        public async Task<UserResponceModel> DeleteManualInvoice(Guid InvoiceId)
        {
            return await ManualInvoice.DeleteManualInvoice(InvoiceId);
        }
        public async Task<UserResponceModel> UpdateManualInvoice(ManualInvoiceMasterModel UpdateInvoice)
        {
            return await ManualInvoice.UpdateManualInvoice(UpdateInvoice);
        }
    }
}
