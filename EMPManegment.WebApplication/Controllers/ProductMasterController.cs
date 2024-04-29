using Aspose.Pdf.Operators;
using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.Crypto;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using EMPManegment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

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
        public async Task<IActionResult> AddProductDetails(ProductRequestModel AddProduct)
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
                List<VendorListDetailsView> VendorNameList = new List<VendorListDetailsView>();
                ApiResponseModel res = await APIServices.GetAsync("", "Vendor/GetVendorsNameList");
                if (res.code == 200)
                {
                    VendorNameList = JsonConvert.DeserializeObject<List<VendorListDetailsView>>(res.data.ToString());
                }
                return new JsonResult(VendorNameList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProductType(ProductTypeView AddProduct)
        {
            try
            {
                if (ModelState.IsValid)
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
                else
                {
                    return new JsonResult(new { Message = "Please Select Product", Icone = "warning" });
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

        public async Task<JsonResult> GetProductById(Guid ProductId)
        {
            try
            {
                List<ProductDetailsView> products = new List<ProductDetailsView>();
                ApiResponseModel response = await APIServices.GetAsync("", "ProductMaster/GetProductById?ProductId=" + ProductId);
                if (response.code == 200)
                {
                    products = JsonConvert.DeserializeObject<List<ProductDetailsView>>(response.data.ToString());
                }
                return new JsonResult(products);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> DisplayProductDetils(Guid ProductId)
        {
            try
            {
                ProductDetailsView products = new ProductDetailsView();
                ApiResponseModel response = await APIServices.PostAsync("", "ProductMaster/DisplayProductDetailsById?ProductId=" + ProductId);
                if (response.code == 200)
                {
                    products = JsonConvert.DeserializeObject<ProductDetailsView>(response.data.ToString());
                }
                return PartialView("~/Views/PurchaseRequest/_DisplayProductDetailPartial.cshtml", products);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult ProductList()
        {
            return View();
        }
        public async Task<IActionResult> GetAllProductList(int? page, string? searchText)
        {
            try
            {
                List<ProductDetailsView> productlist = new List<ProductDetailsView>();
                ApiResponseModel response = await APIServices.PostAsync(searchText, "ProductMaster/GetAllProductList");
                if (response.code == 200)
                {
                    productlist = JsonConvert.DeserializeObject<List<ProductDetailsView>>(response.data.ToString());
                }
                int pageSize = 5;
                var pageNumber = page ?? 1;

                var pagedList = productlist.ToPagedList(pageNumber, pageSize);
                return PartialView("~/Views/ProductMaster/_GetAllProductList.cshtml", pagedList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProductDetailsByVendorId(Guid VendorId)
        {
            try
            {
                List<ProductDetailsView> ProductList = new List<ProductDetailsView>();
                ApiResponseModel postuser = await APIServices.PostAsync("", "ProductMaster/GetProductDetailsByVendorId?VendorId=" + VendorId);
                if (postuser.data != null)
                {
                    ProductList = JsonConvert.DeserializeObject<List<ProductDetailsView>>(postuser.data.ToString());
                }
                else
                {
                    ProductList = new List<ProductDetailsView>();
                    ViewBag.Error = "note found";
                }
                return PartialView("~/Views/ProductMaster/_ProductDetailsbtVendorId.cshtml", ProductList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> DisplayProductDetailsByVendorId(string GetVendorId)
        {
            try
            {

                List<ProductDetailsView> ProductList = new List<ProductDetailsView>();
                ApiResponseModel response = await APIServices.PostAsync("", "ProductMaster/GetProductDetailsByVendorId?VendorId=" + GetVendorId);
                if (response.code == 200)
                {
                    ProductList = JsonConvert.DeserializeObject<List<ProductDetailsView>>(response.data.ToString());
                }
                return PartialView("~/Views/ProductMaster/_ProductDetailsbtVendorId.cshtml", ProductList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProductDetailsById(Guid ProductId)
        {
            try
            {
                ProductDetailsView products = new ProductDetailsView();
                ApiResponseModel response = await APIServices.GetAsync("", "ProductMaster/GetProductDetailsById?ProductId=" + ProductId);
                if (response.code == 200)
                {
                    products = JsonConvert.DeserializeObject<ProductDetailsView>(response.data.ToString());
                }
                return View("ProductDetails", products);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> SearchProductName(string ProductName)
        {
            try
            {
                List<ProductDetailsView> ProductList = new List<ProductDetailsView>();
                ApiResponseModel postuser = await APIServices.PostAsync("", "ProductMaster/SearchProductName?ProductName=" + ProductName);
                if (postuser.data != null)
                {
                    ProductList = JsonConvert.DeserializeObject<List<ProductDetailsView>>(postuser.data.ToString());
                }
                else
                {
                    ProductList = new List<ProductDetailsView>();
                    ViewBag.Error = "note found";
                }
                return PartialView("~/Views/ProductMaster/_ProductDetailsbtVendorId.cshtml", ProductList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult ProductDetails()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DisplayProductDetailsById()
        {
            try
            {
                string Productstatus = HttpContext.Request.Form["PRODUCTID"];
                var GetProduct = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductDetailsView>(Productstatus.ToString());
                ProductDetailsView products = new ProductDetailsView();
                ApiResponseModel response = await APIServices.PostAsync("", "ProductMaster/DisplayProductDetailsById?ProductId=" + GetProduct.Id);
                if (response.code == 200)
                {
                    products = JsonConvert.DeserializeObject<ProductDetailsView>(response.data.ToString());
                    products.RowNumber = GetProduct.RowNumber;
                }
                return PartialView("~/Views/PurchaseOrderMaster/_DisplayProductDetailsById.cshtml", products);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public async Task<IActionResult> EditProductDetails(Guid ProductId)
        {
            try
            {
                ProductDetailsView products = new ProductDetailsView();
                ApiResponseModel response = await APIServices.GetAsync("", "ProductMaster/GetProductDetailsById?ProductId=" + ProductId);
                if (response.code == 200)
                {
                    products = JsonConvert.DeserializeObject<ProductDetailsView>(response.data.ToString());
                }
                return new JsonResult(products);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProductDetails(ProductDetailsView UpdateProduct)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync(UpdateProduct, "ProductMaster/UpdateProductDetails");
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
        [HttpPost]
        public async Task<IActionResult> GetProductDetailsByProductId(int ProductId)
        {
            try
            {
                List<ProductDetailsView> ProductList = new List<ProductDetailsView>();
                ApiResponseModel postuser = await APIServices.PostAsync("", "ProductMaster/GetProductDetailsByProductId?ProductId=" + ProductId);
                if (postuser.data != null)
                {
                    ProductList = JsonConvert.DeserializeObject<List<ProductDetailsView>>(postuser.data.ToString());
                }
                else
                {
                    ProductList = new List<ProductDetailsView>();
                    ViewBag.Error = "not found";
                }
                return PartialView("~/Views/ProductMaster/_ProductDetailsbtVendorId.cshtml", ProductList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> SerchProductByVendor(int ProductId, Guid VendorId)
        {
            try
            {
                List<ProductDetailsView> ProductList = new List<ProductDetailsView>();
                ApiResponseModel postuser = await APIServices.PostAsync("", "ProductMaster/SerchProductByVendor?ProductId=" + ProductId + "&VendorId=" + VendorId);
                if (postuser.data != null)
                {
                    ProductList = JsonConvert.DeserializeObject<List<ProductDetailsView>>(postuser.data.ToString());
                }
                else
                {
                    ProductList = new List<ProductDetailsView>();
                    ViewBag.Error = "not found";
                }
                return new JsonResult(ProductList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> DisplayProductDetailsListById()
        {
            try
            {
                string ProductId = HttpContext.Request.Form["ProductId"];
                var GetProduct = JsonConvert.DeserializeObject<ProductDetailsView>(ProductId.ToString());
                List<ProductDetailsView> Product = new List<ProductDetailsView>();
                ApiResponseModel response = await APIServices.GetAsync("", "ProductMaster/GetProductById?ProductId=" + GetProduct.Id);
                if (response.code == 200)
                {
                    Product = JsonConvert.DeserializeObject<List<ProductDetailsView>>(response.data.ToString());
                }
                return PartialView("~/Views/PurchaseOrderMaster/_DisplayProductDetailsById.cshtml", Product);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> DisplayProductDetailById()
        {
            try
            {
                string ProductId = HttpContext.Request.Form["ProductId"];
                var GetProduct = JsonConvert.DeserializeObject<ProductDetailsView>(ProductId.ToString());
                ProductDetailsView Product = new ProductDetailsView();
                ApiResponseModel response = await APIServices.GetAsync("", "ProductMaster/GetProductDetailsById?ProductId=" + GetProduct.Id);
                if (response.code == 200)
                {
                    Product = JsonConvert.DeserializeObject<ProductDetailsView>(response.data.ToString());
                    Product.RowNumber = Product.RowNumber;
                }
                return PartialView("~/Views/PurchaseOrderMaster/_DisplayProductDetailsById.cshtml", Product);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
