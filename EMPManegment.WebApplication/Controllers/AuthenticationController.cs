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
                ApiResponseModel postuser = await APIServices.PostAsync(forgetpass, "UserLogin/ForgetPassword");
                if (postuser.code == 200)
                {
                    return new JsonResult(new { Message = string.Format(postuser.message), Code = postuser.code });
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

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest login)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    ApiResponseModel responsemodel = await APIServices.PostAsync(login, "UserLogin/Login");
                    LoginResponseModel userlogin = new LoginResponseModel();

                    if (login.RememberMe==true)
                    {
                        string UserNamecookie = login.UserName;
                        string Passwordcookie = login.Password;

                        CookieOptions cookieOptions = new CookieOptions
                        {
                            Expires = DateTime.Now.AddDays(7),
                            HttpOnly = true,
                            Secure = true, 
                            SameSite = SameSiteMode.Strict
                        };

                        Response.Cookies.Append("UserName", UserNamecookie, cookieOptions);
                        Response.Cookies.Append("Password", Passwordcookie, cookieOptions);
                        //CookieOptions cookie = new CookieOptions();
                        //cookie.Expires = DateTime.Now.AddYears(1);

                        //Response.Cookies.Append("UserName", login.UserName);
                        //Response.Cookies.Append("Password", login.Password);
                    }
                    else
                    {
                        Response.Cookies.Delete("UserName");
                        Response.Cookies.Delete("Password");
                    }

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
                                new Claim("FullName", userlogin.Data.FullName),
                                new Claim("UserName", userlogin.Data.UserName),
                                new Claim("ProfileImage", userlogin.Data.ProfileImage),
                                new Claim("IsAdmin", userlogin.Data.Role.ToString()),

                            };
                            UserSession.ProfilePhoto = userlogin.Data.ProfileImage;


                                 var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                                 var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                                 await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
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

        public async Task<IActionResult> UserSingUp()
        {
            try
            {
                HttpClient client = WebAPI.Initil();
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

        [HttpPost]
        public async Task<IActionResult> UserSingUp(LoginDetailsView AddEmployee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var path = Environment.WebRootPath;
                    var filepath = "Content/Image/" + AddEmployee.Image.FileName;
                    var fullpath = Path.Combine(path, filepath);
                    UploadFile(AddEmployee.Image, fullpath);
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
                        Image = filepath,
                        IsActive = AddEmployee.IsActive,
                    };
                    ApiResponseModel postuser = await APIServices.PostAsync(AddUser, "User/AddEmployees");
                    ViewBag.Name = HttpContext.Session.GetString("UserName");
                    if (postuser.code == 200)
                    {
                        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, AddEmployee.UserName) }, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        return RedirectToAction("Login", "UserLogin");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = postuser.message;
                        return View();
                    }
                }
                HttpClient client = WebAPI.Initil();
                ApiResponseModel addEmpResponse = await APIServices.GetAsync("", "User/CheckUser");
                if (addEmpResponse.code == 200)
                {
                    ViewBag.EmpId = addEmpResponse.data;
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<JsonResult> GetDepartment()
        {

            try
            {
                List<Department> getDepartment = new List<Department>();
                HttpClient client = WebAPI.Initil();
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
                HttpClient client = WebAPI.Initil();
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
                HttpClient client = WebAPI.Initil();
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
                HttpClient client = WebAPI.Initil();
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
                HttpClient client = WebAPI.Initil();
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

