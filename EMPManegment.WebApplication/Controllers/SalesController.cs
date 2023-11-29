using Microsoft.AspNetCore.Mvc;

namespace EMPManegment.Web.Controllers
{
    public class SalesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateInvoice()
        {
            return View();
        }

    }
}
