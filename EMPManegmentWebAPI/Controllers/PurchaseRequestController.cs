using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.Purchase_Request;
using EMPManegment.Inretface.Interface.OrderDetails;
using EMPManegment.Inretface.Interface.ProjectDetails;
using EMPManegment.Inretface.Interface.PurchaseRequest;
using EMPManegment.Inretface.Services.OrderDetails;
using EMPManegment.Inretface.Services.PurchaseRequestServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        [Route("CreatePurchaseRequest")]
        public async Task<IActionResult> CreatePurchaseRequest(PurchaseRequestMasterView AddPurchaseRequest)
        {
            ApiResponseModel response = new ApiResponseModel();
            var PurchaseRequest = await purchaseRequest.CreatePurchaseRequest(AddPurchaseRequest);
            if (PurchaseRequest.code == 200)
            {
                response.code = PurchaseRequest.code;
                response.message = PurchaseRequest.message;
            }
            else
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
        [Route("EditPurchaseRequestDetails")]
        public async Task<IActionResult> EditPurchaseRequestDetails(Guid PrId)
        {
            var purchaseRequestDetails = await purchaseRequest.GetPurchaseRequestDetailsById(PrId);
            return Ok(new { code = 200, data = purchaseRequestDetails });
        }
        [HttpPost]
        [Route("UpdatePurchaseRequestDetails")]
        public async Task<IActionResult> UpdatePurchaseRequestDetails(PurchaseRequestModel PurchaseRequestDetails)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
                var updatePR = purchaseRequest.UpdatePurchaseRequestDetails(PurchaseRequestDetails);
                if (updatePR.Result.code == 200)
                {
                    response.code = (int)HttpStatusCode.OK;
                    response.message = updatePR.Result.message;
                }
                else
                {
                    response.message = updatePR.Result.message;
                    response.code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StatusCode(response.code, response);
        }
        [HttpPost]
        [Route("DeletePurchaseRequest")]
        public async Task<IActionResult> DeletePurchaseRequest(string PrNo)
        {
            ApiResponseModel responseModel = new ApiResponseModel();
            var PurchaseRequest = await purchaseRequest.DeletePurchaseRequest(PrNo);
            try
            {
                if (PurchaseRequest != null)
                {
                    responseModel.code = (int)HttpStatusCode.OK;
                    responseModel.message = PurchaseRequest.message;
                }
                else
                {
                    responseModel.message = PurchaseRequest.message;
                    responseModel.code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                responseModel.code = (int)HttpStatusCode.InternalServerError;
            }
            return StatusCode(responseModel.code, responseModel);
        }


        [HttpGet]
        [Route("CheckPRNo")]
        public IActionResult CheckPRNo()
        {
            var checkPRNo = purchaseRequest.CheckPRNo();
            return Ok(new { code = 200, data = checkPRNo.ToString() });
        }

        [HttpPost]
        [Route("GetPRList")]
        public async Task<IActionResult> GetPRList(DataTableRequstModel PRdataTable)
        {
            var purchaseRequestList = await purchaseRequest.GetPRList(PRdataTable);
            return Ok(new { code = 200, data = purchaseRequestList });
        }

        [HttpGet]
        [Route("PurchaseRequestDetailsByPrNo")]
        public async Task<IActionResult> PurchaseRequestDetailsByPrNo(string PrNo)
        {
            var prdetails = await purchaseRequest.PurchaseRequestDetailsByPrNo(PrNo);
            return Ok(new { code = 200, data = prdetails });
        }

        [HttpPost]
        [Route("ApproveUnapprovePR")]
        public async Task<IActionResult> ApproveUnapprovePR(string PrNo)
        {
            UserResponceModel responseModel = new UserResponceModel();
            var PurchaseRequest = await purchaseRequest.ApproveUnapprovePR(PrNo);
            try
            {
                if (PurchaseRequest != null)
                {
                    responseModel.Code = (int)HttpStatusCode.OK;
                    responseModel.Message = PurchaseRequest.Message;
                }
                else
                {
                    responseModel.Message = PurchaseRequest.Message;
                    responseModel.Code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                responseModel.Code = (int)HttpStatusCode.InternalServerError;
            }
            return StatusCode(responseModel.Code, responseModel);
        }
    }
}
