using EMPManagment.API;
using EMPManegment.Inretface.Interface.PurchaseRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Repository.PurchaseRequestRepository
{
    public class PurchaseRequestRepo:IPurchaseRequest
    {
        public PurchaseRequestRepo(BonifatiusEmployeesContext context)
        {
            Context = context;
        }
        public BonifatiusEmployeesContext Context { get; }
    }
}
