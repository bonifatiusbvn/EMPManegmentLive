using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.Invoice
{
    public class CheckInvoiceView
    {
        public Guid? ProjectId { get; set; }

        public string? OrderId { get; set; }

        public string? ProjectName { get; set; }
    }
}
