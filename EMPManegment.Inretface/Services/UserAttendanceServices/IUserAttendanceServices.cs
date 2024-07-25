using EMPManegment.EntityModels.ViewModels.DataTableParameters;
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
        Task<UserAttendanceResponseModel> GetUserAttendanceInTime(UserAttendanceRequestModel GetAttendanceInTime);
        Task<jsonData> GetAttendanceList(MyAttendanceRequestDataTableModel AttendanceRequestModel);
        Task<jsonData> GetUserAttendanceList(DataTableRequstModel GetAttendanceList);
        Task<UserResponceModel> UpdateUserOutTime(UserAttendanceModel UpdateUserOutTime);
        Task<IEnumerable<UserAttendanceModel>> GetUserAttendanceById(int attendanceId);
        Task<jsonData> GetSearchAttendanceList(AttendanceRequestDataTableModel AttendanceRequestModel);
    }
}
