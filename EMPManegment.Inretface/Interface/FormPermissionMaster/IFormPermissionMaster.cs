using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.FormMaster;
using EMPManegment.EntityModels.ViewModels.FormPermissionMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Interface.FormPermissionMaster
{
    public interface IFormPermissionMaster
    {
        Task<List<RolewiseFormPermissionModel>> GetRolewiseFormListById(Guid RoleId);
        Task<ApiResponseModel> UpdateMultipleRolewiseFormPermission(List<RolewiseFormPermissionModel> UpdatedRolewiseFormPermissions);
        Task<ApiResponseModel> CreateUserRole(UserRoleModel roleDetails);
        Task<IEnumerable<FormMasterModel>> FormList();

        Task<ApiResponseModel> CreateRolewisePermissionForm(int FormId, Guid userId);
        Task<ApiResponseModel> CreateUserForm(Guid UserId);
        //Task<List<RolewiseFormPermissionModel>> GetUserFormListById(Guid RoleId);


    }
}
