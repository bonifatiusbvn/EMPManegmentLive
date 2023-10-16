using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.UserAttendance;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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

  

        public async Task<IEnumerable<UserAttendanceModel>> GetUserAttendanceById()
        {
            IEnumerable<UserAttendanceModel> users = from a in Context.TblAttendances
                                                     join
                                                     b in Context.TblUsers on a.User.Id equals b.Id
                                                     select new UserAttendanceModel
                                                     {
                                                         UserName = b.FirstName + ' ' + b.LastName,
                                                         AttendanceId = a.Id,
                                                         Date = a.Date,
                                                         Intime = a.Intime,
                                                         OutTime = a.OutTime,
                                                         TotalHours = a.TotalHours,
                                                         CreatedBy = a.CreatedBy,
                                                         CreatedOn = a.CreatedOn,

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
    }
}
