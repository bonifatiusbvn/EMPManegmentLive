using DinkToPdf.Contracts;
using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.ManualInvoice;
using EMPManegment.Web.Helper;
using EMPManegment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EMPManegment.Web.Controllers
{
    public class ManualInvoiceController : Controller
    {
        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }

        public ManualInvoiceController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices)
        {
            WebAPI = webAPI;
            Environment = environment;
            APIServices = aPIServices;
        }

        [FormPermissionAttribute("CreateInvoiceManual-View")]
        public IActionResult CreateInvoiceManual()
        {
            return View();
        }

        [FormPermissionAttribute("CreateInvoiceManual-Add")]
        [HttpPost]
        public async Task<IActionResult> InsertManualInvoice()
        {
            try
            {
                var InvoiceDetails = HttpContext.Request.Form["ManualInvoiceDetails"];
                var InsertDetails = JsonConvert.DeserializeObject<ManualInvoiceMasterModel>(InvoiceDetails);

                ApiResponseModel postuser = await APIServices.PostAsync(InsertDetails, "ManualInvoice/InsertManualInvoice");
                if (postuser.code == 200)
                {
                    return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
                }
                else
                {
                    return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
