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
#nullable disable
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

        [HttpPost]
        [Route("GetInvoiceDetailsList")]
        public async Task<IActionResult> GetInvoiceDetailsList(DataTableRequstModel InvoiceList)
        {
            try
            {
                var AllInvoiceList = await InvoiceMaster.GetInvoiceDetailsList(InvoiceList);
                return Ok(new { code = (int)HttpStatusCode.OK, data = AllInvoiceList });
            }
            catch (Exception ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpGet]
        [Route("CheckInvoiceNo")]
        public IActionResult CheckInvoiceNo(string porjectname)
        {
            try
            {
                var checkInvoice = InvoiceMaster.CheckInvoiceNo(porjectname);
                return Ok(new { code = (int)HttpStatusCode.OK, data = checkInvoice });
            }
            catch (Exception ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpPost]
        [Route("InsertInvoiceDetails")]
        public async Task<IActionResult> InsertInvoiceDetails(InvoiceMasterModel InvoiceList)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var createInvoice = InvoiceMaster.InsertInvoiceDetails(InvoiceList);
                if (createInvoice.Result.Code != (int)HttpStatusCode.InternalServerError)
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
                if (orderdetails.Result.Code != (int)HttpStatusCode.InternalServerError)
                {
                    response.Data = orderdetails.Result.Data;
                    response.Code = (int)HttpStatusCode.OK;

                }
                else
                {
                    response.Message = orderdetails.Result.Message;
                    response.Code = orderdetails.Result.Code;
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
                return Ok(new { code = (int)HttpStatusCode.OK, data = vendorInloviceList });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpGet]
        [Route("GetLastTransactionByVendorId")]
        public async Task<IActionResult> GetLastTransactionByVendorId(Guid Vid)
        {
            try
            {
                IEnumerable<CreditDebitView> vendorInloviceList = await InvoiceMaster.GetLastTransactionByVendorId(Vid);
                return Ok(new { code = (int)HttpStatusCode.OK, data = vendorInloviceList });
            }
            catch (Exception ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpPost]
        [Route("GetAllTransactionByVendorId")]
        public async Task<IActionResult> GetAllTransactionByVendorId(Guid Vid, DataTableRequstModel dataTable)
        {
            try
            {
                var CreditList = await InvoiceMaster.GetAllTransactionByVendorId(Vid, dataTable);
                return Ok(new { code = (int)HttpStatusCode.OK, data = CreditList });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpPost]
        [Route("GetAllTransaction")]
        public async Task<IActionResult> GetAllTransaction()
        {
            try
            {
                var allCreditList = await InvoiceMaster.GetAllTransaction();
                return Ok(new { code = (int)HttpStatusCode.OK, data = allCreditList.ToList() });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
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
                if (insertcreditdebit.Result.Code != (int)HttpStatusCode.InternalServerError)
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
            try
            {
                IEnumerable<CreditDebitView> creditdebit = await InvoiceMaster.GetCreditDebitListByVendorId(Vid);
                return Ok(new { code = (int)HttpStatusCode.OK, data = creditdebit.ToList() });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpGet]
        [Route("DisplayInvoiceDetails")]
        public async Task<IActionResult> DisplayInvoiceDetails(string OrderId)
        {
            PurchaseOrderResponseModel response = new PurchaseOrderResponseModel();
            try
            {
                var orderdetails = InvoiceMaster.DisplayInvoiceDetails(OrderId);
                if (orderdetails.Result.Code != (int)HttpStatusCode.InternalServerError)
                {
                    response.Data = orderdetails.Result.Data;
                    response.Code = (int)HttpStatusCode.OK;
                }
                else
                {
                    response.Message = orderdetails.Result.Message;
                    response.Code = (int)HttpStatusCode.BadRequest;
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
                if (invoice.Code != (int)HttpStatusCode.NotFound && invoice.Code != (int)HttpStatusCode.InternalServerError)
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
            try
            {
                var getinvoicedetails = await InvoiceMaster.EditInvoiceDetails(InvoiceNo);
                return Ok(new { code = (int)HttpStatusCode.OK, data = getinvoicedetails });
            }
            catch (Exception ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpPost]
        [Route("UpdateInvoiceDetails")]
        public async Task<IActionResult> UpdateInvoiceDetails(InvoiceMasterModel invoiceDetails)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var updateinvoice = InvoiceMaster.UpdateInvoiceDetails(invoiceDetails);
                if (updateinvoice.Result.Code != (int)HttpStatusCode.InternalServerError)
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
                if (orderdetails.Result.Code != (int)HttpStatusCode.NotFound && orderdetails.Result.Code != (int)HttpStatusCode.InternalServerError)
                {
                    response.Data = orderdetails.Result.Data;
                    response.Code = (int)HttpStatusCode.OK;
                }
                else
                {
                    response.Message = orderdetails.Result.Message;
                    response.Code = (int)HttpStatusCode.BadRequest;
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
        public async Task<IActionResult> InvoiceActivity(Guid ProjectId)
        {
            try
            {
                IEnumerable<InvoiceViewModel> emplist = await InvoiceMaster.InvoiceActivity(ProjectId);
                return Ok(new { code = (int)HttpStatusCode.OK, data = emplist.ToList() });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpGet]
        [Route("DisplayInvoiceDetailsById")]
        public async Task<IActionResult> DisplayInvoiceDetailsById(Guid Id)
        {
            try
            {
                var emplist = await InvoiceMaster.DisplayInvoiceDetailsById(Id);
                return Ok(new { code = (int)HttpStatusCode.OK, data = emplist });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpGet]
        [Route("GetProductDetailsById")]
        public async Task<IActionResult> GetProductDetailsById(Guid ProductId)
        {
            try
            {
                var Product = await InvoiceMaster.GetProductDetailsById(ProductId);
                return Ok(new { code = (int)HttpStatusCode.OK, data = Product });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpGet]
        [Route("InvoicActivityByUserId")]
        public async Task<IActionResult> InvoicActivityByUserId(Guid UserId)
        {
            try
            {
                IEnumerable<InvoiceViewModel> invoicelist = await InvoiceMaster.InvoicActivityByUserId(UserId);
                return Ok(new { code = (int)HttpStatusCode.OK, data = invoicelist.ToList() });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpPost]
        [Route("DeleteTransaction")]
        public async Task<IActionResult> DeleteTransaction(int Id)
        {
            UserResponceModel responseModel = new UserResponceModel();
            var GetTrandata = await InvoiceMaster.DeleteTransaction(Id);
            try
            {
                if (GetTrandata.Code != (int)HttpStatusCode.NotFound && GetTrandata.Code != (int)HttpStatusCode.InternalServerError)
                {
                    responseModel.Code = (int)HttpStatusCode.OK;
                    responseModel.Message = GetTrandata.Message;
                }
                else
                {
                    responseModel.Message = GetTrandata.Message;
                    responseModel.Code = GetTrandata.Code;
                }
            }
            catch (Exception)
            {
                responseModel.Code = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = "An error occurred while processing the request.";
            }
            return StatusCode(responseModel.Code, responseModel);
        }
    }
}
