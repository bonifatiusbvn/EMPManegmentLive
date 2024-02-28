﻿using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.Inretface.Interface.OrderDetails;
using EMPManegment.Inretface.Interface.ProductMaster;
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
        [Route("GetInvoiceNoList")]
        public async Task<IActionResult> GetInvoiceNoList()
        {
            var getInvoiceList = await InvoiceMaster.GetInvoiceNoList();
            return Ok(new { code = 200, data = getInvoiceList.ToList() });
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
        public IActionResult CheckInvoiceNo(string OrderId)
        {
            var checkInvoice = InvoiceMaster.CheckInvoiceNo(OrderId);
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
                    response.Code = (int)HttpStatusCode.NotFound;
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
                var vendorInloviceList = InvoiceMaster.GetInvoiceListByVendorId(Vid);
                return Ok(new { code = 200, data = vendorInloviceList });
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
    }
}
