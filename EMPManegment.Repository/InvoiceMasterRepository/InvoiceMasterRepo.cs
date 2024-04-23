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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

                string INVOICEId;
                if (LastOrder == null)
                {
                    INVOICEId = $"BTPL/INVOICE/{projectname}/{lastYear % 100}-{currentYear % 100}-01";
                }
                else
                {
                    if (LastOrder.OrderId.Length >= 25)
                    {
                        int orderNumber = int.Parse(LastOrder.OrderId.Substring(24)) + 1;
                        INVOICEId = $"BTPL/INVOICE/{projectname}/{lastYear % 100}-{currentYear % 100}-" + orderNumber.ToString("D3");
                    }
                    else
                    {
                        throw new Exception("OrderId does not have the expected format.");
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

        public async Task<InvoiceViewModel> GetInvoiceDetailsById(Guid Id)
        {
            InvoiceViewModel invoice = new InvoiceViewModel();
            try
            {
                invoice = (from a in Context.TblInvoices.Where(x => x.Id == Id)
                           join b in Context.TblVendorMasters on a.VandorId equals b.Vid
                           select new InvoiceViewModel
                           {
                               Id = a.Id,
                               InvoiceNo = a.InvoiceNo,
                               VendorName = b.VendorCompany,
                               VandorId = a.VandorId,
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


                           }).First();
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

                    response.Message = "This Invoice Is already Generated";
                    response.Code = 400;
                }
                else
                {
                    var orderDetails = new List<PurchaseOrderDetailView>();
                    var data = await (from a in Context.TblPurchaseOrderMasters
                                      join c in Context.TblProductDetailsMasters on a.ProductId equals c.Id
                                      join d in Context.TblPaymentMethodTypes on a.PaymentMethod equals d.Id
                                      join b in Context.TblVendorMasters on a.VendorId equals b.Vid
                                      where a.OrderId == OrderId
                                      select new PurchaseOrderDetailView
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
                                          SubTotal = a.SubTotal,
                                          GstPerUnit = a.GstPerUnit,
                                          TotalAmount = a.TotalAmount,
                                          AmountPerUnit = a.AmountPerUnit,
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
                                ProductId = item.ProductId,
                                PaymentMethod = item.PaymentMethod,
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
                                SubTotal = item.SubTotal,
                                GstPerUnit = item.GstPerUnit,
                                TotalAmount = item.TotalAmount,
                                AmountPerUnit = item.AmountPerUnit,
                                PaymentMethodName = item.PaymentMethodName,
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

        public async Task<jsonData> GetInvoiceDetailsList(DataTableRequstModel dataTable)
        {
            try
            {
                var data = await (from a in Context.TblInvoices
                                  join b in Context.TblVendorMasters on a.VandorId equals b.Vid
                                  join c in Context.TblProjectMasters on a.ProjectId equals c.ProjectId
                                  where a.IsDeleted != true
                                  select new
                                  {
                                      Invoice = a,
                                      Vendor = b,
                                      Project = c,

                                  }).ToListAsync();

                var groupedInvoiceList = data.GroupBy(x => x.Invoice.InvoiceNo)
                                        .Select(group => group.First())
                                        .Select(item => new InvoiceViewModel
                                        {
                                            Id = item.Invoice.Id,
                                            InvoiceNo = item.Invoice.InvoiceNo,
                                            InvoiceType = item.Invoice.InvoiceType,
                                            InvoiceDate = item.Invoice.InvoiceDate,
                                            VendorName = item.Vendor.VendorCompany,
                                            VandorId = item.Invoice.VandorId,
                                            OrderId = item.Invoice.OrderId,
                                            ProjectId = item.Invoice.ProjectId,
                                            ProjectName = item.Project.ShortName,
                                            DispatchThrough = item.Invoice.DispatchThrough,
                                            Destination = item.Invoice.Destination,
                                            Cgst = item.Invoice.Cgst,
                                            Igst = item.Invoice.Igst,
                                            Sgst = item.Invoice.Sgst,
                                            BuyesOrderNo = item.Invoice.BuyesOrderNo,
                                            BuyesOrderDate = item.Invoice.BuyesOrderDate,
                                            TotalAmount = item.Invoice.TotalAmount,
                                            CreatedOn = item.Invoice.CreatedOn,
                                            CreatedBy = item.Invoice.CreatedBy,
                                            UpdatedOn = item.Invoice.UpdatedOn,
                                            UpdatedBy = item.Invoice.UpdatedBy,
                                        });


                var flattenedData = groupedInvoiceList.Skip(dataTable.skip)
                                                  .Take(dataTable.pageSize)
                                                  .ToList();

                int totalRecord = groupedInvoiceList.Count();

                jsonData jsonData = new jsonData
                {
                    draw = dataTable.draw,
                    recordsFiltered = totalRecord,
                    recordsTotal = totalRecord,
                    data = flattenedData
                };

                return jsonData;
            }
            catch (Exception ex)
            {
                return null;
            }
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
                                                                 OrderId = a.OrderId,
                                                                 InvoiceType = a.InvoiceType,
                                                                 VendorName = b.VendorCompany,
                                                                 VandorId = b.Vid,
                                                                 Date = a.InvoiceDate,
                                                                 TotalGst = a.TotalGst,
                                                                 TotalAmount = a.TotalAmount,
                                                                 InvoiceNo = a.InvoiceNo,
                                                             });
                return invoiceList;
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
                response.Message = "Details Inserted successfully!";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<IEnumerable<CreditDebitView>> GetAllTransactionByVendorId(Guid Vid)
        {
            try
            {
                IEnumerable<CreditDebitView> CreditList = (from a in Context.TblCreditDebitMasters
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

                return CreditList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                        PaymentMethod = InsertInvoice.PaymentMethod,
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
                        Status = "Pending",
                        IsDeleted = false,
                        CreatedOn = DateTime.Now,
                        CreatedBy = InsertInvoice.CreatedBy,
                    };
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

                    Context.TblInvoices.Add(invoicemodel);
                    Context.TblCreditDebitMasters.Add(craditdebit);
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

        public async Task<jsonData> GetAllTransaction(DataTableRequstModel dataTable)
        {
            var allCreditList = from a in Context.TblCreditDebitMasters
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
                                    VendorAddress = b.VendorAddress
                                };

            if (!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDir))
            {
                if (dataTable.sortColumnDir == "asc")
                {
                    allCreditList = allCreditList.OrderBy(e => EF.Property<object>(e, dataTable.sortColumn));
                }
                else
                {
                    allCreditList = allCreditList.OrderByDescending(e => EF.Property<object>(e, dataTable.sortColumn));
                }
            }

            if (!string.IsNullOrEmpty(dataTable.searchValue))
            {
                decimal searchValueDecimal;
                if (decimal.TryParse(dataTable.searchValue, out searchValueDecimal))
                {
                    allCreditList = allCreditList.Where(e =>
                        e.VendorName.Contains(dataTable.searchValue) ||
                        e.PaymentTypeName.Contains(dataTable.searchValue) ||
                        (e.CreditDebitAmount.HasValue && e.CreditDebitAmount.Value == searchValueDecimal) ||
                        (e.PendingAmount.HasValue && e.PendingAmount.Value == searchValueDecimal) ||
                        e.PaymentMethodName.Contains(dataTable.searchValue));
                }
                else
                {
                    allCreditList = allCreditList.Where(e =>
                        e.VendorName.Contains(dataTable.searchValue) ||
                        e.PaymentTypeName.Contains(dataTable.searchValue) ||
                        e.PaymentMethodName.Contains(dataTable.searchValue));
                }
            }


            int totalRecord = await allCreditList.CountAsync();

            var cData = await allCreditList.Skip(dataTable.skip).Take(dataTable.pageSize).ToListAsync();

            jsonData jsonData = new jsonData
            {
                draw = dataTable.draw,
                recordsFiltered = totalRecord,
                recordsTotal = totalRecord,
                data = cData
            };

            return jsonData;
        }

        public async Task<PurchaseOrderResponseModel> DisplayInvoiceDetails(string OrderId)
        {
            PurchaseOrderResponseModel response = new PurchaseOrderResponseModel();
            try
            {
                var orderDetails = new List<PurchaseOrderDetailView>();
                var data = await (from a in Context.TblPurchaseOrderMasters
                                  join c in Context.TblProductDetailsMasters on a.ProductId equals c.Id
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
                                      TotalAmount = a.TotalAmount,
                                      AmountPerUnit = a.AmountPerUnit,
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
                            TotalAmount = item.TotalAmount,
                            AmountPerUnit = item.AmountPerUnit,
                            PaymentMethod = item.PaymentMethod,
                            DeliveryStatus = item.DeliveryStatus,
                            DeliveryDate = item.DeliveryDate,
                            CreatedOn = item.CreatedOn,
                            Type = item.Type,
                            PaymentStatus = item.PaymentStatus,
                            PaymentMethodName = item.PaymentMethodName,
                        });
                    }
                    response.Data = orderDetails;
                    response.Code = 200;
                    response.Message = "Invoice Is Generated successfully";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<UserResponceModel> IsDeletedInvoice(string InvoiceNo)
        {
            UserResponceModel response = new UserResponceModel();
            var GetInvoicedata = Context.TblInvoices.Where(a => a.InvoiceNo == InvoiceNo).FirstOrDefault();

            if (GetInvoicedata != null)
            {
                GetInvoicedata.IsDeleted = true;
                Context.TblInvoices.Update(GetInvoicedata);
                Context.SaveChanges();
                response.Code = 200;
                response.Data = GetInvoicedata;
                response.Message = "Invoice is Deleted Successfully";
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

        public async Task<UserResponceModel> UpdateInvoiceDetails(UpdateInvoiceModel UpdateInvoice)
        {
            UserResponceModel model = new UserResponceModel();
            var invoicedetails = Context.TblInvoices.Where(e => e.Id == UpdateInvoice.Id).FirstOrDefault();
            try
            {
                if (invoicedetails != null)
                {
                    invoicedetails.Id = UpdateInvoice.Id;
                    invoicedetails.InvoiceDate = UpdateInvoice.InvoiceDate;
                    invoicedetails.TotalAmount = UpdateInvoice.TotalAmount;
                    invoicedetails.PaymentMethod = UpdateInvoice.PaymentMethod;
                    invoicedetails.Status = UpdateInvoice.Status;
                    invoicedetails.UpdatedOn = DateTime.Now;
                    invoicedetails.UpdatedBy = UpdateInvoice.UpdatedBy;
                }
                Context.TblInvoices.Update(invoicedetails);
                Context.SaveChanges();
                model.Code = 200;
                model.Message = "Invoice Details Updated Successfully!";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public async Task<PurchaseOrderResponseModel> ShowInvoiceDetailsByOrderId(string OrderId)
        {
            PurchaseOrderResponseModel response = new PurchaseOrderResponseModel();
            try
            {
                bool isInvoiceAlredyExists = Context.TblInvoices.Any(x => x.OrderId == OrderId);
                if (isInvoiceAlredyExists == false)
                {

                    response.Message = "This Invoice Is Not Generated";
                    response.Code = 400;
                }
                else
                {
                    var orderDetails = new List<PurchaseOrderDetailView>();
                    var data = await (from a in Context.TblPurchaseOrderMasters
                                      join c in Context.TblProductDetailsMasters on a.ProductId equals c.Id
                                      join b in Context.TblVendorMasters on a.VendorId equals b.Vid
                                      where a.OrderId == OrderId
                                      select new PurchaseOrderDetailView
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
                                          TotalAmount = a.TotalAmount,
                                          AmountPerUnit = a.AmountPerUnit,
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
                                TotalAmount = item.TotalAmount,
                                AmountPerUnit = item.AmountPerUnit,
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
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }
}
