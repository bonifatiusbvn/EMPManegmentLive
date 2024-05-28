using Azure;
using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using EMPManegment.Inretface.Interface.VendorDetails;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public AddVendorRepo(BonifatiusEmployeesContext context)
        {
            Context = context;
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
                    response.Code = 400;
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
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = "Vendor data successfully inserted";
                    Context.TblVendorMasters.Add(vendormodel);
                    Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                response.Code = 400;
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
            VendorDetailsView vendordata = new VendorDetailsView();
            try
            {
                vendordata = (from d in Context.TblVendorMasters.Where(x => x.Vid == VendorId)
                              join c in Context.TblCountries on d.VendorCountry equals c.Id
                              join s in Context.TblStates on d.VendorState equals s.Id
                              join ct in Context.TblCities on d.VendorCity equals ct.Id
                              join t in Context.TblVendorTypes on d.VendorTypeId equals t.Id
                              select new VendorDetailsView
                              {
                                  Vid = d.Vid,
                                  VendorFirstName = d.VendorFirstName,
                                  VendorLastName = d.VendorLastName,
                                  VendorEmail = d.VendorEmail,
                                  VendorPhone = d.VendorPhone,
                                  VendorContectNo = d.VendorContact,
                                  VendorCountry = d.VendorCountry,
                                  VendorCountryName = c.Country,
                                  VendorState = d.VendorState,
                                  VendorStateName = s.State,
                                  VendorCity = d.VendorCity,
                                  VendorCityName = ct.City,
                                  VendorPinCode = d.VendorPinCode,
                                  VendorAddress = d.VendorAddress,
                                  VendorCompany = d.VendorCompany,
                                  VendorCompanyType = d.VendorCompanyType,
                                  VendorCompanyEmail = d.VendorCompanyEmail,
                                  VendorCompanyNumber = d.VendorCompanyNumber,
                                  VendorCompanyLogo = d.VendorCompanyLogo,
                                  VendorBankAccountNo = d.VendorBankAccountNo,
                                  VendorBankName = d.VendorBankName,
                                  VendorBankBranch = d.VendorBankBranch,
                                  VendorAccountHolderName = d.VendorAccountHolderName,
                                  VendorBankIfsc = d.VendorBankIfsc,
                                  VendorGstnumber = d.VendorGstnumber,
                                  VendorTypeId = d.VendorTypeId,
                                  VendorTypeName = t.VendorName,
                                  FullAddress = d.VendorAddress + "," + ct.City + "," + s.State + "-" + d.VendorPinCode,
                              }).First();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return vendordata;
        }



        public async Task<IEnumerable<VendorListDetailsView>> GetVendorNameList()
        {
            IEnumerable<VendorListDetailsView> GetVendorList = Context.TblVendorMasters.ToList().Select(a => new VendorListDetailsView
            {
                Id = a.Vid,
                VendorCompany = a.VendorCompany,
                

            }).ToList();
            return GetVendorList;
        }

        public async Task<UserResponceModel> UpdateVendorDetails(VendorDetailsView updateVendor)
        {
            UserResponceModel response = new UserResponceModel();
            var Vendordata = await Context.TblVendorMasters.FirstOrDefaultAsync(a => a.Vid == updateVendor.Vid);
            if (Vendordata.Vid != null)
            {
                                  
                Vendordata.Vid = updateVendor.Vid;
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
                Vendordata.VendorState =  updateVendor.VendorState;
                Vendordata.VendorTypeId = updateVendor.VendorTypeId;
                Context.TblVendorMasters.Update(Vendordata);
                await Context.SaveChangesAsync();
            }
            response.Code = 200;
            response.Message = "Vendor data updated successfully!";
            return response;
        }
    }
}
