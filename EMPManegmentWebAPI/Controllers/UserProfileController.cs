using Azure;
using Azure.Core;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.AGGridModels;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.FormMaster;
using EMPManegment.EntityModels.ViewModels.FormPermissionMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.Inretface.Interface.ProjectDetails;
using EMPManegment.Inretface.Interface.UserAttendance;
using EMPManegment.Inretface.Interface.UserList;
using EMPManegment.Inretface.Interface.UsersLogin;
using EMPManegment.Inretface.Services.TaskServices;
using EMPManegment.Inretface.Services.UserAttendanceServices;
using EMPManegment.Inretface.Services.UserListServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Data;
using System.Net;
#nullable disable

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        public IUserDetailsServices UserListServices { get; }
        public IUserAttendanceServices UserAttendance { get; }

        public UserProfileController(IUserDetailsServices userListServices, IUserAttendanceServices userAttendance)
        {
            UserListServices = userListServices;
            UserAttendance = userAttendance;
        }


        [HttpGet]
        [Route("GetAllUsersDetails")]
        [Authorize]
        public async Task<IActionResult> GetAllUsersDetails()
        {
            var userList = await UserListServices.GetUsersDetails();
            return Ok(new { code = (int)HttpStatusCode.OK, data = userList });
        }

        [HttpGet]
        [Route("GetActiveDeactiveUserList")]
        public async Task<IActionResult> GetActiveDeactiveUserList()
        {
            var activedeactive = await UserListServices.GetActiveDeactiveUserList();
            return Ok(new { code = (int)HttpStatusCode.OK, data = activedeactive });
        }


        [HttpPost]
        [Route("GetAllUserList")]
        public async Task<AGGridResponseModel<EmpDetailsView>> GetAllUserList(AGGridRequestModel UserRequest)
        {
            var UserList = await UserListServices.GetUsersList(UserRequest);
            return new AGGridResponseModel<EmpDetailsView>
            {
                Data = UserList.Data,
                RecordsTotal = UserList.RecordsTotal,
            };
        }

        [HttpGet]
        [Route("GetUsersNameList")]
        public async Task<IActionResult> GetUsersNameList()
        {
            var getUsersNameList = await UserListServices.GetUsersNameList();
            return Ok(new { code = (int)HttpStatusCode.OK, data = getUsersNameList.ToList() });
        }

        [HttpPost]
        [Route("ActiveDeactiveUsers")]
        public async Task<IActionResult> ActiveDeactiveUsers(Guid UserId, Guid UpdatedBy)
        {
            UserResponceModel responseModel = new UserResponceModel();

            var userName = await UserListServices.ActiveDeactiveUsers(UserId, UpdatedBy);
            try
            {

                if (userName != null)
                {

                    responseModel.Code = (int)HttpStatusCode.OK;
                    responseModel.Message = userName.Message;
                }
                else
                {
                    responseModel.Message = userName.Message;
                    responseModel.Code = userName.Code;
                }
            }
            catch (Exception ex)
            {
                responseModel.Code = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = "An error occurred while processing the request.";
            }
            return StatusCode(responseModel.Code, responseModel);
        }

        [HttpGet]
        [Route("GetDocumentType")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDocumentType()
        {
            IEnumerable<EmpDocumentView> getDocumentType = await UserListServices.GetDocumentType();
            return Ok(new { code = (int)HttpStatusCode.OK, data = getDocumentType.ToList() });
        }

        [HttpPost]
        [Route("GetDocumentList")]
        public async Task<AGGridResponseModel<DocumentInfoView>> GetDocumentList(AGGridRequestModel DocumentRequest)
        {
            var DocumentList = await UserListServices.GetDocumentList(DocumentRequest);
            return new AGGridResponseModel<DocumentInfoView>
            {
                Data = DocumentList.Data,
                RecordsTotal = DocumentList.RecordsTotal,
            };
        }

        [HttpPost]
        [Route("UploadDocument")]
        public async Task<IActionResult> UploadDocument(DocumentInfoView UploadDocument)
        {
            var uploadDocument = await UserListServices.UploadDocument(UploadDocument);
            return Ok(new { code = (int)HttpStatusCode.OK, data = uploadDocument });
        }

        [HttpPost]
        [Route("ResetUserPassword")]
        public async Task<IActionResult> ResetUserPassword(PasswordResetView ResetPassword)
        {
            ApiResponseModel responseModel = new ApiResponseModel();

            var resetPassword = await UserListServices.ResetPassword(ResetPassword);
            try
            {
                if (resetPassword.Code != (int)HttpStatusCode.NotFound && resetPassword.Code != (int)HttpStatusCode.InternalServerError)
                {

                    responseModel.code = (int)HttpStatusCode.OK;
                    responseModel.message = resetPassword.Message;
                }
                else
                {
                    responseModel.message = resetPassword.Message;
                    responseModel.code = resetPassword.Code;
                }
            }
            catch (Exception ex)
            {
                responseModel.code = (int)HttpStatusCode.InternalServerError;
                responseModel.message = "An error occurred while processing the request.";
            }
            return StatusCode(responseModel.code, responseModel);
        }

        [HttpPost]
        [Route("UnLockScreen")]
        public async Task<IActionResult> UnLockScreen(LoginRequest UnlockScreen)
        {
            LoginResponseModel apiResponseModel = new LoginResponseModel();
            try
            {

                var unlockScreen = await UserListServices.UserLockScreen(UnlockScreen);

                if (unlockScreen.Code != (int)HttpStatusCode.NotFound && unlockScreen.Code != (int)HttpStatusCode.InternalServerError)
                {
                    apiResponseModel.Code = (int)HttpStatusCode.OK;
                    apiResponseModel.Message = unlockScreen.Message;
                }
                else
                {
                    apiResponseModel.Message = unlockScreen.Message;
                    apiResponseModel.Code = unlockScreen.Code;
                }
            }
            catch (Exception ex)
            {
                apiResponseModel.Code = (int)HttpStatusCode.InternalServerError;
                apiResponseModel.Message = "An error occurred while processing the request.";
            }
            return StatusCode(apiResponseModel.Code, apiResponseModel);
        }

        [HttpPost]
        [Route("GetUserAttendanceList")]
        public async Task<AGGridResponseModel<UserAttendanceModel>> GetUserAttendanceList(AGGridRequestModel AttendanceRequest)
        {
            var UserAttendanceList = await UserAttendance.GetUserAttendanceList(AttendanceRequest);
            return new AGGridResponseModel<UserAttendanceModel>
            {
                Data = UserAttendanceList.Data,
                RecordsTotal = UserAttendanceList.RecordsTotal
            };
        }

        [HttpGet]
        [Route("UserEdit")]
        public async Task<IActionResult> UserEdit()
        {
            IEnumerable<EmpDetailsView> userEdit = await UserListServices.UserEdit();
            return Ok(new { code = (int)HttpStatusCode.OK, data = userEdit.ToList() });
        }

        [HttpPost]
        [Route("UpdateUserOutTime")]
        public async Task<IActionResult> UpdateUserOutTime(UserAttendanceModel UpdateOutTime)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var updateUserOutTime = UserAttendance.UpdateUserOutTime(UpdateOutTime);
                if (updateUserOutTime.Result.Code != (int)HttpStatusCode.NotFound && updateUserOutTime.Result.Code != (int)HttpStatusCode.InternalServerError)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = updateUserOutTime.Result.Message;
                }
                else
                {
                    response.Code = updateUserOutTime.Result.Code;
                    response.Message = updateUserOutTime.Result.Message;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "An error occurred while processing the request.";
            }
            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Route("GetUserAttendanceById")]
        public async Task<IActionResult> GetUserAttendanceById(int attendanceId)
        {
            IEnumerable<UserAttendanceModel> getUserAttendance = await UserAttendance.GetUserAttendanceById(attendanceId);
            return Ok(new { code = (int)HttpStatusCode.OK, data = getUserAttendance });
        }

        [HttpGet]
        [Route("GetEmployeeById")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            var userProfile = await UserListServices.GetEmployeeById(id);
            return Ok(new { code = (int)HttpStatusCode.OK, data = userProfile });
        }

        [HttpPost]
        [Route("UpdateUserDetails")]
        public async Task<IActionResult> UpdateUserDetails(UserEditViewModel UpdateUser)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var updateUser = await UserListServices.UpdateUserDetails(UpdateUser);
                if (updateUser.Code != (int)HttpStatusCode.NotFound)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = updateUser.Message;
                }
                else
                {
                    response.Code = updateUser.Code;
                    response.Message = updateUser.Message;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "An error occurred while processing the request.";
            }
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("GetMyAttendanceList")]
        public async Task<AGGridResponseModel<UserAttendanceModel>> GetMyAttendanceList(AGGridRequestModel AttendanceRequest)
        {
            var getAttendanceList = await UserAttendance.GetMyAttendanceList(AttendanceRequest);
            return new AGGridResponseModel<UserAttendanceModel>
            {
                Data = getAttendanceList.Data,
                RecordsTotal = getAttendanceList.RecordsTotal
            };
        }

        [HttpPost]
        [Route("GetMemberList")]
        public async Task<IActionResult> GetMemberList()
        {
            var userList = await UserListServices.GetUsersDetails();
            return Ok(new { code = (int)HttpStatusCode.OK, data = userList });
        }

        [HttpPost]
        [Route("GetSearchAttendanceList")]
        public async Task<IActionResult> GetSearchAttendanceList(AttendanceRequestDataTableModel AttendanceRequestModel)
        {
            var Attendancelist = await UserAttendance.GetSearchAttendanceList(AttendanceRequestModel);
            return Ok(new { code = (int)HttpStatusCode.OK, data = Attendancelist });
        }

        [HttpPost]
        [Route("GetSearchEmplList")]
        public async Task<IActionResult> GetSearchEmpList(EmpDetailsModel EmpList)
        {
            var EmployeeList = await UserListServices.GetSearchEmpList(EmpList);
            return Ok(new { code = (int)HttpStatusCode.OK, data = EmployeeList });
        }
        [HttpPost]
        [Route("UpdateUserExeperience")]
        public async Task<IActionResult> UpdateUserExeperience(EmpDetailsView UpdateDate)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var updateUser = await UserListServices.UpdateUserExeperience(UpdateDate);
                if (updateUser.Code != (int)HttpStatusCode.InternalServerError)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = updateUser.Message;
                }
                else
                {
                    response.Code = updateUser.Code;
                    response.Message = updateUser.Message;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "An error occurred while processing the request.";
            }
            return StatusCode(response.Code, response);
        }
        [HttpPost]
        [Route("UserProfilePhoto")]
        public async Task<IActionResult> UserProfilePhoto(EmpDetailsView Profile)
        {
            var UploadImage = await UserListServices.UserProfilePhoto(Profile);
            return Ok(new { code = (int)HttpStatusCode.OK, data = UploadImage });
        }

        [HttpPost]
        [Route("GetMySearchAttendanceList")]
        public async Task<IActionResult> GetMySearchAttendanceList(SearchAttendanceModel GetSearchAttendanceList)
        {
            IEnumerable<UserAttendanceModel> getAttendanceList = await UserAttendance.GetMySearchAttendanceList(GetSearchAttendanceList);
            return Ok(new { code = (int)HttpStatusCode.OK, data = getAttendanceList.ToList() }); ;
        }

        [HttpPost]
        [Route("AddUserAttendance")]
        public async Task<IActionResult> AddUserAttendance(UserAttendanceModel AddUser)
        {
            UserResponceModel userresponsemodel = new UserResponceModel();
            try
            {
                var Adduser = UserAttendance.AddUserAttendance(AddUser);
                if (Adduser.Result.Code != (int)HttpStatusCode.InternalServerError)
                {
                    userresponsemodel.Code = (int)HttpStatusCode.OK;
                    userresponsemodel.Message = Adduser.Result.Message;
                }
                else
                {
                    userresponsemodel.Message = Adduser.Result.Message;
                    userresponsemodel.Code = Adduser.Result.Code;
                }
            }
            catch (Exception ex)
            {
                userresponsemodel.Code = (int)HttpStatusCode.InternalServerError;
                userresponsemodel.Message = "An error occurred while processing the request.";
            }
            return StatusCode(userresponsemodel.Code, userresponsemodel);
        }
        [HttpGet]
        [Route("GetRolewiseFormPermissionList")]

        public async Task<IActionResult> GetRolewiseFormPermissionList()
        {
            IEnumerable<RolewiseFormPermissionModel> rolewiseFormPermissionList = await UserListServices.GetRolewiseFormPermissionList();
            return Ok(new { code = 200, data = rolewiseFormPermissionList.ToList() });
        }
        [HttpPost]
        [Route("GetUserRolewiseFormListById")]

        public async Task<IActionResult> GetUserRolewiseFormListById(Guid RoleId)
        {
            ApiResponseModel response = new ApiResponseModel();
            List<RolewiseFormPermissionModel> RolewiseFormList = await UserListServices.GetUserRolewiseFormListById(RoleId);

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
        [Route("UpdateUserMultipleRolewiseFormPermission")]

        public async Task<IActionResult> UpdateUserMultipleRolewiseFormPermission(List<RolewiseFormPermissionModel> RolewiseFormPermission)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
                var rolewiseFormPermission = UserListServices.UpdateUserMultipleRolewiseFormPermission(RolewiseFormPermission);
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
        [Route("ActiveDeactiveRole")]
        public async Task<IActionResult> ActiveDeactiveRole(Guid roleId)
        {
            UserResponceModel responseModel = new UserResponceModel();

            var roleData = await UserListServices.ActiveDeactiveRole(roleId);
            try
            {
                if (roleData.Code == 200)
                {
                    responseModel.Code = roleData.Code;
                    responseModel.Message = roleData.Message;
                }
                else
                {
                    responseModel.Message = roleData.Message;
                    responseModel.Code = roleData.Code;
                }
            }
            catch (Exception ex)
            {
                responseModel.Code = (int)HttpStatusCode.InternalServerError;
            }
            return StatusCode(responseModel.Code, responseModel);
        }
    }
}
