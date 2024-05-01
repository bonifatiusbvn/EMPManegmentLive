using EMPManagment.Web.Models.API;
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
        Task<ApiResponseModel> AddPurchaseRequestDetails(PurchaseRequestModel AddPurchaseRequest);

        Task<IEnumerable<PurchaseRequestModel>> GetPurchaseRequestList();

        Task<PurchaseRequestModel> GetPurchaseRequestDetailsById(Guid PrId);
        Task<ApiResponseModel> UpdatePurchaseRequestDetails(PurchaseRequestModel UpdatePurchaseRequest);
        Task<ApiResponseModel> DeletePurchaseRequestDetails(Guid PrId);
    }
}
