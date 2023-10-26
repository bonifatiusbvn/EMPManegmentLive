using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
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

        public async Task<IEnumerable<EmpDetailsView>> GetUsersList()
        {
            return await UserList.GetUsersList();
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
