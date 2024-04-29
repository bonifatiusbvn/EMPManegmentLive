using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.Purchase_Request;
using EMPManegment.Inretface.Interface.ProjectDetails;
using EMPManegment.Inretface.Interface.PurchaseRequest;
using EMPManegment.Inretface.Services.OrderDetails;
using EMPManegment.Inretface.Services.PurchaseRequestServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseRequestController : ControllerBase
    {
        public PurchaseRequestController(IPurchaseRequestServices PurchaseRequest)
        {
            purchaseRequest = PurchaseRequest;
        }
        public IPurchaseRequestServices purchaseRequest { get; }

        [HttpPost]
        [Route("AddPurchaseRequestDetails")]
        public async Task<IActionResult> AddPurchaseRequestDetails(PurchaseRequestModel PuchaseRequestDetails)
        {
            ApiResponseModel response = new ApiResponseModel();
            var PurchaseRequest = await purchaseRequest.AddPurchaseRequestDetails(PuchaseRequestDetails);
            if (PurchaseRequest.code == 200)
            {
                response.code = PurchaseRequest.code;
                response.message = PurchaseRequest.message;
            }
            return StatusCode(response.code, response);
        }
        [HttpPost]
        [Route("GetPurchaseRequestList")]
        public async Task<IActionResult> GetPurchaseRequestList()
        {
            IEnumerable<PurchaseRequestModel> PurchaseRequest = await purchaseRequest.GetPurchaseRequestList();
            return Ok(new { code = 200, data = PurchaseRequest.ToList() });
        }

        [HttpGet]
        [Route("GetPurchaseRequestDetailsById")]
        public async Task<IActionResult> GetPurchaseRequestDetailsById(Guid PrId)
        {
            var purchaseRequestDetails = await purchaseRequest.GetPurchaseRequestDetailsById(PrId);
            return Ok(new { code = 200, data = purchaseRequestDetails });
        }
    }
}
