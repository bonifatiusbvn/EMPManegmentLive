﻿using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.PurchaseOrderModels;
using EMPManegment.Inretface.EmployeesInterface.AddEmployee;
using EMPManegment.Inretface.Interface.ExpenseMaster;
using EMPManegment.Inretface.Interface.InvoiceMaster;
using EMPManegment.Inretface.Interface.OrderDetails;
using EMPManegment.Inretface.Interface.ProductMaster;
using EMPManegment.Inretface.Services.OrderDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrderDetailsController : ControllerBase
    {
        public PurchaseOrderDetailsController(IPurchaseOrderDetailsServices purchaseOrderDetails)
        {
            PurchaseOrderDetails = purchaseOrderDetails;
        }

        public IPurchaseOrderDetailsServices PurchaseOrderDetails { get; }

        [HttpPost]
        [Route("CreatePurchaseOrder")]
        public async Task<IActionResult> CreatePurchaseOrder(PurchaseOrderDetailView orderDetails)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var createOrder = PurchaseOrderDetails.CreatePurchaseOrder(orderDetails);
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
        [Route("GetPurchaseOrderList")]
        public async Task<IActionResult> GetPurchaseOrderList()
        {
            IEnumerable<PurchaseOrderDetailView> orderlist = await PurchaseOrderDetails.GetPurchaseOrderList();
            return Ok(new { code = 200, data = orderlist });

        }
        [HttpPost]
        [Route("GetPurchaseOrderDetailsByStatus")]
        public async Task<IActionResult> GetPurchaseOrderDetailsByStatus(string DeliveryStatus)
        {
            List<PurchaseOrderDetailView> orderdetails = await PurchaseOrderDetails.GetPurchaseOrderDetailsByStatus(DeliveryStatus);
            return Ok(new { code = 200, data = orderdetails.ToList() });
        }

        [HttpGet]
        [Route("CheckPurchaseOrder")]
        public async Task<IActionResult> CheckPurchaseOrder(string projectname)
        {
            try
            {
                var checkOrder = PurchaseOrderDetails.CheckPurchaseOrder(projectname);
                return Ok(new { code = 200, data = checkOrder });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetPurchaseOrderDetailsById")]
        public async Task<IActionResult> GetPurchaseOrderDetailsById(string OrderId)
        {
            List<PurchaseOrderDetailView> orderdetails = await PurchaseOrderDetails.GetPurchaseOrderDetailsById(OrderId);
            return Ok(new { code = 200, data = orderdetails });
        }
        [HttpPost]
        [Route("InsertMultiplePurchaseOrder")]
        public async Task<IActionResult> InsertMultiplePurchaseOrder(PurchaseOrderMasterView orderDetails)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var createOrder = PurchaseOrderDetails.InsertMultiplePurchaseOrder(orderDetails);
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
            IEnumerable<PaymentMethodView> paymentmethod = await PurchaseOrderDetails.GetAllPaymentMethod();
            return Ok(new { code = 200, data = paymentmethod.ToList() });
        }
        [HttpGet]
        [Route("EditPurchaseOrderDetails")]
        public async Task<IActionResult> EditPurchaseOrderDetails(Guid Id)
        {
            var getorderDetails = await PurchaseOrderDetails.EditPurchaseOrderDetails(Id);
            return Ok(new { code = 200, data = getorderDetails });
        }
        [HttpPost]
        [Route("UpdatePurchaseOrderDetails")]
        public async Task<IActionResult> UpdatePurchaseOrderDetails(UpdatePurchaseOrderView PurchaseorderDetails)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var updateorder = PurchaseOrderDetails.UpdatePurchaseOrderDetails(PurchaseorderDetails);
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
        [Route("DeletePurchaseOrderDetails")]
        public async Task<IActionResult> DeletePurchaseOrderDetails(string OrderId)
        {
            UserResponceModel responseModel = new UserResponceModel();
            var order = await PurchaseOrderDetails.DeletePurchaseOrderDetails(OrderId);
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
