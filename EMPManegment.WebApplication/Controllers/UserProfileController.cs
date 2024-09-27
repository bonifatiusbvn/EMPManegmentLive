﻿
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
using System.Security.Claims;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.UserModels;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using X.PagedList;
using X.PagedList.Mvc;
using EMPManegment.EntityModels.Crypto;
using Microsoft.Build.ObjectModelRemoting;
using EMPManegment.Web.Models;
using Newtonsoft.Json.Linq;
using ClosedXML.Excel;
using System.Data;
using System.Reflection;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Drawing.Charts;
using Aspose.Pdf;
using Aspose.Pdf.Text;
using System.IO;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.EntityModels.ViewModels.FormPermissionMaster;
using Microsoft.AspNetCore.Authorization;
using EMPManegment.Web.Helper;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using EMPManegment.EntityModels.ViewModels.FormMaster;
using iTextSharp.text.pdf;
using EMPManegment.EntityModels.ViewModels.Invoice;
using Aspose.Pdf.Operators;
#nullable disable
namespace EMPManegment.Web.Controllers
{
    [Authorize]
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

        [FormPermissionAttribute("Create User-View")]
        public async Task<IActionResult> CreateUser()
        {
            try
            {
                ApiResponseModel AddUserResponse = await APIServices.GetAsync("", "User/CheckUser");
                if (AddUserResponse.code == 200)
                {
                    ViewBag.EmpId = AddUserResponse.data;
                }

                return View();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [FormPermissionAttribute("Create User-Add")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(LoginDetailsView AddEmployee)
        {
            try
            {
                Crypto.Hash(AddEmployee.Password,
                    out byte[] passwordHash,
                    out byte[] passwordSalt);
                var AddUser = new EmpDetailsView()
                {
                    UserName = AddEmployee.UserName,
                    DepartmentId = AddEmployee.DepartmentId,
                    FirstName = AddEmployee.FirstName,
                    LastName = AddEmployee.LastName,
                    Address = AddEmployee.Address,
                    CityId = AddEmployee.CityId,
                    StateId = AddEmployee.StateId,
                    CountryId = AddEmployee.CountryId,
                    DateOfBirth = AddEmployee.DateOfBirth,
                    Email = AddEmployee.Email,
                    Gender = AddEmployee.Gender,
                    PhoneNumber = AddEmployee.PhoneNumber,
                    CreatedOn = DateTime.Now,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    IsActive = true,
                    CreatedBy = _userSession.FullName
                };
                if (AddEmployee.Image != null)
                {
                    var UserImg = Guid.NewGuid() + "_" + AddEmployee.Image.FileName;
                    var path = Environment.WebRootPath;
                    var filepath = "Content/Image/" + UserImg;
                    var fullpath = Path.Combine(path, filepath);
                    UploadFile(AddEmployee.Image, fullpath);
                    AddUser.Image = UserImg;
                }
                else
                {
                    AddUser.Image = null;
                }

                ApiResponseModel postuser = await APIServices.PostAsync(AddUser, "User/UserSingUp");
                if (postuser.code == 200)
                {
                    return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
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


        [FormPermissionAttribute("Users-View")]
        public async Task<IActionResult> UserList()
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
                List<UserDataTblModel> GetUserList = new List<UserDataTblModel>();
                var data = new jsonData();
                ApiResponseModel res = await APIServices.PostAsync(dataTable, "UserProfile/GetAllUserList");
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

        public async Task<IActionResult> UserInfo(Guid Id)
        {
            try
            {
                EmpDetailsView UserDetails = new EmpDetailsView();
                ApiResponseModel res = await APIServices.GetAsync("", "UserProfile/GetEmployeeById?id=" + Id);
                if (res.code == 200)
                {
                    UserDetails = JsonConvert.DeserializeObject<EmpDetailsView>(res.data.ToString());
                }
                return View(UserDetails);
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

        [FormPermissionAttribute("Active Deactive-View")]
        public async Task<IActionResult> UserActiveDecative()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UserActiveDecativeList(Guid? Id, int? DepartmentId, int? page)
        {
            try
            {
                List<EmpDetailsView> ActiveDecative = new List<EmpDetailsView>();
                ApiResponseModel res = await APIServices.GetAsync("", "UserProfile/GetActiveDeactiveUserList");
                if (res.code == 200)
                {
                    ActiveDecative = JsonConvert.DeserializeObject<List<EmpDetailsView>>(res.data.ToString());
                }
                if (Id.HasValue)
                {
                    ActiveDecative = ActiveDecative.Where(a => a.Id == Id).ToList();
                }
                else if (DepartmentId.HasValue)
                {
                    ActiveDecative = ActiveDecative.Where(a => a.DepartmentId == DepartmentId).ToList();
                }

                int pageSize = 5;
                var pageNumber = page ?? 1;

                var pagedList = ActiveDecative.ToPagedList(pageNumber, pageSize);

                return PartialView("~/Views/UserProfile/_UserActiveDeactivePartial.cshtml", pagedList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetSearchEmpList(EmpDetailsModel EmpList)
        {
            try
            {
                List<EmpDetailsView> EmployeeList = new List<EmpDetailsView>();
                ApiResponseModel response = await APIServices.PostAsync(EmpList, "UserProfile/GetSearchEmplList");
                if (response.data.Count != 0)
                {
                    EmployeeList = JsonConvert.DeserializeObject<List<EmpDetailsView>>(response.data.ToString());
                    return PartialView("~/Views/UserProfile/_ActiveInactiveEmployeeList.cshtml", EmployeeList);
                }
                else
                {
                    return new JsonResult(new { Code = 400 });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [FormPermissionAttribute("Active Deactive-Edit")]
        [HttpPost]
        public async Task<IActionResult> UserActiveDecative(Guid UserId, Guid UpdatedBy)
        {
            try
            {

                ApiResponseModel postuser = await APIServices.PostAsync("", "UserProfile/ActiveDeactiveUsers?UserId=" + UserId + "&UpdatedBy" + UpdatedBy);
                if (postuser.code == 200)
                {

                    return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });

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
        [HttpGet]
        public async Task<IActionResult> UserProfile()
        {
            try
            {
                Guid id = _userSession.UserId;
                EmpDetailsView userProfile = new EmpDetailsView();
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
                    UserId = doc.UserId,
                    DocumentTypeId = doc.DocumentTypeId,
                    DocumentName = DocName,
                    CreatedBy = _userSession.FullName,
                    CreatedOn = DateTime.Now,
                };

                ApiResponseModel postuser = await APIServices.PostAsync(uploadDocument, "UserProfile/UploadDocument");
                if (postuser.code == 200)
                {
                    return Json(new { code = 200, message = "Document Uploaded Successfully!" });
                }
                else
                {
                    return Json(new { code = 400, message = "Error in saving the uploaded document" });
                }
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

        [FormPermissionAttribute("Reset Password-View")]
        public async Task<IActionResult> ResetPassword()
        {
            return View();
        }


        [FormPermissionAttribute("Reset Password-Add")]
        [HttpPost]
        public async Task<IActionResult> ResetUserPassword(PasswordResetResponseModel ResetPassword)
        {
            try
            {
                Crypto.Hash(ResetPassword.Password,
                   out byte[] passwordHash,
                   out byte[] passwordSalt);
                var resetPass = new PasswordResetView
                {
                    UserId = ResetPassword.UserId,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    UpdatedBy = _userSession.UserId
                };
                ApiResponseModel postuser = await APIServices.PostAsync(resetPass, "UserProfile/ResetUserPassword");
                if (postuser.code == 200)
                {
                    return Ok(new { Message = postuser.message, Code = postuser.code });
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

        [FormPermissionAttribute("Users Attendance-View")]
        [HttpGet]
        public async Task<IActionResult> UsersAttendance()
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
                var sortColumn = Request.Form["columns[" + Request.Form["order[1][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
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
        public async Task<JsonResult> EditOutTime(int attendanceId)
        {
            try
            {
                List<UserAttendanceModel> Editattend = new List<UserAttendanceModel>();
                ApiResponseModel res = await APIServices.GetAsync("", "UserProfile/GetUserAttendanceById?attendanceId=" + attendanceId);

                if (res.code == 200)
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

        [FormPermissionAttribute("Users Attendance-Edit")]
        [HttpPost]
        public async Task<IActionResult> UpdateOutTime(UserAttendanceModel userAttendance)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync(userAttendance, "UserProfile/UpdateUserOutTime");
                if (postuser.code == 200)
                {
                    return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
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
        [HttpGet]
        public async Task<JsonResult> EditUserDetails(Guid id)
        {
            try
            {
                EmpDetailsView editDetails = new EmpDetailsView();
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


        [FormPermissionAttribute("Users-Edit")]
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
                    UpdatedBy = _userSession.UserId,
                    RoleId = employee.RoleId,
                };
                ApiResponseModel postUser = await APIServices.PostAsync(Updateuser, "UserProfile/UpdateUserDetails");
                if (postUser.code == 200)
                {
                    return Ok(new { Message = postUser.message, Code = postUser.code });
                }
                else
                {
                    return Ok(new { Message = string.Format(postUser.message), Code = postUser.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserRoleAndDepartment()
        {
            try
            {
                var userstausres = HttpContext.Request.Form["USERUPDATE"];
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = "dd-MM-yyyy HH:mm:ss",
                    Error = (sender, args) =>
                    {
                        args.ErrorContext.Handled = true;
                    }
                };

                var statusRequest = JsonConvert.DeserializeObject<UserEditViewModel>(userstausres, settings);
                ApiResponseModel postUser = await APIServices.PostAsync(statusRequest, "UserProfile/UpdateUserDetails");
                if (postUser.code == 200)
                {
                    return Ok(new { Message = postUser.message, Code = postUser.code });
                }
                else
                {
                    return Ok(new { Message = postUser.message, Code = postUser.code });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating user details.", Details = ex.Message });
            }
        }



        [FormPermissionAttribute("Attendance-View")]
        public async Task<IActionResult> MyAttendance()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetAttendanceList(SearchAttendanceModel GetAttendanceList)
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[1][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
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
            var AttendanceData = new SearchAttendanceModel
            {
                UserId = _userSession.UserId,
                Cmonth = GetAttendanceList.Cmonth,
                StartDate = GetAttendanceList.StartDate,
                EndDate = GetAttendanceList.EndDate,
            };
            var AttendanceRequestModel = new MyAttendanceRequestDataTableModel
            {
                SearchAttendance = AttendanceData,
                DataTable = dataTable
            };

            List<UserAttendanceModel> getAttendanceList = new List<UserAttendanceModel>();
            var data = new jsonData();
            HttpClient client = WebAPI.Initil();
            ApiResponseModel res = await APIServices.PostAsync(AttendanceRequestModel, "UserProfile/GetAttendanceList");
            if (res.code == 200)
            {
                data = JsonConvert.DeserializeObject<jsonData>(res.data.ToString());
                getAttendanceList = JsonConvert.DeserializeObject<List<UserAttendanceModel>>(data.data.ToString());
            }
            var jsonData = new
            {
                draw = data.draw,
                recordsFiltered = data.recordsFiltered,
                recordsTotal = data.recordsTotal,
                data = getAttendanceList,
            };
            return new JsonResult(jsonData);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadDocument(string documentName)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Content/UserDocuments", documentName);

            if (!System.IO.File.Exists(filepath))
            {
                return NotFound();
            }

            byte[] bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            string base64String = Convert.ToBase64String(bytes);

            return Json(new { memory = base64String, contentType = "application/pdf", fileName = documentName });
        }

        [HttpPost]
        public async Task<IActionResult> ExportToPdf(SearchAttendanceModel searchAttendanceData)
        {
            try
            {
                HttpClient client = WebAPI.Initil();
                ApiResponseModel response = await APIServices.PostAsync(searchAttendanceData, "UserProfile/GetMySearchAttendanceList");

                if (response.data.Count != 0)
                {
                    var getAttendanceList = JsonConvert.DeserializeObject<List<UserAttendanceModel>>(response.data.ToString());

                    var document = new Aspose.Pdf.Document
                    {
                        PageInfo = new PageInfo { Margin = new MarginInfo(25, 25, 25, 40) }
                    };

                    var pdfPage = document.Pages.Add();

                    Aspose.Pdf.Table table = new Aspose.Pdf.Table
                    {
                        ColumnWidths = "19% 19% 19% 19% 23%",
                        DefaultCellPadding = new MarginInfo(5, 5, 5, 5),
                        Border = new BorderInfo(BorderSide.All, .5f, Aspose.Pdf.Color.Black),
                        DefaultCellBorder = new BorderInfo(BorderSide.All, .2f, Aspose.Pdf.Color.Black),
                    };

                    System.Data.DataTable dt = ToConvertDataTable(getAttendanceList);
                    table.ImportDataTable(dt, true, 0, 0);

                    document.Pages[1].Paragraphs.Add(table);

                    using (var streamout = new MemoryStream())
                    {
                        document.Save(streamout);
                        return new FileContentResult(streamout.ToArray(), "application/pdf")
                        {
                            FileDownloadName = Guid.NewGuid() + "_AttendanceList.pdf",
                        };
                    }
                }
                return RedirectToAction("GetAttendance");
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        public async Task<IActionResult> ExportToExcel(SearchAttendanceModel searchAttendanceData)
        {
            try
            {

                HttpClient client = WebAPI.Initil();
                ApiResponseModel response = await APIServices.PostAsync(searchAttendanceData, "UserProfile/GetMySearchAttendanceList");

                if (response.data.Count != 0)
                {
                    var getAttendanceList = JsonConvert.DeserializeObject<List<UserAttendanceModel>>(response.data.ToString());

                    using (var wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(ToConvertDataTable(getAttendanceList));
                        using (var stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            string fileName = Guid.NewGuid() + "_AttendanceList.xlsx";
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                        }
                    }
                }
                return RedirectToAction("GetAttendance");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        public System.Data.DataTable ToConvertDataTable<T>(List<T> items)
        {
            System.Data.DataTable dt = new System.Data.DataTable(typeof(T).Name);
            PropertyInfo[] propInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in propInfo)
            {
                dt.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[propInfo.Length];
                for (int i = 0; i < propInfo.Length; i++)
                {
                    PropertyInfo prop = propInfo[i];
                    var value = prop.GetValue(item, null);
                    if (value is DateTime dateTimeValue)
                    {
                        if (prop.Name == "Date")
                        {
                            values[i] = dateTimeValue.ToString("dd-MM-yyyy");
                        }
                        else if (prop.Name == "Intime" || prop.Name == "OutTime")
                        {
                            values[i] = dateTimeValue.ToString("hh:mm tt");
                        }
                        else
                        {
                            values[i] = value;
                        }
                    }
                    else if (prop.Name == "TotalHours" && value is TimeSpan timeSpanValue)
                    {
                        values[i] = $"{timeSpanValue.Hours:D2}:{timeSpanValue.Minutes:D2} hr";
                    }
                    else if (prop.Name == "TotalHours" && value is double hoursValue)
                    {
                        TimeSpan timeSpan = TimeSpan.FromHours(hoursValue);
                        values[i] = $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2} hr";
                    }
                    else
                    {
                        values[i] = value;
                    }
                }
                dt.Rows.Add(values);
            }
            dt.Columns.Remove("UserId");
            dt.Columns.Remove("CreatedBy");
            dt.Columns.Remove("AttendanceId");
            dt.Columns.Remove("CreatedOn");
            dt.Columns.Remove("UpdatedOn");
            dt.Columns.Remove("UpdatedBy");
            return dt;
        }

        public IActionResult GetSearchAttendanceList()
        {
            return PartialView("~/Views/UserProfile/_SearchAttendanceList.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> GetUserSearchAttendanceList(searchAttendanceListModel GetAttendanceList)
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[1][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
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

            var AttendanceRequestModel = new AttendanceRequestDataTableModel
            {
                SearchAttendance = GetAttendanceList,
                DataTable = dataTable
            };

            List<UserAttendanceModel> getAttendanceList = new List<UserAttendanceModel>();
            var data = new jsonData();
            HttpClient client = WebAPI.Initil();
            ApiResponseModel res = await APIServices.PostAsync(AttendanceRequestModel, "UserProfile/GetSearchAttendanceList");
            if (res.code == 200)
            {
                data = JsonConvert.DeserializeObject<jsonData>(res.data.ToString());
                getAttendanceList = JsonConvert.DeserializeObject<List<UserAttendanceModel>>(data.data.ToString());
            }
            var jsonData = new
            {
                draw = data.draw,
                recordsFiltered = data.recordsFiltered,
                recordsTotal = data.recordsTotal,
                data = getAttendanceList,
            };
            return new JsonResult(jsonData);
        }

        [FormPermissionAttribute("Form Permission-View")]
        [HttpGet]
        public IActionResult FormPermission()
        {
            return View();
        }

        public async Task<IActionResult> RolewisePermissionListAction()
        {
            try
            {
                ApiResponseModel res = await APIServices.PostAsync("", "MasterList/GetUserRoleList");

                if (res.code == 200)
                {
                    List<UserRoleModel> GetUserRoleList = JsonConvert.DeserializeObject<List<UserRoleModel>>(res.data.ToString());
                    return new JsonResult(GetUserRoleList);
                }
                else
                {
                    return new JsonResult(new { Message = "Failed to retrieve user role list." });
                }
            }
            catch (Exception ex)
            {

                return new JsonResult(new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetRolewiseFormListById(Guid RoleId)
        {
            try
            {
                List<RolewiseFormPermissionModel> RolewiseFormList = new List<RolewiseFormPermissionModel>();
                ApiResponseModel response = await APIServices.PostAsync("", "FormPermissionMaster/GetRolewiseFormListById?RoleId=" + RoleId);
                if (response.code == 200)
                {
                    RolewiseFormList = JsonConvert.DeserializeObject<List<RolewiseFormPermissionModel>>(response.data.ToString());
                    return PartialView("~/Views/UserProfile/_editRolewiseFormPartial.cshtml", RolewiseFormList);
                }
                else
                {
                    return Ok(new { response.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [FormPermissionAttribute("Form Permission-Edit")]
        [HttpPost]
        public async Task<IActionResult> UpdatePermission()
        {
            try
            {
                var rolewisePermissionDetails = HttpContext.Request.Form["RolewisePermissionDetails"];
                var UpdateDetails = JsonConvert.DeserializeObject<List<RolewiseFormPermissionModel>>(rolewisePermissionDetails.ToString());

                ApiResponseModel postuser = await APIServices.PostAsync(UpdateDetails, "FormPermissionMaster/UpdateMultipleRolewiseFormPermission");
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message, postuser.code });
                }
                else
                {
                    return Ok(new { postuser.message, postuser.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [FormPermissionAttribute("Form Permission-Add")]
        [HttpPost]
        public async Task<IActionResult> CreateUserRole(UserRoleModel roleDetails)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync(roleDetails, "FormPermissionMaster/CreateUserRole");
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message, postuser.code });
                }
                else
                {
                    return Ok(new { postuser.message, postuser.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [FormPermissionAttribute("FormCreation-View")]
        [HttpGet]
        public IActionResult FormCreation()
        {
            return View();
        }

        public async Task<JsonResult> GetFormNameList()
        {
            try
            {
                List<FormMasterModel> FormList = new List<FormMasterModel>();
                ApiResponseModel res = await APIServices.GetAsync("", "FormPermissionMaster/FormList");
                if (res.code == 200)
                {
                    FormList = JsonConvert.DeserializeObject<List<FormMasterModel>>(res.data.ToString());
                }
                return new JsonResult(FormList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [FormPermissionAttribute("FormCreation-Add")]
        [HttpPost]
        public async Task<IActionResult> CreateRolewisePermissionForm(int FormId)
        {
            try
            {
                var userId = @_userSession.UserId;
                ApiResponseModel postuser = await APIServices.PostAsync("", "FormPermissionMaster/CreateRolewisePermissionForm?FormId=" + FormId + "&userId=" + userId);
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message, postuser.code });
                }
                else
                {
                    return Ok(new { postuser.message, postuser.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [FormPermissionAttribute("User Form Permission-View")]
        [HttpGet]
        public IActionResult UserFormPermission()
        {
            return View();
        }

        [FormPermissionAttribute("User Form Permission-Add")]
        [HttpPost]
        public async Task<IActionResult> CreateUserForm(Guid UserId)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync("", "FormPermissionMaster/CreateUserForm?UserId=" + UserId);
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message, postuser.code });
                }
                else
                {
                    return Ok(new { postuser.message, postuser.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetUserFormListById(Guid UserId)
        {
            try
            {
                List<UserPermissionModel> UserFormList = new List<UserPermissionModel>();
                ApiResponseModel response = await APIServices.PostAsync("", "FormPermissionMaster/GetUserFormListById?UserId=" + UserId);

                if (response.code == 200)
                {
                    UserFormList = JsonConvert.DeserializeObject<List<UserPermissionModel>>(response.data.ToString());
                    return PartialView("~/Views/UserProfile/_UserFormPermissionPartial.cshtml", UserFormList);
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [FormPermissionAttribute("User Form Permission-Edit")]
        [HttpPost]
        public async Task<IActionResult> UpdateUserPermission()
        {
            try
            {
                var userFormPermission = HttpContext.Request.Form["UserPermissionDetails"];
                var UpdateDetails = JsonConvert.DeserializeObject<List<UserPermissionModel>>(userFormPermission.ToString());

                ApiResponseModel postuser = await APIServices.PostAsync(UpdateDetails, "FormPermissionMaster/UpdateMultipleUserFormPermission");
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message, postuser.code });
                }
                else
                {
                    return Ok(new { postuser.message, postuser.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetInvoiceActivityByUserId()
        {
            try
            {
                var UserId = _userSession.UserId;
                List<InvoiceViewModel> activity = new List<InvoiceViewModel>();
                ApiResponseModel postuser = await APIServices.GetAsync("", "Invoice/InvoicActivityByUserId?UserId=" + UserId);
                if (postuser.data != null)
                {
                    activity = JsonConvert.DeserializeObject<List<InvoiceViewModel>>(postuser.data.ToString());
                }
                else
                {
                    activity = new List<InvoiceViewModel>();
                    ViewBag.Error = "note found";
                }
                return PartialView("~/Views/UserProfile/_UserActivityPartial.cshtml", activity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [FormPermissionAttribute("Users-Edit")]
        [HttpPost]
        public async Task<IActionResult> UpdateUserExeperience(EmpDetailsView UpdateDate)
        {
            try
            {
                var Updateuser = new EmpDetailsView()
                {
                    Id = UpdateDate.Id,
                    LastDate = UpdateDate.LastDate,
                };
                ApiResponseModel postUser = await APIServices.PostAsync(Updateuser, "UserProfile/UpdateUserExeperience");
                if (postUser.code == 200)
                {
                    return Ok(new { Message = postUser.message, Code = postUser.code });
                }
                else
                {
                    return Ok(new { Message = string.Format(postUser.message), Code = postUser.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> UserProfilePhoto(LoginDetailsView Profile)
        {
            try
            {
                if (Profile.Image != null)
                {

                    var UserImg = Guid.NewGuid() + "_" + Profile.Image.FileName;
                    var path = Environment.WebRootPath;
                    var filepath = "Content/Image/" + UserImg;
                    var fullpath = Path.Combine(path, filepath);
                    UploadFile(Profile.Image, fullpath);
                    var UploadImage = new EmpDetailsView()
                    {
                        Id = _userSession.UserId,
                        Image = filepath,

                    };
                    ApiResponseModel postUser = await APIServices.PostAsync(UploadImage, "UserProfile/UserProfilePhoto");
                    if (postUser.code == 200)
                    {
                        return Ok(new { Message = "Profile Uploaded Successfully!", Code = postUser.code });
                    }
                    else
                    {
                        return Ok(new { Message = string.Format(postUser.message), Code = postUser.code });
                    }
                }
                return BadRequest();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddUserAttendance(UserAttendanceModel AddUser)
        {
            try
            {
                var UserDetails = new UserAttendanceModel
                {
                    UserId = AddUser.UserId,
                    Date = AddUser.Date,
                    Intime = AddUser.Intime,
                    OutTime = AddUser.OutTime,
                    TotalHours = AddUser.TotalHours,
                    CreatedBy = _userSession.UserId.ToString(),
                    CreatedOn = DateTime.Now,                 
                };
                ApiResponseModel postuser = await APIServices.PostAsync(UserDetails, "UserProfile/AddUserAttendance");
                if (postuser.code == 200)
                {
                    return Ok(new { Message = "User add  Successfully!", Code = postuser.code });
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
    }
}