using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.Company;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
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

        public async Task<UserResponceModel> UpdateCompanyDetails(CompanyModel updateCompany)
        {
            return await Company.UpdateCompanyDetails(updateCompany);
        }
        public async Task<UserResponceModel> DeleteCompanyDetails(Guid CompanyId)
        {
            return await Company.DeleteCompanyDetails(CompanyId);
        }
        public async Task<jsonData> GetDatatableCompanyList(DataTableRequstModel CompanydataTable)
        {
            return await Company.GetDatatableCompanyList(CompanydataTable);
        }
    }
}
