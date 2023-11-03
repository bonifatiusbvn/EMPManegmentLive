﻿using Azure;
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
using NuGet.Protocol.Core.Types;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : ControllerBase
    {
        public IUserDetailsServices UserListServices { get; }
        public IUserAttendanceServices UserAttendance { get; }

        public UserDetailsController(IUserDetailsServices userListServices,IUserAttendanceServices userAttendance)
        {
            UserListServices = userListServices;
            UserAttendance = userAttendance;
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
            var userNameList = await UserListServices.GetUsersNameList();
            return Ok(new { code = 200, data = userNameList.ToList() });
        }




        [HttpPost]
        [Route("ActiveDeactiveUsers")]

        public async Task<IActionResult> ActiveDeactiveUsers(string UserName)
        {
            UserResponceModel responseModel = new UserResponceModel();

            var user = await UserListServices.ActiveDeactiveUsers(UserName);
            try
            {

                if (user != null)
                {

                    responseModel.Code = (int)HttpStatusCode.OK;
                    responseModel.Message = user.Message;
                }
                else
                {
                    responseModel.Message = user.Message;
                    responseModel.Code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                responseModel.Code = (int)HttpStatusCode.InternalServerError;
            }
            return StatusCode(responseModel.Code, responseModel);
        }

        [HttpGet]
        [Route("GetDocumentType")]
        public async Task<IActionResult> GetDocumentType()
        {
            IEnumerable<EmpDocumentView> dept = await UserListServices.GetDocumentType();
            return Ok(new { code = 200, data = dept.ToList() });
        }

        [HttpGet]
        [Route("GetDocumentList")]

        public async Task<IActionResult> GetDocumentList()
        {
            IEnumerable<DocumentInfoView> userList = await UserListServices.GetDocumentList();
            return Ok(new { code = 200, data = userList.ToList() });
        }

        [HttpPost]
        [Route("UploadDocument")]
        public async Task<IActionResult> UploadDocument(DocumentInfoView doc)
        {
            var document = await UserListServices.UploadDocument(doc);
            return Ok(new { code = 200, data = document });
        }
        [HttpPost]
        [Route("ResetUserPassword")]
        public async Task<IActionResult> ResetUserPassword(PasswordResetView emp)
        {
            ApiResponseModel responseModel = new ApiResponseModel();

            var user = await UserListServices.ResetPassword(emp);
            try
            {

                if (user != null)
                {

                    responseModel.code = (int)HttpStatusCode.OK;
                    responseModel.message = user.Message;
                }
                else
                {
                    responseModel.message = user.Message;
                    responseModel.code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                responseModel.code = (int)HttpStatusCode.InternalServerError;
            }
            return StatusCode(responseModel.code, responseModel);
        }

        [HttpPost]
        [Route("UnLockScreen")]
        public async Task<IActionResult> UnLockScreen(LoginRequest request)
        {
            LoginResponseModel apiResponseModel = new LoginResponseModel();
            try
            {

                var result = await UserListServices.UserLockScreen(request);

                if (result.Code == 200)
                {
                    apiResponseModel.Code = (int)HttpStatusCode.OK;
                    apiResponseModel.Message = result.Message;
                }
                else
                {
                    apiResponseModel.Message = result.Message;
                    apiResponseModel.Code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                apiResponseModel.Code = (int)HttpStatusCode.InternalServerError;
            }
            return StatusCode(apiResponseModel.Code, apiResponseModel);
        }



        [HttpPost]
        [Route("GetUserAttendanceList")]

        public async Task<IActionResult> GetUserAttendanceList(DataTableRequstModel dataTable)
        {
            var userList = await UserAttendance.GetUserAttendanceList(dataTable);
            return Ok(new { code = 200, data = userList });
        }

        [HttpGet]
        [Route("UserEdit")]
        public async Task<IActionResult> UserEdit()
        {
            IEnumerable<EmpDetailsView> userList = await UserListServices.UserEdit();
            return Ok(new { code = 200, data = userList.ToList() });
        }

        [HttpPost]
        [Route("UpdateUserOutTime")]

        public async Task<IActionResult> UpdateUserOutTime(UserAttendanceModel userAttendance)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var result = UserAttendance.UpdateUserOutTime(userAttendance);
                if (result.Result.Code == 200)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = result.Result.Message;
                    response.Icone = result.Result.Icone;   
                }
                
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Route("GetUserAttendanceById")]

        public async Task<IActionResult> GetUserAttendanceById(int attendanceId)
        {
            IEnumerable<UserAttendanceModel> attendance = await UserAttendance.GetUserAttendanceById(attendanceId);
            return Ok(new { code = 200, data = attendance });
        }
        [HttpGet]
        [Route("GetEmployee")]
        public async Task<IActionResult>GetEmployee(Guid id)
        {
            var data  = await UserListServices.GetById(id);
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(data);
            }
        }
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult>UpdateUser(UserEditViewModel employee)
        {
            UserResponceModel response = new UserResponceModel();
            var data = await UserListServices.UpdateUser(employee);
            if (data.Code == 200)
            {
                response.Code = (int)HttpStatusCode.OK;
                response.Message = data.Message;
                response.Icone = data.Icone;
            }
            return StatusCode(response.Code, response);
        }
    }
}
