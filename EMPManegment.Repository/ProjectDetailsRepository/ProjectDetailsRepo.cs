using Azure;
using EMPManagment.API;
using EMPManegment.EntityModels.Common;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.AGGridModels;
using EMPManegment.EntityModels.ViewModels.Company;
using EMPManegment.EntityModels.ViewModels.FormPermissionMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
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

        public async Task<IEnumerable<ProjectDetailView>> GetProjectList(ProjectRequest projectRequest)
        {
            try
            {
                string dbConnectionStr = _configuration.GetConnectionString("EMPDbconn");

                var parameters = new SqlParameter[]
                {
            new SqlParameter("@SearchValue", string.IsNullOrWhiteSpace(projectRequest.SearchValue) ? DBNull.Value : projectRequest.SearchValue),
            new SqlParameter("@ProjectStatus", string.IsNullOrWhiteSpace(projectRequest.ProjectStatus) ? DBNull.Value : projectRequest.ProjectStatus),
            new SqlParameter("@ProjectPriority", string.IsNullOrWhiteSpace(projectRequest.Projectpriority) ? DBNull.Value : projectRequest.Projectpriority),
            new SqlParameter("@FromDate", projectRequest.FromDate.HasValue ? projectRequest.FromDate.Value : (object)DBNull.Value),
            new SqlParameter("@ToDate", projectRequest.ToDate.HasValue ? projectRequest.ToDate.Value : (object)DBNull.Value)
                };

                var ds = DbHelper.GetDataSet("GetProjectList", CommandType.StoredProcedure, parameters, dbConnectionStr);
                var projectList = new List<ProjectDetailView>();

                if (ds?.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var project = new ProjectDetailView
                        {
                            ProjectId = row["ProjectId"] != DBNull.Value ? (Guid)row["ProjectId"] : Guid.Empty,
                            ProjectType = row["ProjectType"]?.ToString(),
                            ProjectTitle = row["ProjectTitle"]?.ToString(),
                            ShortName = row["ShortName"]?.ToString(),
                            ProjectHead = row["ProjectHead"]?.ToString(),
                            ProjectDescription = row["ProjectDescription"]?.ToString(),
                            BuildingName = row["BuildingName"]?.ToString(),
                            Area = row["Area"]?.ToString(),
                            City = row["City"] != DBNull.Value ? Convert.ToInt32(row["City"]) : 0,
                            State = row["State"] != DBNull.Value ? Convert.ToInt32(row["State"]) : 0,
                            Country = row["Country"] != DBNull.Value ? Convert.ToInt32(row["Country"]) : 0,
                            ProjectImage = row["ProjectImage"]?.ToString(),
                            ProjectPath = row["ProjectPath"]?.ToString(),
                            ProjectPriority = row["ProjectPriority"]?.ToString(),
                            ProjectStatus = row["ProjectStatus"]?.ToString(),
                            ProjectStartDate = row["ProjectStartDate"] != DBNull.Value ? Convert.ToDateTime(row["ProjectStartDate"]) : DateTime.MinValue,
                            ProjectEndDate = row["ProjectEndDate"] != DBNull.Value ? Convert.ToDateTime(row["ProjectEndDate"]) : DateTime.MinValue,
                            ProjectDeadline = row["ProjectDeadline"] != DBNull.Value ? Convert.ToDateTime(row["ProjectDeadline"]) : DateTime.MinValue,
                            CreatedOn = row["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(row["CreatedOn"]) : (DateTime?)null
                        };

                        projectList.Add(project);
                    }
                }

                return projectList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<ProjectView>> GetProjectNameList(string? search)
        {
            try
            {
                var query = Context.TblProjectMasters
                    .Select(a => new ProjectView
                    {
                        Id = a.ProjectId,
                        ProjectTitle = a.ProjectTitle + "-" + a.ShortName
                    });

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(pt => pt.ProjectTitle.Contains(search));
                }
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<ProjectView>> GetUserProjectList(Guid UserId)
        {
            try
            {
                string dbConnectionStr = _configuration.GetConnectionString("EMPDbconn");

                var sqlPar = new SqlParameter[]
                {
                   new SqlParameter("@UserId", UserId),
                };

                var DS = DbHelper.GetDataSet("GetProjectListByUserId", CommandType.StoredProcedure, sqlPar, dbConnectionStr);

                List<ProjectView> ProjectList = new List<ProjectView>();

                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        var projectDetails = new ProjectView
                        {
                            Id = row["Id"] != DBNull.Value ? (Guid)row["Id"] : Guid.Empty,
                            ProjectId = row["ProjectId"] != DBNull.Value ? (Guid)row["ProjectId"] : Guid.Empty,
                            ProjectType = row["ProjectType"]?.ToString(),
                            ProjectTitle = row["ProjectTitle"]?.ToString(),
                            Status = row["ProjectStatus"]?.ToString(),
                            ProjectDescription = row["ProjectDescription"]?.ToString(),
                            Image = row["ProjectImage"]?.ToString(),
                            ShortName = row["ShortName"]?.ToString(),
                            UserId = row["UserId"] != DBNull.Value ? (Guid)row["UserId"] : Guid.Empty,
                            StartDate = row["ProjectStartDate"] != DBNull.Value ? (DateTime)row["ProjectStartDate"] : DateTime.MinValue,
                            EndDate = row["ProjectEndDate"] != DBNull.Value ? (DateTime)row["ProjectEndDate"] : DateTime.MinValue,
                            CreatedOn = row["CreatedOn"] != DBNull.Value ? (DateTime)row["CreatedOn"] : DateTime.MinValue
                        };
                        ProjectList.Add(projectDetails);
                    }
                }
                return ProjectList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            try
            {
                string dbConnectionStr = _configuration.GetConnectionString("EMPDbconn");

                var DS = DbHelper.GetDataSet("GetActiveDeactiveUserList", CommandType.StoredProcedure, new SqlParameter[] { }, dbConnectionStr);

                List<EmpDetailsView> UserList = new List<EmpDetailsView>();

                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        var userDetails = new EmpDetailsView
                        {
                            Id = row["Id"] != DBNull.Value ? (Guid)row["Id"] : Guid.Empty,
                            Image = row["Image"]?.ToString(),
                            FirstName = row["FirstName"]?.ToString(),
                            LastName = row["LastName"]?.ToString(),
                        };
                        UserList.Add(userDetails);
                    }
                }
                return UserList.Take(10);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserResponceModel> AddMemberToProject(ProjectMemberMasterView AddMember)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var project = await Context.TblProjectMasters.FirstOrDefaultAsync(e => e.ProjectId == AddMember.ProjectId);

                if (project != null)
                {
                    if (AddMember.ProjectMemberList.Count > 0)
                    {
                        List<string> alreadyExistMembers = new List<string>();

                        foreach (var item in AddMember.ProjectMemberList)
                        {
                            var userId = await Context.TblUsers.FirstOrDefaultAsync(a => (a.FirstName + " " + a.LastName) == item.Fullname);
                            if (userId == null) continue;

                            bool isMemberAlreadyExists = Context.TblProjectMembers.Any(x => x.UserId == userId.Id && x.ProjectId == AddMember.ProjectId);

                            if (isMemberAlreadyExists)
                            {
                                var projectDetail = await Context.TblProjectMembers.SingleOrDefaultAsync(x => x.UserId == userId.Id && x.ProjectId == AddMember.ProjectId);

                                if (projectDetail.IsDeleted == false)
                                {
                                    alreadyExistMembers.Add(item.Fullname);
                                }
                                else
                                {
                                    projectDetail.IsDeleted = false;
                                    projectDetail.ProjectId = project.ProjectId;
                                    projectDetail.ProjectDesignation = userId.Designation;
                                    projectDetail.UpdatedOn = DateTime.Now;
                                    projectDetail.UpdatedBy = AddMember.UpdatedBy;
                                    Context.TblProjectMembers.Update(projectDetail);
                                    await Context.SaveChangesAsync();
                                }
                            }
                            else
                            {
                                var projectmodel = new TblProjectMember()
                                {
                                    Id = Guid.NewGuid(),
                                    ProjectId = project.ProjectId,
                                    UserId = userId.Id,
                                    ProjectDesignation = userId.Designation,
                                    IsDeleted = false,
                                    CreatedBy = AddMember.UpdatedBy,
                                    CreatedOn = DateTime.Now,
                                };

                                Context.TblProjectMembers.Add(projectmodel);
                                await Context.SaveChangesAsync();
                            }
                        }

                        response.Data = project;

                        if (alreadyExistMembers.Count > 0)
                        {
                            response.Code = (int)HttpStatusCode.PartialContent;
                            response.Message = string.Join(", ", alreadyExistMembers) + " already exist(s) in the project.";
                        }
                        else
                        {
                            response.Code = (int)HttpStatusCode.OK;
                            response.Message = "All project members were added successfully!";
                        }
                    }
                    else
                    {
                        response.Code = (int)HttpStatusCode.BadRequest;
                        response.Message = "Select members you want to add.";
                    }
                }
                else
                {
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Message = "Project not found.";
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in adding member(s) to the project.";
            }
            return response;
        }

        public async Task<AGGridResponseModel<ProjectView>> GetProjectMember(AGGridRequestModel ProjectMemberRequest)
        {
            try
            {
                var filterConditions = string.Join(" AND ", ProjectMemberRequest.filters.Select(f =>
                                    $"{f.ColId} LIKE '%{f.FilterValue}%'"));
                string sortColumn = ProjectMemberRequest.SortModel?.FirstOrDefault()?.ColId ?? "FirstName";
                string sortDirection = ProjectMemberRequest.SortModel?.FirstOrDefault()?.Sort ?? "asc";

                var parameters = new List<SqlParameter>
                {
                new SqlParameter("@ProjectId", (object)ProjectMemberRequest.ProjectFilter ?? DBNull.Value),
                new SqlParameter("@SortColumn", sortColumn),
                new SqlParameter("@SortDirection", sortDirection),
                new SqlParameter("@PageSize", ProjectMemberRequest.PageSize),
                new SqlParameter("@Skip", ProjectMemberRequest.StartRow),
                new SqlParameter("@FilterConditions", (object)filterConditions ?? DBNull.Value),
                new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                var dataSet = DbHelper.GetDataSet("GetProjectMember", CommandType.StoredProcedure, parameters.ToArray(), _configuration.GetConnectionString("EMPDbconn"));

                var ProjectList = dataSet.Tables[0].AsEnumerable().Select(row => new ProjectView
                {
                    Id = row["Id"] != DBNull.Value ? (Guid)row["Id"] : Guid.Empty,
                    ProjectId = row["ProjectId"] != DBNull.Value ? (Guid)row["ProjectId"] : Guid.Empty,
                    FirstName = row["FirstName"]?.ToString(),
                    LastName = row["LastName"]?.ToString(),
                    Image = row["Image"]?.ToString(),
                    UserId = row["UserId"] != DBNull.Value ? (Guid)row["UserId"] : Guid.Empty,
                    Designation = row["ProjectDesignation"]?.ToString(),
                }).ToList();

                int totalRecords = (int)parameters.First(p => p.ParameterName == "@TotalRecords").Value;

                return new AGGridResponseModel<ProjectView>
                {
                    Data = ProjectList,
                    RecordsTotal = totalRecords
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the project member.", ex);
            }
        }
        public async Task<List<ProjectView>> ShowProjectMemberList(Guid ProjectId)
        {
            try
            {
                var ProjectMemberList = (from a in Context.TblProjectMembers
                                         join b in Context.TblProjectMasters on a.ProjectId equals b.ProjectId
                                         join c in Context.TblUsers on a.UserId equals c.Id
                                         where a.IsDeleted != true && a.ProjectId == ProjectId
                                         select new ProjectView
                                         {
                                             Id = a.Id,
                                             ProjectId = a.ProjectId,
                                             Fullname = c.FirstName + " " + c.LastName,
                                             FirstName = c.FirstName,
                                             LastName = c.LastName,
                                             Image = c.Image,
                                             UserId = a.UserId,
                                             Designation = a.ProjectDesignation,
                                             ProjectTitle = b.ProjectTitle
                                         }).ToList();

                return ProjectMemberList;
            }
            catch (Exception)
            {
                throw;
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

        public async Task<AGGridResponseModel<ProjectDocumentView>> GetProjectDocument(AGGridRequestModel ProjectDocumentRequest)
        {
            try
            {
                var filterConditions = string.Join(" AND ", ProjectDocumentRequest.filters.Select(f =>
                                    $"{f.ColId} LIKE '%{f.FilterValue}%'"));
                string sortColumn = ProjectDocumentRequest.SortModel?.FirstOrDefault()?.ColId ?? "FirstName";
                string sortDirection = ProjectDocumentRequest.SortModel?.FirstOrDefault()?.Sort ?? "asc";

                var parameters = new List<SqlParameter>
                {
                new SqlParameter("@ProjectId", (object)ProjectDocumentRequest.ProjectFilter ?? DBNull.Value),
                new SqlParameter("@SortColumn", sortColumn),
                new SqlParameter("@SortDirection", sortDirection),
                new SqlParameter("@PageSize", ProjectDocumentRequest.PageSize),
                new SqlParameter("@Skip", ProjectDocumentRequest.StartRow),
                new SqlParameter("@FilterConditions", (object)filterConditions ?? DBNull.Value),
                new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                var dataSet = DbHelper.GetDataSet("GetProjectDocument", CommandType.StoredProcedure, parameters.ToArray(), _configuration.GetConnectionString("EMPDbconn"));

                var ProjectDocumentList = dataSet.Tables[0].AsEnumerable().Select(row => new ProjectDocumentView
                {
                    Id = row["Id"] != DBNull.Value ? (Guid)row["Id"] : Guid.Empty,
                    ProjectId = row["ProjectId"] != DBNull.Value ? (Guid)row["ProjectId"] : Guid.Empty,
                    DocumentName = row["DocumentName"]?.ToString(),
                    Date = row["Date"] != DBNull.Value ? (DateTime)row["Date"] : DateTime.MinValue,
                    FirstName = row["FirstName"]?.ToString(),
                    LastName = row["LastName"]?.ToString(),
                }).ToList();

                int totalRecords = (int)parameters.First(p => p.ParameterName == "@TotalRecords").Value;

                return new AGGridResponseModel<ProjectDocumentView>
                {
                    Data = ProjectDocumentList,
                    RecordsTotal = totalRecords
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the project member.", ex);
            }
        }
        public async Task<List<ProjectDocumentView>> ShowProjectDocumentList(Guid ProjectId)
        {
            try
            {
                var ProjectDocumentList = (from a in Context.TblProjectDocuments
                                           join c in Context.TblUsers on a.UserId equals c.Id
                                           where a.ProjectId == ProjectId
                                           select new ProjectDocumentView
                                           {
                                               Id = a.Id,
                                               ProjectId = a.ProjectId,
                                               FullName = c.FirstName + " " + c.LastName,
                                               FirstName = c.FirstName,
                                               LastName = c.LastName,
                                               DocumentName = a.DocumentName,
                                               Date = a.Date
                                           }).ToList();

                return ProjectDocumentList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<ProjectDetailView>> GetProjectListById(string? searchby, string? searchfor, Guid UserId)
        {
            try
            {
                string dbConnectionStr = _configuration.GetConnectionString("EMPDbconn");

                var sqlPar = new SqlParameter[]
                {
                   new SqlParameter("@UserId", UserId),
                };

                var DS = DbHelper.GetDataSet("GetProjectListByUserId", CommandType.StoredProcedure, sqlPar, dbConnectionStr);

                List<ProjectDetailView> ProjectList = new List<ProjectDetailView>();

                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        var projectDetails = new ProjectDetailView
                        {
                            Id = row["Id"] != DBNull.Value ? (Guid)row["Id"] : Guid.Empty,
                            ProjectId = row["ProjectId"] != DBNull.Value ? (Guid)row["ProjectId"] : Guid.Empty,
                            ProjectType = row["ProjectType"]?.ToString(),
                            ProjectTitle = row["ProjectTitle"]?.ToString(),
                            ProjectStatus = row["ProjectStatus"]?.ToString(),
                            ProjectDescription = row["ProjectDescription"]?.ToString(),
                            ProjectImage = row["ProjectImage"]?.ToString(),
                            ShortName = row["ShortName"]?.ToString(),
                            UserId = row["UserId"] != DBNull.Value ? (Guid)row["UserId"] : Guid.Empty,
                            ProjectStartDate = row["ProjectStartDate"] != DBNull.Value ? (DateTime)row["ProjectStartDate"] : DateTime.MinValue,
                            ProjectEndDate = row["ProjectEndDate"] != DBNull.Value ? (DateTime)row["ProjectEndDate"] : DateTime.MinValue,
                            ProjectDeadline = row["ProjectDeadline"] != DBNull.Value ? (DateTime)row["ProjectDeadline"] : DateTime.MinValue,
                            CreatedOn = row["CreatedOn"] != DBNull.Value ? (DateTime)row["CreatedOn"] : DateTime.MinValue
                        };
                        ProjectList.Add(projectDetails);
                    }
                }

                if (searchby == "ProjectTitle" && searchfor != null)
                {
                    ProjectList = ProjectList.Where(ser => ser.ProjectTitle.ToLower().Contains(searchfor.ToLower())).ToList();
                }
                if (searchby == "ProjectStatus" && searchfor != null)
                {
                    ProjectList = ProjectList.Where(ser => ser.ProjectStatus.ToLower().Contains(searchfor.ToLower())).ToList();
                }
                return ProjectList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                    GetUserdata.IsDeleted = true;
                    GetUserdata.UpdatedOn = DateTime.Now;
                    GetUserdata.UpdatedBy = projectMember.UpdatedBy;
                    Context.TblProjectMembers.Update(GetUserdata);
                    Context.SaveChanges();
                    response.Data = GetUserdata;
                    response.Message = "Project member is deleted succesfully";
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
                string dbConnectionStr = _configuration.GetConnectionString("EMPDbconn");

                var DS = DbHelper.GetDataSet("GetProjectList", CommandType.StoredProcedure, new SqlParameter[] { }, dbConnectionStr);

                List<ProjectDetailView> projectList = new List<ProjectDetailView>();

                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        var projectDetails = new ProjectDetailView
                        {
                            ProjectId = row["ProjectId"] != DBNull.Value ? (Guid)row["ProjectId"] : Guid.Empty,
                            ProjectTitle = row["ProjectTitle"]?.ToString(),
                            ShortName = row["ShortName"]?.ToString(),
                        };
                        projectList.Add(projectDetails);
                    }
                }
                return projectList;
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

        public async Task<ProjectMemberUpdate> EditProjectMemberDesignation(Guid ProjectMemberId)
        {
            try
            {
                var ProjectMember = Context.TblProjectMembers.Where(e => e.Id == ProjectMemberId).FirstOrDefault();
                var ProjectMemberDetails = new ProjectMemberUpdate()
                {
                    Id = ProjectMember.Id,
                    UserId = ProjectMember.UserId,
                    ProjectId = ProjectMember.ProjectId,
                    ProjectDesignation = ProjectMember.ProjectDesignation,
                };
                return ProjectMemberDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserResponceModel> UpdateProjectMemberDesignation(ProjectMemberMasterView UpdateDesignation)
        {
            var response = new UserResponceModel();
            try
            {
                var ProjectMember = Context.TblProjectMembers.Where(e => e.ProjectId == UpdateDesignation.ProjectId && e.UserId == UpdateDesignation.UserId).FirstOrDefault();
                if (ProjectMember == null)
                {
                    response.Code = 400;
                    response.Message = "Error in updating project designation!";
                }
                else
                {
                    ProjectMember.ProjectDesignation = UpdateDesignation.ProjectDesignation;
                    ProjectMember.UpdatedOn = DateTime.Now;
                    ProjectMember.UpdatedBy = UpdateDesignation.UpdatedBy;

                    Context.TblProjectMembers.Update(ProjectMember);
                    Context.SaveChanges();

                    response.Code = 200;
                    response.Message = "Project member designation updated successfully.";
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProjectActivityDetailsModel>> GetProjectActivityDetails(Guid ProjectId)
        {
            try
            {
                string dbConnectionStr = _configuration.GetConnectionString("EMPDbconn");

                var sqlPar = new SqlParameter[]
                {
                   new SqlParameter("@ProjectId", ProjectId),
                };

                var DS = DbHelper.GetDataSet("GetProjectActivityList", CommandType.StoredProcedure, sqlPar, dbConnectionStr);

                List<ProjectActivityDetailsModel> ProjectActivityList = new List<ProjectActivityDetailsModel>();

                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        var ProjectActivities = new ProjectActivityDetailsModel
                        {
                            RecordId = row["RecordId"] != DBNull.Value ? (Guid)row["RecordId"] : Guid.Empty,
                            RecordType = row["RecordType"]?.ToString(),
                            Title = row["Title"]?.ToString(),
                            TaskType = row["TaskType"]?.ToString(),
                            TaskStatus = row["TaskStatus"]?.ToString(),
                            TaskDate = row["TaskDate"] != DBNull.Value ? (DateTime)row["TaskDate"] : DateTime.MinValue,
                            TaskEndDate = row["TaskEndDate"] != DBNull.Value ? (DateTime)row["TaskEndDate"] : DateTime.MinValue,
                            TaskDetails = row["TaskDetails"]?.ToString(),
                            UserName = row["UserName"]?.ToString(),
                            FirstName = row["FirstName"]?.ToString(),
                            LastName = row["LastName"]?.ToString(),
                            Image = row["Image"]?.ToString(),
                            ShippingAddress = row["ShippingAddress"]?.ToString(),
                        };
                        ProjectActivityList.Add(ProjectActivities);
                    }
                }
                return ProjectActivityList;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the project member.", ex);
            }
        }
    }
}
