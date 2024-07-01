using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using EMPManegment.EntityModels.Crypto;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using EMPManegment.EntityModels.ViewModels.ForgetPasswordModels;
using DocumentFormat.OpenXml.Spreadsheet;
using EMPManegment.EntityModels.Common;
using EMPManegment.Web.Helper;

namespace EMPManegment.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        public AuthenticationController(WebAPI webAPI, APIServices aPIServices, IWebHostEnvironment environment)
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
            if (Request.Cookies["UserName"] != null && Request.Cookies["Password"] != null)
            {
                ViewBag.UserName = (Request.Cookies["UserName"].ToString());
                var pwd = Request.Cookies["Password"].ToString();
                ViewBag.Password = pwd;
                ViewBag.checkRememberMe = true;

            }
            
            return View();
        }
        public async Task<IActionResult> ForgetPassword()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> ForgetPassword(SendEmailModel forgetpass)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync(forgetpass, "User/ForgetPassword");
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

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApiResponseModel responsemodel = await APIServices.PostAsync(login, "User/Login");
                    LoginResponseModel userlogin = new LoginResponseModel();

                    if (responsemodel.code != (int)HttpStatusCode.OK)
                    {
                        if (responsemodel.code == (int)HttpStatusCode.Forbidden)
                        {
                            TempData["ErrorMessage"] = responsemodel.message;
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
                        var defaultProfileImage = "~/content/image/image-b2.jpg";


                        var claims = new List<Claim>()
                        {
                            new Claim("UserID", userlogin.Data.Id.ToString()),
                            new Claim("FirstName", userlogin.Data.FirstName),
                            new Claim("LastName", userlogin.Data.LastName),
                            new Claim("FullName", userlogin.Data.FullName),
                            new Claim("UserName", userlogin.Data.UserName),
                            new Claim("ProfileImage", string.IsNullOrEmpty(userlogin.Data.ProfileImage) ? defaultProfileImage : userlogin.Data.ProfileImage),
                            new Claim("IsAdmin", userlogin.Data.Role),
                            new Claim("AccessToken", userlogin.Data.Token),
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        if (login.RememberMe)
                        {
                            CookieOptions cookie = new CookieOptions();
                            cookie.Expires = DateTime.UtcNow.AddDays(7);
                            Response.Cookies.Append("UserName", userlogin.Data.UserName, cookie);
                            Response.Cookies.Append("Password", login.Password, cookie);
                            ViewBag.checkRememberMe = true;
                        }
                        else
                        {
                            Response.Cookies.Delete("UserName");
                            Response.Cookies.Delete("Password");
                        }

                        UserSession.ProfilePhoto = string.IsNullOrEmpty(userlogin.Data.ProfileImage) ? defaultProfileImage : userlogin.Data.ProfileImage;
                        UserSession.FormPermisionData = userlogin.Data.FromPermissionData;

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                        return RedirectToAction("UserHome", "Home");
                    }
                }
                ViewBag.UserName = login.UserName;
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "InternalServer" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                HttpContext.Session.Clear();
                await HttpContext.SignOutAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Login");
        }

        public async Task<JsonResult> GetDepartment()
        {

            try
            {
                List<Department> getDepartment = new List<Department>();
                ApiResponseModel response = await APIServices.GetAsync(null, "MasterList/GetDepartment");
                if (response.code == 200)
                {
                    getDepartment = JsonConvert.DeserializeObject<List<Department>>(response.data.ToString());
                }
                return new JsonResult(getDepartment);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<JsonResult> GetCountrys()
        {

            try
            {
                List<CountryView> countries = new List<CountryView>();
                ApiResponseModel response = await APIServices.GetAsyncId(null, "MasterList/GetCountries");
                if (response.code == 200)
                {
                    countries = JsonConvert.DeserializeObject<List<CountryView>>(response.data.ToString());
                }
                return new JsonResult(countries);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<JsonResult> GetState(int StateId)
        {

            try
            {
                List<StateView> states = new List<StateView>();
                ApiResponseModel response = await APIServices.GetAsyncId(null, "MasterList/GetState?StateId=" + StateId);
                if (response.code == 200)
                {
                    states = JsonConvert.DeserializeObject<List<StateView>>(response.data.ToString());
                }
                return new JsonResult(states);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<JsonResult> GetCity(int CityId)
        {

            try
            {
                List<CityView> cities = new List<CityView>();
                ApiResponseModel response = await APIServices.GetAsyncId(null, "MasterList/GetCities?CityId=" + CityId);
                if (response.code == 200)
                {
                    cities = JsonConvert.DeserializeObject<List<CityView>>(response.data.ToString());
                }
                return new JsonResult(cities);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<JsonResult> GetQuestion()
        {

            try
            {
                List<QuestionView> questions = new List<QuestionView>();
                ApiResponseModel response = await APIServices.GetAsync(null, "MasterList/GetQuestion");
                if (response.code == 200)
                {
                    questions = JsonConvert.DeserializeObject<List<QuestionView>>(response.data.ToString());
                }
                return new JsonResult(questions);

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

    }
}

