using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using EMPManegment.Web.Helper;
using System.Security.Claims;

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
        public async Task<IActionResult> UserActiveDecative(string UserName)
        {
            try
            {

                ApiResponseModel postuser = await APIServices.PostAsync(null, "UserDetails/ActiveDeactiveUsers?UserName=" + UserName);
                if (postuser.code == 200)
                {

                    return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });

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
        public async Task<IActionResult> ResetUserPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetUserPassword(PasswordResetResponseModel emp)
        {
            try
            {
                Crypto.Hash(emp.Password,
                   out byte[] passwordHash,
                   out byte[] passwordSalt);
                var data = new PasswordResetView
                {
                    UserName = emp.UserName,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };
                ApiResponseModel postuser = await APIServices.PostAsync(data, "UserDetails/ResetUserPassword");
                if (postuser.code == 200)
                {
                    return Ok(new { Message = postuser.message, Code = postuser.code });
                }
                else
                {
                    return new JsonResult(new { Message = string.Format(postuser.message), Code = postuser.code });
                }
            }catch (Exception ex)  
            { 
                throw ex;
            }
        }
    }
}
