using EMPManagment.API;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.Common;
using EMPManegment.EntityModels.ViewModels.FormMaster;
using EMPManegment.EntityModels.ViewModels.FormPermissionMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.EntityModels.ViewModels.UserModels;
using EMPManegment.Inretface.Interface.FormPermissionMaster;
using EMPManegment.Inretface.Interface.UserList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Repository.FormPermissionMasterRepository
{
    public class FormPermissionMasterRepo : IFormPermissionMaster
    {
        public FormPermissionMasterRepo(BonifatiusEmployeesContext context, IConfiguration configuration)
        {
            Context = context;
            _configuration = configuration;
        }

        public BonifatiusEmployeesContext Context { get; }
        public IConfiguration _configuration { get; }

        public async Task<List<RolewiseFormPermissionModel>> GetRolewiseFormListById(Guid RoleId)
        {
            try
            {
                string dbConnectionStr = _configuration.GetConnectionString("EMPDbconn");

                var sqlPar = new SqlParameter[]
                {
                   new SqlParameter("@RoleId", RoleId),
                };

                var DS = DbHelper.GetDataSet("GetRolewiseFormListById", CommandType.StoredProcedure, sqlPar, dbConnectionStr);

                List<RolewiseFormPermissionModel> UserData = new List<RolewiseFormPermissionModel>();

                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        var formDetails = new RolewiseFormPermissionModel
                        {
                            Id = row["Id"] != DBNull.Value ? (int)row["Id"] : 0,
                            Role = row["Role"]?.ToString(),
                            RoleId = row["RoleId"] != DBNull.Value ? (Guid)row["RoleId"] : Guid.Empty,
                            FormId = row["FormId"] != DBNull.Value ? (int)row["FormId"] : 0,
                            FormName = row["FormName"]?.ToString(),
                            IsViewAllow = (bool)row["IsViewAllow"],
                            IsEditAllow = (bool)row["IsEditAllow"],
                            IsDeleteAllow = (bool)row["IsDeleteAllow"],
                            IsAddAllow = (bool)row["IsAddAllow"],
                            CreatedBy = row["CreatedBy"] != DBNull.Value ? (Guid)row["CreatedBy"] : Guid.Empty,
                            CreatedOn = row["CreatedOn"] != DBNull.Value ? (DateTime)row["CreatedOn"] : DateTime.MinValue
                        };
                        UserData.Add(formDetails);
                    }
                }
                return UserData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                        response.code = (int)HttpStatusCode.NotFound;
                        response.message = $"Permissions with RoleId {updatedPermission.RoleId} and FormId {updatedPermission.FormId} not found.";
                        return response;
                    }
                }

                await Context.SaveChangesAsync();
                response.message = "Rolewise permissions successfully updated.";
            }
            catch (Exception ex)
            {
                response.code = (int)HttpStatusCode.InternalServerError;
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
                        response.code = (int)HttpStatusCode.NotFound;
                    }
                    else
                    {

                        var GetRoledata = Context.TblRoleMasters.Where(a => a.Role == roleDetails.Role).FirstOrDefault();
                        GetRoledata.IsDelete = false;
                        Context.TblRoleMasters.Update(GetRoledata);
                        Context.SaveChanges();
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

                    response.message = "Role add successfully!";
                    Context.TblRoleMasters.Add(rolemodel);
                    Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                response.code = (int)HttpStatusCode.InternalServerError;
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
                    response.message = "Forms added successfully!";
                }
                else
                {
                    response.code = (int)HttpStatusCode.NotFound;
                    response.message = "Forms already exist!";
                }
            }
            catch (Exception ex)
            {
                response.code = (int)HttpStatusCode.InternalServerError;
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
                    response.code = (int)HttpStatusCode.NotFound;
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


                    response.message = "User add successfully!";

                }
            }
            catch (Exception ex)
            {
                response.code = (int)HttpStatusCode.InternalServerError;
                response.message = "Error in creating user";
            }
            return response;
        }
        public async Task<List<UserPermissionModel>> GetUserFormListById(Guid UserId)
        {
            try
            {
                string dbConnectionStr = _configuration.GetConnectionString("EMPDbconn");

                var sqlPar = new SqlParameter[]
                {
                   new SqlParameter("@UserId", UserId),
                };

                var DS = DbHelper.GetDataSet("GetUserFormListById", CommandType.StoredProcedure, sqlPar, dbConnectionStr);

                List<UserPermissionModel> UserData = new List<UserPermissionModel>();

                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        var formDetails = new UserPermissionModel
                        {
                            Id = row["Id"] != DBNull.Value ? (int)row["Id"] : 0,
                            UserId = row["UserId"] != DBNull.Value ? (Guid)row["UserId"] : Guid.Empty,
                            FormId = row["FormId"] != DBNull.Value ? (int)row["FormId"] : 0,
                            FormName = row["FormName"]?.ToString(),
                            IsViewAllow = (bool)row["IsViewAllow"],
                            IsEditAllow = (bool)row["IsEditAllow"],
                            IsDeleteAllow = (bool)row["IsDeleteAllow"],
                            IsAddAllow = (bool)row["IsAddAllow"],
                            CreatedBy = row["CreatedBy"] != DBNull.Value ? (Guid)row["CreatedBy"] : Guid.Empty,
                            CreatedOn = row["CreatedOn"] != DBNull.Value ? (DateTime)row["CreatedOn"] : DateTime.MinValue
                        };
                        UserData.Add(formDetails);
                    }
                }
                return UserData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                        response.code = (int)HttpStatusCode.NotFound;
                        response.message = $"Permissions with UserId {updatedUserPermission.UserId} and FormId {updatedUserPermission.FormId} not found.";
                        return response;
                    }
                }

                await Context.SaveChangesAsync();
                response.message = "User permissions successfully updated.";
            }
            catch (Exception ex)
            {
                response.code = (int)HttpStatusCode.InternalServerError;
                response.message = "Error in updating user permissions";
            }
            return response;
        }
    }
}
