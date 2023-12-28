using Microsoft.AspNetCore.Mvc;

namespace EMPManegment.Web.Controllers
{
    public class ProductMasterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateProduct()
        {
            return View();
        }
    }
}
