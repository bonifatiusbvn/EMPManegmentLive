using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.POMaster
{
    public class OPMasterView
    {
        public Guid Id { get; set; }

        public Guid? VendorId { get; set; }

        public string? Opid { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public decimal? TotalAmount { get; set; }

        public string? Status { get; set; }
    }
}
