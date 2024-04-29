using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.Company;
using EMPManegment.Inretface.Interface.CompanyMaster;
using EMPManegment.Inretface.Services.CompanyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.Company
{
    public class CompanyServices : ICompanyServices
    {
        public CompanyServices(ICompany company)
        {
            Company = company;
        }

        public ICompany Company { get; }

        public async Task<CompanyModel> GetCompanyDetailsById(Guid CompanyId)
        {
            return await Company.GetCompanyDetailsById(CompanyId);
        }

        public async Task<IEnumerable<CompanyModel>> GetCompanyNameList()
        {
            return await Company.GetCompanyNameList();
        }

        public async Task<ApiResponseModel> AddCompany(CompanyModel AddCompany)
        {
            return await Company.AddCompany(AddCompany);
        }
    }
}
