using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.POMaster;
using EMPManegment.Inretface.Interface.InvoiceMaster;
using EMPManegment.Inretface.Interface.OrderDetails;
using EMPManegment.Inretface.Services.PurchaseOrderSevices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CreatePO(OPMasterView POList)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var createInvoice = POServices.CreatePO(POList);
                if (createInvoice.Result.Code == 200)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = createInvoice.Result.Message;
                }
                else
                {
                    response.Message = createInvoice.Result.Message;
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


    }
}
