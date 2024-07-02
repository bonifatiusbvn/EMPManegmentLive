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

namespace EMPManegment.Web.Controllers
{

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

        public async Task<JsonResult> ProjectList(Guid? ProjectId, string? ProjectName)
        {
            UserSession.ProjectId = ProjectId.ToString();
            UserSession.ProjectName = ProjectId == null ? " " : ProjectName.ToString();
            return new JsonResult("Project Name updated succesfully!");
        }


        public async Task<IActionResult> GetWeatherinfo(string city)
        {

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://weatherapi-com.p.rapidapi.com/current.json?q=" + city),
                Headers =
    {
        { "x-rapidapi-key", "53a9c22907msh20b4c28bfb3bf4bp1cabf0jsn54f99e1b7c3d" },
        { "x-rapidapi-host", "weatherapi-com.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                List<Current> myDeserializedClass = JsonConvert.DeserializeObject<List<Current>>(body);
                return Json(myDeserializedClass);

            }
        }
    }
}