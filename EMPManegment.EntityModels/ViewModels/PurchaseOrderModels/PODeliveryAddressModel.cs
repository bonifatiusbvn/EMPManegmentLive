using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.PurchaseOrderModels
{
    public class PODeliveryAddressModel
    {
        public int Aid { get; set; }

        public Guid Poid { get; set; }

        public string Address { get; set; } = null!;

        public bool? IsDeleted { get; set; }
    }
}
