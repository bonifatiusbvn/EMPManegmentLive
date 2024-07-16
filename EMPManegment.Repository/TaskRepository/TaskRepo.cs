
using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.Inretface.Interface.TaskDetails;
using EMPManegment.Inretface.Interface.UserAttendance;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;
using EMPManegment.EntityModels.Common;
using Microsoft.Extensions.Configuration;
using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.UserModels;

namespace EMPManegment.Repository.TaskRepository
{
    public class TaskRepo : ITaskDetails
    {
        private readonly string? pending;

        public TaskRepo(BonifatiusEmployeesContext context, IConfiguration configuration)
        {
            Context = context;
            _configuration = configuration;
        }

        public BonifatiusEmployeesContext Context { get; }
        public IConfiguration _configuration { get; }

        public async Task<IEnumerable<TaskTypeView>> GetTaskType()
        {
            try
            {
                IEnumerable<TaskTypeView> taskDeals = Context.TblTaskMasters.ToList().Select(a => new TaskTypeView
                {
                    TaskId = a.Id,
                    TaskType = a.TaskType
                });
                return taskDeals;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserResponceModel> AddTaskDetails(TaskDetailsView addtask)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var taskImage = await Context.TblTaskDetails.FirstOrDefaultAsync(a => a.Id == addtask.Id);
                var taskmodel = new TblTaskDetail()
                {
                    Id = Guid.NewGuid(),
                    TaskType = addtask.TaskType,
                    TaskTitle = addtask.TaskTitle,
                    TaskDetails = addtask.TaskDetails,
                    TaskDate = addtask.TaskDate,
                    UserId = addtask.UserId,
                    CreatedBy = addtask.CreatedBy,
                    CreatedOn = DateTime.Now,
                    TaskEndDate = addtask.TaskEndDate,
                    ProjectId = addtask.ProjectId,
                    Document = addtask.Document,
                };
                response.Message = "Task add successfully!";
                Context.TblTaskDetails.Add(taskmodel);
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in creating task.";
            }
            return response;
        }

