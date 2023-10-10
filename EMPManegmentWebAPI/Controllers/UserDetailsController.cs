using Azure;
using Azure.Core;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.UserList;
using EMPManegment.Inretface.Interface.UsersLogin;
using EMPManegment.Inretface.Services.UserListServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : ControllerBase
    {
        public IUserDetailsServices UserListServices { get; }
        public UserDetailsController(IUserDetailsServices userListServices)
        {
            UserListServices = userListServices;
        }


        [HttpGet]
        [Route("GetAllUserList")]

        public async Task<IActionResult> GetAllUserList()
        {
            IEnumerable<EmpDetailsView> userList = await UserListServices.GetUsersList();
            return Ok(new { code = 200, data = userList.ToList() });
        }


        [HttpPost]
        [Route("ActiveDeactiveUsers")]

        public async Task<IActionResult> ActiveDeactiveUsers(string UserName)
        {
             UserResponceModel responseModel = new UserResponceModel();

             var user = await UserListServices.ActiveDeactiveUsers(UserName);
            try
            {

                if (user != null)
                {

                    responseModel.Code = (int)HttpStatusCode.OK;
                    responseModel.Message = user.Message;
                    responseModel.Data = user.Data;
                }
                else
                {
                    responseModel.Message = user.Message;
                    responseModel.Code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                responseModel.Code = (int)HttpStatusCode.InternalServerError;
            }
            return StatusCode(responseModel.Code, responseModel);
        }

    }
}
