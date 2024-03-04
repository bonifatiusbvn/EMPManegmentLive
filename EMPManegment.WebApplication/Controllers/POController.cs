using Microsoft.AspNetCore.Mvc;

namespace EMPManegment.Web.Controllers
{
    public class POController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateOP()
        {
            return View();
        }
    }
}
