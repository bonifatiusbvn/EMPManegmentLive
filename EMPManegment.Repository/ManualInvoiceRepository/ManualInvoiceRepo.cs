using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels.ManualInvoice;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.ManualInvoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Repository.ManualInvoiceRepository
{
    public class ManualInvoiceRepo : IManualInvoice
    {
        public ManualInvoiceRepo(BonifatiusEmployeesContext context)
        {
            Context = context;
        }

        public BonifatiusEmployeesContext Context { get; }

        public async Task<UserResponceModel> InsertManualInvoice(ManualInvoiceMasterModel InvoiceDetails)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var invoice = new TblManualInvoice()
                {
                    Id = Guid.NewGuid(),
                    InvoiceNo = InvoiceDetails.InvoiceNo,
                    VandorName = InvoiceDetails.VandorName,
                    ProjectId = InvoiceDetails.ProjectId,
                    OrderId = InvoiceDetails.OrderId,
                    InvoiceDate = InvoiceDetails.InvoiceDate,
                    BuyesOrderNo = InvoiceDetails.BuyesOrderNo,
                    BuyesOrderDate = InvoiceDetails.BuyesOrderDate,
                    DispatchThrough = InvoiceDetails.DispatchThrough,
                    ShippingAddress = InvoiceDetails.ShippingAddress,
                    Cgst = InvoiceDetails.Cgst,
                    Sgst = InvoiceDetails.Sgst,
                    Igst = InvoiceDetails.Igst,
                    TotalGst = InvoiceDetails.TotalGst,
                    TotalAmount = InvoiceDetails.TotalAmount,
                    PaymentMethod = InvoiceDetails.PaymentMethod,
                    Status = InvoiceDetails.Status,
                    PaymentStatus = InvoiceDetails.PaymentStatus,
                    IsDeleted = false,
                    CreatedBy = InvoiceDetails.CreatedBy,
                    CreatedOn = DateTime.Now,
                };
                Context.TblManualInvoices.Add(invoice);

                foreach (var item in InvoiceDetails.ManualInvoiceDetails)
                {
                    var invoiceDetails = new TblManualInvoiceDetail()
                    {
                        Id = invoice.Id,
                        Product = item.Product,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        Discount = item.Discount,
                        Gst = item.Gst,
                        ProductTotal = item.ProductTotal,
                        IsDeleted = false,
                        CreatedBy = invoice.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    Context.TblManualInvoiceDetails.Add(invoiceDetails);
                }
                await Context.SaveChangesAsync();
                response.Code = (int)HttpStatusCode.OK;
                response.Message = "Invoice successfully inserted.";
            }
            catch (Exception)
            {
                response.Code = 400;
                response.Message = "Error in inserting Invoice.";
            }

            return response;
        }
    }
}
