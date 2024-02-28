using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using EMPManegment.Inretface.Interface.InvoiceMaster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EMPManegment.Repository.InvoiceMasterRepository
{
    public class InvoiceMasterRepo : IInvoiceMaster
    {
        private readonly BonifatiusEmployeesContext Context;

        public InvoiceMasterRepo(BonifatiusEmployeesContext Context)
        {
            Context = Context;
        }


        public string CheckInvoiceNo(string OrderId)
        {
            try
            {
                var LastInvoiceId = Context.TblInvoices.OrderByDescending(e => e.CreatedOn).FirstOrDefault();
                string InvoiceId;
                var invoice = (from a in Context.TblOrderMasters.Where(x => x.OrderId == OrderId)
                               join b in Context.TblProjectMasters
                               on a.ProjectId equals b.ProjectId
                               select new CheckInvoiceView
                               {
                                   OrderId = a.OrderId,
                                   ProjectId = a.ProjectId,
                                   ProjectName = b.ProjectName
                               }).FirstOrDefault();

                if (LastInvoiceId == null)
                {
                    InvoiceId = "BTPL/INVOICE/" + invoice.ProjectName + "/23-24-001";
                }
                else
                {
                    if (LastInvoiceId.InvoiceNo.Length >= 24)
                    {
                        int orderNumber = int.Parse(LastInvoiceId.InvoiceNo.Substring(29)) + 1;
                        InvoiceId = "BTPL/INVOICE/" + invoice.ProjectName + "/23-24-" + orderNumber.ToString("D3");
                    }
                    else
                    {
                        throw new Exception("InvoiceId does not have expected format.");
                    }
                }
                return InvoiceId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<CreditDebitView>> GetCreditDebitListView()
        {
            try
            {
                IEnumerable<CreditDebitView> Payment = Context.TblCreditDebitMasters.ToList().Select(a => new CreditDebitView
                {
                    Id = a.Id,
                    VendorId = a.VendorId,
                    Type = a.Type,
                    InvoiceNo = a.InvoiceNo,
                    Date = a.Date,
                    PaymentType = a.PaymentType,
                    CreditDebitAmount = a.CreditDebitAmount,
                    PendingAmount = a.PendingAmount,
                    TotalAmount = a.TotalAmount,
                    CreatedOn = a.CreatedOn,
                    CreatedBy = a.CreatedBy,
                    UpdatedOn = a.UpdatedOn,
                    UpdatedBy = a.UpdatedBy,
                });
                return Payment;
            }
            catch (Exception )
            {
                throw ;
            }
        }

        public async Task<InvoiceViewModel> GetInvoiceDetailsById(Guid Id)
        {
            InvoiceViewModel invoice = new InvoiceViewModel();
            try
            {
                invoice = (from a in Context.TblInvoices.Where(x => x.Id == Id)
                           join b in Context.TblVendorMasters
                           on a.VandorId equals b.Vid
                           //join d in Context.OrderMasters on a.VandorId equals d.VendorId
                           select new InvoiceViewModel
                           {
                               Id = a.Id,
                               InvoiceNo = a.InvoiceNo,
                               VendorName = b.VendorCompany,
                               VandorId = a.VandorId,
                               //ProductName = c.ProductName,
                               //ProductDetails = c.ProductShortDescription,
                               //HSN = c.Hsn,
                               //Price = c.PerUnitPrice,
                               //TotalGst=c.Gst,
                               DispatchThrough = a.DispatchThrough,
                               Destination = a.Destination,
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
                               //PerUnitPrice=c.PerUnitPrice,
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

        public async Task<OrderResponseModel> GetInvoiceDetailsByOrderId(string OrderId)
        {
            OrderResponseModel response = new OrderResponseModel();
            try
            {
                bool isInvoiceAlredyExists = Context.TblInvoices.Any(x => x.OrderId == OrderId);
                if (isInvoiceAlredyExists == true)
                {

                    response.Message = "This Invoice Is already Generated";
                    response.Code = 400;
                }
                else
                {
                    var orderDetails = new List<OrderDetailView>();
                    var data = await (from a in Context.TblOrderMasters
                                      join c in Context.TblProductDetailsMasters on a.ProductId equals c.Id
                                      join b in Context.TblVendorMasters on a.VendorId equals b.Vid
                                      where a.OrderId == OrderId
                                      select new OrderDetailView
                                      {
                                          Id = a.Id,
                                          OrderId = a.OrderId,
                                          VendorId = a.VendorId,
                                          Type = a.Type,
                                          CompanyName = a.CompanyName,
                                          ProductId = a.ProductId,
                                          VendorAddress = b.VendorAddress,
                                          VendorContact = b.VendorContact,
                                          VendorEmail = b.VendorCompanyEmail,
                                          ProductImage = c.ProductImage,
                                          ProductName = a.ProductName,
                                          ProductShortDescription = a.ProductShortDescription,
                                          Quantity = a.Quantity,
                                          OrderDate = a.OrderDate,
                                          PerUnitPrice = c.PerUnitPrice,
                                          PerUnitWithGstprice = c.PerUnitWithGstprice,
                                          Total = a.Total,
                                          Amount = a.Amount,
                                          PaymentMethod = a.PaymentMethod,
                                          PaymentStatus = a.PaymentStatus,
                                          DeliveryStatus = a.DeliveryStatus,
                                          DeliveryDate = a.DeliveryDate,
                                          CreatedOn = a.CreatedOn,
                                      }).ToListAsync();
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            orderDetails.Add(new OrderDetailView()
                            {
                                Id = item.Id,
                                OrderId = item.OrderId,
                                CompanyName = item.CompanyName,
                                VendorId = item.VendorId,
                                ProductId = item.ProductId,
                                VendorEmail = item.VendorEmail,
                                VendorContact = item.VendorContact,
                                VendorAddress = item.VendorAddress,
                                ProductName = item.ProductName,
                                ProductImage = item.ProductImage,
                                ProductShortDescription = item.ProductShortDescription,
                                Quantity = item.Quantity,
                                OrderDate = item.OrderDate,
                                PerUnitPrice = item.PerUnitPrice,
                                PerUnitWithGstprice = item.PerUnitWithGstprice,
                                Total = item.Total,
                                Amount = item.Amount,
                                PaymentMethod = item.PaymentMethod,
                                DeliveryStatus = item.DeliveryStatus,
                                DeliveryDate = item.DeliveryDate,
                                CreatedOn = item.CreatedOn,
                                Type = item.Type,
                                PaymentStatus = item.PaymentStatus,
                            });
                        }
                        response.Data = orderDetails;
                        response.Code = 200;
                        response.Message = "Invoice Is Generated successfully";
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<IEnumerable<InvoiceViewModel>> GetInvoiceDetailsList()
        {
            IEnumerable<InvoiceViewModel> InvoiceList = from a in Context.TblInvoices
                                                        join b in Context.TblVendorMasters on a.VandorId equals b.Vid
                                                        select new InvoiceViewModel
                                                        {
                                                            Id = a.Id,
                                                            InvoiceNo = a.InvoiceNo,
                                                            InvoiceType = a.InvoiceType,
                                                            InvoiceDate = a.InvoiceDate,
                                                            VendorName = b.VendorCompany,
                                                            VandorId = a.VandorId,
                                                            //ProductName = c.ProductName,
                                                            //ProductDetails = c.ProductShortDescription,
                                                            //HSN = c.Hsn,
                                                            //Price = c.PerUnitPrice,
                                                            //TotalGst = c.Gst,
                                                            DispatchThrough = a.DispatchThrough,
                                                            Destination = a.Destination,
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
                                                            //PerUnitPrice = c.PerUnitPrice
                                                        };
            return InvoiceList;
        }

        public async Task<IEnumerable<InvoiceViewModel>> GetInvoiceListByVendorId(Guid Vid)
        {

            try
            {
                IEnumerable<InvoiceViewModel> invoiceList = (from a in Context.TblInvoices.Where(x => x.VandorId == Vid)
                                                             join b in Context.TblVendorMasters
                                                             on a.VandorId equals b.Vid
                                                             select new InvoiceViewModel
                                                             {
                                                                 Id = a.Id,
                                                                 InvoiceNo = a.InvoiceNo,
                                                                 VendorName = b.VendorCompany,
                                                                 VandorId = a.VandorId,
                                                                 //ProductName = c.ProductName,
                                                                 //ProductDetails = c.ProductShortDescription,
                                                                 //HSN = c.Hsn,
                                                                 //Price = c.PerUnitPrice,
                                                                 //TotalGst=c.Gst,
                                                                 DispatchThrough = a.DispatchThrough,
                                                                 Destination = a.Destination,
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
                                                                 //PerUnitPrice=c.PerUnitPrice,
                                                                 //PaymentMethod = d.PaymentMethod,
                                                                 //PaymentStatus = d.PaymentStatus,
                                                                 //Quantity = d.Quantity,
                                                                 //TotalAmountWithQuantity = d.Total,

                                                             });
                return invoiceList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        public async Task<UserResponceModel> InsertInvoiceDetails(GenerateInvoiceModel InsertInvoice)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                bool isInvoiceAlredyExists = Context.TblInvoices.Any(x => x.OrderId == InsertInvoice.OrderId);
                if (isInvoiceAlredyExists == true)
                {
                    response.Message = "This Invoice Is already Generated";
                    response.Data = InsertInvoice;
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Icone = "warning";
                }
                else
                {
                    var invoicemodel = new TblInvoice()
                    {
                        Id = Guid.NewGuid(),
                        OrderId = InsertInvoice.OrderId,
                        InvoiceType = InsertInvoice.InvoiceType,
                        VandorId = InsertInvoice.VandorId,
                        InvoiceNo = InsertInvoice.InvoiceNo,
                        ProjectId = InsertInvoice.ProjectId,
                        InvoiceDate = DateTime.Now,
                        BuyesOrderDate = InsertInvoice.BuyesOrderDate,
                        BuyesOrderNo = InsertInvoice.BuyesOrderNo,
                        DispatchThrough = InsertInvoice.DispatchThrough,
                        Destination = InsertInvoice.Destination,
                        Cgst = InsertInvoice.Cgst,
                        Sgst = InsertInvoice.Sgst,
                        Igst = InsertInvoice.Igst,
                        TotalGst = InsertInvoice.TotalGst,
                        TotalAmount = InsertInvoice.TotalAmount,
                        CreatedOn = DateTime.Now,
                        CreatedBy = InsertInvoice.CreatedBy,
                    };
                    Context.TblInvoices.Add(invoicemodel);
                    await Context.SaveChangesAsync();
                    response.Code = 200;
                    response.Message = "Invoice Generated successfully!";
                }
            }
            catch (Exception ex)
            {
                response.Code = 500;
                response.Message = "Error creating Invoice: " + ex.Message;
            }
            return response;
        }
    }
}
