using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.ForgetPasswordModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.EmployeesInterface.AddEmployee;
using EMPManegment.Inretface.Interface.UserList;
using EMPManegment.Inretface.Services.AddEmployeeServies;
using EMPManegment.Repository.AddEmpRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.AddEmployee
{
    public class AuthenticationService : IAuthenticationServices
    {
        public IAuthentication Authentication { get; }
        public IUserDetails UserDetails { get; }

        public AuthenticationService(IAuthentication authentication, IUserDetails userDetails)
        {
            Authentication = authentication;
            UserDetails = userDetails;
        }

        public async Task<UserResponceModel> UserSingUp(EmpDetailsView addemployee)
        {
            return await Authentication.UserSingUp(addemployee);
        }

        public string CheckEmloyess()
        {
            return Authentication.CheckEmloyess();
        }


        public async Task<EmpDetailsView> GetById(Guid userId)
        {
            return await UserDetails.GetById(userId);
        }
        public async Task<LoginResponseModel> LoginUser(LoginRequest LoginUser)
        {
            return await Authentication.LoginUser(LoginUser);
        }
        public async Task<bool> EmailSendAsync(string Email, string Subject, string Message)
        {
            return await Authentication.EmailSendAsync(Email, Subject, Message);
        }
        public async Task<UserResponceModel> FindByEmailAsync(SendEmailModel Email)
        {
            return await Authentication.FindByEmailAsync(Email);
        }


    }
}
