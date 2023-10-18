using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Models;

namespace EMPManegment.Inretface.Interface.UserList
{
    public interface IUserDetails
    {
        Task<IEnumerable<EmpDetailsView>> GetUsersList();
         
        Task<UserResponceModel> ActiveDeactiveUsers(string UserName);

        Task<UserResponceModel> EnterInTime(UserAttendanceModel userAttendance);
        Task<UserResponceModel> EnterOutTime(UserAttendanceModel userAttendance);

        Task<UserResponceModel> ResetPassword(PasswordResetView emp);

        
        Task<IEnumerable<EmpDocumentView>> GetDocumentType();
        Task<IEnumerable<DocumentInfoView>> GetDocumentList();
        Task<DocumentInfoView> UploadDocument(DocumentInfoView doc);
        Task<UserResponceModel> UserLockScreen(LoginRequest request);
        Task<UserResponceModel> UserBirsthDayWish(Guid UserId);

    }
}
