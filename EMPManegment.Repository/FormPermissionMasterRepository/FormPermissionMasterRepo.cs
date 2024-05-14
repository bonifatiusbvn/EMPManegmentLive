using EMPManagment.API;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.FormPermissionMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.UserModels;
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

        public async Task<List<RolewiseFormPermissionModel>> GetRolewiseFormListById(Guid RoleId)
        {
            var UserData = new List<RolewiseFormPermissionModel>();
            var data = await (from e in Context.TblRolewiseFormPermissions.Where(x => x.RoleId == RoleId)
                              join f in Context.TblForms on e.FormId equals f.FormId
                              join r in Context.TblRoleMasters on e.RoleId equals r.RoleId
                              orderby f.OrderId ascending
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
                response.message = "Error updating rolewise permissions";
            }
            return response;
        }

        public async Task<ApiResponseModel> CreateUserRole(UserRoleModel roleDetails)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
                bool isRoleAlreadyExists = Context.TblRoleMasters.Any(x => x.Role == roleDetails.Role);
                if (isRoleAlreadyExists == true)
                {
                    var RoleDetail = await Context.TblRoleMasters.SingleOrDefaultAsync(x => x.Role == roleDetails.Role);
                    if (RoleDetail.IsDelete == null || RoleDetail.IsDelete == false)
                    {
                        response.message = "Role already exists";
                        response.code = 404;
                    }
                    else
                    {

                        var GetRoledata = Context.TblRoleMasters.Where(a => a.Role == roleDetails.Role).FirstOrDefault();
                        GetRoledata.IsDelete = false;
                        Context.TblRoleMasters.Update(GetRoledata);
                        Context.SaveChanges();
                        response.code = 200;
                        response.data = roleDetails;
                        response.message = "Role add successfully!";
                    }
                }
                else
                {
                    var rolemodel = new TblRoleMaster()
                    {
                       RoleId = Guid.NewGuid(),
                       Role = roleDetails.Role,
                       IsActive = true,
                       IsDelete = false,
                       CreatedBy = roleDetails.CreatedBy,
                       CreatedOn = DateTime.Now,
                    };          

                    var forms = Context.TblForms.ToList();
                    var roleWiseFormPermissions = new List<TblRolewiseFormPermission>();

                    foreach (var form in forms)
                    {
                        var permissions = new TblRolewiseFormPermission
                        {
                            RoleId = rolemodel.RoleId,
                            FormId = form.FormId,
                            IsAddAllow = true,
                            IsViewAllow = true,
                            IsEditAllow = true,
                            IsDeleteAllow = true,
                            CreatedOn = DateTime.Now,
                            CreatedBy = roleDetails.CreatedBy,
                        };
                        roleWiseFormPermissions.Add(permissions);
                    }
                    Context.TblRolewiseFormPermissions.AddRange(roleWiseFormPermissions);
                    Context.SaveChanges();

                    response.code = 200;
                    response.message = "Role add successfully!";
                    Context.TblRoleMasters.Add(rolemodel);
                    Context.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                response.code = 400;
                response.message = "Error in creating role";
            }
            return response;
        }
    }
}
