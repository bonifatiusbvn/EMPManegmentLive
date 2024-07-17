﻿using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.FormMaster;
using EMPManegment.EntityModels.ViewModels.FormPermissionMaster;
using EMPManegment.EntityModels.ViewModels.UserModels;
using EMPManegment.Inretface.Interface.FormPermissionMaster;
using EMPManegment.Inretface.Interface.MasterList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
#nullable disable
namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FormPermissionMasterController : ControllerBase
    {
        public FormPermissionMasterController(IFormPermissionMaster rolewisePermissionMaster, IFormMaster formMaster)
        {
            RolewisePermissionMaster = rolewisePermissionMaster;
            FormMaster = formMaster;
        }

        public IFormPermissionMaster RolewisePermissionMaster { get; }
        public IFormMaster FormMaster { get; }

        [HttpPost]
        [Route("GetRolewiseFormListById")]
        public async Task<IActionResult> GetRolewiseFormListById(Guid RoleId)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
                List<RolewiseFormPermissionModel> RolewiseFormList = await RolewisePermissionMaster.GetRolewiseFormListById(RoleId);

                if (RolewiseFormList.Count == 0)
                {
                    response.code = (int)HttpStatusCode.NotFound;
                    response.message = "Error in getting FormList.";
                }
                else
                {
                    response.code = (int)HttpStatusCode.OK;
                    response.data = RolewiseFormList.ToList();
                }
            }
            catch (Exception ex)
            {
                response.code = (int)HttpStatusCode.InternalServerError;
                response.message = "An error occurred while processing the request.";
            }
            return StatusCode(response.code, response);
        }

        [HttpPost]
        [Route("UpdateMultipleRolewiseFormPermission")]
        public async Task<IActionResult> UpdateMultipleRolewiseFormPermission(List<RolewiseFormPermissionModel> RolewiseFormPermission)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
                var rolewiseFormPermission = RolewisePermissionMaster.UpdateMultipleRolewiseFormPermission(RolewiseFormPermission);
                if (rolewiseFormPermission.Result.code != (int)HttpStatusCode.NotFound && rolewiseFormPermission.Result.code != (int)HttpStatusCode.InternalServerError)
                {
                    response.code = (int)HttpStatusCode.OK;
                    response.message = rolewiseFormPermission.Result.message;
                }
                else
                {
                    response.message = rolewiseFormPermission.Result.message;
                    response.code = rolewiseFormPermission.Result.code;
                }
            }
            catch (Exception ex)
            {
                response.code = (int)HttpStatusCode.InternalServerError;
                response.message = "An error occurred while processing the request.";
            }
            return StatusCode(response.code, response);
        }

        [HttpPost]
        [Route("CreateUserRole")]
        public async Task<IActionResult> CreateUserRole(UserRoleModel roleDetails)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
                var RoleData = RolewisePermissionMaster.CreateUserRole(roleDetails);
                if (RoleData.Result.code != (int)HttpStatusCode.NotFound && RoleData.Result.code != (int)HttpStatusCode.InternalServerError)
                {
                    response.code = (int)HttpStatusCode.OK;
                    response.message = RoleData.Result.message;
                }
                else
                {
                    response.message = RoleData.Result.message;
                    response.code = RoleData.Result.code;
                }
            }
            catch (Exception ex)
            {
                response.code = (int)HttpStatusCode.InternalServerError;
                response.message = "An error occurred while processing the request.";
            }
            return StatusCode(response.code, response);
        }
        [HttpGet]
        [Route("FormList")]
        public async Task<IActionResult> FormList()
        {
            try
            {
                IEnumerable<FormMasterModel> FormData = await RolewisePermissionMaster.FormList();
                return Ok(new { code = (int)HttpStatusCode.OK, data = FormData.ToList() });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpPost]
        [Route("CreateRolewisePermissionForm")]
        public async Task<IActionResult> CreateRolewisePermissionForm(int FormId, Guid userId)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
                var Form = RolewisePermissionMaster.CreateRolewisePermissionForm(FormId, userId);
                if (Form.Result.code != (int)HttpStatusCode.NotFound && Form.Result.code != (int)HttpStatusCode.InternalServerError)
                {
                    response.code = (int)HttpStatusCode.OK;
                    response.message = Form.Result.message;
                }
                else
                {
                    response.message = Form.Result.message;
                    response.code = Form.Result.code;
                }
            }
            catch (Exception ex)
            {
                response.code = (int)HttpStatusCode.InternalServerError;
                response.message = "An error occurred while processing the request.";
            }
            return StatusCode(response.code, response);
        }

        [HttpPost]
        [Route("CreateUserForm")]
        public async Task<IActionResult> CreateUserForm(Guid UserId)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
                var UserDetail = RolewisePermissionMaster.CreateUserForm(UserId);
                if (UserDetail.Result.code != (int)HttpStatusCode.NotFound && UserDetail.Result.code != (int)HttpStatusCode.InternalServerError)
                {
                    response.code = (int)HttpStatusCode.OK;
                    response.message = UserDetail.Result.message;
                }
                else
                {
                    response.message = UserDetail.Result.message;
                    response.code = UserDetail.Result.code;
                }
            }
            catch (Exception ex)
            {
                response.code = (int)HttpStatusCode.InternalServerError;
                response.message = "An error occurred while processing the request.";
            }
            return StatusCode(response.code, response);
        }
        [HttpPost]
        [Route("GetUserFormListById")]
        public async Task<IActionResult> GetUserFormListById(Guid UserId)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
                List<UserPermissionModel> UserFormList = await RolewisePermissionMaster.GetUserFormListById(UserId);

                if (UserFormList.Count == 0)
                {
                    response.code = (int)HttpStatusCode.NotFound;
                    response.message = "No data found.";
                }
                else
                {
                    response.code = (int)HttpStatusCode.OK;
                    response.data = UserFormList.ToList();
                }
            }
            catch (Exception)
            {
                response.code = (int)HttpStatusCode.InternalServerError;
                response.message = "An error occurred while processing the request.";
            }
            return StatusCode(response.code, response);
        }
        [HttpPost]
        [Route("UpdateMultipleUserFormPermission")]
        public async Task<IActionResult> UpdateMultipleUserFormPermission(List<UserPermissionModel> UpdatedUserFormPermissions)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
                var userFormPermission = RolewisePermissionMaster.UpdateMultipleUserFormPermission(UpdatedUserFormPermissions);
                if (userFormPermission.Result.code != (int)HttpStatusCode.NotFound && userFormPermission.Result.code != (int)HttpStatusCode.InternalServerError)
                {
                    response.code = (int)HttpStatusCode.OK;
                    response.message = userFormPermission.Result.message;
                }
                else
                {
                    response.message = userFormPermission.Result.message;
                    response.code = userFormPermission.Result.code;
                }
            }
            catch (Exception ex)
            {
                response.code = (int)HttpStatusCode.InternalServerError;
                response.message = "An error occurred while processing the request.";
            }
            return StatusCode(response.code, response);
        }
    }
}
