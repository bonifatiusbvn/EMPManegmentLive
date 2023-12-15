using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.Inretface.Interface.ProjectDetails;
using EMPManegment.Inretface.Services.ProjectDetailsServices;
using EMPManegment.Inretface.Services.TaskServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using System.Net;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                    response.Code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Route("GetProjectList")]
        public async Task<IActionResult> GetProjectList(string? searchby, string? searchfor)
        {
                IEnumerable<ProjectDetailView> projectlist = await ProjectDetail.GetProjectList(searchby,searchfor);
                return Ok(new { code = 200, data = projectlist.ToList() });
        }

        [HttpGet]
        [Route("GetProjectDetailsById")]
        public async Task<IActionResult> GetProjectDetailsById(Guid ProjectId)
        {
            var Projectdata = await ProjectDetail.GetProjectDetailsById(ProjectId);
            return Ok(new { code = 200,data =  Projectdata });
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
                    response.Code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("GetProjectMember")]
        public async Task<IActionResult> GetProjectMember(Guid ProjectId)
        {
            IEnumerable<ProjectView> Members = await ProjectDetail.GetProjectMember(ProjectId);
            return Ok(new { code = 200, data = Members.ToList()});
        }
    }
}
