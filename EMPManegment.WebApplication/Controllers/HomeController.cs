using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMPManegment.Web.Controllers
{
    
    public class HomeController : Controller
    {
        public HomeController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices)
        {
            WebAPI = webAPI;
            Environment = environment;
            APIServices = aPIServices;
        }

        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }
        public IActionResult UserHome()
        {


            return View();
        }


        [HttpPost]
        public async Task<IActionResult> EnterUserInOutTime(UserAttendanceModel userAttendance)
        {
            try
            {

                ApiResponseModel postuser = await APIServices.PostAsync(userAttendance, "UserHome/InsertINOutTime");
                if (postuser.code == 200)
                {

                    return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });

                }
                else
                {
                    return new JsonResult(new { Message = string.Format(postuser.message), Code = postuser.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
