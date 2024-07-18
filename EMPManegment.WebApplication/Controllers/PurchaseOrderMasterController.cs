using EMPManagment.Web.Helper;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Diagnostics;
using X.PagedList;
#nullable disable
namespace EMPManegment.Web.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> PurchaseOrders()
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
        public async Task<IActionResult> CreatePurchaseOrder(Guid? Id)
        {
            try
            {
                PurchaseOrderMasterView order = new PurchaseOrderMasterView();
                if (Id == null)
                {
                    string porjectname = UserSession.ProjectName;
                    ApiResponseModel Response = await APIServices.GetAsync("", "PurchaseOrderDetails/CheckPurchaseOrder?projectname=" + porjectname);
                    if (Response.code == 200)
                    {
                        ViewBag.PoId = Response.data;
                    }
                }
                else
                {
                    ApiResponseModel response = await APIServices.GetAsync("", "PurchaseOrderDetails/EditPurchaseOrderDetails?Id=" + Id);
                    if (response.code == 200)
                    {
                        order = JsonConvert.DeserializeObject<PurchaseOrderMasterView>(response.data.ToString());
                        ViewBag.PoId = order.OrderId;
                        var rowNumber = 0;
                        foreach (var item in order.ProductList)
                        {
                            item.RowNumber = rowNumber++;
                        }
                    }
                }
                return View(order);
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

        [FormPermissionAttribute("Create Purchase Order-Edit")]
        [HttpPost]
        public async Task<IActionResult> UpdatePurchaseOrderDetails()
        {
            try
            {
                var OrderDetails = HttpContext.Request.Form["PurchaseOrder"];
                var PODetails = JsonConvert.DeserializeObject<PurchaseOrderMasterView>(OrderDetails.ToString());
                ApiResponseModel postuser = await APIServices.PostAsync(PODetails, "PurchaseOrderDetails/UpdatePurchaseOrderDetails");
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

        [HttpPost]
        public async Task<IActionResult> DisplayPOProductDetailsListById()
        {
            try
            {
                string ProductId = HttpContext.Request.Form["ProductId"];
                var GetProduct = JsonConvert.DeserializeObject<PurchaseOrderDetailsModel>(ProductId.ToString());
                List<PurchaseOrderDetailsModel> Product = new List<PurchaseOrderDetailsModel>();
                ApiResponseModel response = await APIServices.GetAsync("", "PurchaseOrderDetails/GetPOProductDetailsById?ProductId=" + GetProduct.ProductId);
                if (response.code == 200)
                {
                    Product = JsonConvert.DeserializeObject<List<PurchaseOrderDetailsModel>>(response.data.ToString());
                    Product.ForEach(a => a.ProductTotal = (a.Price) + (a.Gstamount));
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
