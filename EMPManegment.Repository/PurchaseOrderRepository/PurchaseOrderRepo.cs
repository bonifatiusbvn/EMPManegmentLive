using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.POMaster;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.Inretface.Interface.PurchaseOrder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Repository.PurchaseOrderRepository
{
    public class PurchaseOrderRepo : IPurchaseOrder
    {

        public PurchaseOrderRepo(BonifatiusEmployeesContext context)
        {
            Context = context;
        }

        public BonifatiusEmployeesContext Context { get; }

        public async Task<UserResponceModel> CreatePO(OPMasterView CreatePO)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var Pomodel = new TblPurchaseOrder()
                {
                    Id = Guid.NewGuid(),
                    VendorId = CreatePO.VendorId,
                    Opid = CreatePO.Opid,
                    OrderDate = CreatePO.OrderDate,
                    DeliveryDate = CreatePO.DeliveryDate,
                    Status = CreatePO.Status,
                    TotalAmount = CreatePO.TotalAmount,
                };
                response.Code = 200;
                response.Message = "PO add successfully!";
                Context.TblPurchaseOrders.Add(Pomodel);
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }
}
