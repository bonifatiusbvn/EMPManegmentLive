using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.Purchase_Request;
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

        public IActionResult CreatePurchaseRequest()
        {
            return View();
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
        public async Task<IActionResult> CreatePurchaseRequest(PurchaseRequestModel AddPurchaseRequest)
        {
            try
            {
                var purchaseRequest = new PurchaseRequestModel()
                {
                    PrId = Guid.NewGuid(),
                    UserId = AddPurchaseRequest.UserId,
                    UserName = AddPurchaseRequest.UserName,
                    ProjectId = AddPurchaseRequest.ProjectId,
                    ProjectName = AddPurchaseRequest.ProjectName,
                    ProductId = AddPurchaseRequest.ProductId,
                    ProductName = AddPurchaseRequest.ProductName,
                    ProductTypeId = AddPurchaseRequest.ProductTypeId,
                    Quantity = AddPurchaseRequest.Quantity,
                    IsApproved = false,
                    IsDeleted = false,
                    CreatedBy = AddPurchaseRequest.CreatedBy,
                    CreatedOn=DateTime.Now,
                };

                ApiResponseModel postUser = await APIServices.PostAsync(purchaseRequest, "PurchaseRequest/CreatePurchaseRequest");
                if (postUser.code == 200)
                {
                    return Ok(new { Message = postUser.message, Code = postUser.code });
                }
                else
                {
                    return new JsonResult(new { Message = string.Format(postUser.message), Code = postUser.code });
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
    }
}
