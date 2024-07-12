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
using Irony.Parsing.Construction;
using EMPManegment.EntityModels.ViewModels.ManualInvoice;
using Aspose.Foundation.UriResolver.RequestResponses;
using DocumentFormat.OpenXml.Spreadsheet;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Abstractions;

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
        public async Task<IActionResult> CreateInvoice(Guid? Id)
        {
            InvoiceMasterModel InvoiceDetails = new InvoiceMasterModel();
            if (Id != null)
            {
                ApiResponseModel Response = await APIServices.GetAsync("", "Invoice/DisplayInvoiceDetailsById?Id=" + Id);
                if (Response.code == 200)
                {
                    InvoiceDetails = JsonConvert.DeserializeObject<InvoiceMasterModel>(Response.data.ToString());
                    ViewBag.InvoiceNo = InvoiceDetails.InvoiceNo;
                    int rowNumber = 0;
                    foreach (var item in InvoiceDetails.InvoiceDetails)
                    {
                        item.RowNumber = rowNumber++;
                    }
                }
            }
            else
            {
                string porjectname = UserSession.ProjectName;
                ApiResponseModel Response = await APIServices.GetAsync("", "Invoice/CheckInvoiceNo?porjectname=" + porjectname);
                if (Response.code == 200)
                {
                    ViewBag.InvoiceNo = Response.data;
                }
            }
            return View(InvoiceDetails);
        }

        [HttpPost]
        public async Task<IActionResult> DisplayInvoiceProductDetailsListById()
        {
            try
            {
                string ProductId = HttpContext.Request.Form["ProductId"];
                var GetProduct = JsonConvert.DeserializeObject<InvoiceDetailsViewModel>(ProductId.ToString());
                List<InvoiceDetailsViewModel> Product = new List<InvoiceDetailsViewModel>();
                ApiResponseModel response = await APIServices.GetAsync("", "Invoice/GetProductDetailsById?ProductId=" + GetProduct.ProductId);
                if (response.code == 200)
                {
                    Product = JsonConvert.DeserializeObject<List<InvoiceDetailsViewModel>>(response.data.ToString());
                    Product.ForEach(a => a.ProductTotal = (a.PerUnitPrice ?? 0) + (a.GstAmount ?? 0));
                }
                return PartialView("~/Views/Invoice/_DisplayInvoiceProductDetailsPartial.cshtml", Product);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                ApiResponseModel response = await APIServices.GetAsync("", "Invoice/DisplayInvoiceDetailsById?Id=" + InvoiceId);
                if (response.code == 200)
                {
                    invoice = JsonConvert.DeserializeObject<InvoiceMasterModel>(response.data.ToString());
                    var number = invoice.TotalAmount;
                    var totalAmountInWords = NumberToWords((decimal)number);
                    ViewData["TotalAmountInWords"] = totalAmountInWords + " " + "Only";
                    var gstamt = invoice.TotalGst;
                    var totalGstInWords = NumberToWords((decimal)gstamt);
                    ViewData["TotalGstInWords"] = totalGstInWords + " " + "Only";
                }
                return View(invoice);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string NumberToWords(decimal number)
        {
            int integerPart = (int)Math.Floor(number);
            decimal decimalPart = number - integerPart;

            string words = "";

            if (integerPart == 0)
            {
                words = "zero";
            }
            else if (integerPart < 0)
            {
                words = "minus " + NumberToWords(Math.Abs(integerPart));
            }
            else
            {
                if ((integerPart / 1000000) > 0)
                {
                    words += NumberToWords(integerPart / 1000000) + " million ";
                    integerPart %= 1000000;
                }

                if ((integerPart / 1000) > 0)
                {
                    words += NumberToWords(integerPart / 1000) + " thousand ";
                    integerPart %= 1000;
                }

                if ((integerPart / 100) > 0)
                {
                    words += NumberToWords(integerPart / 100) + " hundred ";
                    integerPart %= 100;
                }


                if (integerPart > 0)
                {
                    var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                    var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                    if (words != "")
                        words += " ";

                    if (integerPart < 20)
                        words += unitsMap[integerPart];
                    else
                    {
                        words += tensMap[integerPart / 10];
                        if ((integerPart % 10) > 0)
                            words += "-" + unitsMap[integerPart % 10];
                    }
                }
            }

            if (decimalPart > 0)
            {
                decimalPart *= 100;
                words += " and " + NumberToWords((int)decimalPart) + " paisa";
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
                    return Ok(new { Code = response.code, Message = string.Format(response.message) });
                }
                else
                {
                    return Ok(new { Message = string.Format(response.message), Code = response.code });
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
                ApiResponseModel response = await APIServices.GetAsync("", "Invoice/DisplayInvoiceDetailsById?Id=" + Id);
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
                //var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                //var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
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
                    //sortColumn = sortColumn,
                    //sortColumnDir = sortColumnDir
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
                    return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInvoiceDetails()
        {
            try
            {
                var InvoiceDetails = HttpContext.Request.Form["UPDATEINVOICEDETAILS"];
                var UpdateDetails = JsonConvert.DeserializeObject<InvoiceMasterModel>(InvoiceDetails);

                ApiResponseModel postuser = await APIServices.PostAsync(UpdateDetails, "Invoice/UpdateInvoiceDetails");
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
        public IActionResult AllTransaction()
        {
            return View();
        }
        public async Task<IActionResult> AllVendorTransaction(Guid? VendorId, DateTime? Startdate, DateTime? Enddate)
        {
            try
            {
                List<CreditDebitView> transactions = new List<CreditDebitView>();
                ApiResponseModel response = await APIServices.PostAsync("", "Invoice/GetAllTransaction");
                if (response.code == 200)
                {
                    transactions = JsonConvert.DeserializeObject<List<CreditDebitView>>(response.data.ToString());
                }

                if (VendorId.HasValue)
                {
                    transactions = transactions.Where(e => e.VendorId == VendorId.Value).ToList();
                }

                if (Startdate.HasValue && Enddate.HasValue)
                {
                    transactions = transactions.Where(e => e.Date >= Startdate.Value && e.Date <= Enddate.Value).ToList();
                }

                return PartialView("~/Views/Invoice/_AllTransactionPartial.cshtml", transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
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
                    return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
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
                    return Ok(new { Code = response.code, Message = string.Format(response.message) });
                }
                else
                {
                    return Ok(new { Message = string.Format(response.message), Code = response.code, Icone = "warning" });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTransaction(int Id)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync("", "Invoice/DeleteTransaction?Id=" + Id);
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

        public async Task<IActionResult> InvoicePrintDetails(Guid InvoiceId)
        {
            try
            {
                InvoiceMasterModel invoice = new InvoiceMasterModel();
                ApiResponseModel response = await APIServices.GetAsync("", "Invoice/DisplayInvoiceDetailsById?Id=" + InvoiceId);
                if (response.code == 200)
                {
                    invoice = JsonConvert.DeserializeObject<InvoiceMasterModel>(response.data.ToString());
                    var number = invoice.TotalAmount;
                    var totalAmountInWords = NumberToWords((decimal)number);
                    ViewData["TotalAmountInWords"] = totalAmountInWords + " " + "Only";
                    var gstamt = invoice.TotalGst;
                    var totalGstInWords = NumberToWords((decimal)gstamt);
                    ViewData["TotalGstInWords"] = totalGstInWords + " " + "Only";
                }
                return View(invoice);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IActionResult> PrintInvoiceDetails(Guid Id)
        {
            try
            {
                IActionResult result = await InvoicePrintDetails(Id);

                if (result is ViewResult viewResult)
                {
                    var order = viewResult.Model as InvoiceMasterModel;
                    var htmlContent = await RenderViewToStringAsync("InvoicePrintDetails", order, viewResult.ViewData);
                    return Content(htmlContent, "text/html");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<string> RenderViewToStringAsync(string viewName, object model, ViewDataDictionary viewData)
        {
            var viewEngine = HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
            var tempDataProvider = HttpContext.RequestServices.GetService(typeof(ITempDataProvider)) as ITempDataProvider;
            var tempData = new TempDataDictionary(HttpContext, tempDataProvider);
            var actionContext = new ActionContext(HttpContext, RouteData, new ActionDescriptor());

            using (var stringWriter = new StringWriter())
            {
                var viewResult = viewEngine.FindView(actionContext, viewName, false);

                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"View '{viewName}' was not found.");
                }

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewData,
                    tempData,
                    stringWriter,
                    new HtmlHelperOptions()
                );
                await viewResult.View.RenderAsync(viewContext);
                return stringWriter.ToString();
            }
        }
    }
}
