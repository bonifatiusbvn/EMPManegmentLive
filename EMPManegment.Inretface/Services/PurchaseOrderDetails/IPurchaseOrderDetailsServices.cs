using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.PurchaseOrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Services.OrderDetails
{
    public interface IPurchaseOrderDetailsServices
    {
        Task<UserResponceModel> CreatePurchaseOrder(PurchaseOrderDetailView CreatePurchaseOrder);
        Task<IEnumerable<PurchaseOrderDetailView>> GetPurchaseOrderList();
        Task<List<PurchaseOrderDetailView>> GetPurchaseOrderDetailsByStatus(string DeliveryStatus);
        string CheckPurchaseOrder(string projectname);
        Task<List<PurchaseOrderDetailView>> GetPurchaseOrderDetailsById(string OrderId);
        Task<ApiResponseModel> InsertMultiplePurchaseOrder(PurchaseOrderMasterView InsertPurchaseOrder);
        Task<IEnumerable<PaymentMethodView>> GetAllPaymentMethod();
        Task<UpdatePurchaseOrderView> EditPurchaseOrderDetails(Guid Id);
        Task<UserResponceModel> UpdatePurchaseOrderDetails(UpdatePurchaseOrderView UpdatePurchaseorder);
        Task<UserResponceModel> DeletePurchaseOrderDetails(string OrderId);
    }
}
