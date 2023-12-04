using Microsoft.AspNetCore.Mvc;

namespace EMPManegment.Web.Controllers
{
    public class OrderMasterController : Controller
    {
       

        public IActionResult CreateOrder()
        {
            return View();
        }
    }
}
