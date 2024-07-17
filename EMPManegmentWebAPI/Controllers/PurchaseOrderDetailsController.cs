﻿using Azure;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.PurchaseOrderModels;
using EMPManegment.Inretface.Interface.ExpenseMaster;
using EMPManegment.Inretface.Interface.InvoiceMaster;
using EMPManegment.Inretface.Interface.OrderDetails;
using EMPManegment.Inretface.Interface.ProductMaster;
using EMPManegment.Inretface.Services.OrderDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
#nullable disable
namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PurchaseOrderDetailsController : ControllerBase
    {
        public PurchaseOrderDetailsController(IPurchaseOrderDetailsServices purchaseOrderDetails)
        {
            PurchaseOrderDetails = purchaseOrderDetails;
        }

        public IPurchaseOrderDetailsServices PurchaseOrderDetails { get; }

        [HttpGet]
        [Route("GetPurchaseOrderList")]
        public async Task<IActionResult> GetPurchaseOrderList()
        {
            IEnumerable<PurchaseOrderDetailView> orderlist = await PurchaseOrderDetails.GetPurchaseOrderList();
            return Ok(new { code = (int)HttpStatusCode.OK, data = orderlist });

        }
        [HttpPost]
        [Route("GetPurchaseOrderDetailsByStatus")]
        public async Task<IActionResult> GetPurchaseOrderDetailsByStatus(string DeliveryStatus)
        {
            List<PurchaseOrderDetailView> orderdetails = await PurchaseOrderDetails.GetPurchaseOrderDetailsByStatus(DeliveryStatus);
            return Ok(new { code = (int)HttpStatusCode.OK, data = orderdetails.ToList() });
        }

        [HttpGet]
        [Route("CheckPurchaseOrder")]
        public async Task<IActionResult> CheckPurchaseOrder(string projectname)
        {
            try
            {
                var checkOrder = PurchaseOrderDetails.CheckPurchaseOrder(projectname);
                return Ok(new { code = (int)HttpStatusCode.OK, data = checkOrder });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpGet]
        [Route("GetPurchaseOrderDetailsByOrderId")]
        public async Task<IActionResult> GetPurchaseOrderDetailsByOrderId(string OrderId)
        {
            var orderdetails = await PurchaseOrderDetails.GetPurchaseOrderDetailsByOrderId(OrderId);
            return Ok(new { code = (int)HttpStatusCode.OK, data = orderdetails });
        }

        [HttpPost]
        [Route("InsertMultiplePurchaseOrder")]
        public async Task<IActionResult> InsertMultiplePurchaseOrder(PurchaseOrderMasterView PODetails)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
                var createOrder = await PurchaseOrderDetails.InsertMultiplePurchaseOrder(PODetails);
                if (createOrder.code != (int)HttpStatusCode.InternalServerError)
                {
                    response.code = (int)HttpStatusCode.OK;
                    response.message = createOrder.message;
                }
                else
                {
                    response.message = createOrder.message;
                    response.code = createOrder.code;
                }
            }
            catch (Exception ex)
            {
                response.code = (int)HttpStatusCode.InternalServerError;
                response.message = "An error occurred while processing the request.";
            }
            return StatusCode(response.code, response);
        }

        [HttpGet]
        [Route("GetAllPaymentMethod")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPaymentMethod()
        {
            IEnumerable<PaymentMethodView> paymentmethod = await PurchaseOrderDetails.GetAllPaymentMethod();
            return Ok(new { code = (int)HttpStatusCode.OK, data = paymentmethod.ToList() });
        }

        [HttpGet]
        [Route("EditPurchaseOrderDetails")]
        public async Task<IActionResult> EditPurchaseOrderDetails(Guid Id)
        {
            var getorderDetails = await PurchaseOrderDetails.EditPurchaseOrderDetails(Id);
            return Ok(new { code = (int)HttpStatusCode.OK, data = getorderDetails });
        }

        [HttpPost]
        [Route("UpdatePurchaseOrderDetails")]
        public async Task<IActionResult> UpdatePurchaseOrderDetails(PurchaseOrderMasterView PurchaseorderDetails)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var updateorder = PurchaseOrderDetails.UpdatePurchaseOrderDetails(PurchaseorderDetails);
                if (updateorder.Result.Code != (int)HttpStatusCode.InternalServerError)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = updateorder.Result.Message;
                }
                else
                {
                    response.Message = updateorder.Result.Message;
                    response.Code = updateorder.Result.Code;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "An error occurred while processing the request.";
            }
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("DeletePurchaseOrderDetails")]
        public async Task<IActionResult> DeletePurchaseOrderDetails(Guid Id)
        {
            UserResponceModel responseModel = new UserResponceModel();
            var order = await PurchaseOrderDetails.DeletePurchaseOrderDetails(Id);
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
                    responseModel.Code = order.Code;
                }
            }
            catch (Exception ex)
            {
                responseModel.Code = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = "An error occurred while processing the request.";
            }
            return StatusCode(responseModel.Code, responseModel);
        }
        [HttpGet]
        [Route("GetPOProductDetailsById")]
        public async Task<IActionResult> GetPOProductDetailsById(Guid ProductId)
        {
            var getorderDetails = await PurchaseOrderDetails.GetPOProductDetailsById(ProductId);
            return Ok(new { code = (int)HttpStatusCode.OK, data = getorderDetails });
        }
    }
}
