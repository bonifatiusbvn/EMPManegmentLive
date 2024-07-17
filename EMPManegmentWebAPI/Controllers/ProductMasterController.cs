
using Azure;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using EMPManegment.Inretface.Interface.OrderDetails;
using EMPManegment.Inretface.Interface.ProductMaster;
using EMPManegment.Inretface.Interface.ProjectDetails;
using EMPManegment.Inretface.Services.ProductMaster;
using EMPManegment.Inretface.Services.TaskServices;
using EMPManegment.Services.ProductMaster;
using EMPManegment.Services.VendorDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static TheArtOfDev.HtmlRenderer.Adapters.RGraphicsPath;
#nullable disable
namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
                if (result.Result.Code != (int)HttpStatusCode.NotFound && result.Result.Code != (int)HttpStatusCode.InternalServerError)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = result.Result.Message;
                }
                else
                {
                    response.Message = result.Result.Message;
                    response.Code = result.Result.Code;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "An error occurred while processing the request.";
            }
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("AddProductType")]
        [AllowAnonymous]
        public async Task<IActionResult> AddProductType(ProductTypeView AddProduct)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var Product = productMaster.AddProductType(AddProduct);
                if (Product.Result.Code != (int)HttpStatusCode.NotFound && Product.Result.Code != (int)HttpStatusCode.InternalServerError)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = Product.Result.Message;
                }
                else
                {
                    response.Message = Product.Result.Message;
                    response.Code = Product.Result.Code;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "An error occurred while processing the request.";
            }
            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Route("GetProduct")]
        public async Task<IActionResult> GetProduct()
        {
            IEnumerable<ProductTypeView> getProduct = await productMaster.GetProduct();
            return Ok(new { code = (int)HttpStatusCode.OK, data = getProduct.ToList() });
        }

        [HttpGet]
        [Route("GetProductById")]
        public async Task<IActionResult> GetProductById(Guid ProductId)
        {
            List<ProductDetailsView> getProduct = await productMaster.GetProductById(ProductId);
            return Ok(new { code = (int)HttpStatusCode.OK, data = getProduct.ToList() });
        }

        [HttpPost]
        [Route("GetProductDetailsByVendorId")]
        public async Task<IActionResult> GetProductDetailsByVendorId(Guid VendorId)
        {
            List<ProductDetailsView> VendorProductList = await productMaster.GetProductDetailsByVendorId(VendorId);
            return Ok(new { code = (int)HttpStatusCode.OK, data = VendorProductList.ToList() });
        }

        [HttpGet]
        [Route("GetProductDetailsById")]
        public async Task<IActionResult> GetProductDetailsById(Guid ProductId)
        {
            var getProductDetails = await productMaster.GetProductDetailsById(ProductId);
            return Ok(new { code = (int)HttpStatusCode.OK, data = getProductDetails });
        }

        [HttpPost]
        [Route("UpdateProductDetails")]
        public async Task<IActionResult> UpdateProductDetails(ProductDetailsView Product)
        {
            UserResponceModel updateresponsemodel = new UserResponceModel();
            try
            {
                var UpdateProduct = productMaster.UpdateProductDetails(Product);
                if (UpdateProduct.Result.Code != (int)HttpStatusCode.InternalServerError)
                {
                    updateresponsemodel.Code = (int)HttpStatusCode.OK;
                    updateresponsemodel.Message = UpdateProduct.Result.Message;
                    updateresponsemodel.Icone = UpdateProduct.Result.Icone;
                }
                else
                {
                    updateresponsemodel.Message = UpdateProduct.Result.Message;
                    updateresponsemodel.Code = UpdateProduct.Result.Code;
                }
            }
            catch (Exception ex)
            {
                updateresponsemodel.Code = (int)HttpStatusCode.InternalServerError;
                updateresponsemodel.Message = "An error occurred while processing the request.";
            }
            return StatusCode(updateresponsemodel.Code, updateresponsemodel);
        }

        [HttpPost]
        [Route("DeleteProductDetails")]
        public async Task<IActionResult> DeleteProductDetails(Guid ProductId)
        {
            UserResponceModel responseModel = new UserResponceModel();
            var order = await productMaster.DeleteProductDetails(ProductId);
            try
            {
                if (order.Code != (int)HttpStatusCode.NotFound && order.Code != (int)HttpStatusCode.InternalServerError)
                {
                    responseModel.Code = (int)HttpStatusCode.OK;
                    responseModel.Message = order.Message;
                }
                else
                {
                    responseModel.Message = order.Message;
                    responseModel.Code = order.Code;
                }
            }
            catch (Exception ex)
            {
                responseModel.Code = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = "An error occurred while processing the request.";
            }
            return StatusCode(responseModel.Code, responseModel);
        }

        [HttpPost]
        [Route("GetProductDetailsByProductId")]
        public async Task<IActionResult> GetProductDetailsByProductId(int ProductId)
        {
            List<ProductDetailsView> ProductList = await productMaster.GetProductDetailsByProductId(ProductId);
            return Ok(new { code = (int)HttpStatusCode.OK, data = ProductList.ToList() });
        }

        [HttpPost]
        [Route("SerchProductByVendor")]
        public async Task<IActionResult> SerchProductByVendor(int ProductId, Guid VendorId)
        {
            List<ProductDetailsView> ProductList = await productMaster.SerchProductByVendor(ProductId, VendorId);
            return Ok(new { code = (int)HttpStatusCode.OK, data = ProductList.ToList() });
        }

        [HttpPost]
        [Route("DisplayProductDetailsById")]
        public async Task<IActionResult> DisplayProductDetailsById(Guid ProductId)
        {
            ProductDetailsView ProductList = await productMaster.DisplayProductDetailsById(ProductId);
            return Ok(new { code = (int)HttpStatusCode.OK, data = ProductList });
        }

        [HttpPost]
        [Route("GetAllProductList")]
        public async Task<IActionResult> GetAllProductList(string? sortBy)
        {
            IEnumerable<ProductDetailsView> getProductList = await productMaster.GetAllProductList(sortBy);
            return Ok(new { code = (int)HttpStatusCode.OK, data = getProductList.ToList() });
        }
    }
}
