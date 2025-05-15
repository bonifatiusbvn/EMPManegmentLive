using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.AGGridModels;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.FormPermissionMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Interface.UserList
{
    public interface IUserDetails
    {
        Task<AGGridResponseModel<EmpDetailsView>> GetUsersList(AGGridRequestModel UserRequest);
        Task<IEnumerable<EmpDetailsView>> GetUsersNameList();
        Task<UserResponceModel> ActiveDeactiveUsers(Guid UserId, Guid UpdatedBy);
        Task<UserResponceModel> EnterInTime(UserAttendanceModel UserInTime);
        Task<UserResponceModel> EnterOutTime(UserAttendanceModel UserOutTime);
        Task<UserResponceModel> ResetPassword(PasswordResetView ResetPassword);
        Task<IEnumerable<EmpDocumentView>> GetDocumentType();
        Task<IEnumerable<DocumentInfoView>> GetDocumentList(Guid Userid);
        Task<DocumentInfoView> UploadDocument(DocumentInfoView UploadDocument);
        Task<UserResponceModel> UserLockScreen(LoginRequest UserLockScreen);
        Task<UserResponceModel> UserBirsthDayWish(Guid UserId);
        Task<IEnumerable<EmpDetailsView>> UserEdit();
        Task<EmpDetailsView> GetEmployeeById(Guid UserId);
        Task<UserResponceModel> UpdateUserDetails(UserEditViewModel UpdateUser);
        Task<IEnumerable<EmpDetailsView>> GetUsersDetails();
        Task<IEnumerable<EmpDetailsView>> GetActiveDeactiveUserList();
        Task<IEnumerable<EmpDetailsView>> GetSearchEmpList(EmpDetailsModel GetSearchEmpList);
        Task<UserResponceModel> UpdateUserExeperience(EmpDetailsView UpdateDate);
        Task<UserResponceModel> UserProfilePhoto(EmpDetailsView Profile);
        Task<IEnumerable<RolewiseFormPermissionModel>> GetRolewiseFormPermissionList();
        Task<List<RolewiseFormPermissionModel>> GetUserRolewiseFormListById(Guid RoleId);
        Task<ApiResponseModel> UpdateUserMultipleRolewiseFormPermission(List<RolewiseFormPermissionModel> UpdatedRolewiseFormPermissions);
        Task<UserResponceModel> ActiveDeactiveRole(Guid roleId);
    }
}
