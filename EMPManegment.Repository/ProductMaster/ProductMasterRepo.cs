using Azure;
using EMPManagment.API;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using EMPManegment.Inretface.Interface.ProductMaster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Repository.ProductMaster
{
    public class ProductMasterRepo : IProductMaster
    {
        public ProductMasterRepo(BonifatiusEmployeesContext Context)
        {
            this.Context = Context;
        }
        public BonifatiusEmployeesContext Context { get; }

        public async Task<UserResponceModel> AddProductType(ProductTypeView AddProduct)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                bool isProductAlredyExists = Context.TblProductTypeMasters.Any(x => x.Type == AddProduct.ProductName);
                if (isProductAlredyExists == true)
                {
                    response.Message = "This product is already exists";
                    response.Data = AddProduct;
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Icone = "warning";
                }
                else
                {
                    var Product = new TblProductTypeMaster()
                    {
                        Type = AddProduct.ProductName,
                    };
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = "Product successfully inserted";
                    response.Icone = "success";
                    Context.TblProductTypeMasters.Add(Product);
                    Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<UserResponceModel> AddProductDetails(ProductDetailsView AddProduct)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                bool isProductAlredyExists = Context.TblProductDetailsMasters.Any(x => x.ProductName == AddProduct.ProductName);
                if (isProductAlredyExists == true)
                {
                    response.Message = "This product is already exists";
                    response.Data = AddProduct;
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Icone = "warning";
                }

                else
                {
                    var productdetails = new TblProductDetailsMaster()
                    {
                        Id = Guid.NewGuid(),
                        ProductType = AddProduct.ProductType,
                        ProductName = AddProduct.ProductName,
                        ProductDescription = AddProduct.ProductDescription,
                        ProductShortDescription = AddProduct.ProductShortDescription,
                        ProductImage = AddProduct.ProductImage,
                        ProductStocks = AddProduct.ProductStocks,
                        PerUnitPrice = AddProduct.PerUnitPrice,
                        Hsn = AddProduct.Hsn,
                        Gst = AddProduct.Gst,
                        PerUnitWithGstprice = AddProduct.PerUnitWithGstprice,
                        CreatedBy = AddProduct.CreatedBy,
                        CreatedOn = DateTime.Today,
                        UpdatedBy = AddProduct.UpdatedBy,
                        UpdatedOn = AddProduct.UpdatedOn,
                        
                    };
                    response.Code = 200;
                    response.Message = "Product add successfully!";
                    response.Icone = "success";
                    Context.TblProductDetailsMasters.Add(productdetails);
                    Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<IEnumerable<ProductTypeView>> GetProduct()
        {
            try
            {
                IEnumerable<ProductTypeView> Product = Context.TblProductTypeMasters.ToList().Select(a => new ProductTypeView
                {
                    Id = a.Id,
                    ProductName = a.Type,
                });
                return Product;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ProductDetailsView>> GetProductDetailsByVendorId(Guid VendorId)
        {
            try
            {
                var vendorDetails = new List<ProductDetailsView>();
                var data = await (from a in Context.TblProductDetailsMasters
                                  join b in Context.TblProductTypeMasters
                                  on a.ProductType equals b.Id

                                  select new
                                  {
                                      a.Id,
                                      a.ProductImage,
                                      a.ProductStocks,
                                      a.ProductType,
                                      a.PerUnitPrice,
                                      a.ProductName,
                                      b.Type,
                                      a.ProductShortDescription,
                                  }).ToListAsync();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        vendorDetails.Add(new ProductDetailsView()
                        {
                            Id = item.Id,
                            ProductImage = item.ProductImage,
                            ProductName = item.ProductName,
                            ProductStocks = item.ProductStocks,
                            PerUnitPrice = item.PerUnitPrice,
                            ProductShortDescription = item.ProductShortDescription,
                            ProductTypeName = item.Type,
                        });
                    }
                }
                return vendorDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ProductDetailsView>> GetProductById(Guid ProductId)
        {
            try
            {
                var productDetails = new List<ProductDetailsView>();
                var data = await (from a in Context.TblProductDetailsMasters.Where(x => x.Id == ProductId)
                                  join b in Context.TblProductTypeMasters on a.ProductType equals b.Id
                                  select new ProductDetailsView
                                  {
                                      Id = a.Id,
                                      ProductType = a.ProductType,
                                      ProductDescription = a.ProductDescription,
                                      ProductName = a.ProductName,
                                      ProductImage = a.ProductImage,
                                      ProductId = a.Id,
                                      Gst = a.Gst,
                                      Hsn = a.Hsn,
                                      PerUnitWithGstprice = a.PerUnitWithGstprice,
                                      PerUnitPrice = a.PerUnitPrice,
                                      ProductTypeName = b.Type,
                                  }).ToListAsync();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        productDetails.Add(new ProductDetailsView()
                        {
                            Id = item.Id,
                            ProductType = item.ProductType,
                            ProductDescription = item.ProductDescription,
                            ProductName = item.ProductName,
                            ProductId = item.ProductId,
                            ProductImage = item.ProductImage,
                            Gst = item.Gst,
                            Hsn = item.Hsn,
                            PerUnitWithGstprice = item.PerUnitWithGstprice,
                            PerUnitPrice = item.PerUnitPrice,
                            ProductTypeName = item.ProductTypeName,
                        });
                    }
                }
                return productDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProductDetailsView> GetProductDetailsById(Guid ProductId)
        {
            ProductDetailsView Product = new ProductDetailsView();
            try
            {
                Product = (from a in Context.TblProductDetailsMasters.Where(x => x.Id == ProductId)
                           join b in Context.TblProductTypeMasters
                           on a.ProductType equals b.Id

                           select new ProductDetailsView
                           {
                               Id = a.Id,
                               ProductType = a.ProductType,
                               ProductTypeName = b.Type,
                               ProductName = a.ProductName,
                               ProductDescription = a.ProductDescription,
                               ProductShortDescription = a.ProductShortDescription,
                               ProductImage = a.ProductImage,
                               ProductStocks = a.ProductStocks,
                               PerUnitPrice = a.PerUnitPrice,
                               Hsn = a.Hsn,
                               Gst = a.Gst,
                               PerUnitWithGstprice = a.PerUnitWithGstprice,
                               CreatedBy = a.CreatedBy,
                           }).First();
                return Product;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserResponceModel> UpdateProductDetails(ProductDetailsView UpdateProduct)
        {
            UserResponceModel model = new UserResponceModel();
            var getProduct = Context.TblProductDetailsMasters.Where(e => e.Id == UpdateProduct.Id).FirstOrDefault();
            try
            {
                if (getProduct != null)
                {
                    getProduct.ProductName = UpdateProduct.ProductName;
                    getProduct.ProductDescription = UpdateProduct.ProductDescription;
                    getProduct.ProductShortDescription = UpdateProduct.ProductShortDescription;
                    getProduct.PerUnitPrice = UpdateProduct.PerUnitPrice;
                    getProduct.ProductType = UpdateProduct.ProductType;
                    getProduct.ProductImage = UpdateProduct.ProductImage;
                    getProduct.Gst = UpdateProduct.Gst;
                    getProduct.Hsn = UpdateProduct.Hsn;
                    getProduct.Id = UpdateProduct.Id;
                    getProduct.PerUnitWithGstprice = UpdateProduct.PerUnitWithGstprice;
                    getProduct.ProductStocks = UpdateProduct.ProductStocks;

                }
                Context.TblProductDetailsMasters.Update(getProduct);
                Context.SaveChanges();
                model.Code = 200;
                model.Message = "Product details updated successfully!";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public async Task<List<ProductDetailsView>> GetProductDetailsByProductId(int ProductId)
        {
            try
            {
                var productDetails = new List<ProductDetailsView>();
                var data = await Context.TblProductDetailsMasters.Where(x => x.ProductType == ProductId).ToListAsync();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        productDetails.Add(new ProductDetailsView()
                        {
                            Id = item.Id,

                            ProductImage = item.ProductImage,
                            ProductName = item.ProductName,
                            ProductStocks = item.ProductStocks,
                            PerUnitPrice = item.PerUnitPrice,
                        });
                    }
                }
                return productDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ProductDetailsView>> SearchProductName(String ProductName)
        {
            try
            {
                var productDetails = new List<ProductDetailsView>();
                var data = await Context.TblProductDetailsMasters.Where(x => x.ProductName == ProductName).ToListAsync();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        productDetails.Add(new ProductDetailsView()
                        {
                            Id = item.Id,

                            ProductImage = item.ProductImage,
                            ProductName = item.ProductName,
                            ProductStocks = item.ProductStocks,
                            PerUnitPrice = item.PerUnitPrice,
                        });
                    }
                }
                return productDetails;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ProductDetailsView>> SerchProductByVendor(int ProductId, Guid VendorId)
        {
            try
            {
                var productDetails = new List<ProductDetailsView>();
                var data = await Context.TblProductDetailsMasters.Where(x => x.ProductType == ProductId).ToListAsync();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        productDetails.Add(new ProductDetailsView()
                        {
                            Id = item.Id,

                            ProductImage = item.ProductImage,
                            ProductName = item.ProductName,
                            ProductStocks = item.ProductStocks,
                            PerUnitPrice = item.PerUnitPrice,
                            PerUnitWithGstprice = item.PerUnitWithGstprice,
                            Hsn = item.Hsn,
                            Gst = item.Gst,
                            ProductDescription = item.ProductDescription,
                            ProductShortDescription = item.ProductShortDescription,
                        });
                    }
                }
                return productDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProductDetailsView> DisplayProductDetailsById(Guid ProductId)
        {
            try
            {
                var ProductDetails = new ProductDetailsView();
                var data = Context.TblProductDetailsMasters.Where(x => x.Id == ProductId).SingleOrDefault();
                if (data != null)
                {

                    ProductDetails = new ProductDetailsView()
                    {
                        Id = data.Id,

                        ProductType = data.ProductType,
                        ProductId = data.Id,
                        ProductName = data.ProductName,
                        ProductDescription = data.ProductDescription,
                        ProductShortDescription = data.ProductShortDescription,
                        ProductImage = data.ProductImage,
                        ProductStocks = data.ProductStocks,
                        PerUnitPrice = data.PerUnitPrice,
                        Hsn = data.Hsn,
                        Gst = data.Gst,
                        PerUnitWithGstprice = data.PerUnitWithGstprice,
                        CreatedBy = data.CreatedBy,
                    };
                }

                return ProductDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ProductDetailsView>> GetAllProductList(string? searchText)
        {
            try
            {
                IEnumerable<ProductDetailsView> Product = Context.TblProductDetailsMasters.Where(a=>a.IsDeleted==false).ToList().Select(a => new ProductDetailsView
                {
                    Id = a.Id,
                    ProductDescription = a.ProductDescription,
                    ProductShortDescription = a.ProductShortDescription,
                    ProductImage = a.ProductImage,
                    ProductName = a.ProductName,
                    ProductStocks = a.ProductStocks,
                    PerUnitPrice = a.PerUnitPrice,
                    Hsn = a.Hsn,
                    PerUnitWithGstprice = a.PerUnitWithGstprice,
                    Gst = a.Gst,
                });

                if (!string.IsNullOrEmpty(searchText))
                {
                    searchText = searchText.ToLower();
                    Product = Product.Where(u =>
                        u.ProductName.ToLower().Contains(searchText)
                    );
                }

                return Product;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserResponceModel> DeleteProductDetails(Guid ProductId)
        {
            UserResponceModel response = new UserResponceModel();
            var Product = Context.TblProductDetailsMasters.Where(a => a.Id == ProductId).FirstOrDefault();

            if (Product != null)
            {

                Product.IsDeleted = true;
                Context.TblProductDetailsMasters.Update(Product);
                Context.SaveChanges();
                response.Code = 200;
                response.Message = "Product is successfully deleted.";
            }
            else
            {
                response.Code = 400;
                response.Message = "Error in deleting product.";
            }
            return response;
        }
    }
}



