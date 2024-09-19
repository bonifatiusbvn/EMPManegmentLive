using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.Company;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Interface.CompanyMaster
{
    public interface ICompany
    {
        Task<IEnumerable<CompanyModel>> GetCompanyNameList();
        Task<CompanyModel> GetCompanyDetailsById(Guid CompanyId);
        Task<ApiResponseModel> AddCompany(CompanyModel AddCompany);
        Task<UserResponceModel> UpdateCompanyDetails(CompanyModel updateCompany);
        Task<UserResponceModel> DeleteCompanyDetails(Guid CompanyId);
        Task<jsonData> GetDatatableCompanyList(DataTableRequstModel CompanydataTable);
    }
}
