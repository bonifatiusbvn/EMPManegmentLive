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
        [HttpGet]
        [Route("GetInvoiceDetailsList")]
        public async Task<IActionResult> GetInvoiceDetailsList()
        {
            var invoiceList = await InvoiceMaster.GetInvoiceDetailsList();
            return Ok(new { code = 200, data = invoiceList.ToList() });
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
        public async Task<IActionResult> InsertInvoiceDetails(GenerateInvoiceModel InvoiceList)
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
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Icone = createInvoice.Result.Icone;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StatusCode(response.Code, response);
        }
        [HttpGet]
        [Route("GetInvoiceDetailsByOrderId")]
        public async Task<IActionResult> GetInvoiceDetailsByOrderId(string OrderId)
        {
            OrderResponseModel response = new OrderResponseModel();
            try
            {
                var orderdetails = InvoiceMaster.GetInvoiceDetailsByOrderId(OrderId);
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
                throw ex;
            }
            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Route("GetInvoiceListByVendorId")]
        public async Task<IActionResult> GetInvoiceListByVendorId(Guid Vid)
        {
            try
            {
                IEnumerable<InvoiceViewModel> vendorInloviceList = await InvoiceMaster.GetInvoiceListByVendorId(Vid);
                return Ok(new { code = 200, data = vendorInloviceList });
            }
            catch (Exception ex)
            {

                throw ex;
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

                throw ex;
            }


        }

        [HttpGet]
        [Route("GetAllTransactionByVendorId")]
        public async Task<IActionResult> GetAllTransactionByVendorId(Guid Vid)
        {
            try
            {
                IEnumerable<CreditDebitView> allCreditList = await InvoiceMaster.GetAllTransactionByVendorId(Vid);
                return Ok(new { code = 200, data = allCreditList });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("GetAllTransaction")]
        public async Task<IActionResult> GetAllTransaction(DataTableRequstModel CreditList)
        {
            try
            {
                var allCreditList = await InvoiceMaster.GetAllTransaction(CreditList);
                return Ok(new { code = 200, data = allCreditList });
            }
            catch (Exception ex)
            {

                throw ex;
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
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
            OrderResponseModel response = new OrderResponseModel();
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
                throw ex;
            }
            return StatusCode(response.Code, response);
        }
        [HttpPost]
        [Route("IsDeletedInvoice")]
        public async Task<IActionResult> IsDeletedInvoice(string InvoiceNo)
        {
            UserResponceModel responseModel = new UserResponceModel();
            var invoice = await InvoiceMaster.IsDeletedInvoice(InvoiceNo);
            try
            {
                if (invoice != null)
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
                throw ex;
            }
            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Route("ShowInvoiceDetailsByOrderId")]
        public async Task<IActionResult> ShowInvoiceDetailsByOrderId(string OrderId)
        {
            OrderResponseModel response = new OrderResponseModel();
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
                throw ex;
            }
            return StatusCode(response.Code, response);
        }
    }
}
