using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;

namespace EMPManegment.Inretface.Interface.UserList
{
    public interface IUserDetails
    {
        Task<IEnumerable<EmpDetailsView>> GetUsersList();
        Task<IEnumerable<EmpDetailsView>> UserEdit();
        Task<EmpDetailsView> GetById(Guid id);
        Task<Guid> UpdateUser(UserEditViewModel employee);

    }
}
