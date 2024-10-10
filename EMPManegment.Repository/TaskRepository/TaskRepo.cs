
using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.Inretface.Interface.TaskDetails;
using EMPManegment.Inretface.Interface.UserAttendance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Repository.TaskRepository
{
    public class TaskRepo : ITaskDetails
    {
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
                    CreatedOn = DateTime.Now,
                    TaskEndDate = addtask.TaskEndDate,
                };
                response.Code = 200;
                response.Message = "Task add successfully!";
                Context.TblTaskDetails.Add(taskmodel);
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<UserResponceModel> UpdateTaskStatus(TaskDetailsView updatetask)
        {
            UserResponceModel model = new UserResponceModel();
            var gettask = Context.TblTaskDetails.Where(e => e.Id == updatetask.Id).FirstOrDefault();
            try
            {
                if (gettask != null)
                {
                    gettask.TaskStatus = updatetask.TaskStatus;
               
                }
                Context.TblTaskDetails.Update(gettask);
                Context.SaveChanges();
                model.Code = 200;
                model.Message = "Task Status Updated Successfully!";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
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
                                                              TaskTypeName = c.TaskType
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
                                TaskTypeName = m.TaskType
                            }).First();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return taskdata;
        }

        public async Task<IEnumerable<TaskDetailsView>> GetTaskDetails(Guid Taskid)
        {
            try
            {
                IEnumerable<TaskDetailsView>
                AllTaskDetails = from a in Context.TblTaskDetails
                                 join b in Context.TblUsers on a.User.Id equals b.Id
                                 join c in Context.TblTaskMasters on a.TaskType equals c.Id
                                 where b.Id == Taskid
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
                                     TaskTypeName = c.TaskType
                                 };
                return AllTaskDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<TaskDetailsView> GetTaskofpendingTask(TaskDetailsView pendingtask)
        {
            try
            {
                var TaskList = Context.TblTaskDetails.Where(a => a.TaskStatus == pendingtask.TaskStatus).ToList();
                TaskDetailsView Task = new TaskDetailsView();
                {
                    Task.Id = pendingtask.Id;
                    Task.TaskType = pendingtask.TaskType;
                    Task.TaskStatus = pendingtask.TaskStatus;
                    Task.UserId = pendingtask.UserId;
                }
                return Task;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
