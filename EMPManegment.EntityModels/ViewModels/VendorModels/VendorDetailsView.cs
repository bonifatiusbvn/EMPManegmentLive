using EMPManegment.EntityModels.View_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.VendorModels
{
    public class VendorDetailsView
    {
        public int Id { get; set; }

        public string VendorName { get; set; } = null!;

        public string? VendorEmail { get; set; }

        public string? VendorPhone { get; set; }

        public string? VendorAddress { get; set; }

        public string? VendorBankAccountNo { get; set; }

        public string? VendorBankName { get; set; }

        public string? VendorBankIfsc { get; set; }

        public string? VendorGstnumber { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? CreatedBy { get; set; }
    }
}
