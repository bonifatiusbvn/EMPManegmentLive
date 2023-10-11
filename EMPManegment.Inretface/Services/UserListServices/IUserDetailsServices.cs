using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Models;

namespace EMPManegment.Inretface.Services.UserListServices
{
    public interface IUserDetailsServices
    {
       Task<IEnumerable<EmpDetailsView>> GetUsersList();
        Task<UserResponceModel> ActiveDeactiveUsers(string UserName);
        Task<UserResponceModel> EnterInOutTime(UserAttendanceModel userAttendance);
       Task<PasswordResetResponseModel> ResetPassword(PasswordResetView emp);
    }
}
