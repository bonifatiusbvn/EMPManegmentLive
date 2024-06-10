using Azure;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.ForgetPasswordModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Services.AddEmployeeServies;
using EMPManegment.Inretface.Services.UserLoginServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        public IAuthenticationServices Authentication { get; }

        public UserController(IAuthenticationServices authentication)
        {
            Authentication = authentication;
        }




        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest login)
        {
            LoginResponseModel loginresponsemodel = new LoginResponseModel();
            try
            {

                var result = await Authentication.LoginUser(login);

                if (result != null && result.Data != null)
                {

                    loginresponsemodel.Code = (int)HttpStatusCode.OK;
                    loginresponsemodel.Data = result.Data;
                    loginresponsemodel.Message = result.Message;
                }
                else
                {
                    loginresponsemodel.Message = result.Message;
                    loginresponsemodel.Code = result.Code;
                }
            }
            catch (Exception ex)
            {
                loginresponsemodel.Code = (int)HttpStatusCode.InternalServerError;
                loginresponsemodel.Message = "An error occurred while processing the request.";
            }
            return StatusCode(loginresponsemodel.Code, loginresponsemodel);
        }

        [HttpGet]
        [Route("CheckUser")]
        [Authorize]
        public IActionResult CheckUser()
        {
            var checkUser = Authentication.CheckEmloyess();
            return Ok(new { code = 200, data = checkUser });
        }

        [HttpPost]
        [Route("UserSingUp")]
        [Authorize]
        public async Task<IActionResult> UserSingUp(EmpDetailsView AddEmployee)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var addEmployee = Authentication.UserSingUp(AddEmployee);
                if (addEmployee.Result.Code == 200)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message =  addEmployee.Result.Message;

                }
                else
                {
                    response.Code = addEmployee.Result.Code;
                    response.Message = addEmployee.Result.Message;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "An error occurred while processing the request.";
            }
            return StatusCode(response.Code, response);
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
                    htmlString = htmlString.Replace("{{UserName}}", forgetPassword.Data.UserName);
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
                responseModel.message = "An error occurred while processing the request.";
            }
            return StatusCode(responseModel.code, responseModel);
        }

    }
}
