﻿using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Interface.VendorDetails
{
    public interface IAddVendorDetails
    {
        Task<UserResponceModel> AddVendor(VendorDetailsView vendor);
        Task<IEnumerable<VendorDetailsView>> GetVendorsList();
    }
}
