using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels;
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
using System.Linq;
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
                    Amount = CreateOrder.Amount,
                    Total = CreateOrder.Total,
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
            IEnumerable<OrderDetailView> data = from a in Context.TblOrderMasters
                                                join b in Context.TblVendorMasters on a.VendorId equals b.Vid
                                                join c in Context.TblProductTypeMasters on a.ProductType equals c.Id
                                                select new OrderDetailView
                                                {
                                                    OrderId = a.OrderId,
                                                    ProductId = a.ProductId,
                                                    CompanyName = b.VendorCompany,
                                                    VendorId = a.VendorId,
                                                    ProductName = c.Type,
                                                    Quantity = a.Quantity,
                                                    OrderDate = a.OrderDate,
                                                    Total = a.Total,
                                                    Amount = a.Amount,
                                                    PaymentMethod = a.PaymentMethod,
                                                    DeliveryStatus = a.DeliveryStatus,
                                                    DeliveryDate = a.DeliveryDate,
                                                    CreatedOn = a.CreatedOn,
                                                };
            return data;
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
                        Total = item.Total,
                        Amount = item.Amount,
                        PaymentMethod = item.PaymentMethod,
                        DeliveryStatus = item.DeliveryStatus,
                        DeliveryDate = item.DeliveryDate,
                        CreatedOn = item.CreatedOn,
                    });
                }
            }
            return orderDetails;
        }

        public string CheckOrder()
        {
            try
            {
                var LastOrder = Context.TblOrderMasters.OrderByDescending(e => e.CreatedOn).FirstOrDefault();
                string UserOrderId;
                if (LastOrder == null)
                {
                    UserOrderId = "BTPL/PO/PROJ-01/23-24-001";
                }
                else
                {
                    if (LastOrder.OrderId.Length >= 25)
                    {
                        int orderNumber = int.Parse(LastOrder.OrderId.Substring(24)) + 1;
                        UserOrderId = "BTPL/PO/PROJ-01/23-24-" + orderNumber.ToString("D3");
                    }
                    else
                    {
                        throw new Exception("OrderId does not have expected format.");
                    }
                }
                return UserOrderId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<OrderDetailView>> GetOrderDetailsById(string OrderId)
        {
            try
            {
                var orderDetails = new List<OrderDetailView>();
                var data = await (from a in Context.TblOrderMasters
                                  join c in Context.TblProductDetailsMasters on a.ProductId equals c.Id
                                  join b in Context.TblVendorMasters on a.VendorId equals b.Vid
                                  where a.OrderId==OrderId
                                  select new OrderDetailView
                                  {
                                      Id = a.Id,
                                      OrderId = a.OrderId,
                                      VendorId = a.VendorId,
                                      Type = a.Type,
                                      CompanyName = a.CompanyName,
                                      ProductId = a.ProductId,
                                      VendorAddress=b.VendorAddress,
                                      VendorContact=b.VendorContact,
                                      VendorEmail=b.VendorCompanyEmail,
                                      ProductImage = c.ProductImage,
                                      ProductName = a.ProductName,
                                      ProductShortDescription = a.ProductShortDescription,
                                      Quantity = a.Quantity,
                                      OrderDate = a.OrderDate,
                                      PerUnitPrice=c.PerUnitPrice,
                                      PerUnitWithGstprice=c.PerUnitWithGstprice,
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
                }
                return orderDetails;
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
                        Quantity = item.Quantity,
                        Amount = item.Amount,
                        Total = item.Total,
                        OrderDate = item.OrderDate,
                        DeliveryDate = item.DeliveryDate,
                        PaymentMethod = item.PaymentMethod,
                        PaymentStatus = item.PaymentStatus,
                        DeliveryStatus = item.DeliveryStatus,
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
    }
}
