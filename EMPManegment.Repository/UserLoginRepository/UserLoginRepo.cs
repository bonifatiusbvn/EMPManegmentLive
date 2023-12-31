﻿using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.Inretface.Interface.UsersLogin;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using EMPManegment.EntityModels.Crypto;
using Microsoft.AspNetCore.Hosting;

namespace EMPManegment.Repository.UserLoginRepository
{
    public class UserLoginRepo : IUserLogin
    {
        public UserLoginRepo(BonifatiusEmployeesContext context, IWebHostEnvironment environment)
        {
            Context = context;
            Environment = environment;
        }

        public BonifatiusEmployeesContext Context { get; }
        public IWebHostEnvironment Environment { get; }

        public bool GetUserName(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<LoginResponseModel> LoginUser(LoginRequest Loginrequest)
        {
            LoginResponseModel response = new LoginResponseModel();
            try
            {
                  var tblUser = Context.TblUsers.Where(p => p.UserName == Loginrequest.UserName).SingleOrDefault();
                if (tblUser != null)
                {
                    if (tblUser.IsActive == true)
                    {
                        if (tblUser.UserName == Loginrequest.UserName && Crypto.VarifyHash(Loginrequest.Password, tblUser.PasswordHash, tblUser.PasswordSalt))   
                        {

                            LoginView userModel = new LoginView();
                            userModel.UserName = tblUser.UserName;
                            userModel.Id = tblUser.Id;
                            userModel.FullName = tblUser.FirstName +" "+ tblUser.LastName;
                            userModel.FirstName = tblUser.FirstName;
                            userModel.ProfileImage = tblUser.Image;
                            userModel.IsAdmin = tblUser.IsAdmin == null ? false : (bool)tblUser.IsAdmin;
                            response.Data = userModel;
                            response.Code = (int)HttpStatusCode.OK;

                            tblUser.LastLoginDate = DateTime.Now;
                            Context.TblUsers.Update(tblUser);
                            Context.SaveChanges();
                        }
                        else
                        {
                            response.Message = "Your Password Is Wrong";
                        } 
                    }
                    else
                    {
                        response.Code = (int)HttpStatusCode.Forbidden;
                        response.Message = "Your Deactive Contact Your Admin";
                        return response;
                    }
                }
                else
                {
                    response.Message = "User Not Exist";
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }
}
