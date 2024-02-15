using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Pdf;
using PdfSharpCore;
using TheArtOfDev.HtmlRenderer.PdfSharp;
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

namespace EMPManegment.Web.Controllers
{
    public class InvoiceController : Controller
    {
        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }
        public UserSession _userSession { get; }

        public InvoiceController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices, UserSession userSession)
        {
            WebAPI = webAPI;
            Environment = environment;
            APIServices = aPIServices;
            _userSession = userSession;
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
        public async Task<IActionResult> InvoiceDetails()
        {
            return View();
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
    }
}
