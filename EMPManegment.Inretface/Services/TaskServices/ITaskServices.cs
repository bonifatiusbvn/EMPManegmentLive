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
    }
}
