using DinkToPdf.Contracts;
using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.ManualInvoice;
using EMPManegment.Web.Helper;
using EMPManegment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
                List<ManualInvoiceMasterModel> InvoiceList = new List<ManualInvoiceMasterModel>();
                var data = new jsonData();
                ApiResponseModel postuser = await APIServices.PostAsync(dataTable, "ManualInvoice/GetManualInvoiceList");
                if (postuser.data != null)
                {
                    data = JsonConvert.DeserializeObject<jsonData>(postuser.data.ToString());
                    InvoiceList = JsonConvert.DeserializeObject<List<ManualInvoiceMasterModel>>(data.data.ToString());
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
        public IActionResult CreateInvoiceManual()
        {
            return View();
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
    }
}
