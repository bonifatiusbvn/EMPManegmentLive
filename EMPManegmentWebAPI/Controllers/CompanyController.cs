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
            IEnumerable<CompanyModel> company = await Company.GetCompanyNameList();
            return Ok(new { code = 200, data = company.ToList() });
        }

        [HttpGet]
        [Route("GetCompanyDetailsById")]
        public async Task<IActionResult> GetCompanyDetailsById(Guid Id)
        {
            var companyDetails = await Company.GetCompanyDetailsById(Id);
            return Ok(new { code = 200, data = companyDetails });
        }

        [HttpPost]
        [Route("AddCompany")]
        public async Task<IActionResult> AddCompany(CompanyModel AddCompany)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
                var result = await Company.AddCompany(AddCompany);
                if (result.code == 200)
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
