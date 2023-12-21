using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Interface.UserAttendance
{
    public interface IUserAttendance
    {

        Task<UserAttendanceResponseModel> GetUserAttendanceInTime(UserAttendanceRequestModel getAttendanceInTime);
        //Task<IEnumerable<UserAttendanceModel>> EditUserOutTime(UserAttendanceModel userAttendance);
   
        Task<IEnumerable<UserAttendanceModel>> GetAttendanceList(SearchAttendanceModel GetAttendanceList);
        Task<jsonData> GetUserAttendanceList(DataTableRequstModel GetAttendanceList);
        Task<UserResponceModel> UpdateUserOutTime(UserAttendanceModel UpdateUserOutTime);
        Task<IEnumerable<UserAttendanceModel>> GetUserAttendanceById(int attendanceId);
    }
}
