﻿using Azure;
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
using System.Collections;
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
                        PerUnitPrice = AddProduct.PerUnitPrice,
                        IsWithGst=AddProduct.IsWithGst,
                        GstPercentage = AddProduct.GstPercentage,
                        GstAmount = AddProduct.GstAmount,
                        Hsn = AddProduct.Hsn,
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
                                      Hsn = a.Hsn,
                                      PerUnitPrice = a.PerUnitPrice,
                                      IsWithGst = a.IsWithGst,
                                      GstPercentage = a.GstPercentage,
                                      GstAmount = a.GstAmount,
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
                            Hsn = item.Hsn,
                            PerUnitPrice = item.PerUnitPrice,
                            IsWithGst= item.IsWithGst,
                            GstPercentage = item.GstPercentage,
                            GstAmount = item.GstAmount,
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
                               PerUnitPrice = a.PerUnitPrice,
                               IsWithGst = a.IsWithGst,
                               GstPercentage= a.GstPercentage,
                               GstAmount= a.GstAmount,
                               Hsn = a.Hsn,
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
                    getProduct.IsWithGst=UpdateProduct.IsWithGst;
                    getProduct.GstPercentage = UpdateProduct.GstPercentage;
                    getProduct.GstAmount = UpdateProduct.GstAmount;
                    getProduct.ProductType = UpdateProduct.ProductType;
                    getProduct.ProductImage = UpdateProduct.ProductImage;
                    getProduct.Hsn = UpdateProduct.Hsn;
                    getProduct.Id = UpdateProduct.Id;

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
                            PerUnitPrice = item.PerUnitPrice,
                            IsWithGst = item.IsWithGst,
                            GstPercentage = item.GstPercentage,
                            GstAmount = item.GstAmount,
                            Hsn = item.Hsn,
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
                        PerUnitPrice = data.PerUnitPrice,
                        IsWithGst = data.IsWithGst,
                        GstPercentage = data.GstPercentage,
                        GstAmount = data.GstAmount,
                        Hsn = data.Hsn,
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

        public async Task<IEnumerable<ProductDetailsView>> GetAllProductList(string? sortBy)
        {
            List<ProductDetailsView> Product = new List<ProductDetailsView>();
            try
            {
                Product = (from a in Context.TblProductDetailsMasters.Where(a => a.IsDeleted == false)
                           join b in Context.TblProductTypeMasters
                           on a.ProductType equals b.Id
                           orderby a.ProductName ascending 
                           select new ProductDetailsView
                           {
                               Id = a.Id,
                               ProductType = a.ProductType,
                               ProductTypeName = b.Type,
                               ProductName = a.ProductName,
                               ProductDescription = a.ProductDescription,
                               ProductShortDescription = a.ProductShortDescription,
                               ProductImage = a.ProductImage,
                               PerUnitPrice = a.PerUnitPrice,
                               IsWithGst = a.IsWithGst,
                               GstPercentage = a.GstPercentage,
                               GstAmount = a.GstAmount,
                               Hsn = a.Hsn,
                               CreatedBy = a.CreatedBy,
                           }).ToList();
                if (string.IsNullOrEmpty(sortBy))
                {
                    Product = Product.OrderByDescending(a => a.CreatedOn).ToList();
                }
                else
                {
                    string sortOrder = sortBy.StartsWith("Ascending", StringComparison.OrdinalIgnoreCase) ? "ascending" :
                                       sortBy.StartsWith("Descending", StringComparison.OrdinalIgnoreCase) ? "descending" :
                                       string.Empty;

                    if (!string.IsNullOrEmpty(sortOrder))
                    {
                        string field = sortBy.Substring(sortOrder.Length).Trim();
                        switch (field.ToLower())
                        {
                            case "productname":
                                if (sortOrder == "ascending")
                                    Product = Product.OrderBy(a => a.ProductName).ToList();
                                else if (sortOrder == "descending")
                                    Product = Product.OrderByDescending(a => a.ProductName).ToList();
                                break;
                            case "perunitprice":
                                if (sortOrder == "ascending")
                                    Product = Product.OrderBy(a => a.PerUnitPrice).ToList();
                                else if (sortOrder == "descending")
                                    Product = Product.OrderByDescending(a => a.PerUnitPrice).ToList();
                                break;
                            default:
                                break;
                        }
                    }
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



