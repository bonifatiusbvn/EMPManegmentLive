﻿using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.ManualInvoice;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Interface.ManualInvoice
{
    public interface IManualInvoice
    {
        Task<UserResponceModel> InsertManualInvoice(ManualInvoiceMasterModel InvoiceDetails);
        Task<jsonData> GetManualInvoiceList(DataTableRequstModel dataTable);
        Task<ManualInvoiceMasterModel> GetManualInvoiceDetails(Guid InvoiceId);
        Task<UserResponceModel> DeleteManualInvoice(Guid InvoiceId);
        Task<UserResponceModel> UpdateManualInvoice(ManualInvoiceMasterModel UpdateInvoice);
    }
}