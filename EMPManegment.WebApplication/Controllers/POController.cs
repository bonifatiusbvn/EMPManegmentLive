using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.POMaster;
using EMPManegment.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMPManegment.Web.Controllers
{
    public class POController : Controller
    {
        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }
        public UserSession _userSession { get; }
        public POController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices, UserSession userSession)
        {
            WebAPI = webAPI;
            Environment = environment;
            APIServices = aPIServices;
            _userSession = userSession;
        }
        public async Task<IActionResult> CreateOP()
        {
            try
            {
                string projectname = UserSession.ProjectName;
                ApiResponseModel Response = await APIServices.GetAsync("", "POMaster/CheckOPNo?projectname=" + projectname);
                if (Response.code == 200)
                {
                    ViewBag.OrderId = Response.data;
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateOP(OPMasterView CreatePO)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync(CreatePO, "ProjectDetails/CreateProject");
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
