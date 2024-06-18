using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.ManualInvoice;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.InvoiceMaster;
using EMPManegment.Inretface.Interface.OrderDetails;
using EMPManegment.Inretface.Services.InvoiceMaster;
using EMPManegment.Inretface.Services.ManualInvoiceServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManualInvoiceController : ControllerBase
    {
        private readonly IManualInvoiceServices ManualInvoice;
        public ManualInvoiceController(IManualInvoiceServices manualInvoice)
        {
            ManualInvoice = manualInvoice;
        }

        [HttpPost]
        [Route("InsertManualInvoice")]
        public async Task<IActionResult> InsertManualInvoice(ManualInvoiceMasterModel InvoiceDetails)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var createInvoice = ManualInvoice.InsertManualInvoice(InvoiceDetails);
                if (createInvoice.Result.Code == 200)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = createInvoice.Result.Message;
                    response.Data = createInvoice.Result.Data;
                }
                else
                {
                    response.Message = createInvoice.Result.Message;
                    response.Code = createInvoice.Result.Code;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "An error occurred while processing the request.";
            }
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("GetManualInvoiceList")]
        public async Task<IActionResult> GetManualInvoiceList(DataTableRequstModel dataTable)
        {
            var AllInvoiceList = await ManualInvoice.GetManualInvoiceList(dataTable);
            return Ok(new { code = 200, data = AllInvoiceList });
        }

        [HttpGet]
        [Route("GetManualInvoiceDetails")]
        public async Task<IActionResult> GetManualInvoiceDetails(Guid InvoiceId)
        {
            try
            {
                var invoiceId = await ManualInvoice.GetManualInvoiceDetails(InvoiceId);
                return Ok(new { code = 200, data = invoiceId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = ex.Message });
            }
        }

        [HttpPost]
        [Route("DeleteManualInvoice")]
        public async Task<IActionResult> DeleteManualInvoice(Guid InvoiceId)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var invoiceId = await ManualInvoice.DeleteManualInvoice(InvoiceId);
                if(invoiceId.Code == 200)
                {
                    response.Code = invoiceId.Code;
                    response.Message = invoiceId.Message;
                }
                else
                {
                    response.Code = invoiceId.Code;
                    response.Message = invoiceId.Message;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "An error occurred while processing the request.";
            }
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("UpdateManualInvoice")]
        public async Task<IActionResult> UpdateManualInvoice(ManualInvoiceMasterModel UpdateInvoice)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var createInvoice = ManualInvoice.UpdateManualInvoice(UpdateInvoice);
                if (createInvoice.Result.Code == 200)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = createInvoice.Result.Message;
                }
                else
                {
                    response.Message = createInvoice.Result.Message;
                    response.Code = createInvoice.Result.Code;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "An error occurred while processing the request.";
            }
            return StatusCode(response.Code, response);
        }
    }
}
