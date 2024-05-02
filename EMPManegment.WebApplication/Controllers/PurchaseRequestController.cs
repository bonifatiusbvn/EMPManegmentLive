using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.Purchase_Request;
using EMPManegment.EntityModels.ViewModels.PurchaseOrderModels;
using EMPManegment.EntityModels.ViewModels.UserModels;
using EMPManegment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EMPManegment.Web.Controllers
{
    public class PurchaseRequestController : Controller
    {
        public PurchaseRequestController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices)
        {
            WebAPI = webAPI;
            Environment = environment;
            APIServices = aPIServices;
        }
        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }

        public async Task<IActionResult> CreatePurchaseRequest()
        {
            try
            {
                ApiResponseModel Response = await APIServices.GetAsync("", "PurchaseRequest/CheckPRNo");

                if (Response.code == 200)
                {
                    ViewData["PrNo"] = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(Response.data));
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public async Task<IActionResult> GetAllProductDetailsList(string? searchText)
        {
            try
            {
                string apiUrl = $"ProductMaster/GetAllProductList?searchText={searchText}";
                ApiResponseModel response = await APIServices.PostAsync("", apiUrl);
                if (response.code == 200)
                {
                    List<ProductDetailsView> Items = JsonConvert.DeserializeObject<List<ProductDetailsView>>(response.data.ToString());
                    return PartialView("~/Views/PurchaseRequest/_showAllProductPartial.cshtml", Items);
                }
                else
                {
                    return new JsonResult(new { Message = "Failed to retrieve Purchase Order list" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMutiplePurchaseRequest()
        {
            try
            {
                var PurchaseRequestDetails = HttpContext.Request.Form["InsertPRDetails"];
                var PRDetails = JsonConvert.DeserializeObject<PurchaseRequestMasterView>(PurchaseRequestDetails.ToString());
                ApiResponseModel postUser = await APIServices.PostAsync(PRDetails, "PurchaseRequest/CreatePurchaseRequest");
                if (postUser.code == 200)
                {
                    return Ok(new { Message = postUser.message, Code = postUser.code });
                }
                else
                {
                    return Ok(new { Message = postUser.message, Code = postUser.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetPurchaseRequestList()
        {
            try
            {
                List<PurchaseRequestModel> purchaseRequest = new List<PurchaseRequestModel>();
                ApiResponseModel postuser = await APIServices.PostAsync("", "PurchaseRequest/GetPurchaseRequestList");
                if (postuser.data != null)
                {
                    purchaseRequest = JsonConvert.DeserializeObject<List<PurchaseRequestModel>>(postuser.data.ToString());

                }
                else
                {
                    purchaseRequest = new List<PurchaseRequestModel>();
                    ViewBag.Error = "note found";
                }
                purchaseRequest = purchaseRequest.Take(10).ToList();
                return PartialView("~/Views/PurchaseRequest/_DisplayProductDetailPartial.cshtml", purchaseRequest);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IActionResult PurchaseRequestList()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> EditPurchaseRequestDetails(Guid PrId)
        {
            try
            {
                PurchaseRequestModel purchaseRequest = new PurchaseRequestModel();
                ApiResponseModel res = await APIServices.GetAsync("", "PurchaseRequest/EditPurchaseRequestDetails?PrId=" + PrId);
                if (res.code == 200)
                {
                    purchaseRequest = JsonConvert.DeserializeObject<PurchaseRequestModel>(res.data.ToString());
                }
                return new JsonResult(purchaseRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePurchaseRequestDetails(PurchaseRequestModel UpdatePurchaseRequest)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync(UpdatePurchaseRequest, "PurchaseRequest/UpdatePurchaseRequestDetails");
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message });
                }
                return View(UpdatePurchaseRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeletePurchaseRequestDetails(Guid PrId)
        {
            try
            {
                ApiResponseModel purchaseRequest = await APIServices.PostAsync(null, "PurchaseRequest/DeletePurchaseRequestDetails?PrId=" + PrId);
                if (purchaseRequest.code == 200)
                {
                    return Ok(new { Message = string.Format(purchaseRequest.message), Code = purchaseRequest.code });
                }
                else
                {
                    return new JsonResult(new { Message = string.Format(purchaseRequest.message), Code = purchaseRequest.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetPRList()
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
                List<PurchaseRequestModel> purchaseRequestList = new List<PurchaseRequestModel>();
                var data = new jsonData();
                ApiResponseModel res = await APIServices.PostAsync(dataTable, "PurchaseRequest/GetPRList");
                if (res.code == 200)
                {
                    data = JsonConvert.DeserializeObject<jsonData>(res.data.ToString());
                    purchaseRequestList = JsonConvert.DeserializeObject<List<PurchaseRequestModel>>(data.data.ToString());
                }
                var jsonData = new
                {
                    draw = data.draw,
                    recordsFiltered = data.recordsFiltered,
                    recordsTotal = data.recordsTotal,
                    data = purchaseRequestList,
                };
                return new JsonResult(jsonData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
