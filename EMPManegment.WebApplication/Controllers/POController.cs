using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.POMaster;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using EMPManegment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EMPManegment.Web.Controllers
{
    public class POController : Controller
    {
        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }
        public UserSession _userSession { get; }
        public POController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices, UserSession userSession)
        {
            WebAPI = webAPI;
            Environment = environment;
            APIServices = aPIServices;
            _userSession = userSession;
        }
        public async Task<IActionResult> CreateOP()
        {
            try
            {
                string projectname = UserSession.ProjectName;
                ApiResponseModel Response = await APIServices.GetAsync("", "POMaster/CheckOPNo?projectname=" + projectname);
                if (Response.code == 200)
                {
                    ViewBag.OrderId = Response.data;
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePO()
        {
            try
            {
                var PurchaseOrder = HttpContext.Request.Form["PURCHASEORDER"];
                var InsertDetails = JsonConvert.DeserializeObject<List<OPMasterView>>(PurchaseOrder.ToString());
                ApiResponseModel postuser = await APIServices.PostAsync(InsertDetails, "POMaster/CreatePO");
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message });
                }
                else
                {
                    return Ok(new { postuser.message });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> GetPOList()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetPurchaseOrderList()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][POId]"].FirstOrDefault();
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
                List<OPMasterView> POList = new List<OPMasterView>();
                var data = new jsonData();
                ApiResponseModel postuser = await APIServices.PostAsync(dataTable, "POMaster/GetPOList");
                if (postuser.data != null)
                {
                    data = JsonConvert.DeserializeObject<jsonData>(postuser.data.ToString());
                    POList = JsonConvert.DeserializeObject<List<OPMasterView>>(data.data.ToString());
                }
                var jsonData = new
                {
                    draw = data.draw,
                    recordsFiltered = data.recordsFiltered,
                    recordsTotal = data.recordsTotal,
                    data = POList,
                };
                return new JsonResult(jsonData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IActionResult> DisplayPODetails(string POId)
        {
            try
            {
                List<OPMasterView> PODetails = new List<OPMasterView>();
                ApiResponseModel response = await APIServices.GetAsync("", "POMaster/DisplayPODetails?POId=" + POId);
                if (response.code == 200)
                {
                    PODetails = JsonConvert.DeserializeObject<List<OPMasterView>>(response.data.ToString());
                    response.data = PODetails;

                }
                return View(PODetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

