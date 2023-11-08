using Microsoft.AspNetCore.Mvc;

namespace EMPManegment.Web.Controllers
{
    public class TaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserTasks()
        {
            return View();
        }
    }
}
