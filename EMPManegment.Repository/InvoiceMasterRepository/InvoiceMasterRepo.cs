using Azure;
using EMPManagment.API;
using EMPManegment.EntityModels.Common;
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
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
using EMPManegment.EntityModels.ViewModels.ManualInvoice;
using EMPManegment.EntityModels.ViewModels.PurchaseOrderModels;
using EMPManegment.EntityModels.ViewModels.UserModels;
using X.PagedList;
#nullable disable

namespace EMPManegment.Repository.InvoiceMasterRepository
{
    public class InvoiceMasterRepo : IInvoiceMaster
    {
        private readonly BonifatiusEmployeesContext Context;

        public IConfiguration Configuration { get; }

        public InvoiceMasterRepo(BonifatiusEmployeesContext context, IConfiguration configuration)
        {
            Context = context;
            Configuration = configuration;
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

                var Projectsubparts = projectname.Substring(startIndex + 1, endIndex - startIndex - 1);

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
        public async Task<PurchaseOrderResponseModel> GetInvoiceDetailsByOrderId(string OrderId)
        {
            PurchaseOrderResponseModel response = new PurchaseOrderResponseModel();
            try
            {
                bool isInvoiceAlredyExists = Context.TblInvoices.Any(x => x.OrderId == OrderId);
                if (isInvoiceAlredyExists == true)
                {
                    response.Message = "This invoice is already generated";
                    response.Code = (int)HttpStatusCode.NotFound;
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
                                          CompanyId = a.CompanyId,
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

                        response.Message = "Invoice is generated successfully";
                    }

                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in generating invoice.";
            }
            return response;
        }

        public async Task<jsonData> GetInvoiceDetailsList(DataTableRequstModel dataTable)
        {
            try
            {
                string dbConnectionStr = Configuration.GetConnectionString("EMPDbconn");
                var dataSet = DbHelper.GetDataSet("[spGetInvoiceDetailsList]", System.Data.CommandType.StoredProcedure, new SqlParameter[] { }, dbConnectionStr);

                var invoiceList = new List<InvoiceViewModel>();

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var invoice = new InvoiceViewModel
                    {
                        Id = Guid.Parse(row["Id"]?.ToString() ?? Guid.Empty.ToString()),
                        InvoiceNo = row["InvoiceNo"]?.ToString() ?? string.Empty,
                        InvoiceDate = row["InvoiceDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["InvoiceDate"]),
                        VendorName = row["VendorName"]?.ToString() ?? string.Empty,
                        VandorId = Guid.Parse(row["VandorId"]?.ToString() ?? Guid.Empty.ToString()),
                        ProjectId = Guid.Parse(row["ProjectId"]?.ToString() ?? Guid.Empty.ToString()),
                        ProjectName = row["ProjectName"]?.ToString() ?? string.Empty,
                        DispatchThrough = row["DispatchThrough"]?.ToString() ?? string.Empty,
                        BuyesOrderNo = row["BuyesOrderNo"]?.ToString() ?? string.Empty,
                        BuyesOrderDate = row["BuyesOrderDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["BuyesOrderDate"]),
                        TotalAmount = row["TotalAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(row["TotalAmount"]),
                        CreatedOn = row["CreatedOn"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreatedOn"]),
                        CreatedBy = Guid.Parse(row["CreatedBy"]?.ToString() ?? Guid.Empty.ToString()),
                        Date = row["Date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["Date"]),
                    };
                    invoiceList.Add(invoice);
                }

                if (!string.IsNullOrEmpty(dataTable.searchValue))
                {
                    invoiceList = invoiceList.Where(e =>
                        e.VendorName.Contains(dataTable.searchValue, StringComparison.OrdinalIgnoreCase) ||
                        e.InvoiceNo.Contains(dataTable.searchValue, StringComparison.OrdinalIgnoreCase) ||
                        e.ProjectName.Contains(dataTable.searchValue, StringComparison.OrdinalIgnoreCase) ||
                        e.TotalAmount.ToString().Contains(dataTable.searchValue)).ToList();
                }

                var totalRecord = invoiceList.Count;
                var filteredData = invoiceList.Skip(dataTable.skip).Take(dataTable.pageSize).ToList();

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
                string dbConnectionStr = Configuration.GetConnectionString("EMPDbconn");
                var sqlPar = new SqlParameter[]
                {
                    new SqlParameter("@VendorId", Vid),
                };
                var dataSet = DbHelper.GetDataSet("[spGetLastTransactionByVendor]", System.Data.CommandType.StoredProcedure, sqlPar , dbConnectionStr);

                var VendorTransaction = new List<CreditDebitView>();

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var LastTransactions = new CreditDebitView
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        VendorName = row["VendorName"].ToString(),
                        PaymentTypeName = row["PaymentTypeName"].ToString(),
                        PaymentMethodName = row["PaymentMethodName"].ToString(),
                        Date = Convert.ToDateTime(row["Date"]),
                        TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                        PendingAmount = Convert.ToDecimal(row["PendingAmount"]),
                        CreditDebitAmount = Convert.ToDecimal(row["CreditDebitAmount"]),        
                    };
                    VendorTransaction.Add(LastTransactions);
                }

                return VendorTransaction;
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
                    Date = CreditDebit.Date,
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
                response.Message = "Details inserted successfully!";
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
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
                    DispatchDocNo= InsertInvoice.DispatchDocNo,
                    Destination= InsertInvoice.Destination,
                    MotorVehicleNo= InsertInvoice.MotorVehicleNo,
                    ShippingAddress = InsertInvoice.ShippingAddress,
                    TotalDiscount = InsertInvoice.TotalDiscount,
                    TotalGst = InsertInvoice.TotalGst,
                    RoundOff = InsertInvoice.RoundOff,
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
                        DiscountAmount = item.DiscountAmount,
                        GstAmount = item.GstAmount,
                        GstPer = item.GstPer,
                        Igst = item.Igst,
                        ProductTotal = item.ProductTotal,
                        Hsn = item.Hsn,
                        IsDeleted = false,
                        CreatedBy = InsertInvoice.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    Context.TblInvoiceDetails.Add(InvoiceDetails);
                }
                if (InsertInvoice.PaymentStatus == 7)
                {
                    var creditdebit = new TblCreditDebitMaster()
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
                    Context.TblCreditDebitMasters.Add(creditdebit);
                }

                await Context.SaveChangesAsync();
                response.Code = (int)HttpStatusCode.OK;
                response.Message = "Invoice successfully inserted.";
            }
            catch (Exception)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in inserting Invoice.";
            }

            return response;
        }

        public async Task<List<CreditDebitView>> GetAllTransaction()
        {
            try
            {
                string dbConnectionStr = Configuration.GetConnectionString("EMPDbconn");
                var dataSet = DbHelper.GetDataSet("[spGetAllTransaction]", System.Data.CommandType.StoredProcedure, new SqlParameter[] { }, dbConnectionStr);

                var AllTransactionList = new List<CreditDebitView>();

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var Transactions = new CreditDebitView
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        VendorName = row["VendorCompany"].ToString(),
                        PaymentTypeName = row["PaymentTypeName"].ToString(),
                        PaymentMethodName = row["PaymentMethodName"].ToString(),
                        Date = Convert.ToDateTime(row["Date"]),
                        VendorId = Guid.Parse(row["VId"].ToString()),
                        VendorAddress = row["VendorAddress"].ToString(),
                        PendingAmount = Convert.ToDecimal(row["PendingAmount"]),
                        CreditDebitAmount = Convert.ToDecimal(row["CreditDebitAmount"]),
                        TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                    };
                    AllTransactionList.Add(Transactions);
                }

                return AllTransactionList;
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

                    response.Message = "Invoice is generated successfully";
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
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


                    response.Message = "Invoice details are successfully deleted.";
                }
                else
                {
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Message = "No related records found to delete";
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
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
                    DispatchDocNo = UpdateInvoice.DispatchDocNo,
                    Destination=UpdateInvoice.Destination,
                    MotorVehicleNo = UpdateInvoice.MotorVehicleNo,
                    ShippingAddress = UpdateInvoice.ShippingAddress,
                    TotalDiscount = UpdateInvoice.TotalDiscount,
                    TotalGst = UpdateInvoice.TotalGst,
                    RoundOff = UpdateInvoice.RoundOff,
                    TotalAmount = UpdateInvoice.TotalAmount,
                    PaymentMethod = UpdateInvoice.PaymentMethod,
                    Status = UpdateInvoice.Status,
                    PaymentStatus = UpdateInvoice.PaymentStatus,
                    CompanyId = UpdateInvoice.CompanyId,
                    CreatedOn = UpdateInvoice.CreatedOn,
                    UpdatedOn = DateTime.Now,
                    UpdatedBy = UpdateInvoice.UpdatedBy,
                    CreatedBy = UpdateInvoice.CreatedBy,
                    Date = DateTime.Now,
                    IsDeleted = false,
                };
                Context.TblInvoices.Update(invoice);

