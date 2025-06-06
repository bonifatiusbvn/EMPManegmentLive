using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.AGGridModels;
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
        public async Task<IEnumerable<ProjectDetailView>> GetProjectList(ProjectRequest projectRequest)
        {
            return await ProjectDetails.GetProjectList(projectRequest);
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
        public async Task<UserResponceModel> AddMemberToProject(ProjectMemberMasterView AddMember)
        {
            return await ProjectDetails.AddMemberToProject(AddMember);
        }
        public async Task<AGGridResponseModel<ProjectView>> GetProjectMember(AGGridRequestModel ProjectMemberRequest)
        {
            return await ProjectDetails.GetProjectMember(ProjectMemberRequest);
        }

        public async Task<List<ProjectView>> ShowProjectMemberList(Guid ProjectId)
        {
            return await ProjectDetails.ShowProjectMemberList(ProjectId);
        }

        public async Task<UserResponceModel> AddDocumentToProject(ProjectDocumentView AddDocument)
        {
            return await ProjectDetails.AddDocumentToProject(AddDocument);
        }
        public async Task<AGGridResponseModel<ProjectDocumentView>> GetProjectDocument(AGGridRequestModel ProjectDocumentRequest)
        {
            return await ProjectDetails.GetProjectDocument(ProjectDocumentRequest);
        }

        public async Task<List<ProjectDocumentView>> ShowProjectDocumentList(Guid ProjectId)
        {
            return await ProjectDetails.ShowProjectDocumentList(ProjectId);
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

        public async Task<UserResponceModel> DeleteProjectDocument(Guid DocumentId)
        {
            return await ProjectDetails.DeleteProjectDocument(DocumentId);
        }

        public async Task<IEnumerable<ProjectDetailView>> GetProjectsList()
        {
            return await ProjectDetails.GetProjectsList();
        }

        public async Task<UserResponceModel> UpdateProjectDetails(ProjectDetailView updateProject)
        {
            return await ProjectDetails.UpdateProjectDetails(updateProject);
        }

        public async Task<IEnumerable<ProjectView>> GetProjectNameList(string? search)
        {
            return await ProjectDetails.GetProjectNameList(search);
        }

        public async Task<ProjectMemberUpdate> EditProjectMemberDesignation(Guid ProjectMemberId)
        {
            return await ProjectDetails.EditProjectMemberDesignation(ProjectMemberId);
        }

        public async Task<UserResponceModel> UpdateProjectMemberDesignation(ProjectMemberMasterView UpdateDesignation)
        {
            return await ProjectDetails.UpdateProjectMemberDesignation(UpdateDesignation);
        }
    }
}
