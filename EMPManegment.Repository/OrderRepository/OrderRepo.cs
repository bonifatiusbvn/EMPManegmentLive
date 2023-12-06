using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.Inretface.Interface.OrderDetails;
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
                var ordermodel = new OrderMaster()
                {
                   Id = Guid.NewGuid(),
                    OrderId = "Order_" + CreateOrder.OrderId,
                   CompanyName = CreateOrder.CompanyName,
                   Product = CreateOrder.Product,
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
                Context.OrderMasters.Add(ordermodel);
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
            IEnumerable<OrderDetailView> data = Context.OrderMasters.ToList().Select(a => new OrderDetailView
            {
                OrderId = a.OrderId,
                CompanyName = a.CompanyName,
                Product = a.Product,    
                Quantity = a.Quantity,
                OrderDate = a.OrderDate,
                Total = a.Total,
                Amount= a.Amount,
                PaymentMethod = a.PaymentMethod,
                DeliveryStatus = a.DeliveryStatus,
                DeliveryDate = a.DeliveryDate,
                CreatedOn= a.CreatedOn,
            });
            return data;
        }
    }
}
