using EMPManagment.API;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.Company;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.CompanyMaster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
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
                               PinCode = a.PinCode,
                               CreatedBy = a.CreatedBy,
                               Email = a.Email,
                               CompanyLogo = a.CompanyLogo,
                               ContactNumber = a.ContactNumber,
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
                    PinCode = AddCompany.PinCode,
                    Gst = AddCompany.Gst,
                    Email = AddCompany.Email,
                    ContactNumber = AddCompany.ContactNumber,
                    CompanyLogo = AddCompany.CompanyLogo,
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
        public async Task<UserResponceModel> UpdateCompanyDetails(CompanyModel updateCompany)
        {
            UserResponceModel model = new UserResponceModel();
            try
            {
                var getCompanyDetails = Context.TblCompanyMasters.Where(e => e.Id == updateCompany.Id).FirstOrDefault();
                if (getCompanyDetails != null)
                {
                    getCompanyDetails.CompnyName = updateCompany.CompnyName;
                    getCompanyDetails.ContactNumber = updateCompany.ContactNumber;
                    getCompanyDetails.Country = updateCompany.Country;
                    getCompanyDetails.State = updateCompany.State;
                    getCompanyDetails.City = updateCompany.City;
                    getCompanyDetails.PinCode = updateCompany.PinCode;
                    getCompanyDetails.CompanyLogo = updateCompany.CompanyLogo;
                    getCompanyDetails.Email = updateCompany.Email;
                    getCompanyDetails.Gst = updateCompany.Gst;
                    getCompanyDetails.Address = updateCompany.Address;
                    getCompanyDetails.UpdatedOn = DateTime.Now;
                    getCompanyDetails.UpdatedBy = updateCompany.UpdatedBy;


                    Context.TblCompanyMasters.Update(getCompanyDetails);
                    Context.SaveChanges();
                    model.Message = "Company  updated successfully!";
                }
                else
                {
                    model.Code = (int)HttpStatusCode.NotFound;
                    model.Message = "Company doesn't found";
                }
            }
            catch (Exception ex)
            {
                model.Code = (int)HttpStatusCode.InternalServerError;
                model.Message = "Error in updating Company details.";
            }
            return model;
        }
        public async Task<UserResponceModel> DeleteCompanyDetails(Guid CompanyId)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var GetCompanydata = Context.TblCompanyMasters.Where(a => a.Id == CompanyId).FirstOrDefault();

                if (GetCompanydata != null)
                {
                    Context.TblCompanyMasters.Remove(GetCompanydata);
                    Context.SaveChanges();
                    response.Data = GetCompanydata;
                    response.Message = "Company is deleted successfully";
                }
                else
                {
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Message = "Can not found company";
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in deleting company.";
            }
            return response;
        }
        public async Task<jsonData> GetDatatableCompanyList(DataTableRequstModel CompanydataTable)
        {
            try
            {
                var Companylist = await Context.TblCompanyMasters.FromSqlRaw("EXEC GetAllCompanyList").ToListAsync();

                if (!string.IsNullOrEmpty(CompanydataTable.searchValue))
                {
                    Companylist = Companylist.Where(e => e.CompnyName.Contains(CompanydataTable.searchValue) || e.ContactNumber.Contains(CompanydataTable.searchValue) || e.Email.Contains(CompanydataTable.searchValue)).ToList();
                }
                int totalRecord = Companylist.Count;
                if (!string.IsNullOrEmpty(CompanydataTable.sortColumn) && !string.IsNullOrEmpty(CompanydataTable.sortColumnDir))
                {
                    Companylist = SortCompanyList(Companylist, CompanydataTable.sortColumn, CompanydataTable.sortColumnDir);
                }
                var cData = Companylist.Skip(CompanydataTable.skip).Take(CompanydataTable.pageSize).ToList();
                jsonData jsonData = new jsonData
                {
                    draw = CompanydataTable.draw,
                    recordsFiltered = totalRecord,
                    recordsTotal = totalRecord,
                    data = cData
                };
                return jsonData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<TblCompanyMaster> SortCompanyList(List<TblCompanyMaster> companylist, string sortColumn, string sortColumnDir)
        {
            Func<TblCompanyMaster, object> sortExpression = null;

            switch (sortColumn)
            {
                case "ContactNumber":
                    sortExpression = v => v.ContactNumber;
                    break;
                case "CompnyName":
                    sortExpression = v => v.CompnyName;
                    break;
                case "Email":
                    sortExpression = v => v.Email;
                    break;
                default:
                    sortExpression = v => v.CompnyName;
                    break;
            }

            if (sortColumnDir == "asc")
            {
                companylist = companylist.OrderBy(sortExpression).ToList();
            }
            else
            {
                companylist = companylist.OrderByDescending(sortExpression).ToList();
            }

            return companylist;
        }
    }
}
