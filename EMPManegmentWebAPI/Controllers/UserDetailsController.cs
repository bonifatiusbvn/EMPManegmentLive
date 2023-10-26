using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.Inretface.Interface.UserList;
using EMPManegment.Inretface.Services.UserListServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using NuGet.Protocol.Core.Types;

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
        [HttpGet]
        [Route("UserEdit")]
        public async Task<IActionResult> UserEdit()
        {
            IEnumerable<EmpDetailsView> userList = await UserListServices.GetUsersList();
            return Ok(new { code = 200, data = userList.ToList() });
        }
        [HttpGet]
        [Route("GetEmployee")]
        public async Task<IActionResult>GetEmployee(Guid id)
        {
            var data  = await UserListServices.GetById(id);
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(data);
            }
        }
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult>UpdateUser(UserEditViewModel employee)
        {
            var data = await UserListServices.UpdateUser(employee);
            return Ok(data);
        }
    }
}
