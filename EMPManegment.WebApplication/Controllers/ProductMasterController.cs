using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.Crypto;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.Models;
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
        public async Task<IActionResult> AddProductDetails(productDetailsView AddProduct)
        {
            try
            {
                var path = Environment.WebRootPath;
                var filepath = "Content/Image/" + AddProduct.ProductImage.FileName;
                var fullpath = Path.Combine(path, filepath);
                UploadFile(AddProduct.ProductImage, fullpath);
                var ProductDetails = new ProductDetailsView
                {
                    CreatedBy = _userSession.UserId,
                    ProductType = AddProduct.ProductType,
                    ProductName = AddProduct.ProductName,
                    ProductDescription = AddProduct.ProductDescription,
                    ProductShortDescription = AddProduct.ProductShortDescription,
                    ProductImage = filepath,
                    ProductStocks = AddProduct.ProductStocks,
                    PerUnitPrice = AddProduct.PerUnitPrice,
                    Hsn = AddProduct.Hsn,
                    Gst = AddProduct.Gst,
                    PerUnitWithGstprice = AddProduct.PerUnitWithGstprice
                };
                ApiResponseModel postuser = await APIServices.PostAsync(ProductDetails, "ProductMaster/AddProductDetails");
                UserResponceModel responseModel = new UserResponceModel();
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message });
                }
                else
                {

                    return Ok(new { postuser.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UploadFile(IFormFile ImageFile, string ImagePath)
        {
            FileStream stream = new FileStream(ImagePath, FileMode.Create);
            ImageFile.CopyTo(stream);
        }

        [HttpGet]
        public async Task<JsonResult> GetVendorsNameList()
        {
            try
            {
                List<VendorDetailsView> VendorNameList = new List<VendorDetailsView>();
                HttpClient client = WebAPI.Initil();
                ApiResponseModel res = await APIServices.GetAsync("", "VendorController/GetVendorsNameList");
                if (res.code == 200)
                {
                    VendorNameList = JsonConvert.DeserializeObject<List<VendorDetailsView>>(res.data.ToString());
                }
                return new JsonResult(VendorNameList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
