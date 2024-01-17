using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using Microsoft.AspNetCore.Mvc;
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
                HttpClient client = WebAPI.Initil();
                ApiResponseModel res = await APIServices.GetAsync("", "OrderDetails/GetOrderList");
                ApiResponseModel Response = await APIServices.GetAsync("", "OrderDetails/CheckOrder");
                if (res.code == 200 || Response.code == 200)
                {
                    ViewBag.OrderId = Response.data;
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
                HttpClient client = WebAPI.Initil();
                ApiResponseModel res = await APIServices.PostAsync("", "OrderDetails/GetOrderDetailsByStatus?DeliveryStatus="+ DeliveryStatus);
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

    }
}
