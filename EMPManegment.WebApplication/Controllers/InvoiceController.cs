using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public IActionResult InvoiceDetails()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetInvoiceDetailsByOrderId(string OrderId)
        {
            try
            {
                List<OrderDetailView> order = new List<OrderDetailView>();
                ApiResponseModel Response = await APIServices.GetAsync("", "Invoice/CheckInvoiceNo?OrderId=" + OrderId);
                ApiResponseModel response = await APIServices.GetAsync("", "Invoice/GetInvoiceDetailsByOrderId?OrderId=" + OrderId);
                if (response.code == 200 && Response.code == 200)
                {
                    order = JsonConvert.DeserializeObject<List<OrderDetailView>>(response.data.ToString());
                    ViewBag.InvoiceNo = Response.data;
                }
                else
                {
                    return new JsonResult(new { Message = string.Format(response.message), Code = response.code });
                }
                return View("InvoiceDetails", response.data);
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

        [HttpGet]
        public async Task<JsonResult> GetInvoiceNoList()
        {
            try
            {
                List<InvoiceViewModel> InvoiceNoList = new List<InvoiceViewModel>();
                ApiResponseModel res = await APIServices.GetAsync("", "Invoice/GetInvoiceNoList");
                if (res.code == 200)
                {
                    InvoiceNoList = JsonConvert.DeserializeObject<List<InvoiceViewModel>>(res.data.ToString());
                }
                return new JsonResult(InvoiceNoList);
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
    }
}
