using EMPManagment.API;
using EMPManegment.EntityModels.Crypto;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.ForgetPasswordModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.UserModels;
using EMPManegment.Inretface.Interface.UsersLogin;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EMPManegment.Repository.UserLoginRepository
{
    public class UserLoginRepo : IUserLogin
    {
        public UserLoginRepo(BonifatiusEmployeesContext context, IWebHostEnvironment environment, IConfiguration configuration)
        {
            Context = context;
            Environment = environment;
            Configuration = configuration;
        }

        public BonifatiusEmployeesContext Context { get; }
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public bool GetUserName(string username)
        {
            throw new NotImplementedException();
        }
        public async Task<string> AuthenticateUser(LoginRequest login)
        {
            var UserLogin = await Context.TblUsers.FirstOrDefaultAsync(l => l.UserName == login.UserName && Crypto.VarifyHash(login.Password, l.PasswordHash, l.PasswordSalt));
            if (UserLogin == null)
            {
                return null;
            }
            else
            {

                LoginRequest user = new LoginRequest()
                {
                    UserName = UserLogin.UserName,

                };

                var Jtoken = GenerateToken(user);
                return Jtoken;
            }
        }

        public async Task<UserResponceModel> UserSingUp(EmpDetailsView addemp)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                bool isEmailAlredyExists = Context.TblUsers.Any(x => x.Email == addemp.Email);
                if (isEmailAlredyExists == true)
                {
                    response.Message = "User with this email already exists";
                    response.Code = (int)HttpStatusCode.NotFound;
                }
                else
                {
                    var model = new TblUser()
                    {
                        Id = Guid.NewGuid(),
                        UserName = addemp.UserName,
                        DepartmentId = addemp.DepartmentId,
                        FirstName = addemp.FirstName,
                        LastName = addemp.LastName,
                        Address = addemp.Address,
                        CityId = addemp.CityId,
                        StateId = addemp.StateId,
                        CountryId = addemp.CountryId,
                        DateOfBirth = addemp.DateOfBirth,
                        Email = addemp.Email,
                        Gender = addemp.Gender,
                        PhoneNumber = addemp.PhoneNumber,
                        CreatedOn = DateTime.Now,
                        PasswordHash = addemp.PasswordHash,
                        PasswordSalt = addemp.PasswordSalt,
                        Image = addemp.Image,
                        IsActive = true,
                        JoiningDate = DateTime.Now,
                        //Role = 4,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        CreatedBy = addemp.CreatedBy

                    };
                    response.Message = "User Added Sucessfully!";
                    response.Code = (int)HttpStatusCode.OK;
                    Context.TblUsers.Add(model);
                    Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error in creating user.";
                response.Code = 400;
            }
            return response;
        }

        public string GenerateToken(LoginRequest model)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, model.UserName));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim("UserName", model.UserName));
            claims.Add(new Claim("Password", model.Password));

            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(Configuration["Jwt:Issuer"], Configuration["Jwt:Audience"], claims: claims.ToArray(),
                expires: DateTime.Now.AddMinutes(30), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<LoginResponseModel> LoginUser(LoginRequest Loginrequest)
        {
            LoginResponseModel response = new LoginResponseModel();
            try
            {
                var tblUser = await (from a in Context.TblUsers
                                     where a.UserName == Loginrequest.UserName
                                     join b in Context.TblRoleMasters on a.RoleId equals b.RoleId into roles
                                     from role in roles.DefaultIfEmpty()
                                     select new { User = a, Role = role }).FirstOrDefaultAsync();

                if (tblUser != null)
                {
                    if (tblUser.User.IsActive == true)
                    {
                        LoginRequest user = new LoginRequest()
                        {
                            UserName = tblUser.User.UserName,
                            Password = Loginrequest.Password,
                        };
                        var authToken = GenerateToken(user);


                        if (Crypto.VarifyHash(Loginrequest.Password, tblUser.User.PasswordHash, tblUser.User.PasswordSalt))
                        {
                            LoginView userModel = new LoginView
                            {
                                UserName = tblUser.User.UserName,
                                Id = tblUser.User.Id,
                                FullName = $"{tblUser.User.FirstName} {tblUser.User.LastName}",
                                FirstName = tblUser.User.FirstName,
                                LastName = tblUser.User.LastName,
                                ProfileImage = tblUser.User.Image,
                                Role = tblUser.Role.Role,
                                RoleId = tblUser.Role.RoleId,
                                Token = authToken,
                            };

                            response.Data = userModel;
                            response.Code = (int)HttpStatusCode.OK;


                            bool userformPermission = await Context.TblUserFormPermissions.AnyAsync(e => e.UserId == userModel.Id);

                            if (userformPermission == true)
                            {
                                List<FromPermission> FromPermissionData = (from u in Context.TblUserFormPermissions
                                                                           join s in Context.TblForms on u.FormId equals s.FormId
                                                                           where u.UserId == userModel.Id
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
                                                                               isViewAllow = u.IsViewAllow,
                                                                           }).ToList();


                                userModel.FromPermissionData = FromPermissionData;

                            }


                            else
                            {
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
                                                                               isViewAllow = u.IsViewAllow,
                                                                           }).ToList();


                                userModel.FromPermissionData = FromPermissionData;
                            }

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
                response.Message = "Error in Login the User.";
                response.Code = 400;
            }

            return response;
        }


        public string CheckEmloyess()
        {
            try
            {
                var LastEmp = Context.TblUsers.OrderByDescending(e => e.CreatedOn).FirstOrDefault();
                string UserEmpId;
                if (LastEmp == null)
                {
                    UserEmpId = "BONI-UID001";
                }
                else
                {
                    UserEmpId = "BONI-UID" + (Convert.ToUInt32(LastEmp.UserName.Substring(9, LastEmp.UserName.Length - 9)) + 1).ToString("D3");
                }
                return UserEmpId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> EmailSendAsync(string email, string Subject, string message)
        {
            bool status = false;
            try
            {
                EmailSettingView emailSettingView = new EmailSettingView()
                {
                    SecretKey = Configuration.GetValue<string>("AppSettings:SecretKey"),
                    From = Configuration.GetValue<string>("AppSettings:EmailSettings:From"),
                    SmtpServer = Configuration.GetValue<string>("AppSettings:EmailSettings:SmtpServer"),
                    Port = Configuration.GetValue<int>("AppSettings:EmailSettings:Port"),
                    EnableSSL = Configuration.GetValue<bool>("AppSettings:EmailSettings:EnableSSL"),
                };
                MailMessage mailMessage = new MailMessage()
                {
                    From = new MailAddress(emailSettingView.From),
                    Subject = Subject,
                    Body = message,
                    BodyEncoding = System.Text.Encoding.ASCII,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);
                SmtpClient smtpClient = new SmtpClient(emailSettingView.SmtpServer)
                {
                    Port = emailSettingView.Port,
                    Credentials = new NetworkCredential(emailSettingView.From, emailSettingView.SecretKey),
                    EnableSsl = emailSettingView.EnableSSL
                };
                await smtpClient.SendMailAsync(mailMessage);
                status = true;
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }

        public async Task<UserResponceModel> FindByEmailAsync(SendEmailModel Email)
        {
            EmpDetailsView Userdata = new EmpDetailsView();
            UserResponceModel responceModel = new UserResponceModel();
            try
            {
                var userdata = Context.TblUsers.FirstOrDefault(x => x.Email == Email.Email);
                if (userdata != null)
                {
                    Userdata = (from e in Context.TblUsers.Where(x => x.Email == Email.Email)
                                join d in Context.TblDepartments on e.DepartmentId equals d.Id
                                join c in Context.TblCountries on e.CountryId equals c.Id
                                join s in Context.TblStates on e.StateId equals s.Id
                                join ct in Context.TblCities on e.CityId equals ct.Id
                                select new EmpDetailsView
                                {
                                    Id = e.Id,
                                    IsActive = e.IsActive,
                                    UserName = e.UserName,
                                    FirstName = e.FirstName,
                                    LastName = e.LastName,
                                    Image = e.Image,
                                    Gender = e.Gender,
                                    DateOfBirth = e.DateOfBirth,
                                    Email = e.Email,
                                    PhoneNumber = e.PhoneNumber,
                                    Address = e.Address,
                                    CityName = ct.City,
                                    StateName = s.State,
                                    CountryName = c.Country,
                                    DepartmentName = d.Department,
                                    JoiningDate = e.JoiningDate,
                                    Pincode = e.Pincode,
                                    Designation = e.Designation,
                                    DepartmentId = e.DepartmentId,
                                    CityId = e.CityId,
                                    StateId = e.StateId,
                                    CountryId = e.CountryId,
                                }).First();

                    responceModel.Data = Userdata;
                    responceModel.Code = 200;
                    responceModel.Message = "Reset link send on your registered email";

                }
                else
                {
                    responceModel.Code = 400;
                    responceModel.Message = "Invalid email id!";
                }
            }
            catch (Exception ex)
            {
                responceModel.Code = 404;
                responceModel.Message = "Error in finding User.";
            }
            return responceModel;
        }
    }
}
