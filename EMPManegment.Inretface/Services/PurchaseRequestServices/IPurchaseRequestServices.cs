using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.Purchase_Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Services.PurchaseRequestServices
{
    public interface IPurchaseRequestServices
    {
        Task<ApiResponseModel> CreatePurchaseRequest(PurchaseRequestMasterView AddPurchaseRequest);
        Task<jsonData> GetPRList(DataTableRequstModel PRdataTable);
        Task<IEnumerable<PurchaseRequestModel>> GetPurchaseRequestList();
        Task<PurchaseRequestModel> GetPurchaseRequestDetailsById(string PrId);
        Task<ApiResponseModel> UpdatePurchaseRequestDetails(PurchaseRequestMasterView UpdatePurchaseRequest);
        Task<ApiResponseModel> DeletePurchaseRequest(string PrNo);
        string CheckPRNo();
        Task<PurchaseRequestMasterView> PurchaseRequestDetailsByPrNo(string PrNo);
        Task<UserResponceModel> ApproveUnapprovePR(PRIsApprovedMasterModel PRIdList);
        Task<List<PurchaseRequestModel>> ProductDetailsById(Guid ProductId);
    }
}
