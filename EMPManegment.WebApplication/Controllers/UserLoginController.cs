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

                    ApiResponseModel responsemodel = await APIServices.PostAsync(login, "UserLogin/Login");
                    LoginResponseModel userlogin = new LoginResponseModel();
                    if (responsemodel.code != (int)HttpStatusCode.OK)
                    {
                       

                        if (responsemodel.code == (int)HttpStatusCode.Forbidden)
                        { 
                            TempData["ErrorMessage"] = responsemodel.message;
                            return Ok(new { Message = string.Format(responsemodel.message), Code = responsemodel.code });
                        }
                        else
                        {
                            TempData["ErrorMessage"] = responsemodel.message;
                        }
                    }

                    else
                    {
                        var data = JsonConvert.SerializeObject(responsemodel.data);
                        userlogin.Data = JsonConvert.DeserializeObject<LoginView>(data);
                        var claims = new List<Claim>()
                        {
                            new Claim("UserID", userlogin.Data.Id.ToString()),
                            new Claim("FirstName", userlogin.Data.FirstName),
                            new Claim("Fullname", userlogin.Data.FullName),
                            new Claim("UserName", userlogin.Data.UserName),
                            new Claim("ProfileImage", userlogin.Data.ProfileImage),
                            new Claim("IsAdmin", userlogin.Data.Role.ToString()),

                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        HttpContext.Session.SetString("UserID", userlogin.Data.Id.ToString());
                        HttpContext.Session.SetString("FirstName", userlogin.Data.FirstName);
                        HttpContext.Session.SetString("FullName", userlogin.Data.FullName);
                        HttpContext.Session.SetString("UserName", userlogin.Data.UserName);
                        HttpContext.Session.SetString("ProfileImage", userlogin.Data.ProfileImage);
                        HttpContext.Session.SetString("IsAdmin", userlogin.Data.Role.ToString());
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
