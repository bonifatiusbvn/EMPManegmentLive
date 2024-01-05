using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using EMPManegment.Inretface.Interface.VendorDetails;
using EMPManegment.Inretface.Services.VendorDetailsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.VendorDetails
{
    public class VendorServices : IAddVendorDetailsServices
    {
        private readonly IAddVendorDetails details;
        public VendorServices(IAddVendorDetails details)
        {
            this.details = details;
        }
        public async Task<UserResponceModel> AddVendor(VendorDetailsView Addvendor)
        {
            return await details.AddVendor(Addvendor);
        }

        public async Task<VendorDetailsView> GetVendorById(Guid VendorId)
        {
            return await details.GetVendorById(VendorId);
        }

        public async Task<jsonData> GetVendorsList(DataTableRequstModel GetVenderList)
        {
            return await details.GetVendorsList(GetVenderList);
        }

        public async Task<IEnumerable<VendorTypeView>> GetVendorType()
        {
            return await details.GetVendorType();
        }
        public async Task<IEnumerable<VendorListDetailsView>> GetVendorNameList()
        {
            return await details.GetVendorNameList();
        }
    }
}
