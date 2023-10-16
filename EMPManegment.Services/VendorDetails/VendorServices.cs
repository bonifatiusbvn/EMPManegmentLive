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
        public async Task<UserResponceModel> AddVendor(VendorDetailsView vendor)
        {
            return await details.AddVendor(vendor);
        }

        public async Task<IEnumerable<VendorDetailsView>> GetVendorsList()
        {
            return await details.GetVendorsList();
        }
    }
}
