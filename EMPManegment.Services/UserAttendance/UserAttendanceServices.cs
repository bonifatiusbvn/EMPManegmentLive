using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.UserAttendance;
using EMPManegment.Inretface.Services.UserAttendanceServices;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.UserAttendance
{
    public class UserAttendanceServices : IUserAttendanceServices
    {

        public UserAttendanceServices(IUserAttendance userAttendance) 
        {
            UserAttendance = userAttendance;
        }

        public IUserAttendance UserAttendance { get; }

        public Task<IEnumerable<UserAttendanceModel>> GetUserAttendanceById()
        {
            return UserAttendance.GetUserAttendanceById();
        }

      
    }
}
