
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.ForgetPasswordModels;
using EMPManegment.EntityModels.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Interface.UsersLogin
{
    public interface IUserLogin
    {

        public bool GetUserName(string Username);

        Task<string> AuthenticateUser(LoginRequest login);

        string CheckEmloyess();

        Task<UserResponceModel> UserSingUp(EmpDetailsView AddEmployee);

        Task<LoginResponseModel> LoginUser(LoginRequest LoginUserRequest);

        Task<bool> EmailSendAsync(string Email, string Subject, string Message);

        Task<UserResponceModel> FindByEmailAsync(SendEmailModel Email);

        string GenerateToken(LoginRequest model);
    }
}
