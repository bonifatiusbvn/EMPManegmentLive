﻿using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.Inretface.Interface.OrderDetails;
using EMPManegment.Inretface.Services.OrderDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.OrderDetails
{
    public class OrderDetailsServices : IOrderDetailsServices
    {
        public OrderDetailsServices(IOrderDetails orderDetails) 
        {
            OrderDetails = orderDetails;
        }

        public IOrderDetails OrderDetails { get; }

        public async Task<UserResponceModel> CreateOrder(OrderDetailView CreateOrder)
        {
            return await OrderDetails.CreateOrder(CreateOrder);
        }
        public async Task<IEnumerable<OrderDetailView>> GetOrderList()
        {
            return await OrderDetails.GetOrderList();
        }
        public async Task<List<OrderDetailView>> GetOrderDetailsByStatus(string DeliveryStatus)
        {
            return await OrderDetails.GetOrderDetailsByStatus(DeliveryStatus);
        }

        public string CheckOrder()
        {
            return OrderDetails.CheckOrder();
        }

        public async Task<OrderDetailView> GetOrderDetailsById(string OrderId)
        {
            return await OrderDetails.GetOrderDetailsById(OrderId);
        }

        public async Task<UserResponceModel> InsertMultipleOrder(List<OrderView> InsertOrder)
        {
            return await OrderDetails.InsertMultipleOrder(InsertOrder);
        }
    }
}
