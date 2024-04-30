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
using System.Net;
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
                    ShortName = createproject.ShortName,
                    ProjectHead = createproject.ProjectHead,
                    ProjectDescription = createproject.ProjectDescription,
                    BuildingName = createproject.BuildingName,
                    Area = createproject.Area,
                    State = createproject.State,
                    City = createproject.City,
                    Country = createproject.Country,
                    PinCode = createproject.PinCode,
                    ProjectPath = createproject.ProjectPath,
                    ProjectPriority = createproject.ProjectPriority,
                    ProjectStatus = createproject.ProjectStatus,
                    ProjectStartDate = createproject.ProjectStartDate,
                    ProjectEndDate = createproject.ProjectEndDate,
                    ProjectDeadline = createproject.ProjectDeadline,
                    ProjectImage = createproject.ProjectImage,
                    CreatedOn = DateTime.Now,
                    CreatedBy = createproject.CreatedBy,
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
                BuildingName = a.BuildingName,
                Area = a.Area,
                City = a.City,
                State = a.State,
                Country = a.Country,
                ProjectImage = a.ProjectImage,
                PinCode = a.PinCode,
                ProjectPath = a.ProjectPath,
                ProjectPriority = a.ProjectPriority,
                ProjectStatus = a.ProjectStatus,
                ProjectStartDate = a.ProjectStartDate,
                ProjectEndDate = a.ProjectEndDate,
                ProjectDeadline = a.ProjectDeadline,
                CreatedOn = a.CreatedOn
            });
            if (searchby == "ProjectTitle" && searchfor != null)
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
            var data = await (from a in Context.TblProjectMembers
                              join b in Context.TblProjectMasters
                              on a.ProjectId equals b.ProjectId
                              where a.UserId == UserId && a.IsDeleted != false
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
                                  b.ProjectImage,
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
            var projectDetail = await Context.TblProjectMasters.SingleOrDefaultAsync(x => x.ProjectId == ProjectId);
            ProjectDetailView model = new ProjectDetailView()
            {
                ProjectId = projectDetail.ProjectId,
                ProjectTitle = projectDetail.ProjectTitle,
                ProjectImage = projectDetail.ProjectImage,
                ShortName = projectDetail.ShortName,
                BuildingName = projectDetail.BuildingName,
                Area = projectDetail.Area,
                Country = projectDetail.Country,
                State = projectDetail.State,
                City = projectDetail.City,
                PinCode = projectDetail.PinCode,
                ProjectPath = projectDetail.ProjectPath,
                ProjectDeadline = projectDetail.ProjectDeadline,
                ProjectHead = projectDetail.ProjectHead,
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
                bool isMemberAlreadyExists = Context.TblProjectMembers.Any(x => x.UserId == AddMember.UserId && x.ProjectId == AddMember.ProjectId);
                if (isMemberAlreadyExists == true)
                {
                    var projectDetail = await Context.TblProjectMembers.SingleOrDefaultAsync(x => x.UserId == AddMember.UserId && x.ProjectId == AddMember.ProjectId);
                    if (projectDetail.IsDeleted == true)
                    {
                        response.Message = "Member already exists";
                        response.Code = 404;
                    }
                    else
                    {
                        var GetUserdata = Context.TblProjectMembers.Where(a => a.UserId == AddMember.UserId && a.ProjectId == AddMember.ProjectId).FirstOrDefault();
                        GetUserdata.IsDeleted = true;
                        Context.TblProjectMembers.Update(GetUserdata);
                        Context.SaveChanges();
                        response.Code = 200;
                        response.Data = AddMember;
                        response.Message = "Project Member Is Added Succesfully!";
                    }
                }
                else
                {
                    var projectmodel = new TblProjectMember()
                    {
                        Id = Guid.NewGuid(),
                        ProjectId = AddMember.ProjectId,
                        ProjectType = AddMember.ProjectType,
                        ProjectTitle = AddMember.ProjectTitle,
                        UserId = AddMember.UserId,
                        StartDate = AddMember.StartDate,
                        EndDate = AddMember.EndDate,
                        Status = AddMember.Status,
                        IsDeleted = true,
                    };
                    response.Code = 200;
                    response.Message = "Member add successfully!";
                    Context.TblProjectMembers.Add(projectmodel);
                    Context.SaveChanges();
                }
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
                var result = (from e in Context.TblProjectMembers
                              where e.ProjectId == ProjectId
                              join d in Context.TblUsers on e.UserId equals d.Id
                              where e.IsDeleted != false
                              select new ProjectView
                              {
                                  Id = e.Id,
                                  ProjectId = e.ProjectId,
                                  Fullname = d.FirstName + " " + d.LastName,
                                  Image = d.Image,
                                  UserId = e.UserId,
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
                    DocumentName = AddDocument.DocumentName,
                    CreatdBy = AddDocument.CreatdBy,
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
                                  FullName = d.FirstName + " " + d.LastName,
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
            var data = await (from projectDetail in Context.TblProjectMembers
                              join projectMaster in Context.TblProjectMasters
                              on projectDetail.ProjectId equals projectMaster.ProjectId
                              where projectDetail.UserId == UserId && projectDetail.IsDeleted != false
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
                                  projectDetail.IsDeleted,
                                  projectMaster.ProjectImage,
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
                    if (LastProject.ShortName.Length >= 7)
                    {
                        string projectNumberStr = LastProject.ShortName.Substring(5);
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
        public async Task<UserResponceModel> MemberIsDeleted(ProjectMemberUpdate projectMember)
        {
            UserResponceModel response = new UserResponceModel();
            var GetUserdata = Context.TblProjectMembers.Where(a => a.UserId == projectMember.UserId && a.ProjectId == projectMember.ProjectId).FirstOrDefault();

            if (GetUserdata != null)
            {

                if (GetUserdata.IsDeleted == true)
                {
                    GetUserdata.IsDeleted = false;
                    Context.TblProjectMembers.Update(GetUserdata);
                    Context.SaveChanges();
                    response.Code = 200;
                    response.Data = GetUserdata;
                    response.Message = "Project Member Is Deactive Succesfully";
                }

                else
                {
                    GetUserdata.IsDeleted = true;
                    Context.TblProjectMembers.Update(GetUserdata);
                    Context.SaveChanges();
                    response.Code = 200;
                    response.Data = GetUserdata;
                    response.Message = "Project Member Is Active Succesfully";
                }


            }
            return response;
        }
    }
}
