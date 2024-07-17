using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.UserModels;
using EMPManegment.Inretface.Interface.CSC;
using EMPManegment.Services.CSC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
#nullable disable
namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]

    public class MasterListController : ControllerBase
    {
        public MasterListController(IMasterList cSC)
        {
            CSC = cSC;
        }

        public IMasterList CSC { get; }

        [HttpGet]
        [Route("GetCountries")]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                IEnumerable<CountryView> getCountries = await CSC.GetCountries();
                return Ok(new { code = (int)HttpStatusCode.OK, data = getCountries.ToList() });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }



        [HttpGet]
        [Route("GetState")]
        public async Task<IActionResult> GetState(int StateId)
        {
            try
            {
                IEnumerable<StateView> getStates = await CSC.GetStates(StateId);
                return Ok(new { code = (int)HttpStatusCode.OK, data = getStates.ToList() });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpGet]
        [Route("GetCities")]
        public async Task<IActionResult> GetCities(int CityId)
        {
            try
            {
                IEnumerable<CityView> getCities = await CSC.GetCities(CityId);
                return Ok(new { code = (int)HttpStatusCode.OK, data = getCities.ToList() });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpGet]
        [Route("GetQuestion")]
        public async Task<IActionResult> GetQuestion()
        {
            try
            {
                IEnumerable<QuestionView> getQuestion = await CSC.GetQuestion();
                return Ok(new { code = (int)HttpStatusCode.OK, data = getQuestion.ToList() });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpGet]
        [Route("GetDepartment")]
        public async Task<IActionResult> GetDepartment()
        {
            try
            {
                IEnumerable<Department> getDepartment = await CSC.GetDepartment();
                return Ok(new { code = (int)HttpStatusCode.OK, data = getDepartment.ToList() });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpPost]
        [Route("GetUserRoleList")]
        public async Task<IActionResult> GetUserRoleList()
        {
            try
            {
                IEnumerable<UserRoleModel> userRole = await CSC.GetUserRole();
                return Ok(new { code = (int)HttpStatusCode.OK, data = userRole.ToList() });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpGet]
        [Route("GetAllCities")]
        public async Task<IActionResult> GetAllCities()
        {
            try
            {
                IEnumerable<CityView> AllCities = await CSC.GetAllCities();
                return Ok(new { code = (int)HttpStatusCode.OK, data = AllCities.ToList() });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }
    }
}
