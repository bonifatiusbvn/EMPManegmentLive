﻿using EMPManegment.EntityModels.ViewModels.TaskModels;
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
        public async Task<TaskDetailsResponseModel> AddTaskDetails(TaskDetailsView task)
        {
            return await TaskDetails.AddTaskDetails(task);
        }
    }
}
