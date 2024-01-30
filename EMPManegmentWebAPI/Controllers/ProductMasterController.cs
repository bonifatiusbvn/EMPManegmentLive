using DocumentFormat.OpenXml.Spreadsheet;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using EMPManegment.Inretface.Interface.ProductMaster;
using EMPManegment.Inretface.Interface.ProjectDetails;
using EMPManegment.Inretface.Services.ProductMaster;
using EMPManegment.Inretface.Services.TaskServices;
using EMPManegment.Services.ProductMaster;
using EMPManegment.Services.VendorDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductMasterController : ControllerBase
    {
        private readonly IProductMasterServices productMaster;
        public ProductMasterController(IProductMasterServices ProductMaster)
        {
            productMaster = ProductMaster;
        }

        [HttpPost]
        [Route("AddProductDetails")]
        public async Task<IActionResult> AddProductDetails(ProductDetailsView AddProduct)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var result = productMaster.AddProductDetails(AddProduct);
                if (result.Result.Code == 200)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = result.Result.Message;
                }
                else
                {
                    response.Message = result.Result.Message;
                    response.Code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("AddProductType")]
        public async Task<IActionResult> AddProductType(ProductTypeView AddProduct)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var Product = productMaster.AddProductType(AddProduct);
                if (Product.Result.Code == 200)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = Product.Result.Message;
                    response.Icone = Product.Result.Icone;
                }
                else
                {
                    response.Message = Product.Result.Message;
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Icone = Product.Result.Icone;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StatusCode(response.Code, response);
        }
        [HttpGet]
        [Route("GetProduct")]
        public async Task<IActionResult> GetProduct()
        {
            IEnumerable<ProductTypeView> getProduct = await productMaster.GetProduct();
            return Ok(new { code = 200, data = getProduct.ToList() });
        }
        [HttpGet]
        [Route("GetProductById")]
        public async Task<IActionResult> GetProductById(Guid ProductId)
        {
            var getProduct = await productMaster.GetProductById(ProductId);
            return Ok(new { code = 200, data = getProduct.ToList() });
        }

        [HttpPost]
        [Route("GetProductDetailsByVendorId")]
        public async Task<IActionResult> GetProductDetailsByVendorId(Guid VendorId)
        {
            List<ProductDetailsView> VendorProductList = await productMaster.GetProductDetailsByVendorId(VendorId);
            return Ok(new { code = 200, data = VendorProductList.ToList() });
        }

        [HttpGet]
        [Route("GetProductDetailsById")]
        public async Task<IActionResult> GetProductDetailsById(Guid ProductId)
        {
            var getProductDetails = await productMaster.GetProductDetailsById(ProductId);
            return Ok(new { code = 200, data = getProductDetails });
        }

        [HttpPost]
        [Route("UpdateProductDetails")]
        public async Task<IActionResult> UpdateProductDetails(ProductDetailsView Product)
        {
            UserResponceModel updateresponsemodel = new UserResponceModel();
            try
            {
                var UpdateProduct = productMaster.UpdateProductDetails(Product);
                if (UpdateProduct.Result.Code == 200)
                {
                    updateresponsemodel.Code = (int)HttpStatusCode.OK;
                    updateresponsemodel.Message = UpdateProduct.Result.Message;
                    updateresponsemodel.Icone = UpdateProduct.Result.Icone;
                }
                else
                {
                    updateresponsemodel.Message = UpdateProduct.Result.Message;
                    updateresponsemodel.Code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StatusCode(updateresponsemodel.Code, updateresponsemodel);
        }
        [HttpPost]
        [Route("GetProductDetailsByProductId")]
        public async Task<IActionResult> GetProductDetailsByProductId(int ProductId)
        {
            List<ProductDetailsView> ProductList = await productMaster.GetProductDetailsByProductId(ProductId);
            return Ok(new { code = 200, data = ProductList.ToList() });
        }


        [HttpPost]
        [Route("SearchProductName")]
        public async Task<IActionResult> SearchProductName(String ProductName)
        {
            {
                var Product = await productMaster.SearchProductName(ProductName);
                return Ok(new { code = 200, data = Product.ToList() });
            }
        }
        [HttpPost]
        [Route("SerchProductByVendor")]
        public async Task<IActionResult> SerchProductByVendor(int ProductId, Guid VendorId)
        {
            List<ProductDetailsView> ProductList = await productMaster.SerchProductByVendor(ProductId,VendorId);
            return Ok(new { code = 200, data = ProductList.ToList() });
        }
        [HttpPost]
        [Route("DisplayProductDetailsById")]
        public async Task<IActionResult> DisplayProductDetailsById(Guid ProductId)
        {
            List<ProductDetailsView> ProductList = await productMaster.DisplayProductDetailsById(ProductId);
            return Ok(new { code = 200, data = ProductList.ToList() });
        }
    }
}
