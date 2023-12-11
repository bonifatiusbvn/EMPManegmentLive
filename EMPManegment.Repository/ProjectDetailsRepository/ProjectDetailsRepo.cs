using Azure;
using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.EntityModels.ViewModels.UserModels;
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

       public async Task<ProjectDetailView> GetProjectDetailsById(Guid ProjectId)
        {
            var projectDetail = await Context.TblProjectMasters.SingleOrDefaultAsync(x=>x.ProjectId == ProjectId);
            ProjectDetailView model = new ProjectDetailView()
            {
                ProjectId = projectDetail.ProjectId,
                ProjectTitle = projectDetail.ProjectTitle,
                CreatedOn = projectDetail.CreatedOn,
                ProjectEndDate = projectDetail.ProjectEndDate,
                ProjectStatus = projectDetail.ProjectStatus,
                ProjectPriority = projectDetail.ProjectPriority,
                ProjectStartDate = projectDetail.ProjectStartDate,
                ProjectDescription = projectDetail.ProjectDescription,
                ProjectType = projectDetail.ProjectType,
            };
            return model;
        }

        public async Task<IEnumerable<EmpDetailsView>> GetAllMembers()
        {
            IEnumerable<EmpDetailsView> data = Context.TblUsers.ToList().Select(a => new EmpDetailsView
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Image = a.Image,              
            });
            return data;
        }

       public async Task<UserResponceModel> AddMemberToProject(ProjectView AddMember)
        {
            UserResponceModel response = new UserResponceModel();
            try 
            { 
                var projectmodel = new TblProjectDetail()
                {
                    Id = Guid.NewGuid(),
                    ProjectId = AddMember.ProjectId,
                    ProjectType = AddMember.ProjectType,
                    ProjectTitle = AddMember.ProjectTitle,
                    UserId = AddMember.UserId,
                    StartDate = AddMember.StartDate,
                    EndDate = AddMember.EndDate,
                    Status = AddMember.Status,
                };
                response.Code = 200;
                response.Message = "Member add successfully!";
                Context.TblProjectDetails.Add(projectmodel);
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
        public async Task<IEnumerable<ProjectView>> GetProjectMember(Guid ProjectId)
        {
            try
            {
                var result = (from e in Context.TblProjectDetails
                             where e.ProjectId == ProjectId
                             join d in Context.TblUsers on e.UserId equals d.Id
                             select new ProjectView
                             {
                                 Id = e.Id,
                                 Fullname = d.FirstName + " " + d.LastName,
                                 Image = d.Image,
                             }).ToList();
                return result;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
    }
}
