
using EMPManegment.EntityModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Interface.UsersLogin
{
    public interface IUserLogin
    {
        Task<LoginResponseModel> LoginUser(LoginRequest LoginUserRequest);

        public bool GetUserName(string Username);
    }
}
