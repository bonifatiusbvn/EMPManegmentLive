﻿using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.PurchaseOrderModels;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.Web.Helper;
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

        [FormPermissionAttribute("Purchase Order List -View")]
        [HttpGet]
        public async Task<IActionResult> PurchaseOrders(int? page)
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

        [FormPermissionAttribute("Create Purchase Order-Add")]
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

        [FormPermissionAttribute("ShowProductDetails-View")]
        [HttpGet]
        public async Task<IActionResult> PurchaseOrderDetails(string OrderId)
        {
            try
            {
                PurchaseOrderMasterView order = new PurchaseOrderMasterView();
                ApiResponseModel response = await APIServices.GetAsync("", "PurchaseOrderDetails/GetPurchaseOrderDetailsByOrderId?OrderId=" + OrderId);
                if (response.code == 200)
                {
                    order = JsonConvert.DeserializeObject<PurchaseOrderMasterView>(response.data.ToString());
                }
                return View(order);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [FormPermissionAttribute("Create Purchase Order-View")]
        [HttpGet]
        public async Task<IActionResult> CreatePurchaseOrder()
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

        [FormPermissionAttribute("Create Purchase Order-Add")]
        [HttpPost]
        public async Task<IActionResult> InsertMultiplePurchaseOrderDetails()
        {
            try
            {
                var OrderDetails = HttpContext.Request.Form["PurchaseOrder"];
                var PODetails = JsonConvert.DeserializeObject<PurchaseOrderMasterView>(OrderDetails.ToString());
                ApiResponseModel postuser = await APIServices.PostAsync(PODetails, "PurchaseOrderDetails/InsertMultiplePurchaseOrder");
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message, postuser.code });
                }
                else
                {
                    return Ok(new { postuser.message, postuser.code });
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

        [FormPermissionAttribute("Show Payment Details-View")]
        public IActionResult PaymentDetails()
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

        [FormPermissionAttribute("Create Purchase Order-Edit")]
        [HttpPost]
        public async Task<IActionResult> UpdatePurchaseOrderDetails(UpdatePurchaseOrderView orderDetail)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync(orderDetail, "PurchaseOrderDetails/UpdatePurchaseOrderDetails");
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message, postuser.code });
                }
                else
                {
                    return Ok(new { postuser.message, postuser.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [FormPermissionAttribute("Create Purchase Order-Delete")]
        [HttpPost]
        public async Task<IActionResult> DeletePurchaseOrderDetails(Guid Id)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync("", "PurchaseOrderDetails/DeletePurchaseOrderDetails?Id=" + Id);
                if (postuser.code == 200)
                {
                    return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
                }
                else
                {
                    return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> GetAllPOProductList(string? searchText)
        {
            try
            {
                string apiUrl = $"ProductMaster/GetAllProductList?searchText={searchText}";
                ApiResponseModel response = await APIServices.PostAsync("", apiUrl);
                if (response.code == 200)
                {
                    List<ProductDetailsView> Items = JsonConvert.DeserializeObject<List<ProductDetailsView>>(response.data.ToString());
                    return PartialView("~/Views/PurchaseOrderMaster/_showAllPOProductsPartial.cshtml", Items);
                }
                else
                {
                    return Ok(new { Message = "Failed to retrieve Product list" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> DisplayPOProductDetailsListById()
        {
            try
            {
                string ProductId = HttpContext.Request.Form["ProductId"];
                var GetProduct = JsonConvert.DeserializeObject<InvoiceDetailsViewModel>(ProductId.ToString());
                List<InvoiceDetailsViewModel> Product = new List<InvoiceDetailsViewModel>();
                ApiResponseModel response = await APIServices.GetAsync("", "Invoice/GetProductDetailsById?ProductId=" + GetProduct.ProductId);
                if (response.code == 200)
                {
                    Product = JsonConvert.DeserializeObject<List<InvoiceDetailsViewModel>>(response.data.ToString());
                    Product.ForEach(a => a.ProductTotal = (a.PerUnitPrice ?? 0) + (a.PerUnitWithGstprice ?? 0));
                }
                return PartialView("~/Views/PurchaseOrderMaster/_DisplayPOProductDetailsById.cshtml", Product);
            }
            catch (Exception ex)
            {
                throw ex;   
            }
        }
    }
}
