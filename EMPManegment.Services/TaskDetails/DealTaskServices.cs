using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.Inretface.Interface.TaskDetails;
using EMPManegment.Inretface.Services.TaskServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.TaskDetails
{
    public class DealTaskServices : ITaskServices
    {
        public DealTaskServices(ITaskDetails taskDetails)
        {
            TaskDetails = taskDetails;
        }

        public ITaskDetails TaskDetails { get; }

        public async Task<IEnumerable<TaskTypeView>> GetTaskType()
        {
            try
            {
                return await TaskDetails.GetTaskType();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<UserResponceModel> AddTaskDetails(TaskDetailsView addTask)
        {
            return await TaskDetails.AddTaskDetails(addTask);
        }

        public async Task<List<TaskDetailsView>> GetUserTaskDetails(TaskDetailsView Gettaskdetails)
        {
            return await TaskDetails.GetUserTaskDetails(Gettaskdetails);
        }
        public async Task<UserResponceModel> UpdateTaskStatus(TaskDetailsView updatetask)
        {
            return await TaskDetails.UpdateTaskStatus(updatetask);
        }
        
        public async Task<IEnumerable<TaskDetailsView>> GetAllUserTaskDetails()
        {
            return await TaskDetails.GetAllUserTaskDetails();
        }

        public async Task<TaskDetailsView> GetTaskDetailsById(Guid Taskid)
        {
            return await TaskDetails. GetTaskDetailsById(Taskid);
        }

        public async Task<IEnumerable<TaskDetailsView>> GetTaskDetails(Guid Taskid)
        {
            return await TaskDetails.GetTaskDetails(Taskid);
        }


        public async Task<TaskDetailsView> GetTaskofpendingTask(TaskDetailsView pendingtask)
        {
            return await TaskDetails.GetTaskofpendingTask(pendingtask);
        }
    }
}
