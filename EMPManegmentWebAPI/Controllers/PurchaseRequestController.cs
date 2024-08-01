using Azure;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
#nullable disable
namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            try
            {
                var PurchaseRequest = await purchaseRequest.CreatePurchaseRequest(AddPurchaseRequest);
                if (PurchaseRequest.code != (int)HttpStatusCode.InternalServerError)
                {
                    response.code = (int)HttpStatusCode.OK;
                    response.message = PurchaseRequest.message;
                }
                else
                {
                    response.code = PurchaseRequest.code;
                    response.message = PurchaseRequest.message;
                }
            }
            catch (Exception ex)
            {
                response.code = (int)HttpStatusCode.InternalServerError;
                response.message = "An error occurred while processing the request.";
            }
            return StatusCode(response.code, response);
        }

        [HttpPost]
        [Route("GetPurchaseRequestList")]
        public async Task<IActionResult> GetPurchaseRequestList()
        {
            IEnumerable<PurchaseRequestModel> PurchaseRequest = await purchaseRequest.GetPurchaseRequestList();
            return Ok(new { code = (int)HttpStatusCode.OK, data = PurchaseRequest.ToList() });
        }

        [HttpGet]
        [Route("GetPurchaseRequestDetailsById")]
        public async Task<IActionResult> GetPurchaseRequestDetailsById(string PrNo)
        {
            var purchaseRequestDetails = await purchaseRequest.PurchaseRequestDetailsByPrNo(PrNo);
            return Ok(new { code = (int)HttpStatusCode.OK, data = purchaseRequestDetails });
        }

        [HttpPost]
        [Route("UpdatePurchaseRequestDetails")]
        public async Task<IActionResult> UpdatePurchaseRequestDetails(PurchaseRequestMasterView PurchaseRequestDetails)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
                var UpdatePurchaseRequest = purchaseRequest.UpdatePurchaseRequestDetails(PurchaseRequestDetails);
                if (UpdatePurchaseRequest.Result.code != (int)HttpStatusCode.InternalServerError)
                {
                    response.code = (int)HttpStatusCode.OK;
                    response.message = UpdatePurchaseRequest.Result.message;
                }
                else
                {
                    response.message = UpdatePurchaseRequest.Result.message;
                    response.code = UpdatePurchaseRequest.Result.code;
                }
            }
            catch (Exception ex)
            {
                response.code = (int)HttpStatusCode.InternalServerError;
                response.message = "An error occurred while processing the request.";
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
                    responseModel.code = PurchaseRequest.code;
                }
            }
            catch (Exception ex)
            {
                responseModel.code = (int)HttpStatusCode.InternalServerError;
                responseModel.message = "An error occurred while processing the request.";
            }
            return StatusCode(responseModel.code, responseModel);
        }

        [HttpGet]
        [Route("CheckPRNo")]
        public IActionResult CheckPRNo()
        {
            var checkPRNo = purchaseRequest.CheckPRNo();
            return Ok(new { code = (int)HttpStatusCode.OK, data = checkPRNo.ToString() });
        }

        [HttpPost]
        [Route("GetPRList")]
        public async Task<IActionResult> GetPRList(DataTableRequstModel PRdataTable)
        {
            var purchaseRequestList = await purchaseRequest.GetPRList(PRdataTable);
            return Ok(new { code = (int)HttpStatusCode.OK, data = purchaseRequestList });
        }

        [HttpGet]
        [Route("PurchaseRequestDetailsByPrNo")]
        public async Task<IActionResult> PurchaseRequestDetailsByPrNo(string PrNo)
        {
            var prdetails = await purchaseRequest.PurchaseRequestDetailsByPrNo(PrNo);
            return Ok(new { code = (int)HttpStatusCode.OK, data = prdetails });
        }

        [HttpPost]
        [Route("ApproveUnapprovePR")]
        public async Task<IActionResult> ApproveUnapprovePR(PRIsApprovedMasterModel PRIdList)
        {
            UserResponceModel responseModel = new UserResponceModel();
            var isApproved = await purchaseRequest.ApproveUnapprovePR(PRIdList);
            try
            {
                if (isApproved.Code != (int)HttpStatusCode.InternalServerError)
                {
                    responseModel.Code = (int)HttpStatusCode.OK;
                    responseModel.Message = isApproved.Message;
                }
                else
                {
                    responseModel.Message = isApproved.Message;
                    responseModel.Code = isApproved.Code;
                }
            }
            catch (Exception ex)
            {
                responseModel.Code = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = "An error occurred while processing the request.";
            }
            return StatusCode(responseModel.Code, responseModel);
        }

        [HttpGet]
        [Route("ProductDetailsById")]
        public async Task<IActionResult> ProductDetailsById(Guid ProductId)
        {
            var purchaseRequestDetails = await purchaseRequest.ProductDetailsById(ProductId);
            return Ok(new { code = (int)HttpStatusCode.OK, data = purchaseRequestDetails });
        }
    }
}
