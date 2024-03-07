﻿using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.POMaster;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Interface.PurchaseOrder
{
    public interface IPurchaseOrder
    {
        string CheckOPNo(string projectname);
        Task<UserResponceModel> CreatePO(List<OPMasterView> CreatePO);
        Task<jsonData> GetPOList(DataTableRequstModel POdataTable);
    }
}