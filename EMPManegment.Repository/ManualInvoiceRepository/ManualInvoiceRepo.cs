﻿using EMPManagment.API;
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
                                  join b in Context.TblProjectMasters on a.ProjectId equals b.ProjectId
                                  where a.IsDeleted != true
                                  orderby a.CreatedOn descending
                                  select new ManualInvoiceMasterModel
                                  {
                                      Id = a.Id,
                                      InvoiceNo = a.InvoiceNo,
                                      InvoiceDate = a.InvoiceDate,
                                      VendorName = a.VendorName,
                                      VendorPhoneNo = a.VendorPhoneNo,
                                      VendorGstNo  = a.VendorGstNo,
                                      CompanyName = a.CompanyName,
                                      CompanyAddress = a.CompanyAddress,
                                      CompanyGstNo = a.CompanyGstNo,
                                      ProjectId = a.ProjectId,
                                      ProjectName = b.ProjectTitle,
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
    }
}
