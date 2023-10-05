using EMPManegment.EntityModels.View_Model;
using EMPManegment.Inretface.Interface.UserList;
using EMPManegment.Inretface.Services.UserListServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : ControllerBase
    {
        public IUserListServices UserListServices { get; }
        public UserDetailsController(IUserListServices userListServices)
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

    }
}
