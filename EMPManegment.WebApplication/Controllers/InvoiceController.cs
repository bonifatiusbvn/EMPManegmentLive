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
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using Microsoft.AspNetCore.Components;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.Web.Helper;
using Microsoft.AspNetCore.Authorization;



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

        [FormPermissionAttribute("Create Invoice-View")]
        public async Task<IActionResult> CreateInvoice()
        {
            string porjectname = UserSession.ProjectName;
            ApiResponseModel Response = await APIServices.GetAsync("", "Invoice/CheckInvoiceNo?porjectname=" + porjectname);
            if (Response.code == 200)
            {
                ViewBag.InvoiceNo = Response.data;
            }
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

        [FormPermissionAttribute("InvoiceDetails-View")]
        public async Task<IActionResult> OrderInvoiceDetails(string OrderId)
        {
            try
            {
                string porjectname = UserSession.ProjectName;
                ApiResponseModel Response = await APIServices.GetAsync("", "Invoice/CheckInvoiceNo?porjectname=" + porjectname);
                List<PurchaseOrderDetailView> order = new List<PurchaseOrderDetailView>();
                ApiResponseModel response = await APIServices.GetAsync("", "Invoice/GetInvoiceDetailsByOrderId?OrderId=" + OrderId);
                if (response.code == 200)
                {
                    order = JsonConvert.DeserializeObject<List<PurchaseOrderDetailView>>(response.data.ToString());
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

        [FormPermissionAttribute("DisplayInvoiceDetails-View")]
        public async Task<IActionResult> InvoiceDetails(Guid InvoiceId)
        {
            try
            {
                InvoiceMasterModel invoice = new InvoiceMasterModel();
                ApiResponseModel response = await APIServices.GetAsync("", "Invoice/GetInvoiceDetailsById?Id=" + InvoiceId);
                if (response.code == 200)
                {
                    invoice = JsonConvert.DeserializeObject<InvoiceMasterModel>(response.data.ToString());
                    var number = invoice.TotalAmount;
                    var totalAmountInWords = NumberToWords((int)number);
                    ViewData["TotalAmountInWords"] = totalAmountInWords + " " + "Only";
                    var gstamt = invoice.TotalGst;
                    var totalGstInWords = NumberToWords((int)gstamt);
                    ViewData["TotalGstInWords"] = totalGstInWords + " " + "Only";
                }
                return View(invoice);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }
            return words;
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

        [FormPermissionAttribute("InvoiceListView-View")]
        public async Task<IActionResult> Invoices()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetInvoiceListView()
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
                List<InvoiceViewModel> InvoiceList = new List<InvoiceViewModel>();
                var data = new jsonData();
                ApiResponseModel postuser = await APIServices.PostAsync(dataTable, "Invoice/GetInvoiceDetailsList");
                if (postuser.data != null)
                {
                    data = JsonConvert.DeserializeObject<jsonData>(postuser.data.ToString());
                    InvoiceList = JsonConvert.DeserializeObject<List<InvoiceViewModel>>(data.data.ToString());
                }
                var jsonData = new
                {
                    draw = data.draw,
                    recordsFiltered = data.recordsFiltered,
                    recordsTotal = data.recordsTotal,
                    data = InvoiceList,
                };
                return new JsonResult(jsonData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [FormPermissionAttribute("Create Invoice-Add")]
        [HttpPost]
        public async Task<IActionResult> InsertInvoiceDetails()
        {
            try
            {
                var InvoiceDetails = HttpContext.Request.Form["INVOICEDETAILS"];
                var InsertDetails = JsonConvert.DeserializeObject<InvoiceMasterModel>(InvoiceDetails);

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

        [FormPermissionAttribute("VendorInvoiceListView-Add")]
        [HttpPost]
        public async Task<IActionResult> InsertCreditDebitDetails()
        {
            try
            {
                var creditdebit = HttpContext.Request.Form["CREDITDEBITDETAILS"];
                var InsertDetails = JsonConvert.DeserializeObject<CreditDebitView>(creditdebit);

                ApiResponseModel postuser = await APIServices.PostAsync(InsertDetails, "Invoice/InsertCreditDebitDetails");
                if (postuser.code == 200)
                {
                    return Ok(new { Message = string.Format(postuser.message), Code = postuser });
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

        [FormPermissionAttribute("Credit Debit-View")]
        [HttpGet]
        public async Task<IActionResult> VendorTransactions()
        {
            return View();

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

        [FormPermissionAttribute("VendorInvoiceListView-View")]
        [HttpGet]
        public async Task<IActionResult> PayVendors(Guid Vid)
        {
            try
            {
                InvoicePayVendorModel products = new InvoicePayVendorModel();
                ApiResponseModel response = await APIServices.GetAsync("", "Invoice/GetInvoiceListByVendorId?Vid=" + Vid);
                if (response.code == 200)
                {
                    products = JsonConvert.DeserializeObject<InvoicePayVendorModel>(response.data.ToString());
                }
                return View(products);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetLastTransactionByVendorId(Guid Vid)
        {
            try
            {
                List<CreditDebitView> products = new List<CreditDebitView>();
                ApiResponseModel response = await APIServices.GetAsync("", "Invoice/GetLastTransactionByVendorId?Vid=" + Vid);
                if (response.code == 200)
                {
                    products = JsonConvert.DeserializeObject<List<CreditDebitView>>(response.data.ToString());
                }

                return PartialView("~/Views/Invoice/_CreditDebitPartial.cshtml", products);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [FormPermissionAttribute("All Transaction-View")]
        [HttpGet]
        public async Task<IActionResult> VendorAllTransaction(Guid Vid)
        {
            ViewBag.VendorId = Vid;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetVendorTransactionList(Guid? Vid)
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[2][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
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

                List<CreditDebitView> transactionList = new List<CreditDebitView>();
                var data = new jsonData();
                ApiResponseModel response = await APIServices.PostAsync(dataTable, "Invoice/GetAllTransactionByVendorId?Vid=" + Vid);

                if (response.code == 200)
                {
                    data = JsonConvert.DeserializeObject<jsonData>(response.data.ToString());
                    transactionList = JsonConvert.DeserializeObject<List<CreditDebitView>>(data.data.ToString());
                }

                var jsonData = new
                {
                    draw = draw,
                    recordsFiltered = transactionList.Count,
                    recordsTotal = transactionList.Count,
                    data = transactionList,
                };

                return new JsonResult(jsonData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [FormPermissionAttribute("All Transaction-View")]
        [HttpGet]
        public async Task<IActionResult> AllTransaction()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> GetAllTransactiondata()
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
                List<CreditDebitView> transactions = new List<CreditDebitView>();
                var data = new jsonData();
                ApiResponseModel response = await APIServices.PostAsync(dataTable, "Invoice/GetAllTransaction");
                if (response.code == 200)
                {
                    data = JsonConvert.DeserializeObject<jsonData>(response.data.ToString());
                    transactions = JsonConvert.DeserializeObject<List<CreditDebitView>>(data.data.ToString());
                }
                var jsonData = new
                {
                    draw = data.draw,
                    recordsFiltered = data.recordsFiltered,
                    recordsTotal = data.recordsTotal,
                    data = transactions,
                };
                return new JsonResult(jsonData);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetCreditDebitDetailsByVendorId(Guid VendorId)
        {
            try
            {
                List<CreditDebitView> creditdebit = new List<CreditDebitView>();
                ApiResponseModel response = await APIServices.PostAsync("", "Invoice/GetCreditDebitDetailsByVendorId?Vid=" + VendorId);
                if (response.code == 200)
                {
                    creditdebit = JsonConvert.DeserializeObject<List<CreditDebitView>>(response.data.ToString());
                }
                return new JsonResult(creditdebit);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<JsonResult> EditInvoiceDetails(string InvoiceNo)
        {
            try
            {
                UpdateInvoiceModel invoice = new UpdateInvoiceModel();
                ApiResponseModel response = await APIServices.GetAsync("", "Invoice/EditInvoiceDetails?InvoiceNo=" + InvoiceNo);
                if (response.code == 200)
                {
                    invoice = JsonConvert.DeserializeObject<UpdateInvoiceModel>(response.data.ToString());
                }
                return new JsonResult(invoice);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [FormPermissionAttribute("InvoiceListView-Edit")]
        [HttpPost]
        public async Task<IActionResult> UpdateInvoiceDetails(UpdateInvoiceModel invoiceDetails)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync(invoiceDetails, "Invoice/UpdateInvoiceDetails");
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message });
                }
                return View(invoiceDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [FormPermissionAttribute("InvoiceListView-Delete")]
        [HttpPost]
        public async Task<IActionResult> IsDeletedInvoice(Guid InvoiceId)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync("", "Invoice/IsDeletedInvoice?InvoiceId=" + InvoiceId);
                if (postuser.code == 200)
                {
                    return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
                }
                else
                {
                    return new JsonResult(new { Message = string.Format(postuser.message), Code = postuser.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> ShowInvoiceDetailsByOrderId(string OrderId)
        {
            try
            {
                ApiResponseModel response = await APIServices.GetAsync("", "Invoice/ShowInvoiceDetailsByOrderId?OrderId=" + OrderId);
                if (response.code == 200)
                {
                    return Ok(new { Code = response.code });
                }
                else
                {
                    return new JsonResult(new { Message = string.Format(response.message), Code = response.code, Icone = "warning" });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
