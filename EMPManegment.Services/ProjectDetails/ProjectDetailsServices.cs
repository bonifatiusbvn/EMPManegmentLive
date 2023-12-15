﻿using EMPManegment.EntityModels.View_Model;
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
    }
}
