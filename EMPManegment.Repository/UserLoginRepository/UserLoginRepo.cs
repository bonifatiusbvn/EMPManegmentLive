using EMPManagment.API;
using EMPManegment.EntityModels.Crypto;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.Inretface.Interface.UsersLogin;
using Microsoft.AspNetCore.Hosting;
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

        //public async Task<LoginResponseModel> LoginUser(LoginRequest Loginrequest)
        //{
        //    LoginResponseModel response = new LoginResponseModel();
        //    try
        //    {
        //          var tblUser = from a in Context.TblUsers.Where(p => p.UserName == Loginrequest.UserName).SingleOrDefault() 
        //                        join b in Context.TblRoleMasters on  ;
        //        if (tblUser != null)
        //        {
        //            if (tblUser.IsActive == true)
        //            {
        //                if (tblUser.UserName == Loginrequest.UserName && Crypto.VarifyHash(Loginrequest.Password, tblUser.PasswordHash, tblUser.PasswordSalt))   
        //                {

        //                    LoginView userModel = new LoginView();
        //                    userModel.UserName = tblUser.UserName;
        //                    userModel.Id = tblUser.Id;
        //                    userModel.FullName = tblUser.FirstName +" "+ tblUser.LastName;
        //                    userModel.FirstName = tblUser.FirstName;
        //                    userModel.ProfileImage = tblUser.Image;
        //                    userModel.Role =  
        //                    response.Data = userModel;
        //                    response.Code = (int)HttpStatusCode.OK;

        //                    tblUser.LastLoginDate = DateTime.Now;
        //                    Context.TblUsers.Update(tblUser);
        //                    Context.SaveChanges();
        //                }
        //                else
        //                {
        //                    response.Message = "Your Password Is Wrong";
        //                } 
        //            }
        //            else
        //            {
        //                response.Code = (int)HttpStatusCode.Forbidden;
        //                response.Message = "Your Deactive Contact Your Admin";
        //                return response;
        //            }
        //        }
        //        else
        //        {
        //            response.Message = "User Not Exist";
        //            response.Code = (int)HttpStatusCode.NotFound;
        //            response.Data = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return response;
        //}
        public async Task<LoginResponseModel> LoginUser(LoginRequest loginRequest)
        {
            LoginResponseModel response = new LoginResponseModel();

            try
            {
                var tblUser = await (from a in Context.TblUsers
                                     where a.UserName == loginRequest.UserName
                                     join b in Context.TblRoleMasters on a.Role equals b.Id into roles
                                     from role in roles.DefaultIfEmpty()
                                     select new { User = a, Role = role }).FirstOrDefaultAsync();

                if (tblUser != null)
                {
                    if (tblUser.User.IsActive == true)
                    {
                        if (Crypto.VarifyHash(loginRequest.Password, tblUser.User.PasswordHash, tblUser.User.PasswordSalt))
                        {
                            LoginView userModel = new LoginView
                            {
                                UserName = tblUser.User.UserName,
                                Id = tblUser.User.Id,
                                FullName = $"{tblUser.User.FirstName} {tblUser.User.LastName}",
                                FirstName = tblUser.User.FirstName,
                                ProfileImage = tblUser.User.Image,
                                Role = tblUser.Role.Role
                            };

                            response.Data = userModel;
                            response.Code = (int)HttpStatusCode.OK;

                            tblUser.User.LastLoginDate = DateTime.Now;
                            Context.TblUsers.Update(tblUser.User);
                            await Context.SaveChangesAsync();
                        }
                        else
                        {
                            response.Message = "Invalid credentials";
                            response.Code = (int)HttpStatusCode.Unauthorized;
                        }
                    }
                    else
                    {
                        response.Code = (int)HttpStatusCode.Forbidden;
                        response.Message = "Your account is deactivated. Contact your administrator.";
                    }
                }
                else
                {
                    response.Message = "User not found";
                    response.Code = (int)HttpStatusCode.NotFound;
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
