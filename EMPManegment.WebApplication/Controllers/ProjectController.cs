using Microsoft.AspNetCore.Mvc;

namespace EMPManegment.Web.Controllers
{
    public class ProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateProject()
        {
            return View();
        }



    }
}
