using EMPManegment.Inretface.Services.ProductMaster;
using EMPManegment.Services.ProductMaster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductMasterController : ControllerBase
    {
        private readonly IProductMasterServices productMaster;
        public ProductMasterController(IProductMasterServices ProductMaster)
        {
            productMaster = ProductMaster;
        }
    }
}
