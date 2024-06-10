using Azure;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.Inretface.Interface.OrderDetails;
using EMPManegment.Inretface.Interface.ProductMaster;
using EMPManegment.Inretface.Interface.ProjectDetails;
using EMPManegment.Inretface.Services.InvoiceMaster;
using EMPManegment.Inretface.Services.ProductMaster;
using EMPManegment.Inretface.Services.TaskServices;
using EMPManegment.Services.VendorDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using System.Net;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceMasterServices InvoiceMaster;
        public InvoiceController(IInvoiceMasterServices invoiceMaster)
        {
            InvoiceMaster = invoiceMaster;
        }

        [HttpGet]
        [Route("GetInvoiceDetailsById")]
        public async Task<IActionResult> GetInvoiceDetailsById(Guid Id)
        {
            var getInvoice = await InvoiceMaster.GetInvoiceDetailsById(Id);
            return Ok(new { code = 200, data = getInvoice });
        }

        [HttpPost]
        [Route("GetInvoiceDetailsList")]
        public async Task<IActionResult> GetInvoiceDetailsList(DataTableRequstModel InvoiceList)
        {
            var AllInvoiceList = await InvoiceMaster.GetInvoiceDetailsList(InvoiceList);
            return Ok(new { code = 200, data = AllInvoiceList });
        }

        [HttpGet]
        [Route("CheckInvoiceNo")]
        public IActionResult CheckInvoiceNo(string porjectname)
        {
            var checkInvoice = InvoiceMaster.CheckInvoiceNo(porjectname);
            return Ok(new { code = 200, data = checkInvoice });
        }

        [HttpPost]
        [Route("InsertInvoiceDetails")]
        public async Task<IActionResult> InsertInvoiceDetails(InvoiceMasterModel InvoiceList)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var createInvoice = InvoiceMaster.InsertInvoiceDetails(InvoiceList);
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

        [HttpGet]
        [Route("GetInvoiceDetailsByOrderId")]
        public async Task<IActionResult> GetInvoiceDetailsByOrderId(string OrderId)
        {
            PurchaseOrderResponseModel response = new PurchaseOrderResponseModel();
            try
            {
                var orderdetails = InvoiceMaster.GetInvoiceDetailsByOrderId(OrderId);
                if (orderdetails.Result.Code == 400)
                {
                    response.Message = orderdetails.Result.Message;
                    response.Code = orderdetails.Result.Code;
                }
                else
                {
                    response.Data = orderdetails.Result.Data;
                    response.Code = (int)HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "An error occurred while processing the request.";
            }
            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Route("GetInvoiceListByVendorId")]
        public async Task<IActionResult> GetInvoiceListByVendorId(Guid Vid)
        {
            try
            {
                InvoicePayVendorModel vendorInloviceList = await InvoiceMaster.GetInvoiceListByVendorId(Vid);
                return Ok(new { code = 200, data = vendorInloviceList });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 500, message = "An error occurred while processing the request." });
            }
        }

        [HttpGet]
        [Route("GetLastTransactionByVendorId")]
        public async Task<IActionResult> GetLastTransactionByVendorId(Guid Vid)
        {
            try
            {
                IEnumerable<CreditDebitView> vendorInloviceList = await InvoiceMaster.GetLastTransactionByVendorId(Vid);
                return Ok(new { code = 200, data = vendorInloviceList });
            }
            catch (Exception ex)
            {

                return BadRequest(new { code = 500, message = "An error occurred while processing the request." });
            }
        }

        [HttpPost]
        [Route("GetAllTransactionByVendorId")]
        public async Task<IActionResult> GetAllTransactionByVendorId(Guid Vid, DataTableRequstModel dataTable)
        {
            try
            {
                var CreditList = await InvoiceMaster.GetAllTransactionByVendorId(Vid, dataTable);
                return Ok(new { code = 200, data = CreditList });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 500, message = "An error occurred while processing the request." });
            }
        }

        [HttpPost]
        [Route("GetAllTransaction")]
        public async Task<IActionResult> GetAllTransaction()
        {
            try
            {
                var allCreditList = await InvoiceMaster.GetAllTransaction();
                return Ok(new { code = 200, data = allCreditList.ToList() });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 500, message = "An error occurred while processing the request." });
            }
        }

        [HttpPost]
        [Route("InsertCreditDebitDetails")]
        public async Task<IActionResult> InsertCreditDebitDetails(CreditDebitView creditdebit)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var insertcreditdebit = InvoiceMaster.InsertCreditDebitDetails(creditdebit);
                if (insertcreditdebit.Result.Code == 200)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = insertcreditdebit.Result.Message;
                }
                else
                {
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Message = insertcreditdebit.Result.Message;
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
        [Route("GetCreditDebitDetailsByVendorId")]
        public async Task<IActionResult> GetCreditDebitDetailsByVendorId(Guid Vid)
        {
            IEnumerable<CreditDebitView> creditdebit = await InvoiceMaster.GetCreditDebitListByVendorId(Vid);
            return Ok(new { code = 200, data = creditdebit.ToList() });
        }

        [HttpGet]
        [Route("DisplayInvoiceDetails")]
        public async Task<IActionResult> DisplayInvoiceDetails(string OrderId)
        {
            PurchaseOrderResponseModel response = new PurchaseOrderResponseModel();
            try
            {
                var orderdetails = InvoiceMaster.DisplayInvoiceDetails(OrderId);
                if (orderdetails.Result.Code == 400)
                {
                    response.Message = orderdetails.Result.Message;
                    response.Code = (int)HttpStatusCode.BadRequest;
                }
                else
                {
                    response.Data = orderdetails.Result.Data;
                    response.Code = (int)HttpStatusCode.OK;
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
        [Route("IsDeletedInvoice")]
        public async Task<IActionResult> IsDeletedInvoice(Guid InvoiceId)
        {
            UserResponceModel responseModel = new UserResponceModel();
            var invoice = await InvoiceMaster.IsDeletedInvoice(InvoiceId);
            try
            {
                if (invoice.Code == 200)
                {
                    responseModel.Code = (int)HttpStatusCode.OK;
                    responseModel.Message = invoice.Message;
                }
                else
                {
                    responseModel.Message = invoice.Message;
                    responseModel.Code = (int)HttpStatusCode.NotFound;
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
        [Route("EditInvoiceDetails")]
        public async Task<IActionResult> EditInvoiceDetails(string InvoiceNo)
        {
            var getinvoicedetails = await InvoiceMaster.EditInvoiceDetails(InvoiceNo);
            return Ok(new { code = 200, data = getinvoicedetails });
        }

        [HttpPost]
        [Route("UpdateInvoiceDetails")]
        public async Task<IActionResult> UpdateInvoiceDetails(UpdateInvoiceModel invoiceDetails)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var updateinvoice = InvoiceMaster.UpdateInvoiceDetails(invoiceDetails);
                if (updateinvoice.Result.Code == 200)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = updateinvoice.Result.Message;
                }
                else
                {
                    response.Message = updateinvoice.Result.Message;
                    response.Code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "An error occurred while processing the request.";
            }
            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Route("ShowInvoiceDetailsByOrderId")]
        public async Task<IActionResult> ShowInvoiceDetailsByOrderId(string OrderId)
        {
            PurchaseOrderResponseModel response = new PurchaseOrderResponseModel();
            try
            {
                var orderdetails = InvoiceMaster.ShowInvoiceDetailsByOrderId(OrderId);
                if (orderdetails.Result.Code == 400)
                {
                    response.Message = orderdetails.Result.Message;
                    response.Code = (int)HttpStatusCode.BadRequest;
                }
                else
                {
                    response.Data = orderdetails.Result.Data;
                    response.Code = (int)HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "An error occurred while processing the request.";
            }
            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Route("InvoiceActivity")]
        public async Task<IActionResult> InvoiceActivity(Guid ProId)
        {
            IEnumerable<InvoiceViewModel> emplist = await InvoiceMaster.InvoicActivity(ProId);
            return Ok(new { code = 200, data = emplist.ToList() });
        }
    }
}
