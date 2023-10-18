using EMPManegment.EntityModels.ViewModels.Models;

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
        Task<UserAttendanceResponseModel> GetUserAttendanceInTime(UserAttendanceRequestModel userAttendance);
        //Task<IEnumerable<UserAttendanceModel>> EditUserOutTime(UserAttendanceModel userAttendance);
        Task<IEnumerable<UserAttendanceModel>> GetUserAttendanceList();
        Task<UserResponceModel> GetUserAttendanceInTime(UserAttendanceRequestModel userAttendance);
        Task<UserAttendanceModel> UpdateUserOutTime(UserAttendanceModel userAttendance);
        Task<IEnumerable<UserAttendanceModel>> GetUserAttendanceById(int attendanceId);
    }
}
