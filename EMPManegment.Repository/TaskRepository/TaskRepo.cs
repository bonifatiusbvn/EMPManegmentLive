using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.Inretface.Interface.TaskDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
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

        public async Task<UserResponceModel> UpdateDealStatus(TaskDetailsView updatetask)
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
            var data = await Context.TblTaskDetails.Where(x=>x.UserId == GetUserTaskDetails.UserId).ToListAsync();
            if(data != null)
            {
                foreach(var item in data)
                {
                    
                    UserId.Add(new TaskDetailsView()
                    {
                        Id = item.Id,
                        TaskStatus = item.TaskStatus,
                        TaskDate  = item.TaskDate,
                        TaskDetails = item.TaskDetails,
                        TaskEndDate = item.TaskEndDate,
                        TaskTitle = item.TaskTitle,
                    });
                }
            }
            return UserId;
        }
    }
}
