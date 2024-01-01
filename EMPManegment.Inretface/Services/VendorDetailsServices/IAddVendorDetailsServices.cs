using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Services.VendorDetailsServices
{
    public interface IAddVendorDetailsServices
    {
        Task<UserResponceModel> AddVendor(VendorDetailsView AddVendor);
        Task<jsonData> GetVendorsList(DataTableRequstModel VendorsList);
        Task<IEnumerable<VendorTypeView>> GetVendorType();
        Task<VendorDetailsView> GetVendorById(Guid VendorId);
        Task<IEnumerable<VendorDetailsView>> GetVendorNameList();
    }
}
    