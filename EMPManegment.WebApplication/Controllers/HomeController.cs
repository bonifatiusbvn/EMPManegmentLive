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
                string Userid = HttpContext.Session.GetString("UserID");
                UserAttendanceRequestModel userAttendance = new UserAttendanceRequestModel
                {
                    UserId = Guid.Parse(Userid),
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
                string Userid = HttpContext.Session.GetString("UserID");
                Guid UserId = Guid.Parse(Userid);
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

        [HttpGet]
        public async Task<JsonResult> GetTaskType()
        {

            try
            {
                List<TaskTypeView> taskDeals = new List<TaskTypeView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel res = await APIServices.GetAsync(null, "UserHome/GetTaskType");
                if (res.code == 200)
                {
                    taskDeals = JsonConvert.DeserializeObject<List<TaskTypeView>>(res.data.ToString());
                }
                return new JsonResult(taskDeals);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTaskDetails(TaskDetailsView task)
        {
            try
            {
                
                if (ModelState.IsValid) 
                {
                    ApiResponseModel postuser = await APIServices.PostAsync(task, "UserHome/AddTaskDetails");
                    UserResponceModel responseModel = new UserResponceModel();
                    if (postuser.code == 200)
                    {
                        return Ok(new { postuser.message });
                    }
                    else
                    {
                       
                        return Ok(new { postuser.code });
                    }
                }
                return View(task);
                    
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetUserName()
        {
            try
            {
                List<EmpDetailsView> userList = new List<EmpDetailsView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel res = await APIServices.GetAsync("", "UserDetails/GetUsersNameList");
                if (res.code == 200)
                {
                    userList = JsonConvert.DeserializeObject<List<EmpDetailsView>>(res.data.ToString());
                }
                return new JsonResult(userList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetUserTaskDetail()
        {
            try
            {
                string Userid = HttpContext.Session.GetString("UserID");
                TaskDetailsView responceModel = new TaskDetailsView
                {
                    UserId = Guid.Parse(Userid),
                };
                List<TaskDetailsView> userList = new List<TaskDetailsView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel postuser = await APIServices.PostAsync(responceModel, "UserHome/GetUserTaskDetails");
                if(postuser.data != null)
                {
                    userList = JsonConvert.DeserializeObject<List<TaskDetailsView>>(postuser.data.ToString());
                    
                }
                else
                {
                    userList = new List<TaskDetailsView>();
                    ViewBag.Error = "note found";
                }
                return PartialView("~/Views/Home/_UserTaskList.cshtml", userList);
            }
            catch(Exception ex) 
            { 
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserDealStatus()
        {
            try
            {
                var userstausres = HttpContext.Request.Form["STATUSUPDATE"];

                var statusRequest = JsonConvert.DeserializeObject<TaskDetailsView>(userstausres);

                ApiResponseModel postuser = await APIServices.PostAsync(statusRequest, "UserHome/UpdateDealStatus");
                UserResponceModel userResponceModel = new UserResponceModel();
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message });
                }
                else
                {
                    return Ok(new { postuser.code });
                }
            }
            catch(Exception ex) 
            {
                throw ex;
            }
            
        }
    }
}