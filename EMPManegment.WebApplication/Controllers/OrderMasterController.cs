using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PdfSharpCore.Pdf.Content.Objects;

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
        public async Task<IActionResult> CreateOrder()
        {
            try
            {
                List<OrderDetailView> orderList = new List<OrderDetailView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel res = await APIServices.GetAsync("", "OrderDetails/GetOrderList");
                if (res.code == 200)
                {
                    orderList = JsonConvert.DeserializeObject<List<OrderDetailView>>(res.data.ToString());
                }
                return View(orderList);
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
                UserResponceModel responseModel = new UserResponceModel();
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

    }
}
