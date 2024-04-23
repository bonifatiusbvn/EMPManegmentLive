using EMPManegment.Inretface.Interface.OrderDetails;
using EMPManegment.Inretface.Interface.PurchaseRequest;
using EMPManegment.Inretface.Services.PurchaseRequestServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.PurchaseRequest
{
    public class PurchaseRequestServices:IPurchaseRequestServices
    {
        public PurchaseRequestServices(IPurchaseRequest PurchaseRequest)
        {
            purchaseRequest = PurchaseRequest;
        }
        public IPurchaseRequest purchaseRequest { get; }
    }
}
