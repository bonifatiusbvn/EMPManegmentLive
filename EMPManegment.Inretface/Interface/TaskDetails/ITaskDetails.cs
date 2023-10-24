using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Interface.TaskDetails
{
    public interface ITaskDetails
    {
        Task<IEnumerable<TaskTypeView>> GetTaskType();
        Task<TaskDetailsResponseModel> AddTaskDetails(TaskDetailsView task);
    }
}
