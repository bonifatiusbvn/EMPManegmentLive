using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.Inretface.Interface.TaskDetails;
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
                if(task!= null)
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
                else
                {
                    response.Code = 200;
                }
                
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            return response;
        }
    }
}
