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
        Task<IEnumerable<PurchaseOrderDetailView>> GetPurchaseOrderList();
        Task<List<PurchaseOrderDetailView>> GetPurchaseOrderDetailsByStatus(string DeliveryStatus);
        string CheckPurchaseOrder(string projectname);
        Task<PurchaseOrderMasterView> GetPurchaseOrderDetailsByOrderId(string OrderId);
        Task<ApiResponseModel> InsertMultiplePurchaseOrder(PurchaseOrderMasterView InsertPurchaseOrder);
        Task<IEnumerable<PaymentMethodView>> GetAllPaymentMethod();
        Task<PurchaseOrderMasterView> EditPurchaseOrderDetails(Guid Id);
        Task<UserResponceModel> UpdatePurchaseOrderDetails(PurchaseOrderMasterView UpdatePurchaseorder);
        Task<UserResponceModel> DeletePurchaseOrderDetails(Guid Id);
        Task<List<PurchaseOrderDetailsModel>> GetPOProductDetailsById(Guid ProductId);
    }
}
