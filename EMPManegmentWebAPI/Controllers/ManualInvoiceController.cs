using EMPManegment.EntityModels.ViewModels.ManualInvoice;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.InvoiceMaster;
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
