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
using EMPManegment.Web.Helper;
using EMPManegment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;
#nullable disable
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

        [FormPermissionAttribute("Create Product-View")]
        public IActionResult CreateProduct()
        {
            return View();
        }

        [FormPermissionAttribute("Create Product-Add")]
        [HttpPost]
        public async Task<IActionResult> AddProductDetails(ProductRequestModel AddProduct)
        {
            try
            {
                var ProductDetails = new ProductDetailsView
                {
                    CreatedBy = _userSession.UserId,
                    ProductType = AddProduct.ProductType,
                    ProductName = AddProduct.ProductName,
                    ProductDescription = AddProduct.ProductDescription,
                    ProductShortDescription = AddProduct.ProductShortDescription,
                    PerUnitPrice = AddProduct.PerUnitPrice,
                    IsWithGst = AddProduct.IsWithGst,
                    GstPercentage = AddProduct.GstPercentage,
                    GstAmount = AddProduct.GstAmount,
                    Hsn = AddProduct.Hsn,
                };
                if (AddProduct.ProductImage != null)
                {
                    var path = Environment.WebRootPath;
                    var filepath = "Content/Product/" + Guid.NewGuid() + "_" + AddProduct.ProductImage.FileName;
                    var fullpath = Path.Combine(path, filepath);
                    UploadFile(AddProduct.ProductImage, fullpath);
                    ProductDetails.ProductImage = filepath;
                }
                else
                {
                    ProductDetails.ProductImage = null;
                }
                ApiResponseModel postuser = await APIServices.PostAsync(ProductDetails, "ProductMaster/AddProductDetails");
                UserResponceModel responseModel = new UserResponceModel();
                if (postuser.code == 200)
                {
                    return Ok(new { postuser.message, postuser.code });
                }
                else
                {

                    return Ok(new { postuser.message, postuser.code });
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
                        return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
                    }
                    else
                    {
                        return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
                    }
                }
                else
                {
                    return Ok(new { Message = "Please Select Product" });
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

        [FormPermissionAttribute("Product List-View")]
        public IActionResult ProductList()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductList(int? page, int? ProductId, string? ProductName, string? sortBy)
        {
            try
            {
                List<ProductDetailsView> productlist = new List<ProductDetailsView>();
                ApiResponseModel response = await APIServices.PostAsync("", "ProductMaster/GetAllProductList?sortBy=" + sortBy);
                if (response.code == 200)
                {
                    productlist = JsonConvert.DeserializeObject<List<ProductDetailsView>>(response.data.ToString());
                }

                if (ProductId.HasValue)
                {
                    productlist = productlist.Where(e => e.ProductType == ProductId).ToList();
                }
                else if (!string.IsNullOrEmpty(ProductName))
                {
                    productlist = productlist.Where(e => e.ProductName.Contains(ProductName, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (!productlist.Any())
                {
                    TempData["EmptyProductList"] = "No data for selected!";
                }

                int pageSize = 5;
                var pageNumber = page ?? 1;
                var pagedList = productlist.ToPagedList(pageNumber, pageSize);

                return PartialView("~/Views/ProductMaster/_ProductDetailsbtVendorId.cshtml", pagedList);
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


        [FormPermissionAttribute("Product Details-View")]
        public async Task<IActionResult> ProductDetails(Guid? ProductId)
        {
            ProductDetailsView products = new ProductDetailsView();
            ApiResponseModel response = await APIServices.PostAsync("", "ProductMaster/DisplayProductDetailsById?ProductId=" + ProductId);
            if (response.code == 200)
            {
                products = JsonConvert.DeserializeObject<ProductDetailsView>(response.data.ToString());
            }
            return View(products);
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

        [FormPermissionAttribute("Product List-Edit")]
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
                    return Ok(new { Message = string.Format(postuser.message), Code = postuser.code });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProductDetails(Guid ProductId)
        {
            try
            {
                ApiResponseModel postuser = await APIServices.PostAsync("", "ProductMaster/DeleteProductDetails?ProductId=" + ProductId);
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

        [HttpPost]
        public async Task<IActionResult> DisplayProductDetilsListById(Guid ProductId)
        {
            try
            {
                List<ProductDetailsView> products = new List<ProductDetailsView>();
                ApiResponseModel response = await APIServices.GetAsync("", "ProductMaster/GetProductById?ProductId=" + ProductId);
                if (response.code == 200)
                {
                    products = JsonConvert.DeserializeObject<List<ProductDetailsView>>(response.data.ToString());
                }
                return PartialView("~/Views/PurchaseRequest/_AddProductPRPartial.cshtml", products);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> DisplayProductDetilsById(Guid ProductId)
        {
            try
            {
                ProductDetailsView Product = new ProductDetailsView();
                ApiResponseModel response = await APIServices.GetAsync("", "ProductMaster/GetProductDetailsById?ProductId=" + ProductId);
                if (response.code == 200)
                {
                    Product = JsonConvert.DeserializeObject<ProductDetailsView>(response.data.ToString());
                    Product.RowNumber = Product.RowNumber;
                }
                return PartialView("~/Views/PurchaseRequest/_AddProductPRPartial.cshtml", Product);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
