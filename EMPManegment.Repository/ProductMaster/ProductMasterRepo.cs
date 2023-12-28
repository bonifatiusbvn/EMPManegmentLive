using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.Inretface.Interface.ProductMaster;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Task<UserResponceModel> AddProductType(ProductTypeView AddProduct)
        {
            throw new NotImplementedException();
        }
    }
}
