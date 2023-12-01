using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Services.TaskServices
{
    public interface ITaskServices
    {
        Task<IEnumerable<TaskTypeView>> GetTaskType();
        Task<UserResponceModel> AddTaskDetails(TaskDetailsView AddTaskDetails);
        Task<List<TaskDetailsView>> GetUserTaskDetails(TaskDetailsView GetTaskDetails);
        Task<UserResponceModel> UpdateTaskStatus(TaskDetailsView updatetask);
        Task<TaskDetailsView> GetTaskDetailsById(Guid Taskid);
        Task<IEnumerable<TaskDetailsView>> GetAllUserTaskDetails();
        Task<IEnumerable<TaskDetailsView>> GetTaskDetails(Guid Taskid);
        Task<TaskDetailsView> GetTaskofpendingTask(TaskDetailsView pendingtask);
    }
}
