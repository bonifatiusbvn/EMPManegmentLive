using EMPManegment.EntityModels.View_Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.VendorModels
{
    public class VendorDetailsView
    {
        public int Id { get; set; }
        [Required]
        public string VendorName { get; set; } = null!;
        [Required]
        public string? VendorEmail { get; set; }
        [Required]
        public string? VendorPhone { get; set; }
        [Required]
        public string? VendorAddress { get; set; }
        [Required]
        public string? VendorBankAccountNo { get; set; }
        [Required]
        public string? VendorBankName { get; set; }
        [Required]
        public string? VendorBankIfsc { get; set; }
        [Required]
        public string? VendorGstnumber { get; set; }
        [Required]
        public DateTime? CreatedOn { get; set; }
        [Required]
        public string? CreatedBy { get; set; }
    }
}
