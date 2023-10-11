using Azure;
using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.UserList;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Repository.UserListRepository
{
    public class UserDetailsRepo : IUserDetails
    {
        public BonifatiusEmployeesContext Context { get; }
        public IWebHostEnvironment Environment { get; }
        public UserDetailsRepo(BonifatiusEmployeesContext context, IWebHostEnvironment environment)
        {
            Context = context;
            Environment = environment;
        }


        public async Task<IEnumerable<EmpDetailsView>> GetUsersList()
        {
            IEnumerable<EmpDetailsView> result = from e in Context.TblUsers
                                                 join d in Context.TblDepartments on e.DepartmentId equals d.Id
                                                 join c in Context.TblCountries on e.CountryId equals c.Id
                                                 join s in Context.TblStates on e.StateId equals s.Id
                                                 join ct in Context.TblCities on e.CityId equals ct.Id
                                                 select new EmpDetailsView
                                                 {
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
                                                     DepartmentName = d.Department
                                                 };
                                                     return result;
        }

        public async Task<UserResponceModel> ActiveDeactiveUsers(string UserName)
        {
            UserResponceModel response = new UserResponceModel();
            var data = Context.TblUsers.Where(a => a.UserName == UserName).FirstOrDefault();

            if (data != null)
            {

                if(data.IsActive == true)
                {
                    data.IsActive = false;
                    Context.TblUsers.Update(data);
                    Context.SaveChanges();
                    response.Code = 200;
                    response.Data = data;
                    response.Message = "User Deactive Succesfully";
                }

                else
                {
                    data.IsActive = true;
                    Context.TblUsers.Update(data);
                    Context.SaveChanges();
                    response.Code = 200;
                    response.Data = data;
                    response.Message = "User Active Succesfully";
                }


            }
            return response;   
        }

        public async Task<UserResponceModel> EnterInOutTime(UserAttendanceModel userAttendance)
        {
            UserResponceModel response = new UserResponceModel();
            var data = Context.TblAttendances.Where(a => a.UserId == userAttendance.UserId && a.OutTime != null).OrderByDescending(a=>a.Id).FirstOrDefault();
            if (data.OutTime != null)
            {
                if(data.UserId == userAttendance.UserId && data.Intime == null)
                {
                    data.Intime = userAttendance.InTime;
                    data.Date = DateTime.Today;
                    Context.TblAttendances.Add(data);
                    Context.SaveChanges();
                    response.Code = 200;
                    response.Message = "In-Time Enter Successfully";
                    response.Data = data;
                        
                }
                

                else if(data.OutTime == null)
                {
                    data.OutTime = userAttendance.OutTime;
                    Context.TblAttendances.Update(data);
                    Context.SaveChanges();
                    response.Code = 200;
                    response.Message = "Out-Time Enter Successfully";
                    response.Data = data;

                }
                else
                {
                    response.Message = "Your Already Enter IN-Time";
                };

            }

            else
            {
                response.Code = 400;
                response.Message = "You Miss Out-Time Contact Your Admin";
            }
            return response;
        }

        public async Task<PasswordResetResponseModel> ResetPassword(PasswordResetView emp)
        {
            PasswordResetResponseModel response = new PasswordResetResponseModel();
            try
            {
                var data = Context.TblUsers.FirstOrDefault(x => x.UserName == emp.UserName);
                    if(data !=null) 
                {
                   
                    data.PasswordHash = emp.PasswordHash;
                    data.PasswordSalt = emp.PasswordSalt;
                }
                Context.TblUsers.Update(data);
                Context.SaveChanges();
                response.Code=200;
                response.Data = emp;
                response.Message = "Password Updated!";
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            return response;
        }
    }
}
