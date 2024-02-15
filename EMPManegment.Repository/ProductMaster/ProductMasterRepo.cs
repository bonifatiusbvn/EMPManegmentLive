using Azure;
using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using EMPManegment.Inretface.Interface.ProductMaster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                    response.Message = "This Product Is already exists";
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
                    response.Message = "Product Successfully Inserted";
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
                    response.Message = " this product Is already exists";
                    response.Data = AddProduct;
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Icone = "warning";
                }

                else
                {
                    var productdetails = new TblProductDetailsMaster()
                    {
                        Id = Guid.NewGuid(),
                        VendorId = AddProduct.VendorId,
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
                    ProductName = a.Type
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
                var data = await(from a in Context.TblProductDetailsMasters
                    join b in Context.TblProductTypeMasters
                    on a.ProductType equals b.Id
                    where a.VendorId==VendorId
                    select new
                    {
                        a.Id,
                        a.VendorId,
                        a.ProductImage,
                        a.ProductStocks,
                        a.ProductType,
                        a.PerUnitPrice,
                        a.ProductName,
                        b.Type,
                    }).ToListAsync();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        vendorDetails.Add(new ProductDetailsView()
                        {
                            Id = item.Id,
                            VendorId = item.VendorId,
                            ProductImage = item.ProductImage,
                            ProductName = item.ProductName,
                            ProductStocks = item.ProductStocks,
                            PerUnitPrice = item.PerUnitPrice,
                            ProductTypeName=item.Type,
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

        public async Task<List<ProductDetailsView>> GetProductById(Guid VendorId)
        {
            try
            {
                var vendorDetails = new List<ProductDetailsView>();
                var data = await (from a in Context.TblProductDetailsMasters
                                  join b in Context.TblProductTypeMasters
                                  on a.ProductType equals b.Id
                                  where a.VendorId == VendorId
                                  select new
                                  {
                                      a.Id,
                                      a.VendorId,
                                      a.ProductImage,
                                      a.ProductStocks,
                                      a.ProductType,
                                      a.PerUnitPrice,
                                      a.ProductName,
                                      b.Type,
                                  }).ToListAsync();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        vendorDetails.Add(new ProductDetailsView()
                        {
                            Id = item.Id,
                            VendorId = item.VendorId,
                            ProductImage = item.ProductImage,
                            ProductName = item.ProductName,
                            ProductStocks = item.ProductStocks,
                            PerUnitPrice = item.PerUnitPrice,
                            ProductTypeName = item.Type
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

        public async Task<ProductDetailsView> GetProductDetailsById(Guid ProductId)
        {
            ProductDetailsView Product = new ProductDetailsView();
            try
            {
                 Product =(from a in Context.TblProductDetailsMasters.Where(x => x.Id == ProductId)
                                                         join b in Context.TblProductTypeMasters
                                                         on a.ProductType equals b.Id
                                                         join c in Context.TblVendorMasters
                                                         on a.VendorId equals c.Vid
                                                         select new ProductDetailsView
                                                         {
                                                             Id = a.Id,
                                                             VendorName =c.VendorCompany,
                                                             VendorId=a.VendorId,
                                                             ProductType=a.ProductType,
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
                    getProduct.VendorId = UpdateProduct.VendorId;
                }
                Context.TblProductDetailsMasters.Update(getProduct);
                Context.SaveChanges();
                model.Code = 200;
                model.Message = "Product Details Updated Successfully!";
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
                            VendorId = item.VendorId,
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
                            VendorId = item.VendorId,
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
                var data = await Context.TblProductDetailsMasters.Where(x => x.ProductType == ProductId && x.VendorId == VendorId).ToListAsync();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        productDetails.Add(new ProductDetailsView()
                        {
                            Id = item.Id,
                            VendorId = item.VendorId,
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
                        VendorId = data.VendorId,
                        ProductType = data.ProductType,
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
    }

}



