using EMPManagment.API;
using EMPManegment.EntityModels.Crypto;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.UserModels;
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

        public async Task<LoginResponseModel> LoginUser(LoginRequest loginRequest)
        {
            LoginResponseModel response = new LoginResponseModel();

            try
            {
                var tblUser = await (from a in Context.TblUsers
                                     where a.UserName == loginRequest.UserName
                                     join b in Context.TblRoleMasters on a.RoleId equals b.RoleId into roles
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
                                Role = tblUser.Role.Role,
                                RoleId = tblUser.Role.RoleId,
                            };

                            response.Data = userModel;
                            response.Code = (int)HttpStatusCode.OK;


                            List<FromPermission> FromPermissionData = (from u in Context.TblRolewiseFormPermissions
                                                                       join s in Context.TblForms on u.FormId equals s.FormId
                                                                       where u.RoleId == userModel.RoleId 
                                                                       orderby s.OrderId ascending
                                                                       select new FromPermission
                                                                       {
                                                                           FormName = s.FormName,
                                                                           GroupName = s.FormGroup,
                                                                           Controller = s.Controller,
                                                                           Action = s.Action,
                                                                           Add = u.IsAddAllow,
                                                                           View = u.IsViewAllow,
                                                                           Edit = u.IsEditAllow,
                                                                           Delete = u.IsDeleteAllow,
                                                                           isActive = s.IsActive,

                                                                       }).ToList();


                            userModel.FromPermissionData = FromPermissionData;

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
                        response.Message = "Your account is deactivated.contact your administrator.";
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
