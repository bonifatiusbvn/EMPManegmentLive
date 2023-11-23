using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Interface.ProjectDetails
{
    public interface IProjectDetails
    {
        Task<UserResponceModel> CreateProject(ProjectDetailView CreateProject);

        Task<IEnumerable<ProjectDetailView>> GetProjectList(string? searchby, string? searchfor);
    }
}
