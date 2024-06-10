﻿using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Services.ProductMaster
{
    public interface IProductMasterServices
    {
        Task<UserResponceModel> AddProductType(ProductTypeView AddProduct);
        Task<UserResponceModel> AddProductDetails(ProductDetailsView AddProductDetails);
        Task<IEnumerable<ProductTypeView>> GetProduct();
        Task<List<ProductDetailsView>> GetProductById(Guid ProductId);
        Task<List<ProductDetailsView>> GetProductDetailsByVendorId(Guid vendorId);
        Task<ProductDetailsView> GetProductDetailsById(Guid ProductId);
        Task<UserResponceModel> UpdateProductDetails(ProductDetailsView UpdateProduct);
        Task<List<ProductDetailsView>> GetProductDetailsByProductId(int ProductId);
        Task<List<ProductDetailsView>> SerchProductByVendor(int ProductId, Guid VendorId);
        Task<ProductDetailsView> DisplayProductDetailsById(Guid ProductId);
        Task<IEnumerable<ProductDetailsView>> GetAllProductList(string? sortBy);
        Task<UserResponceModel> DeleteProductDetails(Guid ProductId);
    }
}
