using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.ForgetPasswordModels;
using EMPManegment.EntityModels.ViewModels.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.EmployeesInterface.AddEmployee
{
    public interface IAuthentication
    {
        string CheckEmloyess();
        Task<UserResponceModel> UserSingUp(EmpDetailsView AddEmployee);
        Task<LoginResponseModel> LoginUser(LoginRequest LoginUserRequest);
        public bool GetUserName(string Username);
        Task<bool> EmailSendAsync(string Email, string Subject, string Message);
        Task<UserResponceModel> FindByEmailAsync(SendEmailModel Email);

    }
}
