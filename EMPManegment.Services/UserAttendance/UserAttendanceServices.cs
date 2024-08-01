using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.UserAttendance;
using EMPManegment.Inretface.Services.UserAttendanceServices;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
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

        public Task<jsonData> GetUserAttendanceList(DataTableRequstModel AttendancedataTable)
        {
            return UserAttendance.GetUserAttendanceList(AttendancedataTable);
        }
        public Task<UserAttendanceResponseModel> GetUserAttendanceInTime(UserAttendanceRequestModel GetuserAttendance)
        {
            return UserAttendance.GetUserAttendanceInTime(GetuserAttendance);
        }

        public Task<UserResponceModel> UpdateUserOutTime(UserAttendanceModel Updateouttime)
        {
            return UserAttendance.UpdateUserOutTime(Updateouttime);
        }

        public Task<IEnumerable<UserAttendanceModel>> GetUserAttendanceById(int GetattendanceId)
        {
            return UserAttendance.GetUserAttendanceById(GetattendanceId);
        }

        public Task<jsonData> GetAttendanceList(MyAttendanceRequestDataTableModel AttendanceRequestModel)
        {
            return UserAttendance.GetAttendanceList(AttendanceRequestModel);
        }
        public Task<jsonData> GetSearchAttendanceList(AttendanceRequestDataTableModel AttendanceRequestModel)
        {
            return UserAttendance.GetSearchAttendanceList(AttendanceRequestModel);
        }
        public Task<IEnumerable<UserAttendanceModel>> GetMySearchAttendanceList(SearchAttendanceModel GetSearchAttendanceList)
        {
            return UserAttendance.GetMySearchAttendanceList(GetSearchAttendanceList);
        }
    }
}
