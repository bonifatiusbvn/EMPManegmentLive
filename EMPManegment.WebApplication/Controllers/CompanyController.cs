using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.Company;
using EMPManegment.Web.Helper;
using EMPManegment.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
#nullable disable
namespace EMPManegment.Web.Controllers
{
    public class CompanyController : Controller
    {
        public CompanyController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices, UserSession userSession)
        {
            WebAPI = webAPI;
            Environment = environment;
            APIServices = aPIServices;
            UserSession = userSession;
        }

        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }
        public UserSession UserSession { get; }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetCompanyNameList()
        {
            try
            {
                List<CompanyModel> CompanyName = new List<CompanyModel>();
                ApiResponseModel res = await APIServices.GetAsync("", "Company/GetCompanyNameList");
                if (res.code == 200)
                {
                    CompanyName = JsonConvert.DeserializeObject<List<CompanyModel>>(res.data.ToString());
                }
                return new JsonResult(CompanyName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetCompanyDetailsById(Guid CompanyId)
        {
            try
            {
                CompanyModel company = new CompanyModel();
                ApiResponseModel response = await APIServices.GetAsync("", "Company/GetCompanyDetailsById?Id=" + CompanyId);
                if (response.code == 200)
                {
                    company = JsonConvert.DeserializeObject<CompanyModel>(response.data.ToString());
                }
                return new JsonResult(company);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
