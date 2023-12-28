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

        public Task<UserResponceModel> AddProductType(ProductTypeView AddProduct)
        {
            throw new NotImplementedException();
        }
    }
}
