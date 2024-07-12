using Azure;
using EMPManagment.API;
using EMPManegment.EntityModels.Common;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.EntityModels.ViewModels.UserModels;
using EMPManegment.Inretface.Interface.ProjectDetails;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EMPManegment.Repository.ProjectDetailsRepository
{
    public class ProjectDetailsRepo : IProjectDetails
    {
        public ProjectDetailsRepo(BonifatiusEmployeesContext context, IConfiguration configuration)
        {
            Context = context;
            _configuration = configuration;
        }

        public BonifatiusEmployeesContext Context { get; }
        public IConfiguration _configuration { get; }

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

                response.Message = "Project add successfully!";
                Context.TblProjectMasters.Add(projectmodel);
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in creating project.";
            }
            return response;
        }
        public async Task<IEnumerable<ProjectDetailView>> GetProjectList(string? searchby, string? searchfor)
        {

            IEnumerable<ProjectDetailView> data = Context.TblProjectMasters.OrderByDescending(x => x.CreatedOn).ToList().Select(a => new ProjectDetailView
            {
                ProjectId = a.ProjectId,
                ProjectType = a.ProjectType,
                ProjectTitle = a.ProjectTitle,
                ShortName = a.ShortName,
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
                              where a.UserId == UserId && a.IsDeleted != true
                              select new
                              {
                                  a.Id,
                                  a.ProjectId,
                                  b.ProjectType,
                                  b.ProjectTitle,
                                  b.ProjectStatus,
                                  a.CreatedOn,
                                  a.UserId,
                                  b.ProjectStartDate,
                                  b.ProjectEndDate,
                                  b.ProjectDescription,
                                  b.ProjectImage,
                                  b.ShortName,
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
                        Status = item.ProjectStatus,
                        CreatedOn = item.CreatedOn,
                        UserId = item.UserId,
                        StartDate = item.ProjectStartDate,
                        EndDate = item.ProjectEndDate,
                        ProjectDescription = item.ProjectDescription,
                        ShortName = item.ShortName,

                    });
                }
            }
            return UserData;
        }

        public async Task<ProjectDetailView> GetProjectDetailsById(Guid ProjectId)
        {
            try
            {
                string dbConnectionStr = _configuration.GetConnectionString("EMPDbconn");
                var sqlPar = new SqlParameter[]
                {
                    new SqlParameter("@ProjectId", ProjectId),
                };

                var DS = DbHelper.GetDataSet("[GetProjectDetailsById]", System.Data.CommandType.StoredProcedure, sqlPar, dbConnectionStr);

                ProjectDetailView projectDetail = new ProjectDetailView();

                if (DS != null && DS.Tables.Count > 0)
                {
                    if (DS.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = DS.Tables[0].Rows[0];

                        projectDetail.ProjectId = row["ProjectId"] != DBNull.Value ? (Guid)row["ProjectId"] : Guid.Empty;
                        projectDetail.ProjectTitle = row["ProjectTitle"]?.ToString();
                        projectDetail.ProjectImage = row["ProjectImage"]?.ToString();
                        projectDetail.ShortName = row["ShortName"]?.ToString();
                        projectDetail.BuildingName = row["BuildingName"]?.ToString();
                        projectDetail.Area = row["Area"]?.ToString();
                        projectDetail.Country = row["Country"] != DBNull.Value ? (int)row["Country"] : 0;
                        projectDetail.State = row["State"] != DBNull.Value ? (int)row["State"] : 0;
                        projectDetail.City = row["City"] != DBNull.Value ? (int)row["City"] : 0;
                        projectDetail.PinCode = row["PinCode"]?.ToString();
                        projectDetail.ProjectPath = row["ProjectPath"]?.ToString();
                        projectDetail.ProjectDeadline = row["ProjectDeadline"] != DBNull.Value ? (DateTime)row["ProjectDeadline"] : DateTime.MinValue;
                        projectDetail.ProjectHead = row["ProjectHead"]?.ToString();
                        projectDetail.CreatedOn = row["CreatedOn"] != DBNull.Value ? (DateTime)row["CreatedOn"] : DateTime.MinValue;
                        projectDetail.ProjectEndDate = row["ProjectEndDate"] != DBNull.Value ? (DateTime)row["ProjectEndDate"] : DateTime.MinValue;
                        projectDetail.ProjectStatus = row["ProjectStatus"]?.ToString();
                        projectDetail.ProjectPriority = row["ProjectPriority"]?.ToString();
                        projectDetail.ProjectStartDate = row["ProjectStartDate"] != DBNull.Value ? (DateTime)row["ProjectStartDate"] : DateTime.MinValue;
                        projectDetail.ProjectDescription = row["ProjectDescription"]?.ToString();
                        projectDetail.ProjectType = row["ProjectType"]?.ToString();
                        projectDetail.CountryName = row["CountryName"]?.ToString();
                        projectDetail.StateName = row["StateName"]?.ToString();
                        projectDetail.CityName = row["CityName"]?.ToString();
                    }
                }
                return projectDetail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<EmpDetailsView>> GetAllMembers()
        {
            IEnumerable<EmpDetailsView> data = Context.TblUsers.Where(a => a.IsActive == true).ToList().Select(a => new EmpDetailsView
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Image = a.Image,
            }).Take(10);
            return data;
        }

        public async Task<UserResponceModel> AddMemberToProject(ProjectMemberMasterView AddMember)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var project = await Context.TblProjectMasters.FirstOrDefaultAsync(e => e.ProjectId == AddMember.ProjectId);

                if (project != null)
                {
                    foreach (var item in AddMember.ProjectMemberList)
                    {
                        var userId = await Context.TblUsers.FirstOrDefaultAsync(a => (a.FirstName + " " + a.LastName) == item.Fullname);
                        bool isMemberAlreadyExists = Context.TblProjectMembers.Any(x => x.UserId == userId.Id && x.ProjectId == AddMember.ProjectId);

                        if (isMemberAlreadyExists)
                        {
                            var projectDetail = await Context.TblProjectMembers.SingleOrDefaultAsync(x => x.UserId == userId.Id && x.ProjectId == AddMember.ProjectId);

                            if (projectDetail.IsDeleted == false)
                            {
                                response.Message = item.Fullname + " already exists in project.";
                                response.Code = (int)HttpStatusCode.NotFound;
                                return response;
                            }
                            else
                            {
                                projectDetail.IsDeleted = false;
                                projectDetail.ProjectId = project.ProjectId;
                                projectDetail.UpdatedOn = DateTime.Now;
                                projectDetail.UpdatedBy = AddMember.UpdatedBy;
                                Context.TblProjectMembers.Update(projectDetail);
                                await Context.SaveChangesAsync();

                                response.Message = "Project member is added successfully!";
                            }
                        }
                        else
                        {
                            var projectmodel = new TblProjectMember()
                            {
                                Id = Guid.NewGuid(),
                                ProjectId = project.ProjectId,
                                UserId = userId.Id,
                                IsDeleted = false,
                                CreatedBy = AddMember.UpdatedBy,
                                CreatedOn = DateTime.Now,
                            };

                            Context.TblProjectMembers.Add(projectmodel);
                            await Context.SaveChangesAsync();

                            response.Message = "Project member is added successfully!";
                        }
                    }
                    response.Data = project;
                }
                else
                {
                    response.Code = (int)HttpStatusCode.InternalServerError;
                    response.Message = "Project not found.";
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.NotFound;
                response.Message = "Error in adding member to project.";
            }
            return response;
        }

        public async Task<IEnumerable<ProjectView>> GetProjectMember(Guid ProjectId)
        {
            try
            {
                string dbConnectionStr = _configuration.GetConnectionString("EMPDbconn");
                var sqlPar = new SqlParameter[]
                {
                    new SqlParameter("@ProjectId", ProjectId),
                };

                var DS = DbHelper.GetDataSet("[GetProjectMember]", System.Data.CommandType.StoredProcedure, sqlPar, dbConnectionStr);

                List<ProjectView> projectMemberList = new List<ProjectView>();

                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        var ProjectMember = new ProjectView
                        {
                            Id = row["Id"] != DBNull.Value ? (Guid)row["Id"] : Guid.Empty,
                            ProjectId = row["ProjectId"] != DBNull.Value ? (Guid)row["ProjectId"] : Guid.Empty,
                            Fullname = row["Fullname"]?.ToString(),
                            FirstName = row["FirstName"]?.ToString(),
                            LastName = row["LastName"]?.ToString(),
                            Image = row["Image"]?.ToString(),
                            UserId = row["UserId"] != DBNull.Value ? (Guid)row["UserId"] : Guid.Empty,
                            Designation = row["Designation"]?.ToString(),
                            ProjectTitle = row["ProjectTitle"]?.ToString(),
                        };
                        projectMemberList.Add(ProjectMember);
                    }
                }
                return projectMemberList;
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
                    CreadetOn = DateTime.Now,
                };
                response.Message = "Document uploaded successfully!";
                Context.TblProjectDocuments.Add(projectDocumentmodel);
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in adding document to project.";
            }
            return response;
        }

        public async Task<IEnumerable<ProjectDocumentView>> GetProjectDocument(Guid ProjectId)
        {
            try
            {
                string dbConnectionStr = _configuration.GetConnectionString("EMPDbconn");
                var sqlPar = new SqlParameter[]
                {
                    new SqlParameter("@ProjectId", ProjectId),
                };

                var DS = DbHelper.GetDataSet("[GetProjectDocument]", System.Data.CommandType.StoredProcedure, sqlPar, dbConnectionStr);

                List<ProjectDocumentView> projectDocumentList = new List<ProjectDocumentView>();

                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        var ProjectDocument = new ProjectDocumentView
                        {

                            Id = row["Id"] != DBNull.Value ? (Guid)row["Id"] : Guid.Empty,
                            ProjectId = row["ProjectId"] != DBNull.Value ? (Guid)row["ProjectId"] : Guid.Empty,
                            DocumentName = row["DocumentName"]?.ToString(),
                            FullName = row["FullName"]?.ToString(),
                            Date = row["Date"] != DBNull.Value ? (DateTime)row["Date"] : DateTime.MinValue,
                        };
                        projectDocumentList.Add(ProjectDocument);
                    }
                }
                return projectDocumentList;
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
                              where projectDetail.UserId == UserId && projectDetail.IsDeleted != true
                              select new
                              {
                                  projectDetail.Id,
                                  projectDetail.ProjectId,
                                  projectMaster.ProjectType,
                                  projectMaster.ProjectTitle,
                                  projectMaster.ProjectStatus,
                                  projectDetail.CreatedOn,
                                  projectDetail.UserId,
                                  projectMaster.ProjectStartDate,
                                  projectMaster.ProjectEndDate,
                                  projectMaster.ProjectDeadline,
                                  projectMaster.ProjectDescription,
                                  projectDetail.IsDeleted,
                                  projectMaster.ProjectImage,
                                  projectMaster.ShortName,
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
                        ProjectStatus = item.ProjectStatus,
                        CreatedOn = item.CreatedOn,
                        UserId = item.UserId,
                        ProjectStartDate = item.ProjectStartDate,
                        ProjectDeadline = item.ProjectDeadline,
                        ProjectDescription = item.ProjectDescription,
                        ProjectImage = item.ProjectImage,
                        ShortName = item.ShortName,
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
            try
            {
                var GetUserdata = Context.TblProjectMembers.Where(a => a.UserId == projectMember.UserId && a.ProjectId == projectMember.ProjectId).FirstOrDefault();

                if (GetUserdata != null)
                {

                    if (GetUserdata.IsDeleted == true)
                    {
                        GetUserdata.IsDeleted = false;
                        GetUserdata.UpdatedOn = DateTime.Now;
                        GetUserdata.UpdatedBy = projectMember.UpdatedBy;
                        Context.TblProjectMembers.Update(GetUserdata);
                        Context.SaveChanges();
                        response.Data = GetUserdata;
                        response.Message = "Project member is deactive succesfully";
                    }

                    else
                    {
                        GetUserdata.IsDeleted = true;
                        GetUserdata.UpdatedOn = DateTime.Now;
                        GetUserdata.UpdatedBy = projectMember.UpdatedBy;
                        Context.TblProjectMembers.Update(GetUserdata);
                        Context.SaveChanges();
                        response.Data = GetUserdata;
                        response.Message = "Project member is active succesfully";
                    }
                }

            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in active-deactive the project member";
            }
            return response;
        }


        public async Task<UserResponceModel> DeleteProjectDocument(Guid DocumentId)
        {
            UserResponceModel response = new UserResponceModel();
            var GetDocumentdata = Context.TblProjectDocuments.Where(a => a.Id == DocumentId).FirstOrDefault();
            if (GetDocumentdata != null)
            {
                Context.TblProjectDocuments.Remove(GetDocumentdata);
                Context.SaveChanges();
                response.Message = "Project document deleted successfully.";
            }
            else
            {
                response.Code = (int)HttpStatusCode.NotFound;
                response.Message = "There is some problem in your request!";
            }
            return response;
        }

        public async Task<IEnumerable<ProjectDetailView>> GetProjectsList()
        {
            try
            {
                IEnumerable<ProjectDetailView> ProjectList = Context.TblProjectMasters.Select(a => new ProjectDetailView
                {
                    ProjectId = a.ProjectId,
                    ProjectTitle = a.ProjectTitle,
                    ShortName = a.ShortName,
                }).ToList();
                return ProjectList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<UserResponceModel> UpdateProjectDetails(ProjectDetailView updateProject)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var projectData = await Context.TblProjectMasters.FirstOrDefaultAsync(a => a.ProjectId == updateProject.ProjectId);
                if (projectData != null)
                {
                    projectData.ProjectTitle = updateProject.ProjectTitle;
                    projectData.ShortName = updateProject.ShortName;
                    projectData.ProjectDescription = updateProject.ProjectDescription;
                    projectData.ProjectPath = updateProject.ProjectPath;
                    projectData.ProjectPriority = updateProject.ProjectPriority;
                    projectData.BuildingName = updateProject.BuildingName;
                    projectData.Area = updateProject.Area;
                    projectData.Country = updateProject.Country;
                    projectData.City = updateProject.City;
                    projectData.State = updateProject.State;
                    projectData.PinCode = updateProject.PinCode;
                    projectData.ProjectStatus = updateProject.ProjectStatus;
                    projectData.ProjectDeadline = updateProject.ProjectDeadline;
                    projectData.ProjectEndDate = updateProject.ProjectEndDate;
                    projectData.ProjectStartDate = updateProject.ProjectStartDate;
                    projectData.ProjectType = updateProject.ProjectType;
                    projectData.ProjectHead = updateProject.ProjectHead;
                    projectData.UpdatedBy = updateProject.UpdatedBy;
                    projectData.UpdatedOn = DateTime.Now;
                    projectData.ProjectImage = updateProject.ProjectImage;
                    Context.Update(projectData);
                    await Context.SaveChangesAsync();
                    response.Message = "Project updated succesfully!";
                }
                else
                {
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Message = "Project does not found.";
                }

            }
            catch (Exception ex)
            {
                response.Message = "Error in generating invoice.";
                response.Code = (int)HttpStatusCode.InternalServerError;
            }
            return response;
        }
    }
}
