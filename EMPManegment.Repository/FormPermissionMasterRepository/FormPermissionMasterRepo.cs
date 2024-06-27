using EMPManagment.API;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.FormMaster;
using EMPManegment.EntityModels.ViewModels.FormPermissionMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.UserModels;
using EMPManegment.Inretface.Interface.FormPermissionMaster;
using EMPManegment.Inretface.Interface.UserList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
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
                            Item.UpdatedBy = updatedPermission.CreatedBy;
                            Item.UpdatedOn = DateTime.Now;
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
                response.message = "Error in updating rolewise permissions";
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
            catch (Exception ex)
            {
                response.code = 400;
                response.message = "Error in creating role";
            }
            return response;
        }

        public async Task<IEnumerable<FormMasterModel>> FormList()
        {
            try
            {
                IEnumerable<FormMasterModel> Form = Context.TblForms.ToList().Select(a => new FormMasterModel
                {
                    FormId = a.FormId,
                    FormName = a.FormName,
                });
                return Form;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApiResponseModel> CreateRolewisePermissionForm(int FormId, Guid userId)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
                var Roles = await Context.TblRoleMasters.ToListAsync();
                var formDetails = new List<TblRolewiseFormPermission>();

                foreach (var Role in Roles)
                {
                    var isFormAlreadyExist = await Context.TblRolewiseFormPermissions
                        .AnyAsync(e => e.FormId == FormId && e.RoleId == Role.RoleId);

                    if (!isFormAlreadyExist)
                    {
                        var Form = new TblRolewiseFormPermission()
                        {
                            RoleId = Role.RoleId,
                            FormId = FormId,
                            IsAddAllow = true,
                            IsViewAllow = true,
                            IsEditAllow = true,
                            IsDeleteAllow = true,
                            CreatedOn = DateTime.Now,
                            CreatedBy = userId,
                        };
                        formDetails.Add(Form);
                    }
                }

                if (formDetails.Any())
                {
                    await Context.TblRolewiseFormPermissions.AddRangeAsync(formDetails);
                    await Context.SaveChangesAsync();
                    response.code = 200;
                    response.message = "Forms added successfully!";
                }
                else
                {
                    response.code = 400;
                    response.message = "Forms already exist!";
                }
            }
            catch (Exception ex)
            {
                response.code = 500;
                response.message = $"Error in inserting form: {ex.Message}";
            }

            return response;
        }


        [HttpPost]
        public async Task<ApiResponseModel> CreateUserForm(Guid UserId)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {

                bool isRoleAlreadyExists = Context.TblUserFormPermissions.Any(x => x.UserId == UserId);
                if (isRoleAlreadyExists == true)
                {
                    response.message = "User already exists";
                    response.code = 404;
                }

                else
                {

                    var forms = Context.TblForms.ToList();
                    var userFormPermissions = new List<TblUserFormPermission>();

                    foreach (var form in forms)
                    {
                        var permissions = new TblUserFormPermission
                        {
                            UserId = UserId,
                            FormId = form.FormId,
                            IsAddAllow = true,
                            IsViewAllow = true,
                            IsEditAllow = true,
                            IsDeleteAllow = true,
                            CreatedOn = DateTime.Now,
                            CreatedBy = UserId,
                        };
                        userFormPermissions.Add(permissions);
                    }
                    Context.TblUserFormPermissions.AddRange(userFormPermissions);
                    Context.SaveChanges();

                    response.code = 200;
                    response.message = "User add successfully!";

                }
            }
            catch (Exception ex)
            {
                response.code = 400;
                response.message = "Error in creating user";
            }
            return response;
        }
        public async Task<List<UserPermissionModel>> GetUserFormListById(Guid UserId)
        {
            var UserData = new List<UserPermissionModel>();
            var data = await (from e in Context.TblUserFormPermissions.Where(x => x.UserId == UserId)
                              join f in Context.TblForms on e.FormId equals f.FormId
                              orderby f.OrderId ascending
                              select new UserPermissionModel
                              {
                                  Id = e.Id,
                                  UserId = e.UserId,
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

        public async Task<ApiResponseModel> UpdateMultipleUserFormPermission(List<UserPermissionModel> UpdatedUserFormPermissions)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
                foreach (var updatedUserPermission in UpdatedUserFormPermissions)
                {
                    var existingPermissions = await Context.TblUserFormPermissions
                        .Where(up => up.UserId == updatedUserPermission.UserId && up.FormId == updatedUserPermission.FormId)
                        .FirstOrDefaultAsync();

                    if (existingPermissions != null)
                    {
                        existingPermissions.IsAddAllow = updatedUserPermission.IsAddAllow;
                        existingPermissions.IsViewAllow = updatedUserPermission.IsViewAllow;
                        existingPermissions.IsEditAllow = updatedUserPermission.IsEditAllow;
                        existingPermissions.IsDeleteAllow = updatedUserPermission.IsDeleteAllow;
                        existingPermissions.UpdatedBy = updatedUserPermission.CreatedBy;
                        existingPermissions.UpdatedOn = DateTime.Now;
                        Context.Entry(existingPermissions).State = EntityState.Modified;
                    }
                    else
                    {
                        response.code = 404;
                        response.message = $"Permissions with UserId {updatedUserPermission.UserId} and FormId {updatedUserPermission.FormId} not found.";
                        return response;
                    }
                }

                await Context.SaveChangesAsync();
                response.code = 200;
                response.message = "User permissions successfully updated.";
            }
            catch (Exception ex)
            {
                response.code = 400;
                response.message = "Error in updating user permissions";
            }
            return response;
        }
    }
}
