﻿using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using EMPManegment.Web.Helper;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using static System.Net.Mime.MediaTypeNames;

namespace EMPManegment.Web.Controllers
{
    public class UserDetailsController : Controller
    {

        public WebAPI WebAPI { get; }
        public APIServices APIServices { get; }
        public IWebHostEnvironment Environment { get; }
        public UserDetailsController(WebAPI webAPI, APIServices aPIServices, IWebHostEnvironment environment)
        {
            WebAPI = webAPI;
            APIServices = aPIServices;
            Environment = environment;
        }


        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> DisplayUserList()
        {
            try
            {
                List<EmpDetailsView> userList = new List<EmpDetailsView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel res = await APIServices.GetAsync("", "UserDetails/GetAllUserList");
                if (res.code == 200)
                {
                    userList = JsonConvert.DeserializeObject<List<EmpDetailsView>>(res.data.ToString());
                }
                return View(userList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public async Task<IActionResult> UserProfile()
        {
            try
            {

                string id = HttpContext.Session.GetString("UserID");
                EmpDetailsView empList = new EmpDetailsView();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel response = await APIServices.GetAsync("", "AddEmp/GetById?Id=" + id);
                if (response.code == 200)
                {
                    empList = JsonConvert.DeserializeObject<EmpDetailsView>(response.data.ToString());
                }
                return View(empList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult EditUserProfile()
        {
            return View();
        }

        public async Task<JsonResult> GetDocumentType()
        {
            try
            {
                List<EmpDocumentView> documents = new List<EmpDocumentView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel res = await APIServices.GetAsync(null, "UserDetails/GetDocumentType");
                if (res.code == 200)
                {
                    documents = JsonConvert.DeserializeObject<List<EmpDocumentView>>(res.data.ToString());
                }
                return new JsonResult(documents);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<JsonResult> DisplayDocumentList()
        {
            try
            {
                List<DocumentInfoView> documentList = new List<DocumentInfoView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel res = await APIServices.GetAsync("", "UserDetails/GetDocumentList");
                if (res.code == 200)
                {
                    documentList = JsonConvert.DeserializeObject<List<DocumentInfoView>>(res.data.ToString());
                }
                return new JsonResult(documentList);
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }
        [HttpPost]

        public async Task<JsonResult> UploadDocument(EmpDocumentView doc)
        {
            try
            {
                var path = Environment.WebRootPath;
                var filepath = "Content/UserDocuments/" + doc.DocumentName.FileName;
                var fullpath = Path.Combine(path, filepath);
                UploadFile(doc.DocumentName, fullpath);
                var data = new DocumentInfoView()
                {
                    Id = doc.Id,
                    UserId =doc.UserId,
                    DocumentTypeId = doc.DocumentTypeId,
                    DocumentName = doc.DocumentName.FileName,
                CreatedBy = doc.CreatedBy,
                };
                ViewBag.Name = HttpContext.Session.GetString("UserID");
                ApiResponseModel postuser = await APIServices.PostAsync(data, "UserDetails/UploadDocument");
                if (postuser.code == 200)
                {
                    var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, doc.UserId.ToString()) }, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                }
                return new JsonResult(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void UploadFile(IFormFile file, string path)
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);
        }
    }
}
