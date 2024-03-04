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
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                    ProjectName = createproject.ProjectName,
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

        public async Task<List<ProjectView>> GetUserProjectList(Guid UserId)
        {
            var UserData = new List<ProjectView>();
            var data = await (from a in Context.TblProjectDetails
                              join b in Context.TblProjectMasters
                              on a.ProjectId equals b.ProjectId
                              where a.UserId == UserId
                              select new
                              {
                                  a.Id,
                                  a.ProjectId,
                                  a.ProjectType,
                                  a.ProjectTitle,
                                  a.Status,
                                  a.CreatedOn,
                                  a.UserId,
                                  a.TotalMember,
                                  a.StartDate,
                                  a.EndDate,
                                  b.ProjectDescription,
                              }).ToListAsync();

            if (data != null)
            {
                foreach (var item in data)
                {
                    UserData.Add(new ProjectView()
                    {
                        Id = item.Id,
                        ProjectId = item.ProjectId,
                        ProjectType = item.ProjectType,
                        ProjectTitle = item.ProjectTitle,
                        Status = item.Status,
                        CreatedOn = item.CreatedOn,
                        UserId = item.UserId,
                        TotalMember = item.TotalMember,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        ProjectDescription = item.ProjectDescription,
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
                                 //UserRole = e.UserRole,
                                 Designation = d.Designation,
                             }).ToList();
                return result;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        public async Task<UserResponceModel> AddDocumentToProject(ProjectDocumentView AddDocument)
        {
            UserResponceModel response = new UserResponceModel();
            try 
            {
                var projectDocumentmodel = new TblProjectDocument()
                {
                    Id = Guid.NewGuid(),
                    ProjectId = AddDocument.ProjectId,
                    UserId = AddDocument.UserId,
                    Date = DateTime.Today,
                    DocumentName = AddDocument.DocumentName
                };
                response.Code = 200;
                response.Message = "Document uploaded successfully!";
                Context.TblProjectDocuments.Add(projectDocumentmodel);
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

       public async Task<IEnumerable<ProjectDocumentView>> GetProjectDocument(Guid ProjectId)
        {
            try
            {
                var result = (from e in Context.TblProjectDocuments
                              where e.ProjectId == ProjectId
                              join d in Context.TblUsers on e.UserId equals d.Id
                              select new ProjectDocumentView
                              {
                                  Id = e.Id,
                                  ProjectId = ProjectId,
                                  DocumentName = e.DocumentName,
                                  FullName = d.FirstName + " "+d.LastName,
                                  Date = e.Date
                              }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ProjectDetailView>> GetProjectListById(string? searchby, string? searchfor, Guid UserId)
        {
            var UserData = new List<ProjectDetailView>();
            var data = await (from projectDetail in Context.TblProjectDetails
                              join projectMaster in Context.TblProjectMasters
                              on projectDetail.ProjectId equals projectMaster.ProjectId
                              where projectDetail.UserId == UserId
                              select new
                              {
                                  projectDetail.Id,
                                  projectDetail.ProjectId,
                                  projectDetail.ProjectType,
                                  projectDetail.ProjectTitle,
                                  projectDetail.Status,
                                  projectDetail.CreatedOn,
                                  projectDetail.UserId,
                                  projectDetail.TotalMember,
                                  projectMaster.ProjectStartDate,
                                  projectMaster.ProjectEndDate,
                                  projectMaster.ProjectDeadline,
                                  projectMaster.ProjectDescription,
                              }).ToListAsync();

            if (data != null)
            {
                foreach (var item in data)
                {
                    UserData.Add(new ProjectDetailView()
                    {
                        Id = item.Id,
                        ProjectId = item.ProjectId,
                        ProjectType = item.ProjectType,
                        ProjectTitle = item.ProjectTitle,
                        ProjectStatus = item.Status,
                        CreatedOn = item.CreatedOn,
                        UserId = item.UserId,
                        ProjectStartDate = item.ProjectStartDate,
                        ProjectDeadline = item.ProjectDeadline,
                        ProjectDescription = item.ProjectDescription,
                    });
                }
            }
            if (searchby == "ProjectTitle" && searchfor != null)
            {
                UserData = UserData.Where(ser => ser.ProjectTitle.ToLower().Contains(searchfor.ToLower())).ToList();
            }
            if (searchby == "ProjectStatus" && searchfor != null)
            {
                UserData = UserData.Where(ser => ser.ProjectStatus.ToLower().Contains(searchfor.ToLower())).ToList();
            }
            return UserData;
        }

        public string CheckProjectName()
        {
            try
            {
                var LastProject = Context.TblProjectMasters.OrderByDescending(e => e.CreatedOn).FirstOrDefault();
                string UserProjectId;
                if (LastProject == null)
                {
                    UserProjectId = "PROJ-01";
                }
                else
                {
                    if (LastProject.ProjectName.Length >= 7)
                    {
                        string projectNumberStr = LastProject.ProjectName.Substring(5);
                        if (int.TryParse(projectNumberStr, out int projectNumber))
                        {
                            int incrementedProjectNumber = projectNumber + 1;
                            UserProjectId = "PROJ-" + incrementedProjectNumber.ToString("D2");
                        }
                        else
                        {
                            throw new Exception("Unable to parse project number from project name.");
                        }
                    }
                    else
                    {
                        throw new Exception("ProjectName does not have expected format.");
                    }
                }
                return UserProjectId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
