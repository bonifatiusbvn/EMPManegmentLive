using EMPManegment.EntityModels.ViewModels.AGGridModels;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Leave;
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

        public Task<AGGridResponseModel<UserAttendanceModel>> GetUserAttendanceList(AGGridRequestModel AttendanceRequest)
        {
            return UserAttendance.GetUserAttendanceList(AttendanceRequest);
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

        public Task<AGGridResponseModel<UserAttendanceModel>> GetMyAttendanceList(AGGridRequestModel AttendanceRequest)
        {
            return UserAttendance.GetMyAttendanceList(AttendanceRequest);
        }
        public Task<jsonData> GetSearchAttendanceList(AttendanceRequestDataTableModel AttendanceRequestModel)
        {
            return UserAttendance.GetSearchAttendanceList(AttendanceRequestModel);
        }
        public Task<IEnumerable<UserAttendanceModel>> GetMySearchAttendanceList(SearchAttendanceModel GetSearchAttendanceList)
        {
            return UserAttendance.GetMySearchAttendanceList(GetSearchAttendanceList);
        }

        public Task<UserResponceModel> AddUserAttendance(UserAttendanceModel AddUser)
        {
           return UserAttendance.AddUserAttendance(AddUser);
        }

        public Task<UserResponceModel> AddUserLeaveApplication(LeaveMasterModel LeaveDetails)
        {
            return UserAttendance.AddUserLeaveApplication(LeaveDetails);
        }

        public Task<IEnumerable<LeaveReasonModel>> GetAllLeaveReasons()
        {
           return UserAttendance.GetAllLeaveReasons();
        }

        public Task<AGGridResponseModel<LeaveMasterModel>> GetUserLeaveApplicationDetails(AGGridRequestModel UserLeaveRequest)
        {
            return UserAttendance.GetUserLeaveApplicationDetails(UserLeaveRequest);
        }   

        public Task<UserResponceModel> ApproveUserLeaveApplication(ApproveLeaveModel LeaveDetails)
        {
            return UserAttendance.ApproveUserLeaveApplication(LeaveDetails);
        }

        public Task<IEnumerable<LeaveMasterModel>> UserLeaveApproveRequest(Guid userId)
        {
            return UserAttendance.UserLeaveApproveRequest(userId);
        }
    }
}
