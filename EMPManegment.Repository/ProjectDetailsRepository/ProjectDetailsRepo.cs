using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.Inretface.Interface.ProjectDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Repository.ProjectDetailsRepository
{
    public class ProjectDetailsRepo : IProjectDetails
    {
        public ProjectDetailsRepo(BonifatiusEmployeesContext context)
        {
            Context = context;
        }

        public BonifatiusEmployeesContext Context { get; }

        public async Task<UserResponceModel> CreateProject(ProjectDetailView createproject)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var projectmodel = new TblProjectMaster()
                {
                    ProjectId = Guid.NewGuid(),
                    ProjectType = createproject.ProjectType,
                    ProjectTitle = createproject.ProjectTitle,
                    ProjectHead = createproject.ProjectHead,
                    ProjectDescription = createproject.ProjectDescription,
                    ProjectLocation = createproject.ProjectLocation,
                    ProjectPriority = createproject.ProjectPriority,
                    ProjectStatus = createproject.ProjectStatus,
                    ProjectStartDate = createproject.ProjectStartDate,
                    ProjectEndDate = createproject.ProjectEndDate,
                    ProjectDeadline = createproject.ProjectDeadline,
                    CreatedOn = DateTime.Now
                };
                response.Code = 200;
                response.Message = "Project add successfully!";
                Context.TblProjectMasters.Add(projectmodel);
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }
}
