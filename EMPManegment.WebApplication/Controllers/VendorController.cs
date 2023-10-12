using Microsoft.AspNetCore.Mvc;

namespace EMPManegment.Web.Controllers
{
    public class VendorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult AddVandorDetails()
        {
            return View();
        }
    }
}
