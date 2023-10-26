using Azure;
using EMPManagment.API;
using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EMPManegment.Web.Controllers
{
    public class UserDetailsController : Controller
    {

        public WebAPI WebAPI { get; }
        public APIServices APIServices { get; }
        public IWebHostEnvironment Environment { get; }
        public UserDetailsController(WebAPI webAPI, APIServices aPIServices, IWebHostEnvironment environment)
        {
            WebAPI = webAPI;
            APIServices = aPIServices;
            Environment = environment;
        }


        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> DisplayUserList()
        {
            try
            {
                List<EmpDetailsView> userList = new List<EmpDetailsView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel res = await APIServices.GetAsync("", "UserDetails/GetAllUserList");
                if (res.code == 200)
                {
                    userList = JsonConvert.DeserializeObject<List<EmpDetailsView>>(res.data.ToString());
                }
                return View(userList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IActionResult> UserEditList()
        {
            try
            {
                List<EmpDetailsView> userList = new List<EmpDetailsView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel res = await APIServices.GetAsync("", "UserDetails/UserEdit");
                if (res.code == 200)
                {
                    userList = JsonConvert.DeserializeObject<List<EmpDetailsView>>(res.data.ToString());
                }
                return View(userList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> UserActiveDecative()
        {
            try
            {
                List<EmpDetailsView> userList = new List<EmpDetailsView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel res = await APIServices.GetAsync("", "UserDetails/GetAllUserList");
                if (res.code == 200)
                {
                    userList = JsonConvert.DeserializeObject<List<EmpDetailsView>>(res.data.ToString());
                }
                return View(userList);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public async Task<IActionResult> UserActiveDecative(string Id)
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> Edit(Guid id)
        {
            try
            {
                EmpDetailsView emp = new EmpDetailsView();
                HttpClient client = WebAPI.Initil();
                HttpResponseMessage res = await client.GetAsync("UserDetails/GetEmployee?id=" + id);
                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    emp = JsonConvert.DeserializeObject<EmpDetailsView>(result);
                }
                return new JsonResult(emp);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<JsonResult>Update(UserEditViewModel employee)
        {
            var emp = new UserEditViewModel()
            {
                Id = employee.Id,
                DepartmentId = employee.DepartmentId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Address = employee.Address,
                CityId = employee.CityId,
                StateId = employee.StateId,
                CountryId = employee.CountryId,
                PhoneNumber = employee.PhoneNumber,
                DateOfBirth = employee.DateOfBirth,
                Gender = employee.Gender,

        };
            try
            {
                ApiResponseModel postUser = await APIServices.PostAsync(emp, "UserDetails/Update");
                if(postUser.code == 200)
                {
                    TempData["SuccesMessage"] = "Data Successfully Updated!!";
                    return new JsonResult(emp);
                }
                else
                {
                    TempData["ErrorMessage"] = "Opps!! There is some Problem in your request";
                    return new JsonResult(employee);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
