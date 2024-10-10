
using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using EMPManegment.EntityModels.Crypto;

namespace EMPManegment.WebApplication.Controllers
{
    [AllowAnonymous]
    public class EmpAddDetailsController : Controller
    {

        public EmpAddDetailsController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices)
        {
            WebAPI = webAPI;
            Environment = environment;
            APIServices = aPIServices;
        }

        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> AddEmpDetail()
        {
            try
            { 
                HttpClient client = WebAPI.Initil();
                ApiResponseModel AddUserResponse = await APIServices.GetAsync("","AddEmp/CheckUser");
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
        public async Task<IActionResult> AddEmpDetail(LoginDetailsView AddEmployee)
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
                    ApiResponseModel postuser = await APIServices.PostAsync(AddUser, "AddEmp/AddEmployees");
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
                ApiResponseModel addEmpResponse = await APIServices.GetAsync("", "AddEmp/CheckUser");
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
                ApiResponseModel response = await APIServices.GetAsync(null,"AddEmp/GetDepartment");
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
                ApiResponseModel response = await APIServices.GetAsyncId(null,"CSC/GetCountries");
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
                ApiResponseModel response = await APIServices.GetAsyncId(null, "CSC/GetState?StateId="+StateId);
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
                ApiResponseModel response = await APIServices.GetAsyncId(null, "CSC/GetCities?CityId="+ CityId);
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
                ApiResponseModel response = await APIServices.GetAsync(null, "CSC/GetQuestion");
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
