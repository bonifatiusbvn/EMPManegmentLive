using Aspose.Pdf.Operators;
using DocumentFormat.OpenXml.Spreadsheet;
using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.EntityModels.ViewModels.UserModels;
using EMPManegment.Web.Helper;
using EMPManegment.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
#nullable disable
namespace EMPManegment.Web.Controllers
{
    [Authorize]
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

        [FormPermissionAttribute("Task Board-View")]
        public async Task<IActionResult> UserTasks()
        {
            try
            {
                Guid Userid = _userSession.UserId;
                string ProjectId = UserSession.ProjectId;
                List<TaskDetailsView> TaskList = new List<TaskDetailsView>();
                ApiResponseModel response = await APIServices.PostAsync("", "UserHome/GetTaskDetails?Taskid=" + Userid + "&ProjectId=" + ProjectId);
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

        [FormPermissionAttribute("Tasks List-Add")]
        [HttpPost]
        public async Task<IActionResult> AddTaskDetails(TaskImageDetailsModel task)
        {
            try
            {
                if (task.Image != null)
                {
                    var TaskImg = Guid.NewGuid() + "_" + task.Image.FileName;
                    var path = Environment.WebRootPath;
                    var filepath = "Content/TaskDocuments/" + TaskImg;
                    var fullpath = Path.Combine(path, filepath);
                    UploadTaskFile(task.Image, fullpath);
                    var TaskDetails = new TaskDetailsView
                    {
                        CreatedBy = _userSession.UserId,
                        TaskStatus = task.TaskStatus,
                        TaskType = task.TaskType,
                        TaskTypeName = task.TaskTypeName,
                        TaskDate = task.TaskDate,
                        TaskEndDate = task.TaskEndDate,
                        TaskDetails = task.TaskDetails,
                        UserId = task.UserId,
                        TaskTitle = task.TaskTitle,
                        ProjectId = task.ProjectId,
                        Document = filepath,
                    };
                    ApiResponseModel postuser = await APIServices.PostAsync(TaskDetails, "UserHome/AddTaskDetails");
                    if (postuser.code == 200)
                    {
                        return Ok(new { Message = "Task Uploaded Successfully!", Code = postuser.code });
                    }
                    else
                    {
                        return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
                    }

                }
                return BadRequest();
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
                    return Ok(new { postuser.message, postuser.code });
                }
                else
                {
                    return Ok(new { postuser.code, postuser.message });
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
        public async Task<JsonResult> GetAllUserTaskDetail()
        {
            try
            {
                List<TaskDetailsView> TaskList = new List<TaskDetailsView>();
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
                return new JsonResult(TaskList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [FormPermissionAttribute("Tasks List-View")]
        [HttpGet]
        public IActionResult AllTaskDetails()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAllTaskList()
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
                ApiResponseModel postuser = await APIServices.PostAsync(dataTable, "UserHome/GetAllTaskList");
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


        [FormPermissionAttribute("Tasks List-Edit")]
        [HttpPost]
        public async Task<IActionResult> UpdateTaskDetails(TaskDetailsView updateTaskDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApiResponseModel postuser = await APIServices.PostAsync(updateTaskDetails, "UserHome/UpdateTaskDetails");
                    if (postuser.code == 200)
                    {
                        return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
                    }
                    else
                    {
                        return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
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

        [HttpGet]
        public async Task<IActionResult> GetProjectActivity(Guid ProId)
        {
            try
            {

                List<TaskDetailsView> activity = new List<TaskDetailsView>();
                ApiResponseModel postuser = await APIServices.GetAsync("", "UserHome/ProjectActivity?ProId=" + ProId);
                if (postuser.data != null)
                {
                    activity = JsonConvert.DeserializeObject<List<TaskDetailsView>>(postuser.data.ToString());

                }
                else
                {
                    activity = new List<TaskDetailsView>();
                    ViewBag.Error = "note found";
                }
                return PartialView("~/Views/Project/_ProjectActivityPartial.cshtml", activity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> ProjectActivityByUserId()
        {
            try
            {
                Guid UserId = _userSession.UserId;
                List<TaskDetailsView> activity = new List<TaskDetailsView>();
                ApiResponseModel postuser = await APIServices.GetAsync("", "UserHome/ProjectActivityByUserId?UserId=" + UserId);
                if (postuser.data != null)
                {
                    activity = JsonConvert.DeserializeObject<List<TaskDetailsView>>(postuser.data.ToString());

                }
                else
                {
                    activity = new List<TaskDetailsView>();
                    ViewBag.Error = "note found";
                }
                return PartialView("~/Views/Project/_ProjectActivityPartial.cshtml", activity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UploadTaskFile(IFormFile file, string path)
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);
        }
        [HttpGet]
        public async Task<IActionResult> DownloadTaskDocument(string TaskDocument)
        {
            var filepath = TaskDocument;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filepath);
            if (!System.IO.File.Exists(path))
            {
                return Ok(new { Message = "File not found" });
            }

            var memory = new MemoryStream();
            await using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            var contentType = "application/pdf";
            var fileName = Path.GetFileName(path);

            var base64Memory = Convert.ToBase64String(memory.ToArray());

            return Ok(new { memory = base64Memory, contentType, fileName });
        }
        public async Task<IActionResult> DeleteTask(Guid Id)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync("", "UserHome/DeleteTask?Id=" + Id);
                if (postuser.code == 200)
                {
                    return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
                }
                else
                {
                    return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
