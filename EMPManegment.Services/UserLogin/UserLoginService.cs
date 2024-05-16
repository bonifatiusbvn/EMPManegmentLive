
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.Inretface.EmployeesInterface.AddEmployee;
using EMPManegment.Inretface.Interface.UsersLogin;
using EMPManegment.Inretface.Services.UserLoginServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.UserLogin
{
    public class UserLoginService : IUserLoginServices
    {
        public UserLoginService(IUserLogin userLogin)
        {
            UserLogin = userLogin;
        }

        public IUserLogin UserLogin { get; }

        public async Task<LoginResponseModel> LoginUser(LoginRequest LoginUser)
        {
            return await UserLogin.LoginUser(LoginUser);
        }
        public async Task<string> AuthenticateUser(LoginRequest login)
        {
            return await UserLogin.AuthenticateUser(login);
        }

        public string GenerateToken(LoginRequest model)
        {
            return UserLogin.GenerateToken(model);
        }
    }
}
