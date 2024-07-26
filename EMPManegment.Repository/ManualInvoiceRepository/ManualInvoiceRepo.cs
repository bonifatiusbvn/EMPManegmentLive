using EMPManagment.API;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.Common;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.ManualInvoice;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.ManualInvoice;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.Xml;
using EMPManegment.EntityModels.ViewModels.UserModels;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using System.Diagnostics;

#nullable disable

namespace EMPManegment.Repository.ManualInvoiceRepository
{
    public class ManualInvoiceRepo : IManualInvoice
    {
        public ManualInvoiceRepo(BonifatiusEmployeesContext context, IConfiguration configuration)
        {
            Context = context;
            _configuration = configuration;
        }

        public BonifatiusEmployeesContext Context { get; }
        public IConfiguration _configuration { get; }

        public async Task<UserResponceModel> InsertManualInvoice(ManualInvoiceMasterModel InvoiceDetails)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var invoice = new TblManualInvoice()
                {
                    Id = Guid.NewGuid(),
                    InvoiceNo = InvoiceDetails.InvoiceNo,
                    InvoiceType = InvoiceDetails.InvoiceType,
                    VendorName = InvoiceDetails.VendorName,
                    VendorAddress = InvoiceDetails.VendorAddress,
                    VendorGstNo = InvoiceDetails.VendorGstNo,
                    VendorPhoneNo = InvoiceDetails.VendorPhoneNo,
                    CompanyName = InvoiceDetails.CompanyName,
                    CompanyAddress = InvoiceDetails.CompanyAddress,
                    CompanyGstNo = InvoiceDetails.CompanyGstNo,
                    ProjectId = InvoiceDetails.ProjectId,
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
                    RoundOff = InvoiceDetails.RoundOff,
                    CreatedBy = InvoiceDetails.CreatedBy,
                    CreatedOn = DateTime.Now,
                };
                Context.TblManualInvoices.Add(invoice);

