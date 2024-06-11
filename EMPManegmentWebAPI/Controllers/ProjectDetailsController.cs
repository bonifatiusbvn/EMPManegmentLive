using Azure;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.Inretface.Interface.OrderDetails;
using EMPManegment.Inretface.Interface.ProjectDetails;
using EMPManegment.Inretface.Services.ProjectDetailsServices;
using EMPManegment.Inretface.Services.TaskServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using System.Net;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectDetailsController : ControllerBase
    {
        public IProjectDetailServices ProjectDetail { get; }

        public ProjectDetailsController(IProjectDetailServices projectDetail)
        {
            ProjectDetail = projectDetail;
        }

        [HttpPost]
        [Route("CreateProject")]
        public async Task<IActionResult> CreateProject(ProjectDetailView project)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var result = ProjectDetail.CreateProject(project);
                if (result.Result.Code == 200)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = result.Result.Message;
                }
                else
                {
                    response.Message = result.Result.Message;
                    response.Code = result.Result.Code;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "An error occurred while processing the request.";
            }
            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Route("GetProjectList")]
        public async Task<IActionResult> GetProjectList(string? searchby, string? searchfor)
        {
            IEnumerable<ProjectDetailView> projectlist = await ProjectDetail.GetProjectList(searchby, searchfor);
            return Ok(new { code = 200, data = projectlist.ToList() });
        }

        [HttpPost]
        [Route("GetUserProjectList")]
        public async Task<IActionResult> GetUserProjectList(Guid UserId)
        {
            List<ProjectView> userprojectList = await ProjectDetail.GetUserProjectList(UserId);
            return Ok(new { code = 200, data = userprojectList.ToList() });
        }

        [HttpGet]
        [Route("GetProjectDetailsById")]
        public async Task<IActionResult> GetProjectDetailsById(Guid ProjectId)
        {
            var Projectdata = await ProjectDetail.GetProjectDetailsById(ProjectId);
            return Ok(new { code = 200, data = Projectdata });
        }

        [HttpPost]
        [Route("GetMemberList")]
        public async Task<IActionResult> GetAllMembers()
        {
            IEnumerable<EmpDetailsView> emplist = await ProjectDetail.GetAllMembers();
            return Ok(new { code = 200, data = emplist.ToList() });
        }

        [HttpPost]
        [Route("AddMemberToProject")]
        public async Task<IActionResult> AddMemberToProject(ProjectView AddMember)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var result = ProjectDetail.AddMemberToProject(AddMember);
                if (result.Result.Code == 200)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = result.Result.Message;
                }
                else
                {
                    response.Message = result.Result.Message;
                    response.Code = result.Result.Code;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "An error occurred while processing the request.";
            }
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("GetProjectMember")]
        public async Task<IActionResult> GetProjectMember(Guid ProjectId)
        {
            IEnumerable<ProjectView> Members = await ProjectDetail.GetProjectMember(ProjectId);
            return Ok(new { code = 200, data = Members.ToList() });
        }

        [HttpPost]
        [Route("AddDocumentToProject")]
        public async Task<IActionResult> AddDocumentToProject(ProjectDocumentView AddDocument)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var result = ProjectDetail.AddDocumentToProject(AddDocument);
                if (result.Result.Code == 200)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = result.Result.Message;
                }
                else
                {
                    response.Message = result.Result.Message;
                    response.Code = result.Result.Code;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "An error occurred while processing the request.";
            }
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("GetProjectDocument")]
        public async Task<IActionResult> GetProjectDocument(Guid ProjectId)
        {
            IEnumerable<ProjectDocumentView> Documents = await ProjectDetail.GetProjectDocument(ProjectId);
            return Ok(new { code = 200, data = Documents.ToList() });
        }

        [HttpPost]
        [Route("GetProjectListById")]
        public async Task<IActionResult> GetProjectListById(string? searchby, string? searchfor, Guid UserId)
        {
            List<ProjectDetailView> projectlist = await ProjectDetail.GetProjectListById(searchby, searchfor, UserId);
            return Ok(new { code = 200, data = projectlist.ToList() });
        }

        [HttpGet]
        [Route("CheckProjectName")]
        public IActionResult CheckProjectName()
        {
            var checkProject = ProjectDetail.CheckProjectName();
            return Ok(new { code = 200, data = checkProject });
        }

        [HttpPost]
        [Route("IsDeletedMember")]
        public async Task<IActionResult> IsDeletedMember(ProjectMemberUpdate projectMember)
        {
            UserResponceModel responseModel = new UserResponceModel();
            try
            {

                var Member = await ProjectDetail.MemberIsDeleted(projectMember);
                if (Member.Code == 200)
                {

                    responseModel.Code = (int)HttpStatusCode.OK;
                    responseModel.Message = Member.Message;
                }
                else
                {
                    responseModel.Message = Member.Message;
                    responseModel.Code = Member.Code;
                }
            }
            catch (Exception ex)
            {
                responseModel.Code = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = "An error occurred while processing the request.";
            }
            return StatusCode(responseModel.Code, responseModel);
        }
        [HttpPost]
        [Route("DeleteProjectDocument")]
        public async Task<IActionResult> DeleteProjectDocument(Guid DocumentId)
        {
            UserResponceModel responseModel = new UserResponceModel();

            var Member = await ProjectDetail.DeleteProjectDocument(DocumentId);
            if(Member.Code == 200)
            {
                responseModel.Code = Member.Code;
                responseModel.Message=Member.Message;
            }
            else
            {
                responseModel.Code = Member.Code;
                responseModel.Message = Member.Message;
            }
            return StatusCode(responseModel.Code, responseModel);
        }
    }
}
