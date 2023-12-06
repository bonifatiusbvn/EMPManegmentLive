using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using Newtonsoft.Json;
using X.PagedList;
using X.PagedList.Mvc;

namespace EMPManegment.Web.Controllers
{
    public class ProjectController : Controller
    {
        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }
        public UserSession _userSession { get; }

        public ProjectController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices, UserSession userSession)
        {
            WebAPI = webAPI;
            Environment = environment;
            APIServices = aPIServices;
            _userSession = userSession;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateProject()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ProjectList(string? searchby, string? searchfor,int? page) 
        
        {
            try
            {
                List<ProjectDetailView> projectlist = new List<ProjectDetailView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel response = await APIServices.GetAsync("", "ProjectDetails/GetProjectList?searchby="+ searchby + "&searchfor=" + searchfor);
                if (response.code == 200)
                {
                    projectlist = JsonConvert.DeserializeObject<List<ProjectDetailView>>(response.data.ToString());
                }
                var pageProject = projectlist.ToPagedList(page ?? 1, 4);
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

        [HttpPost]
        public async Task<IActionResult> GetUserProjectList()
        {
            try
            {
               
                 ProjectDetailView responceModel = new ProjectDetailView
                 {
                    UserId = _userSession.UserId,
                };
                List<ProjectDetailView> ProjectList = new List<ProjectDetailView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel postuser = await APIServices.PostAsync(responceModel, "ProjectDetails/GetUserProjectList");
                if (postuser.data != null)
                {
                    ProjectList = JsonConvert.DeserializeObject<List<ProjectDetailView>>(postuser.data.ToString());

                }
                else
                {
                    ProjectList = new List<ProjectDetailView>();
                    ViewBag.Error = "note found";
                }
                return PartialView("~/Views/Project/_UserProjectList.cshtml", ProjectList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetProjectList()
        {
            try
            {
           
                ProjectDetailView responceModel = new ProjectDetailView
                {
                    UserId = _userSession.UserId,
                };
                List<ProjectDetailView> ProjectList = new List<ProjectDetailView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel postuser = await APIServices.PostAsync(responceModel, "ProjectDetails/GetUserProjectList");
                if (postuser.data != null)
                {
                    ProjectList = JsonConvert.DeserializeObject<List<ProjectDetailView>>(postuser.data.ToString());

                }
                else
                {
                    ProjectList = new List<ProjectDetailView>();
                    ViewBag.Error = "note found";
                }
                return PartialView("~/Views/Project/_GetProjectsDetails.cshtml", ProjectList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult AddProjectMember()
        {
            return View();  
        }
    }
}
