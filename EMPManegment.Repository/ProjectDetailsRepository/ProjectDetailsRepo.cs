using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.Inretface.Interface.ProjectDetails;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections;
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
        public async Task<IEnumerable<ProjectDetailView>> GetProjectList(string? searchby, string? searchfor)
        {
            IEnumerable<ProjectDetailView> data = Context.TblProjectMasters.ToList().Select(a => new ProjectDetailView
            {
                ProjectId = a.ProjectId,
                ProjectType = a.ProjectType,
                ProjectTitle = a.ProjectTitle,
                ProjectHead = a.ProjectHead,
                ProjectDescription = a.ProjectDescription,
                ProjectLocation = a.ProjectLocation,
                ProjectPriority = a.ProjectPriority,
                ProjectStatus = a.ProjectStatus,
                ProjectStartDate = a.ProjectStartDate,
                ProjectEndDate = a.ProjectEndDate,
                ProjectDeadline = a.ProjectDeadline,
                CreatedOn= a.CreatedOn
            });
            if(searchby == "ProjectTitle" && searchfor != null)
            {
                data = data.Where(ser => ser.ProjectTitle.ToLower().Contains(searchfor.ToLower())).ToList();
            }
            if (searchby == "ProjectStatus" && searchfor != null)
            {
                data = data.Where(ser => ser.ProjectStatus.ToLower().Contains(searchfor.ToLower())).ToList();
            }
            return data;
        }

        public async Task<List<ProjectDetailView>> GetUserProjectList(ProjectDetailView GetUserProjectList)
        {
            var UserData = new List<ProjectDetailView>();
            var data = await Context.TblProjectDetails.Where(x => x.UserId == GetUserProjectList.UserId).ToListAsync();
            if (data != null)
            {
                foreach (var item in data)
                {
                    UserData.Add(new ProjectDetailView()
                    {
                        ProjectId = item.ProjectId,
                        ProjectType = item.ProjectType,
                        ProjectTitle = item.ProjectTitle,
                        CreatedOn = item.CreatedOn,
                    });
                }
            }
            return UserData;
        }
    }
}
