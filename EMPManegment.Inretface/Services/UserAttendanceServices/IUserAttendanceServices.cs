using EMPManegment.EntityModels.ViewModels.AGGridModels;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Leave;
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
        Task<AGGridResponseModel<UserAttendanceModel>> GetMyAttendanceList(AGGridRequestModel AttendanceRequest);
        Task<AGGridResponseModel<UserAttendanceModel>> GetUserAttendanceList(AGGridRequestModel AttendanceRequest);
        Task<UserResponceModel> UpdateUserOutTime(UserAttendanceModel UpdateUserOutTime);
        Task<IEnumerable<UserAttendanceModel>> GetUserAttendanceById(int attendanceId);
        Task<jsonData> GetSearchAttendanceList(AttendanceRequestDataTableModel AttendanceRequestModel);
        Task<IEnumerable<UserAttendanceModel>> GetMySearchAttendanceList(SearchAttendanceModel GetSearchAttendanceList);

        Task<UserResponceModel> AddUserAttendance(UserAttendanceModel AddUser);
        Task<UserResponceModel> AddUserLeaveApplication(LeaveMasterModel LeaveDetails);
        Task<IEnumerable<LeaveReasonModel>> GetAllLeaveReasons();
        Task<AGGridResponseModel<LeaveMasterModel>> GetUserLeaveApplicationDetails(AGGridRequestModel UserLeaveRequest);
        Task<IEnumerable<LeaveMasterModel>> UserLeaveApproveRequest(Guid userId);
        Task<UserResponceModel> ApproveUserLeaveApplication(ApproveLeaveModel LeaveDetails);
    }
}
