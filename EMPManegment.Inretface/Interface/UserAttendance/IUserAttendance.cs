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

        Task<UserAttendanceResponseModel> GetUserAttendanceInTime(UserAttendanceRequestModel userAttendance);
        //Task<IEnumerable<UserAttendanceModel>> EditUserOutTime(UserAttendanceModel userAttendance);
   
        Task<IEnumerable<UserAttendanceModel>> GetAttendanceList(Guid Id, DateTime Cmonth );
        Task<jsonData> GetUserAttendanceList(DataTableRequstModel dataTable);
        Task<UserResponceModel> UpdateUserOutTime(UserAttendanceModel userAttendance);
        Task<IEnumerable<UserAttendanceModel>> GetUserAttendanceById(int attendanceId);
    }
}
