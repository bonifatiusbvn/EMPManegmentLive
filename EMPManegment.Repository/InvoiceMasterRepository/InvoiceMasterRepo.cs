using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using EMPManegment.Inretface.Interface.InvoiceMaster;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Repository.InvoiceMasterRepository
{
    public class InvoiceMasterRepo : IInvoiceMaster
    {
        public InvoiceMasterRepo(BonifatiusEmployeesContext Context)
        {
            this.Context = Context;
        }
        public BonifatiusEmployeesContext Context { get; }
        public async Task<InvoiceViewModel> GetInvoiceDetailsById(Guid Id)
        {
            InvoiceViewModel invoice = new InvoiceViewModel();
            try
            {
                invoice = (from a in Context.TblInvoices.Where(x => x.Id == Id)
                           join b in Context.TblVendorMasters
                           on a.VandorId equals b.Vid
                           join c in Context.TblProductDetailsMasters
                           on a.ProductId equals c.Id
                           //join d in Context.OrderMasters on a.VandorId equals d.VendorId
                           select new InvoiceViewModel
                           {
                               Id = a.Id,
                               InvoiceNo=a.InvoiceNo,
                               VendorName = b.VendorCompany,
                               VandorId = a.VandorId,
                               ProductName = c.ProductName,
                               ProductDetails = c.ProductShortDescription,
                               HSN = c.Hsn,
                               Price = c.PerUnitPrice,
                               TotalGst=c.Gst,
                               DispatchThrough = a.DispatchThrough,
                               Destination = a.Destination,
                               ProductId = a.ProductId,
                               Cgst=a.Cgst,
                               Igst=a.Igst,
                               Sgst=a.Sgst,
                               BuyesOrderNo = a.BuyesOrderNo,
                               BuyesOrderDate=a.BuyesOrderDate,
                               TotalAmount=a.TotalAmount,
                               CreatedOn=a.CreatedOn,
                               CreatedBy = a.CreatedBy,
                               UpdatedOn=a.UpdatedOn,
                               UpdatedBy=a.UpdatedBy,
                               PerUnitPrice=c.PerUnitPrice,
                               //PaymentMethod = d.PaymentMethod,
                               //PaymentStatus = d.PaymentStatus,
                               //Quantity = d.Quantity,
                               //TotalAmountWithQuantity = d.Total,

                           }).First();
                return invoice;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<InvoiceViewModel>> GetInvoiceDetailsList()
        {
            IEnumerable<InvoiceViewModel> InvoiceList = from a in Context.TblInvoices
                                                        join b in Context.TblVendorMasters on a.VandorId equals b.Vid
                                                        join c in Context.TblProductDetailsMasters on a.ProductId equals c.Id
                                                        select new InvoiceViewModel
                                                        {
                                                            Id = a.Id,
                                                            InvoiceNo = a.InvoiceNo,
                                                            InvoiceType = a.InvoiceType,
                                                            InvoiceDate = a.InvoiceDate,
                                                            VendorName = b.VendorCompany,
                                                            VandorId = a.VandorId,
                                                            ProductName = c.ProductName,
                                                            ProductDetails = c.ProductShortDescription,
                                                            HSN = c.Hsn,
                                                            Price = c.PerUnitPrice,
                                                            TotalGst = c.Gst,
                                                            DispatchThrough = a.DispatchThrough,
                                                            Destination = a.Destination,
                                                            ProductId = a.ProductId,
                                                            Cgst = a.Cgst,
                                                            Igst = a.Igst,
                                                            Sgst = a.Sgst,
                                                            BuyesOrderNo = a.BuyesOrderNo,
                                                            BuyesOrderDate = a.BuyesOrderDate,
                                                            TotalAmount = a.TotalAmount,
                                                            CreatedOn = a.CreatedOn,
                                                            CreatedBy = a.CreatedBy,
                                                            UpdatedOn = a.UpdatedOn,
                                                            UpdatedBy = a.UpdatedBy,
                                                            PerUnitPrice = c.PerUnitPrice
                                                        };
            return InvoiceList;
        }

        public async Task<IEnumerable<InvoiceViewModel>> GetInvoiceNoList()
        {
            IEnumerable<InvoiceViewModel> GetInvoiceList = Context.TblInvoices.ToList().Select(a => new InvoiceViewModel
            {
                Id = a.Id,
                InvoiceNo = a.InvoiceNo,

            }).ToList();
            return GetInvoiceList;
        }
    }
}
