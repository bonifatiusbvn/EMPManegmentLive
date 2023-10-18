using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.UserAttendance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace EMPManegment.Repository.UserAttendanceRepository
{
    public class UserAttendanceRepo : IUserAttendance
    {
        public UserAttendanceRepo(BonifatiusEmployeesContext context)
        {
            Context = context;
        }

        public BonifatiusEmployeesContext Context { get; }

  

        public async Task<IEnumerable<UserAttendanceModel>> GetUserAttendanceList()
        {
            IEnumerable<UserAttendanceModel> users = from a in Context.TblAttendances
                                                     join
                                                     b in Context.TblUsers on a.User.Id equals b.Id
                                                     select new UserAttendanceModel 
                                                     {
                                                         UserName = b.FirstName + ' ' + b.LastName,
                                                         UserId =a.UserId,
                                                         AttendanceId = a.Id,
                                                         Date = a.Date,
                                                         Intime = a.Intime,
                                                         OutTime = a.OutTime,
                                                         TotalHours = a.OutTime - a.Intime,
                                                         CreatedOn = a.CreatedOn,
                                                         CreatedBy= a.CreatedBy,
                                                     };

            return users;
        }

        public async Task<UserResponceModel> GetUserAttendanceInTime(UserAttendanceRequestModel userAttendance)
        {
            UserResponceModel response = new UserResponceModel();
            var data = Context.TblAttendances.Where(e => e.UserId == userAttendance.UserId && e.Date == DateTime.Today).FirstOrDefault();
            


            try
            {
                if (data != null)
                {

                    if (data.Date == DateTime.Today && data.Intime != null)
                    {
                        response.Code = 200;
                        response.Data = data.Intime;
                        
                    }

                    else
                    {
                       
                        response.Code = 200;
                        response.Data = data;
                        response.Message = "Kindly Enter In-Time First";
                    }


                }
                return response;

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public async Task<UserAttendanceModel> UpdateUserOutTime(UserAttendanceModel userAttendance)
        {
            try
            {
                var data = Context.TblAttendances.FirstOrDefault(a => a.UserId == userAttendance.UserId);
                if (data != null)
                {
                    data.OutTime = userAttendance.OutTime;

                    Context.TblAttendances.Update(data);
                    Context.SaveChanges();
                }
                return userAttendance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<UserAttendanceModel>> GetUserAttendanceById(int attendanceId)
        {
            IEnumerable<UserAttendanceModel> attendance = from a in Context.TblAttendances
                             join
                             b in Context.TblUsers on a.User.Id equals b.Id where a.Id == attendanceId
                             select new UserAttendanceModel
                             {
                                 UserName = b.FirstName + ' ' + b.LastName,
                                 UserId = a.UserId,
                                 AttendanceId = a.Id,
                                 Date = a.Date,
                                 Intime = a.Intime,
                                 OutTime = a.OutTime,
                                 TotalHours = a.OutTime - a.Intime,
                                 CreatedOn = a.CreatedOn,
                                 CreatedBy = a.CreatedBy,
                             };
            
            return attendance;
        }
    }
}
