using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using EMPManegment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EMPManegment.Web.Controllers
{
    public class ProductMasterController : Controller
    {
        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }
        public UserSession _userSession { get; }

        public ProductMasterController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices, UserSession userSession)
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

        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProductType(ProductTypeView AddProduct)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync(AddProduct, "ProductMaster/AddProductType");
                if (postuser.code == 200)
                {
                    return Ok(new { Message = string.Format(postuser.message), Icone = string.Format(postuser.Icone), Code = postuser.code });
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
        public async Task<JsonResult> GetProduct()
        {
            try
            {
                List<ProductTypeView> products = new List<ProductTypeView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel response = await APIServices.GetAsyncId(null, "ProductMaster/GetProduct");
                if (response.code == 200)
                {
                    products = JsonConvert.DeserializeObject<List<ProductTypeView>>(response.data.ToString());
                }
                return new JsonResult(products);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
