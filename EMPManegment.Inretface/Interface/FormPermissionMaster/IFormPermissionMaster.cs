using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.FormPermissionMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Interface.FormPermissionMaster
{
    public interface IFormPermissionMaster
    {
        Task<List<RolewiseFormPermissionModel>> GetRolewiseFormListById(int RoleId);
        Task<ApiResponseModel> UpdateMultipleRolewiseFormPermission(List<RolewiseFormPermissionModel> UpdatedRolewiseFormPermissions);
    }
}
