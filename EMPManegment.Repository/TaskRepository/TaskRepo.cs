
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
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Identity;

namespace EMPManegment.Repository.TaskRepository
{
    public class TaskRepo : ITaskDetails
    {
        private readonly string? pending;

        public TaskRepo(BonifatiusEmployeesContext context)
        {
            Context = context;
        }

        public BonifatiusEmployeesContext Context { get; }

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
                };
                response.Code = 200;
                response.Message = "Task add successfully!";
                Context.TblTaskDetails.Add(taskmodel);
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                response.Code = 400;
                response.Message = "Error in creating task.";
            }
            return response;
        }

        public async Task<UserResponceModel> UpdateTaskStatus(TaskDetailsView updatetask)
        {
            UserResponceModel responcemodel = new UserResponceModel();
            if (updatetask.TaskStatus == "Completed")
            {
                bool gettaskCheck = Context.TblTaskDetails.Any(e => e.Id == updatetask.Id && e.CreatedBy == updatetask.UserId);
                try
                {
                    if (gettaskCheck == true)
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
                            responcemodel.Code = 200;
                            responcemodel.Message = "Task status updated successfully!";
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        responcemodel.Code = 401;
                        responcemodel.Message = "You aren't authorize!!";
                    }
                }
                catch (Exception ex)
                {
                    responcemodel.Code = 400;
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
                    responcemodel.Code = 200;
                    responcemodel.Message = "Task status updated successfully!";
                }
                catch (Exception ex)
                {
                    responcemodel.Code = 400;
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
                                FirstName= b.FirstName,
                                LastName= b.LastName,
                                UserProfile= b.Image,
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

        public async Task<IEnumerable<TaskDetailsView>> GetTaskDetails(Guid Taskid, Guid ProjectId)
        {
            try
            {
                IEnumerable<TaskDetailsView> AllTaskDetails =
                    from a in Context.TblTaskDetails
                    join b in Context.TblUsers on a.UserId equals b.Id
                    join c in Context.TblTaskMasters on a.TaskType equals c.Id
                    where a.UserId == Taskid || a.CreatedBy == Taskid
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
                        LastName= b.LastName,
                        TaskTypeName = c.TaskType,
                        CreatedBy = a.CreatedBy,
                    };

                return AllTaskDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<jsonData> GetAllUserTaskDetails(DataTableRequstModel AllUserTaskDetails)
        {
            var AllTaskDetailsDataTable = from a in Context.TblTaskDetails
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
                                              TaskTypeName = c.TaskType,
                                          };
            if (!string.IsNullOrEmpty(AllUserTaskDetails.sortColumn) && !string.IsNullOrEmpty(AllUserTaskDetails.sortColumnDir))
            {
                AllTaskDetailsDataTable = AllTaskDetailsDataTable.OrderBy(AllUserTaskDetails.sortColumn + " " + AllUserTaskDetails.sortColumnDir);
            }

            if (!string.IsNullOrEmpty(AllUserTaskDetails.searchValue))
            {
                AllTaskDetailsDataTable = AllTaskDetailsDataTable.Where(e => e.UserName.Contains(AllUserTaskDetails.searchValue) || e.TaskStatus.Contains(AllUserTaskDetails.searchValue) || e.TaskTypeName.Contains(AllUserTaskDetails.searchValue) || e.TaskEndDate.ToString().ToLower().Contains(AllUserTaskDetails.searchValue.ToLower()));
            }
            int totalRecord = AllTaskDetailsDataTable.Count();
            var cData = AllTaskDetailsDataTable.Skip(AllUserTaskDetails.skip).Take(AllUserTaskDetails.pageSize).ToList();

            jsonData jsonData = new jsonData
            {
                draw = AllUserTaskDetails.draw,
                recordsFiltered = totalRecord,
                recordsTotal = totalRecord,
                data = cData
            };
            return jsonData;
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
                    model.Code = 200;
                    model.Message = "Task status updated successfully!";
                }
                else
                {
                    model.Code = 404;
                    model.Message = "Task id doesn't found";
                }        
            }
            catch (Exception ex)
            {
                model.Code = 400;
                model.Message = "Error in updating task details.";
            }
            return model;
        }

    }
}
