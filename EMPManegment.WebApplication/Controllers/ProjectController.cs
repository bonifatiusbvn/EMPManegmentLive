using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using Microsoft.AspNetCore.Mvc;

namespace EMPManegment.Web.Controllers
{
    public class ProjectController : Controller
    {
        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }

        public ProjectController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices)
        {
            WebAPI = webAPI;
            Environment = environment;
            APIServices = aPIServices;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateProject()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectDetailView project)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync(project, "ProjectDetails/CreateProject");
                UserResponceModel responseModel = new UserResponceModel();
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message });
                }

                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
