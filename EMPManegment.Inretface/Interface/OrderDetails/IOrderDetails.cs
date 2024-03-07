using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Interface.OrderDetails
{
    public interface IOrderDetails
    {
        Task<UserResponceModel> CreateOrder(OrderDetailView CreateOrder);
        Task<IEnumerable<OrderDetailView>> GetOrderList();
        Task<List<OrderDetailView>> GetOrderDetailsByStatus(string DeliveryStatus);
        string CheckOrder(string projectname);
        Task<List<OrderDetailView>> GetOrderDetailsById(string OrderId);
        Task<UserResponceModel> InsertMultipleOrder(List<OrderView> InsertOrder);
        Task<IEnumerable<PaymentMethodView>> GetAllPaymentMethod();
        Task<UpdateOrderView> EditOrderDetails(Guid Id);
        Task<UserResponceModel> UpdateOrderDetails(UpdateOrderView Updateorder);
        Task<UserResponceModel> DeleteOrderDetails(string OrderId);
    }
}
