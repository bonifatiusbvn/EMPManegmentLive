using EMPManegment.Inretface.Services.OrderDetails;
using EMPManegment.Inretface.Services.PurchaseRequestServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseRequestController : ControllerBase
    {
        public PurchaseRequestController(IPurchaseRequestServices PurchaseRequest)
        {
            purchaseRequest = PurchaseRequest;
        }
        public IPurchaseRequestServices purchaseRequest { get; }
    }
}