        public async Task<UserResponceModel> UpdateTaskStatus(TaskDetailsView updatetask)
        {
            UserResponceModel responcemodel = new UserResponceModel();
            if (updatetask.TaskStatus == "Completed")
            {
                bool gettaskCheck = Context.TblTaskDetails.Any(e => e.Id == updatetask.Id);
                bool getProjectHead = Context.TblProjectMasters.Any(e => e.ProjectHead == updatetask.ProjectHead && e.ProjectId == updatetask.ProjectId);
                try
                {
                    if (gettaskCheck == true && getProjectHead == true)
                    {
                        var taskstatusupdate = Context.TblTaskDetails.Where(e => e.Id == updatetask.Id).FirstOrDefault();
                        try
                        {
                            if (taskstatusupdate != null)
                            {
                                taskstatusupdate.TaskStatus = updatetask.TaskStatus;
                                taskstatusupdate.IsCompleted = updatetask.TaskStatus;
                                taskstatusupdate.CompletedBy = updatetask.UserId;
                                taskstatusupdate.UpdatedOn = DateTime.Now;
                                taskstatusupdate.UpdatedBy = updatetask.UpdatedBy;
                            }
                            Context.TblTaskDetails.Update(taskstatusupdate);
                            Context.SaveChanges();
                            responcemodel.Message = "Task status updated successfully!";
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        responcemodel.Code = (int)HttpStatusCode.Unauthorized;
                        responcemodel.Message = "You aren't authorize!!";
                    }
                }
                catch (Exception ex)
                {
                    responcemodel.Code = (int)HttpStatusCode.InternalServerError;
                    responcemodel.Message = "Error in updating task status.";
                }
            }
            else
            {
                var gettask = Context.TblTaskDetails.Where(e => e.Id == updatetask.Id).FirstOrDefault();
                try
                {
                    if (gettask != null)
                    {
                        gettask.TaskStatus = updatetask.TaskStatus;
                        gettask.CompletedBy = updatetask.UserId;
                        gettask.UpdatedOn = DateTime.Now;
                        gettask.UpdatedBy = updatetask.UpdatedBy;
                    }
                    Context.TblTaskDetails.Update(gettask);
                    Context.SaveChanges();
                    responcemodel.Message = "Task status updated successfully!";
                }
                catch (Exception ex)
                {
                    responcemodel.Code = (int)HttpStatusCode.InternalServerError;
                    responcemodel.Message = "Error in updating task status.";
                }
            }
            return responcemodel;
        }



        public async Task<List<TaskDetailsView>> GetUserTaskDetails(TaskDetailsView GetUserTaskDetails)
        {
            var UserId = new List<TaskDetailsView>();
            var data = await Context.TblTaskDetails.Where(x => x.UserId == GetUserTaskDetails.UserId).ToListAsync();
            if (data != null)
            {
                foreach (var item in data)
                {
                    UserId.Add(new TaskDetailsView()
                    {
                        Id = item.Id,
                        TaskType = item.TaskType,
                        TaskStatus = item.TaskStatus,
                        TaskDate = item.TaskDate,
                        TaskDetails = item.TaskDetails,
                        TaskEndDate = item.TaskEndDate,
                        TaskTitle = item.TaskTitle,
                    });
                }
            }
            return UserId;
        }

        public async Task<IEnumerable<TaskDetailsView>> GetAllUserTaskDetails()
        {
            IEnumerable<TaskDetailsView> AllTaskDetails = from a in Context.TblTaskDetails
                                                          join b in Context.TblUsers on a.UserId equals b.Id
                                                          join c in Context.TblTaskMasters on a.TaskType equals c.Id
                                                          select new TaskDetailsView
                                                          {
                                                              Id = a.Id,
                                                              TaskType = a.TaskType,
                                                              TaskStatus = a.TaskStatus,
                                                              TaskDate = a.TaskDate,
                                                              TaskDetails = a.TaskDetails,
                                                              TaskEndDate = a.TaskEndDate,
                                                              TaskTitle = a.TaskTitle,
                                                              UserProfile = b.Image,
                                                              UserName = b.UserName,
                                                              FirstName = b.FirstName,
                                                              LastName = b.LastName,
                                                              TaskTypeName = c.TaskType,
                                                              ProjectId = a.ProjectId,

                                                          };
            return AllTaskDetails;
        }

        public async Task<TaskDetailsView> GetTaskDetailsById(Guid Taskid)
        {
            TaskDetailsView taskdata = new TaskDetailsView();
            try
            {
                taskdata = (from d in Context.TblTaskDetails.Where(d => d.Id == Taskid)
                            join m in Context.TblTaskMasters
                            on d.TaskType equals m.Id
                            join b in Context.TblUsers on d.UserId equals b.Id
                            select new TaskDetailsView
                            {
                                Id = d.Id,
                                UserId = d.UserId,
                                TaskType = d.TaskType,
                                TaskDate = d.TaskDate,
                                TaskTitle = d.TaskTitle,
                                TaskEndDate = d.TaskEndDate,
                                TaskDetails = d.TaskDetails,
                                TaskStatus = d.TaskStatus,
                                UserName = b.UserName,
                                FirstName = b.FirstName,
                                LastName = b.LastName,
                                UserProfile = b.Image,
                                TaskTypeName = m.TaskType,
                                CreatedBy = d.CreatedBy
                            }).First();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return taskdata;
        }

        public async Task<IEnumerable<TaskDetailsView>> ProjectActivity(Guid ProId)
        {
            try
            {
                var activity = (from a in Context.TblTaskDetails
                                join b in Context.TblUsers on a.User.Id equals b.Id
                                join c in Context.TblTaskMasters on a.TaskType equals c.Id
                                where a.ProjectId == ProId
                                orderby a.UpdatedOn ascending
                                select new TaskDetailsView
                                {
                                    Id = a.Id,
                                    UserId = b.Id,
                                    TaskType = a.TaskType,
                                    TaskStatus = a.TaskStatus,
                                    TaskDate = a.TaskDate,
                                    TaskDetails = a.TaskDetails,
                                    TaskEndDate = a.TaskEndDate,
                                    TaskTitle = a.TaskTitle,
                                    UserProfile = b.Image,
                                    UserName = b.UserName,
                                    FirstName = b.FirstName,
                                    LastName = b.LastName,
                                    TaskTypeName = c.TaskType,
                                    CreatedBy = a.CreatedBy,
                                    UpdatedOn = a.UpdatedOn,

                                }).Take(3);

                return await activity.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<TaskDetailsView>> ProjectActivityByUserId(Guid UserId)
        {
            try
            {
                var activity = (from a in Context.TblTaskDetails
                                join b in Context.TblUsers on a.User.Id equals b.Id
                                join c in Context.TblTaskMasters on a.TaskType equals c.Id
                                where a.UserId == UserId
                                orderby a.UpdatedOn ascending
                                select new TaskDetailsView
                                {
                                    Id = a.Id,
                                    UserId = b.Id,
                                    TaskType = a.TaskType,
                                    TaskStatus = a.TaskStatus,
                                    TaskDate = a.TaskDate,
                                    TaskDetails = a.TaskDetails,
                                    TaskEndDate = a.TaskEndDate,
                                    TaskTitle = a.TaskTitle,
                                    UserProfile = b.Image,
                                    UserName = b.UserName,
                                    FirstName = b.FirstName,
                                    LastName = b.LastName,
                                    TaskTypeName = c.TaskType,
                                    CreatedBy = a.CreatedBy,
                                    UpdatedOn = a.UpdatedOn,

                                }).Take(3);

                return activity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<TaskDetailsView>> GetTaskDetails(Guid UserId, Guid ProjectId)
        {
            try
            {
                IEnumerable<TaskDetailsView> AllTaskDetails =
                    from a in Context.TblTaskDetails
                    join b in Context.TblUsers on a.UserId equals b.Id
                    join c in Context.TblTaskMasters on a.TaskType equals c.Id
                    join d in Context.TblProjectMasters on a.ProjectId equals d.ProjectId
                    where a.UserId == UserId
                    where a.ProjectId == ProjectId
                    select new TaskDetailsView
                    {
                        Id = a.Id,
                        UserId = b.Id,
                        TaskType = a.TaskType,
                        TaskStatus = a.TaskStatus,
                        TaskDate = a.TaskDate,
                        TaskDetails = a.TaskDetails,
                        TaskEndDate = a.TaskEndDate,
                        TaskTitle = a.TaskTitle,
                        UserProfile = b.Image,
                        UserName = b.UserName,
                        FirstName = b.FirstName,
                        LastName = b.LastName,
                        TaskTypeName = c.TaskType,
                        CreatedBy = a.CreatedBy,
                        ProjectHead = d.ProjectHead,
                        ProjectId = d.ProjectId,
                    };

                return AllTaskDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<jsonData> GetAllTaskList(DataTableRequstModel dataTable)
        {
            try
            {
                string dbConnectionStr = _configuration.GetConnectionString("EMPDbconn");

                var dataSet = DbHelper.GetDataSet("GetAllTaskList", CommandType.StoredProcedure, new SqlParameter[] { }, dbConnectionStr);

                var taskList = ConvertDataSetToTaskList(dataSet);

                if (!string.IsNullOrEmpty(dataTable.searchValue.ToLower()))
                {
                    taskList = taskList.Where(e =>
                        e.TaskTitle.Contains(dataTable.searchValue.ToLower(), StringComparison.OrdinalIgnoreCase) ||
                        e.TaskDetails.Contains(dataTable.searchValue.ToLower(), StringComparison.OrdinalIgnoreCase) ||
                        e.TaskStatus.Contains(dataTable.searchValue.ToLower(), StringComparison.OrdinalIgnoreCase) ||
                        e.TaskDate.ToString().Contains(dataTable.searchValue)).ToList();
                }

                IQueryable<TaskDetailsView> queryabletaskDetails = taskList.AsQueryable();

                if (!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDir))
                {
                    switch (dataTable.sortColumn.ToLower())
                    {
                        case "createdon":
                            queryabletaskDetails = dataTable.sortColumnDir == "asc" ? queryabletaskDetails.OrderBy(e => e.CreatedOn) : queryabletaskDetails.OrderByDescending(e => e.CreatedOn);
                            break;
                        case "username":
                            queryabletaskDetails = dataTable.sortColumnDir == "asc" ? queryabletaskDetails.OrderBy(e => e.UserName) : queryabletaskDetails.OrderByDescending(e => e.UserName);
                            break;
                        case "tasktitle":
                            queryabletaskDetails = dataTable.sortColumnDir == "asc" ? queryabletaskDetails.OrderBy(e => e.TaskTitle) : queryabletaskDetails.OrderByDescending(e => e.TaskTitle);
                            break;
                        case "taskdetails":
                            queryabletaskDetails = dataTable.sortColumnDir == "asc" ? queryabletaskDetails.OrderBy(e => e.TaskDetails) : queryabletaskDetails.OrderByDescending(e => e.TaskDetails);
                            break;
                        case "tasktype":
                            queryabletaskDetails = dataTable.sortColumnDir == "asc" ? queryabletaskDetails.OrderBy(e => e.TaskType) : queryabletaskDetails.OrderByDescending(e => e.TaskType);
                            break;
                        case "taskdate":
                            queryabletaskDetails = dataTable.sortColumnDir == "asc" ? queryabletaskDetails.OrderBy(e => e.TaskDate) : queryabletaskDetails.OrderByDescending(e => e.TaskDate);
                            break;
                        case "taskenddate":
                            queryabletaskDetails = dataTable.sortColumnDir == "asc" ? queryabletaskDetails.OrderBy(e => e.TaskEndDate) : queryabletaskDetails.OrderByDescending(e => e.TaskEndDate);
                            break;
                        case "taskstatus":
                            queryabletaskDetails = dataTable.sortColumnDir == "asc" ? queryabletaskDetails.OrderBy(e => e.TaskStatus) : queryabletaskDetails.OrderByDescending(e => e.TaskStatus);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    queryabletaskDetails = queryabletaskDetails.OrderByDescending(e => e.CreatedOn);
                }

                var totalRecord = queryabletaskDetails.Count();
                var filteredData = queryabletaskDetails.Skip(dataTable.skip).Take(dataTable.pageSize).ToList();

                var jsonData = new jsonData
                {
                    draw = dataTable.draw,
                    recordsFiltered = totalRecord,
                    recordsTotal = totalRecord,
                    data = filteredData
                };

                return jsonData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private List<TaskDetailsView> ConvertDataSetToTaskList(DataSet dataSet)
        {
            var taskDetails = new List<TaskDetailsView>();

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                var task = new TaskDetailsView
                {
                    Id = Guid.Parse(row["Id"].ToString()),
                    TaskType = Convert.ToInt32(row["TaskType"]),
                    TaskStatus = row["TaskStatus"].ToString(),
                    TaskDate = Convert.ToDateTime(row["TaskDate"]),
                    TaskDetails = row["TaskDetails"].ToString(),
                    TaskEndDate = Convert.ToDateTime(row["TaskEndDate"]),
                    TaskTitle = row["TaskTitle"].ToString(),
                    UserProfile = row["UserProfile"].ToString(),
                    UserName = row["UserName"].ToString(),
                    TaskTypeName = row["TaskTypeName"].ToString(),
                    Document = row["Document"].ToString()
                };
                taskDetails.Add(task);
            }

            return taskDetails;
        }

        public async Task<IEnumerable<TaskDetailsView>> GetUserTotalTask(Guid UserId)
        {
            var TaskList = new List<TaskDetailsView>();
            var TotalTask = await Context.TblTaskDetails.Where(x => x.UserId == UserId).ToListAsync();
            if (TotalTask != null)
            {
                foreach (var item in TotalTask)
                {
                    TaskList.Add(new TaskDetailsView()
                    {
                        Id = item.Id,
                        TaskType = item.TaskType,
                        TaskStatus = item.TaskStatus,
                        TaskDate = item.TaskDate,
                        TaskDetails = item.TaskDetails,
                        TaskEndDate = item.TaskEndDate,
                        TaskTitle = item.TaskTitle,
                    });
                }
            }
            return TaskList;
        }

        public async Task<UserResponceModel> UpdateTaskDetails(TaskDetailsView updatetask)
        {
            UserResponceModel model = new UserResponceModel();
            try
            {
                var gettask = Context.TblTaskDetails.Where(e => e.Id == updatetask.Id).FirstOrDefault();
                if (gettask != null)
                {
                    gettask.TaskStatus = updatetask.TaskStatus;
                    gettask.TaskTitle = updatetask.TaskTitle;
                    gettask.TaskDetails = updatetask.TaskDetails;
                    gettask.TaskType = updatetask.TaskType;
                    gettask.TaskDate = updatetask.TaskDate;
                    gettask.TaskEndDate = updatetask.TaskEndDate;
                    gettask.UpdatedOn = DateTime.Now;
                    gettask.UpdatedBy = updatetask.UpdatedBy;


                    Context.TblTaskDetails.Update(gettask);
                    Context.SaveChanges();
                    model.Message = "Task status updated successfully!";
                }
                else
                {
                    model.Code = (int)HttpStatusCode.NotFound;
                    model.Message = "Task id doesn't found";
                }
            }
            catch (Exception ex)
            {
                model.Code = (int)HttpStatusCode.InternalServerError;
                model.Message = "Error in updating task details.";
            }
            return model;
        }

        public async Task<UserResponceModel> DeleteTask(Guid Id)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var GetTaskdata = Context.TblTaskDetails.Where(a => a.Id == Id).FirstOrDefault();

                if (GetTaskdata != null)
                {
                    Context.TblTaskDetails.Remove(GetTaskdata);
                    Context.SaveChanges();
                    response.Data = GetTaskdata;
                    response.Message = "Task is deleted successfully";
                }
                else
                {
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Message = "Can not found";
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error deleting tasks";
            }
            return response;
        }
    }
}
