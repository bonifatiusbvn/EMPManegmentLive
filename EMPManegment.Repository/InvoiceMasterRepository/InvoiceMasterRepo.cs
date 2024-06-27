using Azure;
using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
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
using Microsoft.Extensions.FileSystemGlobbing.Internal.PatternContexts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using static System.Runtime.InteropServices.JavaScript.JSType;
#nullable disable

namespace EMPManegment.Repository.InvoiceMasterRepository
{
    public class InvoiceMasterRepo : IInvoiceMaster
    {
        private readonly BonifatiusEmployeesContext Context;

        public InvoiceMasterRepo(BonifatiusEmployeesContext context)
        {
            Context = context;
        }


        public string CheckInvoiceNo(string projectname)
        {
            try
            {
                var LastOrder = Context.TblInvoices.OrderByDescending(e => e.CreatedOn).FirstOrDefault();
                var currentYear = DateTime.Now.Year;
                var lastYear = currentYear - 1;

                int startIndex = projectname.IndexOf('(');
                int endIndex = projectname.IndexOf(')');

                var Projectsubparts=   projectname.Substring(startIndex + 1, endIndex - startIndex - 1);

                string INVOICEId;
                if (LastOrder == null)
                {
                    INVOICEId = $"BTPL/INVOICE/{Projectsubparts}/{lastYear % 100}-{currentYear % 100}-01";
                }
                else
                {
                    string[] parts = LastOrder.InvoiceNo.Split('/');
                    if (parts.Length >= 4)
                    {
                        string lastPart = parts[parts.Length - 1];
                        string[] subparts = lastPart.Split('-');
                        if (subparts.Length == 3)
                        {
                            int orderNumber = int.Parse(subparts[2]) + 1;
                            INVOICEId = $"BTPL/INVOICE/{Projectsubparts}/{lastYear % 100}-{currentYear % 100}-" + orderNumber.ToString("D3");
                        }
                        else
                        {
                            throw new Exception("Last invoice number does not have the expected format.");
                        }
                    }
                    else
                    {
                        throw new Exception("Last invoice number does not have the expected format.");
                    }
                }
                return INVOICEId;
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
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CreditDebitView>> GetCreditDebitListByVendorId(Guid Vid)
        {
            try
            {
                var creditdebit = new List<CreditDebitView>();
                var data = await Context.TblCreditDebitMasters.Where(x => x.VendorId == Vid).ToListAsync();
                if (data != null)
                {
                    foreach (var a in data)
                    {
                        creditdebit.Add(new CreditDebitView()
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
                    }
                }
                return creditdebit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<InvoiceMasterModel> GetInvoiceDetailsById(Guid Id)
        {
            InvoiceMasterModel invoice = new InvoiceMasterModel();
            try
            {
                invoice = (from a in Context.TblInvoices.Where(x => x.Id == Id)
                           join b in Context.TblVendorMasters on a.VandorId equals b.Vid
                           join c in Context.TblCities on b.VendorCity equals c.Id
                           join d in Context.TblCountries on b.VendorCountry equals d.Id
                           join e in Context.TblStates on b.VendorState equals e.Id
                           join f in Context.TblProjectMasters on a.ProjectId equals f.ProjectId
                           select new InvoiceMasterModel
                           {
                               Id = a.Id,
                               InvoiceNo = a.InvoiceNo,
                               VendorName = b.VendorCompany,
                               VandorId = a.VandorId,
                               DispatchThrough = a.DispatchThrough,
                               Cgst = a.Cgst,
                               Igst = a.Igst,
                               Sgst = a.Sgst,
                               BuyesOrderNo = a.BuyesOrderNo,
                               BuyesOrderDate = a.BuyesOrderDate,
                               TotalAmount = a.TotalAmount,
                               VendorFullAddress = b.VendorAddress + "-" + c.City + "-" + e.State + "-" + d.Country,
                               VendorGstnumber = b.VendorGstnumber,
                               VendorEmail = b.VendorEmail,
                               ProjectName = f.ProjectTitle,
                               VendorCompanyName = b.VendorCompany,
                               VendorBankAccountNo = b.VendorBankAccountNo,
                               VendorBankIfsc = b.VendorBankIfsc,
                               VendorBankBranch = b.VendorBankBranch,
                               VendorAccountHolderName = b.VendorAccountHolderName,
                               VendorBankName = b.VendorBankName,
                               TotalGst = a.TotalGst,
                               RoundOff = a.RoundOff,
                               TotalDiscount= a.TotalDiscount,
                               InvoiceDate = a.InvoiceDate,
                           }).First();
                List<InvoiceDetailsViewModel> Itemlist = (from a in Context.TblInvoiceDetails.Where(a => a.InvoiceRefId == invoice.Id)
                                                          join b in Context.TblProductTypeMasters on a.ProductType equals b.Id
                                                          join c in Context.TblProductDetailsMasters on a.ProductId equals c.Id
                                                          select new InvoiceDetailsViewModel
                                                          {
                                                              Product = a.Product,
                                                              Quantity = a.Quantity,
                                                              ProductTypeName = b.Type,
                                                              ProductTotal = a.ProductTotal,
                                                              GstPer = a.GstPer,
                                                              GstAmount = a.GstAmount,
                                                              ProductType = a.ProductType,
                                                              Hsn = c.Hsn,
                                                              PerUnitPrice = c.PerUnitPrice,
                                                              DiscountAmount=a.DiscountAmount,
                                                              DiscountPer=a.DiscountPer,
                                                              Price = a.Price,
                                                          }).ToList();
                invoice.InvoiceDetails = Itemlist;
                return invoice;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PurchaseOrderResponseModel> GetInvoiceDetailsByOrderId(string OrderId)
        {
            PurchaseOrderResponseModel response = new PurchaseOrderResponseModel();
            try
            {
                bool isInvoiceAlredyExists = Context.TblInvoices.Any(x => x.OrderId == OrderId);
                if (isInvoiceAlredyExists == true)
                {
                    response.Message = "This invoice is already generated";
                    response.Code = 400;
                }
                else
                {
                    var orderDetails = new List<PurchaseOrderDetailView>();
                    var data = await (from a in Context.TblPurchaseOrderMasters
                                      join d in Context.TblPaymentMethodTypes on a.PaymentMethod equals d.Id
                                      join b in Context.TblVendorMasters on a.VendorId equals b.Vid
                                      where a.OrderId == OrderId
                                      select new PurchaseOrderDetailView
                                      {
                                          Id = a.Id,
                                          OrderId = a.OrderId,
                                          VendorId = a.VendorId,
                                          CompanyId=a.CompanyId,
                                          VendorAddress = b.VendorAddress,
                                          VendorContact = b.VendorContact,
                                          VendorEmail = b.VendorCompanyEmail,
                                          OrderDate = a.OrderDate,
                                          SubTotal = a.SubTotal,
                                          TotalAmount = a.TotalAmount,
                                          PaymentMethod = a.PaymentMethod,
                                          PaymentMethodName = d.PaymentMethod,
                                          PaymentStatus = a.PaymentStatus,
                                          DeliveryStatus = a.DeliveryStatus,
                                          DeliveryDate = a.DeliveryDate,
                                          CreatedOn = a.CreatedOn,
                                      }).ToListAsync();
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            orderDetails.Add(new PurchaseOrderDetailView()
                            {
                                Id = item.Id,
                                OrderId = item.OrderId,
                                CompanyName = item.CompanyName,
                                VendorId = item.VendorId,
                                PaymentMethod = item.PaymentMethod,
                                VendorEmail = item.VendorEmail,
                                VendorContact = item.VendorContact,
                                VendorAddress = item.VendorAddress,
                                OrderDate = item.OrderDate,
                                SubTotal = item.SubTotal,
                                TotalAmount = item.TotalAmount,
                                PaymentMethodName = item.PaymentMethodName,
                                DeliveryStatus = item.DeliveryStatus,
                                DeliveryDate = item.DeliveryDate,
                                CreatedOn = item.CreatedOn,
                                PaymentStatus = item.PaymentStatus,
                            });
                        }
                        response.Data = orderDetails;
                        response.Code = 200;
                        response.Message = "Invoice is generated successfully";
                    }

                }
            }
            catch (Exception ex)
            {
                response.Code = 400;
                response.Message = "Error in generating invoice.";
            }
            return response;
        }

        public async Task<jsonData> GetInvoiceDetailsList(DataTableRequstModel dataTable)
        {
            try
            {
                var invoicelist = from a in Context.TblInvoices
                                  join b in Context.TblVendorMasters on a.VandorId equals b.Vid
                                  join c in Context.TblProjectMasters on a.ProjectId equals c.ProjectId
                                  where a.IsDeleted != true
                                  orderby a.CreatedOn descending
                                  select new InvoiceViewModel
                                  {
                                      Id = a.Id,
                                      InvoiceNo = a.InvoiceNo,
                                      InvoiceType = a.InvoiceType,
                                      InvoiceDate = a.InvoiceDate,
                                      VendorName = b.VendorCompany,
                                      VandorId = a.VandorId,
                                      OrderId = a.OrderId,
                                      ProjectId = a.ProjectId,
                                      ProjectName = c.ShortName,
                                      DispatchThrough = a.DispatchThrough,
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
                                      Date = a.Date ?? DateTime.MinValue,
                                  };
                invoicelist = invoicelist.OrderByDescending(pr => pr.Date);

                if (!string.IsNullOrEmpty(dataTable.searchValue))
                {
                    invoicelist = invoicelist.Where(e =>
                        e.VendorName.Contains(dataTable.searchValue) ||
                        e.InvoiceNo.Contains(dataTable.searchValue) ||
                        e.ProjectName.Contains(dataTable.searchValue) ||
                        e.TotalAmount.ToString().Contains(dataTable.searchValue)
                    );
                }

                if (!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDir))
                {
                    invoicelist = dataTable.sortColumnDir == "asc"
                        ? invoicelist.OrderBy(e => EF.Property<object>(e, dataTable.sortColumn))
                        : invoicelist.OrderByDescending(e => EF.Property<object>(e, dataTable.sortColumn));
                }

                var filteredData = await invoicelist
                .ToListAsync();

                var totalRecord = filteredData.Count;


                var jsonData = new jsonData
                {
                    draw = dataTable.draw,
                    recordsFiltered = totalRecord,
                    recordsTotal = totalRecord,
                    data = filteredData
                };

                return jsonData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new jsonData { };
            }
        }


        public async Task<InvoicePayVendorModel> GetInvoiceListByVendorId(Guid Vid)
        {
            try
            {
                InvoicePayVendorModel invoiceList = new InvoicePayVendorModel();

                var invoices = await (from a in Context.TblInvoices
                                      join b in Context.TblVendorMasters on a.VandorId equals b.Vid
                                      where a.VandorId == Vid && a.PaymentStatus == 7
                                      select new InvoiceViewModel
                                      {
                                          Id = a.Id,
                                          OrderId = a.OrderId,
                                          InvoiceType = a.InvoiceType,
                                          VendorName = b.VendorCompany,
                                          VandorId = b.Vid,
                                          Date = a.InvoiceDate,
                                          TotalGst = a.TotalGst,
                                          TotalAmount = a.TotalAmount,
                                          InvoiceNo = a.InvoiceNo,
                                      }).ToListAsync();
                if (invoices.Count > 0)
                {
                    invoiceList.InvoicePaymentTransaction = invoices;
                    return invoiceList;
                }
                else
                {
                    invoiceList.VendorId = Vid;
                    return invoiceList;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<CreditDebitView>> GetLastTransactionByVendorId(Guid Vid)
        {
            try
            {
                IEnumerable<CreditDebitView> invoiceList = (from a in Context.TblCreditDebitMasters
                                                            join b in Context.TblVendorMasters on a.VendorId equals b.Vid
                                                            join c in Context.TblPaymentTypes on a.PaymentType equals c.Id
                                                            join d in Context.TblPaymentMethodTypes on a.PaymentMethod equals d.Id
                                                            where a.VendorId == Vid
                                                            orderby a.Date descending
                                                            select new CreditDebitView
                                                            {
                                                                Id = a.Id,
                                                                VendorName = b.VendorCompany,
                                                                Date = a.Date,
                                                                PaymentTypeName = c.Type,
                                                                PaymentMethodName = d.PaymentMethod,
                                                                PendingAmount = a.PendingAmount,
                                                                CreditDebitAmount = a.CreditDebitAmount,
                                                                TotalAmount = a.TotalAmount,
                                                            }).Take(10);

                return invoiceList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserResponceModel> InsertCreditDebitDetails(CreditDebitView CreditDebit)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var insertcraditdebit = new TblCreditDebitMaster()
                {
                    VendorId = CreditDebit.VendorId,
                    Type = CreditDebit.Type,
                    InvoiceNo = CreditDebit.InvoiceNo,
                    Date = DateTime.Now,
                    PaymentType = CreditDebit.PaymentType,
                    CreditDebitAmount = CreditDebit.CreditDebitAmount,
                    PendingAmount = CreditDebit.PendingAmount,
                    TotalAmount = CreditDebit.TotalAmount,
                    PaymentMethod = CreditDebit.PaymentMethod,
                    CreatedOn = DateTime.Now,
                    CreatedBy = CreditDebit.CreatedBy,
                };

                Context.TblCreditDebitMasters.Add(insertcraditdebit);
                await Context.SaveChangesAsync();
                response.Code = 200;
                response.Message = "Details inserted successfully!";
            }
            catch (Exception ex)
            {
                response.Code = 400;
                response.Message = "Error in inserting credit debit details.";
            }
            return response;
        }

        public async Task<UserResponceModel> InsertInvoiceDetails(InvoiceMasterModel InsertInvoice)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var invoice = new TblInvoice()
                {
                    Id = Guid.NewGuid(),
                    InvoiceType = InsertInvoice.InvoiceType,
                    VandorId = InsertInvoice.VandorId,
                    InvoiceNo = InsertInvoice.InvoiceNo,
                    ProjectId = InsertInvoice.ProjectId,
                    OrderId = InsertInvoice.OrderId,
                    InvoiceDate = InsertInvoice.InvoiceDate,
                    BuyesOrderNo = InsertInvoice.BuyesOrderNo,
                    BuyesOrderDate = InsertInvoice.BuyesOrderDate,
                    DispatchThrough = InsertInvoice.DispatchThrough,
                    ShippingAddress = InsertInvoice.ShippingAddress,
                    Cgst = InsertInvoice.Cgst,
                    Sgst = InsertInvoice.Sgst,
                    Igst = InsertInvoice.Igst,
                    TotalDiscount = InsertInvoice.TotalDiscount,
                    TotalGst = InsertInvoice.TotalGst,
                    RoundOff= InsertInvoice.RoundOff,
                    TotalAmount = InsertInvoice.TotalAmount,
                    PaymentMethod = InsertInvoice.PaymentMethod,
                    Status = InsertInvoice.Status,
                    PaymentStatus = InsertInvoice.PaymentStatus,
                    CompanyId = InsertInvoice.CompanyId,
                    IsDeleted = false,
                    CreatedBy = InsertInvoice.CreatedBy,
                    CreatedOn = DateTime.Now,
                    Date = DateTime.Now,
                };
                Context.TblInvoices.Add(invoice);

                foreach (var item in InsertInvoice.InvoiceDetails)
                {
                    var InvoiceDetails = new TblInvoiceDetail()
                    {
                        InvoiceRefId = invoice.Id,
                        ProductId = item.ProductId,
                        Product = item.Product,
                        ProductType = item.ProductType,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        DiscountPer = item.DiscountPer,
                        DiscountAmount= item.DiscountAmount,
                        GstAmount = item.GstAmount,
                        GstPer=item.GstPer,
                        ProductTotal = item.ProductTotal,
                        Hsn = item.Hsn,
                        IsDeleted = false,
                        CreatedBy = InsertInvoice.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    Context.TblInvoiceDetails.Add(InvoiceDetails);
                }
                if(InsertInvoice.PaymentStatus == 7)
                {
                    var craditdebit = new TblCreditDebitMaster()
                    {
                        VendorId = InsertInvoice.VandorId,
                        Type = InsertInvoice.InvoiceType,
                        InvoiceNo = InsertInvoice.InvoiceNo,
                        Date = DateTime.Now,
                        PaymentType = InsertInvoice.PaymentType,
                        CreditDebitAmount = InsertInvoice.CreditDebitAmount,
                        PendingAmount = InsertInvoice.PendingAmount,
                        TotalAmount = InsertInvoice.TotalAmount,
                        CreatedOn = DateTime.Now,
                        CreatedBy = InsertInvoice.CreatedBy,
                    };
                    Context.TblCreditDebitMasters.Add(craditdebit);
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

        public async Task<List<CreditDebitView>> GetAllTransaction()
        {
            try
            {
                var allCreditList = await (from a in Context.TblCreditDebitMasters
                                           join b in Context.TblVendorMasters on a.VendorId equals b.Vid
                                           join c in Context.TblPaymentTypes on a.PaymentType equals c.Id
                                           join d in Context.TblPaymentMethodTypes on a.PaymentMethod equals d.Id
                                           orderby a.Date descending
                                           select new CreditDebitView
                                           {
                                               Id = a.Id,
                                               VendorName = b.VendorCompany,
                                               Date = a.Date,
                                               PaymentTypeName = c.Type,
                                               PaymentMethodName = d.PaymentMethod,
                                               PendingAmount = a.PendingAmount,
                                               CreditDebitAmount = a.CreditDebitAmount,
                                               TotalAmount = a.TotalAmount,
                                               VendorAddress = b.VendorAddress,
                                               VendorId = b.Vid,
                                           }).ToListAsync();

                return allCreditList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PurchaseOrderResponseModel> DisplayInvoiceDetails(string OrderId)
        {
            PurchaseOrderResponseModel response = new PurchaseOrderResponseModel();
            try
            {
                var orderDetails = new List<PurchaseOrderDetailView>();
                var data = await (from a in Context.TblPurchaseOrderMasters
                                  join b in Context.TblVendorMasters on a.VendorId equals b.Vid
                                  join d in Context.TblInvoices on a.OrderId equals d.OrderId
                                  join e in Context.TblPaymentMethodTypes on a.PaymentMethod equals e.Id
                                  where a.OrderId == OrderId
                                  select new PurchaseOrderDetailView
                                  {
                                      Id = a.Id,
                                      OrderId = a.OrderId,
                                      InvoiceNo = d.InvoiceNo,
                                      VendorId = a.VendorId,
                                      CompanyId = a.CompanyId,
                                      VendorAddress = b.VendorAddress,
                                      VendorContact = b.VendorContact,
                                      VendorEmail = b.VendorCompanyEmail,
                                      OrderDate = a.OrderDate,
                                      TotalAmount = a.TotalAmount,
                                      PaymentMethod = a.PaymentMethod,
                                      PaymentStatus = a.PaymentStatus,
                                      DeliveryStatus = a.DeliveryStatus,
                                      DeliveryDate = a.DeliveryDate,
                                      PaymentMethodName = e.PaymentMethod,
                                      CreatedOn = a.CreatedOn,
                                  }).ToListAsync();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        orderDetails.Add(new PurchaseOrderDetailView()
                        {
                            Id = item.Id,
                            OrderId = item.OrderId,
                            InvoiceNo = item.InvoiceNo,
                            CompanyName = item.CompanyName,
                            VendorId = item.VendorId,
                            VendorEmail = item.VendorEmail,
                            VendorContact = item.VendorContact,
                            VendorAddress = item.VendorAddress,
                            OrderDate = item.OrderDate,
                            TotalAmount = item.TotalAmount,
                            PaymentMethod = item.PaymentMethod,
                            DeliveryStatus = item.DeliveryStatus,
                            DeliveryDate = item.DeliveryDate,
                            CreatedOn = item.CreatedOn,
                            PaymentStatus = item.PaymentStatus,
                            PaymentMethodName = item.PaymentMethodName,
                        });
                    }
                    response.Data = orderDetails;
                    response.Code = 200;
                    response.Message = "Invoice is generated successfully";
                }
            }
            catch (Exception ex)
            {
                response.Code = 400;
                response.Message = "Error in generating invoice";
            }
            return response;
        }

        public async Task<UserResponceModel> IsDeletedInvoice(Guid InvoiceId)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var GetInvoicedata = Context.TblInvoices.Where(a => a.Id == InvoiceId).FirstOrDefault();
                var GetInvoiceDetails = Context.TblInvoiceDetails.Where(a => a.InvoiceRefId == InvoiceId).ToList();

                GetInvoicedata.IsDeleted = true;
                Context.TblInvoices.Update(GetInvoicedata);

                if (GetInvoiceDetails.Any())
                {
                    foreach (var invoice in GetInvoiceDetails)
                    {
                        invoice.IsDeleted = true;
                        Context.TblInvoiceDetails.Update(invoice);
                    }

                    Context.SaveChanges();

                    response.Code = 200;
                    response.Message = "Invoice details are successfully deleted.";
                }
                else
                {
                    response.Code = 404;
                    response.Message = "No related records found to delete";
                }
            }
            catch (Exception ex)
            {
                response.Code = 400;
                response.Message = "Error in deleting invoice";
            }
            return response;
        }

        public async Task<UpdateInvoiceModel> EditInvoiceDetails(string InvoiceNo)
        {
            UpdateInvoiceModel invoice = new UpdateInvoiceModel();
            try
            {
                invoice = (from a in Context.TblInvoices.Where(x => x.InvoiceNo == InvoiceNo)
                           join b in Context.TblVendorMasters
                           on a.VandorId equals b.Vid
                           select new UpdateInvoiceModel
                           {
                               Id = a.Id,
                               InvoiceNo = a.InvoiceNo,
                               CompanyName = b.VendorCompany,
                               TotalAmount = a.TotalAmount,
                               PaymentMethod = a.PaymentMethod,
                               Status = a.Status,
                               InvoiceDate = a.InvoiceDate,

                           }).First();
                return invoice;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserResponceModel> UpdateInvoiceDetails(InvoiceMasterModel UpdateInvoice)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var invoice = new TblInvoice()
                {
                    Id = UpdateInvoice.Id,
                    InvoiceType = UpdateInvoice.InvoiceType,
                    VandorId = UpdateInvoice.VandorId,
                    InvoiceNo = UpdateInvoice.InvoiceNo,
                    ProjectId = UpdateInvoice.ProjectId,
                    OrderId = UpdateInvoice.OrderId,
                    InvoiceDate = UpdateInvoice.InvoiceDate,
                    BuyesOrderNo = UpdateInvoice.BuyesOrderNo,
                    BuyesOrderDate = UpdateInvoice.BuyesOrderDate,
                    DispatchThrough = UpdateInvoice.DispatchThrough,
                    ShippingAddress = UpdateInvoice.ShippingAddress,
                    Cgst = UpdateInvoice.Cgst,
                    Sgst = UpdateInvoice.Sgst,
                    Igst = UpdateInvoice.Igst,
                    TotalDiscount = UpdateInvoice.TotalDiscount,
                    TotalGst = UpdateInvoice.TotalGst,
                    RoundOff=UpdateInvoice.RoundOff,
                    TotalAmount = UpdateInvoice.TotalAmount,
                    PaymentMethod = UpdateInvoice.PaymentMethod,
                    Status = UpdateInvoice.Status,
                    PaymentStatus = UpdateInvoice.PaymentStatus,
                    CompanyId= UpdateInvoice.CompanyId,
                    CreatedOn=UpdateInvoice.CreatedOn,
                    UpdatedOn=DateTime.Now,
                    UpdatedBy=UpdateInvoice.UpdatedBy,
                    CreatedBy=UpdateInvoice.CreatedBy,
                    Date = DateTime.Now,
                    IsDeleted = false,
                };
                Context.TblInvoices.Update(invoice);

                foreach(var item in UpdateInvoice.InvoiceDetails)
                {
                    var existingInvoice = Context.TblInvoiceDetails.FirstOrDefault(e => e.InvoiceRefId == invoice.Id && e.ProductId == item.ProductId);

                    if (existingInvoice != null)
                    {
                        existingInvoice.InvoiceRefId = invoice.Id;
                        existingInvoice.ProductId = item.ProductId;
                        existingInvoice.Product = item.Product;
                        existingInvoice.ProductType = item.ProductType;
                        existingInvoice.Quantity = item.Quantity;
                        existingInvoice.Price = item.Price;
                        existingInvoice.DiscountPer = item.DiscountPer;
                        existingInvoice.DiscountAmount=item.DiscountAmount;
                        existingInvoice.GstPer = item.GstPer;
                        existingInvoice.GstAmount = item.GstAmount;
                        existingInvoice.ProductTotal = item.ProductTotal;
                        existingInvoice.UpdatedOn = DateTime.Now;
                        existingInvoice.UpdatedBy = UpdateInvoice.UpdatedBy;
                        existingInvoice.CreatedBy = UpdateInvoice.CreatedBy;
                        existingInvoice.Hsn = item.Hsn;
                        existingInvoice.IsDeleted = false;

                        Context.TblInvoiceDetails.Update(existingInvoice);
                    }
                    else
                    {
                        var InvoiceDetails = new TblInvoiceDetail()
                        {
                            InvoiceRefId = invoice.Id,
                            ProductId = item.ProductId,
                            Product = item.Product,
                            ProductType = item.ProductType,
                            Quantity = item.Quantity,
                            Price = item.Price,
                            DiscountPer = item.DiscountPer,
                            DiscountAmount = item.DiscountAmount,
                            GstAmount = item.GstAmount,
                            GstPer = item.GstPer,
                            ProductTotal = item.ProductTotal,
                            Hsn = item.Hsn,
                            IsDeleted = false,
                            CreatedBy = invoice.CreatedBy,
                            CreatedOn = DateTime.Now,
                        };
                        Context.TblInvoiceDetails.Add(InvoiceDetails);
                    }
                }
                var deletedInvoice = UpdateInvoice.InvoiceDetails.Select(a => a.ProductId).ToList();

                var InvoiceToRemove = Context.TblInvoiceDetails
                    .Where(e => e.InvoiceRefId == UpdateInvoice.Id && !deletedInvoice.Contains(e.ProductId))
                    .ToList();

                Context.TblInvoiceDetails.RemoveRange(InvoiceToRemove);
                await Context.SaveChangesAsync();
                response.Code = (int)HttpStatusCode.OK;
                response.Message = "Invoice Update successfully.";
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        public async Task<PurchaseOrderResponseModel> ShowInvoiceDetailsByOrderId(string OrderId)
        {
            PurchaseOrderResponseModel response = new PurchaseOrderResponseModel();
            try
            {
                bool isInvoiceAlredyExists = Context.TblInvoices.Any(x => x.OrderId == OrderId);
                if (isInvoiceAlredyExists == false)
                {

                    response.Message = "This invoice is not generated";
                    response.Code = 400;
                }
                else
                {
                    var orderDetails = new List<PurchaseOrderDetailView>();
                    var data = await (from a in Context.TblPurchaseOrderMasters
                                      join b in Context.TblVendorMasters on a.VendorId equals b.Vid
                                      where a.OrderId == OrderId
                                      select new PurchaseOrderDetailView
                                      {
                                          Id = a.Id,
                                          OrderId = a.OrderId,
                                          VendorId = a.VendorId,
                                          CompanyId = a.CompanyId,
                                          VendorAddress = b.VendorAddress,
                                          VendorContact = b.VendorContact,
                                          VendorEmail = b.VendorCompanyEmail,
                                          OrderDate = a.OrderDate,
                                          TotalAmount = a.TotalAmount,
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
                            orderDetails.Add(new PurchaseOrderDetailView()
                            {
                                Id = item.Id,
                                OrderId = item.OrderId,
                                CompanyName = item.CompanyName,
                                VendorId = item.VendorId,
                                VendorEmail = item.VendorEmail,
                                VendorContact = item.VendorContact,
                                VendorAddress = item.VendorAddress,
                                OrderDate = item.OrderDate,
                                TotalAmount = item.TotalAmount,
                                PaymentMethod = item.PaymentMethod,
                                DeliveryStatus = item.DeliveryStatus,
                                DeliveryDate = item.DeliveryDate,
                                CreatedOn = item.CreatedOn,
                                PaymentStatus = item.PaymentStatus,
                            });
                        }
                        response.Data = orderDetails;
                        response.Code = 200;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error in generating invoice.";
                response.Code = 400;
            }
            return response;
        }

        public async Task<IEnumerable<InvoiceViewModel>> InvoicActivity(Guid ProId)
        {
            try
            {
                var invoices = (from a in Context.TblInvoices
                                join b in Context.TblVendorMasters on a.VandorId equals b.Vid
                                join u in Context.TblUsers on a.CreatedBy equals u.Id
                                where a.ProjectId == ProId
                                orderby a.UpdatedOn ascending
                                select new InvoiceViewModel
                                {
                                    Id = a.Id,
                                    InvoiceNo = a.InvoiceNo,
                                    VendorName = b.VendorCompany,
                                    VandorId = a.VandorId,
                                    UserName = u.UserName,
                                    FirstName = u.FirstName,
                                    LastName = u.LastName,
                                    UserImage = u.Image,
                                    Status = a.Status,
                                    ShippingAddress = a.ShippingAddress,
                                    TotalAmount = a.TotalAmount,
                                    CreatedOn = a.CreatedOn,
                                    CreatedBy = a.CreatedBy,
                                    UpdatedOn = a.UpdatedOn,
                                    UpdatedBy = a.UpdatedBy
                                }).Take(3);

                return await invoices.ToListAsync();
            }

            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<jsonData> GetAllTransactionByVendorId(Guid Vid, DataTableRequstModel dataTable)
        {
            try
            {
                var CreditList = (from a in Context.TblCreditDebitMasters
                                  join b in Context.TblVendorMasters on a.VendorId equals b.Vid
                                  join c in Context.TblPaymentTypes on a.PaymentType equals c.Id
                                  join d in Context.TblPaymentMethodTypes on a.PaymentMethod equals d.Id
                                  where a.VendorId == Vid
                                  orderby a.Date descending
                                  select new CreditDebitView
                                  {
                                      Id = a.Id,
                                      VendorName = b.VendorCompany,
                                      Date = a.Date,
                                      PaymentTypeName = c.Type,
                                      PaymentMethodName = d.PaymentMethod,
                                      PendingAmount = a.PendingAmount,
                                      CreditDebitAmount = a.CreditDebitAmount,
                                      TotalAmount = a.TotalAmount,
                                  });


                if (!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDir))
                {
                    if (dataTable.sortColumnDir == "asc")
                    {
                        CreditList = CreditList.OrderBy(e => EF.Property<object>(e, dataTable.sortColumn));
                    }
                    else
                    {
                        CreditList = CreditList.OrderByDescending(e => EF.Property<object>(e, dataTable.sortColumn));
                    }
                }
                if (!string.IsNullOrEmpty(dataTable.searchValue))
                {
                    string searchValue = dataTable.searchValue.ToLower();
                    DateTime searchDate;
                    bool isDate = DateTime.TryParseExact(dataTable.searchValue, "dd MMM yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out searchDate);

                    CreditList = CreditList.Where(e => e.VendorName.ToLower().Contains(searchValue) ||
                                                 (isDate && e.Date == searchDate) ||
                                                 e.CreditDebitAmount.ToString().ToLower().Contains(searchValue) ||
                                                 e.TotalAmount.ToString().ToLower().Contains(searchValue));
                }

                int totalRecord = await CreditList.CountAsync();

                var cData = await CreditList.Skip(dataTable.skip).Take(dataTable.pageSize).ToListAsync();

                jsonData jsonData = new jsonData
                {
                    draw = dataTable.draw,
                    recordsFiltered = totalRecord,
                    recordsTotal = totalRecord,
                    data = cData
                };
                return jsonData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<InvoiceMasterModel> DisplayInvoiceDetailsById(Guid Id)
        {
            InvoiceMasterModel InvoiceList = new InvoiceMasterModel();
            try
            {
                InvoiceList = (from a in Context.TblInvoices.Where(x => x.Id == Id)
                               join c in Context.TblCompanyMasters on a.CompanyId equals c.Id
                               join d in Context.TblVendorMasters on a.VandorId equals d.Vid
                               join f in Context.TblPaymentMethodTypes on a.PaymentMethod equals f.Id
                               join g in Context.TblPaymentTypes on a.PaymentStatus equals g.Id
                               select new InvoiceMasterModel
                               {
                                   Id = Id,
                                   InvoiceType = a.InvoiceType,
                                   VandorId = a.VandorId,
                                   InvoiceNo = a.InvoiceNo,
                                   ProjectId = a.ProjectId,
                                   OrderId = a.OrderId,
                                   InvoiceDate = a.InvoiceDate,
                                   BuyesOrderNo = a.BuyesOrderNo,
                                   BuyesOrderDate = a.BuyesOrderDate,
                                   DispatchThrough = a.DispatchThrough,
                                   ShippingAddress = a.ShippingAddress,
                                   Cgst = a.Cgst,
                                   Sgst = a.Sgst,
                                   Igst = a.Igst,
                                   TotalGst = a.TotalGst,
                                   TotalAmount = a.TotalAmount,
                                   PaymentMethod = a.PaymentMethod,
                                   PaymentStatus = a.PaymentStatus,
                                   PaymentMethodName = f.PaymentMethod,
                                   PaymentStatusName = g.Type,
                                   Status = a.Status,
                                   CompanyId=c.Id,
                                   CompnyName=c.CompnyName,
                                   CompanyGst=c.Gst,
                                   VendorCompanyName = d.VendorCompany,
                                   VendorCompanyNumber = d.VendorCompanyNumber,
                                   VendorGstnumber = d.VendorGstnumber,
                                   VendorAddress = d.VendorAddress,
                                   CreatedOn = a.CreatedOn,
                                   RoundOff= a.RoundOff,
                                   TotalDiscount=a.TotalDiscount,
                               }).FirstOrDefault();
                List<InvoiceDetailsViewModel> Productlist = (from a in Context.TblInvoiceDetails.Where(a => a.InvoiceRefId == InvoiceList.Id)
                                                             join b in Context.TblProductTypeMasters on a.ProductType equals b.Id
                                                             join c in Context.TblProductDetailsMasters on a.ProductId equals c.Id
                                                             select new InvoiceDetailsViewModel
                                                             {
                                                                 ProductId = a.ProductId,
                                                                 Product = c.ProductName,
                                                                 Hsn = a.Hsn,
                                                                 Quantity = a.Quantity,
                                                                 ProductType = a.ProductType,
                                                                 ProductTypeName = b.Type,
                                                                 PerUnitPrice = a.Price,
                                                                 GstAmount = a.GstAmount,
                                                                 GstPer=a.GstPer,
                                                                 ProductTotal = a.ProductTotal,
                                                                 DiscountPer=a.DiscountPer,
                                                                 DiscountAmount=a.DiscountAmount,
                                                             }).ToList();

                InvoiceList.InvoiceDetails = Productlist;
                return InvoiceList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<InvoiceDetailsViewModel>> GetProductDetailsById(Guid ProductId)
        {
            try
            {
                var productDetails = new List<InvoiceDetailsViewModel>();
                var data = await(from a in Context.TblProductDetailsMasters.Where(x => x.Id == ProductId)
                                 join b in Context.TblProductTypeMasters on a.ProductType equals b.Id
                                 select new InvoiceDetailsViewModel
                                 {
                                     ProductId = a.Id,
                                     ProductType = b.Id,
                                     ProductDescription = a.ProductDescription,
                                     Product = a.ProductName,
                                     Hsn = a.Hsn,
                                     PerUnitPrice = a.PerUnitPrice,
                                     GstPer = (decimal)a.GstPercentage,
                                     GstAmount = a.GstAmount,
                                     ProductTypeName = b.Type,
                                 }).ToListAsync();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        productDetails.Add(new InvoiceDetailsViewModel()
                        {
                            Id = item.Id,
                            ProductType = item.ProductType,
                            ProductDescription = item.ProductDescription,
                            Product = item.Product,
                            ProductId = item.ProductId,
                            Hsn = item.Hsn,
                            PerUnitPrice = item.PerUnitPrice,
                            GstAmount = item.GstAmount,
                            GstPer = item.GstPer,
                            ProductTypeName = item.ProductTypeName,
                        });
                    }
                }
                return productDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<InvoiceViewModel>> InvoicActivityByUserId(Guid UserId)
        {
            try
            {
                var invoices = (from a in Context.TblInvoices
                                join b in Context.TblVendorMasters on a.VandorId equals b.Vid
                                join u in Context.TblUsers on a.CreatedBy equals u.Id
                                where a.CreatedBy == UserId
                                orderby a.UpdatedOn ascending
                                select new InvoiceViewModel
                                {
                                    Id = a.Id,
                                    InvoiceNo = a.InvoiceNo,
                                    VendorName = b.VendorCompany,
                                    VandorId = a.VandorId,
                                    UserName = u.UserName,
                                    FirstName = u.FirstName,
                                    LastName = u.LastName,
                                    UserImage = u.Image,
                                    Status = a.Status,
                                    ShippingAddress = a.ShippingAddress,
                                    TotalAmount = a.TotalAmount,
                                    CreatedOn = a.CreatedOn,
                                    CreatedBy = a.CreatedBy,
                                    UpdatedOn = a.UpdatedOn,
                                    UpdatedBy = a.UpdatedBy
                                }).Take(3);
                return await invoices.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
