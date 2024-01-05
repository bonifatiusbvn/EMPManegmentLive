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
        Task<IEnumerable<ProductTypeView>> GetProductById(Guid ProductId);
        Task<List<ProductDetailsView>> GetProductDetailsByVendorId(Guid vendorId);
        //Task<UserResponceModel> AddVendorType(ProductDetailsView AddProductDetails);
    }
}
