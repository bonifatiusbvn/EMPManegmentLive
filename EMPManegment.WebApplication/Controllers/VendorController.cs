using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.Web.Helper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EMPManegment.EntityModels.ViewModels.VendorModels;

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

        
        public async Task<IActionResult> AddVandorDetails(VendorDetailsView vendor)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync(vendor, "AddVendor/AddVendors");
                if (postuser.code == 200)
                {
                    var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, vendor.VendorName) }, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return View();
                }
                else
                {
                    TempData["ErrorMessage"] = postuser.message;
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
