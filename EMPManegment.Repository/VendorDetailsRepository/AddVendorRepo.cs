using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using EMPManegment.Inretface.Interface.VendorDetails;
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

        public async Task<VendorDetailsResponseModel> AddVendor(VendorDetailsView vendor)
        {
            VendorDetailsResponseModel response = new VendorDetailsResponseModel();
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
                    response.Data = vendor;
                    response.Code = (int)HttpStatusCode.OK;
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
    }
}
