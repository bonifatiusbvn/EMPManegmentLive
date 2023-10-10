
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
        public async Task<IActionResult> Login(LoginRequest request)
        {
            LoginResponseModel apiResponseModel = new LoginResponseModel();
            try
            {
               
                var result = await UserLogin.LoginUser(request);
               
                if (result != null && result.Data != null)
                {
                   
                    apiResponseModel.Code = (int)HttpStatusCode.OK;
                    apiResponseModel.Data = result.Data;
                    apiResponseModel.Message = result.Message;
                }
                else
                {
                    apiResponseModel.Message = result.Message;
                    apiResponseModel.Code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                apiResponseModel.Code = (int)HttpStatusCode.InternalServerError;
            }
            return StatusCode(apiResponseModel.Code, apiResponseModel);
        }
    }
}
