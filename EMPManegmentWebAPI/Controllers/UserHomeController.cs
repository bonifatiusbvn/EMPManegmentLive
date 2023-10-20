﻿using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.Inretface.Services.TaskServices;
using EMPManegment.Inretface.Services.UserAttendanceServices;
using EMPManegment.Inretface.Services.UserListServices;
using EMPManegment.Repository.UserAttendanceRepository;
using EMPManegment.Services.UserAttendance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserHomeController : ControllerBase
    {
        public UserHomeController(IUserDetailsServices userDetails,IUserAttendanceServices attendanceServices,ITaskServices taskServices) 
        {
            UserDetails = userDetails;
            AttendanceServices = attendanceServices;
            TaskServices = taskServices;
        }

        public IUserDetailsServices UserDetails { get; }
        public IUserAttendanceServices AttendanceServices { get; }
        public ITaskServices TaskServices { get; }

        [HttpPost]
        [Route("InsertINTime")]

        public async Task<IActionResult> InsertINTime(UserAttendanceModel userAttendance)
        {
            UserResponceModel responseModel = new UserResponceModel();

            var user = await UserDetails.EnterInTime(userAttendance);
            try
            {

                if (user != null)
                {

                    responseModel.Code = (int)HttpStatusCode.OK;
                    responseModel.Message = user.Message;
                    responseModel.Icone = user.Icone;
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

        [HttpPost]
        [Route("InsertOutTime")]

        public async Task<IActionResult> InsertOutTime(UserAttendanceModel userAttendance)
        {
            UserResponceModel responseModel = new UserResponceModel();

            var user = await UserDetails.EnterOutTime(userAttendance);
            try
            {

                if (user != null)
                {

                    responseModel.Code = (int)HttpStatusCode.OK;
                    responseModel.Message = user.Message;
                    responseModel.Icone = user.Icone;

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



        [HttpPost]
        [Route("GetUserAttendanceInTime")]

        public async Task<IActionResult> GetUserAttendanceInTime(UserAttendanceRequestModel userAttendance)
        {
            UserAttendanceResponseModel responseModel = new UserAttendanceResponseModel();

            var user = await AttendanceServices.GetUserAttendanceInTime(userAttendance);
            try
            {

                if (user.Data != null)
                {
                    responseModel.Data = user.Data;
                    responseModel.Code = (int)HttpStatusCode.OK;
                   
                }
                else
                {
                    responseModel.Code = user.Code;
                    
                   
                }
            }
            catch (Exception ex)
            {
                responseModel.Code = (int)HttpStatusCode.InternalServerError;
            }
            return StatusCode(responseModel.Code, responseModel);
        }

        [HttpGet]
        [Route("UserBirsthDayWish")]

        public async Task<IActionResult> UserBirsthDayWish(Guid id)
        {
            UserResponceModel response = new UserResponceModel();

            var user = await UserDetails.UserBirsthDayWish(id);
            try
            {

                if (user.Code == 200)
                {
                    response.Message = user.Message;
                    response.Code = (int)HttpStatusCode.OK;

                }
                else
                {
                    response.Code = (int)HttpStatusCode.OK;

                }

            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
            }
            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Route("GetTaskType")]
        public async Task<IActionResult> GetTaskType()
        {
            IEnumerable<TaskTypeView> taskDeals = await TaskServices.GetTaskType();
            return Ok(new { code = 200, data = taskDeals.ToList() });
        }
    }
}
