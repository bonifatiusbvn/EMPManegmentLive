using EMPManegment.EntityModels.ViewModels.Models;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Services.UserAttendanceServices
{
    public interface IUserAttendanceServices
    {
        Task<IEnumerable<UserAttendanceModel>> GetUserAttendanceById();
        //Task<IEnumerable<UserAttendanceModel>> EditUserOutTime(UserAttendanceModel userAttendance);
    }
}
