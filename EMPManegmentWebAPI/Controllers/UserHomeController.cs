using Azure;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.Inretface.Interface.UserAttendance;
using EMPManegment.Inretface.Services.TaskServices;
using EMPManegment.Inretface.Services.UserAttendanceServices;
using EMPManegment.Inretface.Services.UserListServices;
using EMPManegment.Repository.UserAttendanceRepository;
using EMPManegment.Services.UserAttendance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.Net;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserHomeController : ControllerBase
    {
        public UserHomeController(IUserDetailsServices userDetails, IUserAttendanceServices attendanceServices, ITaskServices taskServices)
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
        public async Task<IActionResult> InsertINTime(UserAttendanceModel InsertInTime)
        {
            UserResponceModel userresponseModel = new UserResponceModel();

            var user = await UserDetails.EnterInTime(InsertInTime);
            try
            {

                if (user != null)
                {

                    userresponseModel.Code = (int)HttpStatusCode.OK;
                    userresponseModel.Message = user.Message;
                    userresponseModel.Icone = user.Icone;
                }
                else
                {
                    userresponseModel.Message = user.Message;
                    userresponseModel.Code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                userresponseModel.Code = (int)HttpStatusCode.InternalServerError;
            }
            return StatusCode(userresponseModel.Code, userresponseModel);
        }

        [HttpPost]
        [Route("InsertOutTime")]
        public async Task<IActionResult> InsertOutTime(UserAttendanceModel InsertOutTime)
        {
            UserResponceModel userresponseModel = new UserResponceModel();

            var user = await UserDetails.EnterOutTime(InsertOutTime);
            try
            {
                if (user != null)
                {
                    userresponseModel.Code = (int)HttpStatusCode.OK;
                    userresponseModel.Message = user.Message;
                    userresponseModel.Icone = user.Icone;
                }
                else
                {
                    userresponseModel.Message = user.Message;
                    userresponseModel.Code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                userresponseModel.Code = (int)HttpStatusCode.InternalServerError;
            }
            return StatusCode(userresponseModel.Code, userresponseModel);
        }

        [HttpPost]
        [Route("GetUserAttendanceInTime")]
        public async Task<IActionResult> GetUserAttendanceInTime(UserAttendanceRequestModel GetAttendance)
        {
            UserAttendanceResponseModel userresponseModel = new UserAttendanceResponseModel();

            var user = await AttendanceServices.GetUserAttendanceInTime(GetAttendance);
            try
            {

                if (user.Data != null)
                {
                    userresponseModel.Data = user.Data;
                    userresponseModel.Code = (int)HttpStatusCode.OK;

                }
                else
                {
                    userresponseModel.Code = user.Code;


                }
            }
            catch (Exception ex)
            {
                userresponseModel.Code = (int)HttpStatusCode.InternalServerError;
            }
            return StatusCode(userresponseModel.Code, userresponseModel);
        }

        [HttpGet]
        [Route("UserBirsthDayWish")]
        public async Task<IActionResult> UserBirsthDayWish(Guid UserId)
        {
            UserResponceModel responsemodel = new UserResponceModel();

            var user = await UserDetails.UserBirsthDayWish(UserId);
            try
            {

                if (user.Code == 200)
                {
                    responsemodel.Message = user.Message;
                    responsemodel.Code = (int)HttpStatusCode.OK;

                }
                else
                {
                    responsemodel.Code = (int)HttpStatusCode.OK;

                }

            }
            catch (Exception ex)
            {
                responsemodel.Code = (int)HttpStatusCode.InternalServerError;
            }
            return StatusCode(responsemodel.Code, responsemodel);
        }

        [HttpGet]
        [Route("GetTaskType")]
        public async Task<IActionResult> GetTaskType()
        {
            IEnumerable<TaskTypeView> getTask = await TaskServices.GetTaskType();
            return Ok(new { code = 200, data = getTask.ToList() });
        }

        [HttpPost]
        [Route("AddTaskDetails")]
        public async Task<IActionResult> AddTaskDetails(TaskDetailsView AddtaskDetails)
        {
            UserResponceModel userresponsemodel = new UserResponceModel();
            try
            {
                var AddTask = TaskServices.AddTaskDetails(AddtaskDetails);
                if (AddTask.Result.Code == 200)
                {
                    userresponsemodel.Code = (int)HttpStatusCode.OK;
                    userresponsemodel.Message = AddTask.Result.Message;
                }
                else
                {
                    userresponsemodel.Message = AddTask.Result.Message;
                    userresponsemodel.Code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StatusCode(userresponsemodel.Code, userresponsemodel);
        }

        [HttpPost]
        [Route("GetUserTaskDetails")]
        public async Task<IActionResult> GetUserTaskDetails(TaskDetailsView GetTask)
        {
            List<TaskDetailsView> usertaskList = await TaskServices.GetUserTaskDetails(GetTask);
            return Ok(new { code = 200, data = usertaskList.ToList() });
        }

        [HttpPost]
        [Route("UpdateTaskStatus")]
        public async Task<IActionResult> UpdateTaskStatus(TaskDetailsView task)
        {
            UserResponceModel updateresponsemodel = new UserResponceModel();
            try
            {
                var UpdateTask = TaskServices.UpdateTaskStatus(task);
                if (UpdateTask.Result.Code == 200)
                {
                    updateresponsemodel.Code = (int)HttpStatusCode.OK;
                    updateresponsemodel.Message = UpdateTask.Result.Message;
                    updateresponsemodel.Icone = UpdateTask.Result.Icone;
                }
                else
                {
                    updateresponsemodel.Message = UpdateTask.Result.Message;
                    updateresponsemodel.Code = (int)HttpStatusCode.NotFound;
                    updateresponsemodel.Icone = UpdateTask.Result.Icone;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StatusCode(updateresponsemodel.Code, updateresponsemodel);
        }
        [HttpGet]
        [Route("GetTaskDetailsById")]
        public async Task<IActionResult> GetTaskDetailsById(Guid Taskid)
        {
            var userTaskDetails = await TaskServices.GetTaskDetailsById(Taskid);
            return Ok(new { code = 200, data = userTaskDetails });
        }

        [HttpGet]
        [Route("GetAllUserTaskDetails")]
        public async Task<IActionResult> GetAllUserTaskDetails()
        {
            IEnumerable<TaskDetailsView> AllTaskList = await TaskServices.GetAllUserTaskDetails();
            return Ok(new { code = 200, data = AllTaskList.ToList() });
        }
        [HttpPost]
        [Route("TaskDetailsDataTable")]
        public async Task<IActionResult> TaskDetailsDataTable(DataTableRequstModel dataTable)
        {
            var AllTaskList = await TaskServices.GetAllUserTaskDetails(dataTable);
            return Ok(new { code = 200, data = AllTaskList });
        }

        [HttpPost]
        [Route("GetTaskDetails")]
        public async Task<IActionResult> GetTaskDetails(Guid Taskid, Guid ProjectId)
        {
            IEnumerable<TaskDetailsView> userTaskDetails = await TaskServices.GetTaskDetails(Taskid, ProjectId);
            return Ok(new { code = 200, data = userTaskDetails.ToList() });
        }

        [HttpGet]
        [Route("GetUserTotalTask")]
        public async Task<IActionResult> GetUserTotalTask(Guid UserId)
        {
            IEnumerable<TaskDetailsView> UserTotalTask = await TaskServices.GetUserTotalTask(UserId);
            return Ok(new { code = 200, data = UserTotalTask.ToList() });
        }
        [HttpPost]
        [Route("UpdateTaskDetails")]
        public async Task<IActionResult> UpdateTaskDetails(TaskDetailsView task)
        {
            UserResponceModel updateresponsemodel = new UserResponceModel();
            try
            {
                var UpdateTask = TaskServices.UpdateTaskDetails(task);
                if (UpdateTask.Result.Code == 200)
                {
                    updateresponsemodel.Code = (int)HttpStatusCode.OK;
                    updateresponsemodel.Message = UpdateTask.Result.Message;
                    updateresponsemodel.Icone = UpdateTask.Result.Icone;
                }
                else
                {
                    updateresponsemodel.Message = UpdateTask.Result.Message;
                    updateresponsemodel.Code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StatusCode(updateresponsemodel.Code, updateresponsemodel);
        }

        [HttpGet]
        [Route("ProjectActivity")]
        public async Task<IActionResult> ProjectActivity(Guid ProId)
        {
            IEnumerable<TaskDetailsView> UserTask = await TaskServices.ProjectActivity(ProId);
            return Ok(new { code = 200, data = UserTask.ToList() });
        }
    }
}
