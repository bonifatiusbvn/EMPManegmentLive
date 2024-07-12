using EMPManagment.API;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.Company;
using EMPManegment.Inretface.Interface.CompanyMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EMPManegment.Repository.CompanyRepository
{
    public class CompanyRepo : ICompany
    {
        public CompanyRepo(BonifatiusEmployeesContext Context)
        {
            this.Context = Context;
        }

        public BonifatiusEmployeesContext Context { get; }

        public async Task<IEnumerable<CompanyModel>> GetCompanyNameList()
        {
            try
            {
                IEnumerable<CompanyModel> CompanyName = Context.TblCompanyMasters.ToList().Select(a => new CompanyModel
                {
                    Id = a.Id,
                    CompnyName = a.CompnyName,
                    Address = a.Address,
                    City = a.City,
                    State = a.State,
                    Country = a.Country,
                    Gst = a.Gst
                });
                return CompanyName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CompanyModel> GetCompanyDetailsById(Guid CompanyId)
        {
            CompanyModel company = new CompanyModel();
            try
            {
                company = (from a in Context.TblCompanyMasters.Where(x => x.Id == CompanyId)
                           join b in Context.TblCities on a.City equals b.Id
                           join c in Context.TblStates on a.State equals c.Id
                           join d in Context.TblCountries on a.Country equals d.Id
                           select new CompanyModel
                           {
                               Id = a.Id,
                               CompnyName = a.CompnyName,
                               Gst = a.Gst,
                               Address = a.Address,
                               City = a.City,
                               CityName = b.City,
                               State = a.State,
                               StateName = c.State,
                               CountryName = d.Country,
                               Country = a.Country,
                               CreatedBy = a.CreatedBy,
                               CreatedOn = a.CreatedOn,
                               FullAddress = a.Address + "," + b.City + "," + c.State
                           }).First();
                return company;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ApiResponseModel> AddCompany(CompanyModel AddCompany)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
                var company = new TblCompanyMaster()
                {
                    Id = Guid.NewGuid(),
                    CompnyName = AddCompany.CompnyName,
                    Address = AddCompany.Address,
                    City = AddCompany.City,
                    State = AddCompany.State,
                    Country = AddCompany.Country,
                    Gst = AddCompany.Gst,
                    CreatedOn = DateTime.Now,
                    CreatedBy = AddCompany.CreatedBy,

                };
                response.message = "Company successfully created";
                Context.TblCompanyMasters.Add(company);
                Context.SaveChanges();
            }
            catch (Exception)
            {
                response.code = (int)HttpStatusCode.InternalServerError;
                response.message = "Error in creating company.";
            }
            return response;
        }
    }
}
