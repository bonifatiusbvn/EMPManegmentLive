﻿
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
using DocumentFormat.OpenXml.Spreadsheet;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using X.PagedList;
using System.Net.Http.Headers;
using Microsoft.CodeAnalysis;
using EMPManegment.EntityModels.ViewModels.Weather;
using EMPManegment.EntityModels.ViewModels.Chat;
using Microsoft.CodeAnalysis.CSharp.Syntax;
#nullable disable
namespace EMPManegment.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserSession _userSession;
        public HomeController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices, UserSession userSession)
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

                    return Ok(new { Message = string.Format(postuser.message), Icone = string.Format(postuser.Icone), Code = postuser.code });

                }
                else
                {
                    return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
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

                    return Ok(new { Message = string.Format(postuser.message), Icone = string.Format(postuser.Icone), Code = postuser.code });

                }
                else
                {
                    return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
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
                if (postuser.code == 200)
                {
                    var data = JsonConvert.SerializeObject(postuser.data);
                    responseModel.Data = JsonConvert.DeserializeObject<UserAttendanceModel>(data);
                    return Ok(new { responseModel.Data, postuser.code });
                }
                else
                {
                    return Ok(new { postuser.code, postuser.message });
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
                ApiResponseModel postuser = await APIServices.GetAsync("", "UserHome/UserBirsthDayWish?UserId=" + UserId);
                UserAttendanceResponseModel responseModel = new UserAttendanceResponseModel();
                if (postuser.code == 200)
                {
                    return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
                }
                else
                {
                    return Ok(new { postuser.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetUserTotalTask()
        {
            try
            {

                var UserId = _userSession.UserId;
                List<TaskDetailsView> TaskList = new List<TaskDetailsView>();
                ApiResponseModel postuser = await APIServices.GetAsync("", "UserHome/GetUserTotalTask?UserId=" + UserId);
                if (postuser.data != null)
                {
                    TaskList = JsonConvert.DeserializeObject<List<TaskDetailsView>>(postuser.data.ToString());
                }
                else
                {
                    TaskList = new List<TaskDetailsView>();
                    ViewBag.Error = "not found";
                }
                return new JsonResult(TaskList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetHomeProjectListPartial(string? searchby, string? searchfor, int? page)
        {
            try
            {
                Guid UserId = _userSession.UserId;
                List<ProjectDetailView> projectlist = new List<ProjectDetailView>();
                ApiResponseModel response = await APIServices.PostAsync("", "ProjectDetails/GetProjectListById?searchby=" + searchby + "&searchfor=" + searchfor + "&UserId=" + UserId);
                if (response.code == 200)
                {
                    projectlist = JsonConvert.DeserializeObject<List<ProjectDetailView>>(response.data.ToString());
                }

                int pageSize = 6;
                var pageNumber = page ?? 1;

                var pagedList = projectlist.ToPagedList(pageNumber, pageSize);

                return PartialView("~/Views/Home/_HomeProjectView.cshtml", pagedList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult UnAuthorised()
        {
            return View();
        }

        public async Task<IActionResult> ProjectList(Guid? ProjectId, string? ProjectName)
        {
            try
            {

                UserSession.ProjectId = ProjectId.ToString();
                UserSession.ProjectName = ProjectId == null ? "All Project" : ProjectName.ToString();

                Guid? projectId = string.IsNullOrEmpty(UserSession.ProjectId) ? null : new Guid(UserSession.ProjectId);
                string projectName = string.IsNullOrEmpty(UserSession.ProjectName) ? null : new(UserSession.ProjectName);
                return Ok();

            }
            catch (Exception ex)
            {

                return BadRequest(new { Message = $"An error occurred: {ex.Message}" });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetWeatherinfo(string city)
        {
            try
            {
                Root root = new Root();
                ApiResponseModel response = await APIServices.GetAsync("", "UserHome/GetWeatherinfo?city=" + city);
                if (response.code == 200)
                {

                    if (response != null)
                    {
                        var data = JsonConvert.SerializeObject(response.data);
                        response.data = JsonConvert.DeserializeObject<Root>(data);
                        return Json(response.data);
                    }
                    else
                    {
                        return Ok(new { message = "No weather data found." });
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> ChatMessages()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetChateMembes(string searchUserName)
        {
            try
            {
                Guid userId = _userSession.UserId;
                List<ChatMessagesView> userChat = new List<ChatMessagesView>();
                ApiResponseModel response = await APIServices.GetAsync("", "UserHome/GetChatMembers?UserId=" + userId);
                if (response.code == 200)
                {
                    userChat = JsonConvert.DeserializeObject<List<ChatMessagesView>>(response.data.ToString());
                }

                if (!string.IsNullOrEmpty(searchUserName))
                {
                    searchUserName = searchUserName.ToLower();
                    userChat = userChat.Where(u => u.UserName.ToLower().Contains(searchUserName)).ToList();
                }

                return PartialView("~/Views/Home/_ChatBoradPartial.cshtml", userChat);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpGet]
        public async Task<IActionResult> GetChateConversation()
        {
            try
            {
                Guid userId = _userSession.UserId;
                List<ChatMessagesView> userChat = new List<ChatMessagesView>();
                ApiResponseModel response = await APIServices.GetAsync("", "UserHome/GetChatMembers?UserId=" + userId);
                if (response.code == 200)
                {
                    userChat = JsonConvert.DeserializeObject<List<ChatMessagesView>>(response.data.ToString());
                }

                return PartialView("~/Views/Home/_ChatBoradPartial.cshtml", userChat);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetChatConversation(Guid id)
        {
            try
            {
                List<ChatMessagesView> userChat = new List<ChatMessagesView>();
                ApiResponseModel response = await APIServices.GetAsync("", "UserHome/GetChatConversation?Id=" + id);
                if (response.code == 200)
                {
                    userChat = JsonConvert.DeserializeObject<List<ChatMessagesView>>(response.data.ToString());
                }

                return PartialView("~/Views/Home/_ChatConversationPartial.cshtml", userChat);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(ChatMessagesView ChatMessage)
        {
            ApiResponseModel postuser = await APIServices.PostAsync(ChatMessage, "UserHome/SendMessageAsync");
            if (postuser.code == 200)
            {
                return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
            }
            else
            {
                return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ReadMessage(ChatMessagesView ChatMessage)
        {
            ApiResponseModel postuser = await APIServices.PostAsync(ChatMessage, "UserHome/ReadMessageAsync");
            if (postuser.code == 200)
            {
                return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
            }
            else
            {
                return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
            }
        }

        public IActionResult ChatHub()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetChatUserInformation(Guid UserId)
        {
            try
            {
                EmpDetailsView UserDetails = new EmpDetailsView();
                ApiResponseModel res = await APIServices.GetAsync("", "UserProfile/GetEmployeeById?id=" + UserId);
                if (res.code == 200)
                {
                    UserDetails = JsonConvert.DeserializeObject<EmpDetailsView>(res.data.ToString());
                }
                return PartialView("~/Views/Home/_ContactPartial.cshtml", UserDetails);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AllUserListForChat(string searchUserName)
        {
            try
            {
                List<EmpDetailsView> AllUserList = new List<EmpDetailsView>();
                ApiResponseModel res = await APIServices.GetAsync("", "UserProfile/GetActiveDeactiveUserList");
                if (res.code == 200)
                {
                    AllUserList = JsonConvert.DeserializeObject<List<EmpDetailsView>>(res.data.ToString());
                }

                if (!string.IsNullOrEmpty(searchUserName))
                {
                    searchUserName = searchUserName.ToLower();
                    AllUserList = AllUserList.Where(u => u.FirstName.ToLower().Contains(searchUserName) || u.LastName.ToLower().Contains(searchUserName)).ToList();
                }

                return PartialView("~/Views/Home/_UserListForChatPartial.cshtml", AllUserList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetUserTaskNotification()
        {
            try
            {
                Guid UserId = _userSession.UserId;
                List<TaskDetailsView> activity = new List<TaskDetailsView>();
                ApiResponseModel postuser = await APIServices.GetAsync("", "UserHome/ProjectActivityByUserId?UserId=" + UserId);
                if (postuser.data != null)
                {
                    activity = JsonConvert.DeserializeObject<List<TaskDetailsView>>(postuser.data.ToString());

                }
                else
                {
                    activity = new List<TaskDetailsView>();
                    ViewBag.Error = "note found";
                }
                return PartialView("~/Views/Home/_UserTaskNotificationPartial.cshtml", activity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> CheckUserConversationId(Guid userId, Guid selectedUserId)
        {
            try
            {
                List<ChatMessagesView> userChat = new List<ChatMessagesView>();
                var chatconversation = new NewChatMessageModel()
                {
                    MyUserId = userId,
                    SelectedUserId = selectedUserId,
                    MyUserIdentity = _userSession.UserName,
                };
                ApiResponseModel response = await APIServices.PostAsync(chatconversation, "UserHome/CheckUserConversationId");
                if (response.code == 200)
                {
                    userChat = JsonConvert.DeserializeObject<List<ChatMessagesView>>(response.data.ToString());
                }

                return PartialView("~/Views/Home/_ChatConversationPartial.cshtml", userChat);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserNewMessagesNotification()
        {
            try
            {
                Guid UserId = _userSession.UserId;
                List<ChatMessagesView> messages = new List<ChatMessagesView>();
                ApiResponseModel postuser = await APIServices.GetAsync("", "UserHome/GetUsersNewMessageList?userId=" + UserId);
                if (postuser.data != null)
                {
                    messages = JsonConvert.DeserializeObject<List<ChatMessagesView>>(postuser.data.ToString());

                }
                return PartialView("~/Views/Home/_UserNewMessageNotificationPartial.cshtml", messages);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserAllNotifications()
        {
            try
            {
                Guid userId = _userSession.UserId;
                AllNotificationModel allNotifications = new AllNotificationModel();
                ApiResponseModel Response = await APIServices.GetAsync("", $"UserHome/GetUsersAllNotificationList?userId={userId}");
                if (Response.code == 200)
                {
                    allNotifications = JsonConvert.DeserializeObject<AllNotificationModel>(Response.data.ToString());
                }
                int totalMessageCount = allNotifications.Messages?.Count ?? 0;
                int totalTaskCount = allNotifications.Tasks?.Count ?? 0;
                int totalCount = totalMessageCount + totalTaskCount;
                ViewBag.TotalCount = totalCount;
                ViewBag.TotalUnreadMessages = totalMessageCount;
                ViewBag.TotalTasks = totalTaskCount;
                return PartialView("~/Views/Home/_UserAllNotificationPartial.cshtml", allNotifications);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}