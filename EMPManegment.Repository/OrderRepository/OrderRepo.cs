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
    public class OrderRepo : IOrderDetails
    {
        public OrderRepo(BonifatiusEmployeesContext context)
        {
            Context = context;
        }

        public BonifatiusEmployeesContext Context { get; }

        public async Task<UserResponceModel> CreateOrder(OrderDetailView CreateOrder)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var ordermodel = new TblOrderMaster()
                {
                    Id = Guid.NewGuid(),
                    OrderId = "Order_" + CreateOrder.OrderId,
                    Type = CreateOrder.Type,
                    CompanyName = CreateOrder.CompanyName,
                    VendorId = CreateOrder.VendorId,
                    ProductType = CreateOrder.Product,
                    Quantity = CreateOrder.Quantity,
                    AmountPerUnit = CreateOrder.AmountPerUnit,
                    TotalAmount = CreateOrder.TotalAmount,
                    OrderDate = CreateOrder.OrderDate,
                    DeliveryDate = CreateOrder.DeliveryDate,
                    PaymentMethod = CreateOrder.PaymentMethod,
                    DeliveryStatus = CreateOrder.DeliveryStatus,
                    CreatedOn = DateTime.Now,
                    CreatedBy = CreateOrder.CreatedBy,
                };
                response.Code = 200;
                response.Message = "Order Created successfully!";
                Context.TblOrderMasters.Add(ordermodel);
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<IEnumerable<OrderDetailView>> GetOrderList()
        {
            try
            {
                var data = await (from a in Context.TblOrderMasters
                                  join b in Context.TblVendorMasters on a.VendorId equals b.Vid
                                  join c in Context.TblProductTypeMasters on a.ProductType equals c.Id
                                  join d in Context.TblPaymentMethodTypes on a.PaymentMethod equals d.Id
                                  select new
                                  {
                                      Order = a,
                                      Vendor = b,
                                      ProductType = c,
                                      PaymentMethod = d
                                  }).ToListAsync();

                var orderList = data.GroupBy(x => x.Order.OrderId)
                                    .Select(group => group.First())
                                    .Select(item => new OrderDetailView
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




        public async Task<List<OrderDetailView>> GetOrderDetailsByStatus(string DeliveryStatus)
        {
            var orderDetails = new List<OrderDetailView>();
            var data = await Context.TblOrderMasters.Where(x => x.DeliveryStatus == DeliveryStatus).ToListAsync();
            if (data != null)
            {
                foreach (var item in data)
                {
                    orderDetails.Add(new OrderDetailView()
                    {
                        ProductId = item.ProjectId,
                        OrderId = item.OrderId,
                        CompanyName = item.CompanyName,
                        Product = item.ProductType,
                        Quantity = item.Quantity,
                        OrderDate = item.OrderDate,
                        TotalAmount = item.TotalAmount,
                        AmountPerUnit = item.AmountPerUnit,
                        PaymentMethod = item.PaymentMethod,
                        DeliveryStatus = item.DeliveryStatus,
                        DeliveryDate = item.DeliveryDate,
                        CreatedOn = item.CreatedOn,
                    });
                }
            }
            return orderDetails;
        }

        public string CheckOrder(string projectname)
        {
            try
            {
                var LastOrder = Context.TblOrderMasters.OrderByDescending(e => e.CreatedOn).FirstOrDefault();
                var currentYear = DateTime.Now.Year;
                var lastYear = currentYear - 1;

                string UserOrderId;
                if (LastOrder == null)
                {
                    UserOrderId = $"BTPL/ODR/{projectname}/{lastYear % 100}-{currentYear % 100}-01";
                }
                else
                {
                    if (LastOrder.OrderId.Length >= 25)
                    {
                        int orderNumber = int.Parse(LastOrder.OrderId.Substring(24)) + 1;
                        UserOrderId = $"BTPL/ODR/{projectname}/{lastYear % 100}-{currentYear % 100}-" + orderNumber.ToString("D3");
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


        public async Task<UserResponceModel> InsertMultipleOrder(List<OrderView> InsertOrder)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                foreach (var item in InsertOrder)
                {
                    var ordermodel = new TblOrderMaster()
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
                    Context.TblOrderMasters.Add(ordermodel);
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

        public async Task<List<OrderDetailView>> GetOrderDetailsById(string OrderId)
        {
            try
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

        public async Task<UpdateOrderView> EditOrderDetails(Guid Id)
        {
            try
            {
                var OrderDetails = new UpdateOrderView();
                var data = Context.TblOrderMasters.Where(x => x.Id == Id).SingleOrDefault();
                if (data != null)
                {

                    OrderDetails = new UpdateOrderView()
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

        public async Task<UserResponceModel> UpdateOrderDetails(UpdateOrderView Updateorder)
        {
            UserResponceModel model = new UserResponceModel();
            var orderdetails = Context.TblOrderMasters.Where(e => e.Id == Updateorder.Id).FirstOrDefault();
            try
            {
                if (orderdetails != null)
                {
                    orderdetails.Id = Updateorder.Id;
                    orderdetails.OrderId = Updateorder.OrderId;
                    orderdetails.OrderDate = Updateorder.OrderDate;
                    orderdetails.CompanyName = Updateorder.CompanyName;
                    orderdetails.ProductName = Updateorder.ProductName;
                    orderdetails.TotalAmount = Updateorder.TotalAmount;
                    orderdetails.PaymentMethod = Updateorder.PaymentMethod;
                    orderdetails.DeliveryStatus = Updateorder.DeliveryStatus;
                    orderdetails.OrderStatus = Updateorder.OrderStatus;
                }
                Context.TblOrderMasters.Update(orderdetails);
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

        public Task<UserResponceModel> DeleteOrderDetails(string OrderId)
        {
            throw new NotImplementedException();
        }
    }
}
