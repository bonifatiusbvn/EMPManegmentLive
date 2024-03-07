using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.POMaster;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Services.PurchaseOrderSevices
{
    public interface IPOServices
    {
        Task<UserResponceModel> CreatePO(List<OPMasterView> CreatePO);
        string CheckOPNo(string projectname);
        Task<jsonData> GetPOList(DataTableRequstModel POdataTable);
    }
}
