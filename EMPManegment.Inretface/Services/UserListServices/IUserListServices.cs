using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMPManegment.EntityModels.View_Model;

namespace EMPManegment.Inretface.Services.UserListServices
{
    public interface IUserListServices
    {
       Task<IEnumerable<EmpDetailsView>> GetUsersList();
    }
}
