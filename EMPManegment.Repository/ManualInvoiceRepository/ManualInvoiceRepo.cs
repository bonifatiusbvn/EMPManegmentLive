using EMPManagment.API;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.ManualInvoice;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.ManualInvoice;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
#nullable disable

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

        public async Task<jsonData> GetManualInvoiceList(DataTableRequstModel dataTable)
        {
            try
            {
                var invoicelist = from a in Context.TblManualInvoices
                                  where a.IsDeleted != true
                                  orderby a.CreatedOn descending
                                  select new ManualInvoiceMasterModel
                                  {
                                      Id = a.Id,
                                      InvoiceNo = a.InvoiceNo,
                                      InvoiceDate = a.InvoiceDate,
                                      VendorName = a.VendorName,
                                      VendorPhoneNo = a.VendorPhoneNo,
                                      VendorGstNo = a.VendorGstNo,
                                      CompanyName = a.CompanyName,
                                      CompanyAddress = a.CompanyAddress,
                                      CompanyGstNo = a.CompanyGstNo,
                                      ProjectId = a.ProjectId,
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
                                      TotalGst = a.TotalGst,
                                      PaymentMethod = a.PaymentMethod,
                                      PaymentStatus = a.PaymentStatus,
                                      ShippingAddress = a.ShippingAddress
                                  };

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

        public async Task<ManualInvoiceMasterModel> GetManualInvoiceDetails(Guid InvoiceId)
        {
            ManualInvoiceMasterModel manualInvoice = new ManualInvoiceMasterModel();
            try
            {
                manualInvoice = (from a in Context.TblManualInvoices.Where(x => x.Id == InvoiceId)
                                 join b in Context.TblProjectMasters on a.ProjectId equals b.ProjectId
                                 join c in Context.TblPaymentMethodTypes on a.PaymentMethod equals c.Id
                                 join d in Context.TblPaymentTypes on a.PaymentStatus equals d.Id
                                 select new ManualInvoiceMasterModel
                                 {
                                     Id = a.Id,
                                     InvoiceNo = a.InvoiceNo,
                                     VendorName = a.VendorName,
                                     VendorAddress = a.VendorAddress,
                                     VendorGstNo = a.VendorGstNo,
                                     VendorPhoneNo = a.VendorPhoneNo,
                                     CompanyName = a.CompanyName,
                                     CompanyAddress = a.CompanyAddress,
                                     CompanyGstNo = a.CompanyGstNo,
                                     ProjectId = a.ProjectId,
                                     ProjectName = b.ProjectTitle,
                                     InvoiceDate = a.InvoiceDate,
                                     BuyesOrderDate = a.BuyesOrderDate,
                                     BuyesOrderNo = a.BuyesOrderNo,
                                     DispatchThrough = a.DispatchThrough,
                                     TotalGst = a.TotalGst,
                                     TotalAmount = a.TotalAmount,
                                     PaymentMethod = a.PaymentMethod,
                                     PaymentStatus = a.PaymentStatus,
                                     ShippingAddress = a.ShippingAddress,
                                     PaymentMethodName = c.PaymentMethod,
                                     PaymentStatusName = d.Type,
                                     Cgst = a.Cgst,
                                     Sgst = a.Sgst,
                                     Igst = a.Igst,
                                     
                                 }).FirstOrDefault();
                List<ManualInvoiceDetailsModel> manualInvoiceDetails = (from a in Context.TblManualInvoiceDetails.Where(a => a.RefId == manualInvoice.Id)
                                                                        join b in Context.TblProductTypeMasters on a.ProductType equals b.Id
                                                                        select new ManualInvoiceDetailsModel
                                                                        {

                                                                            RefId = a.RefId,
                                                                            Product = a.Product,
                                                                            ProductType = a.ProductType,
                                                                            ProductTypeName = b.Type,
                                                                            Quantity = a.Quantity,
                                                                            Price = a.Price,
                                                                            Discount = a.Discount,
                                                                            Gst = a.Gst,
                                                                            ProductTotal = a.ProductTotal,
                                                                        }).ToList();


                manualInvoice.ManualInvoiceDetails = manualInvoiceDetails;
                return manualInvoice;
            }
            catch (Exception ex)
            {
                throw ex;
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

                        response.Code = 200;
                        response.Message = "Manual Invoice details are successfully deleted.";
                    }
                    else
                    {
                        response.Code = 404;
                        response.Message = "No related records found to delete";
                    }
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
                    Id = Guid.NewGuid(),
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
                    UpdatedBy = UpdateInvoice.CreatedBy,
                    UpdatedOn = DateTime.Now,
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
                        existingInvoice.Discount = item.Discount;
                        existingInvoice.Gst = item.Gst;
                        existingInvoice.ProductTotal = item.ProductTotal;
                        existingInvoice.IsDeleted = false;
                        existingInvoice.UpdatedOn = DateTime.Now;
                        existingInvoice.UpdatedBy = ManualInvoice.UpdatedBy;

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
                            Discount = item.Discount,
                            Gst = item.Gst,
                            ProductTotal = item.ProductTotal,
                            CreatedOn = DateTime.Now,
                            CreatedBy = ManualInvoice.CreatedBy,                           
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
                response.Code = 404;
                response.Message = "Error updating manual invoice";
            }
            return response;
        }
    }
}
