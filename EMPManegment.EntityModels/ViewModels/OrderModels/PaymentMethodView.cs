using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.OrderModels
{
    public class PaymentMethodView
    {
        public int Id { get; set; }
        public string PaymentMethod { get; set; } = null!;
    }
}