                foreach (var item in UpdateInvoice.InvoiceDetails)
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
                        existingInvoice.DiscountAmount = item.DiscountAmount;
                        existingInvoice.GstPer = item.GstPer;
                        existingInvoice.GstAmount = item.GstAmount;
                        existingInvoice.Igst = item.Igst;
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
                            Igst = item.Igst,
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
                response.Message = "Invoice Update successfully.";
            }
            catch (Exception)
            {
                response.Message = "Error in updating invoice.";
                response.Code = (int)HttpStatusCode.InternalServerError;
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
                    response.Code = (int)HttpStatusCode.NotFound;
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

                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error in generating invoice.";
                response.Code = (int)HttpStatusCode.InternalServerError;
            }
            return response;
        }

        public async Task<IEnumerable<InvoiceViewModel>> InvoiceActivity(Guid ProjectId)
        {
            try
            {
                string dbConnectionStr = Configuration.GetConnectionString("EMPDbconn");
                var sqlPar = new SqlParameter[]
                {
                    new SqlParameter("@ProjectId", ProjectId),
                };
                var DS = DbHelper.GetDataSet("[InvoiceActivity]", System.Data.CommandType.StoredProcedure, sqlPar, dbConnectionStr);

                List<InvoiceViewModel> ActivityList = new List<InvoiceViewModel>();

                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        var ActivityDetails = new InvoiceViewModel
                        {

                            Id = row["Id"] != DBNull.Value ? (Guid)row["Id"] : Guid.Empty,
                            InvoiceNo = row["InvoiceNo"]?.ToString(),
                            VendorName = row["VendorCompany"]?.ToString(),
                            VandorId = row["VandorId"] != DBNull.Value ? (Guid)row["VandorId"] : Guid.Empty,
                            UserName = row["UserName"]?.ToString(),
                            FirstName = row["FirstName"]?.ToString(),
                            LastName = row["LastName"]?.ToString(),
                            UserImage = row["Image"]?.ToString(),
                            Status = row["Status"]?.ToString(),
                            ShippingAddress = row["ShippingAddress"]?.ToString(),
                            TotalAmount = row["TotalAmount"] != DBNull.Value ? (decimal)row["TotalAmount"] : 0m,
                            CreatedOn = row["CreatedOn"] != DBNull.Value ? (DateTime)row["CreatedOn"] : DateTime.MinValue,
                            CreatedBy = row["CreatedBy"] != DBNull.Value ? (Guid)row["CreatedBy"] : Guid.Empty,
                            UpdatedOn = row["UpdatedOn"] != DBNull.Value ? (DateTime)row["UpdatedOn"] : DateTime.MinValue,
                            UpdatedBy = row["UpdatedBy"] != DBNull.Value ? (Guid)row["UpdatedBy"] : Guid.Empty
                        };
                        ActivityList.Add(ActivityDetails);
                    }
                }
                return ActivityList.Take(3);
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
                string dbConnectionStr = Configuration.GetConnectionString("EMPDbconn");
                var sqlPar = new SqlParameter[]
                {
                   new SqlParameter("@VendorId", Vid),
                };

                var dataSet = DbHelper.GetDataSet("GetAllTransactionByVendorId", CommandType.StoredProcedure, sqlPar, dbConnectionStr);

                var tranactionList = ConvertDataSetToVendorTranactionList(dataSet);

                if (!string.IsNullOrEmpty(dataTable.searchValue.ToLower()))
                {
                    tranactionList = tranactionList.Where(e =>
                        e.VendorName.Contains(dataTable.searchValue.ToLower(), StringComparison.OrdinalIgnoreCase) ||
                        e.CreditDebitAmount.ToString().Contains(dataTable.searchValue.ToLower(), StringComparison.OrdinalIgnoreCase) ||
                        e.TotalAmount.ToString().Contains(dataTable.searchValue.ToLower(), StringComparison.OrdinalIgnoreCase) ||
                        e.Date.ToString().Contains(dataTable.searchValue)).ToList();
                }

                var totalRecord = tranactionList.Count;
                var filteredData = tranactionList.Skip(dataTable.skip).Take(dataTable.pageSize).ToList();

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
                throw ex;
            }
        }

        private List<CreditDebitView> ConvertDataSetToVendorTranactionList(DataSet dataSet)
        {
            var userDetails = new List<CreditDebitView>();
            try
            {

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var userData = new CreditDebitView
                    {
                        Id = row["Id"] != DBNull.Value ? (int)row["Id"] : 0,
                        VendorName = row["VendorName"].ToString(),
                        Date = Convert.ToDateTime(row["Date"]),
                        PaymentTypeName = row["PaymentTypeName"].ToString(),
                        PaymentMethodName = row["PaymentMethodName"].ToString(),
                        PendingAmount = Convert.ToDecimal(row["PendingAmount"]),
                        CreditDebitAmount = Convert.ToDecimal(row["CreditDebitAmount"]),
                        TotalAmount = Convert.ToDecimal(row["TotalAmount"])
                    };
                    userDetails.Add(userData);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return userDetails;
        }

        private Func<CreditDebitView, object> GetSortExpression(string sortColumn)
        {
            switch (sortColumn.ToLower())
            {
                case "UserName":
                    return t => t.VendorName;
                case "DepartmentName":
                    return t => t.PaymentTypeName;
                case "FirstName":
                    return t => t.PaymentMethodName;
                case "LastName":
                    return t => t.Date;
                case "DateOfBirth":
                    return t => t.TotalAmount;
                case "Address":
                    return t => t.PendingAmount;
                case "CityName":
                    return t => t.CreditDebitAmount;
                default:
                    return t => t.CreatedOn;
            }
        }

        public async Task<InvoiceMasterModel> DisplayInvoiceDetailsById(Guid Id)
        {
            try
            {
                string dbConnectionStr = Configuration.GetConnectionString("EMPDbconn");
                var sqlPar = new SqlParameter[]
                {
            new SqlParameter("@InvoiceId", Id),
                };
                var DS = DbHelper.GetDataSet("[GetInvoiceDetailsById]", System.Data.CommandType.StoredProcedure, sqlPar, dbConnectionStr);

                InvoiceMasterModel InvoiceDetails = new InvoiceMasterModel();

                if (DS != null && DS.Tables.Count > 0)
                {
                    if (DS.Tables[0].Rows.Count > 0)
                    {
                        InvoiceDetails.Id = DS.Tables[0].Rows[0]["Id"] != DBNull.Value ? (Guid)DS.Tables[0].Rows[0]["Id"] : Guid.Empty;
                        InvoiceDetails.VandorId = DS.Tables[0].Rows[0]["VandorId"] != DBNull.Value ? (Guid)DS.Tables[0].Rows[0]["VandorId"] : Guid.Empty;
                        InvoiceDetails.CompanyId = DS.Tables[0].Rows[0]["CompanyId"] != DBNull.Value ? (Guid)DS.Tables[0].Rows[0]["CompanyId"] : Guid.Empty;
                        InvoiceDetails.InvoiceNo = DS.Tables[0].Rows[0]["InvoiceNo"]?.ToString();
                        InvoiceDetails.ProjectId = DS.Tables[0].Rows[0]["ProjectId"] != DBNull.Value ? (Guid)DS.Tables[0].Rows[0]["ProjectId"] : Guid.Empty;
                        InvoiceDetails.VendorAddress = DS.Tables[0].Rows[0]["VendorAddress"]?.ToString();
                        InvoiceDetails.VendorCompanyName = DS.Tables[0].Rows[0]["VendorCompanyName"]?.ToString();
                        InvoiceDetails.VendorGstnumber = DS.Tables[0].Rows[0]["VendorGstnumber"]?.ToString();
                        InvoiceDetails.VendorCompanyNumber = DS.Tables[0].Rows[0]["VendorCompanyNumber"]?.ToString();
                        InvoiceDetails.VendorBankName = DS.Tables[0].Rows[0]["VendorBankName"]?.ToString();
                        InvoiceDetails.VendorBankBranch = DS.Tables[0].Rows[0]["VendorBankBranch"]?.ToString();
                        InvoiceDetails.VendorBankAccountNo = DS.Tables[0].Rows[0]["VendorBankAccountNo"]?.ToString();
                        InvoiceDetails.VendorBankIfsc = DS.Tables[0].Rows[0]["VendorBankIfsc"]?.ToString();
                        InvoiceDetails.VendorAccountHolderName = DS.Tables[0].Rows[0]["VendorAccountHolderName"]?.ToString();
                        InvoiceDetails.CompnyName = DS.Tables[0].Rows[0]["CompnyName"]?.ToString();
                        InvoiceDetails.CompanyGst = DS.Tables[0].Rows[0]["CompanyGst"]?.ToString();
                        InvoiceDetails.CompanyAddress = DS.Tables[0].Rows[0]["CompanyAddress"]?.ToString();
                        InvoiceDetails.InvoiceDate = DS.Tables[0].Rows[0]["InvoiceDate"] != DBNull.Value ? (DateTime)DS.Tables[0].Rows[0]["InvoiceDate"] : DateTime.MinValue;
                        InvoiceDetails.BuyesOrderDate = DS.Tables[0].Rows[0]["BuyesOrderDate"] != DBNull.Value ? (DateTime)DS.Tables[0].Rows[0]["BuyesOrderDate"] : DateTime.MinValue;
                        InvoiceDetails.BuyesOrderNo = DS.Tables[0].Rows[0]["BuyesOrderNo"]?.ToString();
                        InvoiceDetails.DispatchThrough = DS.Tables[0].Rows[0]["DispatchThrough"]?.ToString();
                        InvoiceDetails.DispatchDocNo = DS.Tables[0].Rows[0]["DispatchDocNo"]?.ToString();
                        InvoiceDetails.Destination = DS.Tables[0].Rows[0]["Destination"]?.ToString();
                        InvoiceDetails.MotorVehicleNo = DS.Tables[0].Rows[0]["MotorVehicleNo"]?.ToString();
                        InvoiceDetails.CompanyState = DS.Tables[0].Rows[0]["CompanyState"]?.ToString();
                        InvoiceDetails.VendorState = DS.Tables[0].Rows[0]["VendorState"]?.ToString();
                        InvoiceDetails.TotalGst = DS.Tables[0].Rows[0]["TotalGst"] != DBNull.Value ? (decimal)DS.Tables[0].Rows[0]["TotalGst"] : 0m;
                        InvoiceDetails.TotalAmount = DS.Tables[0].Rows[0]["TotalAmount"] != DBNull.Value ? (decimal)DS.Tables[0].Rows[0]["TotalAmount"] : 0m;
                        InvoiceDetails.PaymentMethod = DS.Tables[0].Rows[0]["PaymentMethod"] != DBNull.Value ? (int)DS.Tables[0].Rows[0]["PaymentMethod"] : 0;
                        InvoiceDetails.PaymentStatus = DS.Tables[0].Rows[0]["PaymentStatus"] != DBNull.Value ? (int)DS.Tables[0].Rows[0]["PaymentStatus"] : 0;
                        InvoiceDetails.ShippingAddress = DS.Tables[0].Rows[0]["ShippingAddress"]?.ToString();
                        InvoiceDetails.PaymentMethodName = DS.Tables[0].Rows[0]["PaymentMethodName"]?.ToString();
                        InvoiceDetails.PaymentStatusName = DS.Tables[0].Rows[0]["PaymentStatusName"]?.ToString();
                        InvoiceDetails.CreatedOn = DS.Tables[0].Rows[0]["CreatedOn"] != DBNull.Value ? (DateTime)DS.Tables[0].Rows[0]["CreatedOn"] : DateTime.MinValue;
                        InvoiceDetails.RoundOff = DS.Tables[0].Rows[0]["RoundOff"] != DBNull.Value ? (decimal)DS.Tables[0].Rows[0]["RoundOff"] : 0m;
                        InvoiceDetails.CompanyStateCode = DS.Tables[0].Rows[0]["CompanyStateCode"] != DBNull.Value ? (Int32)DS.Tables[0].Rows[0]["CompanyStateCode"] : 0;
                        InvoiceDetails.VendorStateCode = DS.Tables[0].Rows[0]["VendorStateCode"] != DBNull.Value ? (Int32)DS.Tables[0].Rows[0]["VendorStateCode"] : 0;
                    }

                    InvoiceDetails.InvoiceDetails = new List<InvoiceDetailsViewModel>();

                    foreach (DataRow row in DS.Tables[1].Rows)
                    {
                        var invoiceDetail = new InvoiceDetailsViewModel
                        {
                            ProductId = row["ProductId"] != DBNull.Value ? (Guid)row["ProductId"] : Guid.Empty,
                            Product = row["Product"]?.ToString(),
                            Hsn = row["HSN"] != DBNull.Value ? (int)row["HSN"] : 0,
                            Quantity = row["Quantity"] != DBNull.Value ? (decimal)row["Quantity"] : 0,
                            ProductType = row["ProductType"] != DBNull.Value ? (int)row["ProductType"] : 0,
                            ProductTypeName = row["ProductTypeName"]?.ToString(),
                            PerUnitPrice = row["PerUnitPrice"] != DBNull.Value ? (decimal)row["PerUnitPrice"] : 0m,
                            GstAmount = row["GstAmount"] != DBNull.Value ? (decimal)row["GstAmount"] : 0m,
                            GstPer = row["GstPer"] != DBNull.Value ? (decimal)row["GstPer"] : 0m,
                            Igst = row["IGst"] != DBNull.Value ? (decimal)row["IGst"] : 0m,
                            ProductTotal = row["ProductTotal"] != DBNull.Value ? (decimal)row["ProductTotal"] : 0m,
                            DiscountPer = row["DiscountPer"] != DBNull.Value ? (decimal)row["DiscountPer"] : 0m,
                            DiscountAmount = row["DiscountAmount"] != DBNull.Value ? (decimal)row["DiscountAmount"] : 0m,
                        };

                        InvoiceDetails.InvoiceDetails.Add(invoiceDetail);
                    }
                }
                return InvoiceDetails;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<InvoiceDetailsViewModel>> GetProductDetailsById(Guid ProductId)
        {
            try
            {
                var productDetails = new List<InvoiceDetailsViewModel>();
                var data = await (from a in Context.TblProductDetailsMasters.Where(x => x.Id == ProductId)
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

        public async Task<UserResponceModel> DeleteTransaction(int Id)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var GetTrandata = Context.TblCreditDebitMasters.Where(a => a.Id == Id).FirstOrDefault();

                if (GetTrandata != null)
                {
                    Context.TblCreditDebitMasters.Remove(GetTrandata);
                    Context.SaveChanges();
                    response.Data = GetTrandata;
                    response.Message = "Transaction is deleted successfully";
                }
                else
                {
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Message = "Can not found";
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error deleting transactions";
            }
            return response;
        }
    }
}
