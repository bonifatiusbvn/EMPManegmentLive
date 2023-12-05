using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.EntityModels.ViewModels.UserModels;
using EMPManegment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace EMPManegment.Web.Controllers
{
    public class TaskController : Controller
    {
        public TaskController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices, UserSession userSession)
        {
            WebAPI = webAPI;
            Environment = environment;
            APIServices = aPIServices;
            _userSession = userSession;
        }

        public UserSession _userSession { get; }
        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UserTasks()
        {
            try
            {
                Guid Userid =_userSession.UserId;
                HttpClient client = WebAPI.Initil();
                List<TaskDetailsView> TaskList = new List<TaskDetailsView>();
                ApiResponseModel response = await APIServices.PostAsync("", "UserHome/GetTaskDetails?Taskid=" + Userid);
                if (response.code == 200)
                {
                    TaskList = JsonConvert.DeserializeObject<List<TaskDetailsView>>(response.data.ToString());

                }
                return View(TaskList);
            }
            catch (Exception ex)
            {
                throw ex;
            }

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
                ApiResponseModel res = await APIServices.GetAsync("", "UserProfile/GetUsersNameList");
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
               
                TaskDetailsView responceModel = new TaskDetailsView
                {
                    UserId = _userSession.UserId,
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
        public IActionResult AllTaskDetails()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetAllTaskDetailList()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                var dataTable = new DataTableRequstModel
                {
                    draw = draw,
                    start = start,
                    pageSize = pageSize,
                    skip = skip,
                    lenght = length,
                    searchValue = searchValue,
                    sortColumn = sortColumn,
                    sortColumnDir = sortColumnDir
                };
                List<TaskDetailsView> TaskList = new List<TaskDetailsView>();
                var data = new jsonData();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel postuser = await APIServices.PostAsync(dataTable, "UserHome/GetAllUserTaskDetails");
                if (postuser.data != null)
                {
                    data = JsonConvert.DeserializeObject<jsonData>(postuser.data.ToString());
                    TaskList = JsonConvert.DeserializeObject<List<TaskDetailsView>>(data.data.ToString());
                }
               
                else
                {
                    TaskList = new List<TaskDetailsView>();
                    ViewBag.Error = "not found";
                }
                var jsonData = new
                {
                    draw = data.draw,
                    recordsFiltered = data.recordsFiltered,
                    recordsTotal = data.recordsTotal,
                    data = TaskList,
                };
                return new JsonResult(jsonData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserTaskDetails(TaskDetailsView updateTaskDetails)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    ApiResponseModel postuser = await APIServices.PostAsync(updateTaskDetails, "UserHome/UpdateTaskStatus");
                    if (postuser.code == 200)
                    {
                        return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
                    }
                    else
                    {
                        return new JsonResult(new { Message = string.Format(postuser.message), Code = postuser.code });
                    }
                }
                else
                {
                    return View(updateTaskDetails);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
