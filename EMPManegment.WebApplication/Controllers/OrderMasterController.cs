using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using PdfSharpCore.Pdf.Content.Objects;
using System.Diagnostics;

namespace EMPManegment.Web.Controllers
{
    public class OrderMasterController : Controller
    {
        public OrderMasterController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices)
        {
            WebAPI = webAPI;
            Environment = environment;
            APIServices = aPIServices;
        }

        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }

        [HttpGet]
        public async Task<IActionResult> CreateOrder(int? page)
        {
            try
            {
                List<OrderDetailView> orderList = new List<OrderDetailView>();
                ApiResponseModel res = await APIServices.GetAsync("", "OrderDetails/GetOrderList");
                if (res.code == 200)
                {
                    orderList = JsonConvert.DeserializeObject<List<OrderDetailView>>(res.data.ToString());
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
        public async Task<IActionResult> GetOrderDetailsByStatus(string DeliveryStatus)
        {
            try
            {
                List<OrderDetailView> orderList = new List<OrderDetailView>();
                ApiResponseModel res = await APIServices.PostAsync("", "OrderDetails/GetOrderDetailsByStatus?DeliveryStatus=" + DeliveryStatus);
                if (res.code == 200)
                {
                    orderList = JsonConvert.DeserializeObject<List<OrderDetailView>>(res.data.ToString());
                }
                return PartialView("~/Views/OrderMaster/_DeliveryStatusOrder.cshtml", orderList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDetailView orderDetail)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync(orderDetail, "OrderDetails/CreateOrder");
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

        public IActionResult ShowProductDetails()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderDetailsById(string OrderId)
        {
            try
            {
                OrderDetailView order = new OrderDetailView();
                ApiResponseModel response = await APIServices.GetAsync("", "OrderDetails/GetOrderDetailsById?OrderId=" + OrderId);
                if (response.code == 200)
                {
                    order = JsonConvert.DeserializeObject<OrderDetailView>(response.data.ToString());
                }
                return View("ShowProductDetails", order);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public async Task<IActionResult> CreateOrderView()
        {
            try
            {
                ApiResponseModel Response = await APIServices.GetAsync("", "OrderDetails/CheckOrder");
                if (Response.code == 200)
                {
                    ViewBag.OrderId = Response.data;
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertMultipleOrders()
        {
            try
            {
                var OrderDetails = HttpContext.Request.Form["ORDERDETAILS"];
                var InsertDetails = JsonConvert.DeserializeObject<List<OrderView>>(OrderDetails);
                ApiResponseModel postuser = await APIServices.PostAsync(InsertDetails, "OrderDetails/InsertMultipleOrder");
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
    }
}
