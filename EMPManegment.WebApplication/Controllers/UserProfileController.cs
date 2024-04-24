
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
        public async Task<IActionResult> DisplayUserDetails(Guid Id)
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
        public async Task<IActionResult> UserActiveDecative()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> UserActiveDecativeList(string? searchby, string? searchfor, int? page)
        {
            try
            {
                List<EmpDetailsView> ActiveDecative = new List<EmpDetailsView>();
                ApiResponseModel res = await APIServices.GetAsync("", "UserProfile/GetActiveDeactiveUserList?searchby=" + searchby + "&searchfor=" + searchfor);
                if (res.code == 200)
                {
                    ActiveDecative = JsonConvert.DeserializeObject<List<EmpDetailsView>>(res.data.ToString());
                }
                int pageSize = 4;
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
                    CreatedBy = doc.CreatedBy,
                };

                ApiResponseModel postuser = await APIServices.PostAsync(uploadDocument, "UserProfile/UploadDocument");
                if (postuser.code == 200)
                {
                    return new JsonResult(postuser.message = "Document Uploaded Successfully!");
                }
                else
                {
                    return new JsonResult(postuser.message);
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
                ApiResponseModel postUser = await APIServices.PostAsync(Updateuser, "UserProfile/UpdateUserDetails");
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
        public async Task<JsonResult> GetAttendanceList(SearchAttendanceModel GetAttendanceList)
        {
            try
            {
                List<UserAttendanceModel> getAttendanceList = new List<UserAttendanceModel>();
                var data = new SearchAttendanceModel
                {
                    UserId = _userSession.UserId,
                    Cmonth = GetAttendanceList.Cmonth,
                    StartDate = GetAttendanceList.StartDate,
                    EndDate = GetAttendanceList.EndDate,
                };
                TempData["Cmonth"] = GetAttendanceList.Cmonth;
                TempData["StartDate"] = GetAttendanceList.StartDate;
                TempData["EndDate"] = GetAttendanceList.EndDate;

                HttpClient client = WebAPI.Initil();

                ApiResponseModel response = await APIServices.PostAsync(data, "UserProfile/GetAttendanceList");
                if (response.data.Count != 0)
                {
                    getAttendanceList = JsonConvert.DeserializeObject<List<UserAttendanceModel>>(response.data.ToString());
                }
                else
                {
                    return new JsonResult(new { Message = "No Data Found On Selected Month Or Dates!!" });

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

        public async Task<IActionResult> ExportToPdf()
        {
            try
            {
                List<UserAttendanceModel> getAttendanceList = new List<UserAttendanceModel>();

                DateTime Cmonth = Convert.ToDateTime(TempData["Cmonth"]);
                DateTime StartDate = Convert.ToDateTime(TempData["StartDate"]);
                DateTime EndDate = Convert.ToDateTime(TempData["EndDate"]);
                IEnumerable<UserAttendanceModel> userAttendance = null;

                var data = new SearchAttendanceModel
                {
                    UserId = _userSession.UserId,
                    Cmonth = Cmonth,
                    StartDate = StartDate,
                    EndDate = EndDate,
                };

                HttpClient client = WebAPI.Initil();

                ApiResponseModel response = await APIServices.PostAsync(data, "UserProfile/GetAttendanceList");
                if (response.data.Count != 0)
                {
                    getAttendanceList = JsonConvert.DeserializeObject<List<UserAttendanceModel>>(response.data.ToString());
                    var document = new Document
                    {
                        PageInfo = new PageInfo { Margin = new MarginInfo(25, 25, 25, 40) }
                    };
                    var pdfPage = document.Pages.Add();
                    Aspose.Pdf.Table table = new Aspose.Pdf.Table()
                    {
                        ColumnWidths = "15% 16% 16% 16% 20% 16%",
                        DefaultCellPadding = new MarginInfo(5, 5, 5, 5),
                        Border = new BorderInfo(BorderSide.All, .5f, Aspose.Pdf.Color.Black),
                        DefaultCellBorder = new BorderInfo(BorderSide.All, .2f, Aspose.Pdf.Color.Black),
                    };
                    System.Data.DataTable dt = ToConvertDataTable(getAttendanceList.ToList());
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
                throw ex;
            }
        }

        public async Task<IActionResult> ExportToExcel()
        {
            try
            {
                List<UserAttendanceModel> getAttendanceList = new List<UserAttendanceModel>();

                DateTime Cmonth = Convert.ToDateTime(TempData["Cmonth"]);
                DateTime StartDate = Convert.ToDateTime(TempData["StartDate"]);
                DateTime EndDate = Convert.ToDateTime(TempData["EndDate"]);
                IEnumerable<UserAttendanceModel> userAttendance = null;

                var data = new SearchAttendanceModel
                {
                    UserId = _userSession.UserId,
                    Cmonth = Cmonth,
                    StartDate = StartDate,
                    EndDate = EndDate,
                };

                HttpClient client = WebAPI.Initil();

                ApiResponseModel response = await APIServices.PostAsync(data, "UserProfile/GetAttendanceList");
                if (response.data.Count != 0)
                {
                    getAttendanceList = JsonConvert.DeserializeObject<List<UserAttendanceModel>>(response.data.ToString());
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(ToConvertDataTable(getAttendanceList.ToList()));
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            string FileName = Guid.NewGuid() + "_AttendanceList.xlsx";
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocuments.spreadsheetml.sheet", FileName);

                        }
                    }
                }
                return RedirectToAction("GetAttendance");
            }
            catch (Exception ex)
            {
                throw ex;
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
                for (int i = 1; i < propInfo.Length; i++)
                {
                    values[i] = propInfo[i].GetValue(item, null);
                }
                dt.Rows.Add(values);
            }
            dt.Columns.Remove("UserId");
            dt.Columns.Remove("CreatedBy");
            dt.Columns.Remove("AttendanceId");
            return dt;
        }

        [HttpPost]
        public async Task<IActionResult> GetSearchAttendanceList(searchAttendanceListModel GetAttendanceList)
        {
            try
            {
                List<UserAttendanceModel> getAttendanceList = new List<UserAttendanceModel>();
                ApiResponseModel response = await APIServices.PostAsync(GetAttendanceList, "UserProfile/GetSearchAttendanceList");
                if (response.data.Count != 0)
                {
                    getAttendanceList = JsonConvert.DeserializeObject<List<UserAttendanceModel>>(response.data.ToString());
                    return PartialView("~/Views/UserProfile/_SearchAttendanceList.cshtml", getAttendanceList);
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
        [HttpGet]
        public IActionResult RolewisePermission()
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
                    return PartialView("~/Views/UserProfile/_RolewisePermissionPartial.cshtml", GetUserRoleList);
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
        public async Task<IActionResult> GetRolewiseFormListById(int RoleId)
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

        [HttpPost]
        public async Task<IActionResult> UpdateMultipleRolewiseFormPermission()
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
    }
}

