﻿using System;
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
       Task<jsonData> GetUsersList(DataTableRequstModel userList);
        Task<IEnumerable<EmpDetailsView>> GetUsersNameList();
        Task<UserResponceModel> ActiveDeactiveUsers(string UserName, Guid UpdatedBy);
        Task<UserResponceModel> EnterInTime(UserAttendanceModel EnterInTime);
        Task<UserResponceModel> EnterOutTime(UserAttendanceModel EnterOutTime);
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
    }
}
