using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using EMPManegment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using X.PagedList;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Aspose.Pdf.Operators;
using EMPManegment.Web.Helper;
using Microsoft.AspNetCore.Authorization;
#nullable disable
namespace EMPManegment.Web.Controllers
{
    [Authorize]
    public class ExpenseMasterController : Controller
    {
        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }
        public UserSession _userSession { get; }

        public ExpenseMasterController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices, UserSession userSession)
        {
            WebAPI = webAPI;
            Environment = environment;
            APIServices = aPIServices;
            _userSession = userSession;
        }

        [FormPermissionAttribute("All Expenses List-View")]
        public async Task<IActionResult> Allexpense()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetExpenseDetailsList(bool? unapprove = null, DateTime? TodayDate = null)
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
                List<ExpenseDetailsView> expensedetails = new List<ExpenseDetailsView>();
                var data = new jsonData();
                ApiResponseModel postuser = await APIServices.PostAsync(dataTable, "ExpenseMaster/GetExpenseDetailList?unapprove="+ unapprove + "&TodayDate=" + TodayDate?.ToString("yyyy-MM-dd"));
                if (postuser.data != null)
                {
                    data = JsonConvert.DeserializeObject<jsonData>(postuser.data.ToString());
                    expensedetails = JsonConvert.DeserializeObject<List<ExpenseDetailsView>>(data.data.ToString());
                }
                var jsonData = new
                {
                    draw = data.draw,
                    recordsFiltered = data.recordsFiltered,
                    recordsTotal = data.recordsTotal,
                    data = expensedetails,
                };
                return new JsonResult(jsonData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [FormPermissionAttribute("Expenses-View")]
        public async Task<IActionResult> MyExpense()
        {
            return View();
        }

        [FormPermissionAttribute("All User Expenses-View")]
        public async Task<IActionResult> UserListforExpense()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserExpenseListTable()
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
                List<ExpenseDetailsView> Expense = new List<ExpenseDetailsView>();
                var data = new jsonData();
                Guid UserId = _userSession.UserId;
                ApiResponseModel response = await APIServices.PostAsync(dataTable, "ExpenseMaster/GetUserExpenseDetail?UserId=" + UserId);
                if (response.code == 200)
                {
                    data = JsonConvert.DeserializeObject<jsonData>(response.data.ToString());
                    Expense = JsonConvert.DeserializeObject<List<ExpenseDetailsView>>(data.data.ToString());
                }
                var jsonData = new
                {
                    draw = data.draw,
                    recordsFiltered = data.recordsFiltered,
                    recordsTotal = data.recordsTotal,
                    data = Expense,
                };
                return new JsonResult(jsonData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetUserListTable()
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
                List<UserExpenseDetailsView> Expense = new List<UserExpenseDetailsView>();
                var data = new jsonData();
                Guid UserId = _userSession.UserId;
                ApiResponseModel response = await APIServices.PostAsync(dataTable, "ExpenseMaster/GetUserList");
                if (response.code == 200)
                {
                    data = JsonConvert.DeserializeObject<jsonData>(response.data.ToString());
                    Expense = JsonConvert.DeserializeObject<List<UserExpenseDetailsView>>(data.data.ToString());
                }
                var jsonData = new
                {
                    draw = data.draw,
                    recordsFiltered = data.recordsFiltered,
                    recordsTotal = data.recordsTotal,
                    data = Expense,
                };
                return new JsonResult(jsonData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetExpenseTypeList()
        {
            try
            {
                List<ExpenseTypeView> ExpenseType = new List<ExpenseTypeView>();
                ApiResponseModel res = await APIServices.GetAsync("", "ExpenseMaster/GetAllExpensType");
                if (res.code == 200)
                {
                    ExpenseType = JsonConvert.DeserializeObject<List<ExpenseTypeView>>(res.data.ToString());
                }
                return new JsonResult(ExpenseType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetPaymentTypeList()
        {
            try
            {
                List<PaymentTypeView> PaymentType = new List<PaymentTypeView>();
                ApiResponseModel res = await APIServices.GetAsync("", "ExpenseMaster/GetAllPaymentType");
                if (res.code == 200)
                {
                    PaymentType = JsonConvert.DeserializeObject<List<PaymentTypeView>>(res.data.ToString());
                }
                return new JsonResult(PaymentType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [FormPermissionAttribute("Expenses-Add,All Expenses List-Add,All User Expenses-Add,ApprovedExpense-Add")]
        [HttpPost]
        public async Task<IActionResult> AddexpenseDetails(ExpenseRequestModel Addexpense)
        {
            try
            {
                string userrole = _userSession.UserRoll;
                var ExpenseDetails = new ExpenseDetailsView
                {
                    UserId = Addexpense.UserId,
                    Role = userrole,
                    ExpenseType = Addexpense.ExpenseType,
                    PaymentType = Addexpense.PaymentType,
                    BillNumber = Addexpense.BillNumber,
                    Description = Addexpense.Description,
                    Date = Addexpense.Date,
                    TotalAmount = Addexpense.TotalAmount,
                    CreatedBy = _userSession.UserId,
                    Account = Addexpense.Account,
                };
                if (Addexpense.Image != null)
                {
                    var ExpenseImg = Guid.NewGuid() + "_" + Addexpense.Image.FileName;
                    var path = Environment.WebRootPath;
                    var filepath = "Content/Image/" + ExpenseImg;
                    var fullpath = Path.Combine(path, filepath);
                    UploadFile(Addexpense.Image, fullpath);
                    ExpenseDetails.Image = ExpenseImg;
                }
                else
                {
                    ExpenseDetails.Image = null;
                }
                ApiResponseModel postuser = await APIServices.PostAsync(ExpenseDetails, "ExpenseMaster/AddExpenseDetails");
                UserResponceModel responseModel = new UserResponceModel();
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

        [FormPermissionAttribute("GetPayExpense-Add")]
        [HttpPost]
        public async Task<IActionResult> PayExpense(ExpenseDetailsView Addexpense)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync(Addexpense, "ExpenseMaster/AddExpenseDetails");
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
        public void UploadFile(IFormFile ImageFile, string ImagePath)
        {
            FileStream stream = new FileStream(ImagePath, FileMode.Create);
            ImageFile.CopyTo(stream);
        }

        [HttpGet]
        public async Task<JsonResult> EditExpenseDetails(Guid ExpenseId)
        {
            try
            {
                ExpenseDetailsView ExpenseDetails = new ExpenseDetailsView();
                ApiResponseModel response = await APIServices.GetAsync("", "ExpenseMaster/GetExpenseDetailById?Id=" + ExpenseId);
                if (response.code == 200)
                {
                    ExpenseDetails = JsonConvert.DeserializeObject<ExpenseDetailsView>(response.data.ToString());
                }
                return new JsonResult(ExpenseDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [FormPermissionAttribute("Expenses-Edit,All Expenses List-Edit,All User Expenses-Edit,ApprovedExpense-Edit")]
        [HttpPost]
        public async Task<IActionResult> UpdateExpenseDetails(ExpenseDetailsView ExpenseDetails)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync(ExpenseDetails, "ExpenseMaster/UpdateExpenseDetails");
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
        public async Task<IActionResult> GetExpenseDetailsByUserId()
        {
            try
            {
                var ExpenseDetails = HttpContext.Request.Form["USERID"];
                var InsertDetails = JsonConvert.DeserializeObject<ExpenseDetailsView>(ExpenseDetails);
                List<ExpenseDetailsView> expense = new List<ExpenseDetailsView>();
                ApiResponseModel response = await APIServices.PostAsync("", "ExpenseMaster/GetExpenseDetailByUserId?UserId=" + InsertDetails.UserId);
                if (response.code == 200)
                {
                    expense = JsonConvert.DeserializeObject<List<ExpenseDetailsView>>(response.data.ToString());
                }
                expense = expense.Where(expense => expense.IsApproved == true).ToList();
                return new JsonResult(expense);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [FormPermissionAttribute("ApprovedExpense-View")]
        public IActionResult ApprovedExpense(Guid UserId, string UserName)
        {
            HttpContext.Session.SetString("UserId", UserId.ToString());
            HttpContext.Session.SetString("UserName", UserName.ToString());
            ViewBag.UserId = HttpContext.Session.GetString("UserId");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApproveExpense()
        {
            try
            {
                ApprovedExpense expense = new ApprovedExpense();
                expense.ApprovedBy = _userSession.UserId;
                expense.ApprovedByName = _userSession.FullName;
                var ExpenseId = HttpContext.Request.Form["EXPENSEID"];
                List<string> expenseIdList = ExpenseId.ToString().Split(',').ToList();
                List<ApprovedExpense> UpdateExpense = new List<ApprovedExpense>();


                foreach (string id in expenseIdList)
                {
                    Guid expenseId;
                    if (!Guid.TryParse(id, out expenseId))
                    {
                        continue;
                    }
                    UpdateExpense.Add(new ApprovedExpense
                    {
                        Id = expenseId,
                        ApprovedBy = expense.ApprovedBy,
                        ApprovedByName = expense.ApprovedByName,
                    });
                }
                ApiResponseModel postuser = await APIServices.PostAsync(UpdateExpense, "ExpenseMaster/ApprovedExpense");
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

        [FormPermissionAttribute("Expenses-Delete,All Expenses List-Delete,All User Expenses-Delete,ApprovedExpense-Delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteExpense(Guid Id)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync("", "ExpenseMaster/DeleteExpense?Id=" + Id);
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

        [FormPermissionAttribute("GetPayExpense-View")]
        public async Task<IActionResult> PayExpense()
        {
            ViewBag.UserId = HttpContext.Session.GetString("UserId");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetUserExpenseList(Guid? UserId, string filterType = null, bool? unapprove = null, bool? approve = null, DateTime? startDate = null, DateTime? endDate = null, string account = null, string selectMonthlyExpense = null)
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
                List<ExpenseDetailsView> expense = new List<ExpenseDetailsView>();
                var data = new jsonData();
                ApiResponseModel postuser = await APIServices.PostAsync(dataTable, "ExpenseMaster/GetUserExpenseDetail?UserId=" + UserId + "&filterType=" + filterType + "&unapprove=" + unapprove + "&approve=" + approve + "&startDate=" + startDate?.ToString("yyyy-MM-dd") + "&endDate=" + endDate?.ToString("yyyy-MM-dd") + "&account="+ account + "&selectMonthlyExpense="+ selectMonthlyExpense);
                if (postuser.data != null)
                {
                    data = JsonConvert.DeserializeObject<jsonData>(postuser.data.ToString());
                    expense = JsonConvert.DeserializeObject<List<ExpenseDetailsView>>(data.data.ToString());
                }
                var jsonData = new
                {
                    draw = data.draw,
                    recordsFiltered = data.recordsFiltered,
                    recordsTotal = data.recordsTotal,
                    data = expense,
                };
                return new JsonResult(jsonData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult DisplayExpenseList()
        {
            return PartialView("~/Views/ExpenseMaster/_AllUserExpensePartial.cshtml");
        }

        public IActionResult DisplayUserExpenseList()
        {
            return PartialView("~/Views/ExpenseMaster/_UserExpenseListPartial.cshtml");
        }
        public IActionResult DisplayUserExpenseDetails()
        {
            return PartialView("~/Views/ExpenseMaster/_UserAllDetailsPartial.cshtml");
        }
        public IActionResult DisplayAllUserExpenseDetails()
        {
            return PartialView("~/Views/ExpenseMaster/_AllUserExpenseDetailsPartial.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> DownloadBill(string BillName)
        {
            try
            {
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Content/Image/", BillName);

                if (!System.IO.File.Exists(filepath))
                {
                    return NotFound();
                }

                byte[] bytes = await System.IO.File.ReadAllBytesAsync(filepath);
                string base64String = Convert.ToBase64String(bytes);

                return Json(new { memory = base64String, contentType = "application/pdf", fileName = BillName });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IActionResult> AddExpenseType(ExpenseTypeView ExpenseDetails)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync(ExpenseDetails, "ExpenseMaster/AddExpenseType");
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message, postuser.code });
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
    }
}
