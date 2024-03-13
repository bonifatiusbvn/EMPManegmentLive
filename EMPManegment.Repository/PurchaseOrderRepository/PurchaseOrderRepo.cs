using Azure;
using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
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

namespace EMPManegment.Repository.OrderRepository
{
    public class PurchaseOrderRepo : IPurchaseOrderDetails
    {
        public PurchaseOrderRepo(BonifatiusEmployeesContext context)
        {
            Context = context;
        }

        public BonifatiusEmployeesContext Context { get; }


        public async Task<UserResponceModel> CreatePurchaseOrder(PurchaseOrderDetailView CreatePurchaseOrder)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var ordermodel = new TblPurchaseOrderMaster()
                var LastOrder = Context.TblPurchaseOrders.OrderByDescending(e => e.CreatedOn).FirstOrDefault();
                var currentYear = DateTime.Now.Year;
                var lastYear = currentYear - 1;

                string POId;
                if (LastOrder == null)
                {
                    Id = Guid.NewGuid(),
                    OrderId = "Order_" + CreatePurchaseOrder.OrderId,
                    Type = CreatePurchaseOrder.Type,
                    CompanyName = CreatePurchaseOrder.CompanyName,
                    VendorId = CreatePurchaseOrder.VendorId,
                    ProductType = CreatePurchaseOrder.Product,
                    Quantity = CreatePurchaseOrder.Quantity,
                    AmountPerUnit = CreatePurchaseOrder.AmountPerUnit,
                    TotalAmount = CreatePurchaseOrder.TotalAmount,
                    OrderDate = CreatePurchaseOrder.OrderDate,
                    DeliveryDate = CreatePurchaseOrder.DeliveryDate,
                    PaymentMethod = CreatePurchaseOrder.PaymentMethod,
                    DeliveryStatus = CreatePurchaseOrder.DeliveryStatus,
                    CreatedOn = DateTime.Now,
                    CreatedBy = CreatePurchaseOrder.CreatedBy,
                };
                response.Code = 200;
                response.Message = "Order Created successfully!";
                Context.TblPurchaseOrderMasters.Add(ordermodel);
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<IEnumerable<PurchaseOrderDetailView>> GetPurchaseOrderList()
        {
            try
            {
                var data = await (from a in Context.TblPurchaseOrderMasters
                                  join b in Context.TblVendorMasters on a.VendorId equals b.Vid
                                  join c in Context.TblProductTypeMasters on a.ProductType equals c.Id
                                  join d in Context.TblPaymentMethodTypes on a.PaymentMethod equals d.Id
                                  where a.IsDeleted != false
                                  select new
                                  {
                                      Order = a,
                                      Vendor = b,
                                      ProductType = c,
                                      PaymentMethod = d
                                  }).ToListAsync();

                var orderList = data.GroupBy(x => x.Order.OrderId)
                                    .Select(group => group.First())
                                    .Select(item => new PurchaseOrderDetailView
                                    {
                                        Id = item.Order.Id,
                                        OrderId = item.Order.OrderId,
                                        ProductId = item.Order.ProductId,
                                        CompanyName = item.Vendor.VendorCompany,
                                        VendorId = item.Order.VendorId,
                                        ProductName = item.ProductType.Type,
                                        Quantity = item.Order.Quantity,
                                        OrderDate = item.Order.OrderDate,
                                        TotalAmount = item.Order.TotalAmount,
                                        AmountPerUnit = item.Order.AmountPerUnit,
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
                                  join c in Context.TblProductTypeMasters on a.ProductType equals c.Id
                                  join d in Context.TblPaymentMethodTypes on a.PaymentMethod equals d.Id
                                  where a.IsDeleted != false && a.DeliveryStatus == DeliveryStatus
                                  select new
                                  {
                                      Order = a,
                                      Vendor = b,
                                      ProductType = c,
                                      PaymentMethod = d
                                  }).ToListAsync();

                var orderList = data.Select(item => new PurchaseOrderDetailView
                {
                    Id = item.Order.Id,
                    OrderId = item.Order.OrderId,
                    ProductId = item.Order.ProductId,
                    CompanyName = item.Vendor.VendorCompany,
                    VendorId = item.Order.VendorId,
                    ProductName = item.ProductType.Type,
                    Quantity = item.Order.Quantity,
                    OrderDate = item.Order.OrderDate,
                    TotalAmount = item.Order.TotalAmount,
                    AmountPerUnit = item.Order.AmountPerUnit,
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

                string UserOrderId;
                if (LastOrder == null)
                {
                    UserOrderId = $"BTPL/PO/{projectname}/{lastYear % 100}-{currentYear % 100}-001";
                }
                else
                {
                    if (LastOrder.OrderId.Length >= 25)
                    {
                        int orderNumber = int.Parse(LastOrder.OrderId.Substring(24)) + 1;
                        UserOrderId = $"BTPL/PO/{projectname}/{lastYear % 100}-{currentYear % 100}-" + orderNumber.ToString("D3");
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


        public async Task<UserResponceModel> InsertMultiplePurchaseOrder(List<PurchaseOrderView> InsertPurchaseOrder)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                foreach (var item in InsertPurchaseOrder)
                {
                    var ordermodel = new TblPurchaseOrderMaster()
                    {
                        Id = Guid.NewGuid(),
                        OrderId = item.OrderId,
                        Type = item.Type,
                        CompanyName = item.CompanyName,
                        VendorId = item.VendorId,
                        ProductType = item.ProductType,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        AmountPerUnit = item.AmountPerUnit,
                        TotalGst = item.TotalGst,
                        GstPerUnit = item.GstPerUnit,
                        SubTotal = item.SubTotal,
                        TotalAmount = item.TotalAmount,
                        OrderDate = item.OrderDate,
                        DeliveryDate = item.DeliveryDate,
                        PaymentMethod = item.PaymentMethod,
                        PaymentStatus = item.PaymentStatus,
                        DeliveryStatus = "Pending",
                        OrderStatus = "Confirmed",
                        CreatedOn = DateTime.Now,
                        CreatedBy = item.CreatedBy,
                        ProjectId = item.ProjectId,
                        ProductName = item.ProductName,
                        ProductShortDescription = item.ProductShortDescription,
                    };
                    Context.TblPurchaseOrderMasters.Add(ordermodel);
                }

                await Context.SaveChangesAsync();
                response.Code = 200;
                response.Message = "Orders Created successfully!";
            }
            catch (Exception ex)
            {
                response.Code = 500;
                response.Message = "Error creating orders: " + ex.Message;
            }
            return response;
        }

        public async Task<List<PurchaseOrderDetailView>> GetPurchaseOrderDetailsById(string OrderId)
        {
            try
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
                                      SubTotal = a.SubTotal,
                                      TotalGst = a.TotalGst,
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
                            SubTotal = item.SubTotal,
                            TotalGst = item.TotalGst,
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
                }
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

        public async Task<UpdatePurchaseOrderView> EditPurchaseOrderDetails(Guid Id)
        {
            try
            {
                var OrderDetails = new UpdatePurchaseOrderView();
                var data = Context.TblPurchaseOrderMasters.Where(x => x.Id == Id).SingleOrDefault();
                if (data != null)
                {

                    OrderDetails = new UpdatePurchaseOrderView()
                    {
                        Id = data.Id,
                        OrderId = data.OrderId,
                        OrderDate = data.OrderDate,
                        CompanyName = data.CompanyName,
                        ProductName = data.ProductName,
                        TotalAmount = data.TotalAmount,
                        PaymentMethod = data.PaymentMethod,
                        DeliveryStatus = data.DeliveryStatus,
                        OrderStatus = data.OrderStatus,
                    };
                }

                return OrderDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserResponceModel> UpdatePurchaseOrderDetails(UpdatePurchaseOrderView UpdatePurchaseorder)
        {
            UserResponceModel model = new UserResponceModel();
            var orderdetails = Context.TblPurchaseOrderMasters.Where(e => e.Id == UpdatePurchaseorder.Id).FirstOrDefault();
            try
            {
                if (orderdetails != null)
                {
                    orderdetails.Id = UpdatePurchaseorder.Id;
                    orderdetails.OrderDate = UpdatePurchaseorder.OrderDate;
                    orderdetails.CompanyName = UpdatePurchaseorder.CompanyName;
                    orderdetails.ProductName = UpdatePurchaseorder.ProductName;
                    orderdetails.TotalAmount = UpdatePurchaseorder.TotalAmount;
                    orderdetails.PaymentMethod = UpdatePurchaseorder.PaymentMethod;
                    orderdetails.DeliveryStatus = UpdatePurchaseorder.DeliveryStatus;
                    orderdetails.OrderStatus = UpdatePurchaseorder.OrderStatus;
                }
                Context.TblPurchaseOrderMasters.Update(orderdetails);
                Context.SaveChanges();
                model.Code = 200;
                model.Message = "Order Details Updated Successfully!";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public async Task<UserResponceModel> DeletePurchaseOrderDetails(string OrderId)
        {
            {
                UserResponceModel response = new UserResponceModel();
                var GetOrderdata = Context.TblPurchaseOrderMasters.Where(a => a.OrderId == OrderId).FirstOrDefault();

                if (GetOrderdata != null)
                {
                    GetOrderdata.IsDeleted = false;
                    Context.TblPurchaseOrderMasters.Update(GetOrderdata);
                    Context.SaveChanges();
                    response.Code = 200;
                    response.Data = GetOrderdata;
                    response.Message = "Order is Deleted Successfully";
                }
                return response;
            }
        }


    }
}
