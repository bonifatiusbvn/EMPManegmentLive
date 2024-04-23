﻿using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Diagnostics;

namespace EMPManegment.Web.Controllers
{
    public class PurchaseOrderMasterController : Controller
    {
        public PurchaseOrderMasterController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices)
        {
            WebAPI = webAPI;
            Environment = environment;
            APIServices = aPIServices;
        }

        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }

        [HttpGet]
        public async Task<IActionResult> CreatePurchaseOrder(int? page)
        {
            try
            {
                List<PurchaseOrderDetailView> orderList = new List<PurchaseOrderDetailView>();
                ApiResponseModel res = await APIServices.GetAsync("", "PurchaseOrderDetails/GetPurchaseOrderList");
                if (res.code == 200)
                {
                    orderList = JsonConvert.DeserializeObject<List<PurchaseOrderDetailView>>(res.data.ToString());
                    ViewBag.ordersList = orderList.Count;
                }
                return View(orderList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePurchaseOrder(PurchaseOrderDetailView orderDetail)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync(orderDetail, "PurchaseOrderDetails/CreatePurchaseOrder");
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message });
                }
                return View(orderDetail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetPurchaseOrderDetailsByStatus(string DeliveryStatus)
        {
            try
            {
                List<PurchaseOrderDetailView> orderList = new List<PurchaseOrderDetailView>();
                ApiResponseModel res = await APIServices.PostAsync("", "PurchaseOrderDetails/GetPurchaseOrderDetailsByStatus?DeliveryStatus=" + DeliveryStatus);
                if (res.code == 200)
                {
                    orderList = JsonConvert.DeserializeObject<List<PurchaseOrderDetailView>>(res.data.ToString());
                }
                return PartialView("~/Views/PurchaseOrderMaster/_DeliveryStatusOrder.cshtml", orderList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult ShowProductDetails()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchaseOrderDetailsById(string OrderId)
        {
            try
            {
                List<PurchaseOrderDetailView> order = new List<PurchaseOrderDetailView>();
                ApiResponseModel response = await APIServices.GetAsync("", "PurchaseOrderDetails/GetPurchaseOrderDetailsById?OrderId=" + OrderId);
                if (response.code == 200)
                {
                    order = JsonConvert.DeserializeObject<List<PurchaseOrderDetailView>>(response.data.ToString());
                }
                return View("ShowProductDetails", order);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public async Task<IActionResult> CreatePurchaseOrderView()
        {
            try
            {
                string porjectname = UserSession.ProjectName;
                ApiResponseModel Response = await APIServices.GetAsync("", "PurchaseOrderDetails/CheckPurchaseOrder?projectname=" + porjectname);
                if (Response.code == 200)
                {
                    ViewBag.PoId = Response.data;
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertMultiplePurchaseOrders()
        {
            try
            {
                var OrderDetails = HttpContext.Request.Form["ORDERDETAILS"];
                var InsertDetails = JsonConvert.DeserializeObject<List<PurchaseOrderView>>(OrderDetails.ToString());
                ApiResponseModel postuser = await APIServices.PostAsync(InsertDetails, "PurchaseOrderDetails/InsertMultiplePurchaseOrder");
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message });
                }
                else
                {
                    return Ok(new { postuser.message });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetPaymentMethodList()
        {
            try
            {
                List<PaymentMethodView> PaymentMethod = new List<PaymentMethodView>();
                ApiResponseModel res = await APIServices.GetAsync("", "PurchaseOrderDetails/GetAllPaymentMethod");
                if (res.code == 200)
                {
                    PaymentMethod = JsonConvert.DeserializeObject<List<PaymentMethodView>>(res.data.ToString());
                }
                return new JsonResult(PaymentMethod);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult ShowPaymentDetails()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> EditPurchaseOrderDetails(Guid Id)
        {
            try
            {
                UpdatePurchaseOrderView order = new UpdatePurchaseOrderView();
                ApiResponseModel response = await APIServices.GetAsync("", "PurchaseOrderDetails/EditPurchaseOrderDetails?Id=" + Id);
                if (response.code == 200)
                {
                    order = JsonConvert.DeserializeObject<UpdatePurchaseOrderView>(response.data.ToString());
                }
                return new JsonResult(order);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePurchaseOrderDetails(UpdatePurchaseOrderView orderDetail)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync(orderDetail, "PurchaseOrderDetails/UpdatePurchaseOrderDetails");
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message });
                }
                return View(orderDetail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeletePurchaseOrderDetails(string OrderId)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync(null, "PurchaseOrderDetails/DeletePurchaseOrderDetails?OrderId=" + OrderId);
                if (postuser.code == 200)
                {
                    return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
                }
                else
                {
                    return new JsonResult(new { Message = string.Format(postuser.message), Code = postuser.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}