using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.UserAttendance;
using Kendo.Mvc.Infrastructure;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Telerik.SvgIcons;

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
    }
}
