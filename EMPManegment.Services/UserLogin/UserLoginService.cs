
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.ForgetPasswordModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.UserList;
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
        public async Task<string> AuthenticateUser(LoginView login)
        {
            return await UserLogin.AuthenticateUser(login);
        }

        public string GenerateToken(LoginView model)
        {
            return UserLogin.GenerateToken(model);
        }

        public async Task<UserResponceModel> UserSingUp(EmpDetailsView addemployee)
        {
            return await UserLogin.UserSingUp(addemployee);
        }

        public string CheckEmloyess()
        {
            return UserLogin.CheckEmloyess();
        }

        public async Task<bool> EmailSendAsync(string Email, string Subject, string Message)
        {
            return await UserLogin.EmailSendAsync(Email, Subject, Message);

        }
        public async Task<UserResponceModel> FindByEmailAsync(SendEmailModel Email)
        {
            return await UserLogin.FindByEmailAsync(Email);
        }

        public bool GetUserName(string Username)
        {
            throw new NotImplementedException();
        }
    }
}
