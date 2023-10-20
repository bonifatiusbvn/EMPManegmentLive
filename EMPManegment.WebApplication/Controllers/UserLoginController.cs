using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using static Azure.Core.HttpHeader;
using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.View_Model;

namespace EMPManegment.Web.Controllers
{
    public class UserLoginController : Controller
    {

        public UserLoginController(WebAPI webAPI, APIServices aPIServices, IWebHostEnvironment environment)
        {
            WebAPI = webAPI;
            APIServices = aPIServices;
            Environment = environment;
        }

        public WebAPI WebAPI { get; }
        public APIServices APIServices { get; }
        public IWebHostEnvironment Environment { get; }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Login()
        {
                return View();
        }
        

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest login)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    ApiResponseModel response = await APIServices.PostAsync(login, "UserLogin/Login");
                    LoginResponseModel usermodel = new LoginResponseModel();
                    if (response.code != (int)HttpStatusCode.OK)
                    {
                       

                        if (response.code == (int)HttpStatusCode.Forbidden)
                        { 
                            TempData["ErrorMessage"] = response.message;
                            return Ok(new { Message = string.Format(response.message), Code = response.code });
                        }
                        else
                        {
                            TempData["ErrorMessage"] = response.message;
                        }
                    }

                    else
                    {
                        var data = JsonConvert.SerializeObject(response.data);
                        usermodel.Data = JsonConvert.DeserializeObject<LoginView>(data);
                        var claims = new List<Claim>()
                        {
                            new Claim("UserID", usermodel.Data.Id.ToString()),
                            new Claim("FirstName", usermodel.Data.FirstName),
                            new Claim("Fullname", usermodel.Data.FullName),
                            new Claim("UserName", usermodel.Data.UserName),
                            new Claim("ProfileImage", usermodel.Data.ProfileImage),
                            new Claim("IsAdmin", usermodel.Data.IsAdmin.ToString()),

                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        HttpContext.Session.SetString("UserID", usermodel.Data.Id.ToString());
                        HttpContext.Session.SetString("FirstName", usermodel.Data.FirstName);
                        HttpContext.Session.SetString("FullName", usermodel.Data.FullName);
                        HttpContext.Session.SetString("UserName", usermodel.Data.UserName);
                        HttpContext.Session.SetString("ProfileImage", usermodel.Data.ProfileImage);
                        HttpContext.Session.SetString("IsAdmin", usermodel.Data.IsAdmin.ToString());
                        return RedirectToAction("UserHome", "Home");
                    }  
                   
                }

                return View();

            }
             
            catch (Exception ex)
            {
                return BadRequest(new { Message = "InternalServer" });
            }
        }
        public IActionResult Logout() 
        { 
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var StoredCookies = Request.Cookies.Keys;
            foreach (var Cookie in StoredCookies)
            {
                Response.Cookies.Delete(Cookie);
            }
            return RedirectToAction("Login", "UserLogin");
        }



    }
}
