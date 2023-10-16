using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using EMPManegment.Inretface.Interface.VendorDetails;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    var model = new TblVendorMaster()
                    {
                        VendorName = vendor.VendorName,
                        VendorEmail = vendor.VendorEmail,
                        VendorPhone = vendor.VendorPhone,
                        VendorAddress = vendor.VendorAddress,
                        VendorBankAccountNo = vendor.VendorBankAccountNo,
                        VendorBankName = vendor.VendorBankName,
                        VendorBankIfsc= vendor.VendorBankIfsc,
                        VendorGstnumber = vendor.VendorGstnumber,
                        CreatedOn = vendor.CreatedOn,
                        CreatedBy = vendor.CreatedBy,

                    };
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message= "Vendor Data Successfully Inserted";
                    Context.TblVendorMasters.Add(model);
                    Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<IEnumerable<VendorDetailsView>> GetVendorsList()
        {
            var vendorlist = await Context.TblVendorMasters.ToListAsync();
            List<VendorDetailsView> model = vendorlist.Select(a => new VendorDetailsView
            {
                Id = a.Id,
                VendorName=a.VendorName,
                VendorEmail=a.VendorEmail,
                VendorPhone=a.VendorPhone,
                VendorAddress=a.VendorAddress,
                VendorBankAccountNo=a.VendorBankAccountNo,
                VendorBankName=a.VendorBankName,
                VendorBankIfsc=a.VendorBankIfsc,
                VendorGstnumber=a.VendorGstnumber,
                CreatedOn = DateTime.Now,
                CreatedBy = a.CreatedBy,
            }).ToList();
            return model;
        }
    }
}
