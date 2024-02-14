using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.Inretface.Interface.ProductMaster;
using EMPManegment.Inretface.Services.ProductMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.ProductMaster
{
    public class ProductMasterServices:IProductMasterServices
    {
        private readonly IProductMaster productMaster;
        public ProductMasterServices(IProductMaster ProductMaster)
        {
            productMaster = ProductMaster;
        }

        public async Task<UserResponceModel> AddProductType(ProductTypeView AddProduct)
        {
            return await productMaster.AddProductType(AddProduct);
        }
        public async Task<UserResponceModel> AddProductDetails(ProductDetailsView AddProduct)
        {
            return await productMaster.AddProductDetails(AddProduct);
        }
        public async Task<IEnumerable<ProductTypeView>> GetProduct()
        {
            return await productMaster.GetProduct();
        }

        public async Task<List<ProductDetailsView>> GetProductById(Guid ProductId)
        {
            return await productMaster.GetProductById(ProductId);
        }
        public async Task<List<ProductDetailsView>> GetProductDetailsByVendorId(Guid VendorId)
        {
            return await productMaster.GetProductDetailsByVendorId(VendorId);
        }

        public async Task<ProductDetailsView> GetProductDetailsById(Guid ProductId)
        {
            return await productMaster.GetProductDetailsById(ProductId);
        }

        public async Task<UserResponceModel> UpdateProductDetails(ProductDetailsView UpdateProduct)
        {
            return await productMaster.UpdateProductDetails(UpdateProduct);
        }

        public async Task<List<ProductDetailsView>> GetProductDetailsByProductId(int ProductId)
        {
            return await productMaster.GetProductDetailsByProductId(ProductId);
        }

        public async Task<List<ProductDetailsView>> SerchProductByVendor(int ProductId, Guid VendorId)
        {
            return await productMaster.SerchProductByVendor(ProductId, VendorId);
        }

        public async Task<List<ProductDetailsView>> DisplayProductDetailsById(Guid ProductId)
        {
            return await productMaster.DisplayProductDetailsById(ProductId);
        }
        public async Task<IEnumerable<ProductDetailsView>> SearchProductName(String ProductName)
        {
            return await productMaster.SearchProductName(ProductName);
        }
    }
}
