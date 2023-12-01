
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

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        public UserLoginController(IUserLoginServices userLogin)
        {
            UserLogin = userLogin;
        }

        public IUserLoginServices UserLogin { get; }


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
    }
}
