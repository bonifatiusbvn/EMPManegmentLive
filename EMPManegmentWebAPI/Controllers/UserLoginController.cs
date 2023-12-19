
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

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        public UserLoginController(IUserLoginServices userLogin,IAuthentication authentication)
        {
            UserLogin = userLogin;
            Authentication = authentication;
        }

        public IUserLoginServices UserLogin { get; }
        public IAuthentication Authentication { get; }

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
            var forgetPassword = await Authentication.ForgetPassword(ForgetPassword);
            try
            {

                if (forgetPassword.Code == 200)
                {
                    bool status = await Authentication.EmailSendAsync(ForgetPassword.Email, "Click Here to Reset Your Password ", "https://localhost:7204/UserProfile/ResetUserPassword");
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
