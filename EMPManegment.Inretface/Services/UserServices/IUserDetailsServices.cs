using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;

namespace EMPManegment.Inretface.Services.UserListServices
{
    public interface IUserDetailsServices
    {
       Task<jsonData> GetUsersList(DataTableRequstModel dataTable);
        Task<UserResponceModel> ActiveDeactiveUsers(string UserName);

        Task<UserResponceModel> EnterInTime(UserAttendanceModel userAttendance);
        Task<UserResponceModel> EnterOutTime(UserAttendanceModel userAttendance);

        Task<UserResponceModel> ResetPassword(PasswordResetView emp);


        Task<IEnumerable<EmpDocumentView>> GetDocumentType();
        Task<IEnumerable<DocumentInfoView>> GetDocumentList();
        Task<DocumentInfoView> UploadDocument(DocumentInfoView doc);
        Task<UserResponceModel> UserLockScreen(LoginRequest request);
        Task<UserResponceModel> UserBirsthDayWish(Guid UserId);

        Task<IEnumerable<EmpDetailsView>> UserEdit();
        Task<EmpDetailsView> GetById(Guid id);
        Task<UserResponceModel> UpdateUser(UserEditViewModel employee);
    }
}
