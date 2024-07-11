using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using Newtonsoft.Json;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.UserModels;
using EMPManegment.EntityModels.ViewModels;
using Microsoft.AspNetCore.Identity;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using Microsoft.AspNetCore.Authorization;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Web.Models;
using EMPManegment.Web.Helper;

namespace EMPManegment.Web.Controllers
{
    [Authorize]
    public class VendorController : Controller
    {
        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }
        public UserSession _userSession { get; }

        public VendorController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices, UserSession userSession)
        {
            WebAPI = webAPI;
            Environment = environment;
            APIServices = aPIServices;
            _userSession = userSession;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateVendor(Guid? VId, bool viewMode = false)
        {
            try
            {
                VendorDetailsView vendorDetails = new VendorDetailsView();
                if (VId != null)
                {
                    ApiResponseModel response = await APIServices.GetAsync("", "Vendor/GetVendorDetailsById?vendorId=" + VId);
                    if (response.code == 200)
                    {
                        vendorDetails = JsonConvert.DeserializeObject<VendorDetailsView>(response.data.ToString());

                    }
                }
                ViewBag.ViewMode = viewMode;
                return View(vendorDetails);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditVendorDetails(string VId)
        {
            try
            {

                return RedirectToAction("CreateVendor", new { Vid = VId });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [FormPermissionAttribute("Vendor List-Add")]
        [HttpPost]
        public async Task<IActionResult> AddVandorDetail(AddVendorDetailsView addVandorDetails)
        {
            try
            {
                var addVandor = new VendorDetailsView()
                {
                    VendorFirstName = addVandorDetails.VendorFirstName,
                    VendorLastName = addVandorDetails.VendorLastName,
                    VendorContectNo = addVandorDetails.VendorContectNo,
                    VendorPhone = addVandorDetails.VendorPhone,
                    VendorEmail = addVandorDetails.VendorEmail,
                    VendorCountry = addVandorDetails.VendorCountry,
                    VendorState = addVandorDetails.VendorState,
                    VendorCity = addVandorDetails.VendorCity,
                    VendorAddress = addVandorDetails.VendorAddress,
                    VendorPinCode = addVandorDetails.VendorPinCode,
                    VendorCompany = addVandorDetails.VendorCompany,
                    VendorCompanyType = addVandorDetails.VendorCompanyType,
                    VendorCompanyEmail = addVandorDetails.VendorCompanyEmail,
                    VendorCompanyNumber = addVandorDetails.VendorCompanyNumber,
                    VendorBankName = addVandorDetails.VendorBankName,
                    VendorBankBranch = addVandorDetails.VendorBankBranch,
                    VendorAccountHolderName = addVandorDetails.VendorAccountHolderName,
                    VendorBankAccountNo = addVandorDetails.VendorBankAccountNo,
                    VendorGstnumber = addVandorDetails.VendorGstnumber,
                    VendorBankIfsc = addVandorDetails.VendorBankIfsc,
                    VendorTypeId = addVandorDetails.VendorTypeId,
                    CreatedBy = _userSession.FirstName,
                };
                if (addVandorDetails.VendorCompanyLogo != null)
                {
                    var VendorImg = Guid.NewGuid + "_" + addVandorDetails.VendorCompanyLogo.FileName;
                    var path = Environment.WebRootPath;
                    var filepath = "Content/Image/" + VendorImg;
                    var fullpath = Path.Combine(path, filepath);
                    UploadFile(addVandorDetails.VendorCompanyLogo, fullpath);

                    addVandor.VendorCompanyLogo = VendorImg;
                }
                else
                {
                    addVandor.VendorCompanyLogo = null;
                }
                ApiResponseModel postuser = await APIServices.PostAsync(addVandor, "Vendor/CreateVendors");
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

        [HttpGet]
        public async Task<JsonResult> GetVendorType()
        {

            try
            {
                List<VendorTypeView> VendorType = new List<VendorTypeView>();
                ApiResponseModel response = await APIServices.GetAsyncId(null, "Vendor/GetVendorType");
                if (response.code == 200)
                {
                    VendorType = JsonConvert.DeserializeObject<List<VendorTypeView>>(response.data.ToString());
                }
                return new JsonResult(VendorType);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [FormPermissionAttribute("Vendor List-View")]
        [HttpGet]
        public async Task<IActionResult> VendorList()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetVendorList()
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
                List<VendorDetailsView> vendorList = new List<VendorDetailsView>();
                var data = new jsonData();
                ApiResponseModel res = await APIServices.PostAsync(dataTable, "Vendor/GetVendorList");
                if (res.code == 200)
                {
                    data = JsonConvert.DeserializeObject<jsonData>(res.data.ToString());
                    vendorList = JsonConvert.DeserializeObject<List<VendorDetailsView>>(data.data.ToString());
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
        [HttpGet]
        public async Task<JsonResult> GetVendorDetailsById(Guid VendorId)
        {
            try
            {
                VendorDetailsView vendordetails = new VendorDetailsView();
                ApiResponseModel response = await APIServices.GetAsync("", "Vendor/GetVendorDetailsById?vendorId=" + VendorId);
                if (response.code == 200)
                {
                    vendordetails = JsonConvert.DeserializeObject<VendorDetailsView>(response.data.ToString());
                }
                return new JsonResult(vendordetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [FormPermissionAttribute("Vendor List-Edit")]
        [HttpPost]
        public async Task<IActionResult> UpdateVendorDetails(AddVendorDetailsView VendorDetails)
        {
            try
            {
                var updateVendor = new VendorDetailsView()
                {
                    Vid  = VendorDetails.Vid,
                    VendorFirstName = VendorDetails.VendorFirstName,
                    VendorLastName = VendorDetails.VendorLastName,
                    VendorContectNo = VendorDetails.VendorContectNo,
                    VendorPhone = VendorDetails.VendorPhone,
                    VendorEmail = VendorDetails.VendorEmail,
                    VendorCountry = VendorDetails.VendorCountry,
                    VendorState = VendorDetails.VendorState,
                    VendorCity = VendorDetails.VendorCity,
                    VendorAddress = VendorDetails.VendorAddress,
                    VendorPinCode = VendorDetails.VendorPinCode,
                    VendorCompany = VendorDetails.VendorCompany,
                    VendorCompanyType = VendorDetails.VendorCompanyType,
                    VendorCompanyEmail = VendorDetails.VendorCompanyEmail,
                    VendorCompanyNumber = VendorDetails.VendorCompanyNumber,
                    VendorBankName = VendorDetails.VendorBankName,
                    VendorBankBranch = VendorDetails.VendorBankBranch,
                    VendorAccountHolderName = VendorDetails.VendorAccountHolderName,
                    VendorBankAccountNo = VendorDetails.VendorBankAccountNo,
                    VendorGstnumber = VendorDetails.VendorGstnumber,
                    VendorBankIfsc = VendorDetails.VendorBankIfsc,
                    VendorTypeId = VendorDetails.VendorTypeId,
                    UpdatedBy = _userSession.UserId,
                };
                if (VendorDetails.VendorCompanyLogo != null)
                {
                    var VendorImg = Guid.NewGuid+"_"+ VendorDetails.VendorCompanyLogo.FileName;
                    var path = Environment.WebRootPath;
                    var filepath = "Content/Image/" + VendorImg;
                    var fullpath = Path.Combine(path, filepath);
                    UploadFile(VendorDetails.VendorCompanyLogo, fullpath);

                    updateVendor.VendorCompanyLogo = VendorImg;
                }
                else
                {
                    updateVendor.VendorCompanyLogo = VendorDetails.VendorImageName;
                }

                ApiResponseModel postuser = await APIServices.PostAsync(updateVendor, "Vendor/UpdateVendorDetails");
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