                foreach (var item in InvoiceDetails.ManualInvoiceDetails)
                {
                    var invoiceDetails = new TblManualInvoiceDetail()
                    {
                        RefId = invoice.Id,
                        Product = item.Product,
                        Quantity = item.Quantity,
                        ProductType = item.ProductType,
                        Price = item.Price,
                        DiscountPercent = item.DiscountPercent,
                        Gst = item.Gst,
                        ProductTotal = item.ProductTotal,
                        IsDeleted = false,
                        CreatedBy = invoice.CreatedBy,
                        GstAmount = item.GstAmount,
                        Discount = item.Discount,
                        Hsn = item.Hsn,
                        CreatedOn = DateTime.Now,
                    };
                    Context.TblManualInvoiceDetails.Add(invoiceDetails);
                }
                await Context.SaveChangesAsync();
                response.Message = "Invoice successfully inserted.";
                response.Data = invoice.Id;
            }
            catch (Exception)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in inserting Invoice.";
            }

            return response;
        }

        public async Task<jsonData> GetManualInvoiceList(DataTableRequstModel dataTable)
        {
            try
            {
                string dbConnectionStr = _configuration.GetConnectionString("EMPDbconn");

                var dataSet = DbHelper.GetDataSet("GetManualInvoiceList", CommandType.StoredProcedure, new SqlParameter[] { }, dbConnectionStr);

                var mInvoiceList = ConvertDataSetToMInvoiceList(dataSet);

                if (!string.IsNullOrEmpty(dataTable.searchValue.ToLower()))
                {
                    mInvoiceList = mInvoiceList.Where(e =>
                        e.VendorName.Contains(dataTable.searchValue.ToLower(), StringComparison.OrdinalIgnoreCase) ||
                        e.CompanyName.Contains(dataTable.searchValue.ToLower(), StringComparison.OrdinalIgnoreCase) ||
                        e.InvoiceNo.Contains(dataTable.searchValue.ToLower(), StringComparison.OrdinalIgnoreCase) ||
                        e.ProjectName.Contains(dataTable.searchValue.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
                }

                IQueryable<ManualInvoiceModel> queryablemInvoiceDetails = mInvoiceList.AsQueryable();

                if (!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDir))
                {
                    switch (dataTable.sortColumn.ToLower())
                    {
                        case "createdon":
                            queryablemInvoiceDetails = dataTable.sortColumnDir == "asc" ? queryablemInvoiceDetails.OrderBy(e => e.CreatedOn) : queryablemInvoiceDetails.OrderByDescending(e => e.CreatedOn);
                            break;
                        case "companyname":
                            queryablemInvoiceDetails = dataTable.sortColumnDir == "asc" ? queryablemInvoiceDetails.OrderBy(e => e.CompanyName) : queryablemInvoiceDetails.OrderByDescending(e => e.CompanyName);
                            break;
                        case "vendorname":
                            queryablemInvoiceDetails = dataTable.sortColumnDir == "asc" ? queryablemInvoiceDetails.OrderBy(e => e.VendorName) : queryablemInvoiceDetails.OrderByDescending(e => e.VendorName);
                            break;
                        case "invoiceno":
                            queryablemInvoiceDetails = dataTable.sortColumnDir == "asc" ? queryablemInvoiceDetails.OrderBy(e => e.InvoiceNo) : queryablemInvoiceDetails.OrderByDescending(e => e.InvoiceNo);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    queryablemInvoiceDetails = queryablemInvoiceDetails.OrderByDescending(e => e.CreatedOn);
                }

                var totalRecord = queryablemInvoiceDetails.Count();
                var filteredData = queryablemInvoiceDetails.Skip(dataTable.skip).Take(dataTable.pageSize).ToList();

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

        private List<ManualInvoiceModel> ConvertDataSetToMInvoiceList(DataSet dataSet)
        {
            var mInvoiceDetails = new List<ManualInvoiceModel>();
            try
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var manualInvoice = new ManualInvoiceModel
                    {
                        Id = Guid.Parse(row["Id"].ToString()),
                        InvoiceNo = row["InvoiceNo"]?.ToString(),

                        VendorName = row["VendorName"]?.ToString(),
                        VendorAddress = row["VendorAddress"]?.ToString(),
                        VendorGstNo = row["VendorGstNo"]?.ToString(),
                        VendorPhoneNo = row["VendorPhoneNo"]?.ToString(),
                        CompanyName = row["CompanyName"]?.ToString(),
                        CompanyAddress = row["CompanyAddress"]?.ToString(),
                        CompanyGstNo = row["CompanyGstNo"]?.ToString(),
                        TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                        InvoiceDate = Convert.ToDateTime(row["InvoiceDate"]),
                        ProjectId = row["ProjectId"] != DBNull.Value ? (Guid)row["ProjectId"] : Guid.Empty,
                        BuyesOrderDate = row["BuyesOrderDate"] != DBNull.Value ? (DateTime)row["BuyesOrderDate"] : DateTime.MinValue,
                        BuyesOrderNo = row["BuyesOrderNo"]?.ToString(),
                        DispatchThrough = row["DispatchThrough"]?.ToString(),
                        TotalGst = Convert.ToDecimal(row["TotalGst"]),
                        RoundOff = row["RoundOff"] != DBNull.Value ? (decimal)row["RoundOff"] : 0m,
                        PaymentMethod = row["PaymentMethod"] != DBNull.Value ? (int)row["PaymentMethod"] : 0,
                        PaymentStatus = row["PaymentStatus"] != DBNull.Value ? (int)row["PaymentStatus"] : 0,
                        ShippingAddress = row["ShippingAddress"]?.ToString(),
                        CreatedBy = Guid.Parse(row["CreatedBy"].ToString()),
                        CreatedOn = Convert.ToDateTime(row["CreatedOn"])
                    };
                    mInvoiceDetails.Add(manualInvoice);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return mInvoiceDetails;
        }

        public async Task<ManualInvoiceMasterModel> GetManualInvoiceDetails(Guid InvoiceId)
        {
            try
            {
                string dbConnectionStr = _configuration.GetConnectionString("EMPDbconn");
                var sqlPar = new SqlParameter[]
                {
                   new SqlParameter("@InvoiceId", InvoiceId),
                };
                var DS = DbHelper.GetDataSet("[GetManualInvoiceDetails]", System.Data.CommandType.StoredProcedure, sqlPar, dbConnectionStr);

                ManualInvoiceMasterModel masterInvoiceDetails = new ManualInvoiceMasterModel();

                if (DS != null && DS.Tables.Count > 0)
                {
                    if (DS.Tables[1].Rows.Count > 0)
                    {
                        masterInvoiceDetails.Id = DS.Tables[1].Rows[0]["Id"] != DBNull.Value ? (Guid)DS.Tables[1].Rows[0]["Id"] : Guid.Empty;
                        masterInvoiceDetails.InvoiceNo = DS.Tables[1].Rows[0]["InvoiceNo"]?.ToString();
                        masterInvoiceDetails.InvoicesType = DS.Tables[1].Rows[0]["InvoiceType"]?.ToString();
                        masterInvoiceDetails.VendorName = DS.Tables[1].Rows[0]["VendorName"]?.ToString();
                        masterInvoiceDetails.VendorAddress = DS.Tables[1].Rows[0]["VendorAddress"]?.ToString();
                        masterInvoiceDetails.VendorGstNo = DS.Tables[1].Rows[0]["VendorGstNo"]?.ToString();
                        masterInvoiceDetails.VendorPhoneNo = DS.Tables[1].Rows[0]["VendorPhoneNo"]?.ToString();
                        masterInvoiceDetails.CompanyName = DS.Tables[1].Rows[0]["CompanyName"]?.ToString();
                        masterInvoiceDetails.CompanyAddress = DS.Tables[1].Rows[0]["CompanyAddress"]?.ToString();
                        masterInvoiceDetails.CompanyGstNo = DS.Tables[1].Rows[0]["CompanyGstNo"]?.ToString();
                        masterInvoiceDetails.ProjectId = DS.Tables[1].Rows[0]["ProjectId"] != DBNull.Value ? (Guid)DS.Tables[1].Rows[0]["ProjectId"] : Guid.Empty;
                        masterInvoiceDetails.InvoiceDate = DS.Tables[1].Rows[0]["InvoiceDate"] != DBNull.Value ? (DateTime)DS.Tables[1].Rows[0]["InvoiceDate"] : DateTime.MinValue;
                        masterInvoiceDetails.BuyesOrderDate = DS.Tables[1].Rows[0]["BuyesOrderDate"] != DBNull.Value ? (DateTime)DS.Tables[1].Rows[0]["BuyesOrderDate"] : DateTime.MinValue;
                        masterInvoiceDetails.BuyesOrderNo = DS.Tables[1].Rows[0]["BuyesOrderNo"]?.ToString();
                        masterInvoiceDetails.DispatchThrough = DS.Tables[1].Rows[0]["DispatchThrough"]?.ToString();
                        masterInvoiceDetails.TotalGst = DS.Tables[1].Rows[0]["TotalGst"] != DBNull.Value ? (decimal)DS.Tables[1].Rows[0]["TotalGst"] : 0m;
                        masterInvoiceDetails.TotalAmount = DS.Tables[1].Rows[0]["TotalAmount"] != DBNull.Value ? (decimal)DS.Tables[1].Rows[0]["TotalAmount"] : 0m;
                        masterInvoiceDetails.PaymentMethod = DS.Tables[1].Rows[0]["PaymentMethod"] != DBNull.Value ? (int)DS.Tables[1].Rows[0]["PaymentMethod"] : 0;
                        masterInvoiceDetails.PaymentStatus = DS.Tables[1].Rows[0]["PaymentStatus"] != DBNull.Value ? (int)DS.Tables[1].Rows[0]["PaymentStatus"] : 0;
                        masterInvoiceDetails.ShippingAddress = DS.Tables[1].Rows[0]["ShippingAddress"]?.ToString();
                        masterInvoiceDetails.PaymentMethodName = DS.Tables[1].Rows[0]["PaymentMethodName"]?.ToString();
                        masterInvoiceDetails.PaymentStatusName = DS.Tables[1].Rows[0]["PaymentStatusName"]?.ToString();
                        masterInvoiceDetails.RoundOff = DS.Tables[1].Rows[0]["RoundOff"] != DBNull.Value ? (decimal)DS.Tables[1].Rows[0]["RoundOff"] : 0m;
                    }

                    masterInvoiceDetails.ManualInvoiceDetails = new List<ManualInvoiceDetailsModel>();

                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        var invoiceDetail = new ManualInvoiceDetailsModel
                        {
                            RefId = row["RefId"] != DBNull.Value ? (Guid)row["RefId"] : Guid.Empty,
                            Product = row["Product"]?.ToString(),
                            ProductType = row["ProductType"] != DBNull.Value ? (int)row["ProductType"] : 0,
                            ProductTypeName = row["ProductTypeName"]?.ToString(),
                            Quantity = row["Quantity"] != DBNull.Value ? (decimal)row["Quantity"] : 0,
                            Price = row["Price"] != DBNull.Value ? (decimal)row["Price"] : 0m,
                            Gst = row["Gst"] != DBNull.Value ? (decimal)row["Gst"] : 0m,
                            DiscountPercent = row["DiscountPercent"] != DBNull.Value ? (decimal)row["DiscountPercent"] : 0m,
                            ProductTotal = row["ProductTotal"] != DBNull.Value ? (decimal)row["ProductTotal"] : 0m,
                            GstAmount = row["GstAmount"] != DBNull.Value ? (decimal)row["GstAmount"] : 0m,
                            Discount = row["Discount"] != DBNull.Value ? (decimal)row["Discount"] : 0m,
                            Hsn = row["Hsn"] != DBNull.Value ? (int)row["Hsn"] : 0
                        };

                        masterInvoiceDetails.ManualInvoiceDetails.Add(invoiceDetail);
                    }
                }

                return masterInvoiceDetails;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching manual invoice details", ex);
            }
        }

        public async Task<UserResponceModel> DeleteManualInvoice(Guid InvoiceId)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {

                var GetInvoiceData = Context.TblManualInvoices.Where(a => a.Id == InvoiceId).FirstOrDefault();
                var InvoiceDetails = Context.TblManualInvoiceDetails.Where(a => a.RefId == InvoiceId).ToList();

                if (GetInvoiceData != null)
                {
                    GetInvoiceData.IsDeleted = true;
                    Context.TblManualInvoices.Update(GetInvoiceData);
                    if (InvoiceDetails.Any())
                    {
                        foreach (var MIData in InvoiceDetails)
                        {
                            MIData.IsDeleted = true;
                            Context.TblManualInvoiceDetails.Update(MIData);
                        }

                        Context.SaveChanges();
                        response.Message = "Manual Invoice details are successfully deleted.";
                    }
                    else
                    {
                        response.Code = (int)HttpStatusCode.NotFound;
                        response.Message = "No related records found to delete";
                    }
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
                response.Message = "Error in deleting manual invoice.";
            }
            return response;
        }

        public async Task<UserResponceModel> UpdateManualInvoice(ManualInvoiceMasterModel UpdateInvoice)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var ManualInvoice = new TblManualInvoice()
                {
                    Id = UpdateInvoice.Id,
                    InvoiceNo = UpdateInvoice.InvoiceNo,
                    VendorName = UpdateInvoice.VendorName,
                    VendorAddress = UpdateInvoice.VendorAddress,
                    VendorGstNo = UpdateInvoice.VendorGstNo,
                    VendorPhoneNo = UpdateInvoice.VendorPhoneNo,
                    CompanyName = UpdateInvoice.CompanyName,
                    CompanyAddress = UpdateInvoice.CompanyAddress,
                    CompanyGstNo = UpdateInvoice.CompanyGstNo,
                    ProjectId = UpdateInvoice.ProjectId,
                    InvoiceDate = UpdateInvoice.InvoiceDate,
                    BuyesOrderNo = UpdateInvoice.BuyesOrderNo,
                    BuyesOrderDate = UpdateInvoice.BuyesOrderDate,
                    DispatchThrough = UpdateInvoice.DispatchThrough,
                    ShippingAddress = UpdateInvoice.ShippingAddress,
                    Cgst = UpdateInvoice.Cgst,
                    Sgst = UpdateInvoice.Sgst,
                    Igst = UpdateInvoice.Igst,
                    TotalGst = UpdateInvoice.TotalGst,
                    TotalAmount = UpdateInvoice.TotalAmount,
                    PaymentMethod = UpdateInvoice.PaymentMethod,
                    Status = UpdateInvoice.Status,
                    PaymentStatus = UpdateInvoice.PaymentStatus,
                    IsDeleted = false,
                    UpdatedBy = UpdateInvoice.UpdatedBy,
                    UpdatedOn = DateTime.Now,
                    CreatedBy = UpdateInvoice.CreatedBy,
                    CreatedOn = UpdateInvoice.CreatedOn,
                    RoundOff = UpdateInvoice.RoundOff,

                };
                Context.TblManualInvoices.Update(ManualInvoice);

                foreach (var item in UpdateInvoice.ManualInvoiceDetails)
                {
                    var existingInvoice = Context.TblManualInvoiceDetails.FirstOrDefault(e => e.RefId == ManualInvoice.Id && e.Product == item.Product);

                    if (existingInvoice != null)
                    {
                        existingInvoice.RefId = ManualInvoice.Id;
                        existingInvoice.Product = item.Product;
                        existingInvoice.ProductType = item.ProductType;
                        existingInvoice.Quantity = item.Quantity;
                        existingInvoice.Price = item.Price;
                        existingInvoice.DiscountPercent = item.DiscountPercent;
                        existingInvoice.Gst = item.Gst;
                        existingInvoice.ProductTotal = item.ProductTotal;
                        existingInvoice.IsDeleted = false;
                        existingInvoice.UpdatedOn = DateTime.Now;
                        existingInvoice.UpdatedBy = ManualInvoice.UpdatedBy;
                        existingInvoice.CreatedOn = ManualInvoice.CreatedOn;
                        existingInvoice.CreatedBy = ManualInvoice.CreatedBy;
                        existingInvoice.GstAmount = item.GstAmount;
                        existingInvoice.Discount = item.Discount;
                        existingInvoice.Hsn = item.Hsn;

                        Context.TblManualInvoiceDetails.Update(existingInvoice);
                    }
                    else
                    {
                        var newInvoice = new TblManualInvoiceDetail()
                        {
                            RefId = ManualInvoice.Id,
                            Product = item.Product,
                            ProductType = item.ProductType,
                            Quantity = item.Quantity,
                            Price = item.Price,
                            Gst = item.Gst,
                            DiscountPercent = item.DiscountPercent,
                            ProductTotal = item.ProductTotal,
                            CreatedOn = DateTime.Now,
                            CreatedBy = ManualInvoice.CreatedBy,
                            Discount = item.Discount,
                            GstAmount = item.GstAmount,
                            Hsn = item.Hsn,
                        };

                        Context.TblManualInvoiceDetails.Add(newInvoice);
                    }
                }

                var deletedManualInvoice = UpdateInvoice.ManualInvoiceDetails.Select(a => a.Product).ToList();

                var ManualInvoiceToRemove = Context.TblManualInvoiceDetails
                    .Where(e => e.RefId == UpdateInvoice.Id && !deletedManualInvoice.Contains(e.Product))
                    .ToList();

                Context.TblManualInvoiceDetails.RemoveRange(ManualInvoiceToRemove);
                await Context.SaveChangesAsync();
                response.Code = (int)HttpStatusCode.OK;
                response.Message = "Manual Invoice Update successfully.";
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error updating manual invoice";
            }
            return response;
        }
    }
}
