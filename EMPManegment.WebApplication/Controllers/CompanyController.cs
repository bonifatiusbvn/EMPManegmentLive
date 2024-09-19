using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.Company;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using EMPManegment.Web.Helper;
using EMPManegment.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
#nullable disable
namespace EMPManegment.Web.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        public CompanyController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices, UserSession userSession)
        {
            WebAPI = webAPI;
            Environment = environment;
            APIServices = aPIServices;
            UserSession = userSession;
        }

        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }
        public UserSession UserSession { get; }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetCompanyNameList()
        {
            try
            {
                List<CompanyModel> CompanyName = new List<CompanyModel>();
                ApiResponseModel res = await APIServices.GetAsync("", "Company/GetCompanyNameList");
                if (res.code == 200)
                {
                    CompanyName = JsonConvert.DeserializeObject<List<CompanyModel>>(res.data.ToString());
                }
                return new JsonResult(CompanyName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetCompanyDetailsById(Guid CompanyId)
        {
            try
            {
                CompanyModel company = new CompanyModel();
                ApiResponseModel response = await APIServices.GetAsync("", "Company/GetCompanyDetailsById?Id=" + CompanyId);
                if (response.code == 200)
                {
                    company = JsonConvert.DeserializeObject<CompanyModel>(response.data.ToString());
                }
                return new JsonResult(company);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> CreateCompany(Guid? CompanyId, bool viewMode = false)
        {
            try
            {
                CompanyModel companyDetails = new CompanyModel();
                if (CompanyId != null)
                {
                    ApiResponseModel response = await APIServices.GetAsync("", "Company/GetCompanyDetailsById?Id=" + CompanyId);
                    if (response.code == 200)
                    {
                        companyDetails = JsonConvert.DeserializeObject<CompanyModel>(response.data.ToString());

                    }
                }
                ViewBag.ViewMode = viewMode;
                return View(companyDetails);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditCompanyDetails(string CompanyId)
        {
            try
            {

                return RedirectToAction("CreateCompany", new { CompanyId = CompanyId });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddCompany(CompanyRequestModel AddCompany)
        {
            try
            {
                var addCompany = new CompanyModel()
                {
                    CompnyName = AddCompany.CompnyName,
                    State = AddCompany.State,
                    City = AddCompany.City,
                    Country = AddCompany.Country,
                    PinCode = AddCompany.PinCode,
                    Address = AddCompany.Address,
                    Email = AddCompany.Email,
                    ContactNumber = AddCompany.ContactNumber,
                    Gst = AddCompany.Gst,
                    CreatedBy = UserSession.UserId,
                };
                if (AddCompany.CompanyLogo != null)
                {
                    var CompanyImg = Guid.NewGuid() + "_" + AddCompany.CompanyLogo.FileName;
                    var path = Environment.WebRootPath;
                    var filepath = "Content/Image/" + CompanyImg;
                    var fullpath = Path.Combine(path, filepath);
                    UploadFile(AddCompany.CompanyLogo, fullpath);

                    addCompany.CompanyLogo = CompanyImg;
                }
                else
                {
                    addCompany.CompanyLogo = null;
                }
                ApiResponseModel postuser = await APIServices.PostAsync(addCompany, "Company/AddCompany");
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
        public void UploadFile(IFormFile ImageFile, string ImagePath)
        {
            FileStream stream = new FileStream(ImagePath, FileMode.Create);
            ImageFile.CopyTo(stream);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCompanyDetails(CompanyRequestModel updateCompany)
        {
            try
            {
                var UpdateCompany = new CompanyModel()
                {
                    Id = updateCompany.Id,
                    CompnyName = updateCompany.CompnyName,
                    State = updateCompany.State,
                    City = updateCompany.City,
                    Country = updateCompany.Country,
                    PinCode = updateCompany.PinCode,
                    Address = updateCompany.Address,
                    Email = updateCompany.Email,
                    ContactNumber = updateCompany.ContactNumber,
                    Gst = updateCompany.Gst,
                    UpdatedBy = UserSession.UserId,
                };
                if (updateCompany.CompanyLogo != null)
                {
                    var CompanyImg = Guid.NewGuid() + "_" + updateCompany.CompanyLogo.FileName;
                    var path = Environment.WebRootPath;
                    var filepath = "Content/Image/" + CompanyImg;
                    var fullpath = Path.Combine(path, filepath);
                    UploadFile(updateCompany.CompanyLogo, fullpath);

                    UpdateCompany.CompanyLogo = CompanyImg;
                }
                else
                {
                    UpdateCompany.CompanyLogo = updateCompany.CompanyImageName;
                }
                ApiResponseModel postuser = await APIServices.PostAsync(UpdateCompany, "Company/UpdateCompanyDetails");
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

        public async Task<IActionResult> CompanyList()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetDatatableCompanyList()
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
                List<CompanyModel> vendorList = new List<CompanyModel>();
                var data = new jsonData();
                ApiResponseModel res = await APIServices.PostAsync(dataTable, "Company/GetDatatableCompanyList");
                if (res.code == 200)
                {
                    data = JsonConvert.DeserializeObject<jsonData>(res.data.ToString());
                    vendorList = JsonConvert.DeserializeObject<List<CompanyModel>>(data.data.ToString());
                }
                var jsonData = new
                {
                    draw = data.draw,
                    recordsFiltered = data.recordsFiltered,
                    recordsTotal = data.recordsTotal,
                    data = vendorList,
                };
                return new JsonResult(jsonData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCompanyDetails(Guid CompanyId)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync("", "Company/DeleteCompanyDetails?CompanyId=" + CompanyId);
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
    }
}
