using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
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

       

        [AllowAnonymous]
        [HttpPost("Login")]
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
                    loginresponsemodel.Code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                loginresponsemodel.Code = (int)HttpStatusCode.InternalServerError;
            }
            return StatusCode(loginresponsemodel.Code, loginresponsemodel);
        }

        [HttpGet]
        [Route("CheckUser")]
        public IActionResult CheckUser()
        {
            var checkUser = Authentication.CheckEmloyess();
            return Ok(new { code = 200, data = checkUser });
        }


       

        [HttpPost]
        [Route("UserSingUp")]
        public async Task<IActionResult> UserSingUp(EmpDetailsView AddEmployee)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var addEmployee = Authentication.UserSingUp(AddEmployee);
                if (addEmployee.Result.Code == 200)
                {
                    response.Code = (int)HttpStatusCode.OK;

                }
                else
                {
                    response.Message = addEmployee.Result.Message;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StatusCode(response.Code, response);
        }
    }
}
