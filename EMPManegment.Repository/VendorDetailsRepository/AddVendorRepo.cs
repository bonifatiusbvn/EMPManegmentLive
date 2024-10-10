using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
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
                bool isEmailAlredyExists = Context.TblVendorMasters.Any(x => x.VendorEmail == vendor.VendorEmail);
                if (isEmailAlredyExists == true)
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
                        VendorFirstName =vendor.VendorFirstName,
                        VendorLastName=vendor.VendorLastName,
                        VendorContact = vendor.VendorContectNo,
                        VendorPhone = vendor.VendorPhone,
                        VendorEmail = vendor.VendorEmail,
                        VendorCountry=vendor.VendorCountry,
                        VendorState=vendor.VendorState,
                        VendorCity=vendor.VendorCity,
                        VendorAddress=vendor.VendorAddress,
                        VendorPinCode=vendor.VendorPinCode,
                        VendorCompany=vendor.VendorCompany,
                        VendorCompanyType=vendor.VendorCompanyType,
                        VendorCompanyEmail=vendor.VendorCompanyEmail,
                        VendorCompanyNumber=vendor.VendorCompanyNumber,
                        VendorCompanyLogo=vendor.VendorCompanyLogo,
                        VendorBankName=vendor.VendorBankName,
                        VendorBankBranch=vendor.VendorBankBranch,
                        VendorAccountHolderName=vendor.VendorAccountHolderName,
                        VendorBankAccountNo=vendor.VendorBankAccountNo,
                        VendorGstnumber=vendor.VendorGstnumber,
                        VendorBankIfsc=vendor.VendorBankIfsc,
                        VendorTypeId=vendor.VendorTypeId,
                    };
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message= "Vendor Data Successfully Inserted";
                    Context.TblVendorMasters.Add(vendormodel);
                    Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
        public async Task<jsonData> GetVendorsList(DataTableRequstModel dataTable)
        {
            var vendorlist = Context.TblVendorMasters.Select(a => new VendorDetailsView
            {
                //Id = a.VId,
                //VendorName = a.VendorName,
                VendorEmail = a.VendorEmail,
                VendorPhone = a.VendorPhone,
                VendorAddress = a.VendorAddress,
                VendorBankAccountNo = a.VendorBankAccountNo,
                VendorBankName = a.VendorBankName,
                VendorBankIfsc = a.VendorBankIfsc,
                VendorGstnumber = a.VendorGstnumber,
                CreatedOn = DateTime.Now,
                CreatedBy = a.CreatedBy,
            });
            if (!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDir))
            {
                vendorlist = vendorlist.OrderBy(dataTable.sortColumn + " " + dataTable.sortColumnDir);
            }

            if (!string.IsNullOrEmpty(dataTable.searchValue))
            {
                vendorlist = vendorlist.Where(e => e.VendorFirstName.Contains(dataTable.searchValue) || e.VendorPhone.Contains(dataTable.searchValue) || e.VendorEmail.Contains(dataTable.searchValue));
            }

            int totalRecord = vendorlist.Count();

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

        public async Task<IEnumerable<VendorTypeView>> GetVendorType()
        {
            try
            {
                IEnumerable<VendorTypeView> VendorType = Context.TblVendorTypes.ToList().Select(a => new VendorTypeView
                {
                    Id = a.Id,
                    VendorType = a.VendorType
                });
                return VendorType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
