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

        public async Task<UserResponceModel> AddTaskDetails(TaskDetailsView task)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                     var model = new TblTaskDetail()
                    {
                        Id = Guid.NewGuid(),
                        TaskType = task.TaskType,
                        TaskTitle = task.TaskTitle,
                        TaskDetails = task.TaskDetails,
                        TaskDate = task.TaskDate,
                        UserId = task.UserId,
                        CreatedOn = DateTime.Now,
                        TaskEndDate = task.TaskEndDate,
                    };
                    response.Code = 200;
                    response.Message = "Task add successfully!";
                    Context.TblTaskDetails.Add(model);
                    Context.SaveChanges();                
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            return response;
        }

        public async Task<UserResponceModel> UpdateDealStatus(TaskDetailsView task)
        {
            UserResponceModel model = new UserResponceModel();
            var data = Context.TblTaskDetails.Where(e => e.Id == task.Id).FirstOrDefault();
            try
            {
                if (data != null)
                {
                    data.TaskStatus = task.TaskStatus;
                }
                Context.TblTaskDetails.Update(data);
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

        public async Task<List<TaskDetailsView>> GetUserTaskDetails(TaskDetailsView task)
        {
            var UserId = new List<TaskDetailsView>();
            var data = await Context.TblTaskDetails.Where(x=>x.UserId == task.UserId).ToListAsync();
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
