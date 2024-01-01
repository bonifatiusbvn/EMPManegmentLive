﻿using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using Newtonsoft.Json;
using X.PagedList;
using X.PagedList.Mvc;

namespace EMPManegment.Web.Controllers
{
    public class ProjectController : Controller
    {
        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }
        public UserSession _userSession { get; }

        public ProjectController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices, UserSession userSession)
        {
            WebAPI = webAPI;
            Environment = environment;
            APIServices = aPIServices;
            _userSession = userSession;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateProject()
        {
            return View();
        }

        public async Task<IActionResult> ProjectList(string? searchby, string? searchfor,int? page) 
        
        {
            try
            {
                List<ProjectDetailView> projectlist = new List<ProjectDetailView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel response = await APIServices.GetAsync("", "ProjectDetails/GetProjectList?searchby="+ searchby + "&searchfor=" + searchfor);
                if (response.code == 200)
                {
                    projectlist = JsonConvert.DeserializeObject<List<ProjectDetailView>>(response.data.ToString());
                }
                var pageProject = projectlist.ToPagedList(page ?? 1, 4);
                return View(pageProject);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IActionResult> GetUserProjectList()
        {
            try
            {
                Guid UserId = _userSession.UserId;
                List<ProjectView> projectlist = new List<ProjectView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel response = await APIServices.PostAsync("", "ProjectDetails/GetUserProjectList?UserId="+ UserId);
                if (response.code == 200)
                {
                    projectlist = JsonConvert.DeserializeObject<List<ProjectView>>(response.data.ToString());
                } 
                return PartialView("~/Views/Project/_DisplayUserProjectList.cshtml", projectlist);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectDetailView project)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync(project, "ProjectDetails/CreateProject");
                UserResponceModel responseModel = new UserResponceModel();
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message });
                }

                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> AddProjectMember(Guid Id)
        {
            try
            {
                ProjectDetailView projectDetails = new ProjectDetailView();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel response = await APIServices.GetAsync("", "ProjectDetails/GetProjectDetailsById?ProjectId=" + Id);
                if (response.code == 200)
                {
                    projectDetails = JsonConvert.DeserializeObject<ProjectDetailView>(response.data.ToString());
                    ViewBag.ProjectId = Id;
                }
                return View(projectDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetMemberList()
        {
            try
            {
                List<EmpDetailsView> MembersList = new List<EmpDetailsView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel postuser = await APIServices.PostAsync("", "ProjectDetails/GetMemberList");
                if (postuser.data != null)
                {
                    MembersList = JsonConvert.DeserializeObject<List<EmpDetailsView>>(postuser.data.ToString());

                }
                else
                {
                    MembersList = new List<EmpDetailsView>();
                    ViewBag.Error = "note found";
                }
                return PartialView("~/Views/Project/_inviteprojectmember.cshtml", MembersList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> InviteMemberToProject()
        {
            try
            { 
                var membersinvited = HttpContext.Request.Form["InviteMember"];
                var memberDetails = JsonConvert.DeserializeObject<ProjectView>(membersinvited);

                ApiResponseModel postuser = await APIServices.PostAsync(memberDetails, "ProjectDetails/AddMemberToProject");
                UserResponceModel responseModel = new UserResponceModel();
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message });
                }

                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> ShowProjectMembers(Guid ProjectId)
        {
            try
            {
                List<ProjectView> ProjectMembersList = new List<ProjectView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel postuser = await APIServices.PostAsync("", "ProjectDetails/GetProjectMember?ProjectId=" + ProjectId);
                if (postuser.data != null)
                {
                    ProjectMembersList = JsonConvert.DeserializeObject<List<ProjectView>>(postuser.data.ToString());
                }
                else
                {
                    ProjectMembersList = new List<ProjectView>();
                    ViewBag.Error = "note found";
                }
                return PartialView("~/Views/Project/_ShowProjectMember.cshtml", ProjectMembersList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> ShowTeam(Guid ProjectId,int? page)
        {
            try
            {
                ViewBag.ProjectId = ProjectId;
                List<ProjectView> ProjectMembersList = new List<ProjectView>();
                HttpClient client = WebAPI.Initil(); 
                ApiResponseModel postuser = await APIServices.PostAsync("", "ProjectDetails/GetProjectMember?ProjectId=" + ProjectId);
                if (postuser.data != null)
                {
                    ProjectMembersList = JsonConvert.DeserializeObject<List<ProjectView>>(postuser.data.ToString());
                }
                else
                {
                    ProjectMembersList = new List<ProjectView>();
                    ViewBag.Error = "note found";
                }
                var pageMembersList = ProjectMembersList.ToPagedList(page ?? 1, 4);
                return PartialView("~/Views/Project/_showTeam.cshtml", pageMembersList);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddDocumentToProject(ProjectRequestModel projectDocument)
        {
            try
            {
                var DocName = Guid.NewGuid() + "_" + projectDocument.DocumentName.FileName;
                var path = Environment.WebRootPath;
                var filepath = "Content/UserDocuments/" + DocName;
                var fullpath = Path.Combine(path, filepath);
                UploadFile(projectDocument.DocumentName, fullpath);
                var uploadDocument = new ProjectDocumentView()
                {
                    ProjectId = projectDocument.ProjectId,
                    UserId = _userSession.UserId,
                    DocumentName = DocName,
                };

                ApiResponseModel postuser = await APIServices.PostAsync(uploadDocument, "ProjectDetails/AddDocumentToProject");
                UserResponceModel responseModel = new UserResponceModel();
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message });
                }

                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UploadFile(IFormFile file, string path)
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);
        }

        [HttpPost]
        public async Task<IActionResult> ShowProjectDocuments(Guid ProjectId)
        {
            try
            {
                List<ProjectDocumentView> ProjectDocumentsList = new List<ProjectDocumentView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel postuser = await APIServices.PostAsync("", "ProjectDetails/GetProjectDocument?ProjectId=" + ProjectId);
                if (postuser.data != null)
                {
                    ProjectDocumentsList = JsonConvert.DeserializeObject<List<ProjectDocumentView>>(postuser.data.ToString());
                }
                else
                {
                    ProjectDocumentsList = new List<ProjectDocumentView>();
                    ViewBag.Error = "note found";
                }
                return PartialView("~/Views/Project/_showProjectDocuments.cshtml", ProjectDocumentsList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> ShowUploadedDocuments(Guid ProjectId)
        {
            try
            {
                List<ProjectDocumentView> ProjectDocumentsList = new List<ProjectDocumentView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel postuser = await APIServices.PostAsync("", "ProjectDetails/GetProjectDocument?ProjectId=" + ProjectId);
                if (postuser.data != null)
                {
                    ProjectDocumentsList = JsonConvert.DeserializeObject<List<ProjectDocumentView>>(postuser.data.ToString());
                }
                else
                {
                    ProjectDocumentsList = new List<ProjectDocumentView>();
                    ViewBag.Error = "note found";
                }
                return PartialView("~/Views/Project/_showUploadDocument.cshtml", ProjectDocumentsList);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public async Task<FileResult> DownloadProjectDocument(string DocumentName)
        {
            var filepath = "Content/UserDocuments/" + DocumentName;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filepath);
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var ContentType = "application/pdf";
            var fileName = Path.GetFileName(path);
            return File(memory, ContentType, fileName);
        }        
    }
}
