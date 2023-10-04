using Microsoft.AspNetCore.Mvc;

namespace EMPManegment.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult UserHome()
        {
            return View();
        }
    }
}
