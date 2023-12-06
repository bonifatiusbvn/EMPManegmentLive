using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
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
    }
}
