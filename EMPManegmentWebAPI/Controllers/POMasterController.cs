using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.POMaster;
using EMPManegment.Inretface.Interface.InvoiceMaster;
using EMPManegment.Inretface.Interface.OrderDetails;
using EMPManegment.Inretface.Services.PurchaseOrderSevices;
using EMPManegment.Inretface.Services.TaskServices;
using EMPManegment.Services.VendorDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class POMasterController : ControllerBase
    {
        public POMasterController(IPOServices pOServices)
        {
            POServices = pOServices;
        }

        public IPOServices POServices { get; }

        [HttpPost]
        [Route("CreatePO")]
        public async Task<IActionResult> CreatePO(List<OPMasterView> createPO)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var purchaseorder = POServices.CreatePO(createPO);
                if (purchaseorder.Result.Code == 200)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = purchaseorder.Result.Message;
                }
                else
                {
                    response.Message = purchaseorder.Result.Message;
                    response.Code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Route("CheckOPNo")]
        public async Task<IActionResult> CheckOPNo(string projectname)
        {
            try
            {
                var checkOPNo = POServices.CheckOPNo(projectname);
                return Ok(new { code = 200, data = checkOPNo });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = ex.Message });
            }
        }

        [HttpPost]
        [Route("GetPOList")]
        public async Task<IActionResult> GetPOList(DataTableRequstModel POList)
        {
            var AllPOList = await POServices.GetPOList(POList);
            return Ok(new { code = 200, data = AllPOList });
        }

        [HttpGet]
        [Route("DisplayPODetails")]
        public async Task<IActionResult> DisplayPODetails(string POId)
        {
            POResponseModel response = new POResponseModel();
            try
            {
                var POdetails = POServices.DisplayPODetails(POId);
                if (POdetails.Result.Code == 400)
                {
                    response.Message = POdetails.Result.Message;
                    response.Code = (int)HttpStatusCode.BadRequest;
                }
                else
                {
                    response.Data = POdetails.Result.Data;
                    response.Code = (int)HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StatusCode(response.Code, response);
        }
    }
}
