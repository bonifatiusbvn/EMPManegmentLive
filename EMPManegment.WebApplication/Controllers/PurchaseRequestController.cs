using Microsoft.AspNetCore.Mvc;

namespace EMPManegment.Web.Controllers
{
    public class PurchaseRequestController : Controller
    {
        public IActionResult CreatePurchaseRequest()
        {
            return View();
        }
    }
}
