using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Interface.ProductMaster
{
    public interface IProductMaster
    {
        Task<UserResponceModel> AddProductType(ProductTypeView AddProduct);
        Task<UserResponceModel> AddProductDetails(ProductDetailsView AddProductDetails);
        Task<IEnumerable<ProductTypeView>> GetProduct();
        Task<IEnumerable<ProductTypeView>> GetProductById(Guid ProductId);
        Task<List<ProductDetailsView>> GetProductDetailsByVendorId(Guid vendorId);
        //Task<UserResponceModel> AddVendorType(ProductDetailsView AddProductDetails);
        Task<ProductDetailsView> GetProductDetailsById(Guid ProductId);
        Task<UserResponceModel> UpdateProductDetails(ProductDetailsView UpdateProduct);
        Task<List<ProductDetailsView>> GetProductDetailsByProductId(int ProductId);
        Task<IEnumerable<ProductDetailsView>> GetSearchProductByProductName(String ProductName);
    }
}
