using Azure;
using Azure.Core;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.UserAttendance;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.Inretface.Interface.UserList;
using EMPManegment.Inretface.Interface.UsersLogin;
using EMPManegment.Inretface.Services.UserAttendanceServices;
using EMPManegment.Inretface.Services.UserListServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using Microsoft.EntityFrameworkCore.Metadata;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.Inretface.Interface.ProjectDetails;


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
            return Ok(new { code = 200, data = userList });
        }

        [HttpGet]
        [Route("GetActiveDeactiveUserList")]
        public async Task<IActionResult> GetActiveDeactiveUserList()
        {
            var activedeactive = await UserListServices.GetActiveDeactiveUserList();
            return Ok(new { code = 200, data = activedeactive });
        }


        [HttpPost]
        [Route("GetAllUserList")]
        public async Task<IActionResult> GetAllUserList(DataTableRequstModel dataTable)
        {
            var userList = await UserListServices.GetUsersList(dataTable);
            return Ok(new { code = 200, data = userList });
        }

        [HttpGet]
        [Route("GetUsersNameList")]
        public async Task<IActionResult> GetUsersNameList()
        {
            var getUsersNameList = await UserListServices.GetUsersNameList();
            return Ok(new { code = 200, data = getUsersNameList.ToList() });
        }

        [HttpPost]
        [Route("ActiveDeactiveUsers")]
        public async Task<IActionResult> ActiveDeactiveUsers(string UserName,Guid UpdatedBy)
        {
            UserResponceModel responseModel = new UserResponceModel();

            var userName = await UserListServices.ActiveDeactiveUsers(UserName,UpdatedBy);
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
            return Ok(new { code = 200, data = getDocumentType.ToList() });
        }

        [HttpGet]
        [Route("GetDocumentList")]
        public async Task<IActionResult> GetDocumentList(Guid Userid)
        {
            IEnumerable<DocumentInfoView> getDocumentList = await UserListServices.GetDocumentList(Userid);
            return Ok(new { code = 200, data = getDocumentList.ToList() });
        }

        [HttpPost]
        [Route("UploadDocument")]
        public async Task<IActionResult> UploadDocument(DocumentInfoView UploadDocument)
        {
            var uploadDocument = await UserListServices.UploadDocument(UploadDocument);
            return Ok(new { code = 200, data = uploadDocument });
        }

        [HttpPost]
        [Route("ResetUserPassword")]
        public async Task<IActionResult> ResetUserPassword(PasswordResetView ResetPassword)
        {
            ApiResponseModel responseModel = new ApiResponseModel();

            var resetPassword = await UserListServices.ResetPassword(ResetPassword);
            try
            {

                if (resetPassword != null)
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

                if (unlockScreen.Code == 200)
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
        public async Task<IActionResult> GetUserAttendanceList(DataTableRequstModel dataTable)
        {
            var UserAttendanceList = await UserAttendance.GetUserAttendanceList(dataTable);
            return Ok(new { code = 200, data = UserAttendanceList });
        }

        [HttpGet]
        [Route("UserEdit")]
        public async Task<IActionResult> UserEdit()
        {
            IEnumerable<EmpDetailsView> userEdit = await UserListServices.UserEdit();
            return Ok(new { code = 200, data = userEdit.ToList() });
        }

        [HttpPost]
        [Route("UpdateUserOutTime")]
        public async Task<IActionResult> UpdateUserOutTime(UserAttendanceModel UpdateOutTime)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var updateUserOutTime = UserAttendance.UpdateUserOutTime(UpdateOutTime);
                if (updateUserOutTime.Result.Code == 200)
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
            return Ok(new { code = 200, data = getUserAttendance });
        }

        [HttpGet]
        [Route("GetEmployeeById")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            var userProfile = await UserListServices.GetById(id);
            return Ok(new { code = 200, data = userProfile });
        }

        [HttpPost]
        [Route("UpdateUserDetails")]
        public async Task<IActionResult> UpdateUserDetails(UserEditViewModel UpdateUser)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var updateUser = await UserListServices.UpdateUserDetails(UpdateUser);
                if (updateUser.Code == 200)
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
        [Route("GetAttendanceList")]
        public async Task<IActionResult> GetAttendanceList(SearchAttendanceModel GetAttendanceList)
        {
            IEnumerable<UserAttendanceModel> getAttendanceList = await UserAttendance.GetAttendanceList(GetAttendanceList);
            return Ok(new { code = 200, data = getAttendanceList.ToList() }); ;
        }

        [HttpPost]
        [Route("GetMemberList")]
        public async Task<IActionResult> GetMemberList()
        {
            var userList = await UserListServices.GetUsersDetails();
            return Ok(new { code = 200, data = userList });
        }

        [HttpPost]
        [Route("GetSearchAttendanceList")]
        public async Task<IActionResult> GetSearchAttendanceList(searchAttendanceListModel AttendanceList)
        {
            var Attendancelist = await UserAttendance.GetSearchAttendanceList(AttendanceList);
            return Ok(new { code = 200, data = Attendancelist });
        }

        [HttpPost]
        [Route("GetSearchEmplList")]
        public async Task<IActionResult> GetSearchEmpList(EmpDetailsModel EmpList)
        {
            var EmployeeList = await UserListServices.GetSearchEmpList(EmpList);
            return Ok(new { code = 200, data = EmployeeList });
        }
    }
}
