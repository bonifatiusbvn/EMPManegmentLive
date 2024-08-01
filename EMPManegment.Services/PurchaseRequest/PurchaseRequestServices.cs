using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.Purchase_Request;
using EMPManegment.Inretface.Interface.OrderDetails;
using EMPManegment.Inretface.Interface.PurchaseRequest;
using EMPManegment.Inretface.Services.PurchaseRequestServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

        public async Task<ApiResponseModel> CreatePurchaseRequest(PurchaseRequestMasterView AddPurchaseRequest)
        {
            return await purchaseRequest.CreatePurchaseRequest(AddPurchaseRequest);
        }

        public async Task<ApiResponseModel> DeletePurchaseRequest(string PrNo)
        {
            return await purchaseRequest.DeletePurchaseRequest(PrNo);
        }

        public async Task<PurchaseRequestModel> GetPurchaseRequestDetailsById(Guid PrId)
        {
            return await purchaseRequest.GetPurchaseRequestDetailsById(PrId);
        }

        public async Task<IEnumerable<PurchaseRequestModel>> GetPurchaseRequestList()
        {
            return await purchaseRequest.GetPurchaseRequestList();
        }

        public async Task<ApiResponseModel> UpdatePurchaseRequestDetails(PurchaseRequestMasterView UpdatePurchaseRequest)
        {
            return await purchaseRequest.UpdatePurchaseRequestDetails(UpdatePurchaseRequest);
        }
        public string CheckPRNo()
        {
            return purchaseRequest.CheckPRNo();
        }

        public async Task<jsonData> GetPRList(DataTableRequstModel PRdataTable)
        {
            return await purchaseRequest.GetPRList(PRdataTable);
        }
        public async Task<PurchaseRequestMasterView> PurchaseRequestDetailsByPrNo(string PrNo)
        {
            return await purchaseRequest.PurchaseRequestDetailsByPrNo(PrNo);
        }
        public async Task<UserResponceModel> ApproveUnapprovePR(PRIsApprovedMasterModel PRIdList)
        {
            return await purchaseRequest.ApproveUnapprovePR(PRIdList);
        }

        public Task<PurchaseRequestModel> GetPurchaseRequestDetailsById(string PrId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PurchaseRequestModel>> ProductDetailsById(Guid ProductId)
        {
            return await purchaseRequest.ProductDetailsById(ProductId);
        }
    }
}
