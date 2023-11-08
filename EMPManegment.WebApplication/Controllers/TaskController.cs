using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EMPManegment.Web.Controllers
{
    public class TaskController : Controller
    {
        public TaskController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices)
        {
            WebAPI = webAPI;
            Environment = environment;
            APIServices = aPIServices;
        }

        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserTasks()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetTaskType()
        {
            try
            {
                List<TaskTypeView> getTask = new List<TaskTypeView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel res = await APIServices.GetAsync(null, "UserHome/GetTaskType");
                if (res.code == 200)
                {
                    getTask = JsonConvert.DeserializeObject<List<TaskTypeView>>(res.data.ToString());
                }
                return new JsonResult(getTask);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTaskDetails(TaskDetailsView task)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync(task, "UserHome/AddTaskDetails");
                UserResponceModel responseModel = new UserResponceModel();
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message });
                }
                else
                {

                    return Ok(new { postuser.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetUserName()
        {
            try
            {
                List<EmpDetailsView> userList = new List<EmpDetailsView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel res = await APIServices.GetAsync("", "UserDetails/GetUsersNameList");
                if (res.code == 200)
                {
                    userList = JsonConvert.DeserializeObject<List<EmpDetailsView>>(res.data.ToString());
                }
                return new JsonResult(userList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetUserTaskDetail()
        {
            try
            {
                string Userid = HttpContext.Session.GetString("UserID");
                TaskDetailsView responceModel = new TaskDetailsView
                {
                    UserId = Guid.Parse(Userid),
                };
                List<TaskDetailsView> TaskList = new List<TaskDetailsView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel postuser = await APIServices.PostAsync(responceModel, "UserHome/GetUserTaskDetails");
                if (postuser.data != null)
                {
                    TaskList = JsonConvert.DeserializeObject<List<TaskDetailsView>>(postuser.data.ToString());

                }
                else
                {
                    TaskList = new List<TaskDetailsView>();
                    ViewBag.Error = "note found";
                }
                return PartialView("~/Views/Task/_UserTaskList.cshtml", TaskList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserTaskStatus()
        {
            try
            {
                var userstausres = HttpContext.Request.Form["STATUSUPDATE"];

                var statusRequest = JsonConvert.DeserializeObject<TaskDetailsView>(userstausres);

                ApiResponseModel postuser = await APIServices.PostAsync(statusRequest, "UserHome/UpdateTaskStatus");
                UserResponceModel userResponceModel = new UserResponceModel();
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message });
                }
                else
                {
                    return Ok(new { postuser.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetTaskDetailsById(Guid Id)
        {
            try
            {
                TaskDetailsView usertaskdetails = new TaskDetailsView();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel response = await APIServices.GetAsync("", "UserHome/GetTaskDetailsById?Taskid=" + Id);
                if (response.code == 200)
                {
                    usertaskdetails = JsonConvert.DeserializeObject<TaskDetailsView>(response.data.ToString());
                }
                return new JsonResult(usertaskdetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserTaskDetail()
        {
            try
            {
                List<TaskDetailsView> TaskList = new List<TaskDetailsView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel postuser = await APIServices.GetAsync("", "UserHome/GetAllUserTaskDetails");
                if (postuser.data != null)
                {
                    TaskList = JsonConvert.DeserializeObject<List<TaskDetailsView>>(postuser.data.ToString());
                }
                else
                {
                    TaskList = new List<TaskDetailsView>();
                    ViewBag.Error = "not found";
                }
                return PartialView("~/Views/Task/_UserTaskList.cshtml", TaskList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
