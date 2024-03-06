using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.POMaster;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.Inretface.Interface.PurchaseOrder;
using EMPManegment.Inretface.Services.PurchaseOrderSevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.PurchaseOrderMaster
{
    public class PurchaseOrderServices : IPOServices
    {
        public PurchaseOrderServices(IPurchaseOrder purchaseOrder)
        {
            PurchaseOrder = purchaseOrder;
        }

        public IPurchaseOrder PurchaseOrder { get; }

        public string CheckOPNo(string projectname)
        {
            return PurchaseOrder.CheckOPNo(projectname);
        }

        public async Task<UserResponceModel> CreatePO(List<OPMasterView> CreatePO)
        {
            return await PurchaseOrder.CreatePO(CreatePO);
        }

        public async Task<IEnumerable<OPMasterView>> GetPOList()
        {
            return await PurchaseOrder.GetPOList();
        }
    }
}
