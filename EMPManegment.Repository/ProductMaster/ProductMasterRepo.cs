﻿using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.Inretface.Interface.ProductMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
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
                bool isProductAlredyExists = Context.TblProductTypeMasters.Any(x => x.ProductType == AddProduct.ProductType);
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
                        ProductType = AddProduct.ProductType,
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
                Context.TblProductDetailsMasters.Add(productdetails);
                Context.SaveChanges();
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
                    ProductType = a.ProductType
                });
                return Product;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
