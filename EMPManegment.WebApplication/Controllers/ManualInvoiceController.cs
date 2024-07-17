using DinkToPdf.Contracts;
using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.ManualInvoice;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.Web.Helper;
using EMPManegment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
#nullable disable
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

        [FormPermissionAttribute("ManualInvoiceList-View")]
        public async Task<IActionResult> ManualInvoices()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetManualInvoiceList()
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
                List<ManualInvoiceModel> InvoiceList = new List<ManualInvoiceModel>();
                var data = new jsonData();
                ApiResponseModel postuser = await APIServices.PostAsync(dataTable, "ManualInvoice/GetManualInvoiceList");
                if (postuser.data != null)
                {
                    data = JsonConvert.DeserializeObject<jsonData>(postuser.data.ToString());
                    InvoiceList = JsonConvert.DeserializeObject<List<ManualInvoiceModel>>(data.data.ToString());
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

        [FormPermissionAttribute("CreateInvoiceManual-View")]
        public async Task<IActionResult> CreateInvoiceManual(Guid? InvoiceId)
        {
            try
            {
                ManualInvoiceMasterModel invoiceDetails = new ManualInvoiceMasterModel();
                if (InvoiceId != null)
                {
                    ApiResponseModel response = await APIServices.GetAsync("", "ManualInvoice/GetManualInvoiceDetails?InvoiceId=" + InvoiceId);
                    if (response.code == 200)
                    {
                        invoiceDetails = JsonConvert.DeserializeObject<ManualInvoiceMasterModel>(response.data.ToString());
                        var row = 0;
                        foreach (var item in invoiceDetails.ManualInvoiceDetails)
                        {
                            item.RowNumber = row++;
                        }
                    }
                }
                return View(invoiceDetails);
            }
            catch (Exception ex)
            {
                throw ex;

            }
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
                    return Ok(new { Message = string.Format(postuser.message), Code = postuser.code, Data = postuser.data });
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

        [FormPermissionAttribute("CreateInvoiceManual-Delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteManualInvoice(Guid InvoiceId)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync("", "ManualInvoice/DeleteManualInvoice?InvoiceId=" + InvoiceId);
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

        [FormPermissionAttribute("ManualInvoiceDetails-View")]
        public async Task<IActionResult> ManualInvoiceDetails(Guid InvoiceId)
        {
            try
            {
                ManualInvoiceMasterModel invoice = new ManualInvoiceMasterModel();
                ApiResponseModel response = await APIServices.GetAsync("", "ManualInvoice/GetManualInvoiceDetails?InvoiceId=" + InvoiceId);
                if (response.code == 200)
                {
                    invoice = JsonConvert.DeserializeObject<ManualInvoiceMasterModel>(response.data.ToString());
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


        [FormPermissionAttribute("CreateInvoiceManual-Edit")]
        [HttpPost]
        public async Task<IActionResult> UpdateManualInvoice()
        {
            try
            {
                var InvoiceDetails = HttpContext.Request.Form["UpdateManualInvoice"];
                var UpdateDetails = JsonConvert.DeserializeObject<ManualInvoiceMasterModel>(InvoiceDetails);

                ApiResponseModel postuser = await APIServices.PostAsync(UpdateDetails, "ManualInvoice/UpdateManualInvoice");
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
        public async Task<IActionResult> DisplayProducts()
        {
            try
            {
                List<ManualInvoiceDetailsModel> products = new List<ManualInvoiceDetailsModel>
                {
                    new ManualInvoiceDetailsModel()
                };
                return PartialView("~/Views/ManualInvoice/_AddProductPartialView.cshtml", products);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<IActionResult> ManualInvoicePrintDetails(Guid InvoiceId)
        {
            try
            {
                ManualInvoiceMasterModel invoice = new ManualInvoiceMasterModel();
                ApiResponseModel response = await APIServices.GetAsync("", "ManualInvoice/GetManualInvoiceDetails?InvoiceId=" + InvoiceId);
                if (response.code == 200)
                {
                    invoice = JsonConvert.DeserializeObject<ManualInvoiceMasterModel>(response.data.ToString());
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
        public async Task<IActionResult> PrintManualInvoiceDetails(Guid Id)
        {
            try
            {
                IActionResult result = await ManualInvoicePrintDetails(Id);

                if (result is ViewResult viewResult)
                {
                    var order = viewResult.Model as ManualInvoiceMasterModel;
                    var htmlContent = await RenderViewToStringAsync("ManualInvoicePrintDetails", order, viewResult.ViewData);
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
