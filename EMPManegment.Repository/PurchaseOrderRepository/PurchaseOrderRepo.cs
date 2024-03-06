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

        public string CheckOPNo(string projectname)
        {
            try
            {
                var LastOrder = Context.TblOrderMasters.OrderByDescending(e => e.CreatedOn).FirstOrDefault();
                var currentYear = DateTime.Now.Year;
                var lastYear = currentYear - 1;

                string POId;
                if (LastOrder == null)
                {
                    POId = $"BTPL/OP/{projectname}/{lastYear % 100}-{currentYear % 100}-01";
                }
                else
                {
                    if (LastOrder.OrderId.Length >= 25)
                    {
                        int orderNumber = int.Parse(LastOrder.OrderId.Substring(24)) + 1;
                        POId = $"BTPL/OP/{projectname}/{lastYear % 100}-{currentYear % 100}-" + orderNumber.ToString("D3");
                    }
                    else
                    {
                        throw new Exception("OrderId does not have the expected format.");
                    }
                }
                return POId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
