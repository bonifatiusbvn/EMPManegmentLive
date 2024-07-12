using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.Company;
using EMPManegment.Inretface.Services.CompanyServices;
using EMPManegment.Services.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        public CompanyController(ICompanyServices company)
        {
            Company = company;
        }

        private readonly ICompanyServices Company;

        [HttpGet]
        [Route("GetCompanyNameList")]
        public async Task<IActionResult> GetCompanyNameList()
        {
            try
            {
                IEnumerable<CompanyModel> company = await Company.GetCompanyNameList();
                return Ok(new { code = (int)HttpStatusCode.OK, data = company.ToList() });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpGet]
        [Route("GetCompanyDetailsById")]
        public async Task<IActionResult> GetCompanyDetailsById(Guid Id)
        {
            try
            {
                var companyDetails = await Company.GetCompanyDetailsById(Id);
                return Ok(new { code = (int)HttpStatusCode.OK, data = companyDetails });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpPost]
        [Route("AddCompany")]
        public async Task<IActionResult> AddCompany(CompanyModel AddCompany)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
                var result = await Company.AddCompany(AddCompany);
                if (result.code != (int)HttpStatusCode.InternalServerError)
                {
                    response.code = (int)HttpStatusCode.OK;
                    response.message = result.message;
                }
                else
                {
                    response.message = result.message;
                    response.code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                response.code = (int)HttpStatusCode.InternalServerError;
                response.message = "An error occurred while processing the request.";
            }
            return StatusCode(response.code, response);
        }
    }
}
