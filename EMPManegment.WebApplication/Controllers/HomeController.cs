using Azure;
using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Protocol.Plugins;
using System;
using System.Security.Claims;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using static System.Runtime.InteropServices.JavaScript.JSType;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.Web.Models;

namespace EMPManegment.Web.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly UserSession _userSession;
        public HomeController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices,UserSession userSession)
        {
            WebAPI = webAPI;
            Environment = environment;
            APIServices = aPIServices;
            _userSession = userSession;
        }

        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }
        public IActionResult UserHome()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> EnterUserInTime(UserAttendanceModel userAttendance)
        {
            try
            {

                var postuser = await APIServices.PostAsync(userAttendance, "UserHome/InsertINTime");
                UserResponceModel result = new UserResponceModel();
                if (postuser.code == 200)
                {

                    return Ok(new UserResponceModel { Message = string.Format(postuser.message), Icone = string.Format(postuser.Icone),Code = postuser.code });

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

        [HttpPost]
        public async Task<IActionResult> EnterUserOutTime(UserAttendanceModel userAttendance)
        {
            try
            {

                ApiResponseModel postuser = await APIServices.PostAsync(userAttendance, "UserHome/InsertOutTime");
                if (postuser.code == 200)
                {

                    return Ok(new UserResponceModel { Message = string.Format(postuser.message), Icone = string.Format(postuser.Icone), Code = postuser.code });

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


        [HttpPost]
        public async Task<IActionResult> GetUserAttendanceInTime()
        {
            try
            {
                
                UserAttendanceRequestModel userAttendance = new UserAttendanceRequestModel
                {
                    UserId = _userSession.UserId,
                    Date = DateTime.Now,
                };
                ApiResponseModel postuser = await APIServices.PostAsync(userAttendance, "UserHome/GetUserAttendanceInTime");
                UserAttendanceResponseModel responseModel = new UserAttendanceResponseModel();
                if (postuser.data != null)
                {
                    var data = JsonConvert.SerializeObject(postuser.data);
                    responseModel.Data = JsonConvert.DeserializeObject<UserAttendanceModel>(data);
                    return Ok(new {responseModel.Data});
                }
                else
                {
                    return Ok(new {postuser.code});
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        [HttpGet]
        public async Task<IActionResult> UserBirsthDayWish()
        {
            try
            {
                
                Guid UserId = _userSession.UserId;
                ApiResponseModel postuser = await APIServices.GetAsyncId(UserId,"UserHome/UserBirsthDayWish");
                UserAttendanceResponseModel responseModel = new UserAttendanceResponseModel();
                if (postuser.message != null)
                {
                    return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
                }

                else
                {
                    return Ok(new { postuser.code }); ;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}