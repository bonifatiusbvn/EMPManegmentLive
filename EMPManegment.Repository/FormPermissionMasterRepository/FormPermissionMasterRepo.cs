using EMPManagment.API;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.FormPermissionMaster;
using EMPManegment.Inretface.Interface.FormPermissionMaster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Repository.FormPermissionMasterRepository
{
    public class FormPermissionMasterRepo : IFormPermissionMaster
    {
        public FormPermissionMasterRepo(BonifatiusEmployeesContext context)
        {
            Context = context;
        }

        public BonifatiusEmployeesContext Context { get; }

        public async Task<List<RolewiseFormPermissionModel>> GetRolewiseFormListById(int RoleId)
        {
            var UserData = new List<RolewiseFormPermissionModel>();
            var data = await (from e in Context.TblRolewiseFormPermissions.Where(x => x.RoleId == RoleId)
                              join f in Context.TblForms on e.FormId equals f.FormId
                              join r in Context.TblRoleMasters on e.RoleId equals r.Id
                              where f.IsActive == true
                              select new RolewiseFormPermissionModel
                              {
                                  Id = e.Id,
                                  Role = r.Role,
                                  RoleId = e.RoleId,
                                  FormId = e.FormId,
                                  FormName = f.FormName,
                                  IsViewAllow = e.IsViewAllow,
                                  IsEditAllow = e.IsEditAllow,
                                  IsDeleteAllow = e.IsDeleteAllow,
                                  IsAddAllow = e.IsAddAllow,
                                  CreatedBy = e.CreatedBy,
                                  CreatedOn = e.CreatedOn,
                              }).ToListAsync();


            if (data.Count != 0)
            {
                UserData.AddRange(data);
            }
            return UserData;
        }

        public async Task<ApiResponseModel> UpdateMultipleRolewiseFormPermission(List<RolewiseFormPermissionModel> UpdatedRolewiseFormPermissions)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
                foreach (var updatedPermission in UpdatedRolewiseFormPermissions)
                {
                    var existingPermissions = await Context.TblRolewiseFormPermissions
                        .Where(rp => rp.RoleId == updatedPermission.RoleId && rp.FormId == updatedPermission.FormId)
                        .ToListAsync();

                    if (existingPermissions.Any())
                    {
                        foreach (var Item in existingPermissions)
                        {
                            Item.IsAddAllow = updatedPermission.IsAddAllow;
                            Item.IsViewAllow = updatedPermission.IsViewAllow;
                            Item.IsEditAllow = updatedPermission.IsEditAllow;
                            Item.IsDeleteAllow = updatedPermission.IsDeleteAllow;
                            Context.Entry(Item).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        response.code = 404;
                        response.message = $"Permissions with RoleId {updatedPermission.RoleId} and FormId {updatedPermission.FormId} not found.";
                        return response;
                    }
                }

                await Context.SaveChangesAsync();
                response.code = 200;
                response.message = "Rolewise permissions successfully updated.";
            }
            catch (Exception ex)
            {
                response.code = 400;
                response.message = "Error updating Rolewise Permissions";
            }
            return response;
        }
    }
}
