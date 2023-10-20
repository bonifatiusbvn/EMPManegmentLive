using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.Inretface.Interface.TaskDetails;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
