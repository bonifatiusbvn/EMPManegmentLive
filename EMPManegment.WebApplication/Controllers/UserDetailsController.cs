using Azure;
using EMPManagment.API;
using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using EMPManegment.Web.Helper;
using System.Security.Claims;
using NuGet.Protocol.Plugins;
using System.Net;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Web.Helper;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using static System.Net.Mime.MediaTypeNames;
using EMPManagment.API;
using System.Security.Claims;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using Microsoft.IdentityModel.Tokens;
using EMPManegment.EntityModels.ViewModels.UserModels;
using Azure.Core;
using EMPManegment.EntityModels.ViewModels.TaskModels;

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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetUserList()
        {
            try
            {


                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault()+"][name]"].FirstOrDefault();
                var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                var dataTable = new DataTableRequstModel
                {
                    draw = draw,
                    start = start,
                    pageSize = pageSize,
                    skip = skip,
                    lenght = length,
                    searchValue = searchValue,
                    sortColumn =  sortColumn,
                    sortColumnDir = sortColumnDir
                };
                List<UserDataTblModel> GetUserList = new List<UserDataTblModel>();
                var data = new jsonData();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel res = await APIServices.PostAsync(dataTable,"UserDetails/GetAllUserList");
                if (res.code == 200)
                {
                    data = JsonConvert.DeserializeObject<jsonData>(res.data.ToString());
                    GetUserList = JsonConvert.DeserializeObject<List<UserDataTblModel>>(data.data.ToString());
                }
                var jsonData = new
                {
                    draw = data.draw,
                    recordsFiltered = data.recordsFiltered,
                    recordsTotal = data.recordsTotal,
                    data = GetUserList,
                };



                return new JsonResult(jsonData);  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IActionResult> UserEditList()
        {
            try
            {
                List<EmpDetailsView> EditUser = new List<EmpDetailsView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel res = await APIServices.GetAsync("", "UserDetails/UserEdit");
                if (res.code == 200)
                {
                    EditUser = JsonConvert.DeserializeObject<List<EmpDetailsView>>(res.data.ToString());
                }
                return View(EditUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> UserActiveDecative()
        {
            try
            {
                List<EmpDetailsView> ActiveDecative = new List<EmpDetailsView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel res = await APIServices.GetAsync("", "UserDetails/GetAllUsersDetails");
                if (res.code == 200)
                {
                    ActiveDecative = JsonConvert.DeserializeObject<List<EmpDetailsView>>(res.data.ToString());
                }
                return View(ActiveDecative);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public async Task<IActionResult> UserActiveDecative(string UserName)
        {
            try
            {

                ApiResponseModel postuser = await APIServices.PostAsync(null, "UserDetails/ActiveDeactiveUsers?UserName=" + UserName);
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
        [HttpGet]
        public async Task<IActionResult> UserProfile()
        {
            try
            {

                string id = HttpContext.Session.GetString("UserID");
                EmpDetailsView userProfile = new EmpDetailsView();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel response = await APIServices.GetAsync("", "UserDetails/GetEmployeeById?id=" + id);
                if (response.code == 200)
                {
                    userProfile = JsonConvert.DeserializeObject<EmpDetailsView>(response.data.ToString());
                }
                return View(userProfile);
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
                List<EmpDocumentView> documentsType = new List<EmpDocumentView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel res = await APIServices.GetAsync(null, "UserDetails/GetDocumentType");
                if (res.code == 200)
                {
                    documentsType = JsonConvert.DeserializeObject<List<EmpDocumentView>>(res.data.ToString());
                }
                return new JsonResult(documentsType);
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
                var uploadDocument = new DocumentInfoView()
                {
                    Id = doc.Id,
                    UserId =doc.UserId,
                    DocumentTypeId = doc.DocumentTypeId,
                    DocumentName = doc.DocumentName.FileName,
                CreatedBy = doc.CreatedBy,
                };
                ViewBag.Name = HttpContext.Session.GetString("UserID");
                ApiResponseModel postuser = await APIServices.PostAsync(uploadDocument, "UserDetails/UploadDocument");
                if (postuser.code == 200)
                {
                    var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, doc.UserId.ToString()) }, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                }
                return new JsonResult(uploadDocument);
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
        public async Task<IActionResult> ResetUserPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetUserPassword(PasswordResetResponseModel emp)
        {
            try
            {
                Crypto.Hash(emp.Password,
                   out byte[] passwordHash,
                   out byte[] passwordSalt);
                var resetPass = new PasswordResetView
                {
                    UserName = emp.UserName,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };
                ApiResponseModel postuser = await APIServices.PostAsync(resetPass, "UserDetails/ResetUserPassword");
                if (postuser.code == 200)
                {
                    return Ok(new { Message = postuser.message, Code = postuser.code });
                }
                else
                {
                    return new JsonResult(new { Message = string.Format(postuser.message), Code = postuser.code });
                }
            }catch (Exception ex)  
            { 
                throw ex;
            }
        }
        public async Task<IActionResult> LockScreen()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var StoredCookies = Request.Cookies.Keys;
            foreach (var Cookie in StoredCookies)
            {
                Response.Cookies.Delete(Cookie);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LockScreen(LoginRequest login)
        {
            try
            {
                    ApiResponseModel response = await APIServices.PostAsync(login, "UserDetails/UnLockScreen");
                    if (response.code == (int)HttpStatusCode.OK)
                    {
                       var data = JsonConvert.SerializeObject(response.data);
                       response.data = JsonConvert.DeserializeObject<LoginView>(data);
                       return RedirectToAction("UserHome", "Home");
                    }
                    else
                    {
                    TempData["ErrorMessage"] = response.message;
                    }

                return View();

            }

            catch (Exception ex)
            {
                return BadRequest(new { Message = "InternalServer" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersListById()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetUserAttendanceList()
        {
            try
             {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                var dataTable = new DataTableRequstModel
                {
                    draw = draw,
                    start = start,
                    pageSize = pageSize,
                    skip = skip,
                    lenght = length,
                    searchValue = searchValue,
                    sortColumn = sortColumn,
                    sortColumnDir = sortColumnDir
                };
                List<UserAttendanceModel> UserAttendance = new List<UserAttendanceModel>();
                var data = new jsonData();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel res = await APIServices.PostAsync(dataTable, "UserDetails/GetUserAttendanceList");
                if (res.code == 200)
                {
                    data = JsonConvert.DeserializeObject<jsonData>(res.data.ToString());
                    UserAttendance = JsonConvert.DeserializeObject<List<UserAttendanceModel>>(data.data.ToString());
                }
                var jsonData = new
                {
                    draw = data.draw,
                    recordsFiltered = data.recordsFiltered,
                    recordsTotal = data.recordsTotal,
                    data = UserAttendance,
                };
                return new JsonResult(jsonData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<JsonResult> EditUserAttendanceOutTime(int attendanceId)
        {
            try
            {
                List<UserAttendanceModel> Editattend = new List<UserAttendanceModel>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel res = await APIServices.GetAsync("","UserDetails/GetUserAttendanceById?attendanceId=" + attendanceId);

                if (res.code==200)
                {
                    Editattend = JsonConvert.DeserializeObject<List<UserAttendanceModel>>(res.data.ToString());
                }
                return new JsonResult(Editattend);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUserAttendanceOutTime(UserAttendanceModel userAttendance)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync(userAttendance, "UserDetails/UpdateUserOutTime");
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
        [HttpGet]
        public async Task<JsonResult> EditUserDetails(Guid id)
        {
            try
            {
                EmpDetailsView editDetails = new EmpDetailsView();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel res = await APIServices.GetAsync("", "UserDetails/GetEmployeeById?id=" + id);
                if (res.code == 200)
                {
                    editDetails = JsonConvert.DeserializeObject<EmpDetailsView>(res.data.ToString());
                }
                return new JsonResult(editDetails);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUserDetails(UserEditViewModel employee)
        {
            try
            {
                var Updateuser = new UserEditViewModel()
                {
                    Id = employee.Id,
                    DepartmentId = employee.DepartmentId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    Address = employee.Address,
                    PhoneNumber = employee.PhoneNumber,
                    DateOfBirth = employee.DateOfBirth,
                    Gender = employee.Gender,
                };
                ApiResponseModel postUser = await APIServices.PostAsync(Updateuser, "UserDetails/Update");
                if (postUser.code == 200)
                {
                    return Ok(new { Message = postUser.message, Code = postUser.code });
                }
                else
                {
                    return new JsonResult(new { Message = string.Format(postUser.message), Code = postUser.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> GetAttendance()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetAttendanceList()
        {
            try
            {
                var addmonthobj = HttpContext.Request.Form["FINDBYMONTH"];
                var month = JsonConvert.DeserializeObject<string>(addmonthobj);
                List<UserAttendanceModel> getAttendanceList = new List<UserAttendanceModel>();
                string Userid = HttpContext.Session.GetString("UserID");
                Guid UserId = Guid.Parse(Userid);
                HttpClient client = WebAPI.Initil();
                ApiResponseModel response = await APIServices.GetAsync("", "UserDetails/GetAttendanceList?id=" + UserId+ "&Cmonth=" + month);
                if (response.data.Count != 0)
                {
                    getAttendanceList = JsonConvert.DeserializeObject<List<UserAttendanceModel>>(response.data.ToString());
                }
                else
                {
                    return new JsonResult(new { Message = "No Data Found On Selected Month !!"});

                }
                return new JsonResult(getAttendanceList);
            }  
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }   
}

