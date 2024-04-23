using EMPManagment.Web.Helper;
using Microsoft.AspNetCore.Mvc;

namespace EMPManegment.Web.Controllers
{
    public class PurchaseRequestController : Controller
    {
        public PurchaseRequestController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices)
        {
            WebAPI = webAPI;
            Environment = environment;
            APIServices = aPIServices;
        }
        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }

        public IActionResult CreatePurchaseRequest()
        {
            return View();
        }
    }
}
