using EMPManagment.API;
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
                }
                else
                {
                    var Product = new TblProductTypeMaster()
                    {
                        ProductType = AddProduct.ProductType,
                    };
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = "Vendor Data Successfully Inserted";
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
    }
}
