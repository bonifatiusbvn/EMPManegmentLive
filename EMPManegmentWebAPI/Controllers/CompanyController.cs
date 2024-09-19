using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.Company;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Services.CompanyServices;
using EMPManegment.Services.Company;
using EMPManegment.Services.VendorDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
#nullable disable
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


        [HttpPost]
        [Route("UpdateCompanyDetails")]
        public async Task<IActionResult> UpdateCompanyDetails(CompanyModel updateCompany)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var result = await Company.UpdateCompanyDetails(updateCompany);
                if (result.Code != (int)HttpStatusCode.InternalServerError)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = result.Message;
                }
                else
                {
                    response.Message = result.Message;
                    response.Code = (int)HttpStatusCode.NotFound;
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
        [Route("DeleteCompanyDetails")]
        public async Task<IActionResult> DeleteCompanyDetails(Guid CompanyId)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var result = await Company.DeleteCompanyDetails(CompanyId);
                if (result.Code != (int)HttpStatusCode.InternalServerError)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = result.Message;
                }
                else
                {
                    response.Message = result.Message;
                    response.Code = (int)HttpStatusCode.NotFound;
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
        [Route("GetDatatableCompanyList")]
        public async Task<IActionResult> GetDatatableCompanyList(DataTableRequstModel CompanydataTable)
        {
            var GetCompanyList = await Company.GetDatatableCompanyList(CompanydataTable);
            return Ok(new { code = (int)HttpStatusCode.OK, data = GetCompanyList });
        }
    }
}
