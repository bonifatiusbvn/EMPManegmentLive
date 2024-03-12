using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.Inretface.Interface.OrderDetails;
using EMPManegment.Inretface.Services.OrderDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.OrderDetails
{
    public class PurchaseOrderDetailsServices : IPurchaseOrderDetailsServices
    {
        public PurchaseOrderDetailsServices(IPurchaseOrderDetails purchaseOrderDetails) 
        {
            PurchaseOrderDetails = purchaseOrderDetails;
        }

        public IPurchaseOrderDetails PurchaseOrderDetails { get; }

        public async Task<UserResponceModel> CreatePurchaseOrder(PurchaseOrderDetailView CreatePurchaseOrder)
        {
            return await PurchaseOrderDetails.CreatePurchaseOrder(CreatePurchaseOrder);
        }
        public async Task<IEnumerable<PurchaseOrderDetailView>> GetPurchaseOrderList()
        {
            return await PurchaseOrderDetails.GetPurchaseOrderList();
        }
        public async Task<List<PurchaseOrderDetailView>> GetPurchaseOrderDetailsByStatus(string DeliveryStatus)
        {
            return await PurchaseOrderDetails.GetPurchaseOrderDetailsByStatus(DeliveryStatus);
        }

        public string CheckPurchaseOrder(string projectname)
        {
            return PurchaseOrderDetails.CheckPurchaseOrder(projectname);
        }

        public async Task<List<PurchaseOrderDetailView>> GetPurchaseOrderDetailsById(string OrderId)
        {
            return await PurchaseOrderDetails.GetPurchaseOrderDetailsById(OrderId);
        }

        public async Task<UserResponceModel> InsertMultiplePurchaseOrder(List<PurchaseOrderView> InsertPurchaseOrder)
        {
            return await PurchaseOrderDetails.InsertMultiplePurchaseOrder(InsertPurchaseOrder);
        }

        public async Task<IEnumerable<PaymentMethodView>> GetAllPaymentMethod()
        {
            return await PurchaseOrderDetails.GetAllPaymentMethod();
        }

        public async Task<UpdatePurchaseOrderView> EditPurchaseOrderDetails(Guid Id)
        {
            return await PurchaseOrderDetails.EditPurchaseOrderDetails(Id);
        }

        public async Task<UserResponceModel> UpdatePurchaseOrderDetails(UpdatePurchaseOrderView UpdatePurchaseorder)
        {
            return await PurchaseOrderDetails.UpdatePurchaseOrderDetails(UpdatePurchaseorder);
        }

        public async Task<UserResponceModel> DeletePurchaseOrderDetails(string OrderId)
        {
            return await PurchaseOrderDetails.DeletePurchaseOrderDetails(OrderId);
        }
    }
}
