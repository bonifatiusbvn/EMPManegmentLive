using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.Purchase_Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Interface.PurchaseRequest
{
    public interface IPurchaseRequest
    {
        Task<ApiResponseModel> CreatePurchaseRequest(PurchaseRequestMasterView AddPurchaseRequest);
        Task<jsonData> GetPRList(DataTableRequstModel PRdataTable);
        Task<IEnumerable<PurchaseRequestModel>> GetPurchaseRequestList();
        Task<PurchaseRequestModel> GetPurchaseRequestDetailsById(Guid PrId);
        Task<ApiResponseModel> UpdatePurchaseRequestDetails(PurchaseRequestModel UpdatePurchaseRequest);
        Task<ApiResponseModel> DeletePurchaseRequest(string PrNo);
        string CheckPRNo();
        Task<PurchaseRequestMasterView> PurchaseRequestDetailsByPrNo(string PrNo);
        Task<UserResponceModel> ApproveUnapprovePR(string PrNo);
    }
}                                                                                      
