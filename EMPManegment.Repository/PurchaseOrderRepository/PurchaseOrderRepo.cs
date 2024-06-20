﻿using Azure;
using EMPManagment.API;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.EntityModels.ViewModels.Purchase_Request;
using EMPManegment.EntityModels.ViewModels.PurchaseOrderModels;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using EMPManegment.Inretface.Interface.OrderDetails;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace EMPManegment.Repository.OrderRepository
{
    public class PurchaseOrderRepo : IPurchaseOrderDetails
    {
        public PurchaseOrderRepo(BonifatiusEmployeesContext context)
        {
            Context = context;
        }

        public BonifatiusEmployeesContext Context { get; }


        public async Task<IEnumerable<PurchaseOrderDetailView>> GetPurchaseOrderList()
        {
            try
            {
                var data = await (from a in Context.TblPurchaseOrderMasters
                                  join b in Context.TblVendorMasters on a.VendorId equals b.Vid
                                  join d in Context.TblPaymentMethodTypes on a.PaymentMethod equals d.Id
                                  where a.IsDeleted != true
                                  select new
                                  {
                                      Order = a,
                                      Vendor = b,
                                      PaymentMethod = d,
                                      CreatedOn = a.CreatedOn,
                                  }).ToListAsync();

                var orderList = data.GroupBy(x => x.Order.OrderId)
                                    .Select(group => group.First())
                                    .OrderByDescending(item => item.CreatedOn)
                                    .Select(item => new PurchaseOrderDetailView
                                    {
                                        Id = item.Order.Id,
                                        OrderId = item.Order.OrderId,
                                        CompanyId = item.Order.CompanyId,
                                        VendorId = item.Order.VendorId,
                                        OrderDate = item.Order.OrderDate,
                                        TotalAmount = item.Order.TotalAmount,
                                        PaymentMethod = item.Order.PaymentMethod,
                                        PaymentMethodName = item.PaymentMethod.PaymentMethod,
                                        DeliveryStatus = item.Order.DeliveryStatus,
                                        DeliveryDate = item.Order.DeliveryDate,
                                        CreatedOn = item.Order.CreatedOn,
                                    });

                return orderList.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<PurchaseOrderDetailView>> GetPurchaseOrderDetailsByStatus(string DeliveryStatus)
        {
            try
            {
                var data = await (from a in Context.TblPurchaseOrderMasters
                                  join b in Context.TblVendorMasters on a.VendorId equals b.Vid
                                  join d in Context.TblPaymentMethodTypes on a.PaymentMethod equals d.Id
                                  where a.IsDeleted != true && a.DeliveryStatus == DeliveryStatus
                                  select new
                                  {
                                      Order = a,
                                      Vendor = b,
                                      PaymentMethod = d
                                  }).ToListAsync();

                var orderList = data.Select(item => new PurchaseOrderDetailView
                {
                    Id = item.Order.Id,
                    OrderId = item.Order.OrderId,
                    CompanyId = item.Order.CompanyId,
                    VendorId = item.Order.VendorId,
                    OrderDate = item.Order.OrderDate,
                    TotalAmount = item.Order.TotalAmount,
                    PaymentMethod = item.Order.PaymentMethod,
                    PaymentMethodName = item.PaymentMethod.PaymentMethod,
                    DeliveryStatus = item.Order.DeliveryStatus,
                    DeliveryDate = item.Order.DeliveryDate,
                    CreatedOn = item.Order.CreatedOn,
                }).ToList();

                return orderList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string CheckPurchaseOrder(string projectname)
        {
            try
            {
                var LastOrder = Context.TblPurchaseOrderMasters.OrderByDescending(e => e.CreatedOn).FirstOrDefault();
                var currentYear = DateTime.Now.Year;
                var lastYear = currentYear - 1;

                int startIndex = projectname.IndexOf('(');
                int endIndex = projectname.IndexOf(')');

                var Projectsubparts = projectname.Substring(startIndex + 1, endIndex - startIndex - 1);
                string UserOrderId;
                if (LastOrder == null)
                {
                    UserOrderId = $"BTPL/PO/{Projectsubparts}/{lastYear % 100}-{currentYear % 100}-001";
                }
                else
                {
                    if (LastOrder.OrderId.Length >= 25)
                    {
                        int orderNumber = int.Parse(LastOrder.OrderId.Substring(24)) + 1;
                        UserOrderId = $"BTPL/PO/{Projectsubparts}/{lastYear % 100}-{currentYear % 100}-" + orderNumber.ToString("D3");
                    }
                    else
                    {
                        throw new Exception("OrderId does not have the expected format.");
                    }
                }
                return UserOrderId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<ApiResponseModel> InsertMultiplePurchaseOrder(PurchaseOrderMasterView InsertPurchaseOrder)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
                var PurchaseOrder = new TblPurchaseOrderMaster()
                {
                    Id = Guid.NewGuid(),
                    OrderId = InsertPurchaseOrder.OrderId,
                    VendorId = InsertPurchaseOrder.VendorId,
                    CompanyId = InsertPurchaseOrder.CompanyId,
                    ProjectId = InsertPurchaseOrder.ProjectId,
                    TotalGst = InsertPurchaseOrder.TotalGst,
                    SubTotal = InsertPurchaseOrder.SubTotal,
                    TotalAmount = InsertPurchaseOrder.TotalAmount,
                    DeliveryDate = InsertPurchaseOrder.DeliveryDate,
                    OrderDate = InsertPurchaseOrder.OrderDate,
                    OrderStatus = InsertPurchaseOrder.OrderStatus,
                    PaymentMethod = InsertPurchaseOrder.PaymentMethod,
                    PaymentStatus = InsertPurchaseOrder.PaymentStatus,
                    DeliveryStatus = InsertPurchaseOrder.DeliveryStatus,
                    IsDeleted = false,
                    CreatedBy = InsertPurchaseOrder.CreatedBy,
                    CreatedOn = DateTime.Now,                   
                };
                Context.TblPurchaseOrderMasters.Add(PurchaseOrder);

                foreach (var item in InsertPurchaseOrder.ProductList)
                {
                    var PurchaseOrderDetail = new TblPurchaseOrderDetail()
                    {
                        PorefId = PurchaseOrder.Id,
                        ProductId = item.ProductId,
                        ProductType = item.ProductType,
                        Hsn = item.Hsn,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        DiscountAmt = item.Discount,
                        Gstamount = item.Gstamount,
                        Gstper=item.Gstper,
                        ProductTotal = item.ProductTotal,
                        IsDeleted = false,
                        CreatedBy = InsertPurchaseOrder.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    Context.TblPurchaseOrderDetails.Add(PurchaseOrderDetail);
                }
                    var PurchaseAddress = new TblPodeliveryAddress()
                    {
                        Poid = PurchaseOrder.Id,
                        Address = InsertPurchaseOrder.Address,
                        IsDeleted = false
                    };
                    Context.TblPodeliveryAddresses.Add(PurchaseAddress);

                await Context.SaveChangesAsync();
                response.code = (int)HttpStatusCode.OK;
                response.message = "Purchase order successfully inserted.";
            }
            catch (Exception ex)
            {
                response.code = 400;
                response.message = "Error in creating purchase orders.";
            }
            return response;
        }

        public async Task<PurchaseOrderMasterView> GetPurchaseOrderDetailsByOrderId(string OrderId)
        {
            PurchaseOrderMasterView orderDetails = new PurchaseOrderMasterView();
            try
            {
                orderDetails = (from a in Context.TblPurchaseOrderMasters.Where(x => x.OrderId == OrderId)
                                join b in Context.TblVendorMasters on a.VendorId equals b.Vid
                                join d in Context.TblCompanyMasters on a.CompanyId equals d.Id
                                join e in Context.TblCities on d.City equals e.Id
                                join f in Context.TblStates on d.State equals f.Id
                                join Vst in Context.TblStates on b.VendorState equals Vst.Id
                                join Vct in Context.TblCities on b.VendorCity equals Vct.Id
                                join g in Context.TblPodeliveryAddresses on a.Id equals g.Poid
                                join h in Context.TblPaymentMethodTypes on a.PaymentMethod equals h.Id
                                join i in Context.TblPaymentTypes on a.PaymentStatus equals i.Id
                                select new PurchaseOrderMasterView
                                {
                                    Id = a.Id,
                                    OrderId = a.OrderId,
                                    VendorId = a.VendorId,
                                    CompanyName = d.CompnyName,
                                    CompanyId = a.CompanyId,
                                    CompanyFullAddress = d.Address + "," + e.City + "," + f.State,
                                    CompanyGstNumber = d.Gst,
                                    VendorCompanyName = b.VendorFirstName + " " + b.VendorLastName,
                                    VendorFullAddress = b.VendorAddress + "," + Vct.City + "," + Vst.State + "-" + b.VendorPinCode,
                                    VendorCompanyNumber = b.VendorCompanyNumber,
                                    VendorCompanyEmail = b.VendorCompanyEmail,
                                    VendorAccountHolderName = b.VendorAccountHolderName,
                                    VendorBankAccountNo = b.VendorBankAccountNo,
                                    SubTotal = a.SubTotal,
                                    TotalGst = a.TotalGst,
                                    OrderDate = a.OrderDate,
                                    TotalAmount = a.TotalAmount,
                                    PaymentMethod = a.PaymentMethod,
                                    PaymentMethodName = h.PaymentMethod,
                                    PaymentStatus = a.PaymentStatus,
                                    PaymentStatusName=i.Type,
                                    DeliveryStatus = a.DeliveryStatus,
                                    DeliveryDate = a.DeliveryDate,
                                    CreatedOn = a.CreatedOn,
                                    Address = g.Address,
                                }).FirstOrDefault();
                List<PurchaseOrderDetailsModel> productList = (from a in Context.TblPurchaseOrderDetails.Where(a => a.PorefId == orderDetails.Id)
                                                               join b in Context.TblProductTypeMasters on a.ProductType equals b.Id
                                                               join c in Context.TblProductDetailsMasters on a.ProductId equals c.Id
                                                               select new PurchaseOrderDetailsModel
                                                               {
                                                                   ProductId = a.ProductId,
                                                                   ProductType = a.ProductType,
                                                                   ProductTotal = a.ProductTotal,
                                                                   Quantity = a.Quantity,
                                                                   ProductTypeName = b.Type,
                                                                   Price = a.Price,
                                                                   Gstamount = a.Gstamount,
                                                                   Gstper=a.Gstper,
                                                                   Hsn=a.Hsn,
                                                                   Product=c.ProductName,
                                                                   
                                                               }).ToList();
                orderDetails.ProductList = productList;
                return orderDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<PaymentMethodView>> GetAllPaymentMethod()
        {
            try
            {
                IEnumerable<PaymentMethodView> paymentMethod = Context.TblPaymentMethodTypes.ToList().Select(a => new PaymentMethodView
                {
                    Id = a.Id,
                    PaymentMethod = a.PaymentMethod,
                });
                return paymentMethod;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PurchaseOrderMasterView> EditPurchaseOrderDetails(Guid Id)
        {
            PurchaseOrderMasterView POList = new PurchaseOrderMasterView();
            try
            {
                POList = (from a in Context.TblPurchaseOrderMasters.Where(x => x.Id == Id)
                               join c in Context.TblCompanyMasters on a.CompanyId equals c.Id
                               join d in Context.TblVendorMasters on a.VendorId equals d.Vid
                               join f in Context.TblPaymentMethodTypes on a.PaymentMethod equals f.Id
                               join g in Context.TblPaymentTypes on a.PaymentStatus equals g.Id
                               select new PurchaseOrderMasterView
                               {
                                   Id = Id,
                                   OrderId = a.OrderId,
                                   VendorId = a.VendorId,
                                   ProjectId = a.ProjectId,
                                   CompanyName = c.CompnyName,
                                   CompanyId = a.CompanyId,
                                   TotalAmount = a.TotalAmount,
                                   OrderDate = a.OrderDate,
                                   OrderStatus = a.OrderStatus,
                                   DeliveryDate = a.DeliveryDate,
                                   DeliveryStatus = a.DeliveryStatus,
                                   PaymentMethod = a.PaymentMethod,
                                   PaymentStatus = a.PaymentStatus,
                                   PaymentMethodName = f.PaymentMethod,
                                   PaymentStatusName = g.Type,
                                   CompanyGst = c.Gst,
                                   CompanyFullAddress =c.Address,
                                   VendorCompanyName = d.VendorCompany,
                                   VendorCompanyNumber = d.VendorCompanyNumber,
                                   VendorGstnumber = d.VendorGstnumber,
                                   VendorAddress = d.VendorAddress,
                                   CreatedOn = a.CreatedOn,
                                   CreatedBy= (Guid)a.CreatedBy,
                               }).FirstOrDefault();
                List<PurchaseOrderDetailsModel> Productlist = (from a in Context.TblPurchaseOrderDetails.Where(a => a.PorefId == POList.Id)
                                                             join b in Context.TblProductTypeMasters on a.ProductType equals b.Id
                                                             join c in Context.TblProductDetailsMasters on a.ProductId equals c.Id
                                                             select new PurchaseOrderDetailsModel
                                                             {
                                                                 ProductId = a.ProductId,
                                                                 Product = c.ProductName,
                                                                 Hsn = a.Hsn,
                                                                 Quantity = a.Quantity,
                                                                 ProductType = a.ProductType,
                                                                 ProductTypeName = b.Type,
                                                                 Price = a.Price,
                                                                 Gstper = a.Gstper,
                                                                 Gstamount = a.Gstamount,
                                                                 ProductTotal = a.ProductTotal,
                                                             }).ToList();

                POList.ProductList = Productlist;
                return POList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserResponceModel> DeletePurchaseOrderDetails(Guid Id)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
               
                var GetOrderdata = Context.TblPurchaseOrderMasters.Where(a => a.Id == Id).FirstOrDefault();
                var PODetails = Context.TblPurchaseOrderDetails.Where(a => a.PorefId == Id).ToList();
                var POAddress = Context.TblPodeliveryAddresses.Where(a => a.Poid == Id).FirstOrDefault();

                if(GetOrderdata != null)
                {
                    GetOrderdata.IsDeleted = true;
                    Context.TblPurchaseOrderMasters.Update(GetOrderdata);
                    if (PODetails.Any() || POAddress != null)
                    {
                        foreach (var PODData in PODetails)
                        {
                            PODData.IsDeleted = true;
                            Context.TblPurchaseOrderDetails.Update(PODData);
                        }

                        POAddress.IsDeleted = true;
                        Context.TblPodeliveryAddresses.Update(POAddress);

                        Context.SaveChanges();

                        response.Code = 200;
                        response.Message = "Purchase order details are successfully deleted.";
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
                response.Message = "Error in deleting purchase order.";
            }
            return response;
        }

        public async Task<List<PurchaseOrderDetailsModel>> GetPOProductDetailsById(Guid ProductId)
        {
            try
            {
                var productDetails = new List<PurchaseOrderDetailsModel>();
                var data = await(from a in Context.TblProductDetailsMasters.Where(x => x.Id == ProductId)
                                 join b in Context.TblProductTypeMasters on a.ProductType equals b.Id
                                 select new PurchaseOrderDetailsModel
                                 {
                                     ProductType = b.Id,
                                     Product = a.ProductName,
                                     ProductId = a.Id,
                                     Price = a.PerUnitPrice,
                                     Gstamount = a.GstAmount ?? 0,
                                     Gstper =a.GstPercentage,
                                     ProductTypeName = b.Type,
                                     Hsn=a.Hsn,
                                 }).ToListAsync();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        productDetails.Add(new PurchaseOrderDetailsModel()
                        {
                            ProductId = item.ProductId,
                            ProductType = item.ProductType,
                            Product = item.Product,
                            Price = item.Price,
                            Gstamount = item.Gstamount,
                            Gstper = item.Gstper,
                            ProductTypeName = item.ProductTypeName,
                            Hsn = item.Hsn,
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

        public async Task<UserResponceModel> UpdatePurchaseOrderDetails(PurchaseOrderMasterView UpdatePurchaseorder)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var PurchaseOrder = Context.TblPurchaseOrderMasters.FirstOrDefault(po => po.Id == UpdatePurchaseorder.Id);
                {
                    PurchaseOrder.OrderId = UpdatePurchaseorder.OrderId;
                    PurchaseOrder.VendorId = UpdatePurchaseorder.VendorId;
                    PurchaseOrder.CompanyId = UpdatePurchaseorder.CompanyId;
                    PurchaseOrder.ProjectId = UpdatePurchaseorder.ProjectId;
                    PurchaseOrder.TotalGst = UpdatePurchaseorder.TotalGst;
                    PurchaseOrder.SubTotal = UpdatePurchaseorder.SubTotal;
                    PurchaseOrder.TotalAmount = UpdatePurchaseorder.TotalAmount;
                    PurchaseOrder.DeliveryDate = UpdatePurchaseorder.DeliveryDate;
                    PurchaseOrder.OrderDate = UpdatePurchaseorder.OrderDate;
                    PurchaseOrder.OrderStatus = UpdatePurchaseorder.OrderStatus;
                    PurchaseOrder.PaymentMethod = UpdatePurchaseorder.PaymentMethod;
                    PurchaseOrder.PaymentStatus = UpdatePurchaseorder.PaymentStatus;
                    PurchaseOrder.DeliveryStatus = UpdatePurchaseorder.DeliveryStatus;
                    PurchaseOrder.IsDeleted = false;
                    PurchaseOrder.CreatedBy = UpdatePurchaseorder.CreatedBy;
                    PurchaseOrder.CreatedOn = DateTime.Now;
                    PurchaseOrder.UpdatedOn = DateTime.Now;
                    PurchaseOrder.UpdatedBy = UpdatePurchaseorder.UpdatedBy;
                };
                Context.TblPurchaseOrderMasters.Update(PurchaseOrder);

                foreach (var item in UpdatePurchaseorder.ProductList)
                {
                    var existingPOProductDetails=Context.TblPurchaseOrderDetails.FirstOrDefault(e=>e.PorefId == PurchaseOrder.Id && e.ProductId == item.ProductId);
                    if (existingPOProductDetails != null)
                    {
                        existingPOProductDetails.PorefId = PurchaseOrder.Id;
                        existingPOProductDetails.ProductId = item.ProductId;
                        existingPOProductDetails.ProductType = item.ProductType;
                        existingPOProductDetails.Hsn = item.Hsn;
                        existingPOProductDetails.Quantity = item.Quantity;
                        existingPOProductDetails.Price = item.Price;
                        existingPOProductDetails.DiscountAmt = item.Discount;
                        existingPOProductDetails.Gstamount = item.Gstamount;
                        existingPOProductDetails.Gstper = item.Gstper;
                        existingPOProductDetails.ProductTotal = item.ProductTotal;
                        existingPOProductDetails.IsDeleted = false;
                        existingPOProductDetails.CreatedBy = UpdatePurchaseorder.CreatedBy;
                        existingPOProductDetails.CreatedOn = DateTime.Now;
                        existingPOProductDetails.UpdatedOn = DateTime.Now;
                        existingPOProductDetails.UpdatedBy = UpdatePurchaseorder.UpdatedBy;

                        Context.TblPurchaseOrderDetails.Update(existingPOProductDetails);
                    }
                    else
                    {
                        var PurchaseOrderDetail = new TblPurchaseOrderDetail()
                        {
                            PorefId = PurchaseOrder.Id,
                            ProductId = item.ProductId,
                            ProductType = item.ProductType,
                            Hsn = item.Hsn,
                            Quantity = item.Quantity,
                            Price = item.Price,
                            DiscountAmt = item.Discount,
                            Gstamount = item.Gstamount,
                            Gstper = item.Gstper,
                            ProductTotal = item.ProductTotal,
                            IsDeleted = false,
                            CreatedBy = UpdatePurchaseorder.CreatedBy,
                            CreatedOn = DateTime.Now,
                        };
                        Context.TblPurchaseOrderDetails.Add(PurchaseOrderDetail);
                    }  
                }

                var POProduct=UpdatePurchaseorder.ProductList.Select(p => p.ProductId).ToList();
                var ProductToRemove = Context.TblPurchaseOrderDetails.Where(e => e.PorefId == PurchaseOrder.Id && !POProduct.Contains(e.ProductId)).ToList();
                Context.TblPurchaseOrderDetails.RemoveRange(ProductToRemove);

                var PurchaseAddress = new TblPodeliveryAddress()
                {
                    Poid = PurchaseOrder.Id,
                    Address = UpdatePurchaseorder.Address,
                    IsDeleted = false
                };
                Context.TblPodeliveryAddresses.Update(PurchaseAddress);

                await Context.SaveChangesAsync();
                response.Code = (int)HttpStatusCode.OK;
                response.Message = "Purchase order successfully inserted.";
            }
            catch (Exception ex)
            {
                response.Code = 400;
                response.Message = "Error in creating purchase orders.";
            }
            return response;
        }
    }
}
