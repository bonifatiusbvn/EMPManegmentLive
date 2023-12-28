using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.UserList;
using EMPManegment.Inretface.Services.UserListServices;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.UserList
{
    public class UserDetailsService : IUserDetailsServices
    {
        public IUserDetails UserList { get; }
        public UserDetailsService(IUserDetails userList)
        {
            UserList = userList;
        }

        public async Task<jsonData> GetUsersList(DataTableRequstModel GetUserlist)
        {
            return await UserList.GetUsersList(GetUserlist);
        }

        public Task<UserResponceModel> ActiveDeactiveUsers(string activedeavtiveuser)
        {
            return UserList.ActiveDeactiveUsers(activedeavtiveuser);    
        }


        public Task<UserResponceModel> EnterInTime(UserAttendanceModel enterinTime)
        {
            return UserList.EnterInTime(enterinTime);
        }

        public Task<UserResponceModel> EnterOutTime(UserAttendanceModel enteroutTime)
        {
            return UserList.EnterOutTime(enteroutTime);
        }

        public async Task<UserResponceModel> ResetPassword(PasswordResetView PasswordReset)
        {
            return await UserList.ResetPassword(PasswordReset);
        }
        public async Task<IEnumerable<EmpDocumentView>> GetDocumentType()
        {
            return await UserList.GetDocumentType();
        }

        public async Task<IEnumerable<DocumentInfoView>> GetDocumentList(Guid Userid)
        {
            return await UserList.GetDocumentList(Userid);
        }

        public async Task<DocumentInfoView> UploadDocument(DocumentInfoView UploadDocument)
        {
            return await UserList.UploadDocument(UploadDocument);
        }

        public async Task<UserResponceModel> UserLockScreen(LoginRequest UserLockScreen)
        {
            return await UserList.UserLockScreen(UserLockScreen);
        }

        public async Task<UserResponceModel> UserBirsthDayWish(Guid UserId)
        {
            return await UserList.UserBirsthDayWish(UserId);
        }
        public async Task<IEnumerable<EmpDetailsView>> UserEdit()
        {
            return await UserList.UserEdit();
        }

        public async Task<EmpDetailsView> GetById(Guid userId)
        {
            return await UserList.GetById(userId);
        }

        public async Task<UserResponceModel> UpdateUser(UserEditViewModel UpdateUser)
        {
            return await UserList.UpdateUser(UpdateUser);
        }

        public async Task<IEnumerable<EmpDetailsView>> GetUsersNameList()
        {
            return await UserList.GetUsersNameList();
        }

        public async Task<IEnumerable<EmpDetailsView>> GetUsersDetails()
        {
            return await UserList.GetUsersDetails();
        }
        public async Task<IEnumerable<EmpDetailsView>> GetSearchEmpList(EmpDetailsModel EmpList)
        {
            return await UserList.GetSearchEmpList(EmpList);
        }
    }
}

