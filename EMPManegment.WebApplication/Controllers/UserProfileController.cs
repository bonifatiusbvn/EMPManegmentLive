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
using System.Security.Claims;
using NuGet.Protocol.Plugins;
using System.Net;
using EMPManegment.EntityModels.ViewModels.Models;
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
using X.PagedList;
using X.PagedList.Mvc;
using EMPManegment.EntityModels.Crypto;
using Microsoft.Build.ObjectModelRemoting;
using EMPManegment.Web.Models;

namespace EMPManegment.Web.Controllers
{
    public class UserProfileController : Controller
    {

        public WebAPI WebAPI { get; }
        public APIServices APIServices { get; }
        public IWebHostEnvironment Environment { get; }

        public UserSession _userSession { get; }
        public UserProfileController(WebAPI webAPI, APIServices aPIServices, IWebHostEnvironment environment, UserSession userSession)
        {
            WebAPI = webAPI;
            APIServices = aPIServices;
            Environment = environment;
            _userSession = userSession;
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
                ApiResponseModel res = await APIServices.PostAsync(dataTable,"UserProfile/GetAllUserList");
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
                ApiResponseModel res = await APIServices.GetAsync("", "UserProfile/UserEdit");
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

        public async Task<IActionResult> UserActiveDecative(string? searchby, string? searchfor, int? page)
        {
            try
            {
                List<EmpDetailsView> ActiveDecative = new List<EmpDetailsView>();
                HttpClient client = WebAPI.Initil();                                
                ApiResponseModel res = await APIServices.GetAsync("", "UserProfile/GetAllUsersDetails?searchby=" + searchby +"&searchfor=" + searchfor);
                if (res.code == 200)
                {
                    ActiveDecative = JsonConvert.DeserializeObject<List<EmpDetailsView>>(res.data.ToString());
                }
                var activeDeactivePage = ActiveDecative.ToPagedList(page ?? 1, 4);
                return View(activeDeactivePage);
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

                ApiResponseModel postuser = await APIServices.PostAsync(null, "UserProfile/ActiveDeactiveUsers?UserName=" + UserName);
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
                Guid id = _userSession.UserId;
                EmpDetailsView userProfile = new EmpDetailsView();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel response = await APIServices.GetAsync("", "UserProfile/GetEmployeeById?id=" + id);
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
                ApiResponseModel res = await APIServices.GetAsync(null, "UserProfile/GetDocumentType");
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

                Guid userid = _userSession.UserId;
                HttpClient client = WebAPI.Initil();
                List<DocumentInfoView> documentList = new List<DocumentInfoView>();
                ApiResponseModel res = await APIServices.GetAsync("", "UserProfile/GetDocumentList?Userid=" + userid);
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
                var DocName = Guid.NewGuid() + "_" + doc.DocumentName.FileName;
                var path = Environment.WebRootPath;
                var filepath = "Content/UserDocuments/" + DocName;
                var fullpath = Path.Combine(path, filepath);
                UploadFile(doc.DocumentName, fullpath);
                var uploadDocument = new DocumentInfoView()
                {
                    Id = doc.Id,
                    UserId =doc.UserId,
                    DocumentTypeId = doc.DocumentTypeId,
                    DocumentName = DocName,
                    CreatedBy = doc.CreatedBy,
                };
                ViewBag.Name = HttpContext.Session.GetString("UserID");
                ApiResponseModel postuser = await APIServices.PostAsync(uploadDocument, "UserProfile/UploadDocument");
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
                ApiResponseModel postuser = await APIServices.PostAsync(resetPass, "UserProfile/ResetUserPassword");
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
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LockScreen(LoginRequest login)
        {
            try
            {
                    ApiResponseModel response = await APIServices.PostAsync(login, "UserProfile/UnLockScreen");
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
                ApiResponseModel res = await APIServices.PostAsync(dataTable, "UserProfile/GetUserAttendanceList");
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
                ApiResponseModel res = await APIServices.GetAsync("", "UserProfile/GetUserAttendanceById?attendanceId=" + attendanceId);

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
                ApiResponseModel postuser = await APIServices.PostAsync(userAttendance, "UserProfile/UpdateUserOutTime");
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
                ApiResponseModel res = await APIServices.GetAsync("", "UserProfile/GetEmployeeById?id=" + id);
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
                ApiResponseModel postUser = await APIServices.PostAsync(Updateuser, "UserProfile/Update");
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
                Guid UserId = _userSession.UserId;
                HttpClient client = WebAPI.Initil();
                ApiResponseModel response = await APIServices.GetAsync("", "UserProfile/GetAttendanceList?id=" + UserId+ "&Cmonth=" + month);
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

        [HttpGet]
        public async Task<FileResult> DownloadDocument(string documentName)
        {
            var filepath = "Content/UserDocuments/" + documentName;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filepath);
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                 await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var ContentType = "application/pdf"; 
            var fileName = Path.GetFileName(path);
            return File(memory, ContentType, fileName);
        }
    }
}

