using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Services.ProjectDetailsServices
{
    public interface IProjectDetailServices
    {
        Task<UserResponceModel> CreateProject(ProjectDetailView CreateProject);
    }
}
