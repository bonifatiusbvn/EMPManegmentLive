﻿using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.UserList;
using EMPManegment.Inretface.Services.UserListServices;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IEnumerable<EmpDetailsView>> GetUsersList()
        {
            return await UserList.GetUsersList();
        }

        public Task<UserResponceModel> ActiveDeactiveUsers(string UserName)
        {
            return UserList.ActiveDeactiveUsers(UserName);    
        }


        public Task<UserResponceModel> EnterInTime(UserAttendanceModel userAttendance)
        {
            return UserList.EnterInTime(userAttendance);
        }

        public Task<UserResponceModel> EnterOutTime(UserAttendanceModel userAttendance)
        {
            return UserList.EnterOutTime(userAttendance);
        }

        public async Task<UserResponceModel> ResetPassword(PasswordResetView emp)
        {
            return await UserList.ResetPassword(emp);
        }
        public async Task<IEnumerable<EmpDocumentView>> GetDocumentType()
        {
            return await UserList.GetDocumentType();
        }

        public async Task<IEnumerable<DocumentInfoView>> GetDocumentList()
        {
            return await UserList.GetDocumentList();
        }

        public async Task<DocumentInfoView> UploadDocument(DocumentInfoView doc)
        {
            return await UserList.UploadDocument(doc);
        }

        public async Task<UserResponceModel> UserLockScreen(LoginRequest request)
        {
            return await UserList.UserLockScreen(request);
        }

        public async Task<UserResponceModel> UserBirsthDayWish(Guid UserId)
        {
            return await UserList.UserBirsthDayWish(UserId);
        }
        public async Task<IEnumerable<EmpDetailsView>> UserEdit()
        {
            return await UserList.UserEdit();
        }

        public async Task<EmpDetailsView> GetById(Guid id)
        {
            return await UserList.GetById(id);
        }

        public async Task<Guid> UpdateUser(UserEditViewModel employee)
        {
            return await UserList.UpdateUser(employee);
        }
    }
}

