
using EMPManagment.API;
using EMPManegment.EntityModels.Crypto;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.ForgetPasswordModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.EmployeesInterface.AddEmployee;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace EMPManegment.Repository.AddEmpRepository
{
    public class AddEmpRepo : IAuthentication
    {

        public AddEmpRepo(BonifatiusEmployeesContext context,IConfiguration configuration)
        {
            Context = context;
            Configuration = configuration;
        }

        public BonifatiusEmployeesContext Context { get; }
        public IConfiguration Configuration { get; }

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
                        Role = 4,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,

                    };
                    response.Code = (int)HttpStatusCode.OK;
                    Context.TblUsers.Add(model);
                    Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }          
            return response;
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
                            userModel.FullName = tblUser.FirstName + " " + tblUser.LastName;
                            userModel.FirstName = tblUser.FirstName;
                            userModel.ProfileImage = tblUser.Image;
                            //userModel.Role = tblUser.Role;
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

        public bool GetUserName(string Username)
        {
            throw new NotImplementedException();
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
                    Body = message
                };
                mailMessage.To.Add(email);
                SmtpClient smtpClient = new SmtpClient(emailSettingView.SmtpServer)
                {
                    Port = emailSettingView.Port,
                    Credentials = new NetworkCredential(emailSettingView.From,emailSettingView.SecretKey),
                    EnableSsl = emailSettingView.EnableSSL
                };
                await smtpClient.SendMailAsync(mailMessage);
                status = true;
            }
            catch(Exception)
            {
                status = false;
            }
            return status;
        }

        public async Task<UserResponceModel> ForgetPassword(SendEmailModel forgetpass)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var userdata = Context.TblUsers.FirstOrDefault(x => x.Email == forgetpass.Email);
                if (userdata != null)
                {
                    response.Code = 200;
                    response.Message = "Reset Link send on your Registered Email";
                }
                else
                {
                    response.Code = 400;
                    response.Message = "Invalid Email Id!";
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
