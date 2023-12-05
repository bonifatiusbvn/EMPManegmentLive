using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Services.OrderDetails
{
    public interface IOrderDetailsServices
    {
        Task<UserResponceModel> CreateOrder(OrderDetailView CreateOrder);
        Task<IEnumerable<OrderDetailView>> GetOrderList();
    }
}
