using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;

namespace EMPManegment.Inretface.Interface.UserList
{
    public interface IUserDetails
    {
        Task<jsonData> GetUsersList(DataTableRequstModel GetUsersList);
        Task<IEnumerable<EmpDetailsView>> GetUsersNameList();
        Task<UserResponceModel> ActiveDeactiveUsers(string UserName);
        Task<UserResponceModel> EnterInTime(UserAttendanceModel UserInTime);
        Task<UserResponceModel> EnterOutTime(UserAttendanceModel UserOutTime);
        Task<UserResponceModel> ResetPassword(PasswordResetView ResetPassword);
        Task<IEnumerable<EmpDocumentView>> GetDocumentType();
        Task<IEnumerable<DocumentInfoView>> GetDocumentList(Guid Userid);
        Task<DocumentInfoView> UploadDocument(DocumentInfoView UploadDocument);
        Task<UserResponceModel> UserLockScreen(LoginRequest UserLockScreen);
        Task<UserResponceModel> UserBirsthDayWish(Guid UserId);
        Task<IEnumerable<EmpDetailsView>> UserEdit();
        Task<EmpDetailsView> GetById(Guid UserId);
        Task<UserResponceModel> UpdateUserDetails(UserEditViewModel UpdateUser);
        Task<IEnumerable<EmpDetailsView>> GetUsersDetails();
        Task<IEnumerable<EmpDetailsView>> GetActiveDeactiveUserList();
        Task<IEnumerable<EmpDetailsView>> GetSearchEmpList(EmpDetailsModel GetSearchEmpList);
    }
}
