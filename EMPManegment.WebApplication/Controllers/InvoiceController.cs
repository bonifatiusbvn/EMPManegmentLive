﻿using Microsoft.AspNetCore.Mvc;
using EMPManegment.EntityModels.ViewModels.SalesFolder;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using Newtonsoft.Json;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.Invoice;
using Microsoft.AspNetCore.Hosting;
using Aspose.Pdf.Operators;
using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.Web.Models;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using DinkToPdf.Contracts;
using Aspose.Pdf.Facades;
using DinkToPdf;
using Newtonsoft.Json.Converters;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;




namespace EMPManegment.Web.Controllers
{
    public class InvoiceController : Controller
    {
        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }
        public UserSession _userSession { get; }
        public IConverter PdfConverter { get; }

        public InvoiceController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices, UserSession userSession, IConverter pdfConverter)
        {
            WebAPI = webAPI;
            Environment = environment;
            APIServices = aPIServices;
            _userSession = userSession;
            PdfConverter = pdfConverter;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateInvoice()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GenerateInvoice(InvoiceViewModel data)
        {
            return Json(data);
        }
        [HttpGet]
        public async Task<IActionResult> GenerateInvoicePDF()
        {
            return View();
        }

        public async Task<IActionResult> InvoiceDetails(string OrderId)
        {
            try
            {
                ApiResponseModel Response = await APIServices.GetAsync("", "Invoice/CheckInvoiceNo?OrderId=" + OrderId);
                List<OrderDetailView> order = new List<OrderDetailView>();
                ApiResponseModel response = await APIServices.GetAsync("", "Invoice/GetInvoiceDetailsByOrderId?OrderId=" + OrderId);
                if (response.code == 200)
                {
                    order = JsonConvert.DeserializeObject<List<OrderDetailView>>(response.data.ToString());
                    response.data = order;

                }
                if (Response.code == 200)
                {
                    ViewBag.InvoiceNo = Response.data;
                }
                return View(order);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetInvoiceDetailsByOrderId(string OrderId)
        {
            try
            {
                ApiResponseModel response = await APIServices.GetAsync("", "Invoice/GetInvoiceDetailsByOrderId?OrderId=" + OrderId);
                if (response.code == 200)
                {
                    return Ok(new { Code = response.code });
                }
                else
                {
                    return new JsonResult(new { Message = string.Format(response.message), Code = response.code, Icone = "warning" });
                }
                //return View("InvoiceDetails", response.data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GenerateInvoiceNoById(Guid Id)
        {
            try
            {
                InvoiceViewModel products = new InvoiceViewModel();
                ApiResponseModel response = await APIServices.GetAsync("", "Invoice/GetInvoiceDetailsById?Id=" + Id);
                if (response.code == 200)
                {
                    products = JsonConvert.DeserializeObject<InvoiceViewModel>(response.data.ToString());
                }
                return View("GenerateInvoicePDF", products);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> InvoiceListView()
        {
            try
            {
                List<InvoiceViewModel> invoice = new List<InvoiceViewModel>();
                ApiResponseModel response = await APIServices.GetAsyncId(null, "Invoice/GetInvoiceDetailsList");
                if (response.code == 200)
                {
                    invoice = JsonConvert.DeserializeObject<List<InvoiceViewModel>>(response.data.ToString());
                }
                return View(invoice);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertInvoiceDetails()
        {
            try
            {
                var InvoiceDetails = HttpContext.Request.Form["INVOICEDETAILS"];
                var format = "dd/MM/yyyy";
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
                var InsertDetails = JsonConvert.DeserializeObject<GenerateInvoiceModel>(InvoiceDetails, dateTimeConverter);

                ApiResponseModel postuser = await APIServices.PostAsync(InsertDetails, "Invoice/InsertInvoiceDetails");
                if (postuser.code == 200)
                {
                    return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
                }
                else
                {
                    return new JsonResult(new { Message = string.Format(postuser.message), Icone = string.Format(postuser.Icone), Code = postuser.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> DownloadPdf()
        {
            string Content = HttpContext.Request.Form["DOWNLOADINVOICE"];

            var globalSettings = new GlobalSettings
            {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Landscape,
                DPI = 300
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = Content,
                WebSettings = { DefaultEncoding = "utf-8" },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Footer" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileContent = PdfConverter.Convert(pdf);

            return File(fileContent, "application/pdf", "document.pdf");
        }

        [HttpGet]
        public async Task<IActionResult> CreditDebitListView()
        {
            try
            {
                List<CreditDebitView> invoice = new List<CreditDebitView>();
                ApiResponseModel response = await APIServices.GetAsyncId(null, "Invoice/GetInvoiceDetailsList");
                if (response.code == 200)
                {
                    invoice = JsonConvert.DeserializeObject<List<CreditDebitView>>(response.data.ToString());
                }
                return View(invoice);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetVendorList()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                var dataTable = new DataTableRequstModel
                {
                    draw = draw,
                    start = start,
                    pageSize = pageSize,
                    skip = skip,
                    lenght = length,
                    searchValue = searchValue,
                    sortColumn = sortColumn,
                    sortColumnDir = sortColumnDir
                };
                List<VendorDetailsView> vendorList = new List<VendorDetailsView>();
                var data = new jsonData();
                ApiResponseModel res = await APIServices.PostAsync(dataTable, "Vendor/GetVendorList");
                if (res.code == 200)
                {
                    data = JsonConvert.DeserializeObject<jsonData>(res.data.ToString());
                    vendorList = JsonConvert.DeserializeObject<List<VendorDetailsView>>(data.data.ToString());
                }
                var jsonData = new
                {
                    draw = data.draw,
                    recordsFiltered = data.recordsFiltered,
                    recordsTotal = data.recordsTotal,
                    data = vendorList,
                };
                return new JsonResult(jsonData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        public async Task<IActionResult> VendorInvoiceListView(Guid Vid)
        {
            try
            {
                InvoiceViewModel products = new InvoiceViewModel();
                ApiResponseModel response = await APIServices.GetAsync("", "Invoice/GetInvoiceListByVendorId?Id=" + Vid);
                if (response.code == 200)
                {
                    products = JsonConvert.DeserializeObject<InvoiceViewModel>(response.data.ToString());
                }
                return View(products);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


}
