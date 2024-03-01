﻿using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
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
        Task<IEnumerable<InvoiceViewModel>> GetInvoiceListByVendorId(Guid Vid);
        Task<IEnumerable<CreditDebitView>> GetLastTransactionByVendorId(Guid Vid);
        Task<IEnumerable<CreditDebitView>> GetAllTransactionByVendorId(Guid Vid);
        Task<jsonData> GetAllTransaction(DataTableRequstModel dataTable);
        Task<IEnumerable<InvoiceViewModel>> GetInvoiceDetailsList();
        string CheckInvoiceNo(string OrderId);
        Task<UserResponceModel> InsertInvoiceDetails(GenerateInvoiceModel InsertInvoice);
        Task<OrderResponseModel> GetInvoiceDetailsByOrderId(string OrderId);

    }
}
