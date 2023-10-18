﻿using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.UserAttendance;
using EMPManegment.Inretface.Services.UserAttendanceServices;

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

        public Task<IEnumerable<UserAttendanceModel>> GetUserAttendanceList()
        {
            return UserAttendance.GetUserAttendanceList();
        }

        public Task<UserAttendanceResponseModel> GetUserAttendanceInTime(UserAttendanceRequestModel userAttendance)
        {
            return UserAttendance.GetUserAttendanceInTime(userAttendance);
        }

        public Task<UserAttendanceModel> UpdateUserOutTime(UserAttendanceModel userAttendance)
        {
            return UserAttendance.UpdateUserOutTime(userAttendance);
        }

        public Task<IEnumerable<UserAttendanceModel>> GetUserAttendanceById(int attendanceId)
        {
            return UserAttendance.GetUserAttendanceById(attendanceId);
        }
    }
}
