using Azure;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Chat;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.EntityModels.ViewModels.Weather;
using EMPManegment.Inretface.Interface.ExpenseMaster;
using EMPManegment.Inretface.Interface.UserAttendance;
using EMPManegment.Inretface.Services.Home;
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
using System.Threading.Tasks;
#nullable disable
namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserHomeController : ControllerBase
    {
        public UserHomeController(IUserDetailsServices userDetails, IUserAttendanceServices attendanceServices, ITaskServices taskServices, IUserHomeServices homeServices)
        {
            UserDetails = userDetails;
            AttendanceServices = attendanceServices;
            TaskServices = taskServices;
            HomeServices = homeServices;
        }

        public IUserDetailsServices UserDetails { get; }
        public IUserAttendanceServices AttendanceServices { get; }
        public ITaskServices TaskServices { get; }
        public IUserHomeServices HomeServices { get; }

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

        [HttpGet]
        [Route("GetChatMembers")]
        public async Task<IActionResult> GetChatMembers(Guid UserId)
        {
            IEnumerable<ChatMessagesView> ChatMembers = await HomeServices.GetMyConversationList(UserId);
            return Ok(new { code = (int)HttpStatusCode.OK, data = ChatMembers.ToList() });
        }

        [HttpGet]
        [Route("GetChatConversation")]
        public async Task<IActionResult> GetChatConversation(Guid Id)
        {
            IEnumerable<ChatMessagesView> conversation = await HomeServices.GetMyConversation(Id);
            return Ok(new { code = (int)HttpStatusCode.OK, data = conversation.ToList() });
        }

        [HttpPost]
        [Route("SendMessageAsync")]
        public async Task<IActionResult> SendMessageAsync(ChatMessagesView ChatMessage)
        {
            UserResponceModel responseModel = new UserResponceModel();
            var SendMessage = HomeServices.SendMessageAsync(ChatMessage);
            try
            {
                if (SendMessage != null)
                {
                    responseModel.Code = (int)HttpStatusCode.OK;
                    responseModel.Message = SendMessage.Result.Message;
                }
                else
                {
                    responseModel.Message = SendMessage.Result.Message;
                    responseModel.Code = SendMessage.Result.Code;
                }
            }
            catch (Exception)
            {
                responseModel.Code = (int)HttpStatusCode.InternalServerError;
            }
            return StatusCode(responseModel.Code, responseModel);
        }

        [HttpPost]
        [Route("ReadMessageAsync")]
        public async Task<IActionResult> ReadMessageAsync(ChatMessagesView ChatMessage)
        {
            UserResponceModel responseModel = new UserResponceModel();
            try
            {
                var SendMessage = HomeServices.MarkMessagesAsReadAsync(ChatMessage);
                if (SendMessage != null)
                {
                    responseModel.Code = (int)HttpStatusCode.OK;
                }
                else
                {
                    responseModel.Message = SendMessage.Result.Message;
                    responseModel.Code = SendMessage.Result.Code;
                }
            }
            catch (Exception)
            {
                responseModel.Code = (int)HttpStatusCode.InternalServerError;
            }

            return StatusCode(responseModel.Code, responseModel);
        }


        [HttpPost]
        [Route("CheckUserConversationId")]
        public async Task<IActionResult> CheckUserConversationId(NewChatMessageModel newChatMessage)
        {
            IEnumerable<ChatMessagesView> conversation = await HomeServices.CheckUserConversationId(newChatMessage);
            return Ok(new { code = (int)HttpStatusCode.OK, data = conversation.ToList() });
        }

        [HttpGet]
        [Route("GetUsersNewMessageList")]
        public async Task<IActionResult> GetUsersNewMessageList(Guid userId)
        {
            IEnumerable<ChatMessagesView> messages = await HomeServices.GetUsersNewMessageList(userId);
            return Ok(new { code = (int)HttpStatusCode.OK, data = messages.ToList() });
        }

        [HttpGet]
        [Route("GetUsersAllNotificationList")]
        public async Task<IActionResult> GetUsersAllNotificationList(Guid userId)
        {
            var allNotification = await HomeServices.GetUsersAllNotificationList(userId);
            return Ok(new { code = 200, data = allNotification });
        }

        [HttpPost]
        [Route("DeleteChatMessage")]
        public async Task<IActionResult> DeleteChatMessage(int MessageId)
        {
            UserResponceModel responseModel = new UserResponceModel();
            try
            {
                var MessageDetails = HomeServices.DeleteChatMessage(MessageId);
                if (MessageDetails != null)
                {
                    responseModel.Code = (int)HttpStatusCode.OK;
                    responseModel.Message = MessageDetails.Result.Message;
                    responseModel.Data = MessageDetails.Result.Data;
                }
                else
                {
                    responseModel.Message = MessageDetails.Result.Message;
                    responseModel.Code = MessageDetails.Result.Code;
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
