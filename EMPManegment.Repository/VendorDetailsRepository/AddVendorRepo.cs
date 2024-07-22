using Azure;
using EMPManagment.API;
using EMPManegment.EntityModels.Common;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using EMPManegment.Inretface.Interface.VendorDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace EMPManegment.Repository.VendorDetailsRepository
{
    public class AddVendorRepo : IAddVendorDetails
    {
        public BonifatiusEmployeesContext Context { get; }
        public IConfiguration _configuration { get; }

        public AddVendorRepo(BonifatiusEmployeesContext context, IConfiguration configuration)
        {
            Context = context;
            _configuration = configuration;
        }

        public async Task<UserResponceModel> AddVendor(VendorDetailsView vendor)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                bool isEmailAlreadyExists = Context.TblVendorMasters.Any(x => x.VendorEmail == vendor.VendorEmail);
                if (isEmailAlreadyExists == true)
                {
                    response.Message = "User with this email already exists";
                    response.Data = vendor;
                    response.Code = (int)HttpStatusCode.NotFound;
                }
                else
                {
                    var vendormodel = new TblVendorMaster()
                    {
                        Vid = Guid.NewGuid(),
                        VendorFirstName = vendor.VendorFirstName,
                        VendorLastName = vendor.VendorLastName,
                        VendorContact = vendor.VendorContectNo,
                        VendorPhone = vendor.VendorPhone,
                        VendorEmail = vendor.VendorEmail,
                        VendorCountry = vendor.VendorCountry,
                        VendorState = vendor.VendorState,
                        VendorCity = vendor.VendorCity,
                        VendorAddress = vendor.VendorAddress,
                        VendorPinCode = vendor.VendorPinCode,
                        VendorCompany = vendor.VendorCompany,
                        VendorCompanyType = vendor.VendorCompanyType,
                        VendorCompanyEmail = vendor.VendorCompanyEmail,
                        VendorCompanyLogo = vendor.VendorCompanyLogo,
                        VendorCompanyNumber = vendor.VendorCompanyNumber,
                        VendorBankName = vendor.VendorBankName,
                        VendorBankBranch = vendor.VendorBankBranch,
                        VendorAccountHolderName = vendor.VendorAccountHolderName,
                        VendorBankAccountNo = vendor.VendorBankAccountNo,
                        VendorGstnumber = vendor.VendorGstnumber,
                        VendorBankIfsc = vendor.VendorBankIfsc,
                        VendorTypeId = vendor.VendorTypeId,
                        CreatedBy = vendor.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    response.Message = "Vendor data successfully inserted";
                    Context.TblVendorMasters.Add(vendormodel);
                    Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "There is Some error in creating Vendor";
            }
            return response;
        }
        public async Task<jsonData> GetVendorsList(DataTableRequstModel dataTable)
        {
            var vendorlist = await Context.TblVendorMasters.FromSqlRaw("EXEC spGetAllVendorList").ToListAsync();

            if (!string.IsNullOrEmpty(dataTable.searchValue))
            {
                vendorlist = vendorlist.Where(e => e.VendorFirstName.Contains(dataTable.searchValue) || e.VendorPhone.Contains(dataTable.searchValue) || e.VendorEmail.Contains(dataTable.searchValue) || e.VendorCompany.Contains(dataTable.searchValue)).ToList();
            }

            int totalRecord = vendorlist.Count;

            if (!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDir))
            {
                vendorlist = SortVendorList(vendorlist, dataTable.sortColumn, dataTable.sortColumnDir);
            }

            var cData = vendorlist.Skip(dataTable.skip).Take(dataTable.pageSize).ToList();

            jsonData jsonData = new jsonData
            {
                draw = dataTable.draw,
                recordsFiltered = totalRecord,
                recordsTotal = totalRecord,
                data = cData
            };

            return jsonData;
        }

        private List<TblVendorMaster> SortVendorList(List<TblVendorMaster> vendorlist, string sortColumn, string sortColumnDir)
        {
            Func<TblVendorMaster, object> sortExpression = null;

            switch (sortColumn)
            {
                case "VendorFirstName":
                    sortExpression = v => v.VendorFirstName;
                    break;
                case "VendorLastName":
                    sortExpression = v => v.VendorLastName;
                    break;
                case "VendorEmail":
                    sortExpression = v => v.VendorEmail;
                    break;
                case "VendorPhone":
                    sortExpression = v => v.VendorPhone;
                    break;
                case "VendorCompany":
                    sortExpression = v => v.VendorCompany;
                    break;
                default:
                    sortExpression = v => v.VendorFirstName;
                    break;
            }

            if (sortColumnDir == "asc")
            {
                vendorlist = vendorlist.OrderBy(sortExpression).ToList();
            }
            else
            {
                vendorlist = vendorlist.OrderByDescending(sortExpression).ToList();
            }

            return vendorlist;
        }



        public async Task<IEnumerable<VendorTypeView>> GetVendorType()
        {
            try
            {
                IEnumerable<VendorTypeView> VendorType = Context.TblVendorTypes.ToList().Select(a => new VendorTypeView
                {
                    Id = a.Id,
                    VendorType = a.VendorName
                });
                return VendorType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<VendorDetailsView> GetVendorById(Guid VendorId)
        {

            try
            {
                string dbConnectionStr = _configuration.GetConnectionString("EMPDbconn");
                var sqlPar = new SqlParameter[]
                {
                    new SqlParameter("@VendorId", VendorId),
                };

                var DS = DbHelper.GetDataSet("[GetVendorById]", System.Data.CommandType.StoredProcedure, sqlPar, dbConnectionStr);

                VendorDetailsView vendordata = new VendorDetailsView();

                if (DS != null && DS.Tables.Count > 0)
                {
                    if (DS.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = DS.Tables[0].Rows[0];
                        {
                            vendordata.Vid = row["Vid"] != DBNull.Value ? (Guid)row["Vid"] : Guid.Empty; ;
                            vendordata.VendorFirstName = row["VendorFirstName"]?.ToString();
                            vendordata.VendorLastName = row["VendorLastName"]?.ToString(); ;
                            vendordata.VendorEmail = row["VendorEmail"]?.ToString();
                            vendordata.VendorPhone = row["VendorPhone"]?.ToString();
                            vendordata.VendorContectNo = row["VendorContectNo"]?.ToString();
                            vendordata.VendorCountry = row["VendorCountry"] != DBNull.Value ? (int)row["VendorCountry"] : 0; ;
                            vendordata.VendorCountryName = row["VendorCountryName"]?.ToString();
                            vendordata.VendorState = row["VendorState"] != DBNull.Value ? (int)row["VendorState"] : 0; ;
                            vendordata.VendorStateName = row["VendorStateName"]?.ToString();
                            vendordata.VendorCity = row["VendorCity"] != DBNull.Value ? (int)row["VendorCity"] : 0; ;
                            vendordata.VendorCityName = row["VendorCityName"]?.ToString();
                            vendordata.VendorPinCode = row["VendorPinCode"]?.ToString();
                            vendordata.VendorAddress = row["VendorAddress"]?.ToString();
                            vendordata.VendorCompany = row["VendorCompany"]?.ToString();
                            vendordata.VendorCompanyType = row["VendorCompanyType"]?.ToString();
                            vendordata.VendorCompanyEmail = row["VendorCompanyEmail"]?.ToString();
                            vendordata.VendorCompanyNumber = row["VendorCompanyNumber"]?.ToString();
                            vendordata.VendorCompanyLogo = row["VendorCompanyLogo"]?.ToString();
                            vendordata.VendorBankAccountNo = row["VendorBankAccountNo"]?.ToString();
                            vendordata.VendorBankName = row["VendorBankName"]?.ToString();
                            vendordata.VendorBankBranch = row["VendorBankBranch"]?.ToString();
                            vendordata.VendorAccountHolderName = row["VendorAccountHolderName"]?.ToString();
                            vendordata.VendorBankIfsc = row["VendorBankIfsc"]?.ToString();
                            vendordata.VendorGstnumber = row["VendorGstnumber"]?.ToString();
                            vendordata.VendorTypeId = row["VendorTypeId"] != DBNull.Value ? (int)row["VendorTypeId"] : 0; ;
                            vendordata.VendorTypeName = row["VendorTypeName"]?.ToString();
                            vendordata.FullAddress = row["FullAddress"]?.ToString();
                        };
                    }
                }
                return vendordata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<VendorListDetailsView>> GetVendorNameList()
        {
            try
            {
                IEnumerable<VendorListDetailsView> GetVendorList = Context.TblVendorMasters.ToList().Select(a => new VendorListDetailsView
                {
                    Id = a.Vid,
                    VendorCompany = a.VendorCompany,


                }).ToList();
                return GetVendorList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserResponceModel> UpdateVendorDetails(VendorDetailsView updateVendor)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var Vendordata = await Context.TblVendorMasters.FirstOrDefaultAsync(a => a.Vid == updateVendor.Vid);
                if (Vendordata.Vid != null)
                {

                    Vendordata.Vid = updateVendor.Vid;
                    Vendordata.VendorCompanyLogo = updateVendor.VendorCompanyLogo;
                    Vendordata.VendorFirstName = updateVendor.VendorFirstName;
                    Vendordata.VendorLastName = updateVendor.VendorLastName;
                    Vendordata.VendorEmail = updateVendor.VendorEmail;
                    Vendordata.VendorPhone = updateVendor.VendorPhone;
                    Vendordata.VendorContact = updateVendor.VendorContectNo;
                    Vendordata.VendorPinCode = updateVendor.VendorPinCode;
                    Vendordata.VendorAddress = updateVendor.VendorAddress;
                    Vendordata.VendorCompany = updateVendor.VendorCompany;
                    Vendordata.VendorCompanyType = updateVendor.VendorCompanyType;
                    Vendordata.VendorCompanyEmail = updateVendor.VendorCompanyEmail;
                    Vendordata.VendorCompanyNumber = updateVendor.VendorCompanyNumber;
                    Vendordata.VendorBankAccountNo = updateVendor.VendorBankAccountNo;
                    Vendordata.VendorBankName = updateVendor.VendorBankName;
                    Vendordata.VendorBankBranch = updateVendor.VendorBankBranch;
                    Vendordata.VendorAccountHolderName = updateVendor.VendorAccountHolderName;
                    Vendordata.VendorBankIfsc = updateVendor.VendorBankIfsc;
                    Vendordata.VendorGstnumber = updateVendor.VendorGstnumber;
                    Vendordata.VendorCity = updateVendor.VendorCity;
                    Vendordata.VendorCountry = updateVendor.VendorCountry;
                    Vendordata.VendorState = updateVendor.VendorState;
                    Vendordata.VendorTypeId = updateVendor.VendorTypeId;
                    Vendordata.UpdatedOn = DateTime.Now;
                    Vendordata.UpdatedBy = updateVendor.UpdatedBy;

                    Context.TblVendorMasters.Update(Vendordata);
                    await Context.SaveChangesAsync();
                    response.Message = "Vendor data updated successfully!";
                }
                else
                {
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Message = "Vendor cannot found.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "There is Some error in updating Vendor";
            }
            return response;
        }
    }
}
