using EMPManegment.EntityModels.View_Model;
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
    }
}
