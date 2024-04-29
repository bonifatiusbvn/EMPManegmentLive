using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.Purchase_Request;
using EMPManegment.Inretface.Interface.OrderDetails;
using EMPManegment.Inretface.Interface.PurchaseRequest;
using EMPManegment.Inretface.Services.PurchaseRequestServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.PurchaseRequest
{
    public class PurchaseRequestServices:IPurchaseRequestServices
    {
        public PurchaseRequestServices(IPurchaseRequest PurchaseRequest)
        {
            purchaseRequest = PurchaseRequest;
        }
        public IPurchaseRequest purchaseRequest { get; }

        public async Task<ApiResponseModel> AddPurchaseRequestDetails(PurchaseRequestModel PurchaseRequestDetails)
        {
            return await purchaseRequest.AddPurchaseRequestDetails(PurchaseRequestDetails);
        }

        public async Task<PurchaseRequestModel> GetPurchaseRequestDetailsById(Guid PrId)
        {
            return await purchaseRequest.GetPurchaseRequestDetailsById(PrId);
        }

        public async Task<IEnumerable<PurchaseRequestModel>> GetPurchaseRequestList()
        {
            return await purchaseRequest.GetPurchaseRequestList();
        }
    }
}
