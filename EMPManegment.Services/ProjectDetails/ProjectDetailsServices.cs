using EMPManegment.EntityModels.View_Model;
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
        public async Task<IEnumerable<ProjectDetailView>> GetProjectList(string? searchby, string? searchfor)
        {
            return await ProjectDetails.GetProjectList(searchby,searchfor);
        }
        public async Task<List<ProjectView>> GetUserProjectList(Guid UserId)
        {
            return await ProjectDetails.GetUserProjectList(UserId);
        }
        public async Task<ProjectDetailView> GetProjectDetailsById(Guid ProjectId)
        {
            return await ProjectDetails.GetProjectDetailsById(ProjectId);
        }
        public async Task<IEnumerable<EmpDetailsView>> GetAllMembers()
        {
            return await ProjectDetails.GetAllMembers();
        }
        public async Task<UserResponceModel> AddMemberToProject(ProjectView AddMember)
        {
            return await ProjectDetails.AddMemberToProject(AddMember);
        }
        public async Task<IEnumerable<ProjectView>> GetProjectMember(Guid ProjectId)
        {
            return await ProjectDetails.GetProjectMember(ProjectId);
        }
        public async Task<UserResponceModel> AddDocumentToProject(ProjectDocumentView AddDocument)
        {
            return await ProjectDetails.AddDocumentToProject(AddDocument);
        }
        public async Task<IEnumerable<ProjectDocumentView>> GetProjectDocument(Guid ProjectId)
        {
            return await ProjectDetails.GetProjectDocument(ProjectId);
        }

        public async Task<List<ProjectDetailView>> GetProjectListById(string? searchby, string? searchfor, Guid UserId)
        {
            return await ProjectDetails.GetProjectListById(searchby, searchfor, UserId);
        }

        public string CheckProjectName()
        {
            return ProjectDetails.CheckProjectName();
        }

        public async Task<UserResponceModel> MemberIsDeleted(ProjectMemberUpdate projectMember)
        {
            return await ProjectDetails.MemberIsDeleted(projectMember);
        }
    }
}
