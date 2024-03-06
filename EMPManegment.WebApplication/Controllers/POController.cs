using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.POMaster;
using EMPManegment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public async Task<IActionResult> CreatePO()
        {
            try
            {
                var PurchaseOrder = HttpContext.Request.Form["PURCHASEORDER"];
                var InsertDetails = JsonConvert.DeserializeObject<List<OPMasterView>>(PurchaseOrder.ToString());
                ApiResponseModel postuser = await APIServices.PostAsync(InsertDetails, "POMaster/CreatePO");
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message });
                }
                else
                {
                    return Ok(new { postuser.message });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> GetPOList()
        {
            return View();
        }
    }

}
