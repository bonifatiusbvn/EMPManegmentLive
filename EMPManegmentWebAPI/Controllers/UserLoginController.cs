
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.Inretface.Services.UserLoginServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static Azure.Core.HttpHeader;
using System.Net;
using System.Reflection;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.ForgetPasswordModels;
using EMPManegment.Inretface.EmployeesInterface.AddEmployee;
using System.Linq.Dynamic.Core.Tokenizer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        public UserLoginController(IUserLoginServices userLogin,IAuthentication authentication, IWebHostEnvironment webHostEnvironment)
        {
            UserLogin = userLogin;
            Authentication = authentication;
            WebHostEnvironment = webHostEnvironment;
        }

        public IUserLoginServices UserLogin { get; }
        public IAuthentication Authentication { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest login)
        {
            LoginResponseModel loginresponsemodel = new LoginResponseModel();
            try
            {
               
                var result = await UserLogin.LoginUser(login);
               
                if (result != null && result.Data != null)
                {

                    loginresponsemodel.Code = (int)HttpStatusCode.OK;
                    loginresponsemodel.Data = result.Data;
                    loginresponsemodel.Message = result.Message;
                }
                else
                {
                    loginresponsemodel.Message = result.Message;
                    loginresponsemodel.Code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                loginresponsemodel.Code = (int)HttpStatusCode.InternalServerError;
            }
            return StatusCode(loginresponsemodel.Code, loginresponsemodel);
        }

        [HttpPost]
        [Route("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(SendEmailModel ForgetPassword)
        {
            ApiResponseModel responseModel = new ApiResponseModel();
            var forgetPassword = await Authentication.FindByEmailAsync(ForgetPassword);
            try
            {

                if (forgetPassword.Code == 200)
                {
                    string path = "F:/BonifatiusLive/EMPManegment.WebApplication/Views/Authentication/PasswordResetTemplate.cshtml";
                    string htmlString = System.IO.File.ReadAllText(path);
                    htmlString = htmlString.Replace("{{title}}", "Reset Password");
                    htmlString = htmlString.Replace("{{UserName}}",forgetPassword.Data.UserName);
                    htmlString = htmlString.Replace("{{FirstName}}", forgetPassword.Data.FirstName);
                    htmlString = htmlString.Replace("{{LastName}}", forgetPassword.Data.LastName);
                    htmlString = htmlString.Replace("{{url}}", "https://localhost:7204/UserProfile/ResetUserPassword");
                    bool status = await Authentication.EmailSendAsync(ForgetPassword.Email, "Click Here to Reset Your Password ", htmlString);
                    responseModel.code = (int)HttpStatusCode.OK;
                    responseModel.message = forgetPassword.Message;
                }
                else
                {
                    responseModel.message = forgetPassword.Message;
                    responseModel.code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                responseModel.code = (int)HttpStatusCode.InternalServerError;
            }
            return StatusCode(responseModel.code, responseModel);
        }
    }
}
