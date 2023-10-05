using EMPManegment.EntityModels.View_Model;
using EMPManegment.Inretface.Interface.UserList;
using EMPManegment.Inretface.Services.UserListServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.UserList
{
    public class UserListService : IUserListServices
    {
        public IUserList UserList { get; }
        public UserListService(IUserList userList)
        {
            UserList = userList;
        }

        public async Task<IEnumerable<EmpDetailsView>> GetUsersList()
        {
            return await UserList.GetUsersList();
        }
    }
}
