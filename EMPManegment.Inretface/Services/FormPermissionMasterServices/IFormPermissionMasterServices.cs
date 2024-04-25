using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.FormPermissionMaster;
using EMPManegment.EntityModels.ViewModels.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Services.FormPermissionMasterServices
{
    public interface IFormPermissionMasterServices
    {
        Task<List<RolewiseFormPermissionModel>> GetRolewiseFormListById(int RoleId);
        Task<ApiResponseModel> UpdateMultipleRolewiseFormPermission(List<RolewiseFormPermissionModel> UpdatedRolewiseFormPermissions);
        Task<ApiResponseModel> CreateUserRole(UserRoleModel roleDetails);
    }
}
