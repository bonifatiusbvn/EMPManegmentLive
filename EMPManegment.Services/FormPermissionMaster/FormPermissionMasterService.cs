using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.FormPermissionMaster;
using EMPManegment.EntityModels.ViewModels.UserModels;
using EMPManegment.Inretface.Interface.FormPermissionMaster;
using EMPManegment.Inretface.Interface.MasterList;
using EMPManegment.Inretface.Services.FormPermissionMasterServices;
using EMPManegment.Inretface.Services.MasterList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.FormPermissionMaster
{
    public class FormPermissionMasterService: IFormPermissionMasterServices
    {
        public FormPermissionMasterService(IFormPermissionMaster formPermissionMaster)
        {
            FormPermissionMaster = formPermissionMaster;
        }

        public IFormPermissionMaster FormPermissionMaster { get; }
        public async Task<List<RolewiseFormPermissionModel>> GetRolewiseFormListById(Guid RoleId)
        {
            return await FormPermissionMaster.GetRolewiseFormListById(RoleId);
        }
        public async Task<ApiResponseModel> UpdateMultipleRolewiseFormPermission(List<RolewiseFormPermissionModel> UpdatedRolewiseFormPermissions)
        {
            return await FormPermissionMaster.UpdateMultipleRolewiseFormPermission(UpdatedRolewiseFormPermissions);
        }
        public async Task<ApiResponseModel> CreateUserRole(UserRoleModel roleDetails)
        {
            return await FormPermissionMaster.CreateUserRole(roleDetails);
        }
    }
}
