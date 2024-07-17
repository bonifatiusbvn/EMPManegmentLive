using Azure;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.EntityModels.ViewModels.Weather;
using EMPManegment.Inretface.Interface.ExpenseMaster;
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
using Newtonsoft.Json;
using System.Net;
#nullable disable
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

                if (user.Code != (int)HttpStatusCode.NotFound && user.Code != (int)HttpStatusCode.InternalServerError)
                {
                    userresponseModel.Code = (int)HttpStatusCode.OK;
                    userresponseModel.Message = user.Message;
                    userresponseModel.Icone = user.Icone;
                }
                else
                {
                    userresponseModel.Message = user.Message;
                    userresponseModel.Code = user.Code;
                    userresponseModel.Icone = user.Icone;
                }
            }
            catch (Exception ex)
            {
                userresponseModel.Code = (int)HttpStatusCode.InternalServerError;
                userresponseModel.Message = "An error occurred while processing the request.";
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
                if (user.Code != (int)HttpStatusCode.NotFound && user.Code != (int)HttpStatusCode.InternalServerError)
                {
                    userresponseModel.Code = (int)HttpStatusCode.OK;
                    userresponseModel.Message = user.Message;
                    userresponseModel.Icone = user.Icone;
                }
                else
                {
                    userresponseModel.Message = user.Message;
                    userresponseModel.Code = user.Code;
                    userresponseModel.Icone = user.Icone;
                }
            }
            catch (Exception ex)
            {
                userresponseModel.Code = (int)HttpStatusCode.InternalServerError;
                userresponseModel.Message = "An error occurred while processing the request.";
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
                    userresponseModel.Message = user.Message;
                }
            }
            catch (Exception ex)
            {
                userresponseModel.Code = (int)HttpStatusCode.InternalServerError;
                userresponseModel.Message = "An error occurred while processing the request.";
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

                if (user.Code != (int)HttpStatusCode.NotFound && user.Code != (int)HttpStatusCode.InternalServerError)
                {
                    responsemodel.Message = user.Message;
                    responsemodel.Code = (int)HttpStatusCode.OK;
                }
                else
                {
                    responsemodel.Code = user.Code;
                }
            }
            catch (Exception ex)
            {
                responsemodel.Code = (int)HttpStatusCode.InternalServerError;
                responsemodel.Message = "An error occurred while processing the request.";
            }
            return StatusCode(responsemodel.Code, responsemodel);
        }

        [HttpGet]
        [Route("GetTaskType")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTaskType()
        {
            IEnumerable<TaskTypeView> getTask = await TaskServices.GetTaskType();
            return Ok(new { code = (int)HttpStatusCode.OK, data = getTask.ToList() });
        }

        [HttpPost]
        [Route("AddTaskDetails")]
        public async Task<IActionResult> AddTaskDetails(TaskDetailsView AddtaskDetails)
        {
            UserResponceModel userresponsemodel = new UserResponceModel();
            try
            {
                var AddTask = TaskServices.AddTaskDetails(AddtaskDetails);
                if (AddTask.Result.Code != (int)HttpStatusCode.InternalServerError)
                {
                    userresponsemodel.Code = (int)HttpStatusCode.OK;
                    userresponsemodel.Message = AddTask.Result.Message;
                }
                else
                {
                    userresponsemodel.Message = AddTask.Result.Message;
                    userresponsemodel.Code = AddTask.Result.Code;
                }
            }
            catch (Exception ex)
            {
                userresponsemodel.Code = (int)HttpStatusCode.InternalServerError;
                userresponsemodel.Message = "An error occurred while processing the request.";
            }
            return StatusCode(userresponsemodel.Code, userresponsemodel);
        }

        [HttpPost]
        [Route("GetUserTaskDetails")]
        public async Task<IActionResult> GetUserTaskDetails(TaskDetailsView GetTask)
        {
            List<TaskDetailsView> usertaskList = await TaskServices.GetUserTaskDetails(GetTask);
            return Ok(new { code = (int)HttpStatusCode.OK, data = usertaskList.ToList() });
        }

        [HttpPost]
        [Route("UpdateTaskStatus")]
        public async Task<IActionResult> UpdateTaskStatus(TaskDetailsView task)
        {
            UserResponceModel updateresponsemodel = new UserResponceModel();
            try
            {
                var UpdateTask = TaskServices.UpdateTaskStatus(task);
                if (UpdateTask.Result.Code != (int)HttpStatusCode.Unauthorized && UpdateTask.Result.Code != (int)HttpStatusCode.InternalServerError)
                {
                    updateresponsemodel.Code = (int)HttpStatusCode.OK;
                    updateresponsemodel.Message = UpdateTask.Result.Message;
                }
                else
                {
                    updateresponsemodel.Message = UpdateTask.Result.Message;
                    updateresponsemodel.Code = UpdateTask.Result.Code;
                }
            }
            catch (Exception ex)
            {
                updateresponsemodel.Code = (int)HttpStatusCode.InternalServerError;
                updateresponsemodel.Message = "An error occurred while processing the request.";
            }
            return StatusCode(updateresponsemodel.Code, updateresponsemodel);
        }

        [HttpGet]
        [Route("GetTaskDetailsById")]
        public async Task<IActionResult> GetTaskDetailsById(Guid Taskid)
        {
            var userTaskDetails = await TaskServices.GetTaskDetailsById(Taskid);
            return Ok(new { code = (int)HttpStatusCode.OK, data = userTaskDetails });
        }

        [HttpGet]
        [Route("GetAllUserTaskDetails")]
        public async Task<IActionResult> GetAllUserTaskDetails()
        {
            IEnumerable<TaskDetailsView> AllTaskList = await TaskServices.GetAllUserTaskDetails();
            return Ok(new { code = (int)HttpStatusCode.OK, data = AllTaskList.ToList() });
        }

        [HttpPost]
        [Route("GetAllTaskList")]
        public async Task<IActionResult> GetAllTaskList(DataTableRequstModel dataTable)
        {
            var AllTaskList = await TaskServices.GetAllTaskList(dataTable);
            return Ok(new { code = (int)HttpStatusCode.OK, data = AllTaskList });
        }

        [HttpPost]
        [Route("GetTaskDetails")]
        public async Task<IActionResult> GetTaskDetails(Guid Taskid, Guid ProjectId)
        {
            IEnumerable<TaskDetailsView> userTaskDetails = await TaskServices.GetTaskDetails(Taskid, ProjectId);
            return Ok(new { code = (int)HttpStatusCode.OK, data = userTaskDetails.ToList() });
        }

        [HttpGet]
        [Route("GetUserTotalTask")]
        public async Task<IActionResult> GetUserTotalTask(Guid UserId)
        {
            IEnumerable<TaskDetailsView> UserTotalTask = await TaskServices.GetUserTotalTask(UserId);
            return Ok(new { code = (int)HttpStatusCode.OK, data = UserTotalTask.ToList() });
        }

        [HttpPost]
        [Route("UpdateTaskDetails")]
        public async Task<IActionResult> UpdateTaskDetails(TaskDetailsView task)
        {
            UserResponceModel updateresponsemodel = new UserResponceModel();
            try
            {
                var UpdateTask = TaskServices.UpdateTaskDetails(task);
                if (UpdateTask.Result.Code != (int)HttpStatusCode.NotFound && UpdateTask.Result.Code != (int)HttpStatusCode.InternalServerError)
                {
                    updateresponsemodel.Code = (int)HttpStatusCode.OK;
                    updateresponsemodel.Message = UpdateTask.Result.Message;
                    updateresponsemodel.Icone = UpdateTask.Result.Icone;
                }
                else
                {
                    updateresponsemodel.Message = UpdateTask.Result.Message;
                    updateresponsemodel.Code = UpdateTask.Result.Code;
                }
            }
            catch (Exception ex)
            {
                updateresponsemodel.Code = (int)HttpStatusCode.InternalServerError;
                updateresponsemodel.Message = "An error occurred while processing the request.";
            }
            return StatusCode(updateresponsemodel.Code, updateresponsemodel);
        }

        [HttpGet]
        [Route("ProjectActivity")]
        public async Task<IActionResult> ProjectActivity(Guid ProId)
        {
            IEnumerable<TaskDetailsView> UserTask = await TaskServices.ProjectActivity(ProId);
            return Ok(new { code = (int)HttpStatusCode.OK, data = UserTask.ToList() });
        }

        [HttpGet]
        [Route("ProjectActivityByUserId")]
        public async Task<IActionResult> ProjectActivityByUserId(Guid UserId)
        {
            IEnumerable<TaskDetailsView> UserTask = await TaskServices.ProjectActivityByUserId(UserId);
            return Ok(new { code = (int)HttpStatusCode.OK, data = UserTask.ToList() });
        }

        [HttpGet]
        [Route("GetWeatherinfo")]
        public async Task<IActionResult> GetWeatherinfo(string city)
        {


            ApiResponseModel responsemodel = new ApiResponseModel();
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://weatherapi-com.p.rapidapi.com/current.json?q=" + city),
                Headers =
                {
                 {"x-rapidapi-key", "f12a23d41bmsh3d3c167b925d673p1e2fbcjsn09b999dee74d" },
                 {"x-rapidapi-host", "weatherapi-com.p.rapidapi.com" },
                },
            };

            try
            {
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var getweather = JsonConvert.DeserializeObject<Root>(responseContent);

                    if (getweather != null)
                    {
                        responsemodel.data = getweather;
                        return Ok(responsemodel);
                    }
                    else
                    {
                        return Ok(new { message = "No weather data found." });
                    }
                }
            }
            catch (HttpRequestException httpRequestException)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Error fetching weather data.", detail = httpRequestException.Message });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }
        [HttpPost]
        [Route("DeleteTask")]
        public async Task<IActionResult> DeleteTask(Guid Id)
        {
            UserResponceModel responseModel = new UserResponceModel();
            var GetTaskdata = await TaskServices.DeleteTask(Id);
            try
            {
                if (GetTaskdata != null)
                {
                    responseModel.Code = (int)HttpStatusCode.OK;
                    responseModel.Message = GetTaskdata.Message;
                }
                else
                {
                    responseModel.Message = GetTaskdata.Message;
                    responseModel.Code = GetTaskdata.Code;
                }
            }
            catch (Exception)
            {
                responseModel.Code = (int)HttpStatusCode.InternalServerError;
            }
            return StatusCode(responseModel.Code, responseModel);
        }
    }
}
