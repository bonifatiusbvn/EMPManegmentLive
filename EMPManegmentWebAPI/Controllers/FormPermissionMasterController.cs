using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.FormPermissionMaster;
using EMPManegment.EntityModels.ViewModels.UserModels;
using EMPManegment.Inretface.Interface.FormPermissionMaster;
using EMPManegment.Inretface.Interface.MasterList;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> GetRolewiseFormListById(int RoleId)
        {
            ApiResponseModel response = new ApiResponseModel();
            List<RolewiseFormPermissionModel> RolewiseFormList = await RolewisePermissionMaster.GetRolewiseFormListById(RoleId);

            if (RolewiseFormList.Count == 0)
            {
                response.code = 400;
            }
            else
            {
                response.code = 200;
                response.data = RolewiseFormList.ToList();
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
                if (rolewiseFormPermission.Result.code == 200)
                {
                    response.code = (int)HttpStatusCode.OK;
                    response.message = rolewiseFormPermission.Result.message;
                }
                else
                {
                    response.message = rolewiseFormPermission.Result.message;
                    response.code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                if (RoleData.Result.code == 200)
                {
                    response.code = (int)HttpStatusCode.OK;
                    response.message = RoleData.Result.message;
                }
                else
                {
                    response.message = RoleData.Result.message;
                    response.code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            return StatusCode(response.code, response);
        }
    }
}
