using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.UserAttendance;
using EMPManegment.Inretface.Services.UserAttendanceServices;


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

        public Task<IEnumerable<UserAttendanceModel>> GetAttendanceList(SearchAttendanceModel GetAttendanceList)
        {
            return UserAttendance.GetAttendanceList(GetAttendanceList);
        }
        public Task<IEnumerable<UserAttendanceModel>> GetSearchAttendanceList(searchAttendanceListModel AttendanceList)
        {
            return UserAttendance.GetSearchAttendanceList(AttendanceList);
        }
    }
}
