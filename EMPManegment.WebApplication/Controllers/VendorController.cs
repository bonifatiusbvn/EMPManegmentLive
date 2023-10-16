using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.Web.Helper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using Newtonsoft.Json;

namespace EMPManegment.Web.Controllers
{
    public class VendorController : Controller
    {
        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }

        public VendorController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices)
        {
            WebAPI = webAPI;
            Environment = environment;
            APIServices = aPIServices;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddVandorDetails()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddVandorDetails(VendorDetailsView vendor)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    ApiResponseModel postuser = await APIServices.PostAsync(vendor, "AddVendor/AddVendors");
                    if (postuser.code == 200)
                    {

                        return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });

                    }
                    else
                    {
                        return new JsonResult(new { Message = string.Format(postuser.message), Code = postuser.code });
                    }
                }
                else
                {
                    return View(vendor);
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public async Task<IActionResult> DisplayVendorList()
        {
            try
            {
                List<VendorDetailsView> vendorList = new List<VendorDetailsView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel res = await APIServices.GetAsync("", "AddVendor/GetVendorList");
                if (res.code == 200)
                {
                    vendorList = JsonConvert.DeserializeObject<List<VendorDetailsView>>(res.data.ToString());
                }
                return View(vendorList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
