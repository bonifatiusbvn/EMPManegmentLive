using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.Inretface.Interface.ProjectDetails;
using EMPManegment.Inretface.Services.ProjectDetailsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.ProjectDetails
{
    public class ProjectDetailsServices : IProjectDetailServices
    {
        public ProjectDetailsServices(IProjectDetails projectDetails)
        {
            ProjectDetails = projectDetails;
        }

        public IProjectDetails ProjectDetails { get; }

        public async Task<UserResponceModel> CreateProject(ProjectDetailView createProject)
        {
            return await ProjectDetails.CreateProject(createProject);
        }
        public async Task<IEnumerable<ProjectDetailView>> GetProjectList()
        {
            return await ProjectDetails.GetProjectList();
        }
    }
}
