using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.Inretface.EmployeesInterface.AddEmployee;
using EMPManegment.Inretface.Interface.ExpenseMaster;
using EMPManegment.Inretface.Interface.InvoiceMaster;
using EMPManegment.Inretface.Interface.ProductMaster;
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
            return Ok(new { code = 200, data = orderlist });

        }
        [HttpPost]
        [Route("GetOrderDetailsByStatus")]
        public async Task<IActionResult> GetOrderDetailsByStatus(string DeliveryStatus)
        {
            List<OrderDetailView> orderdetails = await OrderDetails.GetOrderDetailsByStatus(DeliveryStatus);
            return Ok(new { code = 200, data = orderdetails.ToList() });
        }

        [HttpGet]
        [Route("CheckOrder")]
        public async Task<IActionResult> CheckOrder(string projectname)
        {
            try
            {
                var checkOrder = OrderDetails.CheckOrder(projectname);
                return Ok(new { code = 200, data = checkOrder });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetOrderDetailsById")]
        public async Task<IActionResult> GetOrderDetailsById(string OrderId)
        {
            List<OrderDetailView> orderdetails = await OrderDetails.GetOrderDetailsById(OrderId);
            return Ok(new { code = 200, data = orderdetails });
        }
        [HttpPost]
        [Route("InsertMultipleOrder")]
        public async Task<IActionResult> InsertMultipleOrder(List<OrderView> orderDetails)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var createOrder = OrderDetails.InsertMultipleOrder(orderDetails);
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
        [Route("GetAllPaymentMethod")]
        public async Task<IActionResult> GetAllPaymentMethod()
        {
            IEnumerable<PaymentMethodView> paymentmethod = await OrderDetails.GetAllPaymentMethod();
            return Ok(new { code = 200, data = paymentmethod.ToList() });
        }
        [HttpGet]
        [Route("EditOrderDetails")]
        public async Task<IActionResult> EditOrderDetails(Guid Id)
        {
            var getorderDetails = await OrderDetails.EditOrderDetails(Id);
            return Ok(new { code = 200, data = getorderDetails });
        }
        [HttpPost]
        [Route("UpdateOrderDetails")]
        public async Task<IActionResult> UpdateOrderDetails(UpdateOrderView orderDetails)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var updateorder = OrderDetails.UpdateOrderDetails(orderDetails);
                if (updateorder.Result.Code == 200)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = updateorder.Result.Message;
                }
                else
                {
                    response.Message = updateorder.Result.Message;
                    response.Code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StatusCode(response.Code, response);
        }
        [HttpPost]
        [Route("DeleteOrderDetails")]
        public async Task<IActionResult> DeleteOrderDetails(string OrderId)
        {
            UserResponceModel responseModel = new UserResponceModel();
            var order = await OrderDetails.DeleteOrderDetails(OrderId);
            try
            {
                if (order != null)
                {
                    responseModel.Code = (int)HttpStatusCode.OK;
                    responseModel.Message = order.Message;
                }
                else
                {
                    responseModel.Message = order.Message;
                    responseModel.Code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                responseModel.Code = (int)HttpStatusCode.InternalServerError;
            }
            return StatusCode(responseModel.Code, responseModel);
        }
    }
}
