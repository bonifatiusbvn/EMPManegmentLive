using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using Newtonsoft.Json;
using PagedList.Mvc;
using PagedList;

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
        int pageSize = 3;
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateProject()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ProjectList(int pageNo=1)
        {
            try
            {
                List<ProjectDetailView> projectlist = new List<ProjectDetailView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel response = await APIServices.GetAsync("","ProjectDetails/GetProjectList");
                if (response.code == 200)
                {
                    projectlist = JsonConvert.DeserializeObject<List<ProjectDetailView>>(response.data.ToString());
                }
                var pageProject = new PagedList<ProjectDetailView>(projectlist,pageNo,pageSize);
                return View(pageProject);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
