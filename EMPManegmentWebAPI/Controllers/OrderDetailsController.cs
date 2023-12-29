﻿using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.Inretface.Services.OrderDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        public OrderDetailsController(IOrderDetailsServices orderDetails)
        {
            OrderDetails = orderDetails;
        }

        public IOrderDetailsServices OrderDetails { get; }

        [HttpPost]
        [Route("CreateOrder")]
        public async Task<IActionResult> CreateOrder(OrderDetailView orderDetails)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var createOrder = OrderDetails.CreateOrder(orderDetails);
                if (createOrder.Result.Code == 200)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = createOrder.Result.Message;
                }
                else
                {
                    response.Message = createOrder.Result.Message;
                    response.Code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Route("GetOrderList")]
        public async Task<IActionResult> GetOrderList()
        {
            IEnumerable<OrderDetailView> orderlist = await OrderDetails.GetOrderList();
            return Ok(new { code = 200, data = orderlist.ToList() });
         
        }
        [HttpPost]
        [Route("GetOrderDetailsByStatus")]
        public async Task<IActionResult> GetOrderDetailsByStatus(string DeliveryStatus)
        {
            List<OrderDetailView> orderdetails = await OrderDetails.GetOrderDetailsByStatus(DeliveryStatus);
            return Ok(new { code = 200,data = orderdetails.ToList() });
        }
    }
}