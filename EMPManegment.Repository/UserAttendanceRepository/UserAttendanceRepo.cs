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

        public async Task<UserAttendanceResponseModel> GetUserAttendanceInTime(UserAttendanceRequestModel userAttendance)
        {
            UserAttendanceResponseModel response = new UserAttendanceResponseModel();
            var data = Context.TblAttendances.Where(e => e.UserId == userAttendance.UserId && e.Date == DateTime.Today).FirstOrDefault();
            


            try
            {
                if (data != null)
                {
                    UserAttendanceModel attendanceModel = new UserAttendanceModel()
                    {
                        UserId = data.UserId,
                        Date = data.Date,
                        Intime = data.Intime,
                        TotalHours = data.TotalHours,
                        CreatedBy = data.CreatedBy,
                        CreatedOn = data.CreatedOn,
                        OutTime = data.OutTime,
                        AttendanceId= data.Id,
                    };
                        response.Code = 200;
                        response.Data = attendanceModel;
                }
                else
                {
                    response.Message = "Kindly Enter In-Time First";
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
